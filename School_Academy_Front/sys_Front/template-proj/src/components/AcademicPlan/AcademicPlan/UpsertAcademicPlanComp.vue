<script setup>
import { computed, reactive, watch } from "vue";
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import {
  AcademicPlanScopeType,
  AcademicPlanScopeTypeOptions,
} from "@/Generic/AcademicPlanEnums";

const props = defineProps({
  errors: {
    type: Object,
    default: () => ({}),
  },
  plan: {
    type: Object,
    default: null,
  },
  providerResults: {
    type: Array,
    default: () => [],
  },
  providerLoading: {
    type: Boolean,
    default: false,
  },
  providerError: {
    type: [String, Array, Object],
    default: null,
  },
  programs: {
    type: Array,
    default: () => [],
  },
  instituteClasses: {
    type: Array,
    default: () => [],
  },
  classes: {
    type: Array,
    default: () => [],
  },
  stages: {
    type: Array,
    default: () => [],
  },
  stageInfos: {
    type: Array,
    default: () => [],
  },
});

const emit = defineEmits(["submit", "provider-search", "provider-clear"]);

const emptyForm = () => ({
  academicPlanID: "",
  name: "",
  providerInstituteID: "",
  providerInstituteName: "",
  scopeType: AcademicPlanScopeType.Program,
  programID: "",
  instituteClassID: "",
  classID: "",
  stageID: "",
  stageInfoID: "",
  effectiveFrom: new Date().toISOString().substring(0, 10),
  effectiveTo: "",
  notes: "",
});

const form = reactive(emptyForm());

const isEdit = computed(() => !!form.academicPlanID);

const instituteClassOptions = computed(() =>
  props.instituteClasses.filter(
    (item) => item.instituteID === form.providerInstituteID
  )
);

const stageOptions = computed(() =>
  props.stages.filter((stage) => stage.classID === form.classID)
);

const stageInfoOptions = computed(() =>
  props.stageInfos.filter((info) => info.stageID === form.stageID)
);

const formTitle = computed(() =>
  isEdit.value ? "تعديل خطة أكاديمية" : "إضافة خطة أكاديمية"
);

const showProgramSelect = computed(
  () => Number(form.scopeType) === AcademicPlanScopeType.Program
);
const showInstituteClassSelect = computed(
  () => Number(form.scopeType) === AcademicPlanScopeType.InstituteClass
);
const showStageSelect = computed(
  () =>
    Number(form.scopeType) === AcademicPlanScopeType.Stage ||
    Number(form.scopeType) === AcademicPlanScopeType.StageInfo
);
const showStageInfoSelect = computed(
  () => Number(form.scopeType) === AcademicPlanScopeType.StageInfo
);

const onScopeTypeChange = () => {
  form.programID = "";
  form.instituteClassID = "";
  form.classID = "";
  form.stageID = "";
  form.stageInfoID = "";
};

const onClassChange = () => {
  form.stageID = "";
  form.stageInfoID = "";
};

const onStageChange = () => {
  form.stageInfoID = "";
};

const onProviderSelect = (item) => {
  form.providerInstituteID = item.instituteID;
  form.providerInstituteName = item.name;
  form.instituteClassID = "";
};

const onProviderClear = () => {
  form.providerInstituteID = "";
  form.providerInstituteName = "";
  form.instituteClassID = "";
  emit("provider-clear");
};

const onSubmit = () => {
  emit("submit", { ...form });
};

watch(
  () => props.plan,
  (plan) => {
    Object.assign(form, emptyForm());
    if (!plan) return;
    Object.assign(form, {
      academicPlanID: plan.academicPlanID || "",
      name: plan.name || "",
      providerInstituteID: plan.providerInstituteID || "",
      providerInstituteName: plan.providerInstituteName || "",
      scopeType: plan.scopeType || AcademicPlanScopeType.Program,
      programID: plan.programID || "",
      instituteClassID: plan.instituteClassID || "",
      stageID: plan.stageID || "",
      stageInfoID: plan.stageInfoID || "",
      effectiveFrom: plan.effectiveFrom || "",
      effectiveTo: plan.effectiveTo || "",
      notes: plan.notes || "",
    });
  },
  { immediate: true }
);
</script>

<template>
  <div>
    <h2>{{ formTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم الخطة:</label>
        <input v-model="form.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>

      <template v-if="!isEdit">
        <div class="form-group">
          <label>المؤسسة المانحة:</label>
          <GenericSearchByFilter
            :model-value="form.providerInstituteID"
            :name-value="form.providerInstituteName"
            :results="providerResults"
            id-field="instituteID"
            name-field="name"
            placeholder="ابحث عن المؤسسة بالاسم"
            :loading="providerLoading"
            :error="providerError"
            empty-message="لا توجد مؤسسات مطابقة"
            @select="onProviderSelect"
            @clear="onProviderClear"
            @search="$emit('provider-search', $event)"
          />
          <ValidationError :messages="errors?.providerInstituteID" />
        </div>

        <div class="form-group">
          <label>نوع النطاق:</label>
          <select v-model.number="form.scopeType" @change="onScopeTypeChange">
            <option
              v-for="opt in AcademicPlanScopeTypeOptions"
              :key="opt.value"
              :value="opt.value"
            >
              {{ opt.label }}
            </option>
          </select>
        </div>

        <div v-if="showProgramSelect" class="form-group">
          <label>البرنامج:</label>
          <select v-model="form.programID">
            <option value="">اختر البرنامج</option>
            <option
              v-for="program in programs"
              :key="program.programID"
              :value="program.programID"
            >
              {{ program.name }}
            </option>
          </select>
          <ValidationError :messages="errors?.programID" />
        </div>

        <template v-if="showInstituteClassSelect">
          <div class="form-group">
            <label>صف المدرسة:</label>
            <select
              v-model="form.instituteClassID"
              :disabled="!form.providerInstituteID"
            >
              <option value="">
                {{
                  form.providerInstituteID ? "اختر الصف" : "اختر المؤسسة أولاً"
                }}
              </option>
              <option
                v-for="item in instituteClassOptions"
                :key="item.instituteClassID"
                :value="item.instituteClassID"
              >
                {{ item.className }}
              </option>
            </select>
            <ValidationError :messages="errors?.instituteClassID" />
          </div>
        </template>

        <template v-if="showStageSelect">
          <div class="form-group">
            <label>الصف الدراسي:</label>
            <select v-model="form.classID" @change="onClassChange">
              <option value="">اختر الصف</option>
              <option
                v-for="item in classes"
                :key="item.classID"
                :value="item.classID"
              >
                {{ item.name }}
              </option>
            </select>
          </div>
          <div class="form-group">
            <label>المرحلة:</label>
            <select
              v-model="form.stageID"
              :disabled="!form.classID"
              @change="onStageChange"
            >
              <option value="">
                {{ form.classID ? "اختر المرحلة" : "اختر الصف أولاً" }}
              </option>
              <option
                v-for="stage in stageOptions"
                :key="stage.stageID"
                :value="stage.stageID"
              >
                {{ stage.name }}
              </option>
            </select>
            <ValidationError :messages="errors?.stageID" />
          </div>
        </template>

        <div v-if="showStageInfoSelect" class="form-group">
          <label>المادة:</label>
          <select v-model="form.stageInfoID" :disabled="!form.stageID">
            <option value="">
              {{ form.stageID ? "اختر المادة" : "اختر المرحلة أولاً" }}
            </option>
            <option
              v-for="info in stageInfoOptions"
              :key="info.stageInfoID"
              :value="info.stageInfoID"
            >
              {{ info.matterName }}
            </option>
          </select>
          <ValidationError :messages="errors?.stageInfoID" />
        </div>
      </template>

      <template v-else>
        <div class="form-group">
          <label>المؤسسة المانحة:</label>
          <input :value="form.providerInstituteName" type="text" disabled />
        </div>
        <div class="form-group">
          <label>النطاق:</label>
          <input :value="plan?.scopeName" type="text" disabled />
        </div>
      </template>

      <div class="form-group">
        <label>تاريخ البداية:</label>
        <input v-model="form.effectiveFrom" type="date" />
        <ValidationError :messages="errors?.effectiveFrom" />
      </div>

      <div class="form-group">
        <label>تاريخ النهاية (اختياري):</label>
        <input v-model="form.effectiveTo" type="date" />
        <ValidationError :messages="errors?.effectiveTo" />
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="form.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>
