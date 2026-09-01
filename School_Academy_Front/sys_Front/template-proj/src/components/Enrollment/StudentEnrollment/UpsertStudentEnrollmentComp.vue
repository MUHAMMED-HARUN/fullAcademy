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

      <div class="student-card-wrapper">
        <StudentCard v-if="showStudentCard" :-data-req="studentData" />
        <ValidationError :messages="errors?.studentID" />
      </div>

      <!-- Group Search Section -->
      <div v-if="!showGroupList">
        <div class="form-filter mt-4">
          <label>البحث عن مجموعة:</label>
          <select v-model="groupSearchType">
            <option :value="enGroupSearchType.GroupName">
              بحث باسم المجموعة
            </option>
            <option :value="enGroupSearchType.Code">بحث بكود المجموعة</option>
            <option :value="enGroupSearchType.ID">بحث بـ ID المجموعة</option>
          </select>
          <input v-model="groupSearchValue" type="text" />
          <button @click.prevent.stop="onSearchGroup">بحث عن مجموعة</button>
        </div>

        <div class="group-card-wrapper">
          <GroupCard v-if="showGroupCard" :-data-req="groupData" />
          <ValidationError :messages="errors?.groupID" />
        </div>
      </div>

      <div v-if="showGroupList">
        <div class="form-filter mt-4">
          <label>البحث عن مجموعة:</label>
          <select v-model="formData.groupID">
            <option disabled selected>اختر مجموعة</option>
            <option
              v-for="g in teacherGroupList"
              :key="g.groupID"
              :value="g.groupID"
            >
              {{ g.groupName }}
            </option>
          </select>
        </div>

        <div class="group-card-wrapper">
          <GroupCard v-if="showGroupCard" :-data-req="groupData" />
          <ValidationError :messages="errors?.groupID" />
        </div>
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
import { useStudentEnrollmentStore } from "@/store/Enrollment/StudentEnrollmentStore.js";
import StudentCard from "@/components/Person/Student/StudentCard.vue";
import GroupCard from "@/components/Institute/Group/GroupCard.vue";

const enStudentSearchType = Object.freeze({ ID: 1, NationalNumber: 3 });
const enGroupSearchType = Object.freeze({ ID: 0, GroupName: 1, Code: 2 });

export default {
  name: "UpsertStudentEnrollmentComp",
  components: {
    ValidationError,
    StudentCard,
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
    teacherGroupList: {
      type: Object,
      default: () => ({}),
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
      showStudentCard: false,

      enGroupSearchType: enGroupSearchType,
      groupSearchType: enGroupSearchType.GroupName,
      groupSearchValue: "",
      showGroupCard: false,
    };
  },
  emits: ["onSubmit", "onSearchStudent", "onSearchGroup"],
  computed: {
    showGroupList() {
      return (
        Array.isArray(this.teacherGroupList) && this.teacherGroupList.length > 0
      );
    },
    FormTitle() {
      return this.formData.enrollmentID
        ? "تعديل تسجيل الطالب"
        : "تسجيل طالب جديد في مجموعة";
    },
  },
  methods: {
    onSubmit() {
      this.$emit("onSubmit", this.formData);
    },
    onSearchStudent() {
      this.$emit(
        "onSearchStudent",
        this.studentSearchType,
        this.studentSearchValue
      );
    },
    onSearchGroup() {
      this.$emit("onSearchGroup", this.groupSearchType, this.groupSearchValue);
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
      }
    },
    handleStudent(data, isFound) {
      if (isFound) {
        this.formData.studentID = data.studentID;
        this.showStudentCard = true;
      } else {
        this.showStudentCard = false;
      }
    },
    handleGroup(data, isFound) {
      if (isFound) {
        this.formData.groupID = data.groupID;
        this.showGroupCard = true;
      } else {
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
        console.log("student Data in Enrollment form", data);
        if (data && (data.id || data.studentID) != null) {
          this.handleStudent(data, true);
        } else {
          this.handleStudent(null, false);
        }
      },
      immediate: true,
      deep: true,
    },
    groupData: {
      handler(data) {
        if (data && data.groupID != null) {
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
</style>
