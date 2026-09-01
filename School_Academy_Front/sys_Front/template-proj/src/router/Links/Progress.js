import DailyEvaluationView from "@/views/Progress/DailyEvaluationView.vue";
import SessionView from "@/views/Progress/SessionView.vue";
import StageInfoView from "@/views/Progress/StageInfoView.vue";
import StageView from "@/views/Progress/StageView.vue";
import StudentStageView from "@/views/Progress/StudentStageView.vue";

export const ProgressLinks = [
  {
    path: "/session",
    name: "Session",
    component: SessionView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/stage",
    name: "Stage",
    component: StageView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/stageinfo",
    name: "StageInfo",
    component: StageInfoView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/dailyEvaluation",
    name: "DailyEvaluation",
    component: DailyEvaluationView,
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: "/studentStage",
    name: "studentStage",
    component: StudentStageView,
    meta: {
      requiresAuth: true,
    },
  },
];
