<template>
  <div class="space-y-4" dir="rtl">
    <GenericFilterComp
      :fields="filterFields"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <p v-if="AssignmentStore.loading" role="status">جارٍ تحميل الاختبارات…</p>
    <p v-else-if="AssignmentStore.error" class="alert-error" role="alert">
      {{ formatError(AssignmentStore.error) }}
    </p>
    <p
      v-else-if="!AssignmentStore.assignments.length"
      class="rounded-lg border border-dashed p-5 text-center"
      role="status"
    >
      لا توجد اختبارات مطابقة.
    </p>
    <AssigmentList
      v-if="AssignmentStore.assignments.length"
      :can-read="AuthStore.hasPermission('Can_Read_Assignment')"
      :can-update="AuthStore.hasPermission('Can_Update_Assignment')"
      :can-delete="AuthStore.hasPermission('Can_Delete_Assignment')"
      :can-add-result="AuthStore.hasPermission('Can_Create_Result')"
      @show-delete="OnDelete"
      @show-update="onUpdateAssignment"
      :Assigments="AssignmentStore.assignments"
      @show-add-result="OnAddResult"
    />
    <PaginationComp
      :current-page="AssignmentStore.page"
      :page-size="AssignmentStore.pageSize"
      :total-count="AssignmentStore.totalCount"
      :total-pages="AssignmentStore.totalPages"
      @change-page="AssignmentStore.setPage"
      @change-size="AssignmentStore.setPageSize"
    />
    <PopapComp v-if="showAssignmentUpsert" @close="closeAssignmentUpsert">
      <UpsertAssigmentComp
        :nomination-info="AssignmentStore.selectedassignments"
        :errors="AssignmentStore.error"
        :saving="AssignmentStore.loading"
        @submit="onSubmitAssignment"
      />
    </PopapComp>
    <PopapComp v-if="showNewResult" @close="showNewResult = false">
      <UpsertResultComp
        :assignment-scope-infos="AssignmentScopeinfoData"
        :assignment-i-d="asignmentID"
        :student-name="StudentName"
        :selected-result="null"
        :errors="ResultStore.error"
        :saving="ResultStore.loading || AssignmentScopeinfoStore.loading"
        @submit="AddResultSubmit"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import AssigmentList from "@/components/Assigment/Assigment/AssigmentList.vue";
import UpsertAssigmentComp from "@/components/Assigment/Assigment/UpsertAssigmentComp.vue";
import UpsertResultComp from "@/components/Assigment/Result/UpsertResultComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import { useassignmentstore } from "@/store/Assignment/AssignmentStore";
import { useAssignmentScopeinfoStore } from "@/store/Assignment/AssignmentScopeInfoStore";
import { useResultStore } from "@/store/Assignment/ResultStore";
import { useAuthStore } from "@/store/Auth/authStore";
import MessageComp from "@/components/general/messageComp.vue";
export default {
  name: "AssignmentView",
  components: {
    AssigmentList,
    UpsertAssigmentComp,
    UpsertResultComp,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      AssignmentStore: useassignmentstore(),
      AssignmentScopeinfoStore: useAssignmentScopeinfoStore(),
      ResultStore: useResultStore(),
      AuthStore: useAuthStore(),
      showNewResult: false,
      showAssignmentUpsert: false,
      StudentName: "",
      asignmentID: "",
      AssignmentScopeinfoData: [],
      resultMessage: null,
      resultType: false,
      filterFields: [
        {
          key: "StudentName",
          label: "اسم الطالب",
          type: "text",
          operator: "=*",
        },
        {
          key: "AcademicPlanName",
          label: "الخطة الأكاديمية",
          type: "text",
          operator: "=*",
        },
        {
          key: "AssigmentScopeName",
          label: "نطاق الاختبار",
          type: "text",
          operator: "=*",
        },
        {
          key: "AssignmentState",
          label: "حالة الاختبار",
          type: "text",
          operator: "=*",
        },
      ],
    };
  },
  methods: {
    async OnDelete(e) {
      const result = await this.AssignmentStore.deleteAssignment(
        e.assignmentID
      );
      this.resultMessage = result.success ? "تم حذف الاختبار" : result.errors;
      this.resultType = !result.success;
    },
    formatError(value) {
      if (typeof value === "string") return value;
      if (Array.isArray(value)) return value.join("، ");
      if (value && typeof value === "object")
        return Object.values(value).flat().join("، ");
      return "حدث خطأ";
    },
    async onFilterSearch(payload) {
      await this.AssignmentStore.fetchassignments({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },
    async onFilterReset() {
      await this.AssignmentStore.fetchassignments({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },
    async onUpdateAssignment(item) {
      const result = await this.AssignmentStore.fetchForUpsert(
        item.assignmentID
      );
      if (!result.success || !result.data) {
        this.resultMessage = result.errors || "تعذر تحميل الاختبار للتعديل";
        this.resultType = true;
        return;
      }
      this.showAssignmentUpsert = true;
    },
    async onSubmitAssignment(payload) {
      const result = await this.AssignmentStore.updateAssignment(payload);
      this.resultMessage = result.success
        ? "تم تحديث موعد الاختبار"
        : result.errors;
      this.resultType = !result.success;
      if (result.success) this.closeAssignmentUpsert();
    },
    closeAssignmentUpsert() {
      this.showAssignmentUpsert = false;
      this.AssignmentStore.clearselectedassignments();
    },
    async OnAddResult(e) {
      this.StudentName = e.studentName;
      this.asignmentID = e.assignmentID;
      this.ResultStore.clearSelectedResult();
      this.ResultStore.assignmentID = e.assignmentID;
      this.ResultStore.error = null;

      const scopeResult =
        await this.AssignmentScopeinfoStore.fetchAssignmentScopeinfosForAssigment(
          { AssignmentID: e.assignmentID }
        );
      if (!scopeResult.success || !scopeResult.data?.length) {
        this.resultMessage = scopeResult.success
          ? "لا توجد وحدات متاحة لإدخال نتيجة هذا الاختبار"
          : scopeResult.errors;
        this.resultType = true;
        return;
      }
      this.AssignmentScopeinfoData =
        this.AssignmentScopeinfoStore.AssignmentScopeinfos;
      this.showNewResult = true;
    },
    async AddResultSubmit(e) {
      const resVal = e.resultID
        ? await this.ResultStore.updateResult(e)
        : await this.ResultStore.addResult(e);
      if (resVal.success) {
        this.resultMessage = "تم الحفظ بنجاح";
        this.resultType = false;
        this.showNewResult = false;
        await this.AssignmentStore.fetchassignments();
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
  },
  mounted() {
    this.AssignmentStore.fetchassignments();
  },
};
</script>

<style scoped></style>
