<template>
  <section dir="rtl" class="space-y-4">
    <h2>{{ isEdit ? "تعديل الفاتورة" : "إصدار فاتورة من تسجيل أكاديمي" }}</h2>
    <form class="space-y-4" @submit.prevent="submit">
      <template v-if="!isEdit">
        <div class="space-y-2">
          <label class="block">التسجيل الأكاديمي</label>
          <GenericSearchByFilter
            v-model="form.studentAcademicEnrollmentID"
            v-model:name-value="enrollmentLabel"
            :filter-types="[{ value: 'student', label: 'اسم الطالب' }]"
            :results="enrollments"
            id-field="studentAcademicEnrollmentID"
            name-field="displayName"
            placeholder="ابحث عن تسجيل أكاديمي فعال"
            :loading="enrollmentLoading"
            :error="enrollmentError"
            @search="$emit('search-enrollment', $event)"
            @select="selectEnrollment"
            @query-change="clearEnrollment"
            @clear="clearEnrollment"
          />
          <ValidationError :messages="errors?.studentAcademicEnrollmentID" />
        </div>

        <div
          v-if="selectedEnrollment"
          class="rounded-lg border p-3 text-sm"
          aria-live="polite"
        >
          <p><strong>الطالب:</strong> {{ selectedEnrollment.studentName }}</p>
          <p>
            <strong>الخطة:</strong> {{ selectedEnrollment.academicPlanName }} —
            الإصدار {{ selectedEnrollment.academicPlanVersion }}
          </p>
          <p>
            <strong>المواد في لقطة التسجيل:</strong>
            {{
              selectedEnrollment.itemCount ??
              selectedEnrollment.items?.length ??
              "—"
            }}
          </p>
        </div>

        <div class="form-group">
          <label for="invoice-fee">التعرفة ضمن الخطة</label>
          <select
            id="invoice-fee"
            v-model="form.feeStructureID"
            class="form-select"
            :disabled="!selectedEnrollment || feeLoading"
            required
          >
            <option value="" disabled>اختر التعرفة</option>
            <option
              v-for="fee in feeStructures"
              :key="fee.feeStructureID"
              :value="fee.feeStructureID"
            >
              {{ fee.name }} — {{ fee.amount }}
            </option>
          </select>
          <p v-if="feeLoading" role="status">جاري تحميل التعرفات…</p>
          <p
            v-else-if="selectedEnrollment && !feeStructures.length"
            role="status"
          >
            لا توجد تعرفة لهذا الإصدار من الخطة.
          </p>
          <ValidationError :messages="feeError || errors?.feeStructureID" />
        </div>

        <div class="form-group">
          <label for="period-start">بداية فترة الفوترة</label>
          <input
            id="period-start"
            v-model="form.billingPeriodStart"
            class="form-control"
            type="date"
            required
          />
          <ValidationError :messages="errors?.billingPeriodStart" />
        </div>
        <div class="form-group">
          <label for="period-end">نهاية فترة الفوترة</label>
          <input
            id="period-end"
            v-model="form.billingPeriodEnd"
            class="form-control"
            type="date"
            required
          />
          <ValidationError :messages="errors?.billingPeriodEnd" />
        </div>
        <p
          v-if="selectedFee"
          class="rounded-lg bg-[var(--bg-body)] p-3"
          aria-live="polite"
        >
          مبلغ التعرفة الذي سيُحفظ على الفاتورة:
          <strong>{{ selectedFee.amount }}</strong>
        </p>
      </template>

      <template v-else>
        <dl class="grid gap-2 rounded-lg border p-3 sm:grid-cols-2">
          <div>
            <dt>الطالب</dt>
            <dd>{{ initialInvoice?.studentName }}</dd>
          </div>
          <div>
            <dt>التسجيل الأكاديمي</dt>
            <dd>{{ initialInvoice?.studentAcademicEnrollmentID }}</dd>
          </div>
          <div>
            <dt>الخطة وإصدارها</dt>
            <dd>
              {{ initialInvoice?.academicPlanName }} —
              {{ initialInvoice?.academicPlanVersion }}
            </dd>
          </div>
          <div>
            <dt>التعرفة</dt>
            <dd>{{ initialInvoice?.feeStructureName }}</dd>
          </div>
          <div>
            <dt>فترة الفوترة</dt>
            <dd>
              {{ initialInvoice?.billingPeriodStart }} —
              {{ initialInvoice?.billingPeriodEnd }}
            </dd>
          </div>
          <div>
            <dt>المبلغ الأصلي</dt>
            <dd>{{ initialInvoice?.originalAmount }}</dd>
          </div>
          <div>
            <dt>المدفوع</dt>
            <dd>{{ initialInvoice?.paidAmount }}</dd>
          </div>
          <div>
            <dt>المتبقي</dt>
            <dd>{{ initialInvoice?.remainingAmount }}</dd>
          </div>
          <div>
            <dt>تاريخ اكتمال السداد</dt>
            <dd>
              {{ initialInvoice?.dueDate || "لا يوجد تاريخ لدفعة مكتملة" }}
            </dd>
          </div>
          <div>
            <dt>الحالة</dt>
            <dd>{{ invoiceStatusLabel }}</dd>
          </div>
        </dl>
        <div class="form-group">
          <label for="edit-discount">الخصم</label>
          <input
            id="edit-discount"
            v-model.number="form.discountAmount"
            class="form-control"
            type="number"
            min="0"
            step="0.01"
            :disabled="invoiceLocked"
          />
          <ValidationError :messages="errors?.discountAmount" />
        </div>
      </template>

      <div class="form-group">
        <label for="invoice-notes">ملاحظات</label>
        <input
          id="invoice-notes"
          v-model.trim="form.notes"
          class="form-control"
          :disabled="invoiceLocked"
        />
      </div>
      <p v-if="invoiceLocked" class="text-sm text-amber-700" role="status">
        هذه الفاتورة لها دفعات؛ لا يمكن تعديلها أو حذفها.
      </p>
      <button
        class="btn btn-primary"
        type="submit"
        :disabled="
          invoiceLocked ||
          (!isEdit &&
            (!form.studentAcademicEnrollmentID || !form.feeStructureID))
        "
      >
        {{ isEdit ? "حفظ التعديل" : "إصدار الفاتورة" }}
      </button>
    </form>
  </section>
</template>

<script setup>
import { computed, reactive, ref, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import { InvoiceStatusOptions, getEnumLabel } from "@/Generic/FinanceEnums";

const props = defineProps({
  errors: { type: [Object, Array, String], default: () => ({}) },
  enrollments: { type: Array, default: () => [] },
  enrollmentLoading: { type: Boolean, default: false },
  enrollmentError: { type: [String, Array, Object], default: null },
  feeStructures: { type: Array, default: () => [] },
  feeLoading: { type: Boolean, default: false },
  feeError: { type: [String, Array, Object], default: null },
  initialInvoice: { type: Object, default: null },
  invoiceLocked: { type: Boolean, default: false },
});
const emit = defineEmits([
  "on-submit",
  "search-enrollment",
  "select-enrollment",
]);

const enrollmentLabel = ref("");
const selectedEnrollment = ref(null);
const form = reactive({
  studentInvoiceID: "",
  studentAcademicEnrollmentID: "",
  feeStructureID: "",
  billingPeriodStart: "",
  billingPeriodEnd: "",
  discountAmount: 0,
  notes: "",
});
const isEdit = computed(() => Boolean(form.studentInvoiceID));
const invoiceStatusLabel = computed(() =>
  getEnumLabel(InvoiceStatusOptions, props.initialInvoice?.status)
);
const selectedFee = computed(
  () =>
    props.feeStructures.find(
      (fee) => fee.feeStructureID === form.feeStructureID
    ) || null
);

watch(
  () => props.initialInvoice,
  (invoice) => {
    Object.assign(form, {
      studentInvoiceID: invoice?.studentInvoiceID || "",
      studentAcademicEnrollmentID: invoice?.studentAcademicEnrollmentID || "",
      feeStructureID: invoice?.feeStructureID || "",
      billingPeriodStart: invoice?.billingPeriodStart || "",
      billingPeriodEnd: invoice?.billingPeriodEnd || "",
      discountAmount: invoice?.discountAmount ?? 0,
      notes: invoice?.notes || "",
    });
    enrollmentLabel.value = invoice
      ? `${invoice.studentName || ""} — ${invoice.academicPlanName || ""} (v${
          invoice.academicPlanVersion || "—"
        })`
      : "";
    selectedEnrollment.value = invoice
      ? {
          studentAcademicEnrollmentID: invoice.studentAcademicEnrollmentID,
          studentName: invoice.studentName,
          academicPlanName: invoice.academicPlanName,
          academicPlanVersion: invoice.academicPlanVersion,
        }
      : null;
  },
  { immediate: true }
);

function selectEnrollment(enrollment) {
  selectedEnrollment.value = enrollment;
  form.studentAcademicEnrollmentID =
    enrollment?.studentAcademicEnrollmentID || "";
  form.feeStructureID = "";
  emit("select-enrollment", enrollment);
}

function clearEnrollment() {
  selectedEnrollment.value = null;
  form.studentAcademicEnrollmentID = "";
  form.feeStructureID = "";
  emit("select-enrollment", null);
}

function submit() {
  emit("on-submit", { ...form });
}
</script>
