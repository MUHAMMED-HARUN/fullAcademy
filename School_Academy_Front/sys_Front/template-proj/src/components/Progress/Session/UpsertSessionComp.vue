<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم السجل:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>
      <div class="form-group">
        <label>نوع السجل:</label>
        <select v-model.number="formData.type" :disabled="!!parentSession">
          <option :value="0">موضوع</option>
          <option :value="1">مادة</option>
        </select>
        <ValidationError :messages="errors?.type" />
      </div>
      <div
        v-if="parentSession"
        class="form-group rounded-lg border border-[var(--border-color)] p-3"
      >
        <label>الموضوع الأب</label>
        <div class="font-medium">{{ parentSession.name }}</div>
      </div>

      <div v-else-if="Number(formData.type) === 1" class="form-group">
        <label>الموضوع الأب (اختياري):</label>
        <GenericSearchByFilter
          v-model="formData.parentSessionID"
          v-model:name-value="formData.parentSessionName"
          :filter-types="parentFilterTypes"
          :results="parentResults"
          :loading="parentLoading"
          :error="parentError"
          id-field="sessionID"
          name-field="name"
          placeholder="ابحث عن موضوع رئيسي"
          @search="$emit('search-parent', $event.value)"
          @clear="onParentClear"
        />
        <ValidationError :messages="errors?.parentSessionID" />
      </div>
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
import { useSessionStore } from "@/store/Progress/SessionStore";

export default {
  name: "UpsertSessionComp",
  components: {
    ValidationError,
    GenericSearchByFilter,
  },
  props: {
    parentSession: { type: Object, default: null },
    parentResults: { type: Array, default: () => [] },
    parentLoading: { type: Boolean, default: false },
    parentError: { type: [String, Array, Object], default: null },
    errors: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      SessionStore: useSessionStore(),
      parentFilterTypes: [{ value: 1, label: "اسم الموضوع" }],
      formData: {
        sessionID: "",
        name: "",
        note: "",
        type: 0,
        parentSessionID: "",
        parentSessionName: "",
      },
    };
  },
  emits: ["onSubmit", "search-parent", "clear-parent"],
  computed: {
    FormTitle() {
      if (this.formData.sessionID) return "تعديل السجل";
      if (this.parentSession) return `إضافة ابن لـ ${this.parentSession.name}`;
      return "إضافة سجل";
    },
  },
  methods: {
    onSubmit() {
      if (this.parentSession) {
        this.formData.type = 1;
        this.formData.parentSessionID = this.parentSession.sessionID;
        this.formData.parentSessionName = this.parentSession.name;
      }
      this.$emit("onSubmit", this.formData);
    },
    onParentClear() {
      this.formData.parentSessionID = "";
      this.formData.parentSessionName = "";
      this.$emit("clear-parent");
    },
    fillSelectedSession() {
      if (this.SessionStore.selectedSession) {
        const entity = this.SessionStore.selectedSession;
        this.formData = {
          sessionID: entity.sessionID || "",
          name: entity.name || "",
          note: entity.note || "",
          type: entity?.type,
          parentSessionID: entity.parentSessionID || "",
          parentSessionName: entity.parentSessionName || "",
        };
      } else if (this.parentSession) {
        this.formData = {
          sessionID: "",
          name: "",
          note: "",
          type: 1,
          parentSessionID: this.parentSession.sessionID,
          parentSessionName: this.parentSession.name,
        };
      }
    },
  },

  mounted() {
    this.fillSelectedSession();
  },
  watch: {
    "formData.type"(type) {
      if (Number(type) === 0 && !this.parentSession) {
        this.formData.parentSessionID = "";
        this.formData.parentSessionName = "";
        this.$emit("clear-parent");
      }
    },
  },
};
</script>
