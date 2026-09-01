<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <!-- StageID -->
      <div class="form-filter mt-4">
        <label>البحث عن مرحلة:</label>
        <select v-model="stageSearchType">
          <option :value="enStageSearchType.StagemName">بحث باسم مرحلة</option>
        </select>
        <input v-model="stageSearchValue" type="text" />
        <button @click.prevent.stop="onSearchStage">بحث</button>
      </div>

      <div class="stage-card-wrapper">
        <StageCard v-if="showStageCard" :-data-req="stageData" />
        <ValidationError :messages="errors?.stageID" />
      </div>

      <!-- Matter -->
      <div class="form-filter mt-4">
        <label>البحث عن مادة:</label>
        <select v-model="matterSearchType">
          <option :value="enMatterSearchType.MatterName">
            بحث باسم المادة
          </option>
        </select>
        <input v-model="matterSearchValue" type="text" />
        <button @click.prevent.stop="onSearchMatter">بحث</button>
      </div>

      <div class="matter-card-wrapper">
        <SessionCard v-if="showMatterCard" :-data-req="matterData" />
        <ValidationError :messages="errors?.matterID" />
      </div>

      <div class="form-group">
        <label>من:</label>
        <input v-model="formData.from" type="number" />
        <ValidationError :messages="errors?.from" />
      </div>

      <div class="form-group">
        <label>الى:</label>
        <input v-model="formData.to" type="number" />
        <ValidationError :messages="errors?.to" />
      </div>

      <div class="form-group">
        <label>نوع الوحدة:</label>
        <select v-model="formData.uniteType">
          <option :value="enUniteType.Page">صفحة</option>
          <option :value="enUniteType.Unite">وحدة</option>
        </select>
        <ValidationError :messages="errors?.uniteType" />
      </div>

      <div class="form-group">
        <label>نوع الوحدة:</label>
        <select v-model="formData.progressType">
          <option :value="enProgressType.Read">قرائة</option>
          <option :value="enProgressType.Write">كتابة</option>
          <option :value="enProgressType.Save">حفظ</option>
        </select>
        <ValidationError :messages="errors?.progressType" />
      </div>
      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore.js";
import StageCard from "../Stage/StageCard.vue";
import SessionCard from "../Session/SessionCard.vue";
const enStageSearchType = Object.freeze({ ID: 0, StagemName: 1 });
const enMatterSearchType = Object.freeze({ ID: 0, MatterName: 1 });
const enUniteType = Object.freeze({ Page: 0, Unite: 1 });
const enProgressType = Object.freeze({ Read: 0, Save: 1, Write: 2 });

export default {
  name: "UpsertStageInfoComp",
  components: {
    ValidationError,
    StageCard,
    SessionCard,
  },
  props: {
    stageData: {
      type: Object,
      default: () => ({}),
    },
    matterData: {
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
      stageSearchType: enStageSearchType.StagemName,
      stageSearchValue: "",
      showStageCard: false,

      enMatterSearchType: enMatterSearchType,
      matterSearchType: enMatterSearchType.MatterName,
      matterSearchValue: "",
      showMatterCard: false,

      enUniteType: enUniteType,
      enProgressType: enProgressType,

      StageInfoStore: useStageInfoStore(),
      formData: {
        stageInfoID: "",
        from: "",
        to: "",
        stageID: "",
        matterID: "",
        uniteType: 0,
        progressType: 0,
      },
    };
  },
  emits: ["onSubmit", "onSearchStage", "onSearchMatter"],
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
      this.$emit("onSearchStage", this.stageSearchType, this.stageSearchValue);
    },
    onSearchMatter() {
      this.$emit(
        "onSearchMatter",
        this.matterSearchType,
        this.matterSearchValue
      );
    },
    fillSelectedStageInfo() {
      if (this.StageInfoStore.selectedStageInfo) {
        const entity = this.StageInfoStore.selectedStageInfo;
        console.log("entity", entity);
        this.formData = {
          stageInfoID: entity.stageInfoID || "",
          from: entity.from || "",
          to: entity.to || "",
          stageID: entity.stageID || "",
          matterID: entity.matterID || "",
          uniteType: entity.uniteType,
          progressType: entity.progressType,
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
    handleMatter(data, isFound) {
      if (isFound) {
        this.formData.matterID = data.sessionID;
        this.showMatterCard = true;
      } else {
        this.showMatterCard = false;
      }
    },
  },

  mounted() {
    this.fillSelectedStageInfo();
  },
  watch: {
    stageData: {
      handler(data) {
        if (data && data.stageID != null) {
          this.handleStage(data, true);
        } else {
          this.handleStage(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
    matterData: {
      handler(data) {
        if (data && data.sessionID != null) {
          this.handleMatter(data, true);
        } else {
          this.handleMatter(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
