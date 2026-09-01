<template>
  <div>
    <ClassList
      :classes="ClassStore.classes"
      @show-delete-class="OnDelete"
      @show-info-class="onShowInfo"
      @show-update-class="OnUpsertClass"
      @show-add-to-institue-class="onShowAddToInstitueClass"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpserClassComp
        :errors="ClassStore.error"
        :program-data="ProgramStore.selectedProgram"
        @on-submit="OnSubmitUpsert"
        @on-search-program="onSearchProgram"
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
        @onSearchInstitute="onSearchInstitute"
        @onSubmit="OnSubmitUpsertInstitueClass"
      />
    </PopapComp>
    <button @click="showUpsert = true">اضافة مدينة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useClassStore } from "@/store/Institute/ClassStore";
import { useInstituteStore } from "@/store/Institute/useInstituteStore";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";
import { useProgramStore } from "@/store/Institute/useProgramStore";
import MessageComp from "@/components/general/messageComp.vue";
import ClassCard from "@/components/Institute/Class/ClassCard.vue";
import ClassList from "@/components/Institute/Class/ClassList.vue";
import UpserClassComp from "@/components/Institute/Class/UpserClassComp.vue";
import UpserInstituteClassComp from "@/components/Institute/InstituteClass/UpserInstituteClassComp.vue";
import TResult from "@/Generic/TResult";

export default {
  name: "ClassView",
  components: {
    ClassList,
    UpserClassComp,
    ClassCard,
    PopapComp,
    MessageComp,
    UpserInstituteClassComp,
  },
  data() {
    return {
      ClassStore: useClassStore(),
      ProgramStore: useProgramStore(),
      InstituteStore: useInstituteStore(),
      InstituteClassStore: useInstituteClassStore(),
      showUpsert: false,
      showAddToInstitue: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    async OnUpsertClass(e) {
      if (e) {
        this.ClassStore.selectedClass = e;
        await this.onSearchProgram(0, this.ClassStore.selectedClass.programID);
      } else this.ProgramStore.selectedProgram = null;

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
        console.log("res val error", resVal.errors);
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
    async onSearchInstitute(type, val) {
      await this.InstituteStore.fetchInstituteBy({
        Param: val,
        ParamType: type,
      });
    },
    async OnSubmitUpsertInstitueClass(e) {
      var resVal = new TResult();

      resVal = await this.InstituteClassStore.addInstituteClass(e);

      if (resVal.success) {
        this.resultMessage = "تم الحفظ";
        this.resultType = false;
        this.showUpsert = false;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.ClassStore.selectedClass = null;
      this.showAddToInstitue = false;
      this.ClassStore.selectedClass = null;
      this.InstituteStore.selectedInstitute = null;
    },
  },
  mounted() {
    this.ClassStore.fetchClasses();
  },
};
</script>

<style scoped></style>
