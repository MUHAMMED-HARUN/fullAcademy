<template>
  <div class="finance-view-container" dir="rtl">
    <GenericFilterComp
      :fields="filterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <FeeStructureList
      :items="FeeStructureStore.feeStructures"
      :selected-ids="selectedIds"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
    />
    <GenericComboActions
      :actions="visibleActions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />
    <PaginationComp
      :current-page="page"
      :page-size="pageSize"
      :total-count="FeeStructureStore.totalCount"
      :total-pages="totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onPopupClose">
      <UpsertFeeStructureComp
        :errors="FeeStructureStore.error"
        :initial-fee-structure="editingFeeStructure"
        :plan-locked="Boolean(editingFeeStructure?.isUsed)"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onPopupClose"
      ><FeeStructureCard :-data-req="FeeStructureStore.selectedFeeStructure"
    /></PopapComp>
    <button
      v-if="AuthStore.hasPermission('Can_Create_FeeStructure')"
      class="btn-add"
      @click="OnUpsert(null)"
    >
      إضافة تعرفة
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import FeeStructureList from "@/components/Finance/FeeStructure/FeeStructureList.vue";
import FeeStructureCard from "@/components/Finance/FeeStructure/FeeStructureCard.vue";
import UpsertFeeStructureComp from "@/components/Finance/FeeStructure/UpsertFeeStructureComp.vue";
import { useFeeStructureStore } from "@/store/Finance/FeeStructureStore";
import { useAuthStore } from "@/store/Auth/authStore";
import { FeeTypeOptions } from "@/Generic/FinanceEnums";

export default {
  name: "FeeStructureView",
  components: {
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
    GenericComboActions,
    FeeStructureList,
    FeeStructureCard,
    UpsertFeeStructureComp,
  },
  data() {
    return {
      FeeStructureStore: useFeeStructureStore(),
      AuthStore: useAuthStore(),
      showUpsert: false,
      showInfo: false,
      editingFeeStructure: null,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      page: 1,
      pageSize: 10,
      filterPayload: {},
      filterSchema: [
        { key: "name", label: "اسم التعرفة", type: "text", operator: "=*" },
        {
          key: "academicPlanName",
          label: "الخطة الأكاديمية",
          type: "text",
          operator: "=*",
        },
        {
          key: "scopeName",
          label: "النطاق الدراسي",
          type: "text",
          operator: "=*",
        },
        {
          key: "feeType",
          label: "نوع الرسوم",
          type: "select",
          options: FeeTypeOptions,
        },
        {
          key: "planStatus",
          label: "حالة الخطة",
          type: "select",
          options: [
            { value: 1, label: "مسودة" },
            { value: 2, label: "فعالة" },
            { value: 3, label: "مؤرشفة" },
          ],
        },
      ],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "update", label: "تعديل", minSelection: 1, maxSelection: 1 },
        { key: "delete", label: "حذف", minSelection: 1, class: "btn-danger" },
      ],
    };
  },
  computed: {
    visibleActions() {
      const required = {
        info: "Can_Read_FeeStructure",
        update: "Can_Update_FeeStructure",
        delete: "Can_Delete_FeeStructure",
      };
      return this.actions.filter((action) =>
        this.AuthStore.hasPermission(required[action.key])
      );
    },
    selectedItems() {
      return this.FeeStructureStore.feeStructures.filter((item) =>
        this.selectedIds.includes(item.feeStructureID)
      );
    },
    totalPages() {
      return Math.max(
        1,
        Math.ceil(this.FeeStructureStore.totalCount / this.pageSize)
      );
    },
  },
  methods: {
    buildGridifyFilter(rawValues = {}) {
      const terms = [];
      const fields = {
        name: "Name",
        academicPlanName: "AcademicPlanName",
        scopeName: "ScopeName",
      };
      for (const [key, field] of Object.entries(fields)) {
        const value = String(rawValues[key] ?? "").trim();
        if (value) terms.push(`${field}=*${value.replace(/[,*|]/g, " ")}`);
      }
      if (rawValues.feeType !== "" && rawValues.feeType != null)
        terms.push(`FeeType=${rawValues.feeType}`);
      if (rawValues.planStatus !== "" && rawValues.planStatus != null)
        terms.push(`PlanStatus=${rawValues.planStatus}`);
      return terms.join(",");
    },
    buildGridifyOrderBy(value) {
      if (!value) return "";
      const [field, direction = "asc"] = value.trim().split(/\s+/);
      const allowed = {
        name: "Name",
        academicPlanName: "AcademicPlanName",
        scopeName: "ScopeName",
        feeType: "FeeType",
        amount: "Amount",
        planStatus: "PlanStatus",
      };
      const mappedField = allowed[field];
      return mappedField
        ? `${mappedField} ${
            direction.toLowerCase() === "desc" ? "desc" : "asc"
          }`
        : "";
    },
    async loadPage() {
      const result = await this.FeeStructureStore.fetchFeeStructurePage({
        Page: this.page,
        PageSize: this.pageSize,
        Filter: this.buildGridifyFilter(this.filterPayload.rawValues || {}),
        OrderBy: this.buildGridifyOrderBy(this.filterPayload.orderBy),
      });
      if (!result.success) {
        this.resultMessage = result.errors;
        this.resultType = true;
      }
    },
    async onPageChange(page) {
      this.page = page;
      await this.loadPage();
    },
    async onPageSizeChange(size) {
      this.pageSize = size;
      this.page = 1;
      await this.loadPage();
    },
    async onFilterSearch(payload) {
      this.filterPayload = payload || {};
      this.page = 1;
      await this.loadPage();
    },
    async onFilterReset() {
      this.filterPayload = {};
      this.page = 1;
      await this.loadPage();
    },
    onToggleSelectAll(checked) {
      this.selectedIds = checked
        ? this.FeeStructureStore.feeStructures.map(
            (item) => item.feeStructureID
          )
        : [];
    },
    onToggleSelectRow(item) {
      const i = this.selectedIds.indexOf(item.feeStructureID);
      if (i >= 0) this.selectedIds.splice(i, 1);
      else this.selectedIds.push(item.feeStructureID);
    },
    async handleComboAction(key, items) {
      const item = items[0];
      if (key === "info") {
        this.FeeStructureStore.selectedFeeStructure = item;
        this.showInfo = true;
      }
      if (key === "update") await this.OnUpsert(item);
      if (key === "delete") await this.OnDeleteMultiple(items);
    },
    async OnUpsert(item) {
      this.editingFeeStructure = item || null;
      this.showUpsert = true;
    },
    async OnSubmitUpsert(data) {
      const result = data.feeStructureID
        ? await this.FeeStructureStore.updateFeeStructure(data)
        : await this.FeeStructureStore.addFeeStructure(data);
      if (result.success) {
        this.showUpsert = false;
        this.resultMessage = "تم الحفظ بنجاح";
        this.resultType = false;
        await this.loadPage();
      } else {
        this.resultMessage = result.errors;
        this.resultType = true;
      }
    },
    async OnDeleteMultiple(items) {
      if (!confirm(`هل أنت متأكد من حذف ${items.length} سجلات المحددة؟`))
        return;
      for (const item of items) {
        const result = await this.FeeStructureStore.DeleteFeeStructure(
          item.feeStructureID
        );
        if (!result.success) {
          this.resultMessage = result.errors;
          this.resultType = true;
          return;
        }
      }
      this.selectedIds = [];
      this.resultMessage = "تم الحذف بنجاح";
      this.resultType = false;
      await this.loadPage();
    },
    onPopupClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.editingFeeStructure = null;
      this.FeeStructureStore.clearSelectedFeeStructure();
    },
  },
  mounted() {
    this.loadPage();
  },
};
</script>

<style scoped>
.finance-view-container {
  display: flex;
  flex-direction: column;
  gap: 15px;
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
