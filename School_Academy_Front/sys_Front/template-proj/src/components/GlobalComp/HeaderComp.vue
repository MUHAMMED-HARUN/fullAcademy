<template>
  <header
    class="sticky top-0 z-50 w-full border-b border-gray-200 bg-white dark:border-gray-800 dark:bg-gray-900"
  >
    <div
      class="mx-auto flex h-16 max-w-(--breakpoint-2xl) items-center justify-between gap-4 px-4 md:px-6"
    >
      <!-- Logo -->
      <router-link
        to="/"
        class="flex items-center gap-2 text-lg font-semibold text-gray-800 dark:text-white/90"
      >
        <span
          class="flex h-9 w-9 items-center justify-center rounded-xl bg-brand-500 text-white shadow-theme-xs"
          >S</span
        >
        <span class="hidden sm:inline">School Academy</span>
      </router-link>

      <!-- Links -->
      <nav class="hidden flex-1 items-center justify-center gap-1 md:flex">
        <LinksComp />
      </nav>

      <!-- Search (placeholder) -->
      <div class="hidden lg:block">
        <div
          class="flex h-10 items-center gap-2 rounded-[10px] border border-gray-300 bg-gray-50 px-3 text-sm text-gray-500 dark:border-gray-700 dark:bg-gray-800 dark:text-gray-400"
        >
          <span>بحث...</span>
        </div>
      </div>

      <!-- User -->
      <div class="flex items-center gap-2">
        <template v-if="isAuthenticated">
          <span
            class="hidden text-sm font-medium text-gray-700 dark:text-gray-300 sm:inline"
          >
            {{ userName }}
          </span>
          <button
            type="button"
            class="h-9 rounded-[10px] border border-red-300 px-3 text-sm font-medium text-red-500 shadow-theme-xs transition-colors hover:bg-red-50 hover:text-red-600 dark:border-red-500/40 dark:text-red-400 dark:hover:bg-red-500/10"
            @click="logOut"
          >
            خروج
          </button>
        </template>
        <template v-else>
          <button
            type="button"
            class="h-9 rounded-[10px] bg-brand-500 px-4 text-sm font-medium text-white shadow-theme-xs transition-colors hover:bg-brand-600"
            @click="logIn"
          >
            دخول
          </button>
        </template>
      </div>
    </div>

    <!-- Mobile links -->
    <div
      class="border-t border-gray-200 bg-white px-4 py-2 dark:border-gray-800 dark:bg-gray-900 md:hidden"
    >
      <div class="flex flex-wrap gap-2">
        <LinksComp />
      </div>
    </div>
  </header>
</template>

<script setup>
import { computed } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "@/store/Auth/authStore";
import LinksComp from "./linksComp.vue";

const router = useRouter();
const authStore = useAuthStore();

const isAuthenticated = computed(() => authStore.isAuthenticated);
const userName = computed(
  () => authStore.userData?.name || authStore.user?.name || ""
);

function logOut() {
  authStore.logout();
  router.push("/");
}

function logIn() {
  router.push("/login");
}
</script>
