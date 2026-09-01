<template>
  <div>
    <StudentStageList
      @show-delete="OnDelete"
      :-student-stages="StudentStageStore.StudentStages"
    />
    <PopapComp
      v-if="showUpsertStudentStage"
      @close="showUpsertStudentStage = false"
    >
      <UpsertStudentStageComp
        :stages="StageStore.stages"
        :errors="StudentStageStore.error"
        :student-enrollment-i-d="EnrollmentID"
        :student-enrollment-name="studentName"
        @on-submit="NewStudentStageSubmit"
      />
    </PopapComp>

    <!-- رسائل التنبيه والنتائج -->
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import { useStudentStageStore } from "@/store/Progress/StudentStageStore";
import { useStageStore } from "@/store/Progress/StageStore";
import MessageComp from "@/components/general/messageComp.vue";
import { useassignmentstore } from "@/store/Assignment/AssignmentStore";
import TResult from "@/Generic/TResult";
import PopapComp from "@/components/general/popapComp.vue";
import StudentStageList from "@/components/Progress/StudentStage/StudentStageList.vue";
import UpsertStudentStageComp from "@/components/Progress/StudentStage/UpsertStudentStageComp.vue";

export default {
  name: "StudentStageView",
  components: {
    StudentStageList,
    MessageComp,
    UpsertStudentStageComp,
    PopapComp,
  },
  data() {
    return {
      StageStore: useStageStore(),
      StudentStageStore: useStudentStageStore(),
      assignmentstore: useassignmentstore(),
      resultMessage: null,
      resultType: false,
      studentName: "",
      EnrollmentID: "",
      showUpsertStudentStage: false,
    };
  },
  methods: {
    async OnDelete(e) {
      this.StudentStageStore.DeleteStudentStage(e.studentStageID);
    },

    async OnNewStudentStage(e) {
      this.studentName = e.studentName;
      this.EnrollmentID = e.enrollmentID;
      this.showUpsertStudentStage = true;
    },
    async NewStudentStageSubmit(e) {
      var resVal = new TResult();
      if (e.studentStageID !== "") {
        resVal = await this.StudentStageStore.updateStudentStage(e);
      } else {
        resVal = await this.StudentStageStore.addStudentStage(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsertStudentStage = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
  },
  mounted() {
    this.StudentStageStore.fetchStudentStages();
    this.StageStore.fetchStages();
  },
};
</script>

<style scoped></style>
