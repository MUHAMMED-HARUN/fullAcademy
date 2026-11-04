import { defineStore } from "pinia";
import AttendanceService from "@/Service/Attendance/AttendanceService";

export const useAttendanceStore = defineStore("attendance", {
  state: () => ({
    loading: false,
    error: null,
  }),

  actions: {
    async submitAttendance(attendeesList) {
      this.loading = true;
      this.error = null;

      const result = await AttendanceService.saveAttendance(attendeesList);

      if (!result.success) {
        this.error = result.errors;
      }

      this.loading = false;
      return result;
    },
  },
});
