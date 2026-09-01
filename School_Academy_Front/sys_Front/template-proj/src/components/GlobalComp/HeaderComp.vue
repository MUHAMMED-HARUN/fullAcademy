<template>
  <nav>
    <div class="logo">Logo</div>

    <div class="links">
      <LinksComp />
    </div>
    <div class="search">Search</div>

    <div class="user">
      <div v-show="isAuthenticated" class="user-name">{{ getUserName }}</div>
      <span></span>
      <button v-if="isAuthenticated" class="Log-out" @click="log_out">
        log out
      </button>
      <button v-else class="Log-in" @click="log_in">log in</button>
    </div>
  </nav>
</template>
<script>
import LinksComp from "./linksComp.vue";
import { useAuthStore } from "@/store/Auth/authStore.js";
export default {
  name: "HeaderComp",
  components: { LinksComp },
  data() {
    return { AuthStore: useAuthStore() };
  },
  computed: {
    isAuthenticated() {
      return this.AuthStore.isAuthenticated;
    },
    getUserName() {
      return this.AuthStore.currentUser?.username;
    },
  },
  methods: {
    log_out() {
      this.AuthStore.logout();
      this.$router.push({ path: "/" });
    },
    log_in() {
      this.AuthStore.logout();
      this.$router.push({ path: "/login" });
    },
  },
};
</script>
<style scoped lang="scss">
nav {
  display: grid;

  grid-template-columns: minmax(50px, auto) 1fr minmax(50px, auto) minmax(
      70px,
      auto
    );

  grid-template-areas: "logo links search user";

  align-items: center;
  gap: 20px;
}

.logo {
  grid-area: logo;
}

.links {
  grid-area: links;
  gap: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.search {
  grid-area: search;
}

.user {
  grid-area: user;
  display: flex;
  gap: 5px;
  align-items: center;
  .Log-out {
    font-size: 13px;
    padding: 2px;
  }
  .Log-out:hover {
    font-size: 17px;
  }
}
</style>
