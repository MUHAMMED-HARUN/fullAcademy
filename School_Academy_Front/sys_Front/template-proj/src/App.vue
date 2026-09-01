<template>
  <main-comp />
</template>
<script>
import MainComp from "@/components/GlobalComp/MainComp.vue";
export default {
  name: "App",
  components: {
    MainComp,
  },
};
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
}
</style>
