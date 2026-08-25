ALTER   PROCEDURE [dbo].[spEducationLevel_ExistsName]
    @Name      NVARCHAR(100),
    @ExcludeId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM dbo.EducationLevel
        WHERE Name = @Name AND IsDeleted = 0
          AND (@ExcludeId IS NULL OR Id <> @ExcludeId)
    ) THEN 1 ELSE 0 END;
END

ALTER   PROCEDURE [dbo].[spEducationLevel_Insert]
    @Id UNIQUEIDENTIFIER, @Name NVARCHAR(100), @Description NVARCHAR(500) = NULL,
    @Order INT, @CreatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Guard chống race condition: 2 request tạo cùng tên gần như đồng thời.
    IF EXISTS (SELECT 1 FROM dbo.EducationLevel WHERE Name = @Name AND IsDeleted = 0)
    BEGIN
        SELECT -1;
        RETURN;
    END

    INSERT INTO dbo.EducationLevel (Id, Name, Description, [Order], IsDeleted, CreatedAt)
    VALUES (@Id, @Name, @Description, @Order, 0, @CreatedAt);

    SELECT 1;
END

ALTER   PROCEDURE [dbo].[spEducationLevel_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT edu.Id, edu.Name, edu.Description, edu.[Order], edu.CreatedAt, edu.UpdatedAt, 
	       sa.Id , sa.AllowancePercentage, sa.BaseCoefficient, sa.EffectiveFrom,
		   job.Id, job.Title, job.Department, job.OpenSlots,
		   app.Id, app.FullName, app.Email, app.CvFileUrl, app.AppliedAt
    FROM dbo.EducationLevel as edu
	inner join dbo.EducationLevelSalaryCoefficient as sa
	on edu.Id = sa.EducationLevelId
	inner join dbo.JobPosition as job
	on edu.Id = job.MinimumEducationLevelId
	left join  dbo.JobApplication as app
	on job.Id = app.JobPositionId
    WHERE edu.Id = @Id AND edu.IsDeleted = 0;
END

ALTER   PROCEDURE [dbo].[spEducationLevel_SelectList]
    @Keyword NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Description, [Order], IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.EducationLevel
    WHERE IsDeleted = 0
      AND (@Keyword IS NULL OR Name LIKE '%' + @Keyword + '%')
    ORDER BY [Order], Name;
END

ALTER   PROCEDURE [dbo].[spEducationLevel_SoftDelete]
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

ALTER   PROCEDURE [dbo].[spEducationLevel_Update]
    @Id UNIQUEIDENTIFIER, @Name NVARCHAR(100), @Description NVARCHAR(500) = NULL,
    @Order INT, @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.EducationLevel WHERE Name = @Name AND IsDeleted = 0 AND Id <> @Id)
    BEGIN
        SELECT -1;
        RETURN;
    END

    UPDATE dbo.EducationLevel
    SET Name = @Name, Description = @Description, [Order] = @Order, UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END

