<template>
  <div>
    <GenericFilterComp
      :fields="studentFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <StudentList
      :-students="StudentStore.students"
      @show-delete-student="OnDelete"
      @show-info-student="onShowInfo"
      @show-update-student="OnUpsertPerson"
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
        :model-value="Model"
        :person-data="PersonStore.selectedPerson"
        @on-submit="OnSubmitUpsert"
        @search-student="onSearchStudent"
        @updatemodel-value="fillStudent"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <StudentCard :-data-req="StudentStore.selectedStudent" />
    </PopapComp>
    <button @click="showUpsert = true">اضافة مدينة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
import MessageComp from "@/components/general/messageComp.vue";
import StudentCard from "@/components/Person/Student/StudentCard.vue";
import StudentList from "@/components/Person/Student/StudentList.vue";
import UpsertStudentComp from "@/components/Person/Student/UpsertStudentComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
export default {
  name: "StudentView",
  components: {
    StudentList,
    UpsertStudentComp,
    StudentCard,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      PersonStore: usePersonStore(),
      StudentStore: useStudentStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      Model: {
        personID: "",
        studentID: "",
      },
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
          key: "BirthDate",
          label: "من تاريخ ميلاد",
          type: "date",
          operator: ">=",
        },
        {
          key: "BirthDate",
          label: "إلى تاريخ ميلاد",
          type: "date",
          operator: "<=",
        },
      ],
    };
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
      await this.StudentStore.fetchStudents();
    },
    async OnUpsertPerson(e) {
      if (e) {
        this.StudentStore.selectedStudent = e;
        this.Model.personID = e.personID;
        this.Model.studentID = e.studentID;
        this.PersonStore.selectedPerson = e;
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = false;
      if (e.studentID) {
        resVal = await this.StudentStore.updateStudent(e);
      } else {
        resVal = await this.StudentStore.addStudent(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal;
        this.resultType = true;
      }
    },
    async onSearchStudent(type, val) {
      await this.PersonStore.fetchPersonBy({
        Param: val,
        ParamType: type,
      });
    },
    async OnDelete(e) {
      this.StudentStore.DeleteStudent(e.id);
    },
    async onShowInfo(e) {
      this.StudentStore.selectedStudent = this.StudentStore.students.find(
        (p) => p.id === e.id
      );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.StudentStore.selectedStudent = null;
    },
    fillStudent(field) {
      this.Model = field;
    },
  },
  mounted() {
    this.StudentStore.fetchStudents();
  },
};
</script>

<style scoped></style>
