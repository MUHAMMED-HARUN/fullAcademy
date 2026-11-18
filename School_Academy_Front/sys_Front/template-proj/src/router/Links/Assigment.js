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
      requiredPermission: "Can_Read_AssignmentScope",
    },
  },
  {
    path: "/assignment",
    name: "assignment",
    component: AssignmentView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_Assignment",
    },
  },
  {
    path: "/assignmentScopeInfo",
    name: "assignmentScopeInfo",
    component: AssignmentScopeInfoView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_AssignmentScopeInfo",
    },
  },
  {
    path: "/assignmentNomination",
    name: "assignmentNomination",
    component: AssignmentNominationView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_AssignmentNomination",
    },
  },
  {
    path: "/result",
    name: "result",
    component: ResultView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_Result",
    },
  },
];
