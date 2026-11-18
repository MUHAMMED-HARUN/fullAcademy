import { AttendeeType, ContextType } from "@/Generic/AttendanceEnums";

export default class AttendanceMapper {
  static mapToNewAttendanceCommand({ attendances, date }) {
    return {
      Date: date,
      Attendances: attendances.map((item) => ({
        EntityID: item.groupEnrollmentID,
        AttendeeType: AttendeeType.Student,
        Status: Number(item.status),
        ContextID: item.groupID,
        ContextType: ContextType.Group,
      })),
    };
  }

  static mapGroupAttendanceFromApi(items) {
    if (!Array.isArray(items)) return [];
    return items.map((item) => ({
      groupEnrollmentID: item.GroupEnrollmentID,
      groupID: item.GroupID,
      date: item.Date,
      status: Number(item.Status),
    }));
  }
}
