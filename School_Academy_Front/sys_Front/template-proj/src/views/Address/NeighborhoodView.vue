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

    <NeighborhoodList
      :neighborhoods="NeighborhoodStore.neighborhoodPageItems"
      :selected-ids="selectedIds"
      :loading="NeighborhoodStore.pageLoading"
      :error="NeighborhoodStore.pageError"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-update-neighborhood="OnUpsertNeighborhood"
    />

    <PaginationComp
      :current-page="NeighborhoodStore.page"
      :page-size="NeighborhoodStore.pageSize"
      :total-count="NeighborhoodStore.totalCount"
      :total-pages="NeighborhoodStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertNeighborhoodComp
        :errors="NeighborhoodStore.error"
        :loading="NeighborhoodStore.saving"
        :countries="CountryStore.countries"
        :countries-loading="CountryStore.loading"
        :countries-error="CountryStore.error"
        :neighborhood-data="NeighborhoodStore.selectedNeighborhood"
        :city-results="CityStore.citySearchResults"
        :city-search-loading="CityStore.citySearchLoading"
        :city-search-error="CityStore.citySearchError"
        :district-results="DistrictStore.districtSearchResults"
        :district-search-loading="DistrictStore.districtSearchLoading"
        :district-search-error="DistrictStore.districtSearchError"
        @on-submit="OnSubmitUpsert"
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
      @click="OnUpsertNeighborhood(null)"
    >
      إضافة شارع
    </button>
  </div>
</template>

<script>
import NeighborhoodList from "@/components/Address/Neighborhood/NeighborhoodList.vue";
import UpsertNeighborhoodComp from "@/components/Address/Neighborhood/UpsertNeighborhoodComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import { useNeighborhoodStore } from "@/store/Address/useNeighborhoodStore";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
import { useCountryStore } from "@/store/Address/useCountryStore";
import { useCityStore } from "@/store/Address/useCityStore";

export default {
  name: "NeighborhoodView",
  components: {
    NeighborhoodList,
    UpsertNeighborhoodComp,
    PopapComp,
    MessageComp,
    GenericComboActions,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      NeighborhoodStore: useNeighborhoodStore(),
      DistrictStore: useDistrictStore(),
      CountryStore: useCountryStore(),
      CityStore: useCityStore(),
      showUpsert: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      filterSchema: [
        {
          key: "NeighborhoodName",
          label: "الشارع",
          type: "text",
          operator: "=*",
        },
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
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
      ],
    };
  },
  computed: {
    selectedItems() {
      return this.NeighborhoodStore.neighborhoodPageItems.filter((n) =>
        this.selectedIds.includes(n.id)
      );
    },
  },
  methods: {
    OnUpsertNeighborhood(e) {
      if (e) this.NeighborhoodStore.selectedNeighborhood = e;
      else this.NeighborhoodStore.selectedNeighborhood = null;
      this.CityStore.clearCitySearch();
      this.DistrictStore.clearDistrictSearch();
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
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
        this.showUpsert = false;
        this.NeighborhoodStore.selectedNeighborhood = null;
      } else {
        this.resultMessage = resVal.errors || resVal;
        this.resultType = true;
      }
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
    onpopaClose() {
      this.NeighborhoodStore.selectedNeighborhood = null;
      this.CityStore.clearCitySearch();
      this.DistrictStore.clearDistrictSearch();
      this.showUpsert = false;
    },
    async onFilterSearch(payload) {
      this.selectedIds = [];
      await this.NeighborhoodStore.fetchNeighborhoodPage({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },
    async onFilterReset() {
      this.selectedIds = [];
      await this.NeighborhoodStore.fetchNeighborhoodPage({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },
    async onPageChange(newPage) {
      this.selectedIds = [];
      await this.NeighborhoodStore.setPage(newPage);
    },
    async onPageSizeChange(newSize) {
      this.selectedIds = [];
      await this.NeighborhoodStore.setPageSize(newSize);
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.NeighborhoodStore.neighborhoodPageItems.map((n) => n.id)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.id);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.id);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "update":
          await this.OnUpsertNeighborhood(singleItem);
          break;
      }
    },
  },
  mounted() {
    Promise.all([
      this.NeighborhoodStore.fetchNeighborhoodPage(),
      this.CountryStore.fetchCountries(),
    ]);
  },
};
</script>

<style scoped></style>
