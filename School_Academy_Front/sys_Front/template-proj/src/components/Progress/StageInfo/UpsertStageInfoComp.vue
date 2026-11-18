<template>
  <div dir="rtl" class="space-y-4">
    <h2>{{ FormTitle }}</h2>
    <form class="space-y-4" @submit.prevent="onSubmit">
      <div
        v-if="lockStage"
        class="form-group rounded-lg border border-[var(--border-color)] p-3"
      >
        <label>المرحلة</label>
        <StageCard v-if="stageData" :DataReq="stageData" />
        <p v-else class="alert-error" role="alert">
          تعذر تحميل المرحلة المرتبطة بهذه الصفحة.
        </p>
      </div>
      <div v-else class="form-group">
        <label>المرحلة:</label>
        <GenericSearchByFilter
          v-model="formData.stageID"
          v-model:name-value="formData.stageName"
          :filter-types="stageFilterTypes"
          :results="stageResults"
          :loading="stageLoading"
          :error="stageError"
          id-field="stageID"
          name-field="name"
          placeholder="ابحث عن مرحلة"
          @search="$emit('search-stage', $event.value)"
          @select="$emit('select-stage', $event)"
          @clear="onClearStage"
        />
        <StageCard v-if="stageData" class="mt-2" :DataReq="stageData" />
        <ValidationError :messages="errors?.stageID" />
      </div>

      <div class="form-group">
        <label>المادة:</label>
        <GenericSearchByFilter
          v-model="formData.matterID"
          v-model:name-value="formData.matterName"
          :filter-types="matterFilterTypes"
          :results="matterResults"
          :loading="matterLoading"
          :error="matterError"
          id-field="sessionID"
          name-field="name"
          placeholder="ابحث عن مادة"
          @search="$emit('search-matter', $event.value)"
          @select="$emit('select-matter', $event)"
          @clear="onClearMatter"
        />
        <SessionCard v-if="matterData" class="mt-2" :DataReq="matterData" />
        <ValidationError :messages="errors?.matterID" />
      </div>

      <div class="grid gap-4 sm:grid-cols-2">
        <div class="form-group">
          <label>من:</label>
          <input
            v-model.number="formData.from"
            class="form-control"
            type="number"
            min="0"
            step="any"
          />
          <ValidationError :messages="errors?.from" />
        </div>
        <div class="form-group">
          <label>إلى:</label>
          <input
            v-model.number="formData.to"
            class="form-control"
            type="number"
            min="0"
            step="any"
          />
          <ValidationError :messages="errors?.to" />
        </div>
      </div>

      <div class="grid gap-4 sm:grid-cols-2">
        <div class="form-group">
          <label>نوع الوحدة:</label>
          <select v-model.number="formData.uniteType" class="form-select">
            <option :value="enUniteType.Page">صفحة</option>
            <option :value="enUniteType.Unite">وحدة</option>
          </select>
          <ValidationError :messages="errors?.uniteType" />
        </div>
        <div class="form-group">
          <label>نوع التقدم:</label>
          <select v-model.number="formData.progressType" class="form-select">
            <option :value="enProgressType.Read">قراءة</option>
            <option :value="enProgressType.Save">حفظ</option>
            <option :value="enProgressType.Write">كتابة</option>
          </select>
          <ValidationError :messages="errors?.progressType" />
        </div>
      </div>

      <button class="btn btn-primary" type="submit" :disabled="saving">
        {{ saving ? "جارٍ الحفظ…" : "حفظ" }}
      </button>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import StageCard from "@/components/Progress/Stage/StageCard.vue";
import SessionCard from "@/components/Progress/Session/SessionCard.vue";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";

const enUniteType = Object.freeze({ Page: 0, Unite: 1 });
const enProgressType = Object.freeze({ Read: 0, Save: 1, Write: 2 });

export default {
  name: "UpsertStageInfoComp",
  components: {
    ValidationError,
    GenericSearchByFilter,
    StageCard,
    SessionCard,
  },
  props: {
    stageData: { type: Object, default: null },
    matterData: { type: Object, default: null },
    lockStage: { type: Boolean, default: false },
    stageResults: { type: Array, default: () => [] },
    stageLoading: { type: Boolean, default: false },
    stageError: { type: [String, Array, Object], default: null },
    matterResults: { type: Array, default: () => [] },
    matterLoading: { type: Boolean, default: false },
    matterError: { type: [String, Array, Object], default: null },
    errors: { type: Object, default: () => ({}) },
    saving: { type: Boolean, default: false },
  },
  emits: [
    "submit",
    "search-stage",
    "select-stage",
    "clear-stage",
    "search-matter",
    "select-matter",
    "clear-matter",
  ],
  data() {
    return {
      StageInfoStore: useStageInfoStore(),
      enUniteType,
      enProgressType,
      stageFilterTypes: [{ value: 1, label: "اسم المرحلة" }],
      matterFilterTypes: [{ value: 1, label: "اسم المادة" }],
      formData: {
        stageInfoID: "",
        from: 0,
        to: 0,
        stageID: "",
        stageName: "",
        matterID: "",
        matterName: "",
        uniteType: enUniteType.Page,
        progressType: enProgressType.Read,
      },
    };
  },
  computed: {
    FormTitle() {
      return this.formData.stageInfoID
        ? "تعديل تفاصيل المرحلة"
        : "إضافة تفاصيل المرحلة";
    },
  },
  methods: {
    fillSelectedStageInfo() {
      const entity = this.StageInfoStore.selectedStageInfo;
      if (entity) {
        this.formData = {
          stageInfoID: entity.stageInfoID || "",
          from: Number(entity.from ?? 0),
          to: Number(entity.to ?? 0),
          stageID: entity.stageID || "",
          stageName: entity.stageName || "",
          matterID: entity.matterID || "",
          matterName: entity.matterName || "",
          uniteType: Number(entity.uniteType ?? enUniteType.Page),
          progressType: Number(entity.progressType ?? enProgressType.Read),
        };
        return;
      }

      if (this.lockStage && this.stageData) {
        this.formData.stageID = this.stageData.stageID || "";
        this.formData.stageName = this.stageData.name || "";
      }
    },
    onSubmit() {
      if (this.lockStage && this.stageData) {
        this.formData.stageID = this.stageData.stageID;
        this.formData.stageName = this.stageData.name;
      }
      this.$emit("submit", { ...this.formData });
    },
    onClearStage() {
      this.formData.stageID = "";
      this.formData.stageName = "";
      this.$emit("clear-stage");
    },
    onClearMatter() {
      this.formData.matterID = "";
      this.formData.matterName = "";
      this.$emit("clear-matter");
    },
  },
  watch: {
    stageData(data) {
      if (this.lockStage && data) {
        this.formData.stageID = data.stageID || "";
        this.formData.stageName = data.name || "";
      }
    },
    matterData(data) {
      if (data) {
        this.formData.matterID = data.sessionID || "";
        this.formData.matterName = data.name || "";
      }
    },
  },
  mounted() {
    this.fillSelectedStageInfo();
  },
};
</script>
