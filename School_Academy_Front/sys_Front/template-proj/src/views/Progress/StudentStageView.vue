<template>
  <main dir="rtl" class="space-y-4">
    <header class="flex flex-wrap items-start justify-between gap-3">
      <div>
        <h1 class="text-xl font-bold">متابعة المراحل</h1>
        <p v-if="academicPlanName" class="text-sm text-[var(--text-muted)]">
          {{ studentName }} — {{ academicPlanName }}
        </p>
      </div>
      <button
        v-if="
          contextEnrollmentID &&
          AuthStore.hasPermission('Can_Create_StudentStage')
        "
        type="button"
        class="btn btn-primary"
        :disabled="StudentStageStore.loading || stageOptions.length === 0"
        @click="openCreate"
      >
        إضافة متابعة مرحلة
      </button>
    </header>

    <GenericFilterComp
      :fields="filterFields"
      @search="searchStages"
      @reset="resetSearch"
    />
    <p v-if="StudentStageStore.loading" class="py-6 text-center" role="status">
      جارٍ تحميل متابعات المراحل…
    </p>
    <p v-else-if="StudentStageStore.error" class="alert-error" role="alert">
      {{ formatError(StudentStageStore.error) }}
    </p>
    <p
      v-else-if="!StudentStageStore.StudentStages.length"
      class="rounded-lg border border-dashed p-6 text-center text-[var(--text-muted)]"
      role="status"
    >
      لا توجد متابعات مراحل مطابقة.
    </p>
    <StudentStageList
      v-else
      :StudentStages="StudentStageStore.StudentStages"
      :can-delete="AuthStore.hasPermission('Can_Delete_StudentStage')"
      :can-update="AuthStore.hasPermission('Can_Update_StudentStage')"
      @show-delete="onDelete"
      @show-update="onEdit"
    />
    <PaginationComp
      :current-page="StudentStageStore.page"
      :page-size="StudentStageStore.pageSize"
      :total-count="StudentStageStore.totalCount"
      :total-pages="StudentStageStore.totalPages"
      @change-page="StudentStageStore.setPage"
      @change-size="StudentStageStore.setPageSize"
    />

    <PopapComp v-if="showUpsert" @close="closePopup">
      <UpsertStudentStageComp
        :stage-options="stageOptions"
        :studentAcademicEnrollmentID="contextEnrollmentID"
        :student-name="studentName"
        :academic-plan-name="academicPlanName"
        :student-stage="StudentStageStore.selectedStudentStage"
        :errors="StudentStageStore.error"
        :saving="StudentStageStore.loading"
        @submit="submitStudentStage"
      />
    </PopapComp>
    <MessageComp :is-error="resultError" :msg="resultMessage" />
  </main>
</template>

<script>
import { useStudentStageStore } from "@/store/Progress/StudentStageStore";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import { useStudentAcademicEnrollmentStore } from "@/store/Enrollment/StudentAcademicEnrollmentStore";
import { useAuthStore } from "@/store/Auth/authStore";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import StudentStageList from "@/components/Progress/StudentStage/StudentStageList.vue";
import UpsertStudentStageComp from "@/components/Progress/StudentStage/UpsertStudentStageComp.vue";

export default {
  name: "StudentStageView",
  components: {
    StudentStageList,
    UpsertStudentStageComp,
    GenericFilterComp,
    PaginationComp,
    MessageComp,
    PopapComp,
  },
  data() {
    return {
      AuthStore: useAuthStore(),
      StudentStageStore: useStudentStageStore(),
      StageInfoStore: useStageInfoStore(),
      StudentAcademicEnrollmentStore: useStudentAcademicEnrollmentStore(),
      contextEnrollmentID: "",
      studentName: "",
      academicPlanName: "",
      stageOptions: [],
      showUpsert: false,
      resultMessage: null,
      resultError: false,
      filterFields: [
        {
          key: "StudentName",
          label: "اسم الطالب",
          type: "text",
          operator: "=*",
        },
        {
          key: "AcademicPlanName",
          label: "الخطة الأكاديمية",
          type: "text",
          operator: "=*",
        },
        { key: "StageName", label: "المرحلة", type: "text", operator: "=*" },
      ],
    };
  },
  methods: {
    formatError(value) {
      if (typeof value === "string") return value;
      if (Array.isArray(value)) return value.join("، ");
      if (value && typeof value === "object")
        return Object.values(value).flat().join("، ");
      return "حدث خطأ";
    },
    async loadEnrollmentContext(studentAcademicEnrollmentID) {
      this.contextEnrollmentID = studentAcademicEnrollmentID || "";
      this.stageOptions = [];
      this.studentName = "";
      this.academicPlanName = "";
      if (!this.contextEnrollmentID) return false;

      const [enrollmentResult, stageInfoResult] = await Promise.all([
        this.StudentAcademicEnrollmentStore.fetchForUpsert(
          this.contextEnrollmentID
        ),
        this.StageInfoStore.getStudentStageInfo(this.contextEnrollmentID),
      ]);
      if (!enrollmentResult.success || !stageInfoResult.success) {
        this.resultMessage = enrollmentResult.errors || stageInfoResult.errors;
        this.resultError = true;
        return;
      }
      this.studentName = enrollmentResult.data?.studentName || "";
      this.academicPlanName = enrollmentResult.data?.academicPlanName || "";
      this.stageOptions = [
        ...new Map(
          this.StageInfoStore.StageInfos.map((item) => [
            item.stageID,
            { stageID: item.stageID, stageName: item.stageName },
          ])
        ).values(),
      ];
    },
    async searchStages(payload) {
      await this.StudentStageStore.fetchStudentStages({
        Page: 1,
        Filter: payload.filter || "",
        StudentAcademicEnrollmentID: this.contextEnrollmentID,
      });
    },
    async resetSearch() {
      await this.StudentStageStore.fetchStudentStages({
        Page: 1,
        Filter: "",
        StudentAcademicEnrollmentID: this.contextEnrollmentID,
      });
    },
    openCreate() {
      this.StudentStageStore.clearSelectedStudentStage();
      this.StudentStageStore.error = null;
      this.showUpsert = true;
    },
    async onEdit(item) {
      const result = await this.StudentStageStore.fetchStudentStagetBy({
        Param: item.studentStageID,
        ParamType: 0,
      });
      if (!result.success) {
        this.resultMessage = result.errors;
        this.resultError = true;
        return;
      }
      await this.loadEnrollmentContext(item.studentAcademicEnrollmentID);
      this.showUpsert = true;
    },
    async onDelete(item) {
      if (!window.confirm("هل تريد حذف متابعة المرحلة؟")) return;
      const result = await this.StudentStageStore.DeleteStudentStage(
        item.studentStageID
      );
      this.resultMessage = result.success ? "تم حذف المتابعة" : result.errors;
      this.resultError = !result.success;
    },
    async submitStudentStage(payload) {
      const result = payload.studentStageID
        ? await this.StudentStageStore.updateStudentStage(payload)
        : await this.StudentStageStore.addStudentStage(payload);
      this.resultMessage = result.success
        ? "تم حفظ متابعة المرحلة"
        : result.errors;
      this.resultError = !result.success;
      if (result.success) this.closePopup();
    },
    closePopup() {
      this.showUpsert = false;
      this.StudentStageStore.clearSelectedStudentStage();
    },
  },
  async mounted() {
    await this.loadEnrollmentContext(
      this.$route.query.studentAcademicEnrollmentID
    );
    await this.StudentStageStore.fetchStudentStages({
      Page: 1,
      StudentAcademicEnrollmentID: this.contextEnrollmentID,
    });
  },
  watch: {
    "$route.query.studentAcademicEnrollmentID": async function (id) {
      this.closePopup();
      await this.loadEnrollmentContext(id);
      await this.StudentStageStore.fetchStudentStages({
        Page: 1,
        StudentAcademicEnrollmentID: this.contextEnrollmentID,
      });
    },
  },
};
</script>
