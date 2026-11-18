<template>
  <div class="space-y-4" dir="rtl">
    <GenericFilterComp
      :fields="dailyEvaluationFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <button
      v-if="studentAcademicEnrollmentID && canCreateDailyEvaluation"
      type="button"
      class="btn btn-primary"
      @click="onNewDailyEvaluation"
    >
      إضافة متابعة للاشتراك
    </button>

    <p
      v-if="DailyEvaluationStore.loading"
      class="text-sm text-[var(--text-muted)]"
      role="status"
    >
      جارٍ تحميل المتابعات اليومية…
    </p>
    <p v-else-if="DailyEvaluationStore.error" class="alert-error" role="alert">
      {{ DailyEvaluationStore.error }}
    </p>
    <p
      v-else-if="DailyEvaluationStore.evaluations.length === 0"
      class="rounded-lg border border-dashed border-[var(--border-color)] p-4 text-center text-[var(--text-muted)]"
      role="status"
    >
      لا توجد متابعات يومية مطابقة.
    </p>

    <DailyEvaluationList
      v-if="DailyEvaluationStore.evaluations.length > 0"
      :daily-evaluations="DailyEvaluationStore.evaluations"
      :can-read="canReadDailyEvaluation"
      :can-update="canUpdateDailyEvaluation"
      :can-delete="canDeleteDailyEvaluation"
      @show-delete="onDeleteDailyEvaluation"
      @show-info="onShowInfoDailyEvaluation"
      @show-update="onUpsertDailyEvaluation"
    />

    <PaginationComp
      :current-page="DailyEvaluationStore.page"
      :page-size="DailyEvaluationStore.pageSize"
      :total-count="DailyEvaluationStore.totalCount"
      :total-pages="DailyEvaluationStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onPopupClose">
      <UpsertDailyEvaluationComp
        :student-stage-info-data="StageInfoStore.StageInfos"
        :last-daily-evaluation-data="DailyEvaluationStore.lastDailyEvaluation"
        :selected-evaluation="DailyEvaluationStore.selectedEvaluation"
        :student-name="
          activeEnrollment?.studentName ||
          DailyEvaluationStore.selectedEvaluation?.studentName ||
          ''
        "
        :academic-plan-name="
          activeEnrollment?.academicPlanName ||
          DailyEvaluationStore.selectedEvaluation?.academicPlanName ||
          ''
        "
        :student-academic-enrollment-id="
          activeEnrollment?.studentAcademicEnrollmentID ||
          DailyEvaluationStore.selectedEvaluation
            ?.studentAcademicEnrollmentID ||
          studentAcademicEnrollmentID
        "
        :errors="DailyEvaluationStore.error"
        :saving="
          DailyEvaluationStore.loading ||
          DailyEvaluationStore.lastEvaluationLoading
        "
        @submit="onSubmitUpsert"
        @stage-info-change="onStageInfoChange"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onPopupClose">
      <DailyEvaluationCard :DataReq="DailyEvaluationStore.selectedEvaluation" />
    </PopapComp>

    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import DailyEvaluationList from "@/components/Progress/DailyEvaluation/DailyEvaluationList.vue";
import UpsertDailyEvaluationComp from "@/components/Progress/DailyEvaluation/UpsertDailyEvaluationComp.vue";
import DailyEvaluationCard from "@/components/Progress/DailyEvaluation/DailyEvaluationCard.vue";
import { useDailyEvaluationStore } from "@/store/Progress/DailyEvaluationStore";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import { useStudentAcademicEnrollmentStore } from "@/store/Enrollment/StudentAcademicEnrollmentStore";
import { useAuthStore } from "@/store/Auth/authStore";

export default {
  name: "DailyEvaluationView",
  components: {
    DailyEvaluationList,
    UpsertDailyEvaluationComp,
    DailyEvaluationCard,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      DailyEvaluationStore: useDailyEvaluationStore(),
      StageInfoStore: useStageInfoStore(),
      StudentAcademicEnrollmentStore: useStudentAcademicEnrollmentStore(),
      AuthStore: useAuthStore(),
      activeEnrollment: null,
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      dailyEvaluationFilterSchema: [
        {
          key: "StudentName",
          label: "اسم الطالب",
          type: "text",
          operator: "=*",
        },
        {
          key: "Date",
          label: "من تاريخ التقييم",
          type: "date",
          operator: ">=",
        },
        {
          key: "LevelType",
          label: "مستوى التقييم",
          type: "select",
          options: [
            { label: "سيء جدا", value: "سيء جدا" },
            { label: "سيء", value: "سيء" },
            { label: "جيد", value: "جيد" },
            { label: "جيد جدا", value: "جيد جدا" },
            { label: "ممتاز", value: "ممتاز" },
          ],
        },
      ],
    };
  },
  computed: {
    studentAcademicEnrollmentID() {
      return String(this.$route.query.studentAcademicEnrollmentID || "");
    },
    canCreateDailyEvaluation() {
      return this.AuthStore.hasPermission("Can_Create_DailyEvaluation");
    },
    canReadDailyEvaluation() {
      return this.AuthStore.hasPermission("Can_Read_DailyEvaluation");
    },
    canUpdateDailyEvaluation() {
      return this.AuthStore.hasPermission("Can_Update_DailyEvaluation");
    },
    canDeleteDailyEvaluation() {
      return this.AuthStore.hasPermission("Can_Delete_DailyEvaluation");
    },
  },
  methods: {
    async onPageChange(page) {
      await this.DailyEvaluationStore.setPage(page);
    },
    async onPageSizeChange(pageSize) {
      await this.DailyEvaluationStore.setPageSize(pageSize);
    },
    async onFilterSearch(payload) {
      await this.DailyEvaluationStore.fetchEvaluations({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
        StudentAcademicEnrollmentID: this.studentAcademicEnrollmentID,
      });
    },
    async onFilterReset() {
      await this.DailyEvaluationStore.fetchEvaluations({
        Page: 1,
        Filter: "",
        OrderBy: "",
        StudentAcademicEnrollmentID: this.studentAcademicEnrollmentID,
      });
    },
    async loadEnrollmentContext(studentAcademicEnrollmentID) {
      if (!studentAcademicEnrollmentID) {
        this.activeEnrollment = null;
        this.StageInfoStore.StageInfos = [];
        return false;
      }
      this.activeEnrollment = null;
      this.StageInfoStore.StageInfos = [];
      const result = await this.StudentAcademicEnrollmentStore.fetchForUpsert(
        studentAcademicEnrollmentID
      );
      if (!result.success || !result.data) {
        this.activeEnrollment = null;
        this.resultMessage = result.errors || "تعذر تحميل الاشتراك الأكاديمي";
        this.resultType = true;
        return false;
      }
      this.activeEnrollment = result.data;
      const stageInfoResult = await this.StageInfoStore.getStudentStageInfo(
        studentAcademicEnrollmentID
      );
      if (
        !stageInfoResult.success ||
        this.StageInfoStore.StageInfos.length === 0
      ) {
        this.resultMessage = stageInfoResult.success
          ? "لا توجد مواد مصرح بها في لقطة الاشتراك"
          : stageInfoResult.errors;
        this.resultType = true;
        return false;
      }
      return true;
    },
    async onNewDailyEvaluation() {
      const allowed = await this.loadEnrollmentContext(
        this.studentAcademicEnrollmentID
      );
      if (!allowed) return;
      this.DailyEvaluationStore.clearSelectedEvaluation();
      this.DailyEvaluationStore.clearLastDailyEvaluation();
      this.showUpsert = true;
    },
    async onStageInfoChange({ studentAcademicEnrollmentID, stageInfoID }) {
      await this.DailyEvaluationStore.GetlastDailyEvaluation(
        studentAcademicEnrollmentID,
        stageInfoID
      );
    },
    async onUpsertDailyEvaluation(evaluation) {
      const result = await this.DailyEvaluationStore.fetchEvaluationBy({
        Param: evaluation.dailyEvaluationID,
        ParamType: 0,
      });
      if (!result.success || !this.DailyEvaluationStore.selectedEvaluation) {
        this.resultMessage = result.errors || "تعذر تحميل بيانات المتابعة";
        this.resultType = true;
        return;
      }

      const enrollmentID =
        this.DailyEvaluationStore.selectedEvaluation
          .studentAcademicEnrollmentID;
      const enrollmentLoaded = await this.loadEnrollmentContext(enrollmentID);
      if (!enrollmentLoaded) {
        this.DailyEvaluationStore.clearSelectedEvaluation();
        return;
      }
      const hasSelectedStageInfo = this.StageInfoStore.StageInfos.some(
        (stageInfo) =>
          stageInfo.stageInfoID ===
          this.DailyEvaluationStore.selectedEvaluation.stageInfoID
      );
      if (!hasSelectedStageInfo) {
        this.resultMessage =
          "تفاصيل المرحلة الخاصة بهذه المتابعة غير متاحة لهذا الاشتراك";
        this.resultType = true;
        this.DailyEvaluationStore.clearSelectedEvaluation();
        return;
      }

      this.DailyEvaluationStore.clearLastDailyEvaluation();
      this.showUpsert = true;
    },
    async onSubmitUpsert(payload) {
      const result = payload.dailyEvaluationID
        ? await this.DailyEvaluationStore.updateEvaluation(payload)
        : await this.DailyEvaluationStore.addEvaluation(payload);

      this.resultMessage = result.success ? "تم حفظ المتابعة" : result.errors;
      this.resultType = !result.success;
      if (result.success) this.onPopupClose();
    },
    async onDeleteDailyEvaluation(evaluation) {
      if (!window.confirm("هل أنت متأكد من حذف المتابعة اليومية؟")) return;
      const result = await this.DailyEvaluationStore.deleteEvaluation(
        evaluation.dailyEvaluationID
      );
      this.resultMessage = result.success ? "تم حذف المتابعة" : result.errors;
      this.resultType = !result.success;
    },
    onShowInfoDailyEvaluation(evaluation) {
      this.DailyEvaluationStore.selectedEvaluation = evaluation;
      this.showInfo = true;
    },
    onPopupClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.activeEnrollment = null;
      this.DailyEvaluationStore.clearSelectedEvaluation();
      this.DailyEvaluationStore.clearLastDailyEvaluation();
    },
  },
  async mounted() {
    await this.DailyEvaluationStore.fetchEvaluations({
      Page: 1,
      StudentAcademicEnrollmentID: this.studentAcademicEnrollmentID,
    });
  },
  watch: {
    studentAcademicEnrollmentID: async function (id) {
      this.onPopupClose();
      await this.DailyEvaluationStore.fetchEvaluations({
        Page: 1,
        StudentAcademicEnrollmentID: id,
      });
    },
  },
};
</script>
