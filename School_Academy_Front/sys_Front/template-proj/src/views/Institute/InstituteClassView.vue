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

    <InstituteClassList
      :institute-classes="pagedItems"
      :selected-ids="selectedIds"
      :can-read="AuthStore.hasPermission('Can_Read_InstituteClass')"
      :can-delete="AuthStore.hasPermission('Can_Delete_InstituteClass')"
      :can-add-group="AuthStore.isAdmin"
      :can-select="
        AuthStore.hasPermission('Can_Delete_InstituteClass') ||
        AuthStore.isAdmin
      "
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-delete-institute-class="OnDelete"
      @show-info-institute-class="onShowInfo"
      @show-add-group="onShowAddGroup"
    />

    <PaginationComp
      :current-page="page"
      :page-size="pageSize"
      :total-count="filteredCount"
      :total-pages="totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />

    <PopapComp v-if="showInfo" @close="onpopaClose">
      <InstituteClassCard
        :-data-req="InstituteClassStore.selectedInstituteClass"
      />
    </PopapComp>
    <PopapComp v-if="showUpsertGroup" @close="onpopaClose">
      <UpsertGroupComp
        :errors="GroupStore.error"
        :institute-class-data="InstituteClassStore.selectedInstituteClass"
        @on-submit="OnSubmitUpsertGroup"
        @on-search-institute-class="onSearchInstituteClass"
      />
    </PopapComp>

    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import MessageComp from "@/components/general/messageComp.vue";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";
import { useInstituteStore } from "@/store/Institute/useInstituteStore";
import { useClassStore } from "@/store/Institute/ClassStore";
import { useGroupStore } from "@/store/Institute/GroupStore";
import { useAuthStore } from "@/store/Auth/authStore";
import InstituteClassList from "@/components/Institute/InstituteClass/InstituteClassList.vue";
import InstituteClassCard from "@/components/Institute/InstituteClass/InstituteClassCard.vue";
import UpsertGroupComp from "@/components/Institute/Group/UpsertGroupComp.vue";
import { applyClientQuery } from "@/Generic/applyClientQuery";

export default {
  name: "InstituteClassView",
  components: {
    InstituteClassList,
    InstituteClassCard,
    UpsertGroupComp,
    PopapComp,
    MessageComp,
    GenericComboActions,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      InstituteStore: useInstituteStore(),
      InstituteClassStore: useInstituteClassStore(),
      ClassStore: useClassStore(),
      GroupStore: useGroupStore(),
      AuthStore: useAuthStore(),
      showInfo: false,
      showUpsertGroup: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      page: 1,
      pageSize: 10,
      filterPayload: {},
      filterSchema: [
        {
          key: "instituteName",
          label: "المعهد",
          type: "text",
          operator: "=*",
        },
        { key: "className", label: "الصف", type: "text", operator: "=*" },
        { key: "programName", label: "البرنامج", type: "text", operator: "=*" },
        {
          key: "classLevel",
          label: "مستوى الصف",
          type: "text",
          operator: "=*",
        },
      ],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class: "bg-red-500 hover:bg-red-600",
        },
        {
          key: "addGroup",
          label: "إضافة مجموعة",
          minSelection: 1,
          maxSelection: 1,
        },
      ],
    };
  },
  computed: {
    visibleActions() {
      const permissions = {
        info: this.AuthStore.hasPermission("Can_Read_InstituteClass"),
        delete: this.AuthStore.hasPermission("Can_Delete_InstituteClass"),
        addGroup: this.AuthStore.isAdmin,
      };
      return this.actions.filter((action) => permissions[action.key]);
    },
    queryResult() {
      return applyClientQuery(
        this.InstituteClassStore.instituteClasses,
        {
          rawValues: this.filterPayload.rawValues,
          orderBy: this.filterPayload.orderBy,
          page: this.page,
          pageSize: this.pageSize,
        },
        {
          containsFields: ["instituteName", "className", "programName"],
          equalsFields: ["classLevel"],
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
      return this.InstituteClassStore.instituteClasses.filter((ic) =>
        this.selectedIds.includes(ic.instituteClassID)
      );
    },
  },
  methods: {
    async OnDelete(e) {
      const resVal = await this.InstituteClassStore.DeleteInstituteClass(
        e.instituteClassID
      );
      if (resVal && resVal.success) {
        this.resultMessage = "تم الحذف بنجاح";
        this.resultType = false;
      } else {
        this.resultMessage = (resVal && resVal.errors) || "فشل الحذف";
        this.resultType = true;
      }
      this.selectedIds = this.selectedIds.filter(
        (id) => id !== e.instituteClassID
      );
    },
    async onShowInfo(e) {
      this.InstituteClassStore.selectedInstituteClass =
        this.InstituteClassStore.instituteClasses.find(
          (i) => i.instituteClassID === e.instituteClassID
        );
      this.showInfo = true;
    },
    async onShowAddGroup(e) {
      this.InstituteClassStore.selectedInstituteClass = e;
      this.GroupStore.selectedGroup = null;
      this.showUpsertGroup = true;
    },
    async OnSubmitUpsertGroup(e) {
      const resVal = await this.GroupStore.addGroup(e);
      if (resVal.success) {
        this.resultMessage = "تم إنشاء المجموعة بنجاح";
        this.resultType = false;
        this.showUpsertGroup = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async onSearchInstituteClass(type, val) {
      await this.InstituteClassStore.fetchInstituteClassBy({
        Param: val,
        ParamType: type,
      });
    },
    onpopaClose() {
      this.showInfo = false;
      this.showUpsertGroup = false;
      this.InstituteClassStore.selectedInstituteClass = null;
      this.GroupStore.selectedGroup = null;
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
        ? this.pagedItems.map((ic) => ic.instituteClassID)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.instituteClassID);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.instituteClassID);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          await this.onShowInfo(singleItem);
          break;
        case "delete":
          for (const ic of selectedItems) {
            await this.OnDelete(ic);
          }
          this.selectedIds = [];
          break;
        case "addGroup":
          await this.onShowAddGroup(singleItem);
          break;
        case "addByRange":
          this.selectedClassesForRange = selectedItems.map((ic) => ({
            classID: ic.classID,
            name: ic.className,
            programName: ic.programName,
          }));
          this.showUpsertByRange = true;
          break;
      }
    },
  },
  mounted() {
    this.InstituteClassStore.fetchInstituteClasses();
  },
};
</script>

<style scoped></style>
