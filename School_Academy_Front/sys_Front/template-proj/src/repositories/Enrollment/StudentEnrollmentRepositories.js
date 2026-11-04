import clsUtil from "@/Generic/clsUtil";
import { enApiType } from "@/Generic/clsUtil";
const baseUrl = "/StudentEnrollment";
class StudentEnrollmentRepositories {
  async add(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/NewStudentEnrollment`,
      enApiType.post,
      data,
      "error"
    );
    return response;
  }

  async enrollByRange(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/EnrollByRange`,
      enApiType.post,
      data,
      "error"
    );
    return response;
  }

  async update(data) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/UpdateStudentEnrollment`,
      enApiType.put,
      data,
      "error"
    );
    return response;
  }

  async getPagedList(params) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentEnrollmentList`,
      enApiType.get,
      { params: params },
      "error"
    );
    return response;
  }

  async getBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/GetStudentEnrollmentBy`,
      enApiType.get,
      { params: param },
      "error"
    );
    return response;
  }

  async Delete(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/DeleteStudentEnrollment`,
      enApiType.delete,
      param,
      "error"
    );
    return response;
  }

  async IsExistBy(param) {
    const response = await clsUtil.FeachRequset(
      `${baseUrl}/IsExistStudentEnrollmentBy`,
      enApiType.get,
      param,
      "error"
    );
    return response;
  }
}

export default new StudentEnrollmentRepositories();
