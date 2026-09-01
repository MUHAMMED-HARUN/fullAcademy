<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم مرحلة الاختبار:</label>
        <input v-model="formData.name" type="text" />
        <ValidationError :messages="errors?.name" />
      </div>

      <div class="form-filter mt-4">
        <label>البحث عن مرحلة:</label>
        <select v-model="StageSearchType">
          <option :value="enStageSearchType.StageName">بحث باسم المرحلة</option>
        </select>
        <input v-model="StageSearchValue" type="text" />
        <button @click.prevent.stop="onSearchStage">بحث</button>
      </div>

      <div class="program-card-wrapper">
        <StageCard v-if="showStageCard" :-data-req="stageData" />
        <ValidationError :messages="errors?.stageID" />
      </div>

      <div class="form-group">
        <label> ربط الاختبار السابق:</label>
        <select v-model="formData.prevAssignmentScopeID">
          <option value="" disabled>اختر الاختبار السابق</option>
          <option
            v-for="assigScope in AssignmentScopes"
            :value="assigScope.assignmentScopeID"
            :key="assigScope.assignmentScopeID"
          >
            {{ assigScope.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.prevAssignmentScopeID" />
      </div>

      <div class="form-group">
        <label>ملاحظات:</label>
        <input v-model="formData.notes" type="text" />
        <ValidationError :messages="errors?.notes" />
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
import StageCard from "@/components/Progress/Stage/StageCard.vue";
import { useAssignmentScopeStore } from "@/store/Assignment/AssignmentScopeStore";
const enStageSearchType = Object.freeze({ ID: 0, StageName: 1 });

export default {
  name: "UpsertAssignmentScopeComp",
  components: {
    ValidationError,
    StageCard,
  },
  props: {
    AssignmentScopes: {
      type: Object,
      default: () => ({}),
    },
    stageData: {
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
      enStageSearchType: enStageSearchType,
      StageSearchType: enStageSearchType.ProgramName,
      StageSearchValue: "",
      showStageCard: false,

      AssignmentScopeStore: useAssignmentScopeStore(),
      formData: {
        assignmentScopeID: "",
        name: "",
        notes: "",
        stageID: "",
        prevAssignmentScopeID: "",
      },
    };
  },
  emits: ["onSubmit", "onSearchStage"],
  computed: {
    FormTitle() {
      return this.formData.stageID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onSearchStage() {
      this.$emit("onSearchStage", this.StageSearchType, this.StageSearchValue);
    },
    fillSelectedAssignmentScope() {
      if (this.AssignmentScopeStore.selectedAssignmentScope) {
        const entity = this.AssignmentScopeStore.selectedAssignmentScope;
        this.formData = {
          assignmentScopeID: entity.assignmentScopeID || "",
          name: entity.name || "",
          notes: entity.notes || "",
          stageID: entity.stageID || "",
          prevAssignmentScopeID: entity.prevAssignmentScopeID || "",
        };
      }
    },
    handleStage(data, isFound) {
      if (isFound) {
        this.formData.stageID = data.stageID;
        this.showStageCard = true;
      } else {
        this.showStageCard = false;
      }
    },
  },

  mounted() {
    this.fillSelectedAssignmentScope();
  },
  watch: {
    stageData: {
      handler(data) {
        if (data && data.programID != null) {
          this.handleStage(data, true);
        } else {
          this.handleStage(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
