USE [DemoEducationLevelDb]
GO
/****** Object:  StoredProcedure [dbo].[spJobPosition_ExistsName]    Script Date: 09/09/2026 9:56:30 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create     PROCEDURE [dbo].[spJobPosition_ExistsName]
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
/****** Object:  StoredProcedure [dbo].[spJobPosition_Insert]    Script Date: 09/09/2026 9:56:30 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[spJobPosition_Insert]
    @Id UNIQUEIDENTIFIER, @Title NVARCHAR(100), @Department NVARCHAR(500),
    @OpenSlots INT, @EducationLevelId UNIQUEIDENTIFIER, @IsOpen bit, @CreatedAt datetime, @IsDeleted bit, @ParentId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Guard ch?ng race condition: 2 request t?o cùng tên g?n nh? ??ng th?i.
    IF EXISTS (SELECT 1 FROM dbo.JobPosition WHERE Title = @Title AND IsDeleted = 0)
    BEGIN
        SELECT -1;
        RETURN;
    END

	IF @ParentId IS NOT NULL AND NOT EXISTS (
        SELECT 1 FROM dbo.JobPosition WHERE Id = @ParentId AND IsDeleted = 0)
    BEGIN
        SELECT -2; RETURN;
    END

    INSERT INTO dbo.JobPosition (Id, ParentId,Title, Department, OpenSlots, MinimumEducationLevelId, IsOpen, CreatedAt, IsDeleted)
    VALUES (@Id, @ParentId,@Title, @Department , @OpenSlots, @EducationLevelId, @IsOpen, @CreatedAt, @IsDeleted);

    SELECT 1;
END
GO
/****** Object:  StoredProcedure [dbo].[spJobPosition_SelectById]    Script Date: 09/09/2026 9:56:30 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[spJobPosition_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select job.Id, job.Title, job.Department,job.OpenSlots, job.IsOpen, job.CreatedAt, job.UpdatedAt, education.Id, education.Name 
    from dbo.JobPosition as job 
    inner join dbo.EducationLevel as education
	on job.MinimumEducationLevelId = education.Id
    WHERE job.Id = @Id AND job.IsDeleted = 0;
END
GO
/****** Object:  StoredProcedure [dbo].[spJobPosition_SelectList]    Script Date: 09/09/2026 9:56:30 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[spJobPosition_SelectList]
    @Keyword NVARCHAR(100) = NULL,
    @EducationId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    select job.Id, job.Title, job.Department,job.OpenSlots, job.IsOpen,job.IsDeleted, job.CreatedAt, job.UpdatedAt, job.MinimumEducationLevelId as Id, Name, Description
    from dbo.JobPosition as job 
    inner join dbo.EducationLevel as education
    on job.MinimumEducationLevelId = education.Id
	where job.IsDeleted = 0 and education.IsDeleted = 0 and (@EducationId IS NULL OR  job.MinimumEducationLevelId = @EducationId)
	AND (@Keyword IS NULL OR Title LIKE '%' + @Keyword + '%')
END
GO
/****** Object:  StoredProcedure [dbo].[spJobPosition_SelectListJobPostionByEducationLevelId]    Script Date: 09/09/2026 9:56:30 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create     PROCEDURE [dbo].[spJobPosition_SelectListJobPostionByEducationLevelId]
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

GO
/****** Object:  StoredProcedure [dbo].[spJobPosition_SelectTree]    Script Date: 09/09/2026 9:56:30 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create   PROCEDURE [dbo].[spJobPosition_SelectTree]
AS
BEGIN
    SET NOCOUNT ON;

    -- Trả về flat list toàn bộ node chưa xoá; dựng cây lồng nhau (Children) thực hiện ở tầng Service.
    SELECT job.Id, job.Title, job.Department, job.OpenSlots, job.ParentId, job.IsOpen, job.CreatedAt, edu.Id, edu.Name 
    FROM dbo.JobPosition as job 
	inner join dbo.EducationLevel as edu on job.MinimumEducationLevelId = edu.Id
    WHERE job.IsDeleted = 0
    ORDER BY job.Title;
END
GO
/****** Object:  StoredProcedure [dbo].[spJobPosition_SoftDelete]    Script Date: 09/09/2026 9:56:30 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[spJobPosition_SoftDelete]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM dbo.JobPosition WHERE Id = @Id AND IsDeleted = 0)
    BEGIN
        SELECT 0;   -- không tìm thấy
        RETURN;
    END

	DECLARE @Branch TABLE (Id UNIQUEIDENTIFIER PRIMARY KEY);

    ;WITH cte AS (
        SELECT Id FROM dbo.JobPosition WHERE Id = @Id AND IsDeleted = 0
        UNION ALL
        SELECT e.Id
        FROM dbo.JobPosition e
        INNER JOIN cte d ON e.ParentId = d.Id
        WHERE e.IsDeleted = 0
    )
    INSERT INTO @Branch (Id)
    SELECT Id FROM cte;

    -- Chặn cascade nếu BẤT KỲ node nào trong cả nhánh (node gốc hoặc con/cháu) còn JobApplication đang tham chiếu.
    IF EXISTS (
        SELECT 1
        FROM dbo.JobApplication app
        INNER JOIN @Branch b ON app.JobPositionId = b.Id
        WHERE app.IsDeleted = 0
    )
    BEGIN
        SELECT -1;
        RETURN;
    END

    UPDATE dbo.JobPosition
    SET IsDeleted = 1, UpdatedAt = GETDATE()
    WHERE Id IN (SELECT Id FROM @Branch);

    SELECT 1;
END
GO
/****** Object:  StoredProcedure [dbo].[spJobPosition_Update]    Script Date: 09/09/2026 9:56:30 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[spJobPosition_Update]
    @Id UNIQUEIDENTIFIER, @Title NVARCHAR(100), @Department NVARCHAR(500),
    @OpenSlots INT, @EducationLevelId UNIQUEIDENTIFIER, @IsOpen bit,@UpdatedAt DATETIME, @ParentId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.JobPosition WHERE Title = @Title AND IsDeleted = 0 AND Id <> @Id)
    BEGIN
        SELECT -1;
        RETURN;
    END

	IF @ParentId IS NOT NULL
    BEGIN
        IF @ParentId = @Id
        BEGIN
            SELECT -3;   -- không thể tự làm cha của chính mình
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.JobPosition WHERE Id = @ParentId AND IsDeleted = 0)
        BEGIN
            SELECT -2;   -- danh mục cha không tồn tại
            RETURN;
        END

		DECLARE @IsDescendant BIT = 0;   

        -- Chặn vòng lặp: @ParentId không được là hậu duệ của @Id (duyệt xuống từ @Id để tìm toàn bộ con cháu).
        ;WITH Descendants AS (
            SELECT Id FROM dbo.JobPosition WHERE ParentId = @Id AND IsDeleted = 0
            UNION ALL
            SELECT e.Id
            FROM dbo.JobPosition e
            INNER JOIN Descendants d ON e.ParentId = d.Id
            WHERE e.IsDeleted = 0
        )

		SELECT @IsDescendant = 1
               FROM Descendants
               WHERE Id = @ParentId;

         IF @IsDescendant = 1
            BEGIN
              SELECT -4;   -- gây vòng lặp (chọn con/cháu làm cha)
            RETURN;
        END
    END

    UPDATE dbo.JobPosition
    SET Title = @Title, ParentId = @ParentId, Department = @Department, OpenSlots = @OpenSlots, MinimumEducationLevelId = @EducationLevelId,
    IsOpen = @IsOpen, UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO
