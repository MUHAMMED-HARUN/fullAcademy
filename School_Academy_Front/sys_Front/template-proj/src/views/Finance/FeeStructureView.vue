<template>
  <div class="finance-view-container">
    <GenericFilterComp
      :fields="filterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <FeeStructureList
      :items="pagedItems"
      :selected-ids="selectedIds"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
    />

    <GenericComboActions
      :actions="actions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <PaginationComp
      :current-page="page"
      :page-size="pageSize"
      :total-count="filteredCount"
      :total-pages="totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onPopupClose">
      <UpsertFeeStructureComp
        :errors="FeeStructureStore.error"
        :program-data="ProgramStore.selectedProgram"
        :class-data="ClassStore.selectedClass"
        :stage-data="StageStore.selectedStage"
        :stage-infos="StageInfoStore.StageInfos"
        @on-submit="OnSubmitUpsert"
        @on-search-program="onSearchProgram"
        @on-search-class="onSearchClass"
        @on-search-stage="onSearchStage"
      />
    </PopapComp>

    <PopapComp v-if="showInfo" @close="onPopupClose">
      <FeeStructureCard :-data-req="FeeStructureStore.selectedFeeStructure" />
    </PopapComp>

    <PopapComp v-if="showEnroll" @close="onPopupClose">
      <UpsertStudentInvoiceComp
        :errors="StudentInvoiceStore.error"
        :student-data="StudentStore.selectedStudent"
        :fee-structures="FeeStructureStore.feeStructures"
        :initial-invoice="enrollInvoice"
        :lock-fee-structure="true"
        @on-submit="OnSubmitEnroll"
        @on-search-student="onSearchStudent"
      />
    </PopapComp>

    <button class="btn-add" @click="OnUpsert(null)">إضافة خطة رسوم</button>
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
import UpsertStudentInvoiceComp from "@/components/Finance/StudentInvoice/UpsertStudentInvoiceComp.vue";
import { useFeeStructureStore } from "@/store/Finance/FeeStructureStore";
import { useStudentInvoiceStore } from "@/store/Finance/StudentInvoiceStore";
import { useProgramStore } from "@/store/Institute/useProgramStore";
import { useClassStore } from "@/store/Institute/ClassStore";
import { useStageStore } from "@/store/Progress/StageStore";
import { useStageInfoStore } from "@/store/Progress/StageInfoStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
import { applyClientQuery } from "@/Generic/applyClientQuery";
import { FeeTypeOptions } from "@/Generic/FinanceEnums";
import ClassMapper from "@/Mapper/Institute/ClassMapper";
import TResult from "@/Generic/TResult";

export default {
  name: "FeeStructureView",
  components: {
    GenericFilterComp,
    PaginationComp,
    GenericComboActions,
    FeeStructureList,
    FeeStructureCard,
    UpsertFeeStructureComp,
    UpsertStudentInvoiceComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      FeeStructureStore: useFeeStructureStore(),
      StudentInvoiceStore: useStudentInvoiceStore(),
      ProgramStore: useProgramStore(),
      ClassStore: useClassStore(),
      StageStore: useStageStore(),
      StageInfoStore: useStageInfoStore(),
      StudentStore: useStudentStore(),
      showUpsert: false,
      showInfo: false,
      showEnroll: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      page: 1,
      pageSize: 10,
      filterPayload: {},
      enrollInvoice: null,
      filterSchema: [
        { key: "name", label: "اسم الخطة", type: "text", operator: "=*" },
        {
          key: "feeType",
          label: "نوع الرسوم",
          type: "select",
          options: FeeTypeOptions,
        },
        { key: "programName", label: "البرنامج", type: "text", operator: "=*" },
        { key: "className", label: "الصف", type: "text", operator: "=*" },
        { key: "stageName", label: "المرحلة", type: "text", operator: "=*" },
      ],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "enroll",
          label: "تسجيل في هذه الخطة",
          minSelection: 1,
          maxSelection: 1,
          class: "btn-default",
        },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class: "btn-danger",
        },
      ],
    };
  },
  computed: {
    queryResult() {
      return applyClientQuery(
        this.FeeStructureStore.feeStructures,
        {
          rawValues: this.filterPayload.rawValues,
          orderBy: this.filterPayload.orderBy,
          page: this.page,
          pageSize: this.pageSize,
        },
        {
          containsFields: ["name", "programName", "className", "stageName"],
          equalsFields: ["feeType"],
        }
      );
    },
    pagedItems() {
      return this.queryResult.items;
    },
    filteredCount() {
      return this.queryResult.count;
    },
    totalPages() {
      return Math.ceil(this.filteredCount / this.pageSize) || 1;
    },
    selectedItems() {
      return this.FeeStructureStore.feeStructures.filter((item) =>
        this.selectedIds.includes(item.feeStructureID)
      );
    },
  },
  methods: {
    async onPageChange(newPage) {
      this.page = newPage;
    },
    async onPageSizeChange(newSize) {
      this.pageSize = newSize;
      this.page = 1;
    },
    onFilterSearch(payload) {
      this.filterPayload = payload;
      this.page = 1;
    },
    onFilterReset() {
      this.filterPayload = {};
      this.page = 1;
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.pagedItems.map((item) => item.feeStructureID)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.feeStructureID);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.feeStructureID);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          this.onShowInfo(singleItem);
          break;
        case "update":
          await this.OnUpsert(singleItem);
          break;
        case "enroll":
          this.onEnroll(singleItem);
          break;
        case "delete":
          await this.OnDeleteMultiple(selectedItems);
          break;
      }
    },
    async OnUpsert(item) {
      if (item) {
        this.FeeStructureStore.selectedFeeStructure = item;
        if (item.programID) {
          await this.onSearchProgram(0, item.programID);
        }
        if (item.classID) {
          await this.onSearchClass(0, item.classID);
        }
        if (item.stageID) {
          await this.onSearchStage(0, item.stageID);
        }
      } else {
        this.FeeStructureStore.clearSelectedFeeStructure();
        this.ProgramStore.selectedProgram = null;
        this.ClassStore.selectedClass = null;
        this.StageStore.clearSelectedStage();
      }
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      let resVal = new TResult();
      if (e.feeStructureID) {
        resVal = await this.FeeStructureStore.updateFeeStructure(e);
      } else {
        resVal = await this.FeeStructureStore.addFeeStructure(e);
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
    onShowInfo(item) {
      this.FeeStructureStore.selectedFeeStructure = item;
      this.showInfo = true;
    },
    onEnroll(item) {
      this.StudentInvoiceStore.clearSelectedInvoice();
      this.StudentStore.selectedStudent = null;
      this.enrollInvoice = {
        feeStructureID: item.feeStructureID,
        originalAmount: item.amount,
        discountAmount: 0,
        issueDate: new Date().toISOString().substring(0, 10),
        dueDate: new Date().toISOString().substring(0, 10),
      };
      this.showEnroll = true;
    },
    async OnSubmitEnroll(e) {
      const resVal = await this.StudentInvoiceStore.addInvoice(e);
      if (resVal.success) {
        this.resultMessage = "تم تسجيل الطالب في الخطة";
        this.resultType = false;
        this.showEnroll = false;
        this.$router.push({
          path: "/studentInvoice",
          query: { feeStructureID: e.feeStructureID },
        });
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async OnDeleteMultiple(items) {
      if (!confirm(`هل أنت متأكد من حذف ${items.length} سجلات المحددة؟`))
        return;
      for (const item of items) {
        await this.FeeStructureStore.DeleteFeeStructure(item.feeStructureID);
      }
      this.selectedIds = [];
      this.resultMessage = "تم الحذف بنجاح";
      this.resultType = false;
    },
    async onSearchProgram(type, val) {
      await this.ProgramStore.fetchProgrameBy({
        Param: val,
        ParamType: type,
      });
    },
    async onSearchClass(type, val) {
      await this.ClassStore.fetchClassBy({
        Param: val,
        ParamType: type,
      });
      if (this.ClassStore.selectedClass?.ClassID) {
        this.ClassStore.selectedClass = ClassMapper.MapFromApiDTO(
          this.ClassStore.selectedClass
        );
      }
    },
    async onSearchStage(type, val) {
      await this.StageStore.fetchStagetBy({
        Param: val,
        ParamType: type,
      });
    },
    async onSearchStudent(type, val) {
      await this.StudentStore.fetchStudentBy({
        Param: val,
        ParamType: type,
      });
    },
    onPopupClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.showEnroll = false;
      this.enrollInvoice = null;
      this.FeeStructureStore.clearSelectedFeeStructure();
      this.StudentInvoiceStore.clearSelectedInvoice();
      this.StudentStore.selectedStudent = null;
    },
  },
  mounted() {
    this.FeeStructureStore.fetchFeeStructures();
    this.StageInfoStore.fetchStageInfos();
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
