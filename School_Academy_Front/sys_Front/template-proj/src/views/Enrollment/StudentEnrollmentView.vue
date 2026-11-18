<template>
  <div class="enrollment-view-container">
    <!-- 🔹 مكون التصفية العامة -->
    <GenericFilterComp
      :fields="studentEnrollmentFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <p
      v-if="
        AuthStore.hasPermission('Can_Create_Attendance') &&
        !$route.query.groupID
      "
      class="rounded-lg border border-dashed border-[var(--border-color)] p-3 text-sm text-[var(--text-muted)]"
      role="status"
    >
      ابدأ الحضور من مجموعة محددة؛ لا يُسجّل الحضور لاشتراك أكاديمي بلا عضوية
      مجموعة.
    </p>

    <section
      v-if="isAttendanceMode"
      class="flex flex-wrap items-end gap-3 rounded-xl border border-[var(--border-color)] bg-[var(--bg-card)] p-4"
      dir="rtl"
    >
      <div class="form-group min-w-52">
        <label
          >تاريخ حضور
          {{
            StudentEnrollmentStore.enrollments[0]?.groupName || "المجموعة"
          }}</label
        >
        <input
          v-model="attendanceDate"
          type="date"
          :disabled="AttendanceStore.loading"
          @change="onAttendanceDateChange"
        />
      </div>
      <p
        v-if="AttendanceStore.loading"
        class="text-sm text-[var(--text-muted)]"
        role="status"
      >
        جارٍ تحميل حضور المجموعة…
      </p>
      <p v-else-if="AttendanceStore.error" class="alert-error" role="alert">
        {{ formatError(AttendanceStore.error) }}
      </p>
    </section>
    <p
      v-if="
        isAttendanceMode &&
        !AttendanceStore.loading &&
        !AttendanceStore.error &&
        !AttendanceStore.records.length &&
        StudentEnrollmentStore.enrollments.length
      "
      class="rounded-lg border border-dashed border-[var(--border-color)] p-3 text-sm text-[var(--text-muted)]"
      role="status"
    >
      لا توجد سجلات حضور سابقة لهذا اليوم؛ تبدأ الحالات افتراضياً بالحضور.
    </p>

    <!-- قائمة التسجيلات -->
    <StudentEnrollmentList
      :enrollments="StudentEnrollmentStore.enrollments"
      :selected-ids="selectedEnrollmentIds"
      :show-attendance="isAttendanceMode"
      :attendance-loading="AttendanceStore.loading || !!AttendanceStore.error"
      :attendance-map="attendanceRecords"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @update-attendance="onUpdateAttendance"
    />
    <p
      v-if="isAttendanceMode && !StudentEnrollmentStore.enrollments.length"
      class="rounded-lg border border-dashed border-[var(--border-color)] p-6 text-center text-[var(--text-muted)]"
      role="status"
    >
      لا توجد عضويات نشطة في المجموعة لهذا العرض.
    </p>
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
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <StudentEnrollmentCard
        :-data-req="StudentEnrollmentStore.selectedEnrollment"
      />
    </PopapComp>

    <PopapComp v-if="showNewDailyEvaluation" @close="onpopaClose">
      <UpsertDailyEvaluationComp
        :student-stage-info-data="StageinfoStore.StageInfos"
        :last-daily-evaluation-data="DailyEvaluationStore.lastDailyEvaluation"
        :selected-evaluation="null"
        :errors="DailyEvaluationStore.error"
        @submit="OnSubmitNewDailyEvaluation"
        :academic-plan-name="academicPlanName"
        :student-name="studentName"
        :student-academic-enrollment-i-d="StudentAcademicEnrollmentID"
        @stage-info-change="onStageInfoChange"
        :saving="
          DailyEvaluationStore.loading ||
          DailyEvaluationStore.lastEvaluationLoading
        "
      />
    </PopapComp>

    <!-- ترشيح طالب -->
    <PopapComp v-if="showUpsertAssigNomin" @close="onpopaClose">
      <UpsertAssignmentNominationComp
        :assignment-scopes-data="AssignmentScopeStore.AssignmentScopes"
        :student-academic-enrollment-i-d="StudentAcademicEnrollmentID"
        :student-name="studentName"
        :errors="AssignmentNominationStore.error"
        :saving="
          AssignmentNominationStore.loading || AssignmentScopeStore.loading
        "
        @submit="OnSubmitNewAssignmentNomination"
      />
    </PopapComp>

    <PopapComp
      v-if="showUpsertStudentStage"
      @close="showUpsertStudentStage = false"
    >
      <UpsertStudentStageComp
        :stage-options="eligibleStages"
        :errors="StudentStageStore.error"
        :student-academic-enrollment-i-d="StudentAcademicEnrollmentID"
        :student-name="studentName"
        :academic-plan-name="academicPlanName"
        :saving="StudentStageStore.loading"
        @submit="NewStudentStageSubmit"
      />
    </PopapComp>

    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useStudentEnrollmentStore } from "@/store/Enrollment/StudentEnrollmentStore";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import { useStudentStageStore } from "@/store/Progress/StudentStageStore";
import { useDailyEvaluationStore } from "@/store/Progress/DailyEvaluationStore";
import { useGroupStore } from "@/store/Institute/GroupStore";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import StudentEnrollmentCard from "@/components/Enrollment/StudentEnrollment/StudentEnrollmentCard.vue";
import StudentEnrollmentList from "@/components/Enrollment/StudentEnrollment/StudentEnrollmentList.vue";
import UpsertDailyEvaluationComp from "@/components/Progress/DailyEvaluation/UpsertDailyEvaluationComp.vue";
import { useAuthStore } from "@/store/Auth/authStore";
import UpsertAssignmentNominationComp from "@/components/Assigment/AssignmentNomination/UpsertAssignmentNominationComp.vue";
import { useAssignmentScopeStore } from "@/store/Assignment/AssignmentScopeStore";
import { useAssignmentNominationStore } from "@/store/Assignment/AssignmentNominationStore";
import UpsertStudentStageComp from "@/components/Progress/StudentStage/UpsertStudentStageComp.vue";
import { useAttendanceStore } from "@/store/Attendance/AttendanceStore";
import TResult from "@/Generic/TResult";
import { AttendanceStatus } from "@/Generic/AttendanceEnums";

export default {
  name: "StudentEnrollmentView",
  components: {
    GenericFilterComp,
    PaginationComp,
    StudentEnrollmentList,
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
      StageinfoStore: useStageInfoStore(),
      StudentStageStore: useStudentStageStore(),
      DailyEvaluationStore: useDailyEvaluationStore(),
      GroupStore: useGroupStore(),
      AssignmentScopeStore: useAssignmentScopeStore(),
      AssignmentNominationStore: useAssignmentNominationStore(),
      AttendanceStore: useAttendanceStore(),

      searchQuery: "",
      selectedPageSize: 10,
      attendanceDate: new Date().toISOString().slice(0, 10),

      studentName: "",
      EnrollmentID: "",
      StudentAcademicEnrollmentID: "",
      academicPlanName: "",
      eligibleStages: [],
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
        const requiredPermissions = {
          info: "Can_Read_StudentEnrollment",
          delete: "Can_Delete_StudentEnrollment",
          nominate: "Can_Create_AssignmentNomination",
          track: "Can_Create_DailyEvaluation",
          stage: "Can_Create_StudentStage",
        };
        const requiredPermission = requiredPermissions[action.key];
        if (
          requiredPermission &&
          !this.AuthStore.hasPermission(requiredPermission)
        ) {
          return false;
        }
        if (
          action.key === "startAttendance" &&
          (!this.AuthStore.hasPermission("Can_Create_Attendance") ||
            !this.AuthStore.hasPermission("Can_Read_Attendance") ||
            !this.$route.query.groupID)
        ) {
          return false;
        }
        if (
          action.key === "confirmAttendance" &&
          (!this.AuthStore.hasPermission("Can_Create_Attendance") ||
            !this.AuthStore.hasPermission("Can_Read_Attendance") ||
            !this.$route.query.groupID)
        ) {
          return false;
        }
        if (
          action.key === "cancelAttendance" &&
          !this.AuthStore.hasPermission("Can_Update_Attendance")
        ) {
          return false;
        }
        if (
          action.key === "track" &&
          !this.AuthStore.hasPermission("Can_Create_DailyEvaluation")
        ) {
          return false;
        }
        if (
          action.key === "confirmAttendance" &&
          (this.AttendanceStore.loading || this.AttendanceStore.error)
        )
          return false;
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
    formatError(value) {
      if (typeof value === "string") return value;
      if (Array.isArray(value)) return value.join("، ");
      if (value && typeof value === "object")
        return Object.values(value).flat().join("، ");
      return "حدث خطأ أثناء تحميل حضور المجموعة";
    },
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

      const groupID = this.$route.query.groupID;
      if (groupID) {
        filterString = filterString
          ? `${filterString},GroupID=${groupID}`
          : `GroupID=${groupID}`;
      }

      await this.StudentEnrollmentStore.fetchEnrollments({
        Filter: filterString,
        OrderBy: payload.orderBy,
      });
    },
    async onFilterReset() {
      if (this.$route.query.enrollmentID) {
        const query = {};
        if (this.$route.query.groupID)
          query.groupID = this.$route.query.groupID;
        if (this.$route.query.attendance)
          query.attendance = this.$route.query.attendance;
        this.$router.push({ query });
      }
      await this.loadEnrollmentData(1);
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
        const query = {};
        if (this.$route.query.groupID)
          query.groupID = this.$route.query.groupID;
        if (this.$route.query.attendance)
          query.attendance = this.$route.query.attendance;
        this.$router.push({ query });
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
      this.DailyEvaluationStore.clearSelectedEvaluation();
      this.DailyEvaluationStore.clearLastDailyEvaluation();
      const stageInfoResult = await this.StageinfoStore.getStudentStageInfo(
        e.studentAcademicEnrollmentID
      );
      if (
        !stageInfoResult.success ||
        this.StageinfoStore.StageInfos.length === 0
      ) {
        this.resultMessage = stageInfoResult.success
          ? "لا توجد تفاصيل مرحلة مسندة لهذا التسجيل"
          : stageInfoResult.errors;
        this.resultType = true;
        return;
      }
      this.studentName = e.studentName;
      this.StudentAcademicEnrollmentID = e.studentAcademicEnrollmentID;
      this.academicPlanName = e.academicPlanName || "";
      this.showNewDailyEvaluation = true;
    },

    async OnNewAssigmentNomination(e) {
      if (!e.studentAcademicEnrollmentID) {
        this.resultMessage = "الاشتراك الأكاديمي غير متاح";
        this.resultType = true;
        return;
      }
      const scopesResult = await this.AssignmentScopeStore.fetchCanNominateList(
        {
          studentAcademicEnrollmentID: e.studentAcademicEnrollmentID,
        }
      );
      if (
        !scopesResult.success ||
        !this.AssignmentScopeStore.AssignmentScopes.length
      ) {
        this.resultMessage = scopesResult.success
          ? "لا توجد نطاقات اختبار مستحقة لهذا الاشتراك"
          : scopesResult.errors;
        this.resultType = true;
        return;
      }
      this.studentName = e.studentName;
      this.StudentAcademicEnrollmentID = e.studentAcademicEnrollmentID;
      this.AssignmentNominationStore.clearSelectedAssignmentNomination();
      this.AssignmentNominationStore.error = null;
      this.showUpsertAssigNomin = true;
    },
    async OnSubmitNewDailyEvaluation(e) {
      var resVal = new TResult();
      resVal = await this.DailyEvaluationStore.addEvaluation(e);
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
      const resVal = e.assignmentNominationID
        ? await this.AssignmentNominationStore.updateAssignmentNomination(e)
        : await this.AssignmentNominationStore.addAssignmentNomination(e);
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsertAssigNomin = false;
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
      const studentAcademicEnrollmentID = e.studentAcademicEnrollmentID;
      if (!studentAcademicEnrollmentID) {
        this.resultMessage = "الاشتراك الأكاديمي غير متاح";
        this.resultType = true;
        return;
      }
      const result = await this.StageinfoStore.getStudentStageInfo(
        studentAcademicEnrollmentID
      );
      if (!result.success || this.StageinfoStore.StageInfos.length === 0) {
        this.resultMessage = result.success
          ? "لا توجد مراحل مشمولة في لقطة الاشتراك"
          : result.errors;
        this.resultType = true;
        return;
      }

      this.studentName = e.studentName;
      this.StudentAcademicEnrollmentID = studentAcademicEnrollmentID;
      this.academicPlanName = e.academicPlanName || "";
      this.eligibleStages = [
        ...new Map(
          this.StageinfoStore.StageInfos.map((info) => [
            info.stageID,
            { stageID: info.stageID, stageName: info.stageName },
          ])
        ).values(),
      ];
      this.showUpsertStudentStage = true;
    },
    async onStageInfoChange(e) {
      await this.DailyEvaluationStore.GetlastDailyEvaluation(
        e.studentAcademicEnrollmentID,
        e.stageInfoID
      );
    },

    onpopaClose() {
      this.showInfo = false;
      this.showNewDailyEvaluation = false;
      this.StudentEnrollmentStore.selectedEnrollment = null;
      this.StudentAcademicEnrollmentID = "";
      this.academicPlanName = "";
      this.eligibleStages = [];
      this.DailyEvaluationStore.clearSelectedEvaluation();
      this.DailyEvaluationStore.clearLastDailyEvaluation();
      this.showUpsertAssigNomin = false;
      this.AssignmentNominationStore.clearSelectedAssignmentNomination();
      this.showUpsertStudentStage = false;
      this.StudentStageStore.clearSelectedStudentStage();
    },

    async loadEnrollmentData(page = 1) {
      if (this.AuthStore.UserRole === `Teacher`) {
        await this.GroupStore.fetchMyTeachingGroups();
      }

      const filterString = this.buildFilterQuery();
      await this.StudentEnrollmentStore.fetchEnrollments({
        Page: page,
        PageSize: this.selectedPageSize,
        Filter: filterString,
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
        case "startAttendance":
          await this.onStartAttendance();
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

    async onStartAttendance() {
      if (
        !this.AuthStore.hasPermission("Can_Create_Attendance") ||
        !this.AuthStore.hasPermission("Can_Read_Attendance")
      ) {
        this.resultMessage = "ليس لديك صلاحية تسجيل حضور المجموعة";
        this.resultType = true;
        return;
      }

      const groupID = String(this.$route.query.groupID || "");
      if (!groupID) {
        this.resultMessage = "اختر مجموعة أولاً لبدء تسجيل الحضور";
        this.resultType = true;
        return;
      }

      this.isAttendanceMode = true;
      this.attendanceRecords = {};
      const result = await this.AttendanceStore.fetchGroupAttendance(
        groupID,
        this.attendanceDate
      );
      if (!result.success) {
        this.resultMessage = result.errors;
        this.resultType = true;
        this.isAttendanceMode = false;
        return;
      }

      this.initializeAttendanceRecords();
    },

    initializeAttendanceRecords() {
      const initialMap = {};
      const existingMap = Object.fromEntries(
        this.AttendanceStore.records.map((record) => [
          record.groupEnrollmentID,
          record.status,
        ])
      );
      this.StudentEnrollmentStore.enrollments.forEach((enroll) => {
        initialMap[enroll.enrollmentID] =
          existingMap[enroll.enrollmentID] ?? AttendanceStatus.Present;
      });
      this.attendanceRecords = initialMap;
    },

    async onAttendanceDateChange() {
      const groupID = String(this.$route.query.groupID || "");
      if (!groupID) return;
      const result = await this.AttendanceStore.fetchGroupAttendance(
        groupID,
        this.attendanceDate
      );
      if (result.success) this.initializeAttendanceRecords();
      else {
        this.attendanceRecords = {};
        this.resultMessage = result.errors;
        this.resultType = true;
      }
    },

    onCancelAttendance() {
      this.isAttendanceMode = false;
      this.attendanceRecords = {};
      this.AttendanceStore.clearGroupAttendance();
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

      const groupID = String(this.$route.query.groupID || "");
      if (
        !groupID ||
        targetEnrollments.some((enrollment) => enrollment.groupID !== groupID)
      ) {
        this.resultMessage =
          "يجب أن تضم دفعة الحضور عضويات المجموعة المحددة فقط";
        this.resultType = true;
        return;
      }

      const attendeesPayload = targetEnrollments.map((enroll) => ({
        groupEnrollmentID: enroll.groupEnrollmentID,
        groupID,
        status:
          this.attendanceRecords[enroll.enrollmentID] !== undefined
            ? this.attendanceRecords[enroll.enrollmentID]
            : AttendanceStatus.Present,
      }));

      const resVal = await this.AttendanceStore.submitAttendance({
        date: this.attendanceDate,
        attendances: attendeesPayload,
      });

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

  async mounted() {
    await this.loadEnrollmentData();
    if (this.$route.query.attendance === "true") await this.onStartAttendance();
  },

  watch: {
    "$route.query.groupID": {
      async handler() {
        this.onCancelAttendance();
        await this.loadEnrollmentData();
        if (this.$route.query.attendance === "true")
          await this.onStartAttendance();
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
