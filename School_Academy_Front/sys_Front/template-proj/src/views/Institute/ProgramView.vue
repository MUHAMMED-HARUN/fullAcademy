<template>
  <div class="space-y-4">
    <GenericComboActions
      :actions="actions"
      :selected-items="selectedItems"
      @action-click="handleComboAction"
    />

    <ProgramList
      :programs="ProgramStore.programs"
      :selected-ids="selectedIds"
      @toggle-select-all="onToggleSelectAll"
      @toggle-select-row="onToggleSelectRow"
      @show-delete-program="OnDeleteProgram"
      @show-info-program="onShowInfoProgram"
      @show-update-program="OnUpsertProgram"
    />

    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertProgramComp
        :errors="ProgramStore.error"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <ProgramCard :-data-req="ProgramStore.selectedProgram" />
    </PopapComp>

    <button
      type="button"
      class="h-10 rounded-[10px] bg-brand-500 px-4 text-sm font-medium text-white shadow-theme-xs transition-colors hover:bg-brand-600"
      @click="OnUpsertProgram(null)"
    >
      إضافة برنامج
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import GenericComboActions from "@/components/general/GenericComboActions.vue";
import { useProgramStore } from "@/store/Institute/useProgramStore";
import MessageComp from "@/components/general/messageComp.vue";
import UpsertProgramComp from "@/components/Institute/Program/UpsertProgramComp.vue";
import ProgramCard from "@/components/Institute/Program/ProgramCard.vue";
import TResult from "@/Generic/TResult";
import ProgramList from "@/components/Institute/Program/ProgramList.vue";

export default {
  name: "ProgramView",
  components: {
    ProgramList,
    UpsertProgramComp,
    ProgramCard,
    PopapComp,
    MessageComp,
    GenericComboActions,
  },
  data() {
    return {
      ProgramStore: useProgramStore(),

      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      selectedIds: [],
      actions: [
        { key: "info", label: "معلومات", minSelection: 1, maxSelection: 1 },
        { key: "update", label: "تحديث", minSelection: 1, maxSelection: 1 },
        {
          key: "delete",
          label: "حذف",
          minSelection: 1,
          class: "bg-red-500 hover:bg-red-600",
        },
      ],
    };
  },
  computed: {
    selectedItems() {
      return this.ProgramStore.programs.filter((item) =>
        this.selectedIds.includes(item.programID)
      );
    },
  },
  methods: {
    onToggleSelectAll(isChecked) {
      this.selectedIds = isChecked
        ? this.ProgramStore.programs.map((item) => item.programID)
        : [];
    },
    onToggleSelectRow(item) {
      const index = this.selectedIds.indexOf(item.programID);
      if (index > -1) this.selectedIds.splice(index, 1);
      else this.selectedIds.push(item.programID);
    },
    async handleComboAction(actionKey, selectedItems) {
      const singleItem = selectedItems[0];
      switch (actionKey) {
        case "info":
          this.onShowInfoProgram(singleItem);
          break;
        case "update":
          await this.OnUpsertProgram(singleItem);
          break;
        case "delete":
          await this.OnDeleteProgram(singleItem);
          this.selectedIds = [];
          break;
      }
    },
    async OnUpsertProgram(e) {
      if (e) {
        this.ProgramStore.selectedProgram = e;
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.programID) {
        resVal = await this.ProgramStore.updateProgram(e);
      } else {
        resVal = await this.ProgramStore.addProgram(e);
        this.ProgramStore.selectedProgram = null;
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
    async OnDeleteProgram(e) {
      this.ProgramStore.DeleteProgram(e.programID);
    },
    async onShowInfoProgram(e) {
      this.ProgramStore.selectedProgram = this.ProgramStore.programs.find(
        (i) => i.programID === e.programID
      );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.ProgramStore.selectedProgram = null;
    },
  },
  mounted() {
    this.ProgramStore.fetchPrograms();
  },
};
</script>

<style scoped></style>
