ALTER     PROCEDURE [dbo].[spEducationLevelSalaryCoefficient_GetSalaryCoefficientOfEducationLevel]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    select sa.Id, sa.BaseCoefficient, sa.AllowancePercentage, sa.EffectiveFrom, edu.Id as Id, edu.Name from dbo.EducationLevelSalaryCoefficient as sa 
	inner join dbo.EducationLevel as edu
	on sa.EducationLevelId = edu.Id
	where edu.Id = @Id
END

GO 

ALTER     PROCEDURE [dbo].[spEducationLevelSalaryCoefficient_SelectList]
    
AS
BEGIN
    SET NOCOUNT ON;
    select sa.Id, sa.EffectiveFrom, sa.BaseCoefficient, sa.AllowancePercentage, sa.Notes, edu.Id as Id, edu.Name from dbo.EducationLevelSalaryCoefficient as sa 
	inner join dbo.EducationLevel as edu
	on sa.EducationLevelId = edu.Id
END

GO

ALTER     PROCEDURE [dbo].[spEducationLevelSalaryCoefficient_SoftDelete]
    @Id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.EducationLevelSalaryCoefficient SET IsDeleted = 1
    WHERE Id = @Id AND IsDeleted = 0;

    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END

GO

ALTER     PROCEDURE [dbo].[spEducationLevelSalaryCoefficient_Upsert]
    @Id UNIQUEIDENTIFIER, @EducationLevelId UNIQUEIDENTIFIER, @BaseCoefficient DECIMAL(5,2), @AllowancePercentage DECIMAL(5,2),@EffectiveFrom DATETIME,
    @Notes nvarchar(500), @CreatedAt DATETIME, @UpdatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 from dbo.EducationLevel as edu 
	           inner join dbo.EducationLevelSalaryCoefficient as salary 
			   on edu.Id = salary.EducationLevelId where edu.Id = @EducationLevelId)
    BEGIN
        UPDATE dbo.EducationLevelSalaryCoefficient
        SET BaseCoefficient = @BaseCoefficient, AllowancePercentage = @AllowancePercentage, EffectiveFrom = @EffectiveFrom, Notes = @Notes,
        UpdatedAt = @UpdatedAt
        WHERE EducationLevelId = @EducationLevelId AND IsDeleted = 0;
    END
	ELSE
	BEGIN
        INSERT INTO dbo.EducationLevelSalaryCoefficient(Id,EducationLevelId,BaseCoefficient, AllowancePercentage, EffectiveFrom, Notes, CreatedAt, IsDeleted)
        VALUES (@Id, @EducationLevelId, @BaseCoefficient , @AllowancePercentage, @EffectiveFrom, @Notes, @CreatedAt, 0);
	END
    SELECT CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
