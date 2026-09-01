<template>
  <div>
    <InstituteClassList
      @show-delete-institute-class="OnDelete"
      @show-info-institute-class="onShowInfo"
      @show-add-group="onShowAddGroup"
      :institute-classes="this.InstituteClassStore.instituteClasses"
    />
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <InstituteClassCard
        :-data-req="this.InstituteClassStore.selectedInstituteClass"
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
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";
import { useInstituteStore } from "@/store/Institute/useInstituteStore";
import { useClassStore } from "@/store/Institute/ClassStore";
import { useGroupStore } from "@/store/Institute/GroupStore";
import MessageComp from "@/components/general/messageComp.vue";
import InstituteClassList from "@/components/Institute/InstituteClass/InstituteClassList.vue";
import InstituteClassCard from "@/components/Institute/InstituteClass/InstituteClassCard.vue";
import UpsertGroupComp from "@/components/Institute/Group/UpsertGroupComp.vue";

export default {
  name: "InstituteClassView",
  components: {
    InstituteClassList,
    InstituteClassCard,
    UpsertGroupComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      InstituteStore: useInstituteStore(),
      InstituteClassStore: useInstituteClassStore(),
      ClassStore: useClassStore(),
      GroupStore: useGroupStore(),
      showUpsert: false,
      showInfo: false,
      showUpsertGroup: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    async OnDelete(e) {
      if (confirm("هل أنت متأكد من الحذف؟")) {
        this.InstituteClassStore.DeleteInstituteClass(e.instituteClassID);
      }
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
      this.showUpsert = false;
      this.showInfo = false;
      this.showUpsertGroup = false;
      this.InstituteClassStore.selectedInstituteClass = null;
      this.GroupStore.selectedGroup = null;
    },
  },
  mounted() {
    this.InstituteClassStore.fetchInstituteClasses();
  },
};
</script>

<style scoped></style>
