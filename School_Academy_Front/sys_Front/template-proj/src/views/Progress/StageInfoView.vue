<template>
  <div>
    <StageInfoList
      @show-delete-stage-info="OnDeleteStageInfo"
      @show-info-stage-info="onShowInfoStageInfo"
      @show-update-stage-info="OnUpsertStageInfo"
      :-stage-infos="StageInfoStore.StageInfos"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertStageInfoComp
        :errors="StageInfoStore.error"
        :stage-data="StageStore.selectedStage"
        :matter-data="SessionStore.selectedSession"
        @on-search-matter="onSearchMatter"
        @on-search-stage="onSearchStage"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <StageInfoCard :-data-req="StageInfoStore.selectedStageInfo" />
    </PopapComp>
    <button @click="showUpsert = true">اضافة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import { useStageStore } from "@/store/Progress/StageStore";
import { useSessionStore } from "@/store/Progress/SessionStore";
import MessageComp from "@/components/general/messageComp.vue";
import TResult from "@/Generic/TResult";
import StageInfoList from "@/components/Progress/StageInfo/StageInfoList.vue";
import UpsertStageInfoComp from "@/components/Progress/StageInfo/UpsertStageInfoComp.vue";
import StageInfoCard from "@/components/Progress/StageInfo/StageInfoCard.vue";

export default {
  name: "StageInfoView",
  components: {
    StageInfoList,
    StageInfoCard,
    UpsertStageInfoComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      StageInfoStore: useStageInfoStore(),
      SessionStore: useSessionStore(),
      StageStore: useStageStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    async OnUpsertStageInfo(e) {
      if (e) {
        this.StageInfoStore.selectedStageInfo = e;
        this.SessionStore.fetchSessiontBy({
          Param: e.matterID,
          ParamType: 0,
        });
        this.StageStore.fetchStagetBy({
          Param: e.stageID,
          ParamType: 0,
        });
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      console.log("e", e);
      if (e.stageInfoID) {
        resVal = await this.StageInfoStore.updateStageInfo(e);
      } else {
        resVal = await this.StageInfoStore.addStageInfo(e);
        this.StageInfoStore.selectedStageInfo = null;
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
    async OnDeleteStageInfo(e) {
      this.StageInfoStore.DeleteStageInfo(e.stageInfoID);
    },
    async onShowInfoStageInfo(e) {
      this.StageInfoStore.selectedStageInfo =
        this.StageInfoStore.StageInfos.find(
          (i) => i.stageInfoID === e.stageInfoID
        );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.StageInfoStore.selectedStageInfo = null;
    },

    async onSearchStage(type, val) {
      await this.StageStore.fetchStagetBy({
        Param: val,
        ParamType: type,
      });
    },
    async onSearchMatter(type, val) {
      await this.SessionStore.fetchSessiontBy({
        Param: val,
        ParamType: type,
      });
    },
  },
  mounted() {
    this.StageInfoStore.fetchStageInfos();
  },
};
</script>

<style scoped></style>
