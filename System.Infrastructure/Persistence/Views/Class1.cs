using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Infrastructure.Persistence.Views
{
    public class Class1
    {
       static string  vw_StudentEnrollmentInfo = @"
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
        ss.StudentAcademicEnrollmentID,
        ai.StageInfoID,
        ISNULL((TRY_CAST(ai.[To] AS FLOAT) - TRY_CAST(ai.[From] AS FLOAT)) + 1, 0) AS TargetAmount,
        ISNULL(
            (SELECT SUM((TRY_CAST(de.[To] AS FLOAT) - TRY_CAST(de.[From] AS FLOAT)) + 1) 
             FROM [dbo].[DailyEvaluations] de 
             WHERE de.StageInfoID = ai.StageInfoID
               AND de.StudentAcademicEnrollmentID = ss.StudentAcademicEnrollmentID
               AND de.IsActive = 1 
               AND de.LevelType > 1
             )
        , 0) AS AchievedAmount
    FROM [dbo].[StudentStages] ss 
    INNER JOIN [dbo].[StudentAcademicEnrollmentItems] ai
        ON ai.StudentAcademicEnrollmentID = ss.StudentAcademicEnrollmentID
       AND ai.StageID = ss.StageID
       AND ai.IsActive = 1
    WHERE ss.IsActive = 1
),
StageInfoCompletion AS (
    SELECT 
        StudentAcademicEnrollmentID,
        StageInfoID,
        CASE 
            WHEN TargetAmount > 0 AND AchievedAmount >= TargetAmount THEN 1 
            ELSE 0 
        END AS IsCompleted
    FROM StageInfoProgress
),
ScopeEligibility AS (
    SELECT 
        sic.StudentAcademicEnrollmentID,
        asi.AssignmentScopeID
    FROM [dbo].[AssignmentScopeInfos] asi 
    INNER JOIN StageInfoCompletion sic ON asi.StageInfoID = sic.StageInfoID
    WHERE asi.IsActive = 1 AND sic.IsCompleted = 1
    GROUP BY sic.StudentAcademicEnrollmentID, asi.AssignmentScopeID
    HAVING COUNT(asi.Id) = (
        SELECT COUNT(*) 
        FROM [dbo].[AssignmentScopeInfos] asi2 
        WHERE asi2.AssignmentScopeID = asi.AssignmentScopeID AND asi2.IsActive = 1
    )
),
ExcludedScopes AS (
    SELECT an.StudentAcademicEnrollmentID, an.AssignmentScopeID
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
         WHERE sic.StudentAcademicEnrollmentID = se.StudentAcademicEnrollmentID AND sic.IsCompleted = 0
    ) THEN 1 ELSE 0 END AS BIT) AS CanTrack,

    -- 🟢 CanNominate
    CAST(CASE 
        WHEN EXISTS (
            SELECT 1 
            FROM [dbo].[AssignmentNominations] an
            WHERE an.StudentAcademicEnrollmentID = se.StudentAcademicEnrollmentID AND an.IsActive = 1 AND an.NominationState = 2
        ) THEN 0 
        WHEN EXISTS (
            SELECT 1 
            FROM ScopeEligibility elig
            WHERE elig.StudentAcademicEnrollmentID = se.StudentAcademicEnrollmentID
            AND NOT EXISTS (
                SELECT 1 
                FROM ExcludedScopes excl
                WHERE excl.StudentAcademicEnrollmentID = se.StudentAcademicEnrollmentID AND excl.AssignmentScopeID = elig.AssignmentScopeID
            )
        ) THEN 1 ELSE 0 
    END AS BIT) AS CanNominate,

    -- 🟢 CanAddToStage
    CAST(CASE WHEN EXISTS (
        SELECT 1 
        FROM [dbo].[Stages] stg
        WHERE stg.IsActive = 1 
        AND EXISTS (
            SELECT 1
            FROM [dbo].[StudentAcademicEnrollmentItems] ai
            WHERE ai.StudentAcademicEnrollmentID = se.StudentAcademicEnrollmentID
              AND ai.StageID = stg.Id
              AND ai.IsActive = 1
        )
        AND NOT EXISTS (
            SELECT 1 
            FROM [dbo].[StudentStages] ss
             WHERE ss.StageID = stg.Id AND ss.StudentAcademicEnrollmentID = se.StudentAcademicEnrollmentID AND ss.IsActive = 1
        )
    ) THEN 1 ELSE 0 END AS BIT) AS CanAddToStage

FROM [dbo].[StudentEnrollments] se
LEFT JOIN [dbo].[Students] stu ON se.StudentID = stu.Id
LEFT JOIN [dbo].[People] p ON stu.PersonId = p.Id -- (استبدل PersonId بـ PersonID حسب اسم العمود لديك في جدول Students)
LEFT JOIN [dbo].[Groups] g ON se.GroupID = g.Id
WHERE se.IsActive = 1;
GO


";
        static string vw_StudentAvailableNominationScopes = @"
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[vw_StudentAvailableNominationScopes] AS
WITH StageInfoProgress AS (
    SELECT
        ss.StudentAcademicEnrollmentID,
        item.StageInfoID,
        ISNULL((TRY_CAST(item.[To] AS FLOAT) - TRY_CAST(item.[From] AS FLOAT)) + 1, 0) AS TargetAmount,
        ISNULL((
            SELECT SUM((TRY_CAST(de.[To] AS FLOAT) - TRY_CAST(de.[From] AS FLOAT)) + 1)
            FROM [dbo].[DailyEvaluations] de
            WHERE de.StageInfoID = item.StageInfoID
              AND de.StudentAcademicEnrollmentID = ss.StudentAcademicEnrollmentID
              AND de.IsActive = 1
              AND de.LevelType > 1
        ), 0) AS AchievedAmount
    FROM [dbo].[StudentStages] ss
    INNER JOIN [dbo].[StudentAcademicEnrollmentItems] item
        ON item.StudentAcademicEnrollmentID = ss.StudentAcademicEnrollmentID
       AND item.StageID = ss.StageID
       AND item.IsActive = 1
    WHERE ss.IsActive = 1
),
StageInfoCompletion AS (
    SELECT StudentAcademicEnrollmentID, StageInfoID,
           CASE WHEN TargetAmount > 0 AND AchievedAmount >= TargetAmount THEN 1 ELSE 0 END AS IsCompleted
    FROM StageInfoProgress
),
CompletedScopes AS (
    SELECT completion.StudentAcademicEnrollmentID, scopeInfo.AssignmentScopeID
    FROM [dbo].[AssignmentScopeInfos] scopeInfo
    INNER JOIN StageInfoCompletion completion ON completion.StageInfoID = scopeInfo.StageInfoID
    WHERE scopeInfo.IsActive = 1 AND completion.IsCompleted = 1
    GROUP BY completion.StudentAcademicEnrollmentID, scopeInfo.AssignmentScopeID
    HAVING COUNT(scopeInfo.Id) = (
        SELECT COUNT(*) FROM [dbo].[AssignmentScopeInfos] expected
        WHERE expected.AssignmentScopeID = scopeInfo.AssignmentScopeID AND expected.IsActive = 1
    )
),
PassedScopes AS (
    SELECT nomination.StudentAcademicEnrollmentID, nomination.AssignmentScopeID
    FROM [dbo].[AssignmentNominations] nomination
    INNER JOIN [dbo].[Assignments] assignment ON assignment.AssignmentNominationID = nomination.Id
    WHERE nomination.IsActive = 1 AND assignment.IsActive = 1 AND assignment.IsPassed = 1
),
PendingScopes AS (
    SELECT StudentAcademicEnrollmentID, AssignmentScopeID
    FROM [dbo].[AssignmentNominations]
    WHERE IsActive = 1 AND NominationState = 2
)
SELECT
    academicEnrollment.Id AS StudentAcademicEnrollmentID,
    scope.Id AS AssignmentScopeID,
    scope.Name AS AssignmentScopeName
FROM [dbo].[StudentAcademicEnrollments] academicEnrollment
CROSS JOIN [dbo].[AssignmentScopes] scope
WHERE academicEnrollment.IsActive = 1
  AND academicEnrollment.Status = 1
  AND academicEnrollment.ValidFrom <= CAST(GETDATE() AS date)
  AND (academicEnrollment.ValidTo IS NULL OR academicEnrollment.ValidTo >= CAST(GETDATE() AS date))
  AND scope.IsActive = 1
  AND EXISTS (
      SELECT 1 FROM CompletedScopes completed
      WHERE completed.StudentAcademicEnrollmentID = academicEnrollment.Id
        AND completed.AssignmentScopeID = scope.Id
  )
  AND NOT EXISTS (
      SELECT 1 FROM PassedScopes passed
      WHERE passed.StudentAcademicEnrollmentID = academicEnrollment.Id
        AND passed.AssignmentScopeID = scope.Id
  )
  AND NOT EXISTS (
      SELECT 1 FROM PendingScopes pending
      WHERE pending.StudentAcademicEnrollmentID = academicEnrollment.Id
        AND pending.AssignmentScopeID = scope.Id
  )
  AND (
      scope.PrevAssignmentScopeID IS NULL
      OR EXISTS (
          SELECT 1 FROM PassedScopes previous
          WHERE previous.StudentAcademicEnrollmentID = academicEnrollment.Id
            AND previous.AssignmentScopeID = scope.PrevAssignmentScopeID
      )
  );
GO
";

        static public void CreateView( MigrationBuilder builder)
        {
            builder.Sql(vw_StudentEnrollmentInfo);
            builder.Sql(vw_StudentAvailableNominationScopes) ;
        }
    }
}
