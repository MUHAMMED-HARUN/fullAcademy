<template>
  <transition name="fade">
    <div
      v-if="visible"
      class="fixed bottom-5 end-5 z-9999 min-w-[300px] rounded-xl p-4 text-sm text-white shadow-theme-lg"
      :class="isError ? 'bg-red-500' : 'bg-green-500'"
    >
      <ul class="list-disc ps-5">
        <li v-for="(err, index) in GetErrors" :key="index">{{ err }}</li>
      </ul>
    </div>
  </transition>
</template>

<script setup>
import { ref, watch, onBeforeUnmount, computed } from "vue";

const props = defineProps({
  msg: {
    type: [String, Array],
    default: null,
  },
  isError: {
    type: Boolean,
    default: false,
  },
});

const visible = ref(false);
let timer = null;

const GetErrors = computed(() => {
  if (!props.msg) return [];
  if (typeof props.msg === "string") return [props.msg];
  if (Array.isArray(props.msg)) return props.msg;
  return [];
});

watch(
  () => props.msg,
  (newVal) => {
    if (newVal) show();
  },
  { immediate: true }
);

function show() {
  visible.value = true;
  clearTimeout(timer);
  timer = setTimeout(() => {
    visible.value = false;
  }, 4000);
}

onBeforeUnmount(() => clearTimeout(timer));
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease, transform 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(20px);
}
</style>
