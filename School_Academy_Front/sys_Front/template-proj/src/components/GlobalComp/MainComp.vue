<template>
  <div class="flex min-h-screen flex-col bg-gray-50 dark:bg-gray-900">
    <HeaderComp v-if="!isAuthRoute" />

    <main class="flex-1">
      <div
        :class="
          isAuthRoute
            ? 'flex min-h-screen items-center justify-center p-4'
            : 'mx-auto w-full max-w-(--breakpoint-2xl) p-4 md:p-6'
        "
      >
        <router-view />
      </div>
    </main>

    <footer
      v-if="!isAuthRoute"
      class="border-t border-gray-200 bg-white py-4 text-center text-sm text-gray-500 dark:border-gray-800 dark:bg-gray-900 dark:text-gray-400"
    >
      © {{ year }} School Academy
    </footer>

    <!-- Toast Notifications Container -->
    <ToastViewport />
  </div>
</template>

<script setup>
import { computed } from "vue";
import { useRoute } from "vue-router";
import HeaderComp from "./HeaderComp.vue";
import ToastViewport from "./ToastViewport.vue";

const route = useRoute();
const year = computed(() => new Date().getFullYear());
const isAuthRoute = computed(() =>
  ["/login", "/register"].includes(route.path)
);
</script>
