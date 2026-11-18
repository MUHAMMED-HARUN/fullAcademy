import TResult from "@/Generic/TResult";
import AttendanceMapper from "@/Mapper/Attendance/AttendanceMapper";
import AttendanceRepositories from "@/repositories/Attendance/AttendanceRepositories";
import {
  AttendanceBatchSchema,
  GroupAttendanceQuerySchema,
} from "@/validations/Attendance/AttendanceSchema";
import { validate1 } from "@/validations/validator";

class AttendanceService {
  async getGroupAttendance(params) {
    const validation = validate1(params, GroupAttendanceQuerySchema);
    if (!validation.isValid) return new TResult(null, false, validation.errors);
    const response = await AttendanceRepositories.getGroupAttendance({
      GroupID: params.groupID,
      Date: params.date,
    });
    if (!response.success) return new TResult(null, false, response.errors);
    return new TResult(
      AttendanceMapper.mapGroupAttendanceFromApi(response.data),
      true,
      response.errors
    );
  }

  async saveAttendance(payload) {
    const validation = validate1(payload, AttendanceBatchSchema);
    if (!validation.isValid) {
      return new TResult(null, false, validation.errors);
    }

    return await AttendanceRepositories.addBatch(
      AttendanceMapper.mapToNewAttendanceCommand(payload)
    );
  }
}

export default new AttendanceService();
