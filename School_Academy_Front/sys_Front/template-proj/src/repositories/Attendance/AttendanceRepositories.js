import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";

const baseUrl = "/Attendances";

class AttendanceRepositories {
  async addBatch(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewAttendance`,
      enApiType.post,
      data,
      "حدث خطأ أثناء تسجيل الحضور والغياب"
    );
    return response;
  }
}

export default new AttendanceRepositories();
