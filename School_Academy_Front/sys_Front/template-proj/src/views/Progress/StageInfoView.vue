<template>
  <div class="space-y-4" dir="rtl">
    <GenericFilterComp
      :fields="filterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <p
      v-if="StageInfoStore.loading"
      class="text-sm text-[var(--text-muted)]"
      role="status"
    >
      جارٍ تحميل تفاصيل المراحل…
    </p>
    <p v-else-if="StageInfoStore.error" class="alert-error" role="alert">
      {{ StageInfoStore.error }}
    </p>
    <p
      v-else-if="StageInfoStore.stageInfoPageItems.length === 0"
      class="rounded-lg border border-dashed border-[var(--border-color)] p-4 text-center text-[var(--text-muted)]"
      role="status"
    >
      لا توجد تفاصيل مطابقة.
    </p>

    <StageInfoList
      v-if="StageInfoStore.stageInfoPageItems.length > 0"
      :stage-infos="StageInfoStore.stageInfoPageItems"
      :can-read="canReadStageInfo"
      :can-update="canUpdateStageInfo"
      :can-delete="canDeleteStageInfo"
      @show-delete="OnDeleteStageInfo"
      @show-info="onShowInfoStageInfo"
      @show-update="OnUpsertStageInfo"
    />

    <PaginationComp
      :current-page="StageInfoStore.page"
      :page-size="StageInfoStore.pageSize"
      :total-count="StageInfoStore.totalCount"
      :total-pages="StageInfoStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertStageInfoComp
        :errors="StageInfoStore.error"
        :stage-data="StageStore.selectedStage"
        :matter-data="SessionStore.selectedSession"
        :lock-stage="lockStage"
        :stage-results="stageLookupResults"
        :stage-loading="stageLookupLoading"
        :stage-error="stageLookupError"
        :matter-results="matterLookupResults"
        :matter-loading="matterLookupLoading"
        :matter-error="matterLookupError"
        :saving="StageInfoStore.loading"
        @search-stage="onSearchStage"
        @select-stage="onSelectStage"
        @clear-stage="onClearStage"
        @search-matter="onSearchMatter"
        @select-matter="onSelectMatter"
        @clear-matter="onClearMatter"
        @submit="OnSubmitUpsert"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onpopaClose">
      <StageInfoCard :DataReq="StageInfoStore.selectedStageInfo" />
    </PopapComp>

    <button
      v-if="canCreateStageInfo"
      type="button"
      class="btn btn-primary"
      @click="OnNewStageInfo"
    >
      إضافة تفاصيل مرحلة
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import { useStageStore } from "@/store/Progress/StageStore";
import { useSessionStore } from "@/store/Progress/SessionStore";
import { useAuthStore } from "@/store/Auth/authStore";
import MessageComp from "@/components/general/messageComp.vue";
import StageInfoList from "@/components/Progress/StageInfo/StageInfoList.vue";
import UpsertStageInfoComp from "@/components/Progress/StageInfo/UpsertStageInfoComp.vue";
import StageInfoCard from "@/components/Progress/StageInfo/StageInfoCard.vue";

export default {
  name: "StageInfoView",
  components: {
    StageInfoList,
    StageInfoCard,
    UpsertStageInfoComp,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      StageInfoStore: useStageInfoStore(),
      StageStore: useStageStore(),
      SessionStore: useSessionStore(),
      AuthStore: useAuthStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      stageLookupResults: [],
      stageLookupLoading: false,
      stageLookupError: null,
      stageLookupRequestVersion: 0,
      matterLookupResults: [],
      matterLookupLoading: false,
      matterLookupError: null,
      matterLookupRequestVersion: 0,
      filterSchema: [
        {
          key: "StageName",
          label: "اسم المرحلة",
          type: "text",
          operator: "=*",
        },
        {
          key: "MatterName",
          label: "اسم المادة",
          type: "text",
          operator: "=*",
        },
      ],
    };
  },
  computed: {
    lockStage() {
      return Boolean(this.$route.query.stageID);
    },
    canReadStageInfo() {
      return this.AuthStore.hasPermission("Can_Read_StageInfo");
    },
    canCreateStageInfo() {
      return this.AuthStore.hasPermission("Can_Create_StageInfo");
    },
    canUpdateStageInfo() {
      return this.AuthStore.hasPermission("Can_Update_StageInfo");
    },
    canDeleteStageInfo() {
      return this.AuthStore.hasPermission("Can_Delete_StageInfo");
    },
  },
  methods: {
    async OnNewStageInfo() {
      this.StageInfoStore.clearSelectedStageInfo();
      this.SessionStore.selectedSession = null;
      this.clearLookupState();

      const stageID = String(this.$route.query.stageID || "");
      if (stageID && this.StageStore.selectedStage?.stageID !== stageID) {
        await this.StageStore.fetchStagetBy({ Param: stageID, ParamType: 0 });
      } else if (!stageID) {
        this.StageStore.clearSelectedStage();
      }

      this.showUpsert = true;
    },

    async OnUpsertStageInfo(stageInfo) {
      this.StageInfoStore.selectedStageInfo = stageInfo;
      this.clearLookupState();

      const stageRequest = this.StageStore.fetchStagetBy({
        Param: stageInfo.stageID,
        ParamType: 0,
      });
      const matterRequest = this.SessionStore.fetchSessiontBy({
        Param: stageInfo.matterID,
        ParamType: 0,
      });
      await Promise.all([stageRequest, matterRequest]);
      this.showUpsert = true;
    },

    async OnSubmitUpsert(payload) {
      const result = payload.stageInfoID
        ? await this.StageInfoStore.updateStageInfo(payload)
        : await this.StageInfoStore.addStageInfo(payload);

      if (result.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
        this.StageInfoStore.clearSelectedStageInfo();
      } else {
        this.resultMessage = result.errors;
        this.resultType = true;
      }
    },

    async OnDeleteStageInfo(stageInfo) {
      if (!window.confirm("هل أنت متأكد من حذف تفاصيل المرحلة؟")) return;
      const result = await this.StageInfoStore.DeleteStageInfo(
        stageInfo.stageInfoID
      );
      this.resultMessage = result.success ? "تم الحذف" : result.errors;
      this.resultType = !result.success;
    },

    onShowInfoStageInfo(stageInfo) {
      this.StageInfoStore.selectedStageInfo = stageInfo;
      this.showInfo = true;
    },

    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.StageInfoStore.clearSelectedStageInfo();
      this.SessionStore.selectedSession = null;
      this.clearLookupState();
    },

    async onFilterSearch(payload) {
      await this.StageInfoStore.fetchStageInfoPage({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },

    async onFilterReset() {
      await this.StageInfoStore.fetchStageInfoPage({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },

    async onPageChange(page) {
      await this.StageInfoStore.setPage(page);
    },

    async onPageSizeChange(pageSize) {
      await this.StageInfoStore.setPageSize(pageSize);
    },

    async onSearchStage(searchTerm) {
      const term = String(searchTerm || "")
        .trim()
        .replace(/[,*|^$!=()/]/g, " ")
        .trim();
      const requestVersion = ++this.stageLookupRequestVersion;
      this.stageLookupResults = [];
      this.stageLookupError = null;
      if (!term) {
        this.stageLookupLoading = false;
        return;
      }

      this.stageLookupLoading = true;
      try {
        const result = await this.StageStore.fetchStagePage({
          Page: 1,
          PageSize: 10,
          Filter: `StageName=*${term}`,
          OrderBy: "",
        });
        if (requestVersion !== this.stageLookupRequestVersion) return;
        if (result.success) this.stageLookupResults = result.data?.items || [];
        else this.stageLookupError = result.errors;
      } catch (error) {
        if (requestVersion === this.stageLookupRequestVersion)
          this.stageLookupError = error?.message || "تعذر البحث عن المرحلة";
      } finally {
        if (requestVersion === this.stageLookupRequestVersion)
          this.stageLookupLoading = false;
      }
    },

    onSelectStage(stage) {
      this.StageStore.selectedStage = stage;
    },

    onClearStage() {
      this.StageStore.clearSelectedStage();
      this.stageLookupRequestVersion += 1;
      this.stageLookupResults = [];
      this.stageLookupError = null;
      this.stageLookupLoading = false;
    },

    async onSearchMatter(searchTerm) {
      const term = String(searchTerm || "")
        .trim()
        .replace(/[,*|^$!=()/]/g, " ")
        .trim();
      const requestVersion = ++this.matterLookupRequestVersion;
      this.matterLookupResults = [];
      this.matterLookupError = null;
      if (!term) {
        this.matterLookupLoading = false;
        return;
      }

      this.matterLookupLoading = true;
      try {
        const result = await this.SessionStore.fetchSessions({
          Page: 1,
          PageSize: 10,
          Filter: `SessionName=*${term},SessionType=1`,
          OrderBy: "",
        });
        if (requestVersion !== this.matterLookupRequestVersion) return;
        if (result.success) this.matterLookupResults = result.data?.items || [];
        else this.matterLookupError = result.errors;
      } catch (error) {
        if (requestVersion === this.matterLookupRequestVersion)
          this.matterLookupError = error?.message || "تعذر البحث عن المادة";
      } finally {
        if (requestVersion === this.matterLookupRequestVersion)
          this.matterLookupLoading = false;
      }
    },

    onSelectMatter(matter) {
      this.SessionStore.selectedSession = matter;
    },

    onClearMatter() {
      this.SessionStore.selectedSession = null;
      this.matterLookupRequestVersion += 1;
      this.matterLookupResults = [];
      this.matterLookupError = null;
      this.matterLookupLoading = false;
    },

    clearLookupState() {
      this.onClearStage();
      this.onClearMatter();
    },

    async loadStageContextFromRoute() {
      const stageID = String(this.$route.query.stageID || "");
      if (!stageID) {
        this.StageStore.clearSelectedStage();
        return;
      }
      if (this.StageStore.selectedStage?.stageID !== stageID) {
        await this.StageStore.fetchStagetBy({ Param: stageID, ParamType: 0 });
      }
    },
  },
  async mounted() {
    await this.loadStageContextFromRoute();
    await this.StageInfoStore.fetchStageInfoPage({
      Page: 1,
      Filter: "",
      OrderBy: "",
      StageID: String(this.$route.query.stageID || ""),
    });
    if (this.$route.query.stageID && this.canCreateStageInfo)
      await this.OnNewStageInfo();
  },
  beforeRouteLeave() {
    this.SessionStore.fetchSessions({ Page: 1, Filter: "", OrderBy: "" });
    return true;
  },
};
</script>
