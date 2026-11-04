<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم المرحلة:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-group">
        <label for="stage-class">الصف:</label>
        <select
          id="stage-class"
          v-model="formData.classID"
          class="form-select"
          :disabled="classLoading"
          @change="onClassChange"
        >
          <option value="">اختر الصف</option>
          <option
            v-for="classItem in classes"
            :key="classItem.classID"
            :value="classItem.classID"
          >
            {{ classItem.name }}
            {{ classItem.programName ? ` — ${classItem.programName}` : "" }}
          </option>
        </select>
        <p
          v-if="classLoading"
          class="text-sm text-[var(--text-muted)]"
          role="status"
        >
          جارٍ تحميل الصفوف…
        </p>
        <p v-else-if="classError" class="alert-error" role="alert">
          {{ classError }}
        </p>
        <ValidationError :messages="errors?.classID" />
      </div>

      <div v-if="formData.classID" class="form-group">
        <label>المرحلة السابقة (اختياري):</label>
        <GenericSearchByFilter
          v-model="formData.prevStageID"
          v-model:name-value="formData.prevStageName"
          :filter-types="previousStageFilterTypes"
          :results="previousStageResults"
          :loading="previousStageLoading"
          :error="previousStageError"
          id-field="stageID"
          name-field="name"
          placeholder="ابحث عن المرحلة السابقة ضمن الصف نفسه"
          @search="onSearchPreviousStage"
          @clear="onClearPreviousStage"
        />
        <ValidationError :messages="errors?.prevStageID" />
      </div>
      <p v-else class="text-sm text-[var(--text-muted)]" role="status">
        اختر الصف أولاً لتحديد المرحلة السابقة.
      </p>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.note" type="text" />
        <ValidationError :messages="errors?.note" />
      </div>

      <!-- تضمين مكون العنوان الفرعي بربط v-model -->

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import { useStageStore } from "@/store/Progress/StageStore";

export default {
  name: "UpsertStageComp",
  components: {
    ValidationError,
    GenericSearchByFilter,
  },
  props: {
    classes: { type: Array, default: () => [] },
    classLoading: { type: Boolean, default: false },
    classError: { type: [String, Array, Object], default: null },
    previousStageResults: { type: Array, default: () => [] },
    previousStageLoading: { type: Boolean, default: false },
    previousStageError: { type: [String, Array, Object], default: null },
    errors: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      StageStore: useStageStore(),
      previousStageFilterTypes: [{ value: 1, label: "اسم المرحلة" }],
      formData: {
        stageID: "",
        name: "",
        note: "",
        classID: "",
        prevStageID: "",
        prevStageName: "",
      },
    };
  },
  emits: ["onSubmit", "search-previous-stage", "clear-previous-stage"],
  computed: {
    FormTitle() {
      return this.formData.stageID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onClassChange() {
      this.formData.prevStageID = "";
      this.formData.prevStageName = "";
      this.$emit("clear-previous-stage");
    },
    onSearchPreviousStage(search) {
      this.$emit("search-previous-stage", {
        term: search.value,
        classID: this.formData.classID,
        excludeStageID: this.formData.stageID,
      });
    },
    onClearPreviousStage() {
      this.formData.prevStageID = "";
      this.formData.prevStageName = "";
      this.$emit("clear-previous-stage");
    },
    fillSelectedStage() {
      if (this.StageStore.selectedStage) {
        const entity = this.StageStore.selectedStage;
        this.formData = {
          stageID: entity.stageID || "",
          name: entity.name || "",
          note: entity.note || "",
          classID: entity.classID || "",
          prevStageID: entity.prevStageID || "",
          prevStageName: entity.prevStageName || "",
        };
      }
    },
  },

  mounted() {
    this.fillSelectedStage();
  },
};
</script>
