

/****** Object:  StoredProcedure [dbo].[spShift_Insert] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spShift_Insert]
    @Id UNIQUEIDENTIFIER, @FacilityId UNIQUEIDENTIFIER = NULL, @Name NVARCHAR(100), @StartTime TIME(0), @EndTime TIME(0), @CreatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Shift (Id, FacilityId, Name, StartTime, EndTime, IsDeleted, CreatedAt)
    VALUES (@Id, @FacilityId, @Name, @StartTime, @EndTime, 0, @CreatedAt);

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spShift_Update] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spShift_Update]
    @Id UNIQUEIDENTIFIER, @FacilityId UNIQUEIDENTIFIER = NULL, @Name NVARCHAR(100), @StartTime TIME(0), @EndTime TIME(0), @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Shift
    SET FacilityId = @FacilityId, Name = @Name, StartTime = @StartTime, EndTime = @EndTime, UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO

/****** Object:  StoredProcedure [dbo].[spShift_SoftDelete] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spShift_SoftDelete]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Shift WHERE Id = @Id AND IsDeleted = 0)
    BEGIN
        SELECT 0;   -- không tìm thấy
        RETURN;
    END

    UPDATE dbo.Shift
    SET IsDeleted = 1, UpdatedAt = GETDATE()
    WHERE Id = @Id;

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spShift_SelectById] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spShift_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, FacilityId, Name, StartTime, EndTime, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.Shift
    WHERE Id = @Id AND IsDeleted = 0;
END
GO

/****** Object:  StoredProcedure [dbo].[spShift_SelectAll] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spShift_SelectAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, FacilityId, Name, StartTime, EndTime, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.Shift
    WHERE IsDeleted = 0
    ORDER BY Name ASC;
END
GO
