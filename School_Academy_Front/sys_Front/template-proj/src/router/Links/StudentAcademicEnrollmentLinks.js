import StudentAcademicEnrollmentView from "@/views/Enrollment/StudentAcademicEnrollmentView.vue";

export const StudentAcademicEnrollmentLinks = [
  {
    path: "/studentAcademicEnrollment",
    name: "StudentAcademicEnrollment",
    component: StudentAcademicEnrollmentView,
    meta: {
      requiresAuth: true,
      requiredPermission: "Can_Read_StudentAcademicEnrollment",
    },
  },
];
