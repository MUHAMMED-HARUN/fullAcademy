<template>
  <div>
    <AssignmentScopeList
      :can-read="AuthStore.hasPermission('Can_Read_AssignmentScope')"
      :can-update="AuthStore.hasPermission('Can_Update_AssignmentScope')"
      :can-delete="AuthStore.hasPermission('Can_Delete_AssignmentScope')"
      @show-delete="OnDelete"
      @show-info="onShowInfo"
      @show-update="OnUpsert"
      :-assignment-scopes="AssignmentScopeStore.AssignmentScopes"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertAssignmentScopeComp
        :errors="AssignmentScopeStore.error"
        :initial-scope="AssignmentScopeStore.selectedAssignmentScope"
        :assignment-scopes="AssignmentScopeStore.AssignmentScopes"
        :stage-results="stageResults"
        :stage-loading="stageLoading"
        :stage-error="stageError"
        :saving="AssignmentScopeStore.loading"
        @search-stage="onSearchStage"
        @clear-stage="onClearStageLookup"
        @submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <AssignmentScopeCard
        :-data-req="AssignmentScopeStore.selectedAssignmentScope"
      />
    </PopapComp>
    <button
      v-if="AuthStore.hasPermission('Can_Create_AssignmentScope')"
      @click="showUpsert = true"
    >
      اضافة
    </button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useStageStore } from "@/store/Progress/StageStore";
import { useAuthStore } from "@/store/Auth/authStore";
import MessageComp from "@/components/general/messageComp.vue";
import TResult from "@/Generic/TResult";
import AssignmentScopeList from "@/components/Assigment/AssignmentScope/AssignmentScopeList.vue";
import UpsertAssignmentScopeComp from "@/components/Assigment/AssignmentScope/UpsertAssignmentScopeComp.vue";
import AssignmentScopeCard from "@/components/Assigment/AssignmentScope/AssignmentScopeCard.vue";
import { useAssignmentScopeStore } from "@/store/Assignment/AssignmentScopeStore";

export default {
  name: "assignmentScopeView",
  components: {
    AssignmentScopeList,
    AssignmentScopeCard,
    UpsertAssignmentScopeComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      AuthStore: useAuthStore(),
      AssignmentScopeStore: useAssignmentScopeStore(),
      StageStore: useStageStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      stageResults: [],
      stageLoading: false,
      stageError: null,
      stageLookupRequestVersion: 0,
    };
  },
  methods: {
    async OnUpsert(e) {
      if (e) {
        this.AssignmentScopeStore.selectedAssignmentScope = e;
      } else {
        this.AssignmentScopeStore.selectedAssignmentScope = null;
      }
      this.onClearStageLookup();
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.assignmentScopeID) {
        resVal = await this.AssignmentScopeStore.updateAssignmentScope(e);
      } else {
        resVal = await this.AssignmentScopeStore.addAssignmentScope(e);
        this.AssignmentScopeStore.selectedAssignmentScope = null;
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
    async OnDelete(e) {
      this.AssignmentScopeStore.DeleteAssignmentScope(e.assignmentScopeID);
    },
    async onShowInfo(e) {
      this.AssignmentScopeStore.selectedAssignmentScope =
        this.AssignmentScopeStore.AssignmentScopes.find(
          (i) => i.assignmentScopeID === e.assignmentScopeID
        );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.AssignmentScopeStore.selectedAssignmentScope = null;
      this.onClearStageLookup();
    },

    async onSearchStage(searchTerm) {
      const term = String(searchTerm || "")
        .trim()
        .replace(/[,*|^$!=()/]/g, " ")
        .trim();
      const requestVersion = ++this.stageLookupRequestVersion;
      this.stageResults = [];
      this.stageError = null;
      if (!term) {
        this.stageLoading = false;
        return;
      }

      this.stageLoading = true;
      try {
        const result = await this.StageStore.fetchStagePage({
          Page: 1,
          PageSize: 10,
          Filter: `StageName=*${term}`,
          OrderBy: "",
        });
        if (requestVersion !== this.stageLookupRequestVersion) return;
        if (result.success) this.stageResults = result.data?.items || [];
        else this.stageError = result.errors;
      } catch (error) {
        if (requestVersion === this.stageLookupRequestVersion)
          this.stageError = error?.message || "تعذر البحث عن المرحلة";
      } finally {
        if (requestVersion === this.stageLookupRequestVersion)
          this.stageLoading = false;
      }
    },

    onClearStageLookup() {
      this.stageLookupRequestVersion += 1;
      this.stageResults = [];
      this.stageError = null;
      this.stageLoading = false;
    },
  },
  mounted() {
    this.AssignmentScopeStore.fetchAssignmentScopes();
  },
};
</script>

<style scoped></style>
