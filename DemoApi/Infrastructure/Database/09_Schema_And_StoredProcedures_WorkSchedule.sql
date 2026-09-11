

/****** Object:  StoredProcedure [dbo].[spWorkSchedule_Insert] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWorkSchedule_Insert]
    @Id UNIQUEIDENTIFIER,
    @EmployeeId UNIQUEIDENTIFIER,
    @ShiftId UNIQUEIDENTIFIER,
    @FacilityId UNIQUEIDENTIFIER,
    @WorkDate DATETIME,
    @Status NVARCHAR(20),
    @Note NVARCHAR(300) = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @CreatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.WorkSchedule
        (Id, EmployeeId, ShiftId, FacilityId, WorkDate, Status, Note, CreatedBy, IsDeleted, CreatedAt)
    VALUES
        (@Id, @EmployeeId, @ShiftId, @FacilityId, @WorkDate, @Status, @Note, @CreatedBy, 0, @CreatedAt);

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spWorkSchedule_Update] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWorkSchedule_Update]
    @Id UNIQUEIDENTIFIER,
    @EmployeeId UNIQUEIDENTIFIER,
    @ShiftId UNIQUEIDENTIFIER,
    @FacilityId UNIQUEIDENTIFIER,
    @WorkDate DATETIME,
    @Status NVARCHAR(20),
    @Note NVARCHAR(300) = NULL,
    @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.WorkSchedule
    SET EmployeeId = @EmployeeId,
        ShiftId = @ShiftId,
        FacilityId = @FacilityId,
        WorkDate = @WorkDate,
        Status = @Status,
        Note = @Note,
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO

/****** Object:  StoredProcedure [dbo].[spWorkSchedule_SoftDelete] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWorkSchedule_SoftDelete]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.WorkSchedule WHERE Id = @Id AND IsDeleted = 0)
    BEGIN
        SELECT 0;   -- không tìm thấy
        RETURN;
    END

    UPDATE dbo.WorkSchedule
    SET IsDeleted = 1, UpdatedAt = GETDATE()
    WHERE Id = @Id;

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spWorkSchedule_SelectById] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWorkSchedule_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeId, ShiftId, FacilityId, WorkDate, Status, Note, CreatedBy, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.WorkSchedule
    WHERE Id = @Id AND IsDeleted = 0;
END
GO

/****** Object:  StoredProcedure [dbo].[spWorkSchedule_SelectAll] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWorkSchedule_SelectAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeId, ShiftId, FacilityId, WorkDate, Status, Note, CreatedBy, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.WorkSchedule
    WHERE IsDeleted = 0
    ORDER BY WorkDate ASC;
END
GO
