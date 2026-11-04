<template>
  <div class="space-y-4">
    <GenericFilterComp
      :fields="filterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />

    <GenericComboActions
      :actions="visibleActions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <ClassList
      :classes="pagedItems"
      :selected-ids="selectedIds"
      :can-read="AuthStore.hasPermission('Can_Read_Class')"
      :can-update="AuthStore.hasPermission('Can_Update_Class')"
      :can-delete="AuthStore.hasPermission('Can_Delete_Class')"
      :can-create-institute-class="
        AuthStore.hasPermission('Can_Create_InstituteClass')
      "
      :can-select="AuthStore.hasPermission('Can_Create_InstituteClass')"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-delete-class="OnDelete"
      @show-info-class="onShowInfo"
      @show-update-class="OnUpsertClass"
      @show-add-to-institue-class="onShowAddToInstitueClass"
    />

    <PaginationComp
      :current-page="page"
      :page-size="pageSize"
      :total-count="filteredCount"
      :total-pages="totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpserClassComp
        :errors="ClassStore.error"
        :program-data="ProgramStore.selectedProgram"
        @on-submit="OnSubmitUpsert"
        @on-search-program="onSearchProgram"
      />
    </PopapComp>
    <PopapComp v-if="showUpsertByRange" @close="onpopaCloseByRange">
      <UpsertInstituteClassByRangeComp
        :selected-classes="selectedClassesForRange"
        :institute-results="InstituteStore.instituteLookupResults"
        :institute-search-loading="InstituteStore.instituteLookupLoading"
        :institute-search-error="InstituteStore.instituteLookupError"
        :loading="InstituteClassStore.loading"
        :errors="InstituteClassStore.error"
        @on-submit="OnSubmitByRange"
        @search-institute="onSearchInstituteForRange"
        @clear-institute-search="InstituteStore.clearInstituteLookup"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <ClassCard :-data-req="ClassStore.selectedClass" />
    </PopapComp>
    <PopapComp v-if="showAddToInstitue" @close="onpopaClose">
      <UpserInstituteClassComp
        :errors="InstituteClassStore.error"
        :InstituteData="InstituteStore.selectedInstitute"
        :ClassData="ClassStore.selectedClass"
        :institute-results="InstituteStore.instituteLookupResults"
        :institute-search-loading="InstituteStore.instituteLookupLoading"
        :institute-search-error="InstituteStore.instituteLookupError"
        @search-institute="onSearchInstitute"
        @clear-institute-search="InstituteStore.clearInstituteLookup"
        @onSubmit="OnSubmitUpsertInstitueClass"
      />
    </PopapComp>

    <button
      v-if="AuthStore.hasPermission('Can_Create_Class')"
      type="button"
      class="h-10 rounded-[10px] bg-brand-500 px-4 text-sm font-medium text-white shadow-theme-xs transition-colors hover:bg-brand-600"
      @click="OnUpsertClass(null)"
    >
      إضافة صف
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import { useClassStore } from "@/store/Institute/ClassStore";
import { useInstituteStore } from "@/store/Institute/useInstituteStore";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";
import { useAuthStore } from "@/store/Auth/authStore";
import { useProgramStore } from "@/store/Institute/useProgramStore";
import ClassCard from "@/components/Institute/Class/ClassCard.vue";
import ClassList from "@/components/Institute/Class/ClassList.vue";
import UpserClassComp from "@/components/Institute/Class/UpserClassComp.vue";
import UpserInstituteClassComp from "@/components/Institute/InstituteClass/UpserInstituteClassComp.vue";
import UpsertInstituteClassByRangeComp from "@/components/Institute/InstituteClass/UpsertInstituteClassByRangeComp.vue";
import TResult from "@/Generic/TResult";
import { applyClientQuery } from "@/Generic/applyClientQuery";

export default {
  name: "ClassView",
  components: {
    ClassList,
    UpserClassComp,
    ClassCard,
    PopapComp,
    MessageComp,
    GenericComboActions,
    GenericFilterComp,
    PaginationComp,
    UpserInstituteClassComp,
    UpsertInstituteClassByRangeComp,
  },
  data() {
    return {
      ClassStore: useClassStore(),
      InstituteClassStore: useInstituteClassStore(),
      ProgramStore: useProgramStore(),
      InstituteStore: useInstituteStore(),
      AuthStore: useAuthStore(),
      showUpsert: false,
      showAddToInstitue: false,
      showUpsertByRange: false,
      selectedClassesForRange: [],
      showInfo: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      page: 1,
      pageSize: 10,
      filterPayload: {},
      filterSchema: [
        { key: "name", label: "اسم الصف", type: "text", operator: "=*" },
        { key: "level", label: "المستوى", type: "text", operator: "=*" },
        { key: "programName", label: "البرنامج", type: "text", operator: "=*" },
      ],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class: "bg-red-500 hover:bg-red-600",
        },
        {
          key: "addByRange",
          label: "إضافة عدة صفوف إلى معهد",
          minSelection: 1,
          class: "bg-brand-500 hover:bg-brand-600",
        },
      ],
    };
  },
  computed: {
    visibleActions() {
      const requiredPermissions = {
        info: "Can_Read_Class",
        update: "Can_Update_Class",
        delete: "Can_Delete_Class",
        addByRange: "Can_Create_InstituteClass",
      };
      return this.actions.filter((action) =>
        this.AuthStore.hasPermission(requiredPermissions[action.key])
      );
    },
    queryResult() {
      return applyClientQuery(
        this.ClassStore.classes,
        {
          rawValues: this.filterPayload.rawValues,
          orderBy: this.filterPayload.orderBy,
          page: this.page,
          pageSize: this.pageSize,
        },
        {
          containsFields: ["name", "programName"],
          equalsFields: ["level"],
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
      return this.ClassStore.classes.filter((item) =>
        this.selectedIds.includes(item.classID)
      );
    },
  },
  methods: {
    async OnUpsertClass(e) {
      if (e) {
        this.ClassStore.selectedClass = e;
        await this.onSearchProgram(0, this.ClassStore.selectedClass.programID);
      } else {
        this.ProgramStore.selectedProgram = null;
      }
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.classID) {
        resVal = await this.ClassStore.updateClass(e);
      } else {
        resVal = await this.ClassStore.addClass(e);
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
    async onSearchProgram(type, val) {
      await this.ProgramStore.fetchProgrameBy({
        Param: val,
        ParamType: type,
      });
    },
    async OnDelete(e) {
      this.ClassStore.DeleteClass(e.classID);
      this.selectedIds = this.selectedIds.filter((id) => id !== e.classID);
    },
    async onShowInfo(e) {
      this.ClassStore.selectedClass = this.ClassStore.classes.find(
        (c) => c.classID === e.classID
      );
      this.showInfo = true;
    },
    async onShowAddToInstitueClass(e) {
      this.ClassStore.selectedClass = this.ClassStore.classes.find(
        (c) => c.classID === e.classID
      );
      this.showAddToInstitue = true;
    },
    async onSearchInstitute(criteria) {
      await this.InstituteStore.searchInstitutes({ value: criteria.value });
    },
    async onSearchInstituteForRange(criteria) {
      await this.InstituteStore.searchInstitutes({ value: criteria.value });
    },
    async OnSubmitUpsertInstitueClass(e) {
      var resVal = new TResult();
      resVal = await this.InstituteClassStore.addInstituteClass(e);
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showAddToInstitue = false;
        this.ClassStore.selectedClass = null;
        this.InstituteStore.selectedInstitute = null;
        this.InstituteStore.clearInstituteLookup();
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async OnSubmitByRange(e) {
      const resVal = await this.InstituteClassStore.addInstituteClassByRange(e);
      if (resVal && resVal.success) {
        const result = resVal.data || {};
        const skippedIDs = new Set(
          (result.skippedAlreadyLinkedClassIDs || []).map((id) =>
            String(id).toLowerCase()
          )
        );
        const skippedNames = this.selectedClassesForRange
          .filter((cls) => skippedIDs.has(String(cls.classID).toLowerCase()))
          .map((cls) => cls.name);
        this.resultMessage = `تم ربط ${result.addedCount || 0} صف`;
        if (skippedIDs.size) {
          this.resultMessage += `؛ تم تجاوز ${skippedIDs.size} صف مرتبط مسبقاً`;
          if (skippedNames.length) {
            this.resultMessage += `: ${skippedNames.join("، ")}`;
          }
        }
        this.resultType = false;
        this.showUpsertByRange = false;
        this.selectedClassesForRange = [];
        this.selectedIds = [];
        this.InstituteStore.clearInstituteLookup();
      } else {
        this.resultMessage = (resVal && resVal.errors) || "فشل الإضافة";
        this.resultType = true;
      }
    },
    onpopaCloseByRange() {
      this.showUpsertByRange = false;
      this.selectedClassesForRange = [];
      this.InstituteStore.clearInstituteLookup();
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.ClassStore.selectedClass = null;
      this.showAddToInstitue = false;
      this.ClassStore.selectedClass = null;
      this.InstituteStore.selectedInstitute = null;
      this.InstituteStore.clearInstituteLookup();
    },
    onFilterSearch(payload) {
      this.filterPayload = payload;
      this.page = 1;
    },
    onFilterReset() {
      this.filterPayload = {};
      this.page = 1;
    },
    onPageChange(newPage) {
      this.page = newPage;
    },
    onPageSizeChange(newSize) {
      this.pageSize = newSize;
      this.page = 1;
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.pagedItems.map((item) => item.classID)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.classID);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.classID);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          this.onShowInfo(singleItem);
          break;
        case "update":
          await this.OnUpsertClass(singleItem);
          break;
        case "delete":
          await this.OnDelete(singleItem);
          this.selectedIds = [];
          break;
        case "addByRange":
          this.selectedClassesForRange = selectedItems.map((cls) => ({
            classID: cls.classID,
            name: cls.name,
            programName: cls.programName,
          }));
          this.showUpsertByRange = true;
          break;
      }
    },
  },
  mounted() {
    this.ClassStore.fetchClasses();
  },
};
</script>

<style scoped></style>
