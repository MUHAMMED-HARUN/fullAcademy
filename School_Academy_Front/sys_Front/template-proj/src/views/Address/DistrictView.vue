<template>
  <div>
    <DistrictList
      :districts="this.DistrictStore.districts"
      @show-update-district="OnUpsertDistrict"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertDistrictComp
        :errors="DistrictStore.error"
        :cities="CityStore.cities"
        :countries="CountryStore.countries"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <button @click="showUpsert = true">اضافة مدينة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import DistrictList from "@/components/Address/District/DistrictList.vue";
import UpsertDistrictComp from "@/components/Address/District/UpsertDistrictComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
import { useCityStore } from "@/store/Address/useCityStore";
import { useCountryStore } from "@/store/Address/useCountryStore";
import MessageComp from "@/components/general/messageComp.vue";

export default {
  name: "DistrictView",
  components: {
    DistrictList,
    UpsertDistrictComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      DistrictStore: useDistrictStore(),
      CountryStore: useCountryStore(),
      CityStore: useCityStore(),
      showUpsert: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    OnUpsertDistrict(e) {
      if (e) this.DistrictStore.selecteddistricts = e;
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      this.resultType = false;
      if (e.id) {
        var resVal = await this.DistrictStore.updateDistrict(e);
      } else {
        resVal = await this.DistrictStore.addDistrict(e);
        this.DistrictStore.selecteddistricts = null;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = true;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal;
        this.resultType = true;
      }
    },
    OnDeleteDistrict(id) {
      this.DistrictStore.delete(id);
    },
    onpopaClose() {
      this.DistrictStore.selecteddistricts = null;
      this.showUpsert = false;
    },
  },
  mounted() {
    this.DistrictStore.fetchDistricts();
    this.CountryStore.fetchCountries();
    this.CityStore.fetchCities();
  },
};
</script>

<style scoped></style>
