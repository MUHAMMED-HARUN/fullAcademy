import RoleList from "@/components/Auth/RoleList.vue";
import LoginView from "@/views/Auth/LoginView.vue";
import RegisterView from "@/views/Auth/RegisterView.vue";

export const AuthLinls = [
  {
    path: "/register",
    name: "Register",
    component: RegisterView,
    meta: {
      requiresAuth: false,
    },
  },
  {
    path: "/login",
    name: "Login",
    component: LoginView,
    meta: {
      requiresAuth: false,
    },
  },
  {
    path: "/roles",
    name: "roles",
    component: RoleList,
    meta: {
      requiresAuth: true,
    },
  },
];
