Create OR ALTER   PROCEDURE [dbo].[spJobApplication_Insert]
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

ALTER   PROCEDURE [dbo].[spJobApplication_Select]
    @Keyword UNIQUEIDENTIFIER,
	@JobPositionId UNIQUEIDENTIFIER,
	@AppliedFrom Datetime,
	@AppliedTo Datetime
AS
BEGIN
    SET NOCOUNT ON;
    select app.Id, app.FullName, app.Gender, app.CvFileUrl, app.CoverLetter, app.AppliedAt, job.Id, job.Title, job.Department from dbo.JobApplication as app 
	inner join dbo.JobPosition as job
	on app.JobPositionId = job.Id
	where app.FullName like '%@Keyword%' or app.Email like '%@Keyword%' 
	and JobPositionId = @JobPositionId and app.AppliedAt between @AppliedFrom and @AppliedTo
END

ALTER   PROCEDURE [dbo].[spJobApplication_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select app.Id, app.FullName, app.Email, app.Gender, app.CvFileUrl, app.AppliedAt, job.Id, job.Title from dbo.JobApplication as app
	inner join dbo.JobPosition as job
	on app.JobPositionId = job.Id
	where app.Id = @Id and app.IsDeleted = 0
END

ALTER   PROCEDURE [dbo].[spJobApplication_GetListJobApplicationByJobPositionId]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select app.Id, app.FullName, app.Gender, app.CvFileUrl, app.CoverLetter, app.AppliedAt from dbo.JobPosition as job 
	inner join dbo.JobApplication as app
	on job.Id = app.JobPositionId
	where job.Id = @Id and job.IsDeleted = 0
END