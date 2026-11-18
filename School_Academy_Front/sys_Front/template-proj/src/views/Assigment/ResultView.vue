<template>
  <main class="space-y-4" dir="rtl">
    <GenericFilterComp
      :fields="filterFields"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <p v-if="ResultStore.loading" role="status">جارٍ تحميل النتائج…</p>
    <p v-else-if="ResultStore.error" class="alert-error" role="alert">
      {{ formatError(ResultStore.error) }}
    </p>
    <p
      v-else-if="!ResultStore.Results.length"
      class="rounded-lg border border-dashed p-5 text-center"
      role="status"
    >
      لا توجد نتائج مطابقة.
    </p>
    <ResultList
      v-if="
        !ResultStore.loading && !ResultStore.error && ResultStore.Results.length
      "
      :Results="ResultStore.Results"
      :can-update="AuthStore.hasPermission('Can_Update_Result')"
      :can-delete="AuthStore.hasPermission('Can_Delete_Result')"
      @show-delete="onDeleteResult"
      @show-update="onUpdateResult"
    />
    <PaginationComp
      :current-page="ResultStore.page"
      :page-size="ResultStore.pageSize"
      :total-count="ResultStore.totalCount"
      :total-pages="ResultStore.totalPages"
      @change-page="ResultStore.setPage"
      @change-size="ResultStore.setPageSize"
    />

    <PopapComp v-if="showUpsert" @close="closePopup">
      <UpsertResultComp
        :assignment-scope-infos="AssignmentScopeInfos"
        :assignment-i-d="ResultStore.selectedResult?.assignmentID || ''"
        :student-name="ResultStore.selectedResult?.studentName || ''"
        :selected-result="ResultStore.selectedResult"
        :errors="ResultStore.error"
        :saving="ResultStore.loading || AssignmentScopeInfoStore.loading"
        @submit="onSubmitResult"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </main>
</template>

<script>
import { useResultStore } from "@/store/Assignment/ResultStore";
import { useAuthStore } from "@/store/Auth/authStore";
import { useAssignmentScopeinfoStore } from "@/store/Assignment/AssignmentScopeInfoStore";
import ResultList from "@/components/Assigment/Result/ResultList.vue";
import UpsertResultComp from "@/components/Assigment/Result/UpsertResultComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";

export default {
  name: "ResultView",
  components: {
    ResultList,
    UpsertResultComp,
    GenericFilterComp,
    PaginationComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      ResultStore: useResultStore(),
      AuthStore: useAuthStore(),
      AssignmentScopeInfoStore: useAssignmentScopeinfoStore(),
      AssignmentScopeInfos: [],
      showUpsert: false,
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
          key: "AssignmentScopeName",
          label: "نطاق الاختبار",
          type: "text",
          operator: "=*",
        },
        {
          key: "StageInfoMatter",
          label: "وحدة الاختبار",
          type: "text",
          operator: "=*",
        },
        {
          key: "Score",
          label: "النتيجة",
          type: "number",
          operator: "=",
        },
        {
          key: "Score",
          label: "النتيجة",
          type: "number",
          operator: "=",
        },
      ],
    };
  },
  methods: {
    formatError(value) {
      if (typeof value === "string") return value;
      if (Array.isArray(value)) return value.join("، ");
      if (value && typeof value === "object")
        return Object.values(value).flat().join("، ");
      return "حدث خطأ";
    },
    async onFilterSearch(payload) {
      await this.ResultStore.fetchResults({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
        AssignmentID: "",
      });
    },
    async onFilterReset() {
      await this.ResultStore.fetchResults({
        Page: 1,
        Filter: "",
        OrderBy: "",
        AssignmentID: "",
      });
    },
    async onUpdateResult(item) {
      const result = await this.ResultStore.fetchForUpsert(item.resultID);
      if (!result.success || !result.data) {
        this.resultMessage = result.errors || "تعذر تحميل النتيجة للتعديل";
        this.resultType = true;
        return;
      }

      const scopeResult =
        await this.AssignmentScopeInfoStore.fetchAssignmentScopeinfosForAssigment(
          {
            AssignmentID: result.data.assignmentID,
          }
        );
      if (!scopeResult.success) {
        this.resultMessage = scopeResult.errors;
        this.resultType = true;
        return;
      }

      this.AssignmentScopeInfos = [...(scopeResult.data || [])];
      if (
        !this.AssignmentScopeInfos.some(
          (scope) =>
            scope.assignmentScopeInfoID === result.data.assignmentScopeInfoID
        )
      ) {
        this.AssignmentScopeInfos.push({
          assignmentScopeInfoID: result.data.assignmentScopeInfoID,
          assignmentScopeName: result.data.assignmentScopeName,
          stageInfoMatterNameName: result.data.stageInfoMatter,
          maxScore: result.data.maxScore,
          minPassScore: result.data.minPassScore,
        });
      }
      this.showUpsert = true;
    },
    async onSubmitResult(payload) {
      const result = payload.resultID
        ? await this.ResultStore.updateResult(payload)
        : await this.ResultStore.addResult(payload);
      this.resultMessage = result.success ? "تم حفظ النتيجة" : result.errors;
      this.resultType = !result.success;
      if (result.success) this.closePopup();
    },
    async onDeleteResult(item) {
      if (!window.confirm("هل تريد حذف النتيجة وإعادة احتساب الاختبار؟"))
        return;
      const result = await this.ResultStore.DeleteResult(item.resultID);
      this.resultMessage = result.success
        ? "تم حذف النتيجة وإعادة احتساب الاختبار"
        : result.errors;
      this.resultType = !result.success;
    },
    closePopup() {
      this.showUpsert = false;
      this.AssignmentScopeInfos = [];
      this.ResultStore.clearSelectedResult();
    },
  },
  async mounted() {
    await this.ResultStore.fetchResults({ Page: 1, AssignmentID: "" });
  },
};
</script>
