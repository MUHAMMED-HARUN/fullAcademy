<template>
  <div>
    <ResultList
      @show-delete="OnDelete"
      @show-update="OnUpsertSubmit"
      :-results="ResultStore.Results"
    />
    <PopapComp v-if="showUpsert" @close="showUpsert = false">
      <UpsertResultComp
        :-assigment-scope-infoss-data="AssignmentScopeInfos"
        @on-submit="UpsertSubmit"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import { useResultStore } from "@/store/Assignment/ResultStore";
import MessageComp from "@/components/general/messageComp.vue";
import TResult from "@/Generic/TResult";
import PopapComp from "@/components/general/popapComp.vue";
import ResultList from "@/components/Assigment/Result/ResultList.vue";
import UpsertResultComp from "@/components/Assigment/Result/UpsertResultComp.vue";
import { useAssignmentScopeinfoStore } from "@/store/Assignment/AssignmentScopeInfoStore";

export default {
  name: "AssignmentNominationView",
  components: {
    ResultList,
    MessageComp,
    UpsertResultComp,
    PopapComp,
  },
  data() {
    return {
      ResultStore: useResultStore(),
      AssignmentScopeInfoStore: useAssignmentScopeinfoStore(),
      showUpsert: false,
      resultMessage: null,
      resultType: false,
      AssignmentScopeInfos: [],
    };
  },
  methods: {
    async OnDelete(e) {
      this.ResultStore.DeleteResult(e.resultID);
    },
    async OnUpsertSubmit(e) {
      this.ResultStore.selectedResult = e;
      this.AssignmentScopeInfos = this.ResultStore.Results.filter(
        (r) => r.assignmentID == e.assignmentID
      );
      this.showUpsert = true;
    },
    async UpsertSubmit(e) {
      var resVal = new TResult();
      if (e.resultID) {
        resVal = await this.ResultStore.updateResult(e);
      } else {
        resVal = await this.ResultStore.addResult(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.ResultStore.fetchResults();
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
  },
  async mounted() {
    await this.ResultStore.fetchResults();
    await this.AssignmentScopeInfoStore.fetchAssignmentScopeinfos();
  },
};
</script>

<style scoped></style>
