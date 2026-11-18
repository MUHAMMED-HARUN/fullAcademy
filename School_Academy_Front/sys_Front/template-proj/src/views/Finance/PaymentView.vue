<template>
  <div class="finance-view-container" dir="rtl">
    <GenericFilterComp :fields="filterSchema" @search="onFilterSearch" @reset="onFilterReset" />
    <p v-if="PaymentStore.loading" role="status">جاري تحميل الدفعات…</p>
    <p v-else-if="!PaymentStore.payments.length" role="status" class="rounded-lg border border-dashed p-4 text-center">لا توجد دفعات مطابقة.</p>
    <PaymentList
      v-else
      :items="PaymentStore.payments"
      :selected-ids="selectedIds"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
    />
    <GenericComboActions :actions="visibleActions" :selected-items="selectedItems" @action-click="handleComboAction" />
    <PaginationComp
      :current-page="page"
      :page-size="pageSize"
      :total-count="PaymentStore.totalCount"
      :total-pages="PaymentStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onPopupClose">
      <UpsertPaymentComp
        :errors="PaymentStore.error"
        :payable-invoices="StudentInvoiceStore.payableInvoices"
        :lookup-loading="StudentInvoiceStore.payableInvoicesLoading"
        :lookup-error="StudentInvoiceStore.payableInvoicesError"
        :initial-payment="initialPayment"
        :lock-invoice="Boolean(initialPayment?.studentInvoiceID)"
        @search-invoice="onSearchInvoice"
        @select-invoice="onSelectInvoice"
        @on-submit="OnSubmitPayment"
      />
    </PopapComp>
    <PopapComp v-if="showReverse" @close="onPopupClose">
      <ReversePaymentComp
        :payment="reversingPayment"
        :errors="PaymentStore.error"
        :loading="PaymentStore.loading"
        @submit="OnSubmitReverse"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onPopupClose">
      <PaymentCard :data-req="selectedPaymentView" />
    </PopapComp>

    <button v-if="AuthStore.hasPermission('Can_Record_Payment')" class="btn btn-primary self-start" @click="OnUpsert(null)">إضافة دفعة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import PaymentList from "@/components/Finance/Payment/PaymentList.vue";
import PaymentCard from "@/components/Finance/Payment/PaymentCard.vue";
import UpsertPaymentComp from "@/components/Finance/Payment/UpsertPaymentComp.vue";
import ReversePaymentComp from "@/components/Finance/Payment/ReversePaymentComp.vue";
import { usePaymentStore } from "@/store/Finance/PaymentStore";
import { useStudentInvoiceStore } from "@/store/Finance/StudentInvoiceStore";
import { useAuthStore } from "@/store/Auth/authStore";
import { PaymentMethodOptions } from "@/Generic/FinanceEnums";

export default {
  name: "PaymentView",
  components: {
    PopapComp, MessageComp, GenericFilterComp, PaginationComp, GenericComboActions,
    PaymentList, PaymentCard, UpsertPaymentComp, ReversePaymentComp,
  },
  data() {
    return {
      PaymentStore: usePaymentStore(),
      StudentInvoiceStore: useStudentInvoiceStore(),
      AuthStore: useAuthStore(),
      showUpsert: false,
      showReverse: false,
      showInfo: false,
      selectedIds: [],
      page: 1,
      pageSize: 10,
      filterPayload: {},
      initialPayment: null,
      selectedInvoice: null,
      reversingPayment: null,
      resultMessage: null,
      resultType: false,
      filterSchema: [
        { key: "invoiceLabel", label: "الطالب / الخطة / التعرفة", type: "text", operator: "=*" },
        { key: "paymentMethod", label: "طريقة الدفع", type: "select", options: PaymentMethodOptions },
        { key: "paymentDateFrom", label: "من تاريخ الدفع", type: "date" },
        { key: "paymentDateTo", label: "إلى تاريخ الدفع", type: "date" },
        { key: "referenceNumber", label: "المرجع", type: "text", operator: "=*" },
        { key: "isReversed", label: "حالة الدفعة", type: "select", options: [
          { value: false, label: "سارية" }, { value: true, label: "معكوسة" },
        ] },
      ],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "reverse", label: "عكس الدفعة", minSelection: 1, maxSelection: 1, class: "btn-danger", condition: (payment) => !payment.isReversed },
      ],
    };
  },
  computed: {
    visibleActions() {
      const permissions = { info: "Can_Read_Payment", reverse: "Can_Reverse_Payment" };
      return this.actions.filter((action) => this.AuthStore.hasPermission(permissions[action.key]));
    },
    selectedItems() {
      return this.PaymentStore.payments.filter((payment) => this.selectedIds.includes(payment.paymentID));
    },
    selectedPaymentView() {
      return this.selectedItems[0] || this.PaymentStore.selectedPayment;
    },
  },
  methods: {
    safeText(value) { return String(value ?? "").replace(/[,*|=<>^$()\\;]/g, " ").trim(); },
    buildFilter(values = {}) {
      const terms = [];
      const label = this.safeText(values.invoiceLabel);
      const reference = this.safeText(values.referenceNumber);
      if (label) terms.push(`InvoiceLabel=*${label}`);
      if (reference) terms.push(`ReferenceNumber=*${reference}`);
      if (values.paymentMethod !== "" && values.paymentMethod != null) terms.push(`PaymentMethod=${values.paymentMethod}`);
      if (values.isReversed !== "" && values.isReversed != null) terms.push(`IsReversed=${values.isReversed}`);
      const from = this.safeText(values.paymentDateFrom);
      const to = this.safeText(values.paymentDateTo);
      if (from) terms.push(`PaymentDate>=${from}T00:00:00`);
      if (to) terms.push(`PaymentDate<=${to}T23:59:59`);
      return terms.join(",");
    },
    buildOrderBy(value) {
      if (!value) return "";
      const [field, direction = "asc"] = value.trim().split(/\s+/);
      const allowed = { invoiceLabel: "InvoiceLabel", paymentMethod: "PaymentMethod", paymentDate: "PaymentDate", referenceNumber: "ReferenceNumber", amount: "Amount" };
      const mapped = allowed[field];
      return mapped ? `${mapped} ${direction.toLowerCase() === "desc" ? "desc" : "asc"}` : "";
    },
    async loadPage() {
      const result = await this.PaymentStore.fetchPayments({
        Page: this.page,
        PageSize: this.pageSize,
        Filter: this.buildFilter(this.filterPayload.rawValues || {}),
        OrderBy: this.buildOrderBy(this.filterPayload.orderBy),
      });
      if (!result.success) { this.resultMessage = result.errors; this.resultType = true; }
    },
    async onPageChange(value) { this.page = value; await this.loadPage(); },
    async onPageSizeChange(value) { this.pageSize = value; this.page = 1; await this.loadPage(); },
    async onFilterSearch(payload) { this.filterPayload = payload || {}; this.page = 1; await this.loadPage(); },
    async onFilterReset() { this.filterPayload = {}; this.page = 1; await this.loadPage(); },
    onToggleSelectAll(checked) { this.selectedIds = checked ? this.PaymentStore.payments.map((payment) => payment.paymentID) : []; },
    onToggleSelectRow(payment) {
      const index = this.selectedIds.indexOf(payment.paymentID);
      if (index >= 0) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(payment.paymentID);
    },
    async handleComboAction(key, items) {
      const payment = items[0];
      if (key === "info") { this.PaymentStore.selectedPayment = payment; this.showInfo = true; }
      if (key === "reverse") { this.reversingPayment = payment; this.PaymentStore.selectedPayment = payment; this.showReverse = true; }
    },
    async OnUpsert() {
      this.selectedInvoice = null;
      this.initialPayment = null;
      this.PaymentStore.clearSelectedPayment();
      await this.StudentInvoiceStore.fetchPayableInvoices({ Page: 1, PageSize: 10 });
      this.showUpsert = true;
    },
    async onSearchInvoice({ value }) {
      const text = this.safeText(value);
      const filter = text
        ? `StudentName=*${text}|AcademicPlanName=*${text}|FeeStructureName=*${text}`
        : "";
      await this.StudentInvoiceStore.fetchPayableInvoices({ Page: 1, PageSize: 10, Filter: filter });
    },
    onSelectInvoice(invoice) { this.selectedInvoice = invoice || null; },
    async refreshInvoiceSummary(invoiceID) {
      if (!invoiceID) return;
      const invoiceResult = await this.StudentInvoiceStore.fetchInvoiceForUpsert(invoiceID);
      const invoice = invoiceResult.success ? invoiceResult.data : this.selectedInvoice;
      if (invoice?.studentID)
        await this.StudentInvoiceStore.fetchFinanceSummary(invoice.studentID);
    },
    async OnSubmitPayment(data) {
      const result = await this.PaymentStore.addPayment(data);
      if (!result.success) { this.resultMessage = result.errors; this.resultType = true; return; }
      this.showUpsert = false;
      this.resultMessage = "تم تسجيل الدفعة";
      this.resultType = false;
      await this.refreshInvoiceSummary(data.studentInvoiceID);
      await this.loadPage();
    },
    async OnSubmitReverse(data) {
      const invoiceID = this.reversingPayment?.studentInvoiceID;
      const result = await this.PaymentStore.reversePayment(data);
      if (!result.success) { this.resultMessage = result.errors; this.resultType = true; return; }
      this.showReverse = false;
      this.resultMessage = "تم عكس الدفعة وحفظ سبب العكس";
      this.resultType = false;
      await this.refreshInvoiceSummary(invoiceID);
      await this.loadPage();
    },
    async applyRouteContext() {
      const invoiceID = this.$route.query.studentInvoiceID;
      if (!invoiceID) return;
      const result = await this.StudentInvoiceStore.fetchInvoiceForUpsert(invoiceID);
      if (!result.success) { this.resultMessage = result.errors; this.resultType = true; return; }
      const invoice = result.data;
      if (Number(invoice.remainingAmount) <= 0 || Number(invoice.status) === 4) {
        this.resultMessage = "لا يمكن الدفع على فاتورة غير مستحقة";
        this.resultType = true;
        return;
      }
      this.selectedInvoice = invoice;
      this.initialPayment = {
        studentInvoiceID: invoice.studentInvoiceID,
        invoiceLabel: `${invoice.studentName} — ${invoice.academicPlanName} / ${invoice.feeStructureName}`,
        remainingAmount: invoice.remainingAmount,
        amount: invoice.remainingAmount,
        paymentDate: new Date().toISOString().slice(0, 16),
      };
      this.showUpsert = true;
    },
    onPopupClose() {
      this.showUpsert = false;
      this.showReverse = false;
      this.showInfo = false;
      this.initialPayment = null;
      this.selectedInvoice = null;
      this.reversingPayment = null;
      this.PaymentStore.clearSelectedPayment();
    },
  },
  async mounted() {
    await this.loadPage();
    await this.applyRouteContext();
  },
  watch: {
    "$route.query.studentInvoiceID"() { this.applyRouteContext(); },
  },
};
</script>

<style scoped>
.finance-view-container { display: flex; flex-direction: column; gap: 1rem; }
</style>
