<template>
  <div>
    <AssignmentNominationList
      @show-delete="OnDelete"
      @show-new-assignment="OnStartAssigment"
      :-assignment-nominations="AssignmentNomination.AssignmentNominations"
    />
    <PopapComp v-if="showAddAssigment" @close="showAddAssigment = false">
      <UpsertAssigmentComp
        :-nomination-info="AssignmentNomination.selectedAssignmentNomination"
        @on-submit="StartAssigment"
      />
    </PopapComp>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import { useAssignmentScopeStore } from "@/store/Assignment/AssignmentScopeStore";
import { useAssignmentNominationStore } from "@/store/Assignment/AssignmentNominationStore";
import MessageComp from "@/components/general/messageComp.vue";
import AssignmentNominationList from "@/components/Assigment/AssignmentNomination/AssignmentNominationList.vue";
import UpsertAssigmentComp from "@/components/Assigment/Assigment/UpsertAssigmentComp.vue";
import { useassignmentstore } from "@/store/Assignment/AssignmentStore";
import TResult from "@/Generic/TResult";
import PopapComp from "@/components/general/popapComp.vue";

export default {
  name: "AssignmentNominationView",
  components: {
    AssignmentNominationList,
    MessageComp,
    UpsertAssigmentComp,
    PopapComp,
  },
  data() {
    return {
      AssignmentNomination: useAssignmentNominationStore(),
      AssignmentScopeStore: useAssignmentScopeStore(),
      assignmentstore: useassignmentstore(),
      showUpsert: false,
      showAddAssigment: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      stageInfos: [],
    };
  },
  methods: {
    async OnDelete(e) {
      this.AssignmentNomination.DeleteAssignmentNomination(
        e.assignmentNominationID
      );
    },
    async OnStartAssigment(e) {
      alert("dsdf");
      this.AssignmentNomination.selectedAssignmentNomination = e;
      this.showAddAssigment = true;
    },
    async StartAssigment(e) {
      var resVal = new TResult();
      if (e.assignmentID) {
        return;
      } else {
        resVal = await this.assignmentstore.addAssigment(e);
        this.assignmentstore.selectedassignments = null;
      }
      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showAddAssigment = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
  },
  mounted() {
    this.AssignmentNomination.fetchAssignmentNominations();
  },
};
</script>

<style scoped></style>
