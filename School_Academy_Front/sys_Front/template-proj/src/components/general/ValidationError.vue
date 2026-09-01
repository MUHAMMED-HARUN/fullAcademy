<template>
  <span v-if="messages && messages.length" class="error-msg">
    <ul>
      {{
        getMessageType()
      }}
      <div v-if="isArr">
        <li v-for="msg in messages" :key="msg">
          {{ GetMessage }}
        </li>
      </div>
      <li v-if="isArr === false">
        {{ GetMessage }}
      </li>
    </ul>
  </span>
</template>
<script>
import { array } from "zod";

export default {
  props: {
    messages: {
      type: Array,
      default: null,
    },
  },
  data() {
    return {
      isArr: false,
    };
  },
  computed: {
    GetMessage() {
      if (!this.messages) return "";
      if (typeof this.messages === "string") return this.messages;
      if (typeof this.messages === array) return this.messages;
      return "";
    },
  },

  methods: {
    getMessageType() {
      this.isArr = false;

      if (!this.messages);
      if (typeof this.messages === "string") {
        return;
      }
      if (typeof this.messages === array) {
        this.isArr = true;
        return;
      }
      return "";
    },
  },
};
</script>

<style scoped>
.error-msg {
  color: var(--danger-color);
  font-size: 0.85rem;
  font-weight: 600;
  display: block;
  margin-top: var(--spacing-xs);
}
.error-msg ul {
  list-style: none;
  padding: 0;
  margin: 0;
}
</style>
