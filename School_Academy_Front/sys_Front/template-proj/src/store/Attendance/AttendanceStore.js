import { defineStore } from "pinia";
import AttendanceService from "@/Service/Attendance/AttendanceService";

export const useAttendanceStore = defineStore("attendance", {
  state: () => ({
    loading: false,
    error: null,
    records: [],
    attendanceRequestID: 0,
  }),

  actions: {
    async submitAttendance(payload) {
      this.attendanceRequestID += 1;
      this.loading = true;
      this.error = null;
      try {
        const result = await AttendanceService.saveAttendance(payload);
        if (!result.success) this.error = result.errors;
        return result;
      } finally {
        this.loading = false;
      }
    },
    async fetchGroupAttendance(groupID, date) {
      const requestID = ++this.attendanceRequestID;
      this.loading = true;
      this.error = null;
      this.records = [];
      try {
        const result = await AttendanceService.getGroupAttendance({
          groupID,
          date,
        });
        if (requestID !== this.attendanceRequestID) return result;
        if (result.success) this.records = result.data || [];
        else this.error = result.errors;
        return result;
      } finally {
        if (requestID === this.attendanceRequestID) this.loading = false;
      }
    },
    clearGroupAttendance() {
      this.attendanceRequestID += 1;
      this.records = [];
      this.error = null;
      this.loading = false;
    },
  },
});
