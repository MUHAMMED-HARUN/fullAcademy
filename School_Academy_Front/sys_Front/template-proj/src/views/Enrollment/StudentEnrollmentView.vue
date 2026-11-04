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
      :selected-ids="selectedEnrollmentIds"
      :show-attendance="isAttendanceMode"
      :attendance-map="attendanceRecords"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @update-attendance="onUpdateAttendance"
    />
    <!-- 🔹 3. أزرار العمليات المجمعة (توضع تماماً مثل التصميم أسفل الجدول) -->
    <GenericComboActions
      :actions="visibleEnrollmentActions"
      :selected-items="selectedEnrollmentsData"
      @action-click="handleComboAction"
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
        :group-results="GroupStore.groups"
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
import GenericComboActions from "@/components/general/GenericComboActions.vue";
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
import { useAttendanceStore } from "@/store/Attendance/AttendanceStore";
import {
  AttendanceStatus,
  AttendeeType,
  ContextType,
} from "@/Generic/AttendanceEnums";

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
    GenericComboActions,
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
      AttendanceStore: useAttendanceStore(),

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
      isAttendanceMode: false,
      attendanceRecords: {},
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
      selectedEnrollmentIds: [], // معرفات السجلات المحددة
      enrollmentActions: [
        {
          key: "info",
          label: "معلومات",
          minSelection: 1,
          maxSelection: 1, // تعمل على سجل واحد فقط
        },
        {
          key: "update",
          label: "تحديث",
          minSelection: 1,
          maxSelection: 1, // تعمل على سجل واحد فقط
        },
        {
          key: "startAttendance",
          label: "بدء عملية حضور وغياب",
          minSelection: 0, // يظهر ويعمل دائماً
          class: "btn-primary",
        },
        {
          key: "confirmAttendance",
          label: "تأكيد التفقد",
          minSelection: 0,
          class: "btn-success",
        },
        {
          key: "cancelAttendance",
          label: "إلغاء التفقد",
          minSelection: 0,
          class: "btn-secondary",
        },
        {
          key: "nominate",
          label: "اضافة ترشيح",
          minSelection: 1,
          condition: (item) => item.canNominate, // تعمل بشرط أن يكون السجل يسمح بالترشيح
        },
        {
          key: "track",
          label: "اضافة متابعة",
          minSelection: 1,
          condition: (item) => item.canTrack,
        },
        {
          key: "stage",
          label: "اضافة الى مرحلة",
          minSelection: 1,
          condition: (item) => item.canAddToStage,
        },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1, // يمكن أن تعمل على سجل واحد أو أكثر حسب رغبتك
          class: "btn-danger",
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
    selectedEnrollmentsData() {
      return this.StudentEnrollmentStore.enrollments.filter((en) =>
        this.selectedEnrollmentIds.includes(en.enrollmentID)
      );
    },
    visibleEnrollmentActions() {
      return this.enrollmentActions.filter((action) => {
        if (this.isAttendanceMode) {
          return ["confirmAttendance", "cancelAttendance"].includes(action.key);
        } else {
          return !["confirmAttendance", "cancelAttendance"].includes(
            action.key
          );
        }
      });
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
        filters.push(`(StudentName=*${query}|GroupName=*${query})`);
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
      } else if (Array.isArray(e.studentIDs) && e.studentIDs.length > 0) {
        resVal = await this.StudentEnrollmentStore.enrollByRange({
          StudentIDs: e.studentIDs,
          GroupID: e.groupID,
          Date: e.date,
        });
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
      let filter = "";
      if (type === 1) filter = `GroupName=*${val}`;
      else if (type === 2) filter = `Code=*${val}`;
      else filter = `GroupID=${val}`;
      await this.GroupStore.fetchGroups({ Filter: filter });
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
      // لا يمكن إجراء متابعة يومية إذا كان الطالب غائباً أو معذوراً
      const status = this.attendanceRecords[e.enrollmentID];
      if (
        status === AttendanceStatus.Absent ||
        status === AttendanceStatus.Excused
      ) {
        this.resultMessage = "لا يمكن إجراء متابعة يومية للطالب (غياب/إذن)";
        this.resultType = true;
        return;
      }
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

    onToggleSelectAll(isChecked) {
      if (isChecked) {
        this.selectedEnrollmentIds =
          this.StudentEnrollmentStore.enrollments.map((en) => en.enrollmentID);
      } else {
        this.selectedEnrollmentIds = [];
      }
    },
    onToggleSelectRow(enroll) {
      const index = this.selectedEnrollmentIds.indexOf(enroll.enrollmentID);
      if (index > -1) {
        this.selectedEnrollmentIds.splice(index, 1);
      } else {
        this.selectedEnrollmentIds.push(enroll.enrollmentID);
      }
    },

    // موجه تنفيذ الأزرار المجمعة
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];

      switch (actionKey) {
        case "info":
          await this.onShowInfo(singleItem);
          break;
        case "update":
          await this.OnUpsertEnrollment(singleItem);
          break;
        case "startAttendance":
          this.onStartAttendance();
          break;
        case "confirmAttendance":
          await this.onConfirmAttendance(selectedItems);
          break;
        case "cancelAttendance":
          this.onCancelAttendance();
          break;
        case "nominate":
          await this.OnNewAssigmentNomination(singleItem);
          break;
        case "track":
          await this.OnNewDailyEvaluation(singleItem);
          break;
        case "stage":
          await this.OnNewStudentStage(singleItem);
          break;
        case "delete":
          // مثال لحذف متعدد أو مفرد
          await this.OnDeleteMultiple(selectedItems);
          break;
      }
    },

    onStartAttendance() {
      this.isAttendanceMode = true;
      const initialMap = {};
      this.StudentEnrollmentStore.enrollments.forEach((enroll) => {
        initialMap[enroll.enrollmentID] = AttendanceStatus.Present;
      });
      this.attendanceRecords = initialMap;
    },

    onCancelAttendance() {
      this.isAttendanceMode = false;
      this.attendanceRecords = {};
    },

    onUpdateAttendance({ enrollmentID, status }) {
      this.attendanceRecords = {
        ...this.attendanceRecords,
        [enrollmentID]: status,
      };
    },

    async onConfirmAttendance(selectedItems) {
      // إذا تم تحديد صفوف نأخذ المحددة فقط، وإن لم يحدد نأخذ جميع المسجلين المعروضين
      const targetEnrollments =
        selectedItems && selectedItems.length > 0
          ? selectedItems
          : this.StudentEnrollmentStore.enrollments;

      if (!targetEnrollments || targetEnrollments.length === 0) {
        this.resultMessage = "لا يوجد طلاب لتسجيل الحضور لهم";
        this.resultType = true;
        return;
      }

      const attendeesPayload = targetEnrollments.map((enroll) => ({
        entityID: enroll.enrollmentID,
        attendeeType: AttendeeType.Student, // Student
        status:
          this.attendanceRecords[enroll.enrollmentID] !== undefined
            ? this.attendanceRecords[enroll.enrollmentID]
            : AttendanceStatus.Present,
        contextID: enroll.groupID || null,
        contextType: ContextType.Group,
      }));

      const resVal = await this.AttendanceStore.submitAttendance(
        attendeesPayload
      );

      if (resVal.success) {
        this.resultMessage = "تم تأكيد وتسجيل التفقد بنجاح";
        this.resultType = false;
        this.isAttendanceMode = false;
        this.attendanceRecords = {};
      } else {
        this.resultMessage = resVal.errors || "حدث خطأ أثناء حفظ التفقد";
        this.resultType = true;
      }
    },
    async OnDeleteMultiple(items) {
      if (confirm(`هل أنت متأكد من حذف ${items.length} سجلات المحددة؟`)) {
        // يمكنك تنفيذ الحذف المتعدد أو التكرار حسب متطلبات الـ Store لديك
        for (const item of items) {
          await this.StudentEnrollmentStore.DeleteEnrollment(item.enrollmentID);
        }
        this.selectedEnrollmentIds = [];
        this.resultMessage = "تم الحذف بنجاح";
        this.resultType = false;
        await this.loadEnrollmentData(this.StudentEnrollmentStore.page);
      }
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
