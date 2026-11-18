<template>
  <section dir="rtl" class="space-y-4">
    <h2>عكس دفعة</h2>
    <dl class="grid gap-2 rounded-lg border p-3 sm:grid-cols-2">
      <div><dt>الفاتورة</dt><dd>{{ payment?.invoiceLabel }}</dd></div>
      <div><dt>المبلغ الذي سيُعكس</dt><dd>{{ payment?.amount }}</dd></div>
      <div><dt>تاريخ الدفع</dt><dd>{{ payment?.paymentDate }}</dd></div>
      <div><dt>المرجع</dt><dd>{{ payment?.referenceNumber || "—" }}</dd></div>
    </dl>
    <form class="space-y-4" @submit.prevent="$emit('submit', { paymentID: payment?.paymentID, reason })">
      <div class="form-group">
        <label for="reversal-reason">سبب العكس (مطلوب)</label>
        <textarea id="reversal-reason" v-model.trim="reason" class="form-control" maxlength="500" required rows="3" />
        <ValidationError :messages="errors?.reason" />
      </div>
      <p class="text-sm text-amber-700" role="status">سيُحفظ سجل عكس مستقل؛ لن تُحذف الدفعة الأصلية.</p>
      <button class="btn btn-danger" type="submit" :disabled="loading || !reason.trim() || payment?.isReversed">
        {{ loading ? "جاري العكس…" : "تأكيد عكس الدفعة" }}
      </button>
    </form>
  </section>
</template>

<script setup>
import { ref, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";

const props = defineProps({
  payment: { type: Object, default: null },
  errors: { type: [Object, Array, String], default: () => ({}) },
  loading: { type: Boolean, default: false },
});
defineEmits(["submit"]);
const reason = ref("");
watch(() => props.payment?.paymentID, () => { reason.value = ""; });
</script>
