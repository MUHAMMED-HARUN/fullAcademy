<template>
  <section dir="rtl" class="space-y-4">
    <h2>{{ formData.feeStructureID ? "تعديل التعرفة" : "إضافة تعرفة" }}</h2>
    <form class="space-y-4" @submit.prevent="submit">
      <fieldset :disabled="formData.planLocked" class="space-y-4">
        <label class="block"
          >اسم التعرفة
          <input
            v-model.trim="formData.name"
            class="form-control"
            required
            maxlength="200"
          />
          <ValidationError :messages="errors?.name" />
        </label>
        <label class="block"
          >نوع الرسوم
          <select
            v-model.number="formData.feeType"
            class="form-select"
            required
          >
            <option
              v-for="option in FeeTypeOptions"
              :key="option.value"
              :value="option.value"
            >
              {{ option.label }}
            </option>
          </select>
          <ValidationError :messages="errors?.feeType" />
        </label>
        <label class="block"
          >المبلغ
          <input
            v-model.number="formData.amount"
            class="form-control"
            type="number"
            min="0"
            step="0.01"
            required
          />
          <ValidationError :messages="errors?.amount" />
        </label>
        <div>
          <label class="mb-2 block"
            >الخطة الأكاديمية (يجب أن تكون مفعّلة)</label
          >
          <GenericSearchByFilter
            v-model="formData.academicPlanID"
            v-model:name-value="academicPlanName"
            :filter-types="[{ value: 1, label: 'اسم الخطة' }]"
            :results="AcademicPlanStore.academicPlans"
            :loading="AcademicPlanStore.loading"
            :disabled="formData.planLocked"
            :error="AcademicPlanStore.error"
            id-field="academicPlanID"
            name-field="displayName"
            placeholder="ابحث عن خطة أكاديمية مفعّلة"
            @search="searchPlans"
            @select="onPlanSelected"
            @clear="selectedPlan = null"
          />
          <div v-if="selectedPlan" class="mt-2 rounded border p-3 text-sm">
            {{ selectedPlan.providerInstituteName }} ·
            {{ selectedPlan.scopeName }} · إصدار {{ selectedPlan.version }}
          </div>
          <ValidationError :messages="errors?.academicPlanID" />
        </div>
        <label class="block"
          >ملاحظات
          <input v-model.trim="formData.notes" class="form-control" />
        </label>
      </fieldset>
      <p v-if="formData.planLocked" class="text-sm text-amber-700">
        لا يمكن تعديل تعرفة مرتبطة بفواتير سابقة؛ أنشئ تعرفة جديدة على إصدار
        الخطة المناسب.
      </p>
      <button
        class="btn btn-primary"
        type="submit"
        :disabled="!formData.academicPlanID || formData.planLocked"
      >
        حفظ
      </button>
    </form>
  </section>
</template>

<script setup>
import { reactive, ref, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import { FeeType, FeeTypeOptions } from "@/Generic/FinanceEnums";
import { useAcademicPlanStore } from "@/store/AcademicPlan/AcademicPlanStore";

const props = defineProps({
  errors: { type: Object, default: () => ({}) },
  initialFeeStructure: { type: Object, default: null },
  planLocked: { type: Boolean, default: false },
});
const emit = defineEmits(["onSubmit"]);
const AcademicPlanStore = useAcademicPlanStore();
const academicPlanName = ref("");
const selectedPlan = ref(null);
const formData = reactive({
  feeStructureID: "",
  name: "",
  feeType: FeeType.Tuition,
  amount: 0,
  academicPlanID: "",
  notes: "",
  planLocked: false,
});

watch(
  () => props.initialFeeStructure,
  (item) => {
    Object.assign(formData, {
      feeStructureID: item?.feeStructureID || "",
      name: item?.name || "",
      feeType: item?.feeType ?? FeeType.Tuition,
      amount: item?.amount ?? 0,
      academicPlanID: item?.academicPlanID || "",
      notes: item?.notes || "",
      planLocked: props.planLocked || !!(item?.isUsed ?? item?.hasInvoices),
    });
    academicPlanName.value = item?.academicPlanName || "";
    selectedPlan.value = item?.academicPlanID
      ? {
          academicPlanID: item.academicPlanID,
          name: item.academicPlanName,
          providerInstituteName: item.providerInstituteName,
          scopeName: item.scopeName,
          version: item.academicPlanVersion,
        }
      : null;
  },
  { immediate: true }
);

async function searchPlans({ value }) {
  const search = value?.trim().replace(/[,*|]/g, " ");
  await AcademicPlanStore.fetchAcademicPlans({
    Page: 1,
    PageSize: 10,
    Filter: search ? `Name=*${search},Status=2` : "Status=2",
  });
}
function onPlanSelected(plan) {
  selectedPlan.value = plan;
  academicPlanName.value = plan?.name || "";
}
function submit() {
  emit("onSubmit", { ...formData });
}
</script>
