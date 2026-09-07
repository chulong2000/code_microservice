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
    @Order INT, @ParentId UNIQUEIDENTIFIER = NULL, @CreatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Guard chống race condition: 2 request tạo cùng tên gần như đồng thời.
    IF EXISTS (SELECT 1 FROM dbo.EducationLevel WHERE Name = @Name AND IsDeleted = 0)
    BEGIN
        SELECT -1;
        RETURN;
    END

    IF @ParentId IS NOT NULL AND NOT EXISTS (
        SELECT 1 FROM dbo.EducationLevel WHERE Id = @ParentId AND IsDeleted = 0)
    BEGIN
        SELECT -2;
        RETURN;
    END

    INSERT INTO dbo.EducationLevel (Id, Name, Description, [Order], ParentId, IsDeleted, CreatedAt)
    VALUES (@Id, @Name, @Description, @Order, @ParentId, 0, @CreatedAt);

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
    @Keyword        NVARCHAR(100) = NULL,
    @SortColumn     NVARCHAR(50)  = NULL,   -- Name | Order | CreatedAt. Giá trị khác/NULL -> sắp xếp mặc định (Order, Name).
    @SortDescending BIT           = 0,
    @PageIndex      INT           = 1,
    @PageSize       INT           = 20
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageIndex < 1 SET @PageIndex = 1;
    IF @PageSize  < 1 SET @PageSize  = 20;

    -- Result set 1: tổng số bản ghi thoả điều kiện lọc, dùng để tính TotalPages ở tầng ứng dụng.
    SELECT COUNT(1)
    FROM dbo.EducationLevel
    WHERE IsDeleted = 0
      AND (@Keyword IS NULL OR Name LIKE '%' + @Keyword + '%');

    -- Result set 2: dữ liệu của trang hiện tại.
    -- Sắp xếp qua CASE WHEN (không dùng dynamic SQL) để @SortColumn không thể gây SQL injection.
    -- Self-join lấy ParentName để FE hiển thị trực tiếp, không phải tự tra cứu theo ParentId.
    SELECT e.Id, e.Name, e.Description, e.[Order], e.IsDeleted, e.CreatedAt, e.UpdatedAt,
           e.ParentId, p.Name AS ParentName
    FROM dbo.EducationLevel e
    LEFT JOIN dbo.EducationLevel p ON p.Id = e.ParentId AND p.IsDeleted = 0
    WHERE e.IsDeleted = 0
      AND (@Keyword IS NULL OR e.Name LIKE '%' + @Keyword + '%')
    ORDER BY
        CASE WHEN @SortColumn = 'Name'      AND @SortDescending = 0 THEN e.Name END ASC,
        CASE WHEN @SortColumn = 'Name'      AND @SortDescending = 1 THEN e.Name END DESC,
        CASE WHEN @SortColumn = 'CreatedAt' AND @SortDescending = 0 THEN e.CreatedAt END ASC,
        CASE WHEN @SortColumn = 'CreatedAt' AND @SortDescending = 1 THEN e.CreatedAt END DESC,
        CASE WHEN (@SortColumn = 'Order' OR @SortColumn IS NULL) AND @SortDescending = 0 THEN e.[Order] END ASC,
        CASE WHEN (@SortColumn = 'Order' OR @SortColumn IS NULL) AND @SortDescending = 1 THEN e.[Order] END DESC,
        e.Name ASC
    OFFSET (@PageIndex - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END

CREATE OR ALTER PROCEDURE [dbo].[spEducationLevel_SelectTree]
AS
BEGIN
    SET NOCOUNT ON;

    -- Trả về flat list toàn bộ node chưa xoá; dựng cây lồng nhau (Children) thực hiện ở tầng Service.
    SELECT Id, Name, Description, [Order], IsDeleted, CreatedAt, UpdatedAt, ParentId
    FROM dbo.EducationLevel
    WHERE IsDeleted = 0
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
    @Order INT, @ParentId UNIQUEIDENTIFIER = NULL, @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.EducationLevel WHERE Name = @Name AND IsDeleted = 0 AND Id <> @Id)
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

        IF NOT EXISTS (SELECT 1 FROM dbo.EducationLevel WHERE Id = @ParentId AND IsDeleted = 0)
        BEGIN
            SELECT -2;   -- danh mục cha không tồn tại
            RETURN;
        END

        -- Chặn vòng lặp: @ParentId không được là hậu duệ của @Id (duyệt xuống từ @Id để tìm toàn bộ con cháu).
        ;WITH Descendants AS (
            SELECT Id FROM dbo.EducationLevel WHERE ParentId = @Id AND IsDeleted = 0
            UNION ALL
            SELECT e.Id
            FROM dbo.EducationLevel e
            INNER JOIN Descendants d ON e.ParentId = d.Id
            WHERE e.IsDeleted = 0
        )
        IF EXISTS (SELECT 1 FROM Descendants WHERE Id = @ParentId)
        BEGIN
            SELECT -4;   -- gây vòng lặp (chọn con/cháu làm cha)
            RETURN;
        END
    END

    UPDATE dbo.EducationLevel
    SET Name = @Name, Description = @Description, [Order] = @Order, ParentId = @ParentId, UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END

