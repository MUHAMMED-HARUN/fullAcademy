<template>
  <div class="enrollment-view-container">
    <!-- 🔹 مكون التصفية العامة -->
    <GenericFilterComp
      :fields="studentEnrollmentFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <!-- قائمة التسجيلات -->
    <StudentEnrollmentList
      :enrollments="StudentEnrollmentStore.enrollments"
      @show-delete-enrollment="OnDelete"
      @show-info-enrollment="onShowInfo"
      @show-update-enrollment="OnUpsertEnrollment"
      @show-new-daily-evaluation="OnNewDailyEvaluation"
      @show-new-assigment-nomination="OnNewAssigmentNomination"
      @show-new-student-stage="OnNewStudentStage"
    />

    <!-- 🔹 مكون التصفح (Pagination) -->
    <PaginationComp
      :current-page="StudentEnrollmentStore.page"
      :page-size="StudentEnrollmentStore.pageSize"
      :total-count="StudentEnrollmentStore.totalCount"
      :total-pages="StudentEnrollmentStore.totalPages || totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <!-- النوافذ المنبثقة (Popups) -->
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertStudentEnrollmentComp
        :errors="StudentEnrollmentStore.error"
        :student-data="StudentStore.selectedStudent"
        :group-data="GroupStore.selectedGroup"
        :teacher-group-list="GroupStore.groups"
        @on-submit="OnSubmitUpsert"
        @on-search-student="onSearchStudent"
        @on-search-group="onSearchGroup"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onpopaClose">
      <StudentEnrollmentCard
        :-data-req="StudentEnrollmentStore.selectedEnrollment"
      />
    </PopapComp>

    <PopapComp v-if="showNewDailyEvaluation" @close="onpopaClose">
      <UpsertDailyEvaluationComp
        :-student-stage-info-data="StageinfoStore.StageInfos"
        :-last-daily-evaluation-data="
          DailyEvaluationStore.selectedDailyEvaluation
        "
        :errors="DailyEvaluationStore.error"
        @on-submit="OnSubmitNewDailyEvaluation"
        :group-name="groupName"
        :student-name="studentName"
        :enrollment-i-d="EnrollmentID"
        @on-stage-info-change="onStageInfoChange"
      />
    </PopapComp>

    <!-- ترشيح طالب -->
    <PopapComp v-if="showUpsertAssigNomin" @close="onpopaClose">
      <UpsertAssignmentNominationComp
        :errors="AssignmentNominationStore.error"
        :-assigment-scopes-data="AssignmentScopeStore.AssignmentScopes"
        :student-enrollment-i-d="EnrollmentID"
        :student-enrollment-name="studentName"
        @on-submit="OnSubmitNewAssignmentNomination"
      />
    </PopapComp>

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

    <button class="btn-add" @click="OnUpsertEnrollment(null)">
      تسجيل طالب جديد
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useStudentEnrollmentStore } from "@/store/Enrollment/StudentEnrollmentStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import { useStudentStageStore } from "@/store/Progress/StudentStageStore";
import { useStageStore } from "@/store/Progress/StageStore";
import { useDailyEvaluationStore } from "@/store/Progress/DailyEvaluationStore";
import { useGroupStore } from "@/store/Institute/GroupStore";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import StudentEnrollmentCard from "@/components/Enrollment/StudentEnrollment/StudentEnrollmentCard.vue";
import StudentEnrollmentList from "@/components/Enrollment/StudentEnrollment/StudentEnrollmentList.vue";
import UpsertStudentEnrollmentComp from "@/components/Enrollment/StudentEnrollment/UpsertStudentEnrollmentComp.vue";
import TResult from "@/Generic/TResult";
import UpsertDailyEvaluationComp from "@/components/Progress/DailyEvaluation/UpsertDailyEvaluationComp.vue";
import { useAuthStore } from "@/store/Auth/authStore";
import UpsertAssignmentNominationComp from "@/components/Assigment/AssignmentNomination/UpsertAssignmentNominationComp.vue";
import { useAssignmentScopeStore } from "@/store/Assignment/AssignmentScopeStore";
import { useAssignmentNominationStore } from "@/store/Assignment/AssignmentNominationStore";
import UpsertStudentStageComp from "@/components/Progress/StudentStage/UpsertStudentStageComp.vue";

export default {
  name: "StudentEnrollmentView",
  components: {
    GenericFilterComp,
    PaginationComp,
    StudentEnrollmentList,
    UpsertStudentEnrollmentComp,
    UpsertAssignmentNominationComp,
    StudentEnrollmentCard,
    PopapComp,
    MessageComp,
    UpsertDailyEvaluationComp,
    UpsertStudentStageComp,
  },
  data() {
    return {
      AuthStore: useAuthStore(),
      StudentEnrollmentStore: useStudentEnrollmentStore(),
      StudentStore: useStudentStore(),
      StageinfoStore: useStageInfoStore(),
      StageStore: useStageStore(),
      StudentStageStore: useStudentStageStore(),
      DailyEvaluationStore: useDailyEvaluationStore(),
      GroupStore: useGroupStore(),
      AssignmentScopeStore: useAssignmentScopeStore(),
      AssignmentNominationStore: useAssignmentNominationStore(),

      searchQuery: "",
      selectedPageSize: 10,

      groupName: "",
      studentName: "",
      EnrollmentID: "",
      showUpsert: false,
      showInfo: false,
      showUpsertAssigNomin: false,
      resultMessage: null,
      resultType: false,
      showNewDailyEvaluation: false,
      showUpsertStudentStage: false,
      // 🔹 مخطط الفلترة للمكون GenericFilterComp
      studentEnrollmentFilterSchema: [
        {
          key: "StudentName",
          label: "اسم الطالب",
          type: "text",
          operator: "=*",
        },
        {
          key: "GroupName",
          label: "اسم الشعبة",
          type: "text",
          operator: "=*",
        },
        {
          key: "EnrollmentDate",
          label: "من تاريخ التسجيل",
          type: "date",
          operator: ">=",
        },
        {
          key: "EnrollmentDate",
          label: "إلى تاريخ التسجيل",
          type: "date",
          operator: "<=",
        },
      ],
    };
  },
  computed: {
    totalPages() {
      const total = this.StudentEnrollmentStore.totalCount || 0;
      const size = this.StudentEnrollmentStore.pageSize || 10;
      return Math.ceil(total / size) || 1;
    },
  },
  methods: {
    // 🔹 أحداث الفلترة والترقيم
    async onPageChange(newPage) {
      if (typeof this.StudentEnrollmentStore.setPage === "function") {
        await this.StudentEnrollmentStore.setPage(newPage);
      } else {
        await this.changePage(newPage);
      }
    },
    async onPageSizeChange(newSize) {
      if (typeof this.StudentEnrollmentStore.setPageSize === "function") {
        await this.StudentEnrollmentStore.setPageSize(newSize);
      } else {
        this.selectedPageSize = newSize;
        await this.changePageSize();
      }
    },
    async onFilterSearch(payload) {
      let filterString = payload.filter || "";
      const enrollmentID = this.$route.query.enrollmentID;

      if (enrollmentID) {
        filterString = filterString
          ? `${filterString},EnrollmentID=${enrollmentID}`
          : `EnrollmentID=${enrollmentID}`;
      }

      await this.StudentEnrollmentStore.fetchEnrollments({
        filter: filterString,
        orderBy: payload.orderBy,
      });
    },
    async onFilterReset() {
      if (this.$route.query.enrollmentID) {
        this.$router.push({ query: {} });
      }
      await this.StudentEnrollmentStore.fetchEnrollments({});
    },

    // بناء استعلام الفلترة متوافق مع Gridify
    buildFilterQuery() {
      const filters = [];
      const groupID = this.$route.query.groupID;

      if (groupID) {
        filters.push(`GroupID=${groupID}`);
      }

      if (this.searchQuery.trim()) {
        const query = this.searchQuery.trim();
        filters.push(`(StudentName=*${query}*|GroupName=*${query}*)`);
      }

      return filters.join(",");
    },

    async applyFilter() {
      await this.loadEnrollmentData(1);
    },

    async resetFilter() {
      this.searchQuery = "";
      if (this.$route.query.enrollmentID) {
        this.$router.push({ query: {} });
      }
      await this.loadEnrollmentData(1);
    },

    async changePage(newPage) {
      if (newPage >= 1 && newPage <= this.totalPages) {
        await this.loadEnrollmentData(newPage);
      }
    },

    async changePageSize() {
      await this.loadEnrollmentData(1);
    },

    async OnUpsertEnrollment(e) {
      if (e) {
        this.StudentEnrollmentStore.selectedEnrollment = e;
        await this.onSearchStudent(
          1,
          this.StudentEnrollmentStore.selectedEnrollment.studentID
        );
        await this.onSearchGroup(
          0,
          this.StudentEnrollmentStore.selectedEnrollment.groupID
        );
      } else {
        this.StudentEnrollmentStore.selectedEnrollment = null;
        this.StudentStore.selectedStudent = null;
        this.GroupStore.selectedGroup = null;
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.enrollmentID) {
        resVal = await this.StudentEnrollmentStore.updateEnrollment(e);
      } else {
        resVal = await this.StudentEnrollmentStore.addEnrollment(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ بنجاح";
        this.resultType = false;
        this.showUpsert = false;
        await this.loadEnrollmentData(this.StudentEnrollmentStore.page);
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },

    async onSearchStudent(type, val) {
      await this.StudentStore.fetchStudentBy({
        Param: val,
        ParamType: type,
      });
    },

    async onSearchGroup(type, val) {
      await this.GroupStore.fetchGroupBy({
        Param: val,
        ParamType: type,
      });
    },

    async OnDelete(e) {
      if (confirm("هل أنت متأكد من حذف هذا التسجيل؟")) {
        const resVal = await this.StudentEnrollmentStore.DeleteEnrollment(
          e.enrollmentID
        );
        if (resVal.success) {
          this.resultMessage = "تم الحذف بنجاح";
          this.resultType = false;
          await this.loadEnrollmentData(this.StudentEnrollmentStore.page);
        } else {
          this.resultMessage = resVal.errors;
          this.resultType = true;
        }
      }
    },

    async onShowInfo(e) {
      this.StudentEnrollmentStore.selectedEnrollment =
        this.StudentEnrollmentStore.enrollments.find(
          (en) => en.enrollmentID === e.enrollmentID
        );
      this.showInfo = true;
    },

    async OnNewDailyEvaluation(e) {
      await this.StageinfoStore.getStudentStageInfo(e.enrollmentID);
      await this.DailyEvaluationStore.GetlastDailyEvaluation(e.enrollmentID);
      this.studentName = e.studentName;
      this.groupName = e.groupName;
      this.EnrollmentID = e.enrollmentID;
      this.showNewDailyEvaluation = true;
    },

    async OnNewAssigmentNomination(e) {
      await this.AssignmentScopeStore.fetchCanNominateList({
        studentEnrollmentID: e.enrollmentID,
      });
      this.studentName = e.studentName;
      this.EnrollmentID = e.enrollmentID;
      this.showUpsertAssigNomin = true;
    },
    async OnSubmitNewDailyEvaluation(e) {
      var resVal = new TResult();
      if (e.dailyEvaluationID !== "") {
        return;
      } else {
        resVal = await this.DailyEvaluationStore.addEvaluation(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showNewDailyEvaluation = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async OnSubmitNewAssignmentNomination(e) {
      var resVal = new TResult();
      if (e.assignmentNominationID !== "") {
        return;
      } else {
        resVal = await this.AssignmentNominationStore.addAssignmentNomination(
          e
        );
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showNewDailyEvaluation = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },

    async NewStudentStageSubmit(e) {
      var resVal = new TResult();
      if (e.studentStageID !== "" && e.studentStageID !== null) {
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
    async OnNewStudentStage(e) {
      this.studentName = e.studentName;
      this.EnrollmentID = e.enrollmentID;
      this.showUpsertStudentStage = true;
    },
    async onStageInfoChange(e) {
      await this.DailyEvaluationStore.GetlastDailyEvaluation(
        e.studentEnrollmentID,
        e.stageInfoID
      );
    },

    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.showNewDailyEvaluation = false;
      this.StudentEnrollmentStore.selectedEnrollment = null;
      this.StudentStore.selectedStudent = null;
      this.GroupStore.selectedGroup = null;
      this.DailyEvaluationStore.clearSelectedEvaluation();
      this.showUpsertAssigNomin = false;
      this.AssignmentNominationStore.clearSelectedAssignmentNomination();
      this.showUpsertStudentStage = false;
      this.StudentStageStore.clearSelectedStudentStage();
    },

    async loadEnrollmentData(page = 1) {
      if (this.AuthStore.UserRole === `Teacher`) {
        this.GroupStore.GetGroupsByTeacherID(this.AuthStore.UserEntityID);
      }

      const filterString = this.buildFilterQuery();
      await this.StudentEnrollmentStore.fetchEnrollments({
        page: page,
        pageSize: this.selectedPageSize,
        filter: filterString,
      });
    },
  },

  mounted() {
    this.loadEnrollmentData();
    this.StageStore.fetchStages();
  },

  watch: {
    "$route.query.groupID": {
      handler() {
        this.loadEnrollmentData();
      },
    },
  },
};
</script>

<style scoped>
.enrollment-view-container {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.btn-add {
  align-self: flex-start;
  padding: 10px 20px;
  background-color: #198754;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
</style>
