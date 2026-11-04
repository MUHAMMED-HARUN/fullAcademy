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

    <DistrictList
      :districts="DistrictStore.districtPageItems"
      :selected-ids="selectedIds"
      :loading="DistrictStore.pageLoading"
      :error="DistrictStore.pageError"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-info-district="onShowInfo"
      @show-update-district="OnUpsertDistrict"
    />

    <PaginationComp
      :current-page="DistrictStore.page"
      :page-size="DistrictStore.pageSize"
      :total-count="DistrictStore.totalCount"
      :total-pages="DistrictStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertDistrictComp
        :errors="DistrictStore.error"
        :loading="DistrictStore.saving"
        :countries="CountryStore.countries"
        :countries-loading="CountryStore.loading"
        :countries-error="CountryStore.error"
        :city-results="CityStore.citySearchResults"
        :city-search-loading="CityStore.citySearchLoading"
        :city-search-error="CityStore.citySearchError"
        @on-submit="OnSubmitUpsert"
        @search-city="onSearchCity"
        @clear-city-search="CityStore.clearCitySearch"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaCloseInfo">
      <DistrictCard :district="DistrictStore.selecteddistricts" />
    </PopapComp>
    <PopapComp v-if="showUpsertNeighborhood" @close="onpopaCloseNeighborhood">
      <UpsertNeighborhoodComp
        :errors="NeighborhoodStore.error"
        :loading="NeighborhoodStore.saving"
        :countries="CountryStore.countries"
        :countries-loading="CountryStore.loading"
        :neighborhood-data="NeighborhoodStore.selectedNeighborhood"
        :district-data="DistrictStore.selecteddistricts"
        :city-results="CityStore.citySearchResults"
        :city-search-loading="CityStore.citySearchLoading"
        :city-search-error="CityStore.citySearchError"
        :district-results="DistrictStore.districtSearchResults"
        :district-search-loading="DistrictStore.districtSearchLoading"
        :district-search-error="DistrictStore.districtSearchError"
        @on-submit="OnSubmitNeighborhood"
        @search-city="onSearchCity"
        @clear-city-search="CityStore.clearCitySearch"
        @search-district="onSearchDistrict"
        @clear-district-search="DistrictStore.clearDistrictSearch"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />

    <button
      type="button"
      class="btn btn-primary"
      @click="OnUpsertDistrict(null)"
    >
      إضافة حارة
    </button>
  </div>
</template>

<script>
import DistrictList from "@/components/Address/District/DistrictList.vue";
import DistrictCard from "@/components/Address/District/DistrictCard.vue";
import UpsertDistrictComp from "@/components/Address/District/UpsertDistrictComp.vue";
import UpsertNeighborhoodComp from "@/components/Address/Neighborhood/UpsertNeighborhoodComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
import { useCityStore } from "@/store/Address/useCityStore";
import { useNeighborhoodStore } from "@/store/Address/useNeighborhoodStore";
import { useCountryStore } from "@/store/Address/useCountryStore";

export default {
  name: "DistrictView",
  components: {
    DistrictList,
    DistrictCard,
    UpsertDistrictComp,
    UpsertNeighborhoodComp,
    PopapComp,
    MessageComp,
    GenericComboActions,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      DistrictStore: useDistrictStore(),
      CityStore: useCityStore(),
      CountryStore: useCountryStore(),
      NeighborhoodStore: useNeighborhoodStore(),
      showUpsert: false,
      showUpsertNeighborhood: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      filterSchema: [
        {
          key: "DistrictName",
          label: "المنطقة",
          type: "text",
          operator: "=*",
        },
        { key: "CityName", label: "المدينة", type: "text", operator: "=*" },
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
          key: "addChildNeighborhood",
          label: "إضافة شارع",
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
      return this.DistrictStore.districtPageItems.filter((d) =>
        this.selectedIds.includes(d.id)
      );
    },
  },
  methods: {
    OnUpsertDistrict(e) {
      if (e) this.DistrictStore.selecteddistricts = e;
      else this.DistrictStore.selecteddistricts = null;
      this.CityStore.clearCitySearch();
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
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
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal.errors || resVal;
        this.resultType = true;
      }
    },
    onShowInfo(e) {
      this.DistrictStore.selecteddistricts = e;
      this.showInfo = true;
    },
    onpopaCloseInfo() {
      this.showInfo = false;
      this.DistrictStore.selecteddistricts = null;
    },
    async onSearchCity(criteria) {
      if (!criteria.countryID) {
        this.CityStore.clearCitySearch();
        return;
      }
      await this.CityStore.searchCities(criteria);
    },
    async onSearchDistrict(criteria) {
      if (!criteria.cityID) {
        this.DistrictStore.clearDistrictSearch();
        return;
      }
      await this.DistrictStore.searchDistricts(criteria);
    },
    async OnSubmitNeighborhood(e) {
      this.resultType = false;
      var resVal;
      if (e.id) {
        resVal = await this.NeighborhoodStore.updateNeighborhood(e);
      } else {
        resVal = await this.NeighborhoodStore.addNeighborhood(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsertNeighborhood = false;
      } else {
        this.resultMessage = resVal.errors || resVal;
        this.resultType = true;
      }
    },
    onpopaCloseNeighborhood() {
      this.NeighborhoodStore.selectedNeighborhood = null;
      this.CityStore.clearCitySearch();
      this.DistrictStore.clearDistrictSearch();
      this.showUpsertNeighborhood = false;
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          this.onShowInfo(singleItem);
          break;
        case "update":
          await this.OnUpsertDistrict(singleItem);
          break;
        case "addChildNeighborhood":
          this.DistrictStore.selecteddistricts = singleItem;
          this.NeighborhoodStore.selectedNeighborhood = null;
          this.CityStore.clearCitySearch();
          this.DistrictStore.clearDistrictSearch();
          this.showUpsertNeighborhood = true;
          break;
      }
    },
    onpopaClose() {
      this.DistrictStore.selecteddistricts = null;
      this.showUpsert = false;
    },
    async onFilterSearch(payload) {
      this.selectedIds = [];
      await this.DistrictStore.fetchDistrictPage({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },
    async onFilterReset() {
      this.selectedIds = [];
      await this.DistrictStore.fetchDistrictPage({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },
    async onPageChange(newPage) {
      this.selectedIds = [];
      await this.DistrictStore.setPage(newPage);
    },
    async onPageSizeChange(newSize) {
      this.selectedIds = [];
      await this.DistrictStore.setPageSize(newSize);
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.DistrictStore.districtPageItems.map((d) => d.id)
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
      this.DistrictStore.fetchDistricts(),
      this.DistrictStore.fetchDistrictPage(),
      this.CountryStore.fetchCountries(),
    ]);
  },
};
</script>

<style scoped></style>
