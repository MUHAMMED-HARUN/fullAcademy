<template>
  <div class="space-y-4" dir="rtl">
    <GenericFilterComp
      :fields="filterFields"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <p v-if="AssignmentNomination.loading" role="status">
      جارٍ تحميل الترشيحات…
    </p>
    <p v-else-if="AssignmentNomination.error" class="alert-error" role="alert">
      {{ formatError(AssignmentNomination.error) }}
    </p>
    <p
      v-else-if="!AssignmentNomination.AssignmentNominations.length"
      role="status"
      class="rounded-lg border border-dashed p-5 text-center"
    >
      لا توجد ترشيحات مطابقة.
    </p>
    <AssignmentNominationList
      :can-read="AuthStore.hasPermission('Can_Read_AssignmentNomination')"
      :can-update="AuthStore.hasPermission('Can_Update_AssignmentNomination')"
      :can-delete="AuthStore.hasPermission('Can_Delete_AssignmentNomination')"
      :can-start-assignment="AuthStore.hasPermission('Can_Create_Assignment')"
      @show-delete="OnDelete"
      @show-new-assignment="OnStartAssigment"
      :AssignmentNominations="AssignmentNomination.AssignmentNominations"
      @show-update="onUpdateNomination"
    />
    <PaginationComp
      :current-page="AssignmentNomination.page"
      :page-size="AssignmentNomination.pageSize"
      :total-count="AssignmentNomination.totalCount"
      :total-pages="
        Math.max(
          1,
          Math.ceil(
            AssignmentNomination.totalCount / AssignmentNomination.pageSize
          )
        )
      "
      @change-page="AssignmentNomination.setPage"
      @change-size="AssignmentNomination.setPageSize"
    />
    <PopapComp v-if="showAddAssigment" @close="closeAssignmentPopup">
      <UpsertAssigmentComp
        :nomination-info="AssignmentNomination.selectedAssignmentNomination"
        :errors="assignmentstore.error"
        :saving="assignmentstore.loading"
        @submit="StartAssigment"
      />
    </PopapComp>
    <PopapComp v-if="showUpsert" @close="closeNominationPopup">
      <UpsertAssignmentNominationComp
        :assignment-scopes-data="AssignmentScopeStore.AssignmentScopes"
        :student-academic-enrollment-i-d="
          AssignmentNomination.selectedAssignmentNomination
            ?.studentAcademicEnrollmentID || ''
        "
        :student-name="
          AssignmentNomination.selectedAssignmentNomination?.studentName || ''
        "
        :errors="AssignmentNomination.error"
        :saving="AssignmentNomination.loading || AssignmentScopeStore.loading"
        @submit="onSubmitNominationUpdate"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import { useAssignmentScopeStore } from "@/store/Assignment/AssignmentScopeStore";
import { useAuthStore } from "@/store/Auth/authStore";
import { useAssignmentNominationStore } from "@/store/Assignment/AssignmentNominationStore";
import MessageComp from "@/components/general/messageComp.vue";
import AssignmentNominationList from "@/components/Assigment/AssignmentNomination/AssignmentNominationList.vue";
import UpsertAssigmentComp from "@/components/Assigment/Assigment/UpsertAssigmentComp.vue";
import { useassignmentstore } from "@/store/Assignment/AssignmentStore";
import PopapComp from "@/components/general/popapComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import UpsertAssignmentNominationComp from "@/components/Assigment/AssignmentNomination/UpsertAssignmentNominationComp.vue";

export default {
  name: "AssignmentNominationView",
  components: {
    AssignmentNominationList,
    MessageComp,
    UpsertAssigmentComp,
    PopapComp,
    GenericFilterComp,
    PaginationComp,
    UpsertAssignmentNominationComp,
  },
  data() {
    return {
      AssignmentNomination: useAssignmentNominationStore(),
      AuthStore: useAuthStore(),
      AssignmentScopeStore: useAssignmentScopeStore(),
      assignmentstore: useassignmentstore(),
      showAddAssigment: false,
      showInfo: false,
      showUpsert: false,
      resultMessage: null,
      resultType: false,
      stageInfos: [],
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
          key: "AssignmentScopeName",
          label: "نطاق الاختبار",
          type: "text",
          operator: "=*",
        },
      ],
    };
  },
  methods: {
    async OnDelete(e) {
      const result = await this.AssignmentNomination.DeleteAssignmentNomination(
        e.assignmentNominationID
      );
      this.resultMessage = result.success ? "تم حذف الترشيح" : result.errors;
      this.resultType = !result.success;
    },
    async OnStartAssigment(e) {
      this.AssignmentNomination.selectedAssignmentNomination = e;
      this.assignmentstore.clearselectedassignments();
      this.assignmentstore.error = null;
      this.showAddAssigment = true;
    },
    async StartAssigment(e) {
      const result = e.assignmentID
        ? await this.assignmentstore.updateAssignment(e)
        : await this.assignmentstore.addAssignment(e);
      if (result.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showAddAssigment = false;
        await this.AssignmentNomination.fetchAssignmentNominations();
        this.AssignmentNomination.clearSelectedAssignmentNomination();
        this.assignmentstore.clearselectedassignments();
      } else {
        this.resultMessage = result.errors;
        this.resultType = true;
      }
    },
    closeAssignmentPopup() {
      this.showAddAssigment = false;
      this.AssignmentNomination.clearSelectedAssignmentNomination();
      this.assignmentstore.clearselectedassignments();
    },
    formatError(value) {
      if (typeof value === "string") return value;
      if (Array.isArray(value)) return value.join("، ");
      if (value && typeof value === "object")
        return Object.values(value).flat().join("، ");
      return "حدث خطأ";
    },
    async onFilterSearch(payload) {
      await this.AssignmentNomination.fetchAssignmentNominations({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },
    async onFilterReset() {
      await this.AssignmentNomination.fetchAssignmentNominations({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },
    async onUpdateNomination(item) {
      const result =
        await this.AssignmentNomination.fetchAssignmentNominationtBy({
          AssignmentNominationID: item.assignmentNominationID,
        });
      if (!result.success || !result.data) {
        this.resultMessage = result.errors || "تعذر تحميل الترشيح";
        this.resultType = true;
        return;
      }
      const enrollmentID = result.data.studentAcademicEnrollmentID;
      const scopesResult = await this.AssignmentScopeStore.fetchCanNominateList(
        {
          studentAcademicEnrollmentID: enrollmentID,
        }
      );
      const scopes = this.AssignmentScopeStore.AssignmentScopes;
      if (!scopesResult.success) {
        this.resultMessage = scopesResult.errors;
        this.resultType = true;
        return;
      }
      if (
        !scopes.some(
          (scope) => scope.assignmentScopeID === result.data.assignmentScopeID
        )
      ) {
        scopes.push({
          assignmentScopeID: result.data.assignmentScopeID,
          name: result.data.assignmentScopeName,
        });
      }
      this.showUpsert = true;
    },
    async onSubmitNominationUpdate(payload) {
      const result = await this.AssignmentNomination.updateAssignmentNomination(
        payload
      );
      this.resultMessage = result.success ? "تم تحديث الترشيح" : result.errors;
      this.resultType = !result.success;
      if (result.success) this.closeNominationPopup();
    },
    closeNominationPopup() {
      this.showUpsert = false;
      this.AssignmentNomination.clearSelectedAssignmentNomination();
      this.AssignmentScopeStore.AssignmentScopes = [];
    },
  },
  mounted() {
    this.AssignmentNomination.fetchAssignmentNominations();
  },
};
</script>

<style scoped></style>
