

/****** Object:  StoredProcedure [dbo].[spLeaveRequest_Insert] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spLeaveRequest_Insert]
    @Id UNIQUEIDENTIFIER,
    @EmployeeId UNIQUEIDENTIFIER,
    @LeaveType NVARCHAR(30),
    @FromDate DATETIME,
    @ToDate DATETIME,
    @Reason NVARCHAR(300) = NULL,
    @Status NVARCHAR(20),
    @ApprovedBy UNIQUEIDENTIFIER = NULL,
    @ApprovedAt DATETIME = NULL,
    @CreatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeaveRequest
        (Id, EmployeeId, LeaveType, FromDate, ToDate, Reason, Status, ApprovedBy, ApprovedAt, IsDeleted, CreatedAt)
    VALUES
        (@Id, @EmployeeId, @LeaveType, @FromDate, @ToDate, @Reason, @Status, @ApprovedBy, @ApprovedAt, 0, @CreatedAt);

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spLeaveRequest_Update] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spLeaveRequest_Update]
    @Id UNIQUEIDENTIFIER,
    @EmployeeId UNIQUEIDENTIFIER,
    @LeaveType NVARCHAR(30),
    @FromDate DATETIME,
    @ToDate DATETIME,
    @Reason NVARCHAR(300) = NULL,
    @Status NVARCHAR(20),
    @ApprovedBy UNIQUEIDENTIFIER = NULL,
    @ApprovedAt DATETIME = NULL,
    @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LeaveRequest
    SET EmployeeId = @EmployeeId,
        LeaveType = @LeaveType,
        FromDate = @FromDate,
        ToDate = @ToDate,
        Reason = @Reason,
        Status = @Status,
        ApprovedBy = @ApprovedBy,
        ApprovedAt = @ApprovedAt,
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO

/****** Object:  StoredProcedure [dbo].[spLeaveRequest_SoftDelete] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spLeaveRequest_SoftDelete]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.LeaveRequest WHERE Id = @Id AND IsDeleted = 0)
    BEGIN
        SELECT 0;   -- không tìm thấy
        RETURN;
    END

    UPDATE dbo.LeaveRequest
    SET IsDeleted = 1, UpdatedAt = GETDATE()
    WHERE Id = @Id;

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spLeaveRequest_SelectById] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spLeaveRequest_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeId, LeaveType, FromDate, ToDate, Reason, Status, ApprovedBy, ApprovedAt, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.LeaveRequest
    WHERE Id = @Id AND IsDeleted = 0;
END
GO

/****** Object:  StoredProcedure [dbo].[spLeaveRequest_SelectAll] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spLeaveRequest_SelectAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeId, LeaveType, FromDate, ToDate, Reason, Status, ApprovedBy, ApprovedAt, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.LeaveRequest
    WHERE IsDeleted = 0
    ORDER BY FromDate DESC;
END
GO
