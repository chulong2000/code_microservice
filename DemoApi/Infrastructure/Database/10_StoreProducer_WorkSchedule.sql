ALTER PROCEDURE [dbo].[spWorkSchedule_BulkSoftDelete]
    @Ids [dbo].[GuidListType] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ws
    SET IsDeleted = 1, UpdatedAt = GETDATE()
    FROM dbo.WorkSchedule ws
    JOIN @Ids i ON i.Id = ws.Id
    WHERE ws.IsDeleted = 0;

    SELECT @@ROWCOUNT;
END

ALTER PROCEDURE [dbo].[spWorkSchedule_Insert]
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

ALTER PROCEDURE [dbo].[spWorkSchedule_SelectAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeId, ShiftId, FacilityId, WorkDate, Status, Note, CreatedBy, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.WorkSchedule
    WHERE IsDeleted = 0
    ORDER BY WorkDate ASC;
END

ALTER PROCEDURE [dbo].[spWorkSchedule_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeId, ShiftId, FacilityId, WorkDate, Status, Note, CreatedBy, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.WorkSchedule
    WHERE Id = @Id AND IsDeleted = 0;
END

ALTER PROCEDURE [dbo].[spWorkSchedule_SelectDraftInScope]
    @FacilityId UNIQUEIDENTIFIER,
    @FromDate DATETIME,
    @ToDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeId, ShiftId, FacilityId, WorkDate, Status, Note, CreatedBy, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.WorkSchedule
    WHERE IsDeleted = 0
        AND FacilityId = @FacilityId
        AND Status = 'Draft'
        AND CAST(WorkDate AS DATE) BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE);
END

ALTER PROCEDURE [dbo].[spWorkSchedule_SoftDelete]
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

ALTER PROCEDURE [dbo].[spWorkSchedule_Update]
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

ALTER   PROC [dbo].[WorkSchedule_bulkInsert]
@workSchedules typWorkSchedule ReadOnly
AS
BEGIN
      SET NOCOUNT ON;
      INSERT INTO dbo.WorkSchedule(Id, EmployeeId, ShiftId, FacilityId, WorkDate, Status, Note, CreatedBy, CreatedAt, UpdatedAt, PublishedAt, PublishedBy, confirmedAt, confirmedBy, IsDeleted)
      SELECT * FROM @workSchedules;

	  Select @@ROWCOUNT;
END