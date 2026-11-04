<template>
  <div class="space-y-4">
    <GenericFilterComp
      :fields="countryFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <GenericComboActions
      :actions="actions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <CountryList
      :countries="CountryStore.countryPageItems"
      :selected-ids="selectedIds"
      :loading="CountryStore.pageLoading"
      :error="CountryStore.pageError"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-update-country="OnUpsertCountry"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertCountryComp
        :errors="CountryStore.error"
        :loading="CountryStore.saving"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>

    <PaginationComp
      :current-page="CountryStore.page"
      :page-size="CountryStore.pageSize"
      :total-count="CountryStore.totalCount"
      :total-pages="CountryStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />
    <PopapComp v-if="showUpsertCity" @close="onpopaCloseCity">
      <UpsertCityComp
        :errors="CityStore.error"
        :loading="CityStore.saving"
        :countries="CountryStore.countries"
        :countries-loading="CountryStore.loading"
        :country-data="CountryStore.selectedCountry"
        :city-data="CityStore.selectedCity"
        @on-submit="OnSubmitCity"
      />
    </PopapComp>

    <button
      type="button"
      class="btn btn-primary"
      @click="OnUpsertCountry(null)"
    >
      إضافة دولة
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import CountryList from "@/components/Address/Country/CountryList.vue";
import UpsertCountryComp from "@/components/Address/Country/UpsertCountryComp.vue";
import UpsertCityComp from "@/components/Address/City/UpsertCityComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import { useCountryStore } from "@/store/Address/useCountryStore";
import { useCityStore } from "@/store/Address/useCityStore";

export default {
  name: "CountryView",
  components: {
    CountryList,
    UpsertCountryComp,
    UpsertCityComp,
    PopapComp,
    MessageComp,
    GenericComboActions,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      CountryStore: useCountryStore(),
      CityStore: useCityStore(),
      showUpsert: false,
      showUpsertCity: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      countryFilterSchema: [
        { key: "Name", label: "الدولة", type: "text", operator: "=*" },
        { key: "Code", label: "رمز الدولة", type: "text", operator: "=*" },
      ],
      actions: [
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "addChildCity",
          label: "إضافة مدينة",
          minSelection: 1,
          maxSelection: 1,
          class: "bg-brand-500 hover:bg-brand-600",
        },
      ],
    };
  },
  computed: {
    selectedItems() {
      return this.CountryStore.countryPageItems.filter((country) =>
        this.selectedIds.includes(country.id)
      );
    },
  },
  methods: {
    OnUpsertCountry(e) {
      if (e) this.CountryStore.selectedCountry = e;
      else this.CountryStore.selectedCountry = null;
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      this.resultType = false;
      var resVal;
      if (e.id) {
        resVal = await this.CountryStore.updateCountry(e);
      } else {
        resVal = await this.CountryStore.addCountry(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.CountryStore.selectedCountry = null;
      } else {
        this.resultMessage = resVal.errors || "تعذر حفظ الدولة";
        this.resultType = true;
      }
    },
    OnDeleteCountry(id) {
      this.CountryStore.deleteCountry(id);
      this.selectedIds = this.selectedIds.filter((x) => x !== id);
    },
    async OnSubmitCity(e) {
      var resVal = false;
      if (e.id || e.cityID) {
        resVal = await this.CityStore.updateCity(e);
      } else {
        resVal = await this.CityStore.addCity(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsertCity = false;
      } else {
        this.resultMessage = resVal.errors || resVal;
        this.resultType = true;
      }
    },
    onpopaCloseCity() {
      this.CityStore.selectedCity = null;
      this.showUpsertCity = false;
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "update":
          await this.OnUpsertCountry(singleItem);
          break;
        case "delete":
          await this.OnDeleteCountry(singleItem.ID);
          this.selectedIds = [];
          break;
        case "addChildCity":
          this.CountryStore.selectedCountry = singleItem;
          this.CityStore.selectedCity = null;
          this.showUpsertCity = true;
          break;
      }
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.CountryStore.countryPageItems.map((country) => country.id)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.id);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.id);
    },
    async onFilterSearch(payload) {
      this.selectedIds = [];
      await this.CountryStore.fetchCountryPage({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },
    async onFilterReset() {
      this.selectedIds = [];
      await this.CountryStore.fetchCountryPage({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },
    async onPageChange(page) {
      this.selectedIds = [];
      await this.CountryStore.setPage(page);
    },
    async onPageSizeChange(pageSize) {
      this.selectedIds = [];
      await this.CountryStore.setPageSize(pageSize);
    },
    onpopaClose() {
      this.CountryStore.selectedCountry = null;
      this.showUpsert = false;
    },
  },
  mounted() {
    this.CountryStore.fetchCountries();
    this.CountryStore.fetchCountryPage();
  },
};
</script>

<style scoped></style>
