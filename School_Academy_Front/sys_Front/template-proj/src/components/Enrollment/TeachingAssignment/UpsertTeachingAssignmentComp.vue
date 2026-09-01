<template>
  <div>
    <h2>{{ FormTitle }}</h2>
    <form @submit.prevent="onSubmit">
      <!-- Student Search Section -->
      <div class="form-filter">
        <label>البحث عن معلم:</label>
        <select v-model="teacherSearchType">
          <option :value="enTeacherSearchType.NationalNumber">
            بحث بالرقم الوطني
          </option>
          <option :value="enTeacherSearchType.ID">بحث بـ ID المعلم</option>
        </select>
        <input v-model="teacherSearchValue" type="text" />
        <button @click.prevent.stop="onSearchTeacher">بحث عن معلم</button>
      </div>

      <div class="teacher-card-wrapper">
        <TeacherCard v-if="showTeacherCard" :-data-req="teacherData" />
        <ValidationError :messages="errors?.teacherID" />
      </div>

      <!-- Group Search Section -->
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

      <div class="form-filter mt-4">
        <label>البحث عن المادة:</label>
        <select v-model="subjectSearchType">
          <option :value="enSubjectSearchType.SubjectName">
            بحث باسم المادة
          </option>
        </select>
        <input v-model="subjectSearchValue" type="text" />
        <button @click.prevent.stop="onSearchSubject">بحث عن مادة</button>
      </div>

      <div class="subject-card-wrapper">
        <SessionCard v-if="showSubjectCard" :-data-req="subjectData" />
        <ValidationError :messages="errors?.subjectID" />
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
import { useTeachingAssignmentStore } from "@/store/Enrollment/TeachingAssignmentStore.js";
import GroupCard from "@/components/Institute/Group/GroupCard.vue";
import SessionCard from "@/components/Progress/Session/SessionCard.vue";
import TeacherCard from "@/components/Person/Teacher/TeacherCard.vue";

const enTeacherSearchType = Object.freeze({ ID: 1, NationalNumber: 3 });
const enGroupSearchType = Object.freeze({ ID: 0, GroupName: 1, Code: 2 });
const enSubjectSearchType = Object.freeze({ ID: 0, SubjectName: 1 });

export default {
  name: "UpsertTeachingAssignmentComp",
  components: {
    ValidationError,
    TeacherCard,
    GroupCard,
    SessionCard,
  },
  props: {
    errors: {
      type: Object,
      default: () => ({}),
    },
    teacherData: {
      type: Object,
      default: () => ({}),
    },
    groupData: {
      type: Object,
      default: () => ({}),
    },
    subjectData: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      TeachingAssignmentStore: useTeachingAssignmentStore(),
      formData: {
        enrollmentID: "",
        teacherID: "",
        groupID: "",
        subjectID: "",
        date: new Date().toISOString().substring(0, 10),
      },
      teacherSearchType: enTeacherSearchType.NationalNumber,
      enTeacherSearchType: enTeacherSearchType,
      teacherSearchValue: "",
      showTeacherCard: false,

      enGroupSearchType: enGroupSearchType,
      groupSearchType: enGroupSearchType.GroupName,
      groupSearchValue: "",
      showGroupCard: false,

      enSubjectSearchType: enSubjectSearchType,
      subjectSearchType: enSubjectSearchType.SubjectName,
      subjectSearchValue: "",
      showSubjectCard: false,
    };
  },
  emits: ["onSubmit", "onSearchTeacher", "onSearchGroup", "onSearchSubject"],
  computed: {
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
    onSearchTeacher() {
      this.$emit(
        "onSearchTeacher",
        this.teacherSearchType,
        this.teacherSearchValue
      );
    },
    onSearchGroup() {
      this.$emit("onSearchGroup", this.groupSearchType, this.groupSearchValue);
    },
    onSearchSubject() {
      this.$emit(
        "onSearchSubject",
        this.subjectSearchType,
        this.subjectSearchValue
      );
    },
    fillSelectedEnrollment() {
      if (this.TeachingAssignmentStore.selectedEnrollment) {
        const entity = this.TeachingAssignmentStore.selectedEnrollment;
        this.formData = {
          enrollmentID: entity.enrollmentID || "",
          teacherID: entity.teacherID || "",
          groupID: entity.groupID || "",
          subjectID: entity.subjectID || "",
          date: entity.date || new Date().toISOString().substring(0, 10),
        };
      }
    },
    handleTeacher(data, isFound) {
      if (isFound) {
        this.formData.teacherID = data.TeacherID;
        this.showTeacherCard = true;
      } else {
        this.showTeacherCard = false;
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
    handleSubject(data, isFound) {
      if (isFound) {
        alert(data.sessionID);
        this.formData.subjectID = data.sessionID;
        this.showSubjectCard = true;
      } else {
        this.showSubjectCard = false;
      }
    },
  },

  mounted() {
    this.fillSelectedEnrollment();
  },
  watch: {
    teacherData: {
      handler(data) {
        if (data && (data.teacherID || data.TeacherID) != null) {
          this.handleTeacher(data, true);
        } else {
          this.handleTeacher(null, false);
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
    subjectData: {
      handler(data) {
        if (data && (data.subjectID || data.sessionID) != null) {
          this.handleSubject(data, true);
        } else {
          this.handleSubject(null, false);
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
