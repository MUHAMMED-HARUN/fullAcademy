<template>
  <div class="space-y-4" dir="rtl">
    <GenericFilterComp
      :fields="stageFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <p
      v-if="StageStore.loading"
      class="text-sm text-[var(--text-muted)]"
      role="status"
    >
      جارٍ تحميل المراحل…
    </p>
    <p v-else-if="StageStore.error" class="alert-error" role="alert">
      {{ StageStore.error }}
    </p>
    <p
      v-else-if="StageStore.stagePageItems.length === 0"
      class="rounded-lg border border-dashed border-[var(--border-color)] p-4 text-center text-[var(--text-muted)]"
      role="status"
    >
      لا توجد مراحل مطابقة.
    </p>
    <StageList
      :stages="StageStore.stagePageItems"
      :can-read="canReadStage"
      :can-create="canCreateStageInfo"
      :can-update="canUpdateStage"
      :can-delete="canDeleteStage"
      @show-delete="OnDeleteStage"
      @show-info="onShowInfoStage"
      @show-update="OnUpsertStage"
      @add-stage-info="onAddStageInfo"
    />
    <PaginationComp
      :current-page="StageStore.page"
      :page-size="StageStore.pageSize"
      :total-count="StageStore.totalCount"
      :total-pages="StageStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertStageComp
        :errors="StageStore.error"
        :classes="ClassStore.classes"
        :class-loading="ClassStore.loading"
        :class-error="ClassStore.error"
        :previous-stage-results="StageStore.previousStageResults"
        :previous-stage-loading="StageStore.previousStageLoading"
        :previous-stage-error="StageStore.previousStageError"
        @search-previous-stage="onSearchPreviousStage"
        @clear-previous-stage="StageStore.clearPreviousStageLookup"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <StageCard :-data-req="StageStore.selectedStage" />
    </PopapComp>
    <button
      v-if="canCreateStage"
      type="button"
      class="btn btn-primary"
      @click="OnUpsertStage(null)"
    >
      إضافة مرحلة
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useStageStore } from "@/store/Progress/StageStore";
import MessageComp from "@/components/general/messageComp.vue";
import TResult from "@/Generic/TResult";
import StageList from "@/components/Progress/Stage/StageList.vue";
import UpsertStageComp from "@/components/Progress/Stage/UpsertStageComp.vue";
import StageCard from "@/components/Progress/Stage/StageCard.vue";
import { useClassStore } from "@/store/Institute/ClassStore";
import { useAuthStore } from "@/store/Auth/authStore";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";

export default {
  name: "StageView",
  components: {
    StageList,
    StageCard,
    UpsertStageComp,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      StageStore: useStageStore(),
      ClassStore: useClassStore(),
      AuthStore: useAuthStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      stageFilterSchema: [
        {
          key: "StageName",
          label: "اسم المرحلة",
          type: "text",
          operator: "=*",
        },
        { key: "ClassName", label: "اسم الصف", type: "text", operator: "=*" },
        {
          key: "PrevStageName",
          label: "المرحلة السابقة",
          type: "text",
          operator: "=*",
        },
      ],
    };
  },
  computed: {
    canReadStage() {
      return this.AuthStore.hasPermission("Can_Read_Stage");
    },
    canCreateStage() {
      return this.AuthStore.hasPermission("Can_Create_Stage");
    },
    canCreateStageInfo() {
      return this.AuthStore.hasPermission("Can_Create_StageInfo");
    },
    canUpdateStage() {
      return this.AuthStore.hasPermission("Can_Update_Stage");
    },
    canDeleteStage() {
      return this.AuthStore.hasPermission("Can_Delete_Stage");
    },
  },
  methods: {
    async OnUpsertStage(e) {
      this.StageStore.clearPreviousStageLookup();
      if (!this.ClassStore.classes.length && !this.ClassStore.loading)
        await this.ClassStore.fetchClasses();
      if (e) {
        this.StageStore.selectedStage = e;
      } else {
        this.StageStore.selectedStage = null;
      }
      this.showUpsert = true;
    },

    async onAddStageInfo(stage) {
      this.StageStore.selectedStage = stage;
      await this.$router.push({
        path: "/stageinfo",
        query: { stageID: stage.stageID },
      });
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.stageID) {
        resVal = await this.StageStore.updateStage(e);
      } else {
        resVal = await this.StageStore.addStage(e);
        this.StageStore.selectedStage = null;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.StageStore.selectedStage = null;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async OnDeleteStage(e) {
      if (!window.confirm("هل أنت متأكد من حذف هذه المرحلة؟")) return;
      const result = await this.StageStore.DeleteStage(e.stageID);
      this.resultMessage = result.success ? "تم الحذف" : result.errors;
      this.resultType = !result.success;
    },
    async onShowInfoStage(e) {
      this.StageStore.selectedStage = e;
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.StageStore.selectedStage = null;
      this.StageStore.clearPreviousStageLookup();
    },

    async onFilterSearch(payload) {
      await this.StageStore.fetchStagePage({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },
    async onFilterReset() {
      await this.StageStore.fetchStagePage({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },
    async onPageChange(page) {
      await this.StageStore.setPage(page);
    },
    async onPageSizeChange(size) {
      await this.StageStore.setPageSize(size);
    },
    async onSearchPreviousStage(criteria) {
      await this.StageStore.searchPreviousStages(criteria);
    },
  },
  mounted() {
    this.StageStore.clearSelectedStage();
    this.StageStore.fetchStagePage();
    this.ClassStore.fetchClasses();
  },
};
</script>

<style scoped></style>
