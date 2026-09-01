<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم المشروع:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
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
import { useProgramStore } from "@/store/Institute/useProgramStore";

export default {
  name: "UpsertProgramComp",
  components: {
    ValidationError,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      ProgramStore: useProgramStore(),
      formData: {
        programID: "",
        name: "",
        note: "",
      },
    };
  },
  emits: ["onSubmit"],
  computed: {
    FormTitle() {
      return this.formData.programID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    fillSelectedProgram() {
      if (this.ProgramStore.selectedProgram) {
        const entity = this.ProgramStore.selectedProgram;
        this.formData = {
          programID: entity.programID || "",
          name: entity.name || "",
          note: entity.note || "",
        };
      }
    },
  },

  mounted() {
    this.fillSelectedProgram();
  },
};
</script>
