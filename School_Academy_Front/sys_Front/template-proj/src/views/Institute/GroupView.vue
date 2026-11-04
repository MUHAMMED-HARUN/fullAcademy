<template>
  <div class="space-y-4">
    <GenericComboActions
      :actions="actions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <GroupList
      :groups="GroupStore.groups"
      :selected-ids="selectedIds"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-delete-group="OnDelete"
      @show-info-group="onShowInfo"
      @show-update-group="OnUpsertGroup"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertGroupComp
        :errors="GroupStore.error"
        :institute-class-data="InstituteClassStore.selectedInstituteClass"
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
        :student-data="StudentStore.selectedStudent"
        :group-data="GroupStore.selectedGroup"
        @on-submit="OnSubmitEnrollment"
        @on-search-student="onSearchStudent"
      />
    </PopapComp>

    <button
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
import { useGroupStore } from "@/store/Institute/GroupStore";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";
import { useStudentEnrollmentStore } from "@/store/Enrollment/StudentEnrollmentStore";
import { useStudentStore } from "@/store/Person/useStudentStore";
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
    UpsertStudentEnrollmentComp,
  },
  data() {
    return {
      GroupStore: useGroupStore(),
      InstituteClassStore: useInstituteClassStore(),
      StudentEnrollmentStore: useStudentEnrollmentStore(),
      StudentStore: useStudentStore(),
      showUpsert: false,
      showInfo: false,
      showUpsertEnrollment: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      studentName: "",
      groupName: "",
      EnrollmentID: "",
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
      ],
    };
  },
  computed: {
    selectedItems() {
      return this.GroupStore.groups.filter((g) =>
        this.selectedIds.includes(g.groupID)
      );
    },
  },
  methods: {
    async OnUpsertGroup(e) {
      if (e) {
        this.GroupStore.selectedGroup = e;
        await this.onSearchInstituteClass(
          0,
          this.GroupStore.selectedGroup.instituteClassID
        );
      } else {
        this.GroupStore.selectedGroup = null;
        this.InstituteClassStore.selectedInstituteClass = null;
      }
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
    async onSearchInstituteClass(type, val) {
      await this.InstituteClassStore.fetchInstituteClassBy({
        Param: val,
        ParamType: type,
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
    async onSearchStudent(type, val) {
      await this.StudentStore.fetchStudentBy({
        Param: val,
        ParamType: type,
      });
    },
    onpopaCloseEnrollment() {
      this.showUpsertEnrollment = false;
      this.StudentEnrollmentStore.selectedEnrollment = null;
      this.StudentStore.selectedStudent = null;
      this.GroupStore.selectedGroup = null;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.GroupStore.selectedGroup = null;
      this.InstituteClassStore.selectedInstituteClass = null;
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
          this.StudentStore.selectedStudent = null;
          this.showUpsertEnrollment = true;
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
