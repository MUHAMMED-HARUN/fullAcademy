import PersonView from "@/views/Person/PersonView.vue";
import StudentView from "@/views/Person/StudentView.vue";
import TeacherView from "@/views/Person/TeacherView.vue";

export const PersonLinks = [
  {
    path: "/persons",
    name: "Persons",
    component: PersonView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/students",
    name: "student",
    component: StudentView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/teachers",
    name: "teacher",
    component: TeacherView,
    meta: {
      requiresAuth: true,
    },
  },
];
