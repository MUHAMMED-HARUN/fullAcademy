export const AttendanceStatus = Object.freeze({
  Present: 1, // حضور
  Absent: 2, // غياب
  Late: 3, // تأخر
  Excused: 4, // إذن / معذور
});

export const AttendeeType = Object.freeze({
  Student: 1,
  Teacher: 2,
  Employee: 3,
});

export const ContextType = Object.freeze({
  None: 0,
  Group: 1,
  Session: 2,
});

export const AttendanceStatusOptions = [
  { value: AttendanceStatus.Present, label: "حضور" },
  { value: AttendanceStatus.Absent, label: "غياب" },
  { value: AttendanceStatus.Late, label: "تأخير" },
  { value: AttendanceStatus.Excused, label: "إذن" },
];
