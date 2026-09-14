

/****** Object:  StoredProcedure [dbo].[spEmployee_Insert] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployee_Insert]
    @Id UNIQUEIDENTIFIER,
    @EmployeeCode NVARCHAR(50),
    @JobApplicationId UNIQUEIDENTIFIER = NULL,
    @JobPositionId UNIQUEIDENTIFIER,
    @PrimaryFacilityId UNIQUEIDENTIFIER,
    @FullName NVARCHAR(200),
    @Email NVARCHAR(150) = NULL,
    @PhoneNumber NVARCHAR(20) = NULL,
    @DateOfBirth DATETIME = NULL,
    @Gender NVARCHAR(20) = NULL,
    @HireDate DATETIME,
    @Status NVARCHAR(20),
    @CreatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Employee
        (Id, EmployeeCode, JobApplicationId, JobPositionId, PrimaryFacilityId, FullName, Email, PhoneNumber, DateOfBirth, Gender, HireDate, Status, IsDeleted, CreatedAt)
    VALUES
        (@Id, @EmployeeCode, @JobApplicationId, @JobPositionId, @PrimaryFacilityId, @FullName, @Email, @PhoneNumber, @DateOfBirth, @Gender, @HireDate, @Status, 0, @CreatedAt);

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spEmployee_Update] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployee_Update]
    @Id UNIQUEIDENTIFIER,
    @EmployeeCode NVARCHAR(50),
    @JobApplicationId UNIQUEIDENTIFIER = NULL,
    @JobPositionId UNIQUEIDENTIFIER,
    @PrimaryFacilityId UNIQUEIDENTIFIER,
    @FullName NVARCHAR(200),
    @Email NVARCHAR(150) = NULL,
    @PhoneNumber NVARCHAR(20) = NULL,
    @DateOfBirth DATETIME = NULL,
    @Gender NVARCHAR(20) = NULL,
    @HireDate DATETIME,
    @Status NVARCHAR(20),
    @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Employee
    SET EmployeeCode = @EmployeeCode,
        JobApplicationId = @JobApplicationId,
        JobPositionId = @JobPositionId,
        PrimaryFacilityId = @PrimaryFacilityId,
        FullName = @FullName,
        Email = @Email,
        PhoneNumber = @PhoneNumber,
        DateOfBirth = @DateOfBirth,
        Gender = @Gender,
        HireDate = @HireDate,
        Status = @Status,
        UpdatedAt = @UpdatedAt
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO

/****** Object:  StoredProcedure [dbo].[spEmployee_SoftDelete] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployee_SoftDelete]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Employee WHERE Id = @Id AND IsDeleted = 0)
    BEGIN
        SELECT 0;   -- không tìm thấy
        RETURN;
    END

    UPDATE dbo.Employee
    SET IsDeleted = 1, UpdatedAt = GETDATE()
    WHERE Id = @Id;

    SELECT 1;
END
GO

/****** Object:  StoredProcedure [dbo].[spEmployee_SelectById] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployee_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeCode, JobApplicationId, JobPositionId, PrimaryFacilityId, FullName, Email, PhoneNumber, DateOfBirth, Gender, HireDate, Status, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.Employee
    WHERE Id = @Id AND IsDeleted = 0;
END
GO

/****** Object:  StoredProcedure [dbo].[spEmployee_SelectAll] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spEmployee_SelectAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, EmployeeCode, JobApplicationId, JobPositionId, PrimaryFacilityId, FullName, Email, PhoneNumber, DateOfBirth, Gender, HireDate, Status, IsDeleted, CreatedAt, UpdatedAt
    FROM dbo.Employee
    WHERE IsDeleted = 0
    ORDER BY FullName ASC;
END
GO
