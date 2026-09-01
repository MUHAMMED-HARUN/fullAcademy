<template>
  <div>
    <InstituteList
      @show-delete-institutes="OnDeleteInstitute"
      @show-info-institutes="onShowInfoInstitute"
      @show-update-institutes="OnUpsertInstitute"
      :institutes="this.InstituteStore.institutes"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertInstituteComp
        :countries="this.CountryStore.countries"
        :cities="this.CitiesList"
        :districts="this.DistrictsList"
        :neighborhoods="this.NeighborhoodsList"
        :errors="this.InstituteStore.error"
        :address="this.AddressStore.selectedaddress"
        @on-submit="OnSubmitUpsert"
        @country-change="onCountryChange"
        @city-change="onCityChange"
        @district-change="onDistrictChange"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <InstituteCard :-data-req="this.InstituteStore.selectedInstitute" />
    </PopapComp>
    <button @click="showUpsert = true">اضافة مدينة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useInstituteStore } from "@/store/Institute/useInstituteStore";
import { useAddressStore } from "@/store/Address/useAddreessStore";
import { useCountryStore } from "@/store/Address/useCountryStore";
import { useCityStore } from "@/store/Address/useCityStore";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
import { useNeighborhoodStore } from "@/store/Address/useNeighborhoodStore";
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
  },
  data() {
    return {
      InstituteStore: useInstituteStore(),
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
    };
  },
  methods: {
    async OnUpsertInstitute(e) {
      if (e) {
        this.InstituteStore.selectedInstitute = e;
        var param = {
          param: e.instituteID,
          ParamType: 3,
        };
        await this.AddressStore.fetchAddressBy(param);
        this.onCountryChange(this.AddressStore.selectedaddress.countryID);
        this.onCityChange(this.AddressStore.selectedaddress.cityID);
        this.onDistrictChange(this.AddressStore.selectedaddress.districtID);
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
    this.InstituteStore.fetchInstitutes();
  },
};
</script>

<style scoped></style>
