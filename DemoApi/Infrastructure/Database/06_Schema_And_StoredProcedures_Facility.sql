USE [DemoEducationLevelDb]
GO
/****** Object:  Table [dbo].[Facility] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Facility](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](150) NOT NULL,
	[Address] [nvarchar](300) NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Facility] ADD DEFAULT ((0)) FOR [IsDeleted]
GO

/****** Object:  StoredProcedure [dbo].[spFacility_ExistsName] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spFacility_ExistsName]
    @Name      NVARCHAR(150),
    @ExcludeId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM dbo.Facility
        WHERE Name = @Name AND IsDeleted = 0
          AND (@ExcludeId IS NULL OR Id <> @ExcludeId)
    ) THEN 1 ELSE 0 END;
END
GO

/****** Object:  StoredProcedure [dbo].[spFacility_Insert] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spFacility_Insert]
    @Id UNIQUEIDENTIFIER, @Name NVARCHAR(150), @Address NVARCHAR(300) = NULL, @CreatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Guard chống race condition: 2 request tạo cùng tên gần như đồng thời.
    IF EXISTS (SELECT 1 FROM dbo.Facility WHERE Name = @Name AND IsDeleted = 0)
    BEGIN
        SELECT -1;
        RETURN;
    END

    INSERT INTO dbo.Facility (Id, Name, Address, IsDeleted, CreatedAt)
    VALUES (@Id, @Name, @Address, 0, @CreatedAt);

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spFacility_SelectById] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spFacility_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Name, Address, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.Facility
    WHERE Id = @Id AND IsDeleted = 0;
END
GO

/****** Object:  StoredProcedure [dbo].[spFacility_SelectList] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spFacility_SelectList]
    @Keyword        NVARCHAR(150) = NULL,
    @SortColumn     NVARCHAR(50)  = NULL,   -- Name | CreatedAt. Giá trị khác/NULL -> sắp xếp mặc định (Name).
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
    FROM dbo.Facility
    WHERE IsDeleted = 0
      AND (@Keyword IS NULL OR Name LIKE '%' + @Keyword + '%' OR Address LIKE '%' + @Keyword + '%');

    -- Result set 2: dữ liệu của trang hiện tại.
    -- Sắp xếp qua CASE WHEN (không dùng dynamic SQL) để @SortColumn không thể gây SQL injection.
    SELECT Id, Name, Address, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.Facility
    WHERE IsDeleted = 0
      AND (@Keyword IS NULL OR Name LIKE '%' + @Keyword + '%' OR Address LIKE '%' + @Keyword + '%')
    ORDER BY
        CASE WHEN @SortColumn = 'CreatedAt' AND @SortDescending = 0 THEN CreatedAt END ASC,
        CASE WHEN @SortColumn = 'CreatedAt' AND @SortDescending = 1 THEN CreatedAt END DESC,
        CASE WHEN (@SortColumn = 'Name' OR @SortColumn IS NULL) AND @SortDescending = 0 THEN Name END ASC,
        CASE WHEN (@SortColumn = 'Name' OR @SortColumn IS NULL) AND @SortDescending = 1 THEN Name END DESC,
        Name ASC
    OFFSET (@PageIndex - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

/****** Object:  StoredProcedure [dbo].[spFacility_SoftDelete] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spFacility_SoftDelete]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Facility WHERE Id = @Id AND IsDeleted = 0)
    BEGIN
        SELECT 0;   -- không tìm thấy
        RETURN;
    END

    UPDATE dbo.Facility
    SET IsDeleted = 1, UpdatedAt = GETDATE()
    WHERE Id = @Id;

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spFacility_Update] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spFacility_Update]
    @Id UNIQUEIDENTIFIER, @Name NVARCHAR(150), @Address NVARCHAR(300) = NULL, @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM dbo.Facility WHERE Name = @Name AND IsDeleted = 0 AND Id <> @Id)
    BEGIN
        SELECT -1;
        RETURN;
    END

    UPDATE dbo.Facility
    SET Name = @Name, Address = @Address, UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO
