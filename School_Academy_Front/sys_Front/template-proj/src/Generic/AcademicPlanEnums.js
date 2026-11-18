export const AcademicPlanScopeType = Object.freeze({
  Program: 1,
  InstituteClass: 2,
  Stage: 3,
  StageInfo: 4,
});

export const AcademicPlanScopeTypeOptions = [
  { value: AcademicPlanScopeType.Program, label: "برنامج" },
  { value: AcademicPlanScopeType.InstituteClass, label: "صف المدرسة" },
  { value: AcademicPlanScopeType.Stage, label: "مرحلة" },
  { value: AcademicPlanScopeType.StageInfo, label: "مادة واحدة" },
];

export const AcademicPlanStatus = Object.freeze({
  Draft: 1,
  Active: 2,
  Archived: 3,
});

export const AcademicPlanStatusOptions = [
  { value: AcademicPlanStatus.Draft, label: "مسودة" },
  { value: AcademicPlanStatus.Active, label: "نشطة" },
  { value: AcademicPlanStatus.Archived, label: "مؤرشفة" },
];
