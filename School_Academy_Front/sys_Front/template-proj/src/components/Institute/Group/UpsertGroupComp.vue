<template>
  <section dir="rtl" class="space-y-4">
    <h2 class="text-lg font-semibold">
      {{ formData.groupID ? "تعديل مجموعة" : "إضافة مجموعة" }}
    </h2>
    <form class="space-y-4" @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم المجموعة:</label>
        <input v-model="formData.groupName" type="text" maxlength="150" />
        <ValidationError :messages="errors?.groupName" />
      </div>
      <div class="form-group">
        <label>كود المجموعة:</label>
        <input v-model="formData.code" type="text" maxlength="50" />
        <ValidationError :messages="errors?.code" />
      </div>

      <div class="form-group">
        <label>صف المعهد:</label>
        <GenericSearchByFilter
          :model-value="formData.instituteClassID"
          :name-value="instituteClassName"
          :results="instituteClassResults"
          :filter-types="filterTypes"
          id-field="instituteClassID"
          name-field="displayName"
          placeholder="ابحث باسم الصف أو المعهد"
          :loading="instituteClassLoading"
          :error="instituteClassError"
          :disabled="!!groupData?.hasActiveRelationships"
          @update:name-value="instituteClassName = $event"
          @search="$emit('onSearchInstituteClass', $event)"
          @select="onSelected"
          @clear="clearInstituteClassSelection"
        />
        <ValidationError :messages="errors?.instituteClassID" />
        <p
          v-if="groupData?.hasActiveRelationships"
          class="mt-1 text-sm text-[var(--text-muted)]"
          role="status"
        >
          لا يمكن تغيير الصف لوجود عضويات طلاب أو تكليفات تدريس نشطة.
        </p>
        <InstituteClassCard
          v-if="showCard && selectedInstituteClass"
          :data-req="selectedInstituteClass"
        />
      </div>

      <button type="submit" class="btn btn-primary" :disabled="isSaving">
        {{ isSaving ? "جارٍ الحفظ" : "حفظ" }}
      </button>
    </form>
  </section>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import InstituteClassCard from "../InstituteClass/InstituteClassCard.vue";

export default {
  name: "UpsertGroupComp",
  components: { ValidationError, GenericSearchByFilter, InstituteClassCard },
  props: {
    errors: { type: [Object, Array, String], default: () => ({}) },
    groupData: { type: Object, default: null },
    instituteClassData: { type: Object, default: null },
    instituteClassResults: { type: Array, default: () => [] },
    instituteClassLoading: { type: Boolean, default: false },
    instituteClassError: { type: [String, Array, Object], default: null },
    isSaving: { type: Boolean, default: false },
  },
  emits: ["onSubmit", "onSearchInstituteClass"],
  data() {
    return {
      formData: {
        groupID: "",
        groupName: "",
        code: "",
        instituteClassID: "",
      },
      filterTypes: [
        { value: "ClassName", label: "اسم الصف" },
        { value: "InstituteName", label: "اسم المعهد" },
      ],
      instituteClassName: "",
      selectedInstituteClass: null,
      showCard: false,
    };
  },
  watch: {
    groupData: {
      immediate: true,
      handler(group) {
        this.formData = {
          groupID: group?.groupID || "",
          groupName: group?.groupName || "",
          code: group?.code || "",
          instituteClassID: group?.instituteClassID || "",
        };
        if (!group) this.clearInstituteClassSelection();
      },
    },
    instituteClassData: {
      immediate: true,
      handler(item) {
        if (!item?.instituteClassID) {
          if (!this.formData.groupID) this.clearInstituteClassSelection();
          return;
        }
        this.formData.instituteClassID = item.instituteClassID;
        this.instituteClassName =
          item.displayName || `${item.className} — ${item.instituteName}`;
        this.selectedInstituteClass = item;
        this.showCard = true;
      },
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", { ...this.formData });
    },
    onSelected(item) {
      this.formData.instituteClassID = item.instituteClassID;
      this.instituteClassName = item.displayName;
      this.selectedInstituteClass = item;
      this.showCard = true;
    },
    clearInstituteClassSelection() {
      this.formData.instituteClassID = "";
      this.instituteClassName = "";
      this.selectedInstituteClass = null;
      this.showCard = false;
    },
  },
};
</script>
