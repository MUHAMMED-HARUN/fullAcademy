<template>
  <!--
    ToastViewport.vue
    حاوية Toast الثابتة — ضعها مرة واحدة في AdminLayout أو App.vue

    الموضع: top-20 end-4  (أعلى اليمين — لا يغطي الهيدر)
    RTL: خاصية end-4 منطقية تعمل مع dir="rtl" تلقائياً.

    حسب قواعد: 05-notifications-toast.md
  -->
  <div
    class="pointer-events-none fixed end-4 top-20 z-[99999] flex w-full max-w-sm flex-col gap-3"
    aria-live="polite"
    aria-label="Toast notifications"
  >
    <transition-group name="toast" tag="div" class="flex flex-col gap-3">
      <div v-for="t in toasts" :key="t.id" class="pointer-events-auto w-full">
        <!-- Alert Box -->
        <div
          :class="[
            'relative grid w-full gap-0.5 rounded-xl border px-3 py-3 text-start text-sm shadow-theme-lg',
            'has-[>svg]:grid-cols-[auto_1fr] has-[>svg]:gap-x-2',
            variantClass(t.variant),
          ]"
          role="alert"
        >
          <!-- أيقونة الحالة -->
          <svg
            v-if="t.variant === 'success'"
            xmlns="http://www.w3.org/2000/svg"
            class="row-span-2 mt-0.5 size-5 translate-y-0.5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M9 12.75 11.25 15 15 9.75M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0z"
            />
          </svg>

          <svg
            v-else-if="t.variant === 'destructive'"
            xmlns="http://www.w3.org/2000/svg"
            class="row-span-2 mt-0.5 size-5 translate-y-0.5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M12 9v3.75m9-.75a9 9 0 1 1-18 0 9 9 0 0 1 18 0zm-9 3.75h.008v.008H12v-.008z"
            />
          </svg>

          <svg
            v-else-if="t.variant === 'warning'"
            xmlns="http://www.w3.org/2000/svg"
            class="row-span-2 mt-0.5 size-5 translate-y-0.5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126zM12 15.75h.007v.008H12v-.008z"
            />
          </svg>

          <svg
            v-else
            xmlns="http://www.w3.org/2000/svg"
            class="row-span-2 mt-0.5 size-5 translate-y-0.5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="m11.25 11.25.041-.02a.75.75 0 0 1 1.063.852l-.708 2.836a.75.75 0 0 0 1.063.853l.041-.021M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0zm-9-3.75h.008v.008H12V8.25z"
            />
          </svg>

          <!-- النص -->
          <div class="flex flex-col gap-0.5">
            <p class="text-sm font-medium text-gray-800 dark:text-white/90">
              {{ t.title }}
            </p>
            <p
              v-if="t.description"
              class="text-xs text-gray-500 dark:text-gray-400"
            >
              {{ t.description }}
            </p>
          </div>

          <!-- زر الإغلاق -->
          <button
            type="button"
            class="absolute end-2 top-2 rounded-lg p-0.5 text-gray-400 hover:text-gray-600 dark:text-gray-500 dark:hover:text-gray-300"
            :aria-label="'Dismiss notification'"
            @click="toast.dismiss(t.id)"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="size-4"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M6 18 18 6M6 6l12 12"
              />
            </svg>
          </button>
        </div>
      </div>
    </transition-group>
  </div>
</template>

<script>
import { defineComponent } from "vue";
import { useToast } from "@/Generic/useToast";

export default defineComponent({
  name: "ToastViewport",

  setup() {
    const toast = useToast();
    const { toasts } = toast;

    /**
     * Tailwind classes لكل variant حسب 05-notifications-toast.md
     * @param {'success'|'destructive'|'warning'|'info'|'default'} variant
     */
    function variantClass(variant) {
      switch (variant) {
        case "success":
          return "bg-green-50 border-green-500 text-green-700 dark:bg-green-500/10 dark:border-green-500/30 dark:text-green-400";
        case "destructive":
          return "bg-red-50 border-red-500 text-red-700 dark:bg-red-500/10 dark:border-red-500/30 dark:text-red-400";
        case "warning":
          return "bg-amber-50 border-amber-500 text-amber-700 dark:bg-amber-500/10 dark:border-amber-500/30 dark:text-amber-400";
        case "info":
          return "bg-sky-50 border-sky-500 text-sky-700 dark:bg-sky-500/10 dark:border-sky-500/30 dark:text-sky-400";
        default:
          return "bg-white border-gray-200 text-gray-700 dark:border-gray-800 dark:bg-[#1E2634] dark:text-gray-300";
      }
    }

    return { toasts, toast, variantClass };
  },
});
</script>

<style scoped>
/* ─── Toast Animations ──────────────────────────────────────────────── */
.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}
.toast-enter-from {
  opacity: 0;
  transform: translateX(100%);
}
.toast-leave-to {
  opacity: 0;
  transform: translateX(100%);
}
.toast-move {
  transition: transform 0.3s ease;
}
</style>
