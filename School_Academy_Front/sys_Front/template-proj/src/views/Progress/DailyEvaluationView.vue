<template>
  <div>
    <!-- مكون الفلترة العامة -->
    <GenericFilterComp
      :fields="dailyEvaluationFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <!-- قائمة التقييمات اليومية -->
    <DailyEvaluationList
      @show-delete-daily-evaluation="OnDeleteDailyEvaluation"
      @show-info-daily-evaluation="onShowInfoDailyEvaluation"
      @show-update-daily-evaluation="OnUpsertDailyEvaluation"
      :-daily-evaluations="DailyEvaluationStore.evaluations"
    />

    <!-- مكون التصفح (Pagination) -->
    <PaginationComp
      :current-page="DailyEvaluationStore.page"
      :page-size="DailyEvaluationStore.pageSize"
      :total-count="DailyEvaluationStore.totalCount"
      :total-pages="DailyEvaluationStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <!-- نافذة الإضافة والتعديل -->
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertDailyEvaluationComp
        :errors="DailyEvaluationStore.error"
        :-student-stage-info-data="useStageInfoStore.StageInfos"
        :-last-daily-evaluation-data="DailyEvaluationStore.selectedEvaluation"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>

    <!-- نافذة عرض تفاصيل التقييم -->
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <DailyEvaluationCard
        :-data-req="DailyEvaluationStore.selectedEvaluation"
      />
    </PopapComp>

    <!-- زر إضافة تقييم جديد -->
    <button @click="showUpsert = true">إضافة تقييم يومي</button>

    <!-- رسائل التنبيه والنتائج -->
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";

import DailyEvaluationList from "@/components/Progress/DailyEvaluation/DailyEvaluationList.vue";
import UpsertDailyEvaluationComp from "@/components/Progress/DailyEvaluation/UpsertDailyEvaluationComp.vue";
import DailyEvaluationCard from "@/components/Progress/DailyEvaluation/DailyEvaluationCard.vue";

import { useDailyEvaluationStore } from "@/store/Progress/DailyEvaluationStore";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import TResult from "@/Generic/TResult";

export default {
  name: "DailyEvaluationView",
  components: {
    DailyEvaluationList,
    UpsertDailyEvaluationComp,
    DailyEvaluationCard,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      DailyEvaluationStore: useDailyEvaluationStore(),
      useStageInfoStore: useStageInfoStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,

      // مخطط الفلترة الخاص بالتقييمات اليومية المتوافق مع Gridify
      dailyEvaluationFilterSchema: [
        {
          key: "studentName",
          label: "اسم الطالب",
          type: "text",
          operator: "=*",
        },
        {
          key: "date",
          label: "من تاريخ التقييم",
          type: "date",
          operator: ">=",
        },
        {
          key: "date",
          label: "إلى تاريخ التقييم",
          type: "date",
          operator: "<=",
        },
        {
          key: "levelType",
          label: "التقييم",
          type: "select",
          options: [
            { label: "ممتاز", value: "ممتاز" },
            { label: "جيد جدا", value: "جيد جدا" },
            { label: "جيد", value: "جيد" },
            { label: "سيء", value: "سيء" },
            { label: "سيء جدا", value: "سيء جدا" },
          ],
        },
      ],
    };
  },
  methods: {
    async onPageChange(newPage) {
      await this.DailyEvaluationStore.setPage(newPage);
    },
    async onPageSizeChange(newSize) {
      await this.DailyEvaluationStore.setPageSize(newSize);
    },
    async onFilterSearch(payload) {
      await this.DailyEvaluationStore.fetchEvaluations({
        Filter: payload.filter,
        OrderBy: payload.orderBy,
      });
    },
    async onFilterReset() {
      await this.DailyEvaluationStore.fetchDailyEvaluations();
    },
    async OnUpsertDailyEvaluation(e) {
      if (e) {
        this.DailyEvaluationStore.selectedEvaluation = e;
        this.useStageInfoStore.getStudentStageInfo(e.studentEnrollmentID);
      }
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.dailyEvaluationID) {
        resVal = await this.DailyEvaluationStore.updateDailyEvaluation(e);
      } else {
        resVal.success = false;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async OnDeleteDailyEvaluation(e) {
      this.DailyEvaluationStore.deleteEvaluation(e.dailyEvaluationID);
    },
    async onShowInfoDailyEvaluation(e) {
      this.DailyEvaluationStore.selectedEvaluation =
        this.DailyEvaluationStore.evaluations.find(
          (i) => i.dailyEvaluationID === e.dailyEvaluationID
        );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.DailyEvaluationStore.selectedEvaluation = null;
    },
  },
  mounted() {
    this.DailyEvaluationStore.fetchEvaluations({});
  },
};
</script>

<style scoped></style>
