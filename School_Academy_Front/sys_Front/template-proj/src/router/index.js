import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";
import { useAuthStore } from "@/store/Auth/authStore.js";
import { AuthLinls } from "./Links/AuthLinks.js";
import { AddressLinks } from "./Links/Address.js";
import { PersonLinks } from "./Links/PersonLinks.js";
import { InstituteLinks } from "./Links/InstituteLinks.js";
import { EnrollmentLinks } from "./Links/EnrollmentLinks.js";
import { ProgressLinks } from "./Links/Progress.js";
import { AssigmentsLinks } from "./Links/Assigment.js";
const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  ...AuthLinls,
  ...AddressLinks,
  ...PersonLinks,
  ...InstituteLinks,
  ...EnrollmentLinks,
  ...ProgressLinks,
  ...AssigmentsLinks,
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});
router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next({
      path: "/login",
      query: {
        redirect: to.fullPath,
      },
    });
    return;
  }
  next();
});
export default router;
