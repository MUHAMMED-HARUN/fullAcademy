<template>
  <ul
    v-if="displayMessages.length"
    class="error-msg"
    role="alert"
    aria-live="polite"
  >
    <li
      v-for="(message, index) in displayMessages"
      :key="`${index}-${message}`"
    >
      {{ message }}
    </li>
  </ul>
</template>

<script setup>
import { computed } from "vue";

const props = defineProps({
  messages: {
    type: [String, Array, Object],
    default: null,
  },
});

const flattenMessages = (value) => {
  if (typeof value === "string") return [value];
  if (Array.isArray(value)) return value.flatMap(flattenMessages);
  if (value && typeof value === "object") {
    return Object.values(value).flatMap(flattenMessages);
  }
  return [];
};

const displayMessages = computed(() => flattenMessages(props.messages));
</script>

<style scoped>
.error-msg {
  display: block;
  margin: var(--spacing-xs) 0 0;
  padding: 0;
  list-style: none;
  color: var(--danger-color);
  font-size: 0.85rem;
  font-weight: 600;
}
</style>
