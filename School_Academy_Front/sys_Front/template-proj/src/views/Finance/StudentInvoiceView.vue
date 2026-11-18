<template>
  <div class="finance-view-container" dir="rtl">
    <section
      v-if="StudentInvoiceStore.financeSummary"
      class="grid gap-3 rounded-xl border bg-[var(--bg-card)] p-4 sm:grid-cols-2 lg:grid-cols-4"
      aria-live="polite"
    >
      <p>
        <strong>{{ StudentInvoiceStore.financeSummary.studentName }}</strong>
      </p>
      <p>
        إجمالي الفواتير: {{ StudentInvoiceStore.financeSummary.invoiceCount }}
      </p>
      <p>المدفوع: {{ StudentInvoiceStore.financeSummary.totalPaidAmount }}</p>
      <p>
        المتبقي: {{ StudentInvoiceStore.financeSummary.totalRemainingAmount }}
      </p>
      <p>المتأخر: {{ StudentInvoiceStore.financeSummary.overdueAmount }}</p>
    </section>

    <GenericFilterComp
      :fields="filterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <p v-if="StudentInvoiceStore.loading" role="status">جاري تحميل الفواتير…</p>
    <p
      v-else-if="!StudentInvoiceStore.invoices.length"
      role="status"
      class="rounded-lg border border-dashed p-4 text-center"
    >
      لا توجد فواتير مطابقة.
    </p>
    <StudentInvoiceList
      v-else
      :items="StudentInvoiceStore.invoices"
      :selected-ids="selectedIds"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
    />
    <GenericComboActions
      :actions="visibleActions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />
    <PaginationComp
      :current-page="page"
      :page-size="pageSize"
      :total-count="StudentInvoiceStore.totalCount"
      :total-pages="StudentInvoiceStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onPopupClose">
      <UpsertStudentInvoiceComp
        :errors="StudentInvoiceStore.error"
        :enrollments="StudentAcademicEnrollmentStore.enrollments"
        :enrollment-loading="StudentAcademicEnrollmentStore.loading"
        :enrollment-error="StudentAcademicEnrollmentStore.error"
        :fee-structures="FeeStructureStore.feeStructures"
        :fee-loading="FeeStructureStore.loading"
        :fee-error="FeeStructureStore.error"
        :initial-invoice="editingInvoice"
        :invoice-locked="Boolean(editingInvoice?.hasPayments)"
        @search-enrollment="onSearchEnrollment"
        @select-enrollment="onSelectEnrollment"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onPopupClose">
      <StudentInvoiceCard :data-req="StudentInvoiceStore.selectedInvoice" />
    </PopapComp>
    <PopapComp v-if="showPayment" @close="onPopupClose">
      <UpsertPaymentComp
        :errors="PaymentStore.error"
        :initial-payment="initialPayment"
        :lock-invoice="true"
        @on-submit="OnSubmitPayment"
      />
    </PopapComp>

    <button
      v-if="AuthStore.hasPermission('Can_Create_StudentInvoice')"
      class="btn btn-primary self-start"
      @click="OnUpsert(null)"
    >
      إصدار فاتورة من تسجيل أكاديمي
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import StudentInvoiceList from "@/components/Finance/StudentInvoice/StudentInvoiceList.vue";
import StudentInvoiceCard from "@/components/Finance/StudentInvoice/StudentInvoiceCard.vue";
import UpsertStudentInvoiceComp from "@/components/Finance/StudentInvoice/UpsertStudentInvoiceComp.vue";
import UpsertPaymentComp from "@/components/Finance/Payment/UpsertPaymentComp.vue";
import { useStudentInvoiceStore } from "@/store/Finance/StudentInvoiceStore";
import { useFeeStructureStore } from "@/store/Finance/FeeStructureStore";
import { usePaymentStore } from "@/store/Finance/PaymentStore";
import { useStudentAcademicEnrollmentStore } from "@/store/Enrollment/StudentAcademicEnrollmentStore";
import { useAuthStore } from "@/store/Auth/authStore";
import { InvoiceStatus, InvoiceStatusOptions } from "@/Generic/FinanceEnums";

export default {
  name: "StudentInvoiceView",
  components: {
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
    GenericComboActions,
    StudentInvoiceList,
    StudentInvoiceCard,
    UpsertStudentInvoiceComp,
    UpsertPaymentComp,
  },
  data() {
    return {
      StudentInvoiceStore: useStudentInvoiceStore(),
      FeeStructureStore: useFeeStructureStore(),
      StudentAcademicEnrollmentStore: useStudentAcademicEnrollmentStore(),
      PaymentStore: usePaymentStore(),
      AuthStore: useAuthStore(),
      showUpsert: false,
      showInfo: false,
      showPayment: false,
      editingInvoice: null,
      initialPayment: null,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      page: 1,
      pageSize: 10,
      filterPayload: {},
      filterSchema: [
        {
          key: "studentName",
          label: "اسم الطالب",
          type: "text",
          operator: "=*",
        },
        {
          key: "feeStructureName",
          label: "التعرفة",
          type: "text",
          operator: "=*",
        },
        {
          key: "academicPlanName",
          label: "الخطة الأكاديمية",
          type: "text",
          operator: "=*",
        },
        {
          key: "status",
          label: "الحالة",
          type: "select",
          options: InvoiceStatusOptions,
        },
        { key: "issueDateFrom", label: "من تاريخ الإصدار", type: "date" },
        { key: "issueDateTo", label: "إلى تاريخ الإصدار", type: "date" },
      ],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        {
          key: "update",
          label: "تعديل",
          minSelection: 1,
          maxSelection: 1,
          condition: (invoice) => !invoice.hasPayments,
        },
        {
          key: "addPayment",
          label: "إضافة دفعة",
          minSelection: 1,
          maxSelection: 1,
          class: "btn-default",
          condition: (invoice) =>
            Number(invoice.remainingAmount) > 0 &&
            Number(invoice.status) !== InvoiceStatus.Cancelled &&
            Number(invoice.status) !== InvoiceStatus.Paid,
        },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class: "btn-danger",
          condition: (invoice) => !invoice.hasPayments,
        },
      ],
    };
  },
  computed: {
    visibleActions() {
      const permissions = {
        info: "Can_Read_StudentInvoice",
        update: "Can_Update_StudentInvoice",
        addPayment: "Can_Record_Payment",
        delete: "Can_Delete_StudentInvoice",
      };
      return this.actions.filter((action) =>
        this.AuthStore.hasPermission(permissions[action.key])
      );
    },
    selectedItems() {
      return this.StudentInvoiceStore.invoices.filter((invoice) =>
        this.selectedIds.includes(invoice.studentInvoiceID)
      );
    },
  },
  methods: {
    safeFilterValue(value) {
      return String(value ?? "")
        .replace(/[,*|=<>^$()\\;]/g, " ")
        .trim();
    },
    buildGridifyFilter(values = {}) {
      const terms = [];
      const textFields = {
        studentName: "StudentName",
        feeStructureName: "FeeStructureName",
        academicPlanName: "AcademicPlanName",
      };
      for (const [key, field] of Object.entries(textFields)) {
        const value = this.safeFilterValue(values[key]);
        if (value) terms.push(`${field}=*${value}`);
      }
      if (values.status !== "" && values.status != null)
        terms.push(`Status=${values.status}`);
      const from = this.safeFilterValue(values.issueDateFrom);
      const to = this.safeFilterValue(values.issueDateTo);
      if (from) terms.push(`IssueDate>=${from}`);
      if (to) terms.push(`IssueDate<=${to}`);
      return terms.join(",");
    },
    buildGridifyOrderBy(value) {
      if (!value) return "";
      const [field, direction = "asc"] = value.trim().split(/\s+/);
      const allowed = {
        studentName: "StudentName",
        feeStructureName: "FeeStructureName",
        academicPlanName: "AcademicPlanName",
        issueDate: "IssueDate",
        dueDate: "DueDate",
        status: "Status",
      };
      const mapped = allowed[field];
      return mapped
        ? `${mapped} ${direction.toLowerCase() === "desc" ? "desc" : "asc"}`
        : "";
    },
    async loadPage() {
      const result = await this.StudentInvoiceStore.fetchInvoices({
        Page: this.page,
        PageSize: this.pageSize,
        Filter: this.buildGridifyFilter(this.filterPayload.rawValues || {}),
        OrderBy: this.buildGridifyOrderBy(this.filterPayload.orderBy),
      });
      if (!result.success) {
        this.resultMessage = result.errors;
        this.resultType = true;
      }
    },
    async onPageChange(value) {
      this.page = value;
      await this.loadPage();
    },
    async onPageSizeChange(value) {
      this.pageSize = value;
      this.page = 1;
      await this.loadPage();
    },
    async onFilterSearch(value) {
      this.filterPayload = value || {};
      this.page = 1;
      await this.loadPage();
    },
    async onFilterReset() {
      this.filterPayload = {};
      this.page = 1;
      await this.loadPage();
    },
    onToggleSelectAll(checked) {
      this.selectedIds = checked
        ? this.StudentInvoiceStore.invoices.map(
            (invoice) => invoice.studentInvoiceID
          )
        : [];
    },
    onToggleSelectRow(invoice) {
      const index = this.selectedIds.indexOf(invoice.studentInvoiceID);
      if (index >= 0) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(invoice.studentInvoiceID);
    },
    async handleComboAction(key, invoices) {
      const invoice = invoices[0];
      if (key === "info") {
        this.StudentInvoiceStore.selectedInvoice = invoice;
        this.showInfo = true;
      }
      if (key === "update") await this.OnUpsert(invoice);
      if (key === "addPayment") this.onAddPayment(invoice);
      if (key === "delete") await this.OnDeleteMultiple(invoices);
    },
    async OnUpsert(invoice) {
      this.FeeStructureStore.feeStructures = [];
      if (invoice) {
        const result = await this.StudentInvoiceStore.fetchInvoiceForUpsert(
          invoice.studentInvoiceID
        );
        if (!result.success) {
          this.resultMessage = result.errors;
          this.resultType = true;
          return;
        }
        this.editingInvoice = result.data;
      } else {
        this.StudentInvoiceStore.clearSelectedInvoice();
        this.editingInvoice = null;
        this.StudentAcademicEnrollmentStore.clearSelectedEnrollment();
      }
      this.showUpsert = true;
    },
    async onSearchEnrollment({ value }) {
      const textFilter = this.StudentAcademicEnrollmentStore.buildLookupFilter(
        "student",
        value
      );
      await this.StudentAcademicEnrollmentStore.fetchEnrollments({
        Page: 1,
        PageSize: 10,
        Filter: textFilter,
        ActiveOnly: true,
      });
    },
    async onSelectEnrollment(enrollment) {
      if (!enrollment) {
        this.StudentAcademicEnrollmentStore.clearSelectedEnrollment();
        this.FeeStructureStore.feeStructures = [];
        this.StudentInvoiceStore.financeSummary = null;
        return;
      }
      this.StudentAcademicEnrollmentStore.selectedEnrollment = enrollment;
      await this.FeeStructureStore.fetchByAcademicPlan(
        enrollment.academicPlanID
      );
      const summaryResult = await this.StudentInvoiceStore.fetchFinanceSummary(
        enrollment.studentID
      );
      if (!summaryResult.success) {
        this.resultMessage = summaryResult.errors;
        this.resultType = true;
      }
    },
    async OnSubmitUpsert(data) {
      const result = data.studentInvoiceID
        ? await this.StudentInvoiceStore.updateInvoice(data)
        : await this.StudentInvoiceStore.addInvoice(data);
      if (result.success) {
        this.showUpsert = false;
        this.resultMessage = data.studentInvoiceID
          ? "تم تحديث الفاتورة"
          : "تم إصدار الفاتورة";
        this.resultType = false;
        await this.loadPage();
        if (this.StudentAcademicEnrollmentStore.selectedEnrollment?.studentID)
          await this.StudentInvoiceStore.fetchFinanceSummary(
            this.StudentAcademicEnrollmentStore.selectedEnrollment.studentID
          );
      } else {
        this.resultMessage = result.errors;
        this.resultType = true;
      }
    },
    onAddPayment(invoice) {
      this.PaymentStore.clearSelectedPayment();
      this.initialPayment = {
        studentInvoiceID: invoice.studentInvoiceID,
        invoiceLabel: `${invoice.studentName} — ${invoice.academicPlanName} / ${invoice.feeStructureName}`,
        remainingAmount: invoice.remainingAmount,
        amount: invoice.remainingAmount,
        paymentDate: new Date().toISOString().slice(0, 16),
      };
      this.showPayment = true;
    },
    async OnSubmitPayment(data) {
      const result = await this.PaymentStore.addPayment(data);
      if (result.success) {
        this.showPayment = false;
        this.resultMessage = "تم تسجيل الدفعة";
        this.resultType = false;
        const invoiceResult = await this.StudentInvoiceStore.fetchInvoiceForUpsert(data.studentInvoiceID);
        if (invoiceResult.success) {
          await this.StudentInvoiceStore.fetchFinanceSummary(invoiceResult.data.studentID);
        }
        await this.loadPage();
      } else {
        this.resultMessage = result.errors;
        this.resultType = true;
      }
    },
    async OnDeleteMultiple(invoices) {
      if (!confirm(`هل أنت متأكد من حذف ${invoices.length} فواتير؟`)) return;
      for (const invoice of invoices) {
        const result = await this.StudentInvoiceStore.DeleteInvoice(
          invoice.studentInvoiceID
        );
        if (!result.success) {
          this.resultMessage = result.errors;
          this.resultType = true;
          return;
        }
      }
      this.selectedIds = [];
      this.resultMessage = "تم حذف الفواتير";
      this.resultType = false;
      await this.loadPage();
    },
    onPopupClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.showPayment = false;
      this.editingInvoice = null;
      this.initialPayment = null;
      this.StudentInvoiceStore.clearSelectedInvoice();
      this.StudentAcademicEnrollmentStore.clearSelectedEnrollment();
      this.PaymentStore.clearSelectedPayment();
    },
  },
  async mounted() {
    await this.loadPage();
    if (
      !this.AuthStore.isAdmin &&
      this.AuthStore.userData?.entityType?.toLowerCase() === "student"
    )
      await this.StudentInvoiceStore.fetchFinanceSummary();
  },
};
</script>

<style scoped>
.finance-view-container {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
</style>
