<template>
  <div class="space-y-4">
    <GenericComboActions
      :actions="visibleActions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <InstituteList
      :institutes="InstituteStore.institutes"
      :selected-ids="selectedIds"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-delete-institutes="OnDeleteInstitute"
      @show-info-institutes="onShowInfoInstitute"
      @show-update-institutes="OnUpsertInstitute"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertInstituteComp
        :countries="CountryStore.countries"
        :countries-loading="CountryStore.loading"
        :countries-error="CountryStore.error"
        :city-results="CityStore.cityLookup('instituteAddress').results"
        :city-search-loading="CityStore.cityLookup('instituteAddress').loading"
        :city-search-error="CityStore.cityLookup('instituteAddress').error"
        :district-results="DistrictStore.districtSearchResults"
        :district-search-loading="DistrictStore.districtSearchLoading"
        :district-search-error="DistrictStore.districtSearchError"
        :neighborhood-results="NeighborhoodStore.neighborhoodSearchResults"
        :neighborhood-search-loading="
          NeighborhoodStore.neighborhoodSearchLoading
        "
        :neighborhood-search-error="NeighborhoodStore.neighborhoodSearchError"
        :errors="InstituteStore.error"
        :address="AddressStore.selectedaddress"
        :loading="InstituteStore.loading"
        @on-submit="OnSubmitUpsert"
        @search-city="onSearchAddressCity"
        @clear-city-search="CityStore.clearCitySearchFor('instituteAddress')"
        @search-district="onSearchAddressDistrict"
        @clear-district-search="DistrictStore.clearDistrictSearch"
        @search-neighborhood="onSearchAddressNeighborhood"
        @clear-neighborhood-search="NeighborhoodStore.clearNeighborhoodSearch"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <InstituteCard :-data-req="InstituteStore.selectedInstitute" />
    </PopapComp>

    <button
      v-if="AuthStore.hasPermission('Can_Create_Institute')"
      type="button"
      class="h-10 rounded-[10px] bg-brand-500 px-4 text-sm font-medium text-white shadow-theme-xs transition-colors hover:bg-brand-600"
      @click="OnUpsertInstitute(null)"
    >
      إضافة معهد
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import { useInstituteStore } from "@/store/Institute/useInstituteStore";
import { useAddressStore } from "@/store/Address/useAddreessStore";
import { useCountryStore } from "@/store/Address/useCountryStore";
import { useCityStore } from "@/store/Address/useCityStore";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
import { useNeighborhoodStore } from "@/store/Address/useNeighborhoodStore";
import { useAuthStore } from "@/store/Auth/authStore";
import MessageComp from "@/components/general/messageComp.vue";
import InstituteList from "@/components/Institute/Institute/InstituteList.vue";
import UpsertInstituteComp from "@/components/Institute/Institute/UpsertInstituteComp.vue";
import InstituteCard from "@/components/Institute/Institute/InstituteCard.vue";
import TResult from "@/Generic/TResult";

export default {
  name: "InstituteView",
  components: {
    InstituteList,
    UpsertInstituteComp,
    InstituteCard,
    PopapComp,
    MessageComp,
    GenericComboActions,
  },
  data() {
    return {
      InstituteStore: useInstituteStore(),
      AddressStore: useAddressStore(),
      CountryStore: useCountryStore(),
      CityStore: useCityStore(),
      DistrictStore: useDistrictStore(),
      NeighborhoodStore: useNeighborhoodStore(),
      AuthStore: useAuthStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class: "bg-red-500 hover:bg-red-600",
        },
      ],
    };
  },
  computed: {
    visibleActions() {
      const permissions = {
        info: "Can_Read_Institute",
        update: "Can_Update_Institute",
        delete: "Can_Delete_Institute",
      };
      return this.actions.filter((action) =>
        this.AuthStore.hasPermission(permissions[action.key])
      );
    },
    selectedItems() {
      return this.InstituteStore.institutes.filter((item) =>
        this.selectedIds.includes(item.instituteID)
      );
    },
  },
  methods: {
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.InstituteStore.institutes.map((item) => item.instituteID)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.instituteID);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.instituteID);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          this.onShowInfoInstitute(singleItem);
          break;
        case "update":
          await this.OnUpsertInstitute(singleItem);
          break;
        case "delete":
          await this.OnDeleteInstitute(singleItem);
          this.selectedIds = [];
          break;
      }
    },
    async OnUpsertInstitute(e) {
      this.resultMessage = null;
      this.resultType = false;
      this.clearAddressSearches();

      if (e) {
        this.InstituteStore.selectedInstitute = e;
        var param = {
          param: e.instituteID,
          ParamType: 3,
        };
        const addressResult = await this.AddressStore.fetchAddressBy(param);
        if (!addressResult.success || !this.AddressStore.selectedaddress) {
          this.resultMessage =
            addressResult.errors || "تعذر جلب عنوان المعهد للتعديل";
          this.resultType = true;
          this.InstituteStore.selectedInstitute = null;
          return;
        }
      } else {
        this.InstituteStore.selectedInstitute = null;
        this.AddressStore.clearSelectedAddress();
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.instituteID) {
        resVal = await this.InstituteStore.updateInstitute(e);
      } else {
        resVal = await this.InstituteStore.addInstitute(e);
        this.InstituteStore.selectedInstitute = null;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.InstituteStore.selectedInstitute = null;
        this.AddressStore.clearSelectedAddress();
        this.clearAddressSearches();
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async OnDeleteInstitute(e) {
      this.InstituteStore.DeleteInstitute(e.instituteID);
    },
    async onShowInfoInstitute(e) {
      this.InstituteStore.selectedInstitute =
        this.InstituteStore.institutes.find(
          (i) => i.instituteID === e.instituteID
        );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.InstituteStore.selectedInstitute = null;
      this.AddressStore.clearSelectedAddress();
      this.clearAddressSearches();
    },
    async onSearchAddressCity(criteria) {
      await this.CityStore.searchCitiesFor(criteria, "instituteAddress");
    },
    async onSearchAddressDistrict(criteria) {
      await this.DistrictStore.searchDistricts(criteria);
    },
    async onSearchAddressNeighborhood(criteria) {
      await this.NeighborhoodStore.searchNeighborhoods(criteria);
    },
    clearAddressSearches() {
      this.CityStore.clearCitySearchFor("instituteAddress");
      this.DistrictStore.clearDistrictSearch();
      this.NeighborhoodStore.clearNeighborhoodSearch();
    },
  },
  mounted() {
    this.CountryStore.fetchCountries();
    this.InstituteStore.fetchInstitutes();
  },
};
</script>

<style scoped></style>
