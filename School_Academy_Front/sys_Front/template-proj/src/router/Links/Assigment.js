import AssignmentNominationView from "@/views/Assigment/AssignmentNominationView.vue";
import AssignmentScopeInfoView from "@/views/Assigment/AssignmentScopeInfoView.vue";
import AssignmentScopeView from "@/views/Assigment/AssignmentScopeView.vue";
import AssignmentView from "@/views/Assigment/AssignmentView.vue";
import ResultView from "@/views/Assigment/ResultView.vue";

export const AssigmentsLinks = [
  {
    path: "/assignmentScope",
    name: "assignmentScope",
    component: AssignmentScopeView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/assignment",
    name: "assignment",
    component: AssignmentView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/assignmentScopeInfo",
    name: "assignmentScopeInfo",
    component: AssignmentScopeInfoView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/assignmentNomination",
    name: "assignmentNomination",
    component: AssignmentNominationView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/result",
    name: "result",
    component: ResultView,
    meta: {
      requiresAuth: true,
    },
  },
];
