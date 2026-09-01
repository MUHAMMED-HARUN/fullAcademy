<template>
  <div>
    <AssignmentScopeInfoList
      @show-delete="OnDelete"
      @show-info="onShowInfo"
      @show-update="OnUpsert"
      :-assigment-scope-infos="AssignmentScopeinfoStore.AssignmentScopeinfos"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertAssignmentScopeInfoComp
        :errors="StageInfoStore.error"
        :stage-infos-data="stageInfos"
        :-assigment-scopes-data="AssignmentScopeStore.AssignmentScopes"
        @on-assigment-scope-change="onAssigmentScopeChange"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <AssignmentScopeInfoCard
        :-data-req="AssignmentScopeinfoStore.selectedAssignmentScopeinfo"
      />
    </PopapComp>
    <button @click="showUpsert = true">اضافة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import { useAssignmentScopeinfoStore } from "@/store/Assignment/AssignmentScopeInfoStore";
import MessageComp from "@/components/general/messageComp.vue";
import TResult from "@/Generic/TResult";
import UpsertAssignmentScopeInfoComp from "@/components/Assigment/AssignmentScopeInfo/UpsertAssignmentScopeInfoComp.vue";
import AssignmentScopeInfoList from "@/components/Assigment/AssignmentScopeInfo/AssignmentScopeInfoList.vue";
import AssignmentScopeInfoCard from "@/components/Assigment/AssignmentScopeInfo/AssignmentScopeInfoCard.vue";
import { useAssignmentScopeStore } from "@/store/Assignment/AssignmentScopeStore";

export default {
  name: "StageInfoView",
  components: {
    AssignmentScopeInfoList,
    AssignmentScopeInfoCard,
    UpsertAssignmentScopeInfoComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      StageInfoStore: useStageInfoStore(),
      AssignmentScopeinfoStore: useAssignmentScopeinfoStore(),
      AssignmentScopeStore: useAssignmentScopeStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      stageInfos: [],
    };
  },
  methods: {
    async OnUpsert(e) {
      if (e) {
        this.AssignmentScopeinfoStore.selectedAssignmentScopeinfo = e;
        this.AssignmentScopeStore.fetchAssignmentScopes();
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.assignmentScopeInfoID) {
        resVal = await this.AssignmentScopeinfoStore.updateAssignmentScopeinfo(
          e
        );
      } else {
        resVal = await this.AssignmentScopeinfoStore.addAssignmentScopeinfo(e);
        this.AssignmentScopeinfoStore.selectedAssignmentScopeinfo = null;
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
    async OnDelete(e) {
      this.AssignmentScopeinfoStore.DeleteAssignmentScopeinfo(
        e.assignmentScopeInfoID
      );
    },
    async onShowInfo(e) {
      this.AssignmentScopeinfoStore.selectedAssignmentScopeinfo =
        this.AssignmentScopeinfoStore.AssignmentScopeinfos.find(
          (i) => i.assignmentScopeInfoID === e.assignmentScopeInfoID
        );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.AssignmentScopeinfoStore.clearSelectedAssignmentScopeinfo();
    },

    async onAssigmentScopeChange(assigScope) {
      this.stageInfos = this.StageInfoStore.StageInfos.filter(
        (x) => x.stageID == assigScope.stageID
      );
    },
  },
  mounted() {
    this.StageInfoStore.fetchStageInfos();
    this.AssignmentScopeStore.fetchAssignmentScopes();
    this.AssignmentScopeinfoStore.fetchAssignmentScopeinfos();
  },
};
</script>

<style scoped></style>
