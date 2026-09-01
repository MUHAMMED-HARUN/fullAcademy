<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم المادة:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>
      <div class="form-group">
        <label>نوع المادة:</label>
        <select v-model="formData.type">
          <option value="" disabled>اختر نوع المادة</option>
          <option value="0">موضوع</option>
          <option value="1">مادة</option>
        </select>
        <ValidationError :messages="errors?.type" />
      </div>
      <div class="form-group">
        <label>نوع المادة:</label>
        <select v-model="formData.parentSessionID">
          <option value="" disabled>اختر نوع المادة</option>
          <option
            v-for="session in Sessions"
            :value="session.sessionID"
            :key="session.sessionID"
          >
            {{ session.name }}
          </option>
        </select>
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
import { useSessionStore } from "@/store/Progress/SessionStore";

export default {
  name: "UpsertSessionComp",
  components: {
    ValidationError,
  },
  props: {
    Sessions: {
      type: Object,
      default: () => ({}),
    },
    errors: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      SessionStore: useSessionStore(),
      formData: {
        sessionID: "",
        name: "",
        note: "",
        type: 1,
        parentSessionID: "",
      },
    };
  },
  emits: ["onSubmit"],
  computed: {
    FormTitle() {
      return this.formData.sessionID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
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
        };
      }
    },
  },

  mounted() {
    this.fillSelectedSession();
  },
};
</script>
