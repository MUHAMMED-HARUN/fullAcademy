<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-group">
        <label>اسم المجموعة:</label>
        <input v-model="formData.groupName" type="text" />
        <ValidationError :messages="errors?.groupName" />
      </div>
      <div class="form-group">
        <label>كود المجموعة:</label>
        <input v-model="formData.code" type="text" />
        <ValidationError :messages="errors?.code" />
      </div>

      <div class="form-group">
        <label>صف المعهد:</label>
        <GenericSearchByFilter
          :filter-types="filterTypes"
          :results="searchResults"
          id-field="instituteClassID"
          name-field="instituteName"
          placeholder="ابحث عن صف معهد"
          :loading="isSearching"
          v-model="formData.instituteClassID"
          v-model:name-value="instituteClassName"
          @search="onSearch"
          @select="onSelected"
          @clear="clearInstituteClassSelection"
        />
        <ValidationError :messages="errors?.instituteClassID" />
        <InstituteClassCard
          v-if="showCard"
          :data-req="selectedInstituteClass"
        />
      </div>

      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import GenericSearchByFilter from "@/components/general/GenericSearchByFilter.vue";
import InstituteClassCard from "../InstituteClass/InstituteClassCard.vue";
import { useGroupStore } from "@/store/Institute/GroupStore.js";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";

export default {
  name: "UpsertGroupComp",
  components: {
    ValidationError,
    GenericSearchByFilter,
    InstituteClassCard,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    instituteClassData: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      GroupStore: useGroupStore(),
      InstituteClassStore: useInstituteClassStore(),
      formData: {
        groupID: "",
        groupName: "",
        code: "",
        instituteClassID: "",
      },
      filterTypes: [{ value: 3, label: "بحث باسم الصف" }],
      searchResults: [],
      instituteClassName: "",
      selectedInstituteClass: null,
      isSearching: false,
      showCard: false,
    };
  },
  emits: ["onSubmit", "onSearchInstituteClass"],
  computed: {
    FormTitle() {
      return this.formData.groupID ? "تعديل مجموعة" : "إضافة مجموعة";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    async onSearch(payload) {
      this.isSearching = true;
      await this.InstituteClassStore.fetchInstituteClasses();
      const q = String(payload.value || "").toLowerCase();
      this.searchResults = (
        this.InstituteClassStore.instituteClasses || []
      ).filter(
        (ic) =>
          String(ic.instituteName || "")
            .toLowerCase()
            .includes(q) ||
          String(ic.className || "")
            .toLowerCase()
            .includes(q)
      );
      this.isSearching = false;
    },
    onSelected(item) {
      this.formData.instituteClassID = item.instituteClassID;
      this.instituteClassName = item.instituteName || item.className;
      this.selectedInstituteClass = item;
      this.showCard = true;
    },
    clearInstituteClassSelection() {
      this.selectedInstituteClass = null;
      this.showCard = false;
    },
    fillSelectedGroup() {
      if (this.GroupStore.selectedGroup) {
        const entity = this.GroupStore.selectedGroup;
        this.formData = {
          groupID: entity.groupID || "",
          groupName: entity.groupName || "",
          code: entity.code || "",
          instituteClassID: entity.instituteClassID || "",
        };
      }
    },
    handle(data, isFound) {
      if (isFound) {
        this.formData.instituteClassID = data.instituteClassID;
        this.selectedInstituteClass = data;
        this.showCard = true;
      } else {
        this.selectedInstituteClass = null;
        this.showCard = false;
      }
    },
  },

  mounted() {
    this.fillSelectedGroup();
  },
  watch: {
    instituteClassData: {
      handler(data) {
        if (data && data.instituteClassID != null) {
          this.handle(data, true);
        } else {
          this.handle(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
