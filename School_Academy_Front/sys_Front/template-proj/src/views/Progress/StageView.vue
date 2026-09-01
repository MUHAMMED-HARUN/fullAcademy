<template>
  <div>
    <StageList
      @show-delete-Stage="OnDeleteStage"
      @show-info-Stage="onShowInfoStage"
      @show-update-Stage="OnUpsertStage"
      :-Stages="StageStore.stages"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertStageComp
        :errors="StageStore.error"
        :-stages="StageStore.stages"
        :program-data="ProgramStore.selectedProgram"
        @on-search-program="onSearchProgram"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <StageCard :-data-req="StageStore.selectedStage" />
    </PopapComp>
    <button @click="showUpsert = true">اضافة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useStageStore } from "@/store/Progress/StageStore";
import MessageComp from "@/components/general/messageComp.vue";
import TResult from "@/Generic/TResult";
import StageList from "@/components/Progress/Stage/StageList.vue";
import UpsertStageComp from "@/components/Progress/Stage/UpsertStageComp.vue";
import StageCard from "@/components/Progress/Stage/StageCard.vue";
import { useProgramStore } from "@/store/Institute/useProgramStore";

export default {
  name: "StageView",
  components: {
    StageList,
    StageCard,
    UpsertStageComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      StageStore: useStageStore(),
      ProgramStore: useProgramStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    async OnUpsertStage(e) {
      if (e) {
        this.StageStore.selectedStage = e;
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.stageID) {
        resVal = await this.StageStore.updateStage(e);
      } else {
        resVal = await this.StageStore.addStage(e);
        this.StageStore.selectedStage = null;
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
    async OnDeleteStage(e) {
      this.StageStore.DeleteStage(e.stageID);
    },
    async onShowInfoStage(e) {
      this.StageStore.selectedStage = this.StageStore.stages.find(
        (i) => i.stageID === e.stageID
      );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.StageStore.selectedStage = null;
    },

    async onSearchProgram(type, val) {
      await this.ProgramStore.fetchProgrameBy({
        Param: val,
        ParamType: type,
      });
    },
  },
  mounted() {
    this.StageStore.fetchStages();
  },
};
</script>

<style scoped></style>
