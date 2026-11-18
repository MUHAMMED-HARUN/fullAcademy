const fullName = (student) =>
  [student?.FirstName, student?.FatherName, student?.LastName]
    .filter(Boolean)
    .join(" ");

export default class StudentAcademicEnrollmentMapper {
  static mapCreateToApi(data) {
    return {
      StudentID: data.studentID,
      AcademicPlanID: data.academicPlanID,
      EnrollmentDate: data.enrollmentDate,
      ValidFrom: data.validFrom,
      ValidTo: data.validTo || null,
    };
  }

  static mapFromApi(data) {
    if (!data) return null;
    return {
      studentAcademicEnrollmentID: data.StudentAcademicEnrollmentID,
      studentID: data.StudentID,
      studentName: data.StudentName,
      academicPlanID: data.AcademicPlanID,
      academicPlanName: data.AcademicPlanName,
      academicPlanVersion: data.AcademicPlanVersion,
      enrollmentDate: data.EnrollmentDate,
      status: data.Status,
      validFrom: data.ValidFrom,
      validTo: data.ValidTo,
      items: (data.Items || []).map((item) => ({
        stageInfoID: item.StageInfoID,
        from: item.From,
        to: item.To,
        stageID: item.StageID,
        stageName: item.StageName,
        matterID: item.MatterID,
        matterName: item.MatterName,
        uniteType: item.UniteType,
        uniteTypeName: item.UniteTypeName,
        progressType: item.ProgressType,
        progressTypeName: item.ProgressTypeName,
      })),
    };
  }

  static mapLookupFromApi(data) {
    if (!data) return null;
    const studentName = data.StudentName || fullName(data.Student);
    const academicPlanName = data.AcademicPlanName || "";
    return {
      studentAcademicEnrollmentID: data.StudentAcademicEnrollmentID,
      studentID: data.StudentID,
      studentName,
      academicPlanID: data.AcademicPlanID,
      academicPlanName,
      academicPlanVersion: data.AcademicPlanVersion,
      enrollmentDate: data.EnrollmentDate,
      status: data.Status,
      validFrom: data.ValidFrom,
      validTo: data.ValidTo,
      itemCount: data.ItemCount,
      displayName: `${studentName} — ${academicPlanName} (v${data.AcademicPlanVersion})`,
    };
  }
}
