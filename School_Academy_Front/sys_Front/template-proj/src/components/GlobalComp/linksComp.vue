<template>
  <div class="flex flex-wrap items-center gap-1">
    <!-- الرئيسية -->
    <router-link
      to="/"
      class="rounded-lg px-2.5 py-1.5 text-sm font-medium text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-white/5"
    >
      الرئيسية
    </router-link>

    <div
      v-for="group in visibleGroups"
      :key="group.name"
      class="group relative"
    >
      <button
        type="button"
        class="flex items-center gap-1 rounded-lg px-2.5 py-1.5 text-sm font-medium text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-white/5"
      >
        {{ group.name }}
        <svg
          class="size-3.5 transition-transform group-hover:rotate-180"
          viewBox="0 0 20 20"
          fill="currentColor"
        >
          <path
            fill-rule="evenodd"
            d="M5.23 7.21a.75.75 0 011.06.02L10 11.17l3.71-3.94a.75.75 0 111.08 1.04l-4.25 4.5a.75.75 0 01-1.08 0l-4.25-4.5a.75.75 0 01.02-1.06z"
            clip-rule="evenodd"
          />
        </svg>
      </button>
      <div
        class="invisible absolute start-0 top-full z-50 mt-1 min-w-[12rem] rounded-xl border border-gray-200 bg-white p-1.5 opacity-0 shadow-theme-lg transition-all duration-150 group-hover:visible group-hover:opacity-100 dark:border-gray-800 dark:bg-[#1E2635]"
      >
        <router-link
          v-for="link in group.links"
          :key="link.to"
          :to="link.to"
          class="block rounded-lg px-3 py-2 text-sm font-medium text-gray-700 transition-colors hover:bg-gray-100 dark:text-gray-300 dark:hover:bg-white/5"
        >
          {{ link.label }}
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed } from "vue";
import { useAuthStore } from "@/store/Auth/authStore";

const authStore = useAuthStore();

const groups = [
  {
    name: "المصادقة",
    links: [
      { to: "/register", label: "register", roleType: -1 },
      { to: "/roles", label: "Roles", roleType: 1 },
    ],
  },
  {
    name: "العناوين",
    links: [
      { to: "/country", label: "Country", roleType: 1 },
      { to: "/city", label: "City", roleType: 1 },
      { to: "/district", label: "District", roleType: 1 },
      { to: "/neighborhood", label: "Neighborhood", roleType: 1 },
    ],
  },
  {
    name: "الأشخاص",
    links: [
      { to: "/persons", label: "Person", roleType: 1 },
      { to: "/students", label: "Student", roleType: 1 },
      { to: "/teachers", label: "Teacher", roleType: 1 },
    ],
  },
  {
    name: "المؤسسة",
    links: [
      { to: "/institute", label: "Institute", roleType: 1 },
      { to: "/program", label: "program", roleType: 1 },
      { to: "/class", label: "class", roleType: 1 },
      { to: "/instituteClass", label: "instituteClass", roleType: 1 },
      { to: "/group", label: "group", roleType: 1 },
    ],
  },
  {
    name: "التقدم",
    links: [
      { to: "/session", label: "session", roleType: 1 },
      { to: "/stage", label: "Stage", roleType: 1 },
      { to: "/stageinfo", label: "Stage info", roleType: 1 },
    ],
  },
  {
    name: "التقييم",
    links: [
      { to: "/dailyEvaluation", label: "Daily Evaluation", roleType: -1 },
      { to: "/studentStage", label: "Student Stage", roleType: -1 },
    ],
  },
  {
    name: "التسجيل",
    links: [
      { to: "/studentEnrollment", label: "studentEnrollment", roleType: -1 },
      { to: "/teachingAssignment", label: "TeachingAssignment", roleType: -1 },
    ],
  },
  {
    name: "التعيين",
    links: [
      { to: "/assignmentScope", label: "Assignment Scope", roleType: 1 },
      {
        to: "/assignmentScopeInfo",
        label: "Assignment Scope info",
        roleType: 1,
      },
      {
        to: "/assignmentNomination",
        label: "Assignment Nomination",
        roleType: 1,
      },
      { to: "/assignment", label: "Assignment", roleType: 1 },
      { to: "/result", label: "Result", roleType: 1 },
    ],
  },
  {
    name: "المالية",
    links: [
      { to: "/feeStructure", label: "Fee Structure", roleType: 1 },
      { to: "/studentInvoice", label: "Student Invoice", roleType: 1 },
      { to: "/payment", label: "Payment", roleType: 1 },
    ],
  },
];

function canVisable(roleType) {
  if (roleType === -1) return true;
  if (roleType === 1) return authStore.UserRole === "Admin";
  if (roleType === 0) return authStore.UserRole === "Teacher";
  return false;
}

const visibleGroups = computed(() =>
  groups
    .map((g) => ({
      ...g,
      links: g.links.filter((l) => canVisable(l.roleType)),
    }))
    .filter((g) => g.links.length > 0)
);
</script>
