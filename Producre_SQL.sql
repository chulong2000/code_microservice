

CREATE OR ALTER PROCEDURE dbo.spJobPosition_ExistsName
    @Title      NVARCHAR(100),
    @ExcludeId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM dbo.JobPosition
        WHERE Title = @Title AND IsDeleted = 0
          AND (@ExcludeId IS NULL OR Id <> @ExcludeId)
    ) THEN 1 ELSE 0 END;
END
GO


CREATE OR ALTER PROCEDURE dbo.spEducationLevel_CountJobPosition
   
AS
BEGIN
    SET NOCOUNT ON;
    Select edu.Name, count(job.id) as numberOfJobPosition from dbo.EducationLevel as edu
	left join dbo.JobPosition as job
	on edu.Id = job.MinimumEducationLevelId
	group by edu.Name
END

Select * from dbo.EducationLevel as edu
	left join dbo.JobPosition as job
	on edu.Id = job.MinimumEducationLevelId
	



select * from dbo.JobPosition

select * from dbo.EducationLevel

CREATE OR ALTER PROCEDURE dbo.spJobPosition_Insert
    @Id UNIQUEIDENTIFIER, @Title NVARCHAR(100), @Department NVARCHAR(500),
    @OpenSlots INT, @EducationLevelId UNIQUEIDENTIFIER, @IsOpen bit, @CreatedAt datetime, @IsDeleted bit
AS
BEGIN
    SET NOCOUNT ON;

    -- Guard ch?ng race condition: 2 request t?o cùng tên g?n nh? ??ng th?i.
    IF EXISTS (SELECT 1 FROM dbo.JobPosition WHERE Title = @Title AND IsDeleted = 0)
    BEGIN
        SELECT -1;
        RETURN;
    END

    INSERT INTO dbo.JobPosition (Id, Title, Department, OpenSlots, MinimumEducationLevelId, IsOpen, CreatedAt, IsDeleted)
    VALUES (@Id, @Title, @Department , @OpenSlots, @EducationLevelId, @IsOpen, @CreatedAt, @IsDeleted);

    SELECT 1;
END
GO

CREATE OR ALTER PROCEDURE dbo.spJobPosition_Update
    @Id UNIQUEIDENTIFIER, @Title NVARCHAR(100), @Department NVARCHAR(500),
    @OpenSlots INT, @EducationLevelId UNIQUEIDENTIFIER, @IsOpen bit,@UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.JobPosition WHERE Title = @Title AND IsDeleted = 0 AND Id <> @Id)
    BEGIN
        SELECT -1;
        RETURN;
    END

    UPDATE dbo.JobPosition
    SET Title = @Title, Department = @Department, OpenSlots = @OpenSlots, MinimumEducationLevelId = @EducationLevelId,
    IsOpen = @IsOpen, UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO

CREATE OR ALTER PROCEDURE dbo.spJobPosition_SoftDelete
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.JobPosition SET IsDeleted = 1
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO

CREATE OR ALTER PROCEDURE dbo.spJobPosition_SelectList
    @Keyword NVARCHAR(100) = NULL,
    @EducationId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select job.Id, job.Title, job.Department,job.OpenSlots, job.CreatedAt, job.UpdatedAt, job.MinimumEducationLevelId as Id, Name, Description
    from dbo.JobPosition as job 
    inner join dbo.EducationLevel as education
    on job.MinimumEducationLevelId = education.Id
	where job.IsDeleted = 0 and job.MinimumEducationLevelId = @EducationId
	AND (@Keyword IS NULL OR Title LIKE '%' + @Keyword + '%')
END
GO

CREATE OR ALTER PROCEDURE dbo.spJobPosition_SelectById
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select job.Id, job.Title, job.Department,job.OpenSlots, job.CreatedAt, job.UpdatedAt, education.Id, education.Name 
    from dbo.JobPosition as job 
    inner join dbo.EducationLevel as education
	on job.MinimumEducationLevelId = education.Id
    WHERE job.Id = @Id AND job.IsDeleted = 0;
END
GO


CREATE OR ALTER PROCEDURE dbo.spJobPosition_SelectListJobPostionByEducationLevelId
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select job.Id, job.Title, job.Department,job.OpenSlots, job.CreatedAt, job.UpdatedAt
    from dbo.JobPosition as job 
    inner join dbo.EducationLevel as education
	on job.MinimumEducationLevelId = education.Id
    WHERE education.Id = @Id AND job.IsDeleted = 0;
END







select * from dbo.EducationLevel;

Update dbo.EducationLevel 
set IsDeleted = 0 where id = 'C8E643B8-1CBB-48EC-8AF9-F9622FDF3792'

select * from dbo.JobPosition;

Delete from dbo.JobPosition where Id = '8CA9B0AE-4DD1-42DF-8894-83E3E592BEBA'

select * from dbo.JobPosition;



select job.Id, job.Title, job.Department,job.OpenSlots, job.CreatedAt, job.UpdatedAt, education.Id as educationId, education.Name as educationName, education.Description
    from dbo.JobPosition as job 
    inner join dbo.EducationLevel as education
    on job.MinimumEducationLevelId = education.Id
	where job.IsDeleted = 0 and job.MinimumEducationLevelId = 'C8E643B8-1CBB-48EC-8AF9-F9622FDF3792'
	AND  Name LIKE '%' + '' + '%'


	select job.Id, job.Title, job.Department,job.OpenSlots, job.CreatedAt, job.UpdatedAt, job.MinimumEducationLevelId, Name, Description
    from dbo.JobPosition as job 
    inner join dbo.EducationLevel as education
    on job.MinimumEducationLevelId = education.Id
	where job.IsDeleted = 0 and job.MinimumEducationLevelId = 'C8E643B8-1CBB-48EC-8AF9-F9622FDF3792'

	select * from dbo.JobPosition;






CREATE OR ALTER PROCEDURE dbo.spEducationLevel_SoftDelete
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM dbo.JobPosition 
        WHERE MinimumEducationLevelId = @Id 
          AND IsDeleted = 0
    )
    BEGIN
        SELECT -1;
        RETURN;
    END

    UPDATE dbo.EducationLevel SET IsDeleted = 1
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END