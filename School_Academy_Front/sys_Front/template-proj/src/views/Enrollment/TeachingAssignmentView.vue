<template>
  <div>
    <!-- 🔹 مكون التصفية العامة -->
    <GenericFilterComp
      :fields="teachingAssignmentFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <!-- 🔹 قائمة الأدمن -->
    <TeachingAssignmentList
      v-if="AuthStore.UserRole === 'Admin'"
      :enrollments="TeachingAssignmentStore.enrollments"
      @show-delete-enrollment="OnDelete"
      @show-info-enrollment="onShowInfo"
      @show-update-enrollment="OnUpsertEnrollment"
    />

    <!-- 🔹 قائمة المعلم -->
    <TeachingGroupList
      v-if="AuthStore.UserRole === 'Teacher'"
      :enrollments="TeachingAssignmentStore.enrollments"
      @show-student-group="RouteStudentGroup"
    />

    <!-- 🔹 مكون التصفح (Pagination) -->
    <PaginationComp
      :current-page="TeachingAssignmentStore.page"
      :page-size="TeachingAssignmentStore.pageSize"
      :total-count="TeachingAssignmentStore.totalCount"
      :total-pages="TeachingAssignmentStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <!-- 🔹 النافذة المنبثقة للإضافة والتعديل -->
    <PopapComp
      v-if="AuthStore.UserRole === 'Admin' && showUpsert"
      @close="onpopaClose"
    >
      <UpsertTeachingAssignmentComp
        :errors="TeachingAssignmentStore.error"
        :teacherData="TeacherStore.selectedTeacher"
        :group-data="GroupStore.selectedGroup"
        :subjectData="SessionStore.selectedSession"
        @on-submit="OnSubmitUpsert"
        @on-search-teacher="onSearchTeacher"
        @on-search-group="onSearchGroup"
        @on-search-subject="onSearchSubject"
      />
    </PopapComp>

    <!-- 🔹 النافذة المنبثقة للبطاقة/التفاصيل -->
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <TeachingAssignmentCard
        :data-req="TeachingAssignmentStore.selectedEnrollment"
      />
    </PopapComp>

    <button
      v-if="AuthStore.UserRole === 'Admin'"
      @click="OnUpsertEnrollment(null)"
    >
      تسجيل معلم جديد
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import { useRouter } from "vue-router";
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";

import TeachingAssignmentList from "@/components/Enrollment/TeachingAssignment/TeachingAssignmentList.vue";
import UpsertTeachingAssignmentComp from "@/components/Enrollment/TeachingAssignment/UpsertTeachingAssignmentComp.vue";
import TeachingAssignmentCard from "@/components/Enrollment/TeachingAssignment/TeachingAssignmentCard.vue";
import TeachingGroupList from "@/components/Enrollment/TeachingAssignment/TeacherRole/TeachingGroupList.vue";

import { useGroupStore } from "@/store/Institute/GroupStore";
import { useTeachingAssignmentStore } from "@/store/Enrollment/TeachingAssignmentStore";
import { useTeacherStore } from "@/store/Person/useTeacherStore";
import { useSessionStore } from "@/store/Progress/SessionStore";
import { useAuthStore } from "@/store/Auth/authStore";
import TResult from "@/Generic/TResult";

export default {
  name: "TeachingAssignmentView",
  components: {
    GenericFilterComp,
    PaginationComp,
    TeachingAssignmentList,
    UpsertTeachingAssignmentComp,
    TeachingAssignmentCard,
    PopapComp,
    MessageComp,
    TeachingGroupList,
  },
  data() {
    return {
      router: useRouter(),
      AuthStore: useAuthStore(),
      TeachingAssignmentStore: useTeachingAssignmentStore(),
      TeacherStore: useTeacherStore(),
      GroupStore: useGroupStore(),
      SessionStore: useSessionStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      // 🔹 مخطط التصفية المستند إلى حقول DTO الخطيئة (TeacherGroupInfo / TeachingAssignmentInfo)
      teachingAssignmentFilterSchema: [
        {
          key: "TeacherName",
          label: "اسم المعلم",
          type: "text",
          operator: "=*",
        },
        {
          key: "GroupName",
          label: "اسم الشعبة",
          type: "text",
          operator: "=*",
        },
        {
          key: "SubjectName",
          label: "اسم المادة",
          type: "text",
          operator: "=*",
        },
        {
          key: "EnrollmentDate",
          label: "من تاريخ التسجيل",
          type: "date",
          operator: ">=",
        },
        {
          key: "EnrollmentDate",
          label: "إلى تاريخ التسجيل",
          type: "date",
          operator: "<=",
        },
      ],
    };
  },
  methods: {
    // 🔹 إدارة الترقيم (Pagination)
    async onPageChange(newPage) {
      await this.TeachingAssignmentStore.setPage(newPage);
    },
    async onPageSizeChange(newSize) {
      await this.TeachingAssignmentStore.setPageSize(newSize);
    },
    RouteStudentGroup(e) {
      this.router.push({
        path: "/studentEnrollment",
        query: {
          groupID: e.groupID, // إرسال المعامل عبر الـ Query Params
        },
      });
    },
    // 🔹 إدارة التصفية (Filter)
    async onFilterSearch(payload) {
      await this.TeachingAssignmentStore.fetchEnrollments(
        {
          Filter: payload.filter,
          OrderBy: payload.orderBy,
        },
        this.AuthStore.UserRole
      );
    },
    async onFilterReset() {
      await this.TeachingAssignmentStore.fetchEnrollments({
        Role: this.AuthStore.UserRole,
      });
    },

    // 🔹 الإجراءات الأخرى
    async OnUpsertEnrollment(e) {
      if (e) {
        this.TeachingAssignmentStore.selectedEnrollment = e;
        await this.onSearchTeacher(
          1,
          this.TeachingAssignmentStore.selectedEnrollment.teacherID
        );
        await this.onSearchGroup(
          0,
          this.TeachingAssignmentStore.selectedEnrollment.groupID
        );
        await this.onSearchSubject(
          0,
          this.TeachingAssignmentStore.selectedEnrollment.subjectID
        );
      } else {
        this.TeachingAssignmentStore.selectedEnrollment = null;
        this.TeacherStore.selectedTeacher = null;
        this.GroupStore.selectedGroup = null;
        this.SessionStore.selectedSession = null;
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      let resVal = new TResult();
      if (e.enrollmentID) {
        resVal = await this.TeachingAssignmentStore.updateEnrollment(e);
      } else {
        resVal = await this.TeachingAssignmentStore.addEnrollment(e);
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ بنجاح";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async onSearchTeacher(type, val) {
      await this.TeacherStore.fetchTeacherBy({
        Param: val,
        ParamType: type,
      });
    },
    async onSearchGroup(type, val) {
      await this.GroupStore.fetchGroupBy({
        Param: val,
        ParamType: type,
      });
    },
    async onSearchSubject(type, val) {
      await this.SessionStore.fetchSessiontBy({
        Param: val,
        ParamType: type,
      });
    },
    async OnDelete(e) {
      if (confirm("هل أنت متأكد من حذف هذا التسجيل؟")) {
        const resVal = await this.TeachingAssignmentStore.DeleteEnrollment(
          e.enrollmentID
        );
        if (resVal.success) {
          this.resultMessage = "تم الحذف بنجاح";
          this.resultType = false;
        } else {
          this.resultMessage = resVal.errors;
          this.resultType = true;
        }
      }
    },
    async onShowInfo(e) {
      this.TeachingAssignmentStore.selectedEnrollment =
        this.TeachingAssignmentStore.enrollments.find(
          (en) => en.enrollmentID === e.enrollmentID
        );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.TeachingAssignmentStore.selectedEnrollment = null;
      this.TeacherStore.selectedTeacher = null;
      this.GroupStore.selectedGroup = null;
      this.SessionStore.selectedSession = null;
    },
  },
  mounted() {
    this.TeachingAssignmentStore.fetchEnrollments({}, this.AuthStore.UserRole);
  },
};
</script>
