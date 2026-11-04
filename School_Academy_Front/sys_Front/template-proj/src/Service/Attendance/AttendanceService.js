import TResult from "@/Generic/TResult";
import AttendanceMapper from "@/Mapper/Attendance/AttendanceMapper";
import AttendanceRepositories from "@/repositories/Attendance/AttendanceRepositories";
import { AttendanceBatchSchema } from "@/validations/Attendance/AttendanceSchema";
import { validate1 } from "@/validations/validator";

class AttendanceService {
  async saveAttendance(attendeesList) {
    const validation = validate1(attendeesList, AttendanceBatchSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    const payload = AttendanceMapper.mapToNewAttendanceCommand(attendeesList);
    return await AttendanceRepositories.addBatch(payload);
  }
}

export default new AttendanceService();
