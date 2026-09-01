<template>
  <div>
    <GroupList
      :groups="GroupStore.groups"
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
    <button @click="OnUpsertGroup(null)">اضافة مجموعة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useGroupStore } from "@/store/Institute/GroupStore";
import { useInstituteClassStore } from "@/store/Institute/InstituteClassStore";
import MessageComp from "@/components/general/messageComp.vue";
import GroupCard from "@/components/Institute/Group/GroupCard.vue";
import GroupList from "@/components/Institute/Group/GroupList.vue";
import UpsertGroupComp from "@/components/Institute/Group/UpsertGroupComp.vue";
import TResult from "@/Generic/TResult";

export default {
  name: "GroupView",
  components: {
    GroupList,
    UpsertGroupComp,
    GroupCard,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      GroupStore: useGroupStore(),
      InstituteClassStore: useInstituteClassStore(),
      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
    };
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
        console.log("res val error", resVal.errors);
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
      if (confirm("هل أنت متأكد من حذف هذه المجموعة؟")) {
        const resVal = await this.GroupStore.DeleteGroup(e.groupID);
        if (resVal.success) {
          this.resultMessage = "تم الحذف بنجاح";
          this.resultType = false;
        } else {
          this.resultMessage = resVal.errors;
          this.resultType = true;
        }
      }
    },
    async onShowInfo(e) {
      this.GroupStore.selectedGroup = this.GroupStore.groups.find(
        (g) => g.groupID === e.groupID
      );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.GroupStore.selectedGroup = null;
      this.InstituteClassStore.selectedInstituteClass = null;
    },
  },
  mounted() {
    this.GroupStore.fetchGroups();
  },
};
</script>

<style scoped></style>
