import ClassView from "@/views/Institute/ClassView.vue";
import InstituteClassView from "@/views/Institute/InstituteClassView.vue";
import InstituteView from "@/views/Institute/InstituteView.vue";
import ProgramView from "@/views/Institute/ProgramView.vue";
import GroupView from "@/views/Institute/GroupView.vue";

export const InstituteLinks = [
  {
    path: "/institute",
    name: "Institute",
    component: InstituteView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/program",
    name: "program",
    component: ProgramView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/class",
    name: "class",
    component: ClassView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/instituteClass",
    name: "instituteClass",
    component: InstituteClassView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/group",
    name: "group",
    component: GroupView,
    meta: {
      requiresAuth: true,
    },
  },
];
