

/****** Object:  StoredProcedure [dbo].[spLeaveRequest_SelectByFilter] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spLeaveRequest_SelectByFilter]
    @EmployeeId UNIQUEIDENTIFIER = NULL,
    @Status NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeId, LeaveType, FromDate, ToDate, Reason, Status, ApprovedBy, ApprovedAt, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.LeaveRequest
    WHERE IsDeleted = 0
        AND (@EmployeeId IS NULL OR EmployeeId = @EmployeeId)
        AND (@Status IS NULL OR Status = @Status)
    ORDER BY FromDate DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[spLeaveRequest_Approve] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spLeaveRequest_Approve]
    @Id UNIQUEIDENTIFIER,
    @ApprovedBy UNIQUEIDENTIFIER = NULL,
    @ApprovedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LeaveRequest
    SET Status = 'Approved',
        ApprovedBy = @ApprovedBy,
        ApprovedAt = @ApprovedAt,
        UpdatedAt = @ApprovedAt
    WHERE Id = @Id AND IsDeleted = 0 AND Status = 'Pending';

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO

/****** Object:  StoredProcedure [dbo].[spLeaveRequest_Reject] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spLeaveRequest_Reject]
    @Id UNIQUEIDENTIFIER,
    @ApprovedBy UNIQUEIDENTIFIER = NULL,
    @ApprovedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LeaveRequest
    SET Status = 'Rejected',
        ApprovedBy = @ApprovedBy,
        ApprovedAt = @ApprovedAt,
        UpdatedAt = @ApprovedAt
    WHERE Id = @Id AND IsDeleted = 0 AND Status = 'Pending';

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO
