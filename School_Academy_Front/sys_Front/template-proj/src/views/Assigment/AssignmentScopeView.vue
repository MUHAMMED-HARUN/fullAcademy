<template>
  <div>
    <AssignmentScopeList
      @show-delete="OnDelete"
      @show-info="onShowInfo"
      @show-update="OnUpsert"
      :-assignment-scopes="AssignmentScopeStore.AssignmentScopes"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertAssignmentScopeComp
        :errors="AssignmentScopeStore.error"
        :-assignment-scopes="AssignmentScopeStore.AssignmentScopes"
        :stage-data="StageStore.selectedStage"
        @on-search-stage="onSearchStage"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <AssignmentScopeCard
        :-data-req="AssignmentScopeStore.selectedAssignmentScope"
      />
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
import AssignmentScopeList from "@/components/Assigment/AssignmentScope/AssignmentScopeList.vue";
import UpsertAssignmentScopeComp from "@/components/Assigment/AssignmentScope/UpsertAssignmentScopeComp.vue";
import AssignmentScopeCard from "@/components/Assigment/AssignmentScope/AssignmentScopeCard.vue";
import { useAssignmentScopeStore } from "@/store/Assignment/AssignmentScopeStore";

export default {
  name: "assignmentScopeView",
  components: {
    AssignmentScopeList,
    AssignmentScopeCard,
    UpsertAssignmentScopeComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      AssignmentScopeStore: useAssignmentScopeStore(),
      StageStore: useStageStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    async OnUpsert(e) {
      if (e) {
        this.AssignmentScopeStore.selectedAssignmentScope = e;
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.assignmentScopeID) {
        resVal = await this.AssignmentScopeStore.updateAssignmentScope(e);
      } else {
        resVal = await this.AssignmentScopeStore.addAssignmentScope(e);
        this.AssignmentScopeStore.selectedAssignmentScope = null;
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
      this.AssignmentScopeStore.DeleteAssignmentScope(e.assignmentScopeID);
    },
    async onShowInfo(e) {
      this.AssignmentScopeStore.selectedAssignmentScope =
        this.AssignmentScopeStore.AssignmentScopes.find(
          (i) => i.assignmentScopeID === e.assignmentScopeID
        );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.StageStore.selectedStage = null;
    },

    async onSearchStage(type, val) {
      await this.StageStore.fetchStagetBy({
        Param: val,
        ParamType: type,
      });
    },
  },
  mounted() {
    this.AssignmentScopeStore.fetchAssignmentScopes();
  },
};
</script>

<style scoped></style>
