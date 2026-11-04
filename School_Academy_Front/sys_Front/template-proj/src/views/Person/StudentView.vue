<template>
  <section class="space-y-5" dir="rtl" aria-label="إدارة الطلاب">
    <header class="flex flex-wrap items-center justify-between gap-3">
      <div>
        <h1 class="text-2xl font-bold text-[var(--primary-color)]">الطلاب</h1>
        <p class="text-sm text-[var(--text-muted)]">
          إدارة سجلات الطلاب والبحث ضمن النتائج.
        </p>
      </div>
      <button
        class="btn btn-primary"
        :disabled="StudentStore.saving"
        @click="OnAddStudent"
      >
        إضافة طالب
      </button>
    </header>
    <GenericFilterComp
      :fields="studentFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <StudentList
      :students="StudentStore.students"
      :selected-ids="selectedStudentIds"
      :loading="StudentStore.loading"
      :error="StudentStore.listError"
      @show-delete-student="OnDelete"
      @show-info-student="onShowInfo"
      @show-update-student="OnUpsertPerson"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
    />
    <GenericComboActions
      :actions="studentActions"
      :selected-items="selectedStudents"
      @action-click="handleComboAction"
    />
    <!-- 🔹 إضافة مكون التصفح هنا -->
    <PaginationComp
      :current-page="StudentStore.page"
      :page-size="StudentStore.pageSize"
      :total-count="StudentStore.totalCount"
      :total-pages="StudentStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertStudentComp
        :errors="StudentStore.error"
        v-model="Model"
        :person-data="PersonStore.selectedPerson"
        :person-search-results="PersonStore.personSearchResults"
        :person-search-loading="PersonStore.personSearchLoading"
        :person-search-error="PersonStore.personSearchError"
        :saving="StudentStore.saving"
        @submit="OnSubmitUpsert"
        @search-person="onSearchPerson"
        @select-person="onSelectPerson"
        @clear-person="onClearPerson"
        @person-query-change="onPersonQueryChange"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <StudentCard :data-req="StudentStore.selectedStudent" />
    </PopapComp>
    <PopapComp v-if="showUpsertEnrollment" @close="onpopaCloseEnrollment">
      <UpsertStudentEnrollmentComp
        :errors="StudentEnrollmentStore.error"
        :student-data="StudentStore.selectedStudent"
        :group-data="GroupStore.selectedGroup"
        :group-results="GroupStore.groups"
        @on-submit="OnSubmitEnrollment"
        @on-search-student="onSearchStudentEnrollment"
        @on-search-group="onSearchGroupEnrollment"
        @update:group-data="onGroupDataChanged"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </section>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
import { useStudentEnrollmentStore } from "@/store/Enrollment/StudentEnrollmentStore";
import { useGroupStore } from "@/store/Institute/GroupStore";
import MessageComp from "@/components/general/messageComp.vue";
import StudentCard from "@/components/Person/Student/StudentCard.vue";
import StudentList from "@/components/Person/Student/StudentList.vue";
import UpsertStudentComp from "@/components/Person/Student/UpsertStudentComp.vue";
import UpsertStudentEnrollmentComp from "@/components/Enrollment/StudentEnrollment/UpsertStudentEnrollmentComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import TResult from "@/Generic/TResult";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
export default {
  name: "StudentView",
  components: {
    StudentList,
    UpsertStudentComp,
    UpsertStudentEnrollmentComp,
    StudentCard,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    GenericComboActions,
    PaginationComp,
  },
  data() {
    return {
      PersonStore: usePersonStore(),
      StudentStore: useStudentStore(),
      StudentEnrollmentStore: useStudentEnrollmentStore(),
      GroupStore: useGroupStore(),
      showUpsert: false,
      showInfo: false,
      showUpsertEnrollment: false,
      resultMessage: null,
      resultType: false,
      Model: {
        personID: "",
        studentID: "",
      },
      selectedStudentIds: [],
      studentActions: [
        {
          key: "info",
          label: "معلومات",
          minSelection: 1,
          maxSelection: 1,
          class:
            "bg-[var(--secondary-color)] hover:bg-[var(--secondary-color-hover)]",
        },
        {
          key: "update",
          label: "تحديث",
          minSelection: 1,
          maxSelection: 1,
          class:
            "bg-[var(--primary-color)] hover:bg-[var(--primary-color-hover)]",
        },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class:
            "bg-[var(--danger-color)] hover:bg-[var(--danger-color-hover)]",
        },
        {
          key: "enrollStudent",
          label: "تسجيل طالب",
          minSelection: 1,
          maxSelection: 1,
          class:
            "bg-[var(--primary-color)] hover:bg-[var(--primary-color-hover)]",
        },
      ],
      studentFilterSchema: [
        {
          key: "FirstName",
          label: "الاسم الأول",
          type: "text",
          operator: "=*",
        },
        {
          key: "NationalNumber",
          label: "الرقم الوطني",
          type: "text",
          operator: "=*",
        },
        {
          key: "Gendor",
          label: "الجنس",
          type: "select",
          options: [
            { label: "ذكر", value: true },
            { label: "أنثى", value: false },
          ],
        },
        {
          key: "BirthDateFrom",
          label: "من تاريخ ميلاد",
          type: "date",
          operator: ">=",
        },
        {
          key: "BirthDateTo",
          label: "إلى تاريخ ميلاد",
          type: "date",
          operator: "<=",
        },
      ],
    };
  },
  computed: {
    selectedStudents() {
      return this.StudentStore.students.filter((s) =>
        this.selectedStudentIds.includes(s.studentID || s.id)
      );
    },
  },
  methods: {
    async onPageChange(newPage) {
      await this.StudentStore.setPage(newPage);
    },
    async onPageSizeChange(newSize) {
      await this.StudentStore.setPageSize(newSize);
    },
    async onFilterSearch(payload) {
      // payload يحتوي على القيمة الجاهزة للـ filter الخاص بـ Gridify
      // مثال: payload.filter = "FirstName=*احمد,Gender=true"
      await this.StudentStore.fetchStudents({
        Filter: payload.filter,
        OrderBy: payload.orderBy,
      });
    },
    async onFilterReset() {
      await this.StudentStore.fetchStudents({ Filter: "", OrderBy: "" });
    },
    OnAddStudent() {
      this.Model = { personID: "", studentID: "" };
      this.StudentStore.selectedStudent = null;
      this.StudentStore.error = null;
      this.PersonStore.resetPersonCandidateSearch();
      this.showUpsert = true;
    },
    async OnUpsertPerson(student) {
      const studentID = student?.studentID;
      if (!studentID) return;

      this.PersonStore.resetPersonCandidateSearch();
      const result = await this.StudentStore.fetchStudentBy({
        Param: studentID,
        ParamType: 1,
      });
      if (!result.success || !result.data?.personID) {
        this.resultMessage = this.formatMessages(
          result.errors || "تعذر تحميل بيانات الطالب والشخص المرتبط به"
        );
        this.resultType = true;
        return;
      }

      this.Model = {
        studentID: result.data.studentID,
        personID: result.data.personID,
      };
      this.PersonStore.selectPersonCandidate(result.data);
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      const result = e.studentID
        ? await this.StudentStore.updateStudent(e)
        : await this.StudentStore.addStudent(e);
      if (result.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.onpopaClose();
      } else {
        this.resultMessage = this.formatMessages(result.errors);
        this.resultType = true;
      }
    },
    async onSearchPerson(criteria) {
      await this.PersonStore.searchPersonCandidates(criteria);
    },
    onSelectPerson(person) {
      this.PersonStore.selectPersonCandidate(person);
    },
    onClearPerson() {
      this.PersonStore.clearSelectedPersonCandidate();
    },
    onPersonQueryChange() {
      this.PersonStore.resetPersonCandidateSearch();
    },
    formatMessages(value) {
      const flatten = (item) => {
        if (typeof item === "string") return [item];
        if (Array.isArray(item)) return item.flatMap(flatten);
        if (item && typeof item === "object") {
          return Object.values(item).flatMap(flatten);
        }
        return [];
      };
      return flatten(value).join("، ") || "حدث خطأ غير متوقع";
    },
    async OnDelete(e) {
      if (!e?.studentID) return;
      const result = await this.StudentStore.deleteStudent(e.studentID);
      this.resultMessage = result.success
        ? "تم حذف الطالب بنجاح"
        : this.formatMessages(result.errors);
      this.resultType = !result.success;
      this.selectedStudentIds = this.selectedStudentIds.filter(
        (id) => id !== e.studentID
      );
    },
    async onShowInfo(e) {
      this.StudentStore.selectedStudent = e;
      this.showInfo = true;
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          await this.onShowInfo(singleItem);
          break;
        case "update":
          await this.OnUpsertPerson(singleItem);
          break;
        case "delete":
          await this.OnDelete(singleItem);
          this.selectedStudentIds = [];
          break;
        case "enrollStudent":
          await this.OnEnrollStudent(singleItem);
          break;
      }
    },
    async OnEnrollStudent(student) {
      this.StudentStore.selectedStudent = student;
      this.GroupStore.selectedGroup = null;
      this.GroupStore.fetchGroups();
      this.showUpsertEnrollment = true;
    },
    async OnSubmitEnrollment(e) {
      var resVal = new TResult();
      if (Array.isArray(e.studentIDs) && e.studentIDs.length > 0) {
        resVal = await this.StudentEnrollmentStore.enrollByRange({
          StudentIDs: e.studentIDs,
          GroupID: e.groupID,
          Date: e.date,
        });
      } else if (e.enrollmentID) {
        resVal = await this.StudentEnrollmentStore.updateEnrollment(e);
      } else {
        resVal = await this.StudentEnrollmentStore.addEnrollment(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ بنجاح";
        this.resultType = false;
        this.showUpsertEnrollment = false;
      } else {
        this.resultMessage = resVal.errors || resVal;
        this.resultType = true;
      }
    },
    async onSearchStudentEnrollment(type, val) {
      await this.StudentStore.fetchStudentBy({
        Param: val,
        ParamType: type,
      });
    },
    async onSearchGroupEnrollment(type, val) {
      let filter = "";
      if (type === 1) filter = `GroupName=*${val}`;
      else if (type === 2) filter = `Code=*${val}`;
      else filter = `GroupID=${val}`;
      await this.GroupStore.fetchGroups({ Filter: filter });
    },
    onGroupDataChanged(group) {
      this.GroupStore.selectedGroup = group;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.StudentStore.clearSelectedStudent();
      this.Model = { studentID: "", personID: "" };
      this.PersonStore.resetPersonCandidateSearch();
    },
    onpopaCloseEnrollment() {
      this.showUpsertEnrollment = false;
      this.StudentStore.selectedStudent = null;
      this.GroupStore.selectedGroup = null;
    },
    onToggleSelectAll(isChecked) {
      this.selectedStudentIds = isChecked
        ? this.StudentStore.students.map((s) => s.studentID || s.id)
        : [];
    },
    onToggleSelectRow(item) {
      const id = item.studentID || item.id;
      const index = this.selectedStudentIds.indexOf(id);
      if (index > -1) this.selectedStudentIds.splice(index, 1);
      else this.selectedStudentIds.push(id);
    },
  },
  mounted() {
    this.StudentStore.fetchStudents();
  },
};
</script>

<style scoped></style>
