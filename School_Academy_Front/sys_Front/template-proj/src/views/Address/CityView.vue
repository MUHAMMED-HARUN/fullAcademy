<template>
  <div>
    <CityList
      :cities="this.CityStore.cities"
      @show-update-city="OnUpsertCity"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertCityComp
        :countries="CountryStore.countries"
        :errors="CityStore.error"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <button @click="showUpsert = true">اضافة مدينة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import CityList from "@/components/Address/City/CityList.vue";
import UpsertCityComp from "@/components/Address/City/UpsertCityComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import { useCountryStore } from "@/store/Address/useCountryStore";
import { useCityStore } from "@/store/Address/useCityStore";
import MessageComp from "@/components/general/messageComp.vue";

export default {
  name: "CityView",
  components: {
    CityList,
    UpsertCityComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      CountryStore: useCountryStore(),
      CityStore: useCityStore(),
      showUpsert: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    OnUpsertCity(e) {
      if (e) this.CityStore.selectedCity = e;
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      var resVal = false;
      if (e.id) {
        resVal = await this.CityStore.updateCity(e);
      } else {
        resVal = await this.CityStore.addCity(e);
        this.CountryStore.selectedCountry = null;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal;
        this.resultType = true;
      }
    },
    OnDeleteCountry(id) {
      this.CityStore.delteCity(id);
    },
    onpopaClose() {
      this.CityStore.selectedCity = null;
      this.showUpsert = false;
    },
  },
  mounted() {
    this.CountryStore.fetchCountries();
    this.CityStore.fetchCities();
  },
};
</script>

<style scoped></style>
