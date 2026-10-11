<template>
  <div>
    <SessionList
      @show-delete-session="OnDeleteSession"
      @show-info-session="onShowInfoSession"
      @show-update-session="OnUpsertSession"
      :-sessions="SessionStore.sessions"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertSessionComp
        :errors="SessionStore.error"
        :-sessions="GetSessionByType(0)"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <SessionCard :-data-req="SessionStore.selectedSession" />
    </PopapComp>
    <button @click="showUpsert = true">اضافة مدينة</button>
    <MessageComp :is-error="resultType" :msg="resultMessage" />
  </div>
</template>

<script>
import PopapComp from "@/components/general/popapComp.vue";
import { useSessionStore } from "@/store/Progress/SessionStore";
import MessageComp from "@/components/general/messageComp.vue";
import TResult from "@/Generic/TResult";
import SessionList from "@/components/Progress/Session/SessionList.vue";
import UpsertSessionComp from "@/components/Progress/Session/UpsertSessionComp.vue";
import SessionCard from "@/components/Progress/Session/SessionCard.vue";

export default {
  name: "SessionView",
  components: {
    SessionList,
    SessionCard,
    UpsertSessionComp,
    PopapComp,
    MessageComp,
  },
  data() {
    return {
      SessionStore: useSessionStore(),

      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
    };
  },
  methods: {
    async OnUpsertSession(e) {
      if (e) {
        this.SessionStore.selectedSession = e;
      }
      this.showUpsert = true;
    },

    async OnSubmitUpsert(e) {
      var resVal = new TResult();
      if (e.sessionID) {
        resVal = await this.SessionStore.updateSession(e);
      } else {
        resVal = await this.SessionStore.addSession(e);
        this.SessionStore.selectedSession = null;
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
    async OnDeleteSession(e) {
      this.SessionStore.DeleteSession(e.sessionID);
    },
    async onShowInfoSession(e) {
      this.SessionStore.selectedSession = this.SessionStore.sessions.find(
        (i) => i.sessionID === e.sessionID
      );
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.SessionStore.selectedSession = null;
    },
    GetSessionByType(type) {
      return this.SessionStore.sessions.filter((i) => i.type === type);
    },
  },
  mounted() {
    this.SessionStore.fetchSessions();
  },
};
</script>

<style scoped></style>
