<template>
  <div>
    <!-- 🔹 فلتر البحث الشامل -->
    <GenericFilterComp
      :fields="teacherFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <TeacherList
      :Teachers="TeacherStore.Teachers"
      @show-delete-Teacher="OnDelete"
      @show-info-Teacher="onShowInfo"
      @show-update-Teacher="OnUpsertPerson"
    />

    <!-- 🔹 مكون التصفح -->
    <PaginationComp
      :current-page="TeacherStore.page"
      :page-size="TeacherStore.pageSize"
      :total-count="TeacherStore.totalCount"
      :total-pages="TeacherStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertTeacherComp
        :errors="TeacherStore.error"
        :model-value="Model"
        :person-data="PersonStore.selectedPerson"
        @on-submit="OnSubmitUpsert"
        @search-Teacher="onSearchTeacher"
        @updatemodel-value="fillTeacher"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onpopaClose">
      <TeacherCard :-data-req="TeacherStore.selectedTeacher" />
    </PopapComp>

    <button @click="showUpsert = true">إضافة معلم</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { usePersonStore } from "@/store/Person/usePersonStore";
import { useTeacherStore } from "@/store/Person/useTeacherStore";
import MessageComp from "@/components/general/messageComp.vue";
import TeacherCard from "@/components/Person/Teacher/TeacherCard.vue";
import TeacherList from "@/components/Person/Teacher/TeacherList.vue";
import UpsertTeacherComp from "@/components/Person/Teacher/UpsertTeachertComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";

export default {
  name: "TeacherView",
  components: {
    TeacherList,
    UpsertTeacherComp,
    TeacherCard,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      PersonStore: usePersonStore(),
      TeacherStore: useTeacherStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      Model: {
        personID: "",
        TeacherID: "",
      },
      teacherFilterSchema: [
        {
          key: "FirstName",
          label: "الاسم الأول",
          type: "text",
          operator: "=*",
        },
        {
          key: "LastName",
          label: "الكنية",
          type: "text",
          operator: "=*",
        },
        {
          key: "NationalNumber",
          label: "الرقم الوطني",
          type: "text",
          operator: "=*",
        },
      ],
    };
  },
  methods: {
    async onPageChange(newPage) {
      await this.TeacherStore.setPage(newPage);
    },

    async onPageSizeChange(newSize) {
      await this.TeacherStore.setPageSize(newSize);
    },

    async onFilterSearch(payload) {
      await this.TeacherStore.fetchTeachers({
        Filter: payload.filter,
        OrderBy: payload.orderBy,
      });
    },

    async onFilterReset() {
      await this.TeacherStore.fetchTeachers();
    },

    async OnUpsertPerson(e) {
      if (e) {
        this.TeacherStore.selectedTeacher = e;
        this.Model.personID = e.personID;
        this.Model.TeacherID = e.TeacherID;
        this.PersonStore.selectedPerson = e;
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = false;
      if (e.TeacherID) {
        resVal = await this.TeacherStore.updateTeacher(e);
      } else {
        resVal = await this.TeacherStore.addTeacher(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal.errors || "حدث خطأ أثناء الحفظ";
        this.resultType = true;
      }
    },

    async onSearchTeacher(type, val) {
      await this.PersonStore.fetchPersonBy({
        Param: val,
        ParamType: type,
      });
    },

    async OnDelete(e) {
      await this.TeacherStore.DeleteTeacher(e.id);
    },

    async onShowInfo(e) {
      this.TeacherStore.selectedTeacher = this.TeacherStore.Teachers.find(
        (p) => p.id === e.id
      );
      this.showInfo = true;
    },

    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.TeacherStore.selectedTeacher = null;
    },

    fillTeacher(field) {
      this.Model = field;
    },
  },

  mounted() {
    this.TeacherStore.fetchTeachers();
  },
};
</script>
