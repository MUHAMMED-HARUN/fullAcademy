import AcademicPlanView from "@/views/AcademicPlan/AcademicPlanView.vue";

export const AcademicPlanLinks = [
  {
    path: "/academicPlan",
    name: "AcademicPlan",
    component: AcademicPlanView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_AcademicPlan",
    },
  },
];
