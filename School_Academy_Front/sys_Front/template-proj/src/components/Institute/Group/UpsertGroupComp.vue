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
      <div class="form-filter">
        <label>نوع البحث عن صف معهد</label>
        <select v-model="searchType">
          <option selected :value="enSearchType.ClassName">
            بحث باسم الصف
          </option>
          <option :value="enSearchType.ID">بحث بـ ID صف معهد</option>
        </select>
        <input v-model="searchValue" type="text" />
        <button @click.prevent.stop="onSearchInstituteClass">بحث</button>
      </div>

      <div class="institute-class-card">
        <InstituteClassCard v-if="showCard" :-data-req="instituteClassData" />
        <ValidationError :messages="errors?.instituteClassID" />
      </div>
      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useGroupStore } from "@/store/Institute/GroupStore.js";
import InstituteClassCard from "../InstituteClass/InstituteClassCard.vue";

const enSearchType = Object.freeze({ ClassName: 3, ID: 0 });

export default {
  name: "UpsertGroupComp",
  components: {
    ValidationError,
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
      formData: {
        groupID: "",
        groupName: "",
        code: "",
        instituteClassID: "",
      },
      enSearchType: enSearchType,
      searchType: enSearchType.ClassName,
      searchValue: "",
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
    onSearchInstituteClass() {
      this.$emit("onSearchInstituteClass", this.searchType, this.searchValue);
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
        this.showCard = true;
      } else {
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
        console.log("instituteClass Data in Group form", data);
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
