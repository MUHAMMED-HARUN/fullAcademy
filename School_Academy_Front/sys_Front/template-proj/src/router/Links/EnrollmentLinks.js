import StudentEnrollmentView from "@/views/Enrollment/StudentEnrollmentView.vue";
import TeachingAssignmentView from "@/views/Enrollment/TeachingAssignmentView.vue";

export const EnrollmentLinks = [
  {
    path: "/studentEnrollment",
    name: "studentEnrollment",
    component: StudentEnrollmentView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/teachingAssignment",
    name: "TeachingAssignment",
    component: TeachingAssignmentView,
    meta: {
      requiresAuth: true,
    },
  },
];
