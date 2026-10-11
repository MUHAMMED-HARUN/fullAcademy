using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Infrastructure.Persistence.Views
{
    internal class Class1
    {
        string vw_StudentEnrollmentInfo = @"
GO

/****** Object:  View [dbo].[vw_StudentEnrollmentInfo]    Script Date: 22/09/2026 11:51:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW [dbo].[vw_StudentEnrollmentInfo] AS
WITH 
StageInfoProgress AS (
    SELECT 
        ss.EnrollmentStudentID,
        si.Id AS StageInfoID,
        ISNULL((TRY_CAST(si.[To] AS FLOAT) - TRY_CAST(si.[From] AS FLOAT)) + 1, 0) AS TargetAmount,
        ISNULL(
            (SELECT SUM((TRY_CAST(de.[To] AS FLOAT) - TRY_CAST(de.[From] AS FLOAT)) + 1) 
             FROM [dbo].[DailyEvaluations] de 
             WHERE de.StageInfoID = si.Id 
               AND de.StudentEnrollmentID = ss.EnrollmentStudentID 
               AND de.IsActive = 1 
               AND de.LevelType > 1
            )
        , 0) AS AchievedAmount
    FROM [dbo].[StudentStages] ss 
    INNER JOIN [dbo].[StageInfos] si ON ss.StageID = si.StageID AND si.IsActive = 1
    WHERE ss.IsActive = 1
),
StageInfoCompletion AS (
    SELECT 
        EnrollmentStudentID,
        StageInfoID,
        CASE 
            WHEN TargetAmount > 0 AND AchievedAmount >= TargetAmount THEN 1 
            ELSE 0 
        END AS IsCompleted
    FROM StageInfoProgress
),
ScopeEligibility AS (
    SELECT 
        sic.EnrollmentStudentID,
        asi.AssignmentScopeID
    FROM [dbo].[AssignmentScopeInfos] asi 
    INNER JOIN StageInfoCompletion sic ON asi.StageInfoID = sic.StageInfoID
    WHERE asi.IsActive = 1 AND sic.IsCompleted = 1
    GROUP BY sic.EnrollmentStudentID, asi.AssignmentScopeID
    HAVING COUNT(asi.Id) = (
        SELECT COUNT(*) 
        FROM [dbo].[AssignmentScopeInfos] asi2 
        WHERE asi2.AssignmentScopeID = asi.AssignmentScopeID AND asi2.IsActive = 1
    )
),
ExcludedScopes AS (
    SELECT an.StudentEnrollmentID, an.AssignmentScopeID
    FROM [dbo].[AssignmentNominations] an
    WHERE an.IsActive = 1 
    AND EXISTS (
        SELECT 1 
        FROM [dbo].[Assignments] a
        WHERE a.AssignmentNominationID = an.Id AND a.IsPassed = 1 AND a.IsActive = 1
    )
)
SELECT 
    se.[Id] AS EnrollmentID,
    se.[StudentID],
    CONCAT_WS(' ', p.[Firstname], p.[Fathername], p.[LastName]) AS StudentName,
    se.[GroupID],
    g.[GroupName] AS GroupName,
    se.[Date],

    -- 🟢 CanTrack
    CAST(CASE WHEN EXISTS (
        SELECT 1 
        FROM StageInfoCompletion sic 
        WHERE sic.EnrollmentStudentID = se.Id AND sic.IsCompleted = 0
    ) THEN 1 ELSE 0 END AS BIT) AS CanTrack,

    -- 🟢 CanNominate
    CAST(CASE 
        WHEN EXISTS (
            SELECT 1 
            FROM [dbo].[AssignmentNominations] an
            WHERE an.StudentEnrollmentID = se.Id AND an.IsActive = 1 AND an.NominationState = 2
        ) THEN 0 
        WHEN EXISTS (
            SELECT 1 
            FROM ScopeEligibility elig
            WHERE elig.EnrollmentStudentID = se.Id 
            AND NOT EXISTS (
                SELECT 1 
                FROM ExcludedScopes excl
                WHERE excl.StudentEnrollmentID = se.Id AND excl.AssignmentScopeID = elig.AssignmentScopeID
            )
        ) THEN 1 ELSE 0 
    END AS BIT) AS CanNominate,

    -- 🟢 CanAddToStage
    CAST(CASE WHEN EXISTS (
        SELECT 1 
        FROM [dbo].[Stages] stg
        WHERE stg.IsActive = 1 
        AND NOT EXISTS (
            SELECT 1 
            FROM [dbo].[StudentStages] ss
            WHERE ss.StageID = stg.Id AND ss.EnrollmentStudentID = se.Id AND ss.IsActive = 1
        )
    ) THEN 1 ELSE 0 END AS BIT) AS CanAddToStage

FROM [dbo].[StudentEnrollments] se
LEFT JOIN [dbo].[Students] stu ON se.StudentID = stu.Id
LEFT JOIN [dbo].[People] p ON stu.PersonId = p.Id -- (استبدل PersonId بـ PersonID حسب اسم العمود لديك في جدول Students)
LEFT JOIN [dbo].[Groups] g ON se.GroupID = g.Id
WHERE se.IsActive = 1;
GO


";
        string vw_StudentAvailableNominationScopes = @"
GO

/****** Object:  View [dbo].[vw_StudentAvailableNominationScopes]    Script Date: 22/09/2026 11:50:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   VIEW [dbo].[vw_StudentAvailableNominationScopes] AS
WITH 
-- 1. حساب تقدم تفاصيل المراحل لكل الطلاب
StageInfoProgress AS (
    SELECT 
        ss.EnrollmentStudentID,
        si.Id AS StageInfoID,
        ISNULL((TRY_CAST(si.[To] AS FLOAT) - TRY_CAST(si.[From] AS FLOAT)) + 1, 0) AS TargetAmount,
        ISNULL(
            (SELECT SUM((TRY_CAST(de.[To] AS FLOAT) - TRY_CAST(de.[From] AS FLOAT)) + 1) 
             FROM [dbo].[DailyEvaluations] de 
             WHERE de.StageInfoID = si.Id 
               AND de.StudentEnrollmentID = ss.EnrollmentStudentID 
               AND de.IsActive = 1 
               AND de.LevelType > 1
            )
        , 0) AS AchievedAmount
    FROM [dbo].[StudentEnrollments] se
    INNER JOIN [dbo].[StudentStages] ss ON ss.EnrollmentStudentID = se.Id AND ss.IsActive = 1
    INNER JOIN [dbo].[StageInfos] si ON ss.StageID = si.StageID AND si.IsActive = 1
    WHERE se.IsActive = 1
),

-- 2. تحديد تفاصيل المراحل المكتملة لكل الطلاب
StageInfoCompletion AS (
    SELECT 
        EnrollmentStudentID,
        StageInfoID,
        CASE WHEN TargetAmount > 0 AND AchievedAmount >= TargetAmount THEN 1 ELSE 0 END AS IsCompleted
    FROM StageInfoProgress
),

-- 3. نطاقات الاختبار المكتملة لكل طالب
CompletedScopes AS (
    SELECT 
        sic.EnrollmentStudentID,
        asi.AssignmentScopeID
    FROM [dbo].[AssignmentScopeInfos] asi
    INNER JOIN StageInfoCompletion sic ON asi.StageInfoID = sic.StageInfoID
    WHERE asi.IsActive = 1 AND sic.IsCompleted = 1
    GROUP BY sic.EnrollmentStudentID, asi.AssignmentScopeID
    HAVING COUNT(asi.Id) = (
        SELECT COUNT(*) 
        FROM [dbo].[AssignmentScopeInfos] asi2 
        WHERE asi2.AssignmentScopeID = asi.AssignmentScopeID AND asi2.IsActive = 1
    )
),

-- 4. النطاقات التي نجح فيها الطالب مسبقاً
PassedScopes AS (
    SELECT 
        an.StudentEnrollmentID,
        an.AssignmentScopeID
    FROM [dbo].[AssignmentNominations] an
    INNER JOIN [dbo].[Assignments] a ON a.AssignmentNominationID = an.Id
    WHERE an.IsActive = 1 
      AND a.IsActive = 1 
      AND a.IsPassed = 1
),

-- 5. النطاقات التي لديه فيها ترشيح نشط قيد الانتظار حالياً
PendingScopes AS (
    SELECT 
        an.StudentEnrollmentID,
        an.AssignmentScopeID
    FROM [dbo].[AssignmentNominations] an
    WHERE an.IsActive = 1 
      AND an.NominationState = 2
)

-- 6. الاستعلام النهائي لجلب الاختبارات المتاحة لكل الطلاب مع مراعاة التسلسل
SELECT 
    CAST(se.[Id] AS VARCHAR(36)) AS EnrollmentID,
    CAST(se.[StudentID] AS VARCHAR(36)) AS StudentID,
    CONCAT_WS(' ', p.[Firstname], p.[Fathername], p.[LastName]) AS StudentName,
    CAST(se.[GroupID] AS VARCHAR(36)) AS GroupID,
    g.[GroupName] AS GroupName,
    CAST(asp.[Id] AS VARCHAR(36)) AS AssignmentScopeID,
    asp.[Name] AS AssignmentScopeName,
    se.[Date]
FROM [dbo].[StudentEnrollments] se
LEFT JOIN [dbo].[Students] stu ON se.StudentID = stu.Id
LEFT JOIN [dbo].[People] p ON stu.PersonId = p.Id
LEFT JOIN [dbo].[Groups] g ON se.GroupID = g.Id
CROSS JOIN [dbo].[AssignmentScopes] asp
WHERE se.IsActive = 1 
  AND asp.IsActive = 1
  -- ألا يكون محذوفاً وأنه أكمل متطلبات تفاصيله الدراسية
  AND EXISTS (
      SELECT 1 FROM CompletedScopes cs 
      WHERE cs.EnrollmentStudentID = se.Id AND cs.AssignmentScopeID = asp.Id
  )
  -- ألا يكون الطالب قد نجح فيه مسبقاً
  AND NOT EXISTS (
      SELECT 1 FROM PassedScopes ps 
      WHERE ps.StudentEnrollmentID = se.Id AND ps.AssignmentScopeID = asp.Id
  )
  -- ألا يكون لديه اختبار قيد الانتظار لهذا النطاق حالياً
  AND NOT EXISTS (
      SELECT 1 FROM PendingScopes pnd 
      WHERE pnd.StudentEnrollmentID = se.Id AND pnd.AssignmentScopeID = asp.Id
  )
  -- مراعاة تسلسل الاختبارات (الشرط السابق)
  AND (
      asp.PrevAssignmentScopeID IS NULL 
      OR EXISTS (
          SELECT 1 FROM PassedScopes ps 
          WHERE ps.StudentEnrollmentID = se.Id AND ps.AssignmentScopeID = asp.PrevAssignmentScopeID
      )
  );
GO


";

    }
}
