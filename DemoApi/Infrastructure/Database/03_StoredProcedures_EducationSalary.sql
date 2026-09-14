ALTER PROCEDURE [dbo].[spEmployee_Insert]
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

ALTER PROCEDURE [dbo].[spEmployee_SelectAll]
    @FacilityId UNIQUEIDENTIFIER,
    @JobPositionId UNIQUEIDENTIFIER,
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT emp.Id, emp.EmployeeCode, emp.FullName, emp.Email, emp.PhoneNumber, emp.Status ,faci.Id, faci.Name, job.Id, job.Title
    FROM dbo.Employee as emp
	inner join dbo.Facility as faci on emp.PrimaryFacilityId = faci.Id
	inner join dbo.JobPosition as job on emp.JobPositionId = job.Id
    WHERE emp.IsDeleted = 0 and faci.IsDeleted = 0 and job.IsDeleted = 0
	      AND (@FacilityId is null or emp.PrimaryFacilityId = @FacilityId)
		  AND (@JobPositionId is null or emp.JobPositionId = @JobPositionId)
		  AND (@Status is null or emp.Status = @Status)
    ORDER BY FullName ASC;
END

ALTER PROCEDURE [dbo].[spEmployee_SelectById]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT emp.Id, emp.EmployeeCode, emp.JobApplicationId, emp.FullName, emp.Email, emp.PhoneNumber, emp.DateOfBirth, emp.Gender, emp.HireDate, emp.Status,
	       job.Id, job.Title,
		   facility.Id, facility.Name
    FROM dbo.Employee as emp 
	inner join dbo.JobPosition as job on emp.JobPositionId = job.Id
	inner join dbo.Facility as facility on emp.PrimaryFacilityId = facility.Id
    WHERE emp.Id = @Id AND emp.IsDeleted = 0;
END

ALTER PROCEDURE [dbo].[spEmployee_SoftDelete]
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

ALTER PROCEDURE [dbo].[spEmployee_Update]
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
