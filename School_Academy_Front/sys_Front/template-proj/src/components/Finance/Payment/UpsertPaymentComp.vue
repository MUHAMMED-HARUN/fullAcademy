<template>
  <section dir="rtl" class="space-y-4">
    <h2>تسجيل دفعة جديدة</h2>
    <form class="space-y-4" @submit.prevent="submit">
      <div v-if="!lockInvoice" class="space-y-2">
        <label class="block">الفاتورة المستحقة</label>
        <GenericSearchByFilter
          v-model="form.studentInvoiceID"
          v-model:name-value="invoiceLabel"
          :filter-types="[{ value: 'invoice', label: 'الطالب أو الخطة أو التعرفة' }]"
          :results="payableInvoices"
          id-field="studentInvoiceID"
          name-field="invoiceLabel"
          placeholder="ابحث عن فاتورة مستحقة"
          :loading="lookupLoading"
          :error="lookupError"
          @search="$emit('search-invoice', $event)"
          @select="selectInvoice"
          @query-change="clearInvoice"
          @clear="clearInvoice"
        />
      </div>
      <div v-else class="rounded-lg border p-3" aria-live="polite">
        <p><strong>الفاتورة:</strong> {{ invoiceLabel || initialPayment?.invoiceLabel }}</p>
        <p><strong>المتبقي قبل الدفع:</strong> {{ remainingAmount }}</p>
      </div>
      <ValidationError :messages="errors?.studentInvoiceID" />

      <div v-if="!lockInvoice && selectedInvoice" class="rounded-lg border p-3 text-sm" aria-live="polite">
        <p>{{ selectedInvoice.invoiceLabel }}</p>
        <p>المتبقي قبل الدفع: {{ remainingAmount }}</p>
      </div>

      <div class="form-group">
        <label for="payment-amount">المبلغ</label>
        <input id="payment-amount" v-model.number="form.amount" class="form-control" type="number" min="0.01" step="0.01" required />
        <ValidationError :messages="errors?.amount" />
        <p v-if="Number(form.amount) > Number(remainingAmount)" class="text-sm text-red-700" role="alert">المبلغ يتجاوز المتبقي على الفاتورة.</p>
      </div>
      <div class="form-group">
        <label for="payment-date">تاريخ الدفع</label>
        <input id="payment-date" v-model="form.paymentDate" class="form-control" type="datetime-local" required />
        <ValidationError :messages="errors?.paymentDate" />
      </div>
      <div class="form-group">
        <label for="payment-method">طريقة الدفع</label>
        <select id="payment-method" v-model.number="form.paymentMethod" class="form-select" required>
          <option v-for="option in PaymentMethodOptions" :key="option.value" :value="option.value">{{ option.label }}</option>
        </select>
        <ValidationError :messages="errors?.paymentMethod" />
      </div>
      <div class="form-group">
        <label for="payment-reference">رقم المرجع</label>
        <input id="payment-reference" v-model.trim="form.referenceNumber" class="form-control" />
        <ValidationError :messages="errors?.referenceNumber" />
      </div>
      <div class="form-group">
        <label for="payment-notes">ملاحظات</label>
        <input id="payment-notes" v-model.trim="form.notes" class="form-control" />
        <ValidationError :messages="errors?.notes" />
      </div>
      <button class="btn btn-primary" type="submit" :disabled="!form.studentInvoiceID || !remainingAmount || Number(form.amount) > Number(remainingAmount)">تسجيل الدفعة</button>
    </form>
  </section>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import { PaymentMethod, PaymentMethodOptions } from "@/Generic/FinanceEnums";

const props = defineProps({
  errors: { type: [Object, Array, String], default: () => ({}) },
  payableInvoices: { type: Array, default: () => [] },
  lookupLoading: { type: Boolean, default: false },
  lookupError: { type: [String, Array, Object], default: null },
  initialPayment: { type: Object, default: null },
  lockInvoice: { type: Boolean, default: false },
});
const emit = defineEmits(["on-submit", "search-invoice", "select-invoice"]);
const invoiceLabel = ref("");
const selectedInvoice = ref(null);
const form = reactive({
  studentInvoiceID: "",
  amount: 0,
  paymentDate: new Date().toISOString().slice(0, 16),
  paymentMethod: PaymentMethod.Cash,
  referenceNumber: "",
  notes: "",
});
const remainingAmount = computed(() => selectedInvoice.value?.remainingAmount ?? props.initialPayment?.remainingAmount ?? 0);

watch(() => props.initialPayment, (initial) => {
  Object.assign(form, {
    studentInvoiceID: initial?.studentInvoiceID || "",
    amount: initial?.amount ?? initial?.remainingAmount ?? 0,
    paymentDate: initial?.paymentDate || new Date().toISOString().slice(0, 16),
    paymentMethod: initial?.paymentMethod ?? PaymentMethod.Cash,
    referenceNumber: initial?.referenceNumber || "",
    notes: initial?.notes || "",
  });
  invoiceLabel.value = initial?.invoiceLabel || "";
  selectedInvoice.value = initial || null;
}, { immediate: true });

function selectInvoice(invoice) {
  selectedInvoice.value = invoice;
  form.studentInvoiceID = invoice?.studentInvoiceID || "";
  form.amount = invoice?.remainingAmount ?? 0;
  invoiceLabel.value = invoice?.invoiceLabel || "";
  emit("select-invoice", invoice);
}

function clearInvoice() {
  selectedInvoice.value = null;
  form.studentInvoiceID = "";
  form.amount = 0;
  invoiceLabel.value = "";
  emit("select-invoice", null);
}

function submit() {
  emit("on-submit", { ...form });
}
</script>
