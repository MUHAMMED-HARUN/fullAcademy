<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>فاتورة الطالب:</label>
        <select
          v-model="formData.studentInvoiceID"
          :disabled="lockInvoice || isEdit"
        >
          <option disabled value="">اختر الفاتورة</option>
          <option
            v-for="invoice in invoiceOptions"
            :key="invoice.studentInvoiceID"
            :value="invoice.studentInvoiceID"
          >
            {{ invoice.studentName }} - {{ invoice.feeStructureName }} (متبقي:
            {{ invoice.remainingAmount }})
          </option>
        </select>
        <ValidationError :messages="errors?.studentInvoiceID" />
      </div>

      <div class="form-group" v-if="selectedInvoice">
        <label>المتبقي على الفاتورة:</label>
        <input :value="selectedInvoice.remainingAmount" type="text" disabled />
      </div>

      <div class="form-group">
        <label>المبلغ:</label>
        <input
          v-model.number="formData.amount"
          type="number"
          min="0.01"
          step="0.01"
        />
        <ValidationError :messages="errors?.amount" />
      </div>

      <div class="form-group">
        <label>تاريخ الدفع:</label>
        <input v-model="formData.paymentDate" type="datetime-local" />
        <ValidationError :messages="errors?.paymentDate" />
      </div>

      <div class="form-group">
        <label>طريقة الدفع:</label>
        <select v-model.number="formData.paymentMethod">
          <option disabled value="">اختر الطريقة</option>
          <option
            v-for="opt in paymentMethodOptions"
            :key="opt.value"
            :value="opt.value"
          >
            {{ opt.label }}
          </option>
        </select>
        <ValidationError :messages="errors?.paymentMethod" />
      </div>

      <div class="form-group">
        <label>رقم المرجع:</label>
        <input v-model="formData.referenceNumber" type="text" />
        <ValidationError :messages="errors?.referenceNumber" />
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { usePaymentStore } from "@/store/Finance/PaymentStore";
import {
  InvoiceStatus,
  PaymentMethod,
  PaymentMethodOptions,
} from "@/Generic/FinanceEnums";

export default {
  name: "UpsertPaymentComp",
  components: {
    ValidationError,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    invoices: {
      type: Array,
      default: () => [],
    },
    initialPayment: {
      type: Object,
      default: null,
    },
    lockInvoice: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      PaymentStore: usePaymentStore(),
      paymentMethodOptions: PaymentMethodOptions,
      formData: {
        paymentID: "",
        studentInvoiceID: "",
        amount: 0,
        paymentDate: new Date().toISOString().substring(0, 16),
        paymentMethod: PaymentMethod.Cash,
        referenceNumber: "",
        notes: "",
      },
    };
  },
  emits: ["onSubmit"],
  computed: {
    isEdit() {
      return Boolean(this.formData.paymentID);
    },
    FormTitle() {
      return this.isEdit ? "تعديل دفعة" : "إضافة دفعة";
    },
    invoiceOptions() {
      if (this.isEdit || this.lockInvoice) return this.invoices || [];
      return (this.invoices || []).filter(
        (invoice) =>
          Number(invoice.remainingAmount) > 0 &&
          Number(invoice.status) !== InvoiceStatus.Cancelled
      );
    },
    selectedInvoice() {
      return (this.invoices || []).find(
        (invoice) => invoice.studentInvoiceID === this.formData.studentInvoiceID
      );
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    fillForm() {
      const entity =
        this.PaymentStore.selectedPayment || this.initialPayment || null;
      if (!entity) return;
      this.formData = {
        paymentID: entity.paymentID || "",
        studentInvoiceID: entity.studentInvoiceID || "",
        amount:
          entity.amount ||
          entity.remainingAmount ||
          this.selectedInvoice?.remainingAmount ||
          0,
        paymentDate:
          entity.paymentDate || new Date().toISOString().substring(0, 16),
        paymentMethod: entity.paymentMethod || PaymentMethod.Cash,
        referenceNumber: entity.referenceNumber || "",
        notes: entity.notes || "",
      };
    },
  },
  mounted() {
    this.fillForm();
  },
  watch: {
    "formData.studentInvoiceID"(val) {
      if (this.isEdit || !val) return;
      if (this.formData.amount) return;
      if (this.selectedInvoice) {
        this.formData.amount = this.selectedInvoice.remainingAmount;
      }
    },
  },
};
</script>
