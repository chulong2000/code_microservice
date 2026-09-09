USE [DemoEducationLevelDb]
GO
/****** Object:  StoredProcedure [dbo].[JobApplication_Update]    Script Date: 09/09/2026 9:54:31 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[JobApplication_Update]
    @Id UNIQUEIDENTIFIER,@JobPositionId UNIQUEIDENTIFIER, @FullName NVARCHAR(100), @Email NVARCHAR(500),
    @PhoneNumber NVARCHAR(100), @DateOfBirth DATETIME, @Gender VARCHAR(40),@CvFileUrl VARCHAR(120), @CoverLetter VARCHAR(120),
	@YearsOfExperience INT, @AppliedAt DATETIME, @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.JobApplication
    SET JobPositionId = @JobPositionId, FullName = @FullName, 
	    Email = @Email, PhoneNumber = @PhoneNumber,
		DateOfBirth = @DateOfBirth, Gender = @Gender,
		CvFileUrl = @CvFileUrl, CoverLetter = @CoverLetter,
		YearsOfExperience = @YearsOfExperience, AppliedAt = @AppliedAt,
		UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO
/****** Object:  StoredProcedure [dbo].[spJobApplication_GetListJobApplicationByJobPositionId]    Script Date: 09/09/2026 9:54:31 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[spJobApplication_GetListJobApplicationByJobPositionId]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select app.Id, app.FullName, app.Gender, app.CvFileUrl, app.CoverLetter, app.AppliedAt from dbo.JobPosition as job 
	inner join dbo.JobApplication as app
	on job.Id = app.JobPositionId
	where job.Id = @Id and job.IsDeleted = 0
END
GO
/****** Object:  StoredProcedure [dbo].[spJobApplication_Insert]    Script Date: 09/09/2026 9:54:31 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create     PROCEDURE [dbo].[spJobApplication_Insert]
    @Id UNIQUEIDENTIFIER, @JobPositionId UNIQUEIDENTIFIER, @FullName NVARCHAR(500),
    @Email varchar(200), @PhoneNumber varchar(50), @DateOfBirth DateTime, @Gender varchar(120), @CvFileUrl varchar(150), 
	@CoverLetter nvarchar (500), @YearOfExperience int, @AppliedAt Datetime, @CreatedAt datetime
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.JobApplication (Id, JobPositionId, FullName, Email, PhoneNumber, DateOfBirth, Gender, CvFileUrl, CoverLetter, YearsOfExperience, AppliedAt, CreatedAt, IsDeleted)
    VALUES (@Id, @JobPositionId, @FullName, @Email, @PhoneNumber, @DateOfBirth, @Gender, @CvFileUrl, @CoverLetter, @YearOfExperience, @AppliedAt, @CreatedAt, 0);

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO
/****** Object:  StoredProcedure [dbo].[spJobApplication_Select]    Script Date: 09/09/2026 9:54:31 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spJobApplication_Select]
    @Keyword nvarchar(100) = NULL,
	@JobPositionId UNIQUEIDENTIFIER = Null,
	@AppliedFrom Datetime = null,
	@AppliedTo Datetime = null
AS
BEGIN
    SET NOCOUNT ON;
	DECLARE @Pattern nvarchar(102) = N'%' + @Keyword + N'%';
    select app.Id, app.FullName, app.Gender, app.Email,app.DateOfBirth, app.YearsOfExperience ,app.PhoneNumber,app.CvFileUrl, app.CoverLetter, app.AppliedAt, job.Id, job.Title, job.Department from dbo.JobApplication as app 
	inner join dbo.JobPosition as job
	on app.JobPositionId = job.Id
	 WHERE (@Keyword IS NULL
           OR app.FullName LIKE @Pattern
           OR app.Email    LIKE @Pattern)      -- 
      AND (@JobPositionId IS NULL OR app.JobPositionId = @JobPositionId)
      AND (@AppliedFrom   IS NULL OR app.AppliedAt >= @AppliedFrom)
      AND (@AppliedTo     IS NULL OR app.AppliedAt <  DATEADD(DAY, 1, @AppliedTo))
	  AND job.IsDeleted = 0
	  AND app.IsDeleted = 0;
END
GO
/****** Object:  StoredProcedure [dbo].[spJobApplication_SelectById]    Script Date: 09/09/2026 9:54:31 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[spJobApplication_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select app.Id, app.FullName, app.Email, app.PhoneNumber ,app.Gender, app.DateOfBirth, app.YearsOfExperience ,app.CvFileUrl, app.AppliedAt, job.Id, job.Title from dbo.JobApplication as app
	inner join dbo.JobPosition as job
	on app.JobPositionId = job.Id
	where app.Id = @Id and app.IsDeleted = 0
END
GO
/****** Object:  StoredProcedure [dbo].[spJobApplication_SoftDelete]    Script Date: 09/09/2026 9:54:31 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create    PROCEDURE [dbo].[spJobApplication_SoftDelete]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.JobApplication SET IsDeleted = 1
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO
