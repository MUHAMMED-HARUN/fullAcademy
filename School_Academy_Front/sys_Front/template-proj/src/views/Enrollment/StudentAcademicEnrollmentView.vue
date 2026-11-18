<template>
  <main dir="rtl" class="space-y-4">
    <header class="flex flex-wrap items-center justify-between gap-3">
      <div>
        <h1 class="text-xl font-bold">الاشتراكات الأكاديمية</h1>
        <p class="text-sm text-[var(--text-muted)]">
          تسجيل مستقل عن عضوية الطالب في المجموعة.
        </p>
      </div>
      <button
        v-if="AuthStore.hasPermission('Can_Create_StudentAcademicEnrollment')"
        class="btn btn-primary"
        type="button"
        @click="openCreate"
      >
        تسجيل اشتراك
      </button>
    </header>

    <form class="flex flex-wrap gap-2" @submit.prevent="searchList">
      <input
        v-model="listSearch"
        type="search"
        class="form-control min-w-60 flex-1"
        placeholder="بحث باسم الطالب أو الخطة"
      />
      <button class="btn btn-outline-primary" type="submit">بحث</button>
      <button
        class="btn btn-outline-secondary"
        type="button"
        @click="resetSearch"
      >
        مسح
      </button>
    </form>

    <p v-if="EnrollmentStore.error" class="alert-error" role="alert">
      {{ flatten(EnrollmentStore.error) }}
    </p>
    <div v-if="EnrollmentStore.loading" class="py-8 text-center" role="status">
      جارٍ التحميل…
    </div>
    <div
      v-else-if="!EnrollmentStore.enrollments.length"
      class="rounded-lg border border-dashed p-8 text-center text-[var(--text-muted)]"
    >
      لا توجد اشتراكات.
    </div>
    <div v-else class="table-responsive">
      <table class="data-table min-w-full">
        <thead>
          <tr>
            <th>الطالب</th>
            <th>الخطة الأكاديمية</th>
            <th>الإصدار</th>
            <th>تاريخ التسجيل</th>
            <th>الصلاحية</th>
            <th>العناصر</th>
            <th />
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="item in EnrollmentStore.enrollments"
            :key="item.studentAcademicEnrollmentID"
          >
            <td>{{ item.studentName }}</td>
            <td>{{ item.academicPlanName }}</td>
            <td>{{ item.academicPlanVersion }}</td>
            <td>{{ item.enrollmentDate }}</td>
            <td>{{ item.validFrom }} — {{ item.validTo || "مفتوحة" }}</td>
            <td>{{ item.itemCount }}</td>
            <td>
              <button
                class="btn btn-sm btn-outline-primary"
                @click="showDetails(item.studentAcademicEnrollmentID)"
              >
                التفاصيل
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <PaginationComp
      :current-page="EnrollmentStore.page"
      :page-size="EnrollmentStore.pageSize"
      :total-count="EnrollmentStore.totalCount"
      :total-pages="EnrollmentStore.totalPages"
      @change-page="loadPage"
      @change-size="changePageSize"
    />

    <PopapComp v-if="showForm" @close="closePopup">
      <UpsertStudentAcademicEnrollmentComp
        :errors="EnrollmentStore.error"
        :student-results="studentResults"
        :student-loading="StudentStore.loading"
        :student-error="StudentStore.error"
        :plan-results="planResults"
        :plan-loading="PlanStore.loading"
        :plan-error="PlanStore.error"
        :saving="EnrollmentStore.loading"
        @student-search="searchStudents"
        @plan-search="searchPlans"
        @submit="submitEnrollment"
      />
    </PopapComp>
    <PopapComp v-if="showDetailsPopup" @close="closePopup">
      <UpsertStudentAcademicEnrollmentComp
        :enrollment="EnrollmentStore.selectedEnrollment"
        :saving="EnrollmentStore.loading"
      />
      <div class="mt-4 flex flex-wrap gap-2">
        <button
          v-if="AuthStore.hasPermission('Can_Create_StudentStage')"
          type="button"
          class="btn btn-outline-primary"
          @click="openStudentStages"
        >
          متابعة المراحل
        </button>
        <button
          v-if="AuthStore.hasPermission('Can_Create_DailyEvaluation')"
          type="button"
          class="btn btn-outline-primary"
          @click="openDailyEvaluations"
        >
          متابعة يومية
        </button>
      </div>
    </PopapComp>
    <MessageComp :is-error="resultError" :msg="resultMessage" />
  </main>
</template>

<script>
import { useAuthStore } from "@/store/Auth/authStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
import { useAcademicPlanStore } from "@/store/AcademicPlan/AcademicPlanStore";
import { useStudentAcademicEnrollmentStore } from "@/store/Enrollment/StudentAcademicEnrollmentStore";
import UpsertStudentAcademicEnrollmentComp from "@/components/Enrollment/StudentAcademicEnrollment/UpsertStudentAcademicEnrollmentComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";

export default {
  name: "StudentAcademicEnrollmentView",
  components: {
    UpsertStudentAcademicEnrollmentComp,
    PaginationComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      AuthStore: useAuthStore(),
      StudentStore: useStudentStore(),
      PlanStore: useAcademicPlanStore(),
      EnrollmentStore: useStudentAcademicEnrollmentStore(),
      studentResults: [],
      planResults: [],
      studentRequestID: 0,
      planRequestID: 0,
      listSearch: "",
      showForm: false,
      showDetailsPopup: false,
      resultMessage: null,
      resultError: false,
    };
  },
  methods: {
    flatten(value) {
      if (Array.isArray(value)) return value.join("، ");
      if (value && typeof value === "object")
        return Object.values(value).flat().join("، ");
      return value || "حدث خطأ";
    },
    safeText(value) {
      return String(value || "")
        .replace(/[,*|=<>^$()\\;]/g, " ")
        .trim();
    },
    async searchStudents({ filterType, value }) {
      const requestID = ++this.studentRequestID;
      const filter = `${filterType}=*${this.safeText(value)}`;
      const result = await this.StudentStore.fetchStudents({
        Filter: filter,
        Page: 1,
        PageSize: 20,
      });
      if (requestID === this.studentRequestID)
        this.studentResults = result.success ? this.StudentStore.students : [];
    },
    async searchPlans({ value }) {
      const requestID = ++this.planRequestID;
      const text = this.safeText(value);
      const filter = text ? `Status=2,Name=*${text}` : "Status=2";
      const result = await this.PlanStore.fetchAcademicPlans({
        Page: 1,
        PageSize: 20,
        Filter: filter,
      });
      if (requestID === this.planRequestID)
        this.planResults = result.success ? this.PlanStore.academicPlans : [];
    },
    async searchList() {
      const text = this.safeText(this.listSearch);
      const filter = text
        ? `StudentName=*${text}|AcademicPlanName=*${text}`
        : "";
      await this.EnrollmentStore.fetchEnrollments({ Page: 1, Filter: filter });
    },
    async resetSearch() {
      this.listSearch = "";
      await this.EnrollmentStore.fetchEnrollments({ Page: 1, Filter: "" });
    },
    async loadPage(page) {
      await this.EnrollmentStore.fetchEnrollments({ Page: page });
    },
    async changePageSize(size) {
      await this.EnrollmentStore.fetchEnrollments({ Page: 1, PageSize: size });
    },
    openCreate() {
      this.EnrollmentStore.error = null;
      this.showForm = true;
    },
    async submitEnrollment(payload) {
      const result = await this.EnrollmentStore.enroll(payload);
      if (result.success) {
        this.showForm = false;
        this.resultMessage = "تم تسجيل الاشتراك الأكاديمي بنجاح";
        this.resultError = false;
        await this.EnrollmentStore.fetchEnrollments({ Page: 1 });
      } else {
        this.resultMessage = result.errors;
        this.resultError = true;
      }
    },
    async showDetails(id) {
      const result = await this.EnrollmentStore.fetchForUpsert(id);
      if (result.success) this.showDetailsPopup = true;
      else {
        this.resultMessage = result.errors;
        this.resultError = true;
      }
    },
    openStudentStages() {
      const id =
        this.EnrollmentStore.selectedEnrollment?.studentAcademicEnrollmentID;
      if (!id) return;
      this.$router.push({
        name: "studentStage",
        query: { studentAcademicEnrollmentID: id },
      });
    },
    openDailyEvaluations() {
      const id =
        this.EnrollmentStore.selectedEnrollment?.studentAcademicEnrollmentID;
      if (!id) return;
      this.$router.push({
        name: "DailyEvaluation",
        query: { studentAcademicEnrollmentID: id },
      });
    },
    closePopup() {
      this.showForm = false;
      this.showDetailsPopup = false;
      this.EnrollmentStore.clearSelectedEnrollment();
    },
  },
  mounted() {
    this.EnrollmentStore.fetchEnrollments();
  },
};
</script>
