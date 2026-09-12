

/****** Object:  User-Defined Table Type [dbo].[WorkScheduleTableType] ******/
CREATE TYPE [dbo].[WorkScheduleTableType] AS TABLE
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL,
    [ShiftId] UNIQUEIDENTIFIER NOT NULL,
    [FacilityId] UNIQUEIDENTIFIER NOT NULL,
    [WorkDate] DATETIME NOT NULL,
    [Status] NVARCHAR(20) NOT NULL,
    [Note] NVARCHAR(300) NULL,
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [CreatedAt] DATETIME NOT NULL
);
GO

/****** Object:  StoredProcedure [dbo].[spWorkSchedule_BulkInsert] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spWorkSchedule_BulkInsert]
    @Items [dbo].[WorkScheduleTableType] READONLY
AS
BEGIN
    SET NOCOUNT ON;

    -- Bỏ qua các cặp (EmployeeId, WorkDate) đã có lịch làm việc, tránh tạo trùng
    INSERT INTO dbo.WorkSchedule
        (Id, EmployeeId, ShiftId, FacilityId, WorkDate, Status, Note, CreatedBy, IsDeleted, CreatedAt)
    SELECT
        i.Id, i.EmployeeId, i.ShiftId, i.FacilityId, i.WorkDate, i.Status, i.Note, i.CreatedBy, 0, i.CreatedAt
    FROM @Items i
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.WorkSchedule ws
        WHERE ws.EmployeeId = i.EmployeeId
          AND ws.WorkDate = i.WorkDate
          AND ws.IsDeleted = 0
    );

    SELECT @@ROWCOUNT;
END
GO
