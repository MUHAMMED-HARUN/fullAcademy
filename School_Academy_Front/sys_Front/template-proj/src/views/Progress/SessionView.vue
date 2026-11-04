<template>
  <div class="space-y-4" dir="rtl">
    <GenericFilterComp
      :fields="sessionFilterSchema"
      @search="onFilterSearch"
      @reset="onFilterReset"
    />
    <p
      v-if="SessionStore.loading"
      class="text-sm text-[var(--text-muted)]"
      role="status"
    >
      جارٍ تحميل السجلات…
    </p>
    <p v-else-if="SessionStore.error" class="alert-error" role="alert">
      {{ SessionStore.error }}
    </p>
    <p
      v-else-if="SessionStore.sessions.length === 0"
      class="rounded-lg border border-dashed border-[var(--border-color)] p-4 text-center text-[var(--text-muted)]"
      role="status"
    >
      لا توجد سجلات مطابقة.
    </p>
    <SessionList
      :sessions="SessionStore.sessions"
      :can-read="canReadSession"
      :can-create="canCreateSession"
      :can-update="canUpdateSession"
      :can-delete="canDeleteSession"
      @show-delete="OnDeleteSession"
      @show-info="onShowInfoSession"
      @show-update="OnUpsertSession"
      @add-child="OnAddChildSession"
    />
    <PaginationComp
      :current-page="SessionStore.page"
      :page-size="SessionStore.pageSize"
      :total-count="SessionStore.totalCount"
      :total-pages="SessionStore.totalPages"
      @change-page="onPageChange"
      @change-size="onPageSizeChange"
    />
    <PopapComp v-if="showUpsert" @close="onpopaClose">
      <UpsertSessionComp
        :errors="SessionStore.error"
        :parent-session="parentSessionForChild"
        :parent-results="SessionStore.lookupResults"
        :parent-loading="SessionStore.lookupLoading"
        :parent-error="SessionStore.lookupError"
        @search-parent="onSearchParents"
        @clear-parent="SessionStore.clearParentLookup"
        @on-submit="OnSubmitUpsert"
      />
    </PopapComp>
    <PopapComp v-if="showInfo" @close="onpopaClose">
      <SessionCard :-data-req="SessionStore.selectedSession" />
    </PopapComp>
    <button
      v-if="canCreateSession"
      type="button"
      class="btn btn-primary"
      @click="OnUpsertSession(null)"
    >
      إضافة سجل
    </button>
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
import GenericFilterComp from "@/components/general/GenericFilterComp.vue";
import PaginationComp from "@/components/general/PaginationComp.vue";
import { useAuthStore } from "@/store/Auth/authStore";

export default {
  name: "SessionView",
  components: {
    SessionList,
    SessionCard,
    UpsertSessionComp,
    PopapComp,
    MessageComp,
    GenericFilterComp,
    PaginationComp,
  },
  data() {
    return {
      SessionStore: useSessionStore(),
      AuthStore: useAuthStore(),

      showUpsert: false,
      showInfo: false,
      resultMessage: null,
      resultType: false,
      parentSessionForChild: null,
      sessionFilterSchema: [
        {
          key: "SessionName",
          label: "اسم السجل",
          type: "text",
          operator: "=*",
        },
        {
          key: "SessionType",
          label: "النوع",
          type: "select",
          options: [
            { label: "موضوع", value: 0 },
            { label: "مادة", value: 1 },
          ],
        },
      ],
    };
  },
  computed: {
    canReadSession() {
      return this.AuthStore.hasPermission("Can_Read_Session");
    },
    canCreateSession() {
      return this.AuthStore.hasPermission("Can_Create_Session");
    },
    canUpdateSession() {
      return this.AuthStore.hasPermission("Can_Update_Session");
    },
    canDeleteSession() {
      return this.AuthStore.hasPermission("Can_Delete_Session");
    },
  },
  methods: {
    async OnUpsertSession(e) {
      this.parentSessionForChild = null;
      this.SessionStore.clearParentLookup();
      if (e) {
        this.SessionStore.selectedSession = e;
      } else {
        this.SessionStore.selectedSession = null;
      }
      this.showUpsert = true;
    },

    OnAddChildSession(parentSession) {
      this.SessionStore.selectedSession = null;
      this.parentSessionForChild = parentSession;
      this.SessionStore.clearParentLookup();
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
        this.parentSessionForChild = null;
        this.SessionStore.selectedSession = null;
      } else {
        this.resultMessage = resVal.errors;
        this.resultType = true;
      }
    },
    async OnDeleteSession(e) {
      if (!window.confirm("هل أنت متأكد من حذف هذا السجل؟")) return;
      const result = await this.SessionStore.DeleteSession(e.sessionID);
      this.resultMessage = result.success ? "تم الحذف" : result.errors;
      this.resultType = !result.success;
    },
    async onShowInfoSession(e) {
      this.SessionStore.selectedSession = e;
      this.showInfo = true;
    },
    onpopaClose() {
      this.showUpsert = false;
      this.showInfo = false;
      this.SessionStore.selectedSession = null;
      this.parentSessionForChild = null;
      this.SessionStore.clearParentLookup();
    },
    async onFilterSearch(payload) {
      await this.SessionStore.fetchSessions({
        Page: 1,
        Filter: payload.filter || "",
        OrderBy: payload.orderBy || "",
      });
    },
    async onFilterReset() {
      await this.SessionStore.fetchSessions({
        Page: 1,
        Filter: "",
        OrderBy: "",
      });
    },
    async onPageChange(page) {
      await this.SessionStore.setPage(page);
    },
    async onPageSizeChange(size) {
      await this.SessionStore.setPageSize(size);
    },
    async onSearchParents(searchTerm) {
      await this.SessionStore.searchParents(searchTerm);
    },
  },
  mounted() {
    this.SessionStore.fetchSessions();
  },
};
</script>

<style scoped></style>
