<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <!-- Student Search Section -->
      <div class="form-filter">
        <label>البحث عن طالب:</label>
        <select v-model="studentSearchType">
          <option :value="enStudentSearchType.NationalNumber">
            بحث بالرقم الوطني
          </option>
          <option :value="enStudentSearchType.ID">بحث بـ ID الطالب</option>
        </select>
        <input v-model="studentSearchValue" type="text" />
        <button @click.prevent.stop="onSearchStudent">بحث عن طالب</button>
      </div>

      <!-- Selected students list -->
      <div class="selected-students mt-2">
        <div
          v-for="(s, idx) in selectedStudents"
          :key="s.studentID || s.id || idx"
          class="flex items-center justify-between rounded-lg border border-gray-200 bg-gray-50 px-3 py-2 text-sm dark:border-gray-800 dark:bg-white/5"
        >
          <span>{{ s.firstName }} {{ s.fatherName }} {{ s.lastName }}</span>
          <button
            type="button"
            class="text-xs font-medium text-red-500 hover:text-red-600"
            @click="removeStudent(idx)"
          >
            إزالة
          </button>
        </div>
        <ValidationError :messages="errors?.studentID" />
      </div>

      <!-- Group Search Section (SearchFilterCombo) -->
      <div class="form-group mt-4">
        <label>البحث عن مجموعة:</label>
        <SearchFilterCombo
          :filter-types="groupFilterTypes"
          :results="groupResults"
          id-field="groupID"
          name-field="groupName"
          v-model="formData.groupID"
          :name-value="groupNameValue"
          @update:name-value="groupNameValue = $event"
          @search="onSearchGroup"
          @select="onSelectGroup"
          @clear="onClearGroup"
        />
        <ValidationError :messages="errors?.groupID" />
      </div>

      <div class="group-card-wrapper" v-if="showGroupCard">
        <GroupCard :data-req="selectedGroupData" />
      </div>

      <!-- Date Input Section -->
      <div class="form-group mt-4">
        <label>تاريخ التسجيل:</label>
        <input v-model="formData.date" type="date" />
        <ValidationError :messages="errors?.date" />
      </div>

      <div class="form-group">
        <button type="submit">حفظ التسجيل</button>
      </div>
    </form>
  </div>
</template>

<script>
import ValidationError from "@/components/general/ValidationError.vue";
import SearchFilterCombo from "@/components/general/SearchFilterCombo.vue";
import { useStudentEnrollmentStore } from "@/store/Enrollment/StudentEnrollmentStore.js";

import GroupCard from "@/components/Institute/Group/GroupCard.vue";

const enStudentSearchType = Object.freeze({ ID: 1, NationalNumber: 3 });
const enGroupSearchType = Object.freeze({ GroupName: 1, Code: 2, ID: 0 });

export default {
  name: "UpsertStudentEnrollmentComp",
  components: {
    ValidationError,
    SearchFilterCombo,
    GroupCard,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    studentData: {
      type: Object,
      default: () => ({}),
    },
    groupData: {
      type: Object,
      default: () => ({}),
    },
    groupResults: {
      type: Array,
      default: () => [],
    },
  },
  data() {
    return {
      StudentEnrollmentStore: useStudentEnrollmentStore(),
      formData: {
        enrollmentID: "",
        studentID: "",
        groupID: null,
        date: new Date().toISOString().substring(0, 10),
      },
      enStudentSearchType: enStudentSearchType,
      studentSearchType: enStudentSearchType.NationalNumber,
      studentSearchValue: "",
      selectedStudents: [],

      groupFilterTypes: [
        { value: enGroupSearchType.GroupName, label: "بحث باسم المجموعة" },
        { value: enGroupSearchType.Code, label: "بحث بكود المجموعة" },
        { value: enGroupSearchType.ID, label: "بحث بـ ID المجموعة" },
      ],
      groupNameValue: "",
      selectedGroupData: null,
      showGroupCard: false,
    };
  },
  emits: ["onSubmit", "onSearchStudent", "onSearchGroup", "update:groupData"],
  computed: {
    FormTitle() {
      return this.formData.enrollmentID
        ? "تعديل تسجيل الطالب"
        : "تسجيل طالب/طلاب في مجموعة";
    },
  },
  methods: {
    onSubmit() {
      const payload = {
        studentIDs: this.selectedStudents.map(
          (s) => s.studentID || s.id || s.StudentID
        ),
        groupID: this.formData.groupID,
        date: this.formData.date,
      };
      // fallback for single edit mode
      if (this.formData.enrollmentID) {
        payload.enrollmentID = this.formData.enrollmentID;
        payload.studentID = this.formData.studentID;
      }
      this.$emit("onSubmit", payload);
    },
    onSearchStudent() {
      this.$emit(
        "onSearchStudent",
        this.studentSearchType,
        this.studentSearchValue
      );
    },
    onSearchGroup(payload) {
      this.$emit("onSearchGroup", payload.filterType, payload.value);
    },
    onSelectGroup(item) {
      this.formData.groupID = item.groupID;
      this.groupNameValue = item.groupName;
      this.selectedGroupData = item;
      this.showGroupCard = true;
      this.$emit("update:groupData", item);
    },
    onClearGroup() {
      this.selectedGroupData = null;
      this.groupNameValue = "";
      this.showGroupCard = false;
      this.$emit("update:groupData", null);
    },
    removeStudent(idx) {
      this.selectedStudents.splice(idx, 1);
    },
    fillSelectedEnrollment() {
      if (this.StudentEnrollmentStore.selectedEnrollment) {
        const entity = this.StudentEnrollmentStore.selectedEnrollment;
        this.formData = {
          enrollmentID: entity.enrollmentID || "",
          studentID: entity.studentID || "",
          groupID: entity.groupID || "",
          date: entity.date || new Date().toISOString().substring(0, 10),
        };
        if (entity.studentID) {
          this.selectedStudents = [
            { studentID: entity.studentID, firstName: entity.studentName },
          ];
        }
      }
    },
    handleStudent(data, isFound) {
      if (isFound && data) {
        const id = data.studentID || data.id || data.StudentID;
        if (
          !this.selectedStudents.some(
            (s) => (s.studentID || s.id || s.StudentID) === id
          )
        ) {
          this.selectedStudents.push(data);
        }
      }
    },
    handleGroup(data, isFound) {
      if (isFound) {
        this.formData.groupID = data.groupID || data.GroupID;
        this.groupNameValue = data.groupName || data.GroupName || "";
        this.selectedGroupData = data;
        this.showGroupCard = true;
      } else {
        this.selectedGroupData = null;
        this.showGroupCard = false;
      }
    },
  },
  mounted() {
    this.fillSelectedEnrollment();
  },
  watch: {
    studentData: {
      handler(data) {
        if (data && (data.id || data.studentID || data.StudentID)) {
          this.handleStudent(data, true);
        }
      },
      immediate: true,
      deep: true,
    },
    groupData: {
      handler(data) {
        if (data && (data.groupID || data.GroupID)) {
          this.handleGroup(data, true);
        } else {
          this.handleGroup(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
  },
};
</script>

<style scoped>
.mt-4 {
  margin-top: 1rem;
}
.selected-students {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}
</style>
