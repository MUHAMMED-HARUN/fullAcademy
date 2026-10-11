<template>
  <div>
    <AssigmentList
      @show-delete="OnDelete"
      :-assigments="AssignmentStore.assignments"
      @show-add-result="OnAddResult"
    />
    <PopapComp v-if="showNewResult" @close="showNewResult = false">
      <UpsertResultComp
        @on-submit="AddResultSubmit"
        :errors="ResultStore.error"
        :-assigment-scope-infoss-data="AssignmentScopeinfoData"
        :student-enrollment-name="StudentName"
        :asignment-i-d="asignmentID"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import AssigmentList from "@/components/Assigment/Assigment/AssigmentList.vue";
import UpsertResultComp from "@/components/Assigment/Result/UpsertResultComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
("@/components/Assigment/Assigment/AssigmentList.vue");
import { useassignmentstore } from "@/store/Assignment/AssignmentStore";
import { useAssignmentScopeinfoStore } from "@/store/Assignment/AssignmentScopeInfoStore";
import { useResultStore } from "@/store/Assignment/ResultStore";
import TResult from "@/Generic/TResult";
import MessageComp from "@/components/general/messageComp.vue";
export default {
  name: "AssignmentView",
  components: {
    AssigmentList,
    UpsertResultComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      AssignmentStore: useassignmentstore(),
      AssignmentScopeinfoStore: useAssignmentScopeinfoStore(),
      ResultStore: useResultStore(),
      showNewResult: false,
      StudentName: "",
      asignmentID: "",
      AssignmentScopeinfoData: [],
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    async OnDelete(e) {
      this.AssignmentStore.DeleteAssignment(e.assignmentID);
    },
    async OnAddResult(e) {
      this.StudentName = e.studentName;
      this.asignmentID = e.assignmentID;

      await this.AssignmentScopeinfoStore.fetchAssignmentScopeinfosForAssigment(
        { AssignmentID: e.assignmentID }
      );
      this.AssignmentScopeinfoData =
        this.AssignmentScopeinfoStore.AssignmentScopeinfos;
      this.showNewResult = true;
    },
    async AddResultSubmit(e) {
      var resVal = new TResult();
      if (e.resultID) {
        return;
      } else {
        resVal = await this.ResultStore.addResult(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ بنجاح";
        this.resultType = false;
        this.showNewResult = false;
        await this.ResultStore.fetchResults();
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
  },
  mounted() {
    this.AssignmentStore.fetchassignments();
    this.AssignmentScopeinfoStore.fetchAssignmentScopeinfos();
  },
};
</script>

<style scoped></style>
