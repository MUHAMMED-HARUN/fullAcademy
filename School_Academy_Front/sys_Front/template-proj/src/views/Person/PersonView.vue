<template>
  <div>
    <!-- 🔹 فلتر البحث الشامل -->
    <GenericFilterComp
      :fields="personFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <PersonList
      :-persons="PersonStore.persons"
      @show-update-person="OnUpsertPerson"
      @show-delete-person="OnDeletePerson"
      @show-info-person="onShowInfoPerson"
    />

    <!-- 🔹 مكون التصفح -->
    <PaginationComp
      :current-page="PersonStore.page"
      :page-size="PersonStore.pageSize"
      :total-count="PersonStore.totalCount"
      :total-pages="PersonStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertPersonAddress
        :countries="CountryStore.countries"
        :cities="CitiesList"
        :districts="DistrictsList"
        :neighborhoods="NeighborhoodsList"
        :errors="PersonStore.error"
        :address="AddressStore.selectedaddress"
        @on-submit="OnSubmitUpsert"
        @country-change="onCountryChange"
        @city-change="onCityChange"
        @district-change="onDistrictChange"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onpopaClose">
      <PersonCard :-data-req="PersonStore.selectedPerson" />
    </PopapComp>

    <button @click="showUpsert = true">إضافة شخص</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useAddressStore } from "@/store/Address/useAddreessStore";
import { useCountryStore } from "@/store/Address/useCountryStore";
import { useCityStore } from "@/store/Address/useCityStore";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
import { useNeighborhoodStore } from "@/store/Address/useNeighborhoodStore";
import MessageComp from "@/components/general/messageComp.vue";
import PersonList from "@/components/Person/Person/PersonList.vue";
import UpsertPersonAddress from "@/components/Person/Person/upsertPersonAddress.vue";
import PersonCard from "@/components/Person/Person/PersonCard.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";

export default {
  name: "PersonView",
  components: {
    PersonList,
    UpsertPersonAddress,
    PopapComp,
    MessageComp,
    PersonCard,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      PersonStore: usePersonStore(),
      AddressStore: useAddressStore(),
      CountryStore: useCountryStore(),
      CityStore: useCityStore(),
      DistrictStore: useDistrictStore(),
      NeighborhoodStore: useNeighborhoodStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      CitiesList: [],
      DistrictsList: [],
      NeighborhoodsList: [],
      personFilterSchema: [
        {
          key: "FirstName",
          label: "الاسم الأول",
          type: "text",
          operator: "=*",
        },
        {
          key: "LastName",
          label: "الكنية",
          type: "text",
          operator: "=*",
        },
        {
          key: "NationalNumber",
          label: "الرقم الوطني",
          type: "text",
          operator: "=*",
        },
        {
          key: "Gendor",
          label: "الجنس",
          type: "select",
          options: [
            { label: "ذكر", value: true },
            { label: "أنثى", value: false },
          ],
        },
      ],
    };
  },
  methods: {
    async onPageChange(newPage) {
      await this.PersonStore.setPage(newPage);
    },

    async onPageSizeChange(newSize) {
      await this.PersonStore.setPageSize(newSize);
    },

    async onFilterSearch(payload) {
      await this.PersonStore.fetchPersons({
        Filter: payload.filter,
        OrderBy: payload.orderBy,
      });
    },

    async onFilterReset() {
      await this.PersonStore.fetchPersons();
    },

    async OnUpsertPerson(e) {
      if (e) {
        this.PersonStore.selectedPerson = e;
        var param = {
          param: e.id,
          ParamType: 2,
        };
        await this.AddressStore.fetchAddressBy(param);
        this.onCountryChange(this.AddressStore.selectedaddress?.countryID);
        this.onCityChange(this.AddressStore.selectedaddress?.cityID);
        this.onDistrictChange(this.AddressStore.selectedaddress?.districtID);
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = false;
      if (e.id) {
        resVal = await this.PersonStore.updatePerson(e);
      } else {
        resVal = await this.PersonStore.addPerson(e);
        this.CountryStore.selectedCountry = null;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal.errors || "حدث خطأ أثناء الحفظ";
        this.resultType = true;
      }
    },

    async OnDeletePerson(e) {
      await this.PersonStore.DeletePerson(e.id);
    },

    async onShowInfoPerson(e) {
      this.PersonStore.selectedPerson = this.PersonStore.persons.find(
        (p) => p.id === e.id
      );
      this.showInfo = true;
    },

    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.PersonStore.selectedPerson = null;
    },

    onCountryChange(e) {
      this.CitiesList = this.CityStore.cities.filter(
        (city) => city.countryID == e
      );
    },

    onCityChange(e) {
      this.DistrictsList = this.DistrictStore.districts.filter(
        (d) => d.cityID == e
      );
    },

    onDistrictChange(e) {
      this.NeighborhoodsList = this.NeighborhoodStore.neighborhoods.filter(
        (n) => n.districtID == e
      );
    },
  },

  mounted() {
    this.CountryStore.fetchCountries();
    this.CityStore.fetchCities();
    this.DistrictStore.fetchDistricts();
    this.NeighborhoodStore.fetchNeighborhoods();
    this.PersonStore.fetchPersons();
  },
};
</script>
