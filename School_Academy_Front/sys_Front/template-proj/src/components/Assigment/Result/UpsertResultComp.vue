<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>الطالب :</label>
        <label>{{ studentEnrollmentName }}</label>
      </div>
      <div class="form-group">
        <label>نطاق الاخنبار :</label>
        <label>{{ AssigmentScopeInfossData[0]?.assignmentScopeName }}</label>
      </div>

      <div class="matter-card-wrapper">
        <label>وحدة الاختبار</label>
        <select v-model="formData.assignmentScopeInfoID">
          <option disabled selected>اختر نطاق</option>
          <option
            v-for="assigScopeinfo in AssigmentScopeInfossData"
            :key="assigScopeinfo.assignmentScopeInfoID"
            :value="assigScopeinfo.assignmentScopeInfoID"
          >
            {{ assigScopeinfo.stageInfoMatterNameName }}
          </option>
        </select>
        <ValidationError :messages="errors?.assignmentScopeInfoID" />
      </div>

      <div class="form-group">
        <label>النتيجة :</label>
        <input v-model="formData.score" type="number" />
        <ValidationError :messages="errors?.score" />
      </div>

      <div class="form-group">
        <label>ملاحظات :</label>
        <input v-model="formData.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useResultStore } from "@/store/Assignment/ResultStore";

export default {
  name: "UpsertStageInfoComp",
  components: {
    ValidationError,
  },
  props: {
    AssigmentScopeInfossData: {
      type: Object,
      default: () => ({}),
    },

    studentEnrollmentName: {
      type: String,
      default: () => "",
    },
    asignmentID: {
      type: String,
      default: () => "",
    },
    errors: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      ResultStore: useResultStore(),
      formData: {
        resultID: "",
        assignmentID: null,
        assignmentScopeInfoID: null,
        score: "",
        notes: "",
      },
    };
  },
  emits: ["onSubmit"],
  computed: {
    FormTitle() {
      return this.formData.resultID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },

    fillSelectedResult() {
      if (this.ResultStore.selectedResult) {
        const entity = this.ResultStore.selectedResult;
        this.formData = {
          resultID: entity.resultID || "",
          assignmentID: entity.assignmentID || "",
          assignmentScopeInfoID: entity.assignmentScopeInfoID || "",
          score: entity.score || 0,
          notes: entity.notes || "",
        };
      } else if (this.asignmentID !== null || this.asignmentID !== "") {
        this.formData.assignmentID = this.asignmentID;
      }
    },
  },

  mounted() {
    this.fillSelectedResult();
  },
};
</script>
