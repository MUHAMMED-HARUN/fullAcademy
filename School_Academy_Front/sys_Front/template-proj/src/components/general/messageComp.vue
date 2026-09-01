<template>
  <transition name="fade">
    <div
      v-if="visible"
      :class="['toast', isError ? 'toast-error' : 'toast-success']"
    >
      <ul>
        <li v-for="(err, index) in GetErrors" :key="index">{{ err }}</li>
      </ul>
    </div>
  </transition>
</template>

<script>
export default {
  name: "messageComp",
  props: {
    msg: {
      type: [String, Array], // ممكن string او array
      default: null,
    },
    isError: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      visible: false,
      timer: 5,
    };
  },
  computed: {
    GetErrors() {
      if (!this.msg) return [];
      if (typeof this.msg === "string") return [this.msg];
      if (Array.isArray(this.msg)) return this.msg;
      return [];
    },
  },
  watch: {
    // اول ما msg يتغير نعرض ونعمل تايمر 4 ثواني
    msg: {
      immediate: true,
      handler(newVal) {
        if (newVal) {
          this.show();
        }
      },
    },
  },
  methods: {
    show() {
      this.visible = true;
      clearTimeout(this.timer);
      this.timer = setTimeout(() => {
        this.visible = false;
      }, 4000); // 4 ثواني
    },
  },
  beforeUnmount() {
    clearTimeout(this.timer);
  },
};
</script>

<style scoped>
.toast {
  position: fixed;
  bottom: 20px;
  right: 20px; /* في الزاوية تحت يمين */
  min-width: 300px;
  padding: var(--spacing-md);
  border-radius: var(--radius-md);
  color: var(--bg-card);
  z-index: 9999;
  box-shadow: var(--shadow-lg);
  font-family: var(--font-base);
}

.toast-error {
  background-color: var(--danger-color);
}

.toast-success {
  background-color: var(--success-color);
}

.toast ul {
  margin: 0;
  padding-left: 20px;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity var(--transition-normal),
    transform var(--transition-normal);
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(20px);
}
</style>
