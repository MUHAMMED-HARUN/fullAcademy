<template>
  <div class="layout">
    <header class="header">
      <HeaderComp />
    </header>

    <main class="content">
      <router-view />
    </main>

    <footer class="footer">Footer</footer>
  </div>
</template>

<script>
import HeaderComp from "./HeaderComp.vue";

export default {
  name: "MainComp",
  components: {
    HeaderComp,
  },
};
</script>

<style scoped lang="scss">
.layout {
  display: grid;
  grid-template-rows: auto 1fr auto;
  min-height: 100vh;
  max-width: 1400px;
  width: 100%;
  margin: 0 auto;
}

.header {
  padding: 10px;
  grid-row: 1;
  background: #2334;
}

.content {
  grid-row: 2;
  padding: 20px;
}

.footer {
  padding: 10px;
  grid-row: 3;
}
</style>
