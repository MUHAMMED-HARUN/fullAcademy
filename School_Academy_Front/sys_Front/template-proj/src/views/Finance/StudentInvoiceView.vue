<template>
  <div class="finance-view-container">
    <GenericFilterComp
      :fields="filterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <StudentInvoiceList
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
      <UpsertStudentInvoiceComp
        :errors="StudentInvoiceStore.error"
        :student-data="StudentStore.selectedStudent"
        :fee-structures="FeeStructureStore.feeStructures"
        :initial-invoice="initialInvoice"
        :lock-fee-structure="lockFeeStructure"
        @on-submit="OnSubmitUpsert"
        @on-search-student="onSearchStudent"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onPopupClose">
      <StudentInvoiceCard :-data-req="StudentInvoiceStore.selectedInvoice" />
    </PopapComp>

    <PopapComp v-if="showPayment" @close="onPopupClose">
      <UpsertPaymentComp
        :errors="PaymentStore.error"
        :invoices="StudentInvoiceStore.invoices"
        :initial-payment="initialPayment"
        :lock-invoice="true"
        @on-submit="OnSubmitPayment"
      />
    </PopapComp>

    <button class="btn-add" @click="OnUpsert(null)">تسجيل طالب في خطة</button>
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
import { useStudentStore } from "@/store/Person/useStudentStore";
import { applyClientQuery } from "@/Generic/applyClientQuery";
import { InvoiceStatus, InvoiceStatusOptions } from "@/Generic/FinanceEnums";
import TResult from "@/Generic/TResult";

export default {
  name: "StudentInvoiceView",
  components: {
    GenericFilterComp,
    PaginationComp,
    GenericComboActions,
    StudentInvoiceList,
    StudentInvoiceCard,
    UpsertStudentInvoiceComp,
    UpsertPaymentComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      StudentInvoiceStore: useStudentInvoiceStore(),
      FeeStructureStore: useFeeStructureStore(),
      PaymentStore: usePaymentStore(),
      StudentStore: useStudentStore(),
      showUpsert: false,
      showInfo: false,
      showPayment: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      page: 1,
      pageSize: 10,
      filterPayload: {},
      lockFeeStructure: false,
      initialInvoice: null,
      initialPayment: null,
      filterSchema: [
        {
          key: "studentName",
          label: "اسم الطالب",
          type: "text",
          operator: "=*",
        },
        {
          key: "feeStructureName",
          label: "اسم الخطة",
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
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "addPayment",
          label: "إضافة دفعة",
          minSelection: 1,
          maxSelection: 1,
          class: "btn-default",
          condition: (item) =>
            Number(item.remainingAmount) > 0 &&
            Number(item.status) !== InvoiceStatus.Cancelled &&
            Number(item.status) !== InvoiceStatus.Paid,
        },
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
    queryResult() {
      return applyClientQuery(
        this.StudentInvoiceStore.invoices,
        {
          rawValues: this.filterPayload.rawValues,
          orderBy: this.filterPayload.orderBy,
          page: this.page,
          pageSize: this.pageSize,
        },
        {
          containsFields: ["studentName", "feeStructureName"],
          equalsFields: ["status", "feeStructureID", "studentID"],
          gteFields: { issueDateFrom: "issueDate" },
          lteFields: { issueDateTo: "issueDate" },
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
      return this.StudentInvoiceStore.invoices.filter((item) =>
        this.selectedIds.includes(item.studentInvoiceID)
      );
    },
  },
  methods: {
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
      if (this.$route.query.feeStructureID || this.$route.query.enroll) {
        this.$router.push({ query: {} });
      }
      this.page = 1;
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.pagedItems.map((item) => item.studentInvoiceID)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.studentInvoiceID);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.studentInvoiceID);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          this.onShowInfo(singleItem);
          break;
        case "update":
          await this.OnUpsert(singleItem);
          break;
        case "addPayment":
          this.onAddPayment(singleItem);
          break;
        case "delete":
          await this.OnDeleteMultiple(selectedItems);
          break;
      }
    },
    async OnUpsert(item) {
      this.lockFeeStructure = false;
      this.initialInvoice = null;
      if (item) {
        this.StudentInvoiceStore.selectedInvoice = item;
        if (item.studentID) {
          await this.onSearchStudent(1, item.studentID);
        }
      } else {
        this.StudentInvoiceStore.clearSelectedInvoice();
        this.StudentStore.selectedStudent = null;
        const feeStructureID = this.$route.query.feeStructureID;
        if (feeStructureID) {
          const plan = this.FeeStructureStore.feeStructures.find(
            (p) => p.feeStructureID === feeStructureID
          );
          this.initialInvoice = {
            feeStructureID,
            originalAmount: plan?.amount || 0,
            discountAmount: 0,
            issueDate: new Date().toISOString().substring(0, 10),
            dueDate: new Date().toISOString().substring(0, 10),
          };
          this.lockFeeStructure = true;
        }
      }
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      let resVal = new TResult();
      if (e.studentInvoiceID) {
        resVal = await this.StudentInvoiceStore.updateInvoice(e);
      } else {
        resVal = await this.StudentInvoiceStore.addInvoice(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ بنجاح";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    onShowInfo(item) {
      this.StudentInvoiceStore.selectedInvoice = item;
      this.showInfo = true;
    },
    onAddPayment(item) {
      this.PaymentStore.clearSelectedPayment();
      this.initialPayment = {
        studentInvoiceID: item.studentInvoiceID,
        amount: item.remainingAmount,
        paymentDate: new Date().toISOString().substring(0, 16),
      };
      this.showPayment = true;
    },
    async OnSubmitPayment(e) {
      const resVal = await this.PaymentStore.addPayment(e);
      if (resVal.success) {
        this.resultMessage = "تم إضافة الدفعة بنجاح";
        this.resultType = false;
        this.showPayment = false;
        await this.StudentInvoiceStore.fetchInvoices();
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async OnDeleteMultiple(items) {
      if (!confirm(`هل أنت متأكد من حذف ${items.length} سجلات المحددة؟`))
        return;
      for (const item of items) {
        await this.StudentInvoiceStore.DeleteInvoice(item.studentInvoiceID);
      }
      this.selectedIds = [];
      this.resultMessage = "تم الحذف بنجاح";
      this.resultType = false;
    },
    async onSearchStudent(type, val) {
      await this.StudentStore.fetchStudentBy({
        Param: val,
        ParamType: type,
      });
    },
    onPopupClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.showPayment = false;
      this.lockFeeStructure = false;
      this.initialInvoice = null;
      this.initialPayment = null;
      this.StudentInvoiceStore.clearSelectedInvoice();
      this.PaymentStore.clearSelectedPayment();
      this.StudentStore.selectedStudent = null;
    },
    applyRouteQuery() {
      const feeStructureID = this.$route.query.feeStructureID;
      if (feeStructureID) {
        this.filterPayload = {
          rawValues: { feeStructureID },
        };
      }
      if (this.$route.query.enroll === "1") {
        this.OnUpsert(null);
      }
    },
  },
  async mounted() {
    await Promise.all([
      this.StudentInvoiceStore.fetchInvoices(),
      this.FeeStructureStore.fetchFeeStructures(),
    ]);
    this.applyRouteQuery();
  },
  watch: {
    "$route.query.feeStructureID"() {
      this.applyRouteQuery();
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
