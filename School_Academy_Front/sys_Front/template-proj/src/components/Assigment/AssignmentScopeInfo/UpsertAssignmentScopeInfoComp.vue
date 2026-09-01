<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <!-- assigmentScope -->
      <div class="matter-card-wrapper">
        <label>نطاق اختبار</label>
        <select
          v-model="formData.assignmentScopeID"
          v-on:change="onAssigmentScopeChange"
        >
          <option disabled selected>اختر نطاق</option>
          <option
            v-for="assigScope in AssigmentScopesData"
            :key="assigScope.assignmentScopeID"
            :value="assigScope.assignmentScopeID"
          >
            {{ assigScope.name }}
          </option>
        </select>
        <ValidationError :messages="errors?.assignmentScopeID" />
      </div>
      <!-- StageInfoID -->
      <div class="matter-card-wrapper">
        <label>مرحلة وحدة</label>
        <select v-model="formData.stageInfoID">
          <option disabled selected>اختر مرحلة وحدة</option>
          <option
            v-for="stageInfo in stageInfosData"
            :key="stageInfo.stageInfoID"
            :value="stageInfo.stageInfoID"
          >
            {{ stageInfo.matterName }}
          </option>
        </select>
        <ValidationError :messages="errors?.stageInfoID" />
      </div>

      <div class="form-group">
        <label>العلامة التامة:</label>
        <input v-model="formData.maxScore" type="number" />
        <ValidationError :messages="errors?.maxScore" />
      </div>
      <div class="form-group">
        <label>الحد الادنى:</label>
        <input v-model="formData.minPassScore" type="number" />
        <ValidationError :messages="errors?.minPassScore" />
      </div>
      <div class="form-group">
        <label>الوزن :</label>
        <input v-model="formData.weight" type="number" />
        <ValidationError :messages="errors?.weight" />
      </div>
      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useAssignmentScopeinfoStore } from "@/store/Assignment/AssignmentScopeInfoStore.js";

export default {
  name: "UpsertStageInfoComp",
  components: {
    ValidationError,
  },
  props: {
    stageInfosData: {
      type: Object,
      default: () => ({}),
    },
    AssigmentScopesData: {
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
      AssigmentScopeInfoStore: useAssignmentScopeinfoStore(),
      formData: {
        assignmentScopeInfoID: "",
        maxScore: 0,
        minPassScore: 0,
        weight: 0,
        stageInfoID: "",
        assignmentScopeID: "",
      },
    };
  },
  emits: ["onSubmit", "onAssigmentScopeChange"],
  computed: {
    FormTitle() {
      return this.formData.assignmentScopeInfoID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },

    fillSelectedAssigmentScopeInfo() {
      if (this.AssigmentScopeInfoStore.selectedAssignmentScopeinfo) {
        const entity = this.AssigmentScopeInfoStore.selectedAssignmentScopeinfo;
        this.formData = {
          assignmentScopeInfoID: entity.assignmentScopeInfoID || "",
          maxScore: entity.maxScore || 0,
          minPassScore: entity.minPassScore || 0,
          weight: entity.weight || 0,
          stageInfoID: entity.stageInfoID || "",
          assignmentScopeID: entity.assignmentScopeID || "",
        };
      }
    },
    handleStageInfo(data, isFound) {
      if (isFound) {
        this.formData.stageInfoID = data.stageInfoID;
      }
    },
    handleAssignmentScope(data, isFound) {
      if (isFound) {
        this.formData.assignmentScopeInfoID = data.assignmentScopeInfoID;
      }
    },

    async onAssigmentScopeChange() {
      var assigmentStage = this.AssigmentScopesData?.find(
        (x) => x.assignmentScopeID == this.formData.assignmentScopeID
      );
      this.$emit("onAssigmentScopeChange", assigmentStage);
    },
  },

  mounted() {
    this.fillSelectedAssigmentScopeInfo();
  },
  /* watch: {
    stageInfosData: {
      handler(data) {
        if (data && data.stageID != null) {
          this.handleStageInfo(data, true);
        } else {
          this.handleStageInfo(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
    AssigmentScopesData: {
      handler(data) {
        if (data && data.sessionID != null) {
          this.handleAssignmentScope(data, true);
        } else {
          this.handleAssignmentScope(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },*/
};
</script>
