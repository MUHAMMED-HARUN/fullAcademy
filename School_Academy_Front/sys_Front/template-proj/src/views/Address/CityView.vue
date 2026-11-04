<template>
  <div class="space-y-4">
    <GenericFilterComp
      :fields="filterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <GenericComboActions
      :actions="actions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <CityList
      :cities="CityStore.cityPageItems"
      :selected-ids="selectedIds"
      :loading="CityStore.pageLoading"
      :error="CityStore.pageError"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-info-city="onShowInfo"
      @show-update-city="OnUpsertCity"
    />

    <PaginationComp
      :current-page="CityStore.page"
      :page-size="CityStore.pageSize"
      :total-count="CityStore.totalCount"
      :total-pages="CityStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertCityComp
        :errors="CityStore.error"
        :loading="CityStore.saving"
        :countries="CountryStore.countries"
        :countries-loading="CountryStore.loading"
        :country-data="CountryStore.selectedCountry"
        :city-data="CityStore.selectedCity"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaCloseInfo">
      <CityCard :city="CityStore.selectedCity" />
    </PopapComp>
    <PopapComp v-if="showUpsertDistrict" @close="onpopaCloseDistrict">
      <UpsertDistrictComp
        :errors="DistrictStore.error"
        :loading="DistrictStore.saving"
        :countries="CountryStore.countries"
        :countries-loading="CountryStore.loading"
        :city-data="CityStore.selectedCity"
        :city-results="CityStore.citySearchResults"
        :city-search-loading="CityStore.citySearchLoading"
        :city-search-error="CityStore.citySearchError"
        @on-submit="OnSubmitDistrict"
        @search-city="onSearchDistrictCity"
        @clear-city-search="CityStore.clearCitySearch"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />

    <button type="button" class="btn btn-primary" @click="OnUpsertCity(null)">
      إضافة مدينة
    </button>
  </div>
</template>

<script>
import CityList from "@/components/Address/City/CityList.vue";
import CityCard from "@/components/Address/City/CityCard.vue";
import UpsertCityComp from "@/components/Address/City/UpsertCityComp.vue";
import UpsertDistrictComp from "@/components/Address/District/UpsertDistrictComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import { useCountryStore } from "@/store/Address/useCountryStore";
import { useCityStore } from "@/store/Address/useCityStore";
import { useDistrictStore } from "@/store/Address/useDistrictStore";

export default {
  name: "CityView",
  components: {
    CityList,
    CityCard,
    UpsertCityComp,
    UpsertDistrictComp,
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
      DistrictStore: useDistrictStore(),
      showUpsert: false,
      showUpsertDistrict: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      filterSchema: [
        { key: "Name", label: "المدينة", type: "text", operator: "=*" },
        { key: "Code", label: "الرمز", type: "text", operator: "=*" },
        {
          key: "CountryName",
          label: "الدولة",
          type: "text",
          operator: "=*",
        },
      ],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "addChildDistrict",
          label: "إضافة حارة",
          minSelection: 1,
          maxSelection: 1,
          class:
            "bg-[var(--primary-color)] hover:bg-[var(--primary-color-hover)]",
        },
      ],
    };
  },
  computed: {
    selectedItems() {
      return this.CityStore.cityPageItems.filter((city) =>
        this.selectedIds.includes(city.id)
      );
    },
  },
  methods: {
    async OnUpsertCity(e) {
      this.CountryStore.selectedCountry = null;
      if (e) {
        this.CityStore.selectedCity = e;
      } else {
        this.CityStore.selectedCity = null;
      }
      if (this.CountryStore.countries.length === 0) {
        await this.CountryStore.fetchCountries();
      }
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      const resVal = e.id
        ? await this.CityStore.updateCity(e)
        : await this.CityStore.addCity(e);
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.CityStore.selectedCity = null;
      } else {
        this.resultMessage = resVal.errors || resVal;
        this.resultType = true;
      }
    },
    onShowInfo(e) {
      this.CityStore.selectedCity = e;
      this.showInfo = true;
    },
    onpopaCloseInfo() {
      this.showInfo = false;
      this.CityStore.selectedCity = null;
    },
    async OnSubmitDistrict(e) {
      this.resultType = false;
      var resVal;
      if (e.id) {
        resVal = await this.DistrictStore.updateDistrict(e);
      } else {
        resVal = await this.DistrictStore.addDistrict(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsertDistrict = false;
      } else {
        this.resultMessage = resVal.errors || resVal;
        this.resultType = true;
      }
    },
    onpopaCloseDistrict() {
      this.DistrictStore.selecteddistricts = null;
      this.showUpsertDistrict = false;
    },
    async onSearchDistrictCity(criteria) {
      await this.CityStore.searchCities(criteria);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          this.onShowInfo(singleItem);
          break;
        case "update":
          await this.OnUpsertCity(singleItem);
          break;
        case "addChildDistrict":
          this.CityStore.selectedCity = singleItem;
          this.DistrictStore.selecteddistricts = null;
          this.showUpsertDistrict = true;
          break;
      }
    },
    onpopaClose() {
      this.CityStore.selectedCity = null;
      this.showUpsert = false;
    },
    async onFilterSearch(payload) {
      this.selectedIds = [];
      await this.CityStore.fetchCityPage({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },
    async onFilterReset() {
      this.selectedIds = [];
      await this.CityStore.fetchCityPage({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },
    async onPageChange(newPage) {
      this.selectedIds = [];
      await this.CityStore.setPage(newPage);
    },
    async onPageSizeChange(newSize) {
      this.selectedIds = [];
      await this.CityStore.setPageSize(newSize);
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.CityStore.cityPageItems.map((city) => city.id)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.id);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.id);
    },
  },
  mounted() {
    Promise.all([
      this.CountryStore.fetchCountries(),
      this.CityStore.fetchCityPage(),
    ]);
  },
};
</script>

<style scoped></style>
