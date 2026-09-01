<template>
  <div class="trans" @click="$emit('close')"></div>
  <div class="box">
    <div class="box-content">
      <slot></slot>
    </div>
    <div class="action">
      <button class="btn btn-secondary" @click="$emit('close')">إغلاق</button>
    </div>
  </div>
</template>
<script>
export default {
  name: "popapComp",
  props: ["handelSave", "handleClose"],
  emits: ["close"],
};
</script>
<style scoped>
.trans {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(15, 23, 42, 0.4); /* Slate dark overlay */
  backdrop-filter: blur(6px); /* Backdrop blur for premium aesthetic */
  z-index: 999;
  transition: all var(--transition-normal);
}
.box {
  width: 90%;
  max-width: 600px;
  height: auto;
  max-height: 90vh;
  background-color: var(--bg-card);
  border-radius: var(--radius-lg);
  box-shadow: var(--shadow-xl);
  border: 1px solid var(--border-color);
  padding: var(--spacing-lg);
  position: fixed;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 1000;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  overflow-y: auto;
}
.box-content {
  flex-grow: 1;
}
.action {
  display: flex;
  justify-content: flex-end;
  margin-top: var(--spacing-md);
  border-top: 1px solid var(--border-color);
  padding-top: var(--spacing-sm);
}
</style>
