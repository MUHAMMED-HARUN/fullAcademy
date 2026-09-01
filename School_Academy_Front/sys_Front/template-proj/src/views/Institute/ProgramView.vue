<template>
  <div>
    <ProgramList
      @show-delete-program="OnDeleteProgram"
      @show-info-program="onShowInfoProgram"
      @show-update-program="OnUpsertProgram"
      :programs="ProgramStore.programs"
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
    <button @click="showUpsert = true">اضافة مدينة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
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
  },
  data() {
    return {
      ProgramStore: useProgramStore(),

      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
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
