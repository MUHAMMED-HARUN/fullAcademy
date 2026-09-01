<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <div class="form-filter">
        <label>نوع البحث</label>
        <select v-model="searchType">
          <option selected :value="this.enSearchType.InstituteName">
            بحث باسم المعهد
          </option>
          <option disabled :value="enSearchType.InstituteID">
            بحث بـ ID المعهد
          </option>
        </select>
        <input v-model="searchValue" type="text" />
        <button @click.prevent.stop="onSearchInstitute">بحث</button>
      </div>

      <div class="Institut-card">
        <InstituteCard v-if="showInstituteCard" :-data-req="InstituteData" />
        <ValidationError :messages="errors?.instituteID" />
      </div>
      <div class="class-card">
        <ClassCard v-if="showClassCard" :-data-req="ClassData" />
        <ValidationError :messages="errors?.classID" />
      </div>
      <div class="form-group">
        <button type="submit">حفظ</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore.js";
import ClassCard from "../Class/ClassCard.vue";
import InstituteCard from "../Institute/InstituteCard.vue";
const enSearchType = Object.freeze({ InstituteID: 0, InstituteName: 1 });

export default {
  name: "UpsertInstituteClassComp",
  components: {
    ValidationError,
    ClassCard,
    InstituteCard,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    InstituteData: {
      type: Object,
      default: () => ({}),
    },
    ClassData: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      InstituteClassStore: useInstituteClassStore(),
      formData: {
        instituteClassID: "",
        classID: "",
        instituteID: "",
      },
      enSearchType: enSearchType,
      searchType: enSearchType.nationalNumber,
      searchValue: "",
      showClassCard: false,
      showInstituteCard: false,
    };
  },
  emits: ["onSubmit", "onSearchInstituteClass"],
  computed: {
    FormTitle() {
      return this.formData.instituteClassID ? "تعديل " : "إضافة ";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onSearchInstitute() {
      this.$emit("onSearchInstitute", this.searchType, this.searchValue);
    },

    fillSelectedInstituteClass() {
      if (this.InstituteClassStore.selectedInstituteClass) {
        const entity = this.InstituteClassStore.selectedInstituteClass;
        this.formData = {
          instituteClassID: entity.instituteClassID || "",
          classID: entity.classID || "",
          instituteID: entity.instituteID || "",
        };
      }
    },
    handleClassData(data, isFound) {
      if (isFound) {
        this.formData.classID = data.classID;
        this.showClassCard = true;
      } else {
        this.showClassCard = false;
      }
    },
    handleInstituteData(data, isFound) {
      if (isFound) {
        this.formData.instituteID = data.instituteID;
        this.showInstituteCard = true;
      } else {
        this.showInstituteCard = false;
      }
    },
  },

  mounted() {
    this.fillSelectedInstituteClass();
  },
  watch: {
    ClassData: {
      // التصحيح: الاسم يجب أن يكون handler
      handler(data) {
        if (data?.classID != null) {
          // إضافة فحص للـ null للكائن نفسه
          this.handleClassData(data, true);
        } else {
          this.handleClassData(null, false);
        }
      },
      immediate: true,
      deep: true, // يفضل إضافة deep إذا كان الكائن يتغير داخلياً
    },
    InstituteData: {
      handler(data) {
        console.log("data institue", data);
        if (data?.instituteID != null) {
          this.handleInstituteData(data, true);
        } else {
          this.handleInstituteData(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>
