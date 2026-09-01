<template>
  <div>
    <CountryList
      :countries="this.CountryStore.countries"
      @show-update-country="OnUpsertCountry"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertCountryComp
        :errors="this.CountryStore.error"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <button @click="showUpsert = true">Add Country</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import CountryList from "@/components/Address/Country/CountryList.vue";
import UpsertCountryComp from "@/components/Address/Country/UpsertCountryComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import { useCountryStore } from "@/store/Address/useCountryStore";

export default {
  name: "CountryView",
  components: {
    CountryList,
    UpsertCountryComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      CountryStore: useCountryStore(),
      showUpsert: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    OnUpsertCountry(e) {
      if (e) this.CountryStore.selectedCountry = e;
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      this.resultType = false;
      var resVal;
      if (e.ID) {
        resVal = await this.CountryStore.updateCountry(e);
      } else {
        resVal = await this.CountryStore.addCountry(e);
        this.CountryStore.selectedCountry = null;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal;
        this.resultType = true;
        alert("error");
      }
      console.log(resVal);
    },

    OnDeleteCountry(id) {
      this.CountryStore.deleteCountry(id);
    },
    onpopaClose() {
      this.CountryStore.selectedCountry = null;
      this.showUpsert = false;
    },
  },
  mounted() {
    this.CountryStore.fetchCountries();
  },
};
</script>

<style scoped></style>
