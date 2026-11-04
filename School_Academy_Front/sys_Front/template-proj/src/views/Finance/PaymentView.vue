<template>
  <div class="finance-view-container">
    <GenericFilterComp
      :fields="filterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <PaymentList
      :items="pagedItems"
      :selected-ids="selectedIds"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
    />

    <GenericComboActions
      :actions="actions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <PaginationComp
      :current-page="page"
      :page-size="pageSize"
      :total-count="filteredCount"
      :total-pages="totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onPopupClose">
      <UpsertPaymentComp
        :errors="PaymentStore.error"
        :invoices="StudentInvoiceStore.invoices"
        :initial-payment="initialPayment"
        :lock-invoice="lockInvoice"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onPopupClose">
      <PaymentCard :-data-req="selectedPaymentView" />
    </PopapComp>

    <button class="btn-add" @click="OnUpsert(null)">إضافة دفعة</button>
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
import { usePaymentStore } from "@/store/Finance/PaymentStore";
import { useStudentInvoiceStore } from "@/store/Finance/StudentInvoiceStore";
import { applyClientQuery } from "@/Generic/applyClientQuery";
import { PaymentMethodOptions } from "@/Generic/FinanceEnums";
import TResult from "@/Generic/TResult";

export default {
  name: "PaymentView",
  components: {
    GenericFilterComp,
    PaginationComp,
    GenericComboActions,
    PaymentList,
    PaymentCard,
    UpsertPaymentComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      PaymentStore: usePaymentStore(),
      StudentInvoiceStore: useStudentInvoiceStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      page: 1,
      pageSize: 10,
      filterPayload: {},
      lockInvoice: false,
      initialPayment: null,
      filterSchema: [
        {
          key: "invoiceLabel",
          label: "الطالب / الخطة",
          type: "text",
          operator: "=*",
        },
        {
          key: "paymentMethod",
          label: "طريقة الدفع",
          type: "select",
          options: PaymentMethodOptions,
        },
        {
          key: "paymentDateFrom",
          label: "من تاريخ الدفع",
          type: "date",
        },
        {
          key: "paymentDateTo",
          label: "إلى تاريخ الدفع",
          type: "date",
        },
        {
          key: "referenceNumber",
          label: "المرجع",
          type: "text",
          operator: "=*",
        },
      ],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class: "btn-danger",
        },
      ],
    };
  },
  computed: {
    decoratedPayments() {
      return this.PaymentStore.payments.map((item) => ({
        ...item,
        invoiceLabel: this.getInvoiceLabel(item.studentInvoiceID),
      }));
    },
    queryResult() {
      return applyClientQuery(
        this.decoratedPayments,
        {
          rawValues: this.filterPayload.rawValues,
          orderBy: this.filterPayload.orderBy,
          page: this.page,
          pageSize: this.pageSize,
        },
        {
          containsFields: ["invoiceLabel", "referenceNumber"],
          equalsFields: ["paymentMethod", "studentInvoiceID"],
          gteFields: { paymentDateFrom: "paymentDate" },
          lteFields: { paymentDateTo: "paymentDate" },
        }
      );
    },
    pagedItems() {
      return this.queryResult.items;
    },
    filteredCount() {
      return this.queryResult.count;
    },
    totalPages() {
      return Math.ceil(this.filteredCount / this.pageSize) || 1;
    },
    selectedItems() {
      return this.decoratedPayments.filter((item) =>
        this.selectedIds.includes(item.paymentID)
      );
    },
    selectedPaymentView() {
      return this.selectedItems[0] || this.PaymentStore.selectedPayment;
    },
  },
  methods: {
    getInvoiceLabel(studentInvoiceID) {
      const invoice = this.StudentInvoiceStore.invoices.find(
        (item) => item.studentInvoiceID === studentInvoiceID
      );
      if (!invoice) return studentInvoiceID;
      return `${invoice.studentName} - ${invoice.feeStructureName}`;
    },
    async onPageChange(newPage) {
      this.page = newPage;
    },
    async onPageSizeChange(newSize) {
      this.pageSize = newSize;
      this.page = 1;
    },
    onFilterSearch(payload) {
      this.filterPayload = payload;
      this.page = 1;
    },
    onFilterReset() {
      this.filterPayload = {};
      if (this.$route.query.studentInvoiceID) {
        this.$router.push({ query: {} });
      }
      this.page = 1;
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.pagedItems.map((item) => item.paymentID)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.paymentID);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.paymentID);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          this.onShowInfo(singleItem);
          break;
        case "update":
          this.OnUpsert(singleItem);
          break;
        case "delete":
          await this.OnDeleteMultiple(selectedItems);
          break;
      }
    },
    OnUpsert(item) {
      this.lockInvoice = false;
      this.initialPayment = null;
      if (item) {
        this.PaymentStore.selectedPayment = item;
      } else {
        this.PaymentStore.clearSelectedPayment();
        const invoiceID = this.$route.query.studentInvoiceID;
        if (invoiceID) {
          const invoice = this.StudentInvoiceStore.invoices.find(
            (inv) => inv.studentInvoiceID === invoiceID
          );
          this.initialPayment = {
            studentInvoiceID: invoiceID,
            amount: invoice?.remainingAmount || 0,
          };
          this.lockInvoice = true;
        }
      }
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      let resVal = new TResult();
      if (e.paymentID) {
        resVal = await this.PaymentStore.updatePayment(e);
      } else {
        resVal = await this.PaymentStore.addPayment(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ بنجاح";
        this.resultType = false;
        this.showUpsert = false;
        await this.StudentInvoiceStore.fetchInvoices();
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    onShowInfo(item) {
      this.PaymentStore.selectedPayment = item;
      this.showInfo = true;
    },
    async OnDeleteMultiple(items) {
      if (!confirm(`هل أنت متأكد من حذف ${items.length} سجلات المحددة؟`))
        return;
      for (const item of items) {
        await this.PaymentStore.DeletePayment(item.paymentID);
      }
      this.selectedIds = [];
      this.resultMessage = "تم الحذف بنجاح";
      this.resultType = false;
      await this.StudentInvoiceStore.fetchInvoices();
    },
    onPopupClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.lockInvoice = false;
      this.initialPayment = null;
      this.PaymentStore.clearSelectedPayment();
    },
    applyRouteFilter() {
      const studentInvoiceID = this.$route.query.studentInvoiceID;
      if (studentInvoiceID) {
        this.filterPayload = {
          rawValues: { studentInvoiceID },
        };
      }
      if (this.$route.query.add === "1" && studentInvoiceID) {
        this.OnUpsert(null);
      }
    },
  },
  async mounted() {
    await Promise.all([
      this.PaymentStore.fetchPayments(),
      this.StudentInvoiceStore.fetchInvoices(),
    ]);
    this.applyRouteFilter();
  },
  watch: {
    "$route.query.studentInvoiceID"() {
      this.applyRouteFilter();
    },
  },
};
</script>

<style scoped>
.finance-view-container {
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
