<template>
  <div class="table-responsive">
    <table class="data-table">
      <thead>
        <tr>
          <th class="checkbox-col">
            <input
              type="checkbox"
              :checked="isAllSelected"
              @change="$emit('toggle-select-all', $event.target.checked)"
            />
          </th>
          <th>اسم الطالب</th>
          <th>اسم المجموعة</th>
          <th>تاريخ التسجيل</th>
          <th v-if="showAttendance" class="attendance-col">حالة الحضور</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="enroll in enrollments" :key="enroll.enrollmentID">
          <td class="checkbox-col">
            <input
              type="checkbox"
              :checked="selectedIds.includes(enroll.enrollmentID)"
              @change="$emit('toggle-select-row', enroll)"
            />
          </td>
          <td>{{ enroll.studentName }}</td>
          <td>{{ enroll.groupName }}</td>
          <td>{{ enroll.date }}</td>
          <td v-if="showAttendance" class="attendance-col">
            <select
              class="attendance-select"
              :value="getAttendanceStatus(enroll.enrollmentID)"
              @change="
                $emit('update-attendance', {
                  enrollmentID: enroll.enrollmentID,
                  studentID: enroll.studentID,
                  status: Number($event.target.value),
                })
              "
            >
              <option
                v-for="opt in attendanceOptions"
                :key="opt.value"
                :value="opt.value"
              >
                {{ opt.label }}
              </option>
            </select>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import {
  AttendanceStatus,
  AttendanceStatusOptions,
} from "@/Generic/AttendanceEnums";

export default {
  name: "StudentEnrollmentList",
  props: {
    enrollments: {
      type: Array,
      required: true,
    },
    selectedIds: {
      type: Array,
      default: () => [],
    },
    showAttendance: {
      type: Boolean,
      default: false,
    },
    attendanceMap: {
      type: Object,
      default: () => ({}),
    },
  },
  data() {
    return {
      attendanceOptions: AttendanceStatusOptions,
    };
  },
  computed: {
    isAllSelected() {
      return (
        this.enrollments.length > 0 &&
        this.enrollments.every((en) =>
          this.selectedIds.includes(en.enrollmentID)
        )
      );
    },
  },
  methods: {
    getAttendanceStatus(enrollmentID) {
      if (
        this.attendanceMap &&
        this.attendanceMap[enrollmentID] !== undefined
      ) {
        return this.attendanceMap[enrollmentID];
      }
      return AttendanceStatus.Present;
    },
  },
  emits: ["toggle-select-all", "toggle-select-row", "update-attendance"],
};
</script>

<style scoped>
.table-responsive {
  width: 100%;
  overflow-x: auto;
  background: #fff;
  border: 1px solid #e0e0e0;
  border-radius: 6px;
}
.data-table {
  width: 100%;
  border-collapse: collapse;
  text-align: right;
}
.data-table th,
.data-table td {
  padding: 12px;
  border-bottom: 1px solid #eee;
  font-size: 14px;
}
.data-table th {
  background-color: #f8f9fa;
  font-weight: 600;
  color: #333;
}
.checkbox-col {
  width: 40px;
  text-align: center;
}
.attendance-col {
  width: 140px;
  text-align: center;
}
.attendance-select {
  width: 100%;
  padding: 6px 10px;
  border: 1px solid #ced4da;
  border-radius: 4px;
  background-color: #fff;
  font-size: 13px;
  color: #333;
  outline: none;
  cursor: pointer;
  transition: border-color 0.2s;
}
.attendance-select:focus {
  border-color: #0d6efd;
}
</style>
