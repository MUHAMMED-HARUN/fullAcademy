import { AttendeeType } from "@/Generic/AttendanceEnums";

export default class AttendanceMapper {
  static mapToNewAttendanceCommand(attendeesList) {
    if (!attendeesList || !Array.isArray(attendeesList)) {
      return { Attendances: [] };
    }

    return {
      Attendances: attendeesList.map((item) => ({
        EntityID: item.entityID || item.studentID || item.enrollmentID,
        AttendeeType: item.attendeeType ?? AttendeeType.Student,
        Status: Number(item.status),
        ContextID: item.contextID || null,
        ContextType:
          item.contextType !== undefined ? Number(item.contextType) : null,
      })),
    };
  }
}
