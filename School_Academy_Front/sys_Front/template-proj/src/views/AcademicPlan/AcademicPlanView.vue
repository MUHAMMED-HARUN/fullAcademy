<script setup>
import { computed, onMounted, ref } from "vue";
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import AcademicPlanList from "@/components/AcademicPlan/AcademicPlan/AcademicPlanList.vue";
import AcademicPlanCard from "@/components/AcademicPlan/AcademicPlan/AcademicPlanCard.vue";
import UpsertAcademicPlanComp from "@/components/AcademicPlan/AcademicPlan/UpsertAcademicPlanComp.vue";
import { useAcademicPlanStore } from "@/store/AcademicPlan/AcademicPlanStore";
import { useAuthStore } from "@/store/Auth/authStore";
import { useInstituteStore } from "@/store/Institute/useInstituteStore";
import { useProgramStore } from "@/store/Institute/useProgramStore";
import { useClassStore } from "@/store/Institute/ClassStore";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";
import { useStageStore } from "@/store/Progress/StageStore";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";

const academicPlanStore = useAcademicPlanStore();
const authStore = useAuthStore();
const instituteStore = useInstituteStore();
const programStore = useProgramStore();
const classStore = useClassStore();
const instituteClassStore = useInstituteClassStore();
const stageStore = useStageStore();
const stageInfoStore = useStageInfoStore();

const showUpsert = ref(false);
const showInfo = ref(false);
const upsertPlan = ref(null);
const infoPlan = ref(null);
const searchText = ref("");
const resultMessage = ref(null);
const resultType = ref(false);
const upsertErrors = ref({});

const canRead = computed(() =>
  authStore.hasPermission("Can_Read_AcademicPlan")
);
const canCreate = computed(() =>
  authStore.hasPermission("Can_Create_AcademicPlan")
);
const canUpdate = computed(() =>
  authStore.hasPermission("Can_Update_AcademicPlan")
);
const canDelete = computed(() =>
  authStore.hasPermission("Can_Delete_AcademicPlan")
);
const canActivate = computed(() =>
  authStore.hasPermission("Can_Update_AcademicPlan")
);

const showResult = (result, successMessage) => {
  resultMessage.value = result.success ? successMessage : result.errors;
  resultType.value = !result.success;
};

const buildNameFilter = (text) => {
  const safe = String(text ?? "")
    .replace(/[,*|^$!=()/]/g, " ")
    .trim();
  return safe ? `Name=*${safe}` : "";
};

const loadPlans = (params = {}) =>
  academicPlanStore.fetchAcademicPlans({
    Page: params.Page ?? 1,
    PageSize: params.PageSize ?? academicPlanStore.pageSize,
    Filter: params.Filter ?? academicPlanStore.currentFilter,
  });

const onSearch = () => {
  academicPlanStore.currentFilter = buildNameFilter(searchText.value);
  loadPlans({ Page: 1, Filter: academicPlanStore.currentFilter });
};

const onReset = () => {
  searchText.value = "";
  academicPlanStore.currentFilter = "";
  loadPlans({ Page: 1, Filter: "" });
};

const onPageChange = (page) => academicPlanStore.setPage(page);
const onPageSizeChange = (size) => academicPlanStore.setPageSize(size);

const onOpenCreate = () => {
  upsertErrors.value = {};
  upsertPlan.value = null;
  academicPlanStore.clearSelectedAcademicPlan();
  showUpsert.value = true;
};

const onOpenUpdate = async (item) => {
  upsertErrors.value = {};
  const result = await academicPlanStore.fetchAcademicPlanForUpsert(
    item.academicPlanID
  );
  if (!result.success) return showResult(result);
  upsertPlan.value = academicPlanStore.selectedAcademicPlan;
  showUpsert.value = true;
};

const onOpenInfo = async (item) => {
  const result = await academicPlanStore.fetchAcademicPlanForUpsert(
    item.academicPlanID
  );
  if (!result.success) return showResult(result);
  infoPlan.value = academicPlanStore.selectedAcademicPlan;
  showInfo.value = true;
};

const onSubmitUpsert = async (payload) => {
  const isUpdate = !!payload.academicPlanID;
  const result = isUpdate
    ? await academicPlanStore.updateAcademicPlan(payload)
    : await academicPlanStore.addAcademicPlan(payload);
  upsertErrors.value =
    !result.success && result.errors && typeof result.errors === "object"
      ? result.errors
      : {};
  if (result.success) {
    showResult(result, "تم حفظ الخطة بنجاح");
    showUpsert.value = false;
  } else {
    showResult(result);
  }
};

const onProviderSearch = (criteria) =>
  instituteStore.searchInstitutes(criteria);
const onProviderClear = () => instituteStore.clearInstituteLookup();

const onActivate = async (item) => {
  if (!confirm(`هل تريد تفعيل الخطة "${item.name}"؟`)) return;
  showResult(
    await academicPlanStore.activateAcademicPlan(item.academicPlanID),
    "تم تفعيل الخطة"
  );
};

const onDelete = async (item) => {
  if (!confirm(`هل تريد حذف الخطة "${item.name}"؟`)) return;
  showResult(
    await academicPlanStore.deleteAcademicPlan(item.academicPlanID),
    "تم حذف الخطة"
  );
};

const onPopupClose = () => {
  showUpsert.value = false;
  showInfo.value = false;
  upsertPlan.value = null;
  infoPlan.value = null;
  upsertErrors.value = {};
  academicPlanStore.clearSelectedAcademicPlan();
  instituteStore.clearInstituteLookup();
};

const loadScopeLists = () => {
  programStore.fetchPrograms();
  classStore.fetchClasses();
  instituteClassStore.fetchInstituteClasses();
  stageStore.fetchStages();
  stageInfoStore.fetchStageInfos();
};

onMounted(() => {
  loadPlans({ Page: 1, Filter: "" });
  loadScopeLists();
});
</script>

<template>
  <div class="academic-plan-view">
    <div class="search-bar">
      <input
        v-model="searchText"
        type="search"
        placeholder="ابحث باسم الخطة"
        @keydown.enter.prevent="onSearch"
      />
      <button
        type="button"
        :disabled="academicPlanStore.loading"
        @click="onSearch"
      >
        بحث
      </button>
      <button type="button" class="btn-outline" @click="onReset">
        إعادة تعيين
      </button>
    </div>

    <AcademicPlanList
      :items="academicPlanStore.allAcademicPlans"
      :can-read="canRead"
      :can-update="canUpdate"
      :can-activate="canActivate"
      :can-delete="canDelete"
      @info="onOpenInfo"
      @update="onOpenUpdate"
      @activate="onActivate"
      @delete="onDelete"
    />

    <PaginationComp
      :current-page="academicPlanStore.page"
      :page-size="academicPlanStore.pageSize"
      :total-count="academicPlanStore.totalCount"
      :total-pages="academicPlanStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onPopupClose">
      <UpsertAcademicPlanComp
        :errors="upsertErrors"
        :plan="upsertPlan"
        :provider-results="instituteStore.instituteLookupResults"
        :provider-loading="instituteStore.instituteLookupLoading"
        :provider-error="instituteStore.instituteLookupError"
        :programs="programStore.programs"
        :institute-classes="instituteClassStore.instituteClasses"
        :classes="classStore.classes"
        :stages="stageStore.stages"
        :stage-infos="stageInfoStore.StageInfos"
        @submit="onSubmitUpsert"
        @provider-search="onProviderSearch"
        @provider-clear="onProviderClear"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onPopupClose">
      <AcademicPlanCard :plan="infoPlan" />
    </PopapComp>

    <button
      v-if="canCreate"
      class="btn-add"
      type="button"
      @click="onOpenCreate"
    >
      إضافة خطة أكاديمية
    </button>

    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<style scoped>
.academic-plan-view {
  display: flex;
  flex-direction: column;
  gap: 15px;
}
.search-bar {
  display: flex;
  gap: 8px;
  align-items: center;
}
.search-bar input {
  flex: 1;
  max-width: 360px;
}
.btn-outline {
  background: transparent;
  border: 1px solid #6c757d;
  color: #6c757d;
  border-radius: 4px;
  padding: 6px 12px;
  cursor: pointer;
}
.btn-add {
  align-self: flex-start;
  padding: 10px 20px;
  background-color: #198754;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
</style>
