<template>
  <div>
    <NeighborhoodList
      :neighborhoods="this.NeighborhoodStore.neighborhoods"
      @show-update-neighborhood="OnUpsertNeighborhood"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertNeighborhoodComp
        :errors="NeighborhoodStore.error"
        :districts="DistrictStore.districts"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <button @click="showUpsert = true">اضافة شارع</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import NeighborhoodList from "@/components/Address/Neighborhood/NeighborhoodList.vue";
import UpsertNeighborhoodComp from "@/components/Address/Neighborhood/UpsertNeighborhoodComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import { useNeighborhoodStore } from "@/store/Address/useNeighborhoodStore";
import { useDistrictStore } from "@/store/Address/useDistrictStore";
import MessageComp from "@/components/general/messageComp.vue";

export default {
  name: "DistrictView",
  components: {
    NeighborhoodList,
    UpsertNeighborhoodComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      DistrictStore: useDistrictStore(),
      NeighborhoodStore: useNeighborhoodStore(),
      showUpsert: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    OnUpsertNeighborhood(e) {
      if (e) this.NeighborhoodStore.selectedNeighborhood = e;
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      this.resultType = false;
      if (e.id) {
        var resVal = await this.NeighborhoodStore.updateNeighborhood(e);
      } else {
        resVal = await this.NeighborhoodStore.addNeighborhood(e);
        this.NeighborhoodStore.selectedNeighborhood = null;
      }
      console.log(resVal);
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal.success;
        this.resultType = true;
      }
    },
    OnDeleteNeighborhood(id) {
      this.NeighborhoodStore.delete(id);
    },
    onpopaClose() {
      this.NeighborhoodStore.selectedNeighborhood = null;
      this.showUpsert = false;
    },
  },
  mounted() {
    this.DistrictStore.fetchDistricts();
    this.NeighborhoodStore.fetchNeighborhoods();
  },
};
</script>

<style scoped></style>
