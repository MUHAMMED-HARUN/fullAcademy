<template>
  <div class="space-y-4">
    <form class="flex flex-wrap gap-2" @submit.prevent="searchGroups">
      <input
        v-model="groupSearch"
        type="search"
        class="form-control min-w-60 flex-1"
        placeholder="بحث باسم المجموعة أو كودها"
      />
      <button type="submit" class="btn btn-outline-primary">بحث</button>
      <button
        type="button"
        class="btn btn-outline-secondary"
        @click="resetGroupSearch"
      >
        مسح
      </button>
    </form>

    <GenericComboActions
      :actions="visibleActions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <p v-if="GroupStore.loading" class="py-6 text-center" role="status">
      جارٍ تحميل المجموعات…
    </p>
    <p v-else-if="GroupStore.error" class="alert-error" role="alert">
      {{ formatError(GroupStore.error) }}
    </p>
    <p
      v-else-if="!GroupStore.groups.length"
      class="rounded-lg border border-dashed p-6 text-center text-[var(--text-muted)]"
    >
      لا توجد مجموعات مطابقة.
    </p>
    <GroupList
      v-else
      :groups="GroupStore.groups"
      :selected-ids="selectedIds"
      :can-read="AuthStore.hasPermission('Can_Read_Group')"
      :can-update="AuthStore.hasPermission('Can_Update_Group')"
      :can-delete="AuthStore.hasPermission('Can_Delete_Group')"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-delete-group="OnDelete"
      @show-info-group="onShowInfo"
      @show-update-group="OnUpsertGroup"
    />
    <PaginationComp
      :current-page="GroupStore.page"
      :page-size="GroupStore.pageSize"
      :total-count="GroupStore.totalCount"
      :total-pages="GroupStore.totalPages"
      @change-page="GroupStore.setPage"
      @change-size="GroupStore.setPageSize"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertGroupComp
        :errors="GroupStore.error"
        :group-data="GroupStore.selectedGroup"
        :institute-class-data="InstituteClassStore.selectedInstituteClass"
        :institute-class-results="
          InstituteClassStore.instituteClassLookupResults
        "
        :institute-class-loading="
          InstituteClassStore.instituteClassLookupLoading
        "
        :institute-class-error="InstituteClassStore.instituteClassLookupError"
        :is-saving="GroupStore.loading"
        @on-submit="OnSubmitUpsert"
        @on-search-institute-class="onSearchInstituteClass"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <GroupCard :-data-req="GroupStore.selectedGroup" />
    </PopapComp>
    <PopapComp v-if="showUpsertEnrollment" @close="onpopaCloseEnrollment">
      <UpsertStudentEnrollmentComp
        :errors="StudentEnrollmentStore.error"
        :group-data="GroupStore.selectedGroup"
        :enrollment-results="StudentAcademicEnrollmentStore.enrollments"
        :enrollment-loading="StudentAcademicEnrollmentStore.loading"
        :enrollment-error="StudentAcademicEnrollmentStore.error"
        @on-submit="OnSubmitEnrollment"
        @on-search-academic-enrollment="onSearchAcademicEnrollment"
        @on-clear-academic-enrollment="clearAcademicEnrollmentSearch"
      />
    </PopapComp>

    <button
      v-if="AuthStore.hasPermission('Can_Create_Group')"
      type="button"
      class="h-10 rounded-[10px] bg-brand-500 px-4 text-sm font-medium text-white shadow-theme-xs transition-colors hover:bg-brand-600"
      @click="OnUpsertGroup(null)"
    >
      إضافة مجموعة
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import { useGroupStore } from "@/store/Institute/GroupStore";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";
import { useStudentEnrollmentStore } from "@/store/Enrollment/StudentEnrollmentStore";
import { useStudentAcademicEnrollmentStore } from "@/store/Enrollment/StudentAcademicEnrollmentStore";
import { useAuthStore } from "@/store/Auth/authStore";
import MessageComp from "@/components/general/messageComp.vue";
import GroupCard from "@/components/Institute/Group/GroupCard.vue";
import GroupList from "@/components/Institute/Group/GroupList.vue";
import UpsertGroupComp from "@/components/Institute/Group/UpsertGroupComp.vue";
import UpsertStudentEnrollmentComp from "@/components/Enrollment/StudentEnrollment/UpsertStudentEnrollmentComp.vue";
import TResult from "@/Generic/TResult";

export default {
  name: "GroupView",
  components: {
    GroupList,
    UpsertGroupComp,
    GroupCard,
    PopapComp,
    MessageComp,
    GenericComboActions,
    PaginationComp,
    UpsertStudentEnrollmentComp,
  },
  data() {
    return {
      AuthStore: useAuthStore(),
      GroupStore: useGroupStore(),
      InstituteClassStore: useInstituteClassStore(),
      StudentEnrollmentStore: useStudentEnrollmentStore(),
      StudentAcademicEnrollmentStore: useStudentAcademicEnrollmentStore(),
      showUpsert: false,
      showInfo: false,
      showUpsertEnrollment: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      groupSearch: "",
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
          key: "enrollStudent",
          label: "تسجيل طالب",
          minSelection: 1,
          maxSelection: 1,
          class: "bg-brand-500 hover:bg-brand-600",
        },
        {
          key: "attendance",
          label: "حضور المجموعة",
          minSelection: 1,
          maxSelection: 1,
          class: "btn-primary",
        },
      ],
    };
  },
  computed: {
    visibleActions() {
      const requiredPermissions = {
        info: "Can_Read_Group",
        update: "Can_Update_Group",
        delete: "Can_Delete_Group",
        enrollStudent: "Can_Create_StudentEnrollment",
        attendance: "Can_Create_Attendance",
      };
      return this.actions.filter((action) => {
        if (!this.AuthStore.hasPermission(requiredPermissions[action.key]))
          return false;
        if (
          action.key === "attendance" &&
          (!this.AuthStore.hasPermission("Can_Read_StudentEnrollment") ||
            !this.AuthStore.hasPermission("Can_Read_Attendance"))
        )
          return false;
        return true;
      });
    },
    selectedItems() {
      return this.GroupStore.groups.filter((g) =>
        this.selectedIds.includes(g.groupID)
      );
    },
  },
  methods: {
    formatError(value) {
      if (typeof value === "string") return value;
      if (Array.isArray(value)) return value.join("، ");
      if (value && typeof value === "object")
        return Object.values(value).flat().join("، ");
      return "حدث خطأ أثناء تحميل المجموعات";
    },
    safeText(value) {
      return String(value || "")
        .replace(/[,*|=<>^$()\\;]/g, " ")
        .trim();
    },
    async searchGroups() {
      const text = this.safeText(this.groupSearch);
      const filter = text ? `GroupName=*${text}|Code=*${text}` : "";
      this.selectedIds = [];
      await this.GroupStore.fetchGroups({ Page: 1, Filter: filter });
    },
    async resetGroupSearch() {
      this.groupSearch = "";
      this.selectedIds = [];
      await this.GroupStore.fetchGroups({ Page: 1, Filter: "" });
    },
    async OnUpsertGroup(e) {
      if (e) {
        this.GroupStore.selectedGroup = e;
        this.GroupStore.error = null;
        this.InstituteClassStore.clearInstituteClassLookup();
        this.InstituteClassStore.clearSelectedInstituteClass();
        const result = await this.InstituteClassStore.fetchInstituteClassBy({
          Param: e.instituteClassID,
          ParamType: 1,
        });
        if (!result.success) {
          this.resultMessage = result.errors;
          this.resultType = true;
          return;
        }
      } else {
        this.GroupStore.selectedGroup = null;
        this.GroupStore.error = null;
        this.InstituteClassStore.selectedInstituteClass = null;
      }
      this.InstituteClassStore.clearInstituteClassLookup();
      this.showUpsert = true;
    },
    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.groupID) {
        resVal = await this.GroupStore.updateGroup(e);
      } else {
        resVal = await this.GroupStore.addGroup(e);
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
    async onSearchInstituteClass({ filterType, value }) {
      const filter = this.InstituteClassStore.buildLookupFilter(
        filterType,
        value
      );
      await this.InstituteClassStore.fetchInstituteClassLookup({
        Page: 1,
        PageSize: 20,
        Filter: filter,
      });
    },
    async OnDelete(e) {
      const resVal = await this.GroupStore.DeleteGroup(e.groupID);
      if (resVal.success) {
        this.resultMessage = "تم الحذف بنجاح";
        this.resultType = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
      this.selectedIds = this.selectedIds.filter((id) => id !== e.groupID);
    },
    async onShowInfo(e) {
      this.GroupStore.selectedGroup = this.GroupStore.groups.find(
        (g) => g.groupID === e.groupID
      );
      this.showInfo = true;
    },
    async OnSubmitEnrollment(e) {
      var resVal = new TResult();
      resVal = await this.StudentEnrollmentStore.addEnrollment(e);
      if (resVal.success) {
        this.resultMessage = "تم تسجيل الطالب في المجموعة";
        this.resultType = false;
        this.showUpsertEnrollment = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async onSearchAcademicEnrollment({ filterType, value }) {
      const groupID =
        this.GroupStore.selectedGroup?.groupID ||
        this.GroupStore.selectedGroup?.GroupID;
      if (!groupID) return;
      const filter = this.StudentAcademicEnrollmentStore.buildLookupFilter(
        filterType,
        value
      );
      await this.StudentAcademicEnrollmentStore.fetchEnrollments({
        Page: 1,
        PageSize: 20,
        GroupID: groupID,
        Filter: filter,
      });
    },
    clearAcademicEnrollmentSearch() {
      this.StudentAcademicEnrollmentStore.enrollments = [];
      this.StudentAcademicEnrollmentStore.totalCount = 0;
      this.StudentAcademicEnrollmentStore.error = null;
      this.StudentAcademicEnrollmentStore.groupID = "";
      this.StudentAcademicEnrollmentStore.filter = "";
    },
    onpopaCloseEnrollment() {
      this.showUpsertEnrollment = false;
      this.StudentEnrollmentStore.selectedEnrollment = null;
      this.clearAcademicEnrollmentSearch();
      this.GroupStore.selectedGroup = null;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.GroupStore.selectedGroup = null;
      this.InstituteClassStore.selectedInstituteClass = null;
      this.InstituteClassStore.clearInstituteClassLookup();
    },
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.GroupStore.groups.map((g) => g.groupID)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.groupID);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.groupID);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          await this.onShowInfo(singleItem);
          break;
        case "update":
          await this.OnUpsertGroup(singleItem);
          break;
        case "delete":
          await this.OnDelete(singleItem);
          this.selectedIds = [];
          break;
        case "enrollStudent":
          this.GroupStore.selectedGroup = singleItem;
          this.clearAcademicEnrollmentSearch();
          this.showUpsertEnrollment = true;
          break;
        case "attendance":
          this.$router.push({
            name: "studentEnrollment",
            query: { groupID: singleItem.groupID, attendance: "true" },
          });
          break;
      }
    },
  },
  mounted() {
    this.GroupStore.fetchGroups();
  },
};
</script>

<style scoped></style>
