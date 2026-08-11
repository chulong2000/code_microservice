CREATE TABLE dbo.EducationLevel
(
    Id          UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name        NVARCHAR(100)    NOT NULL,
    Description NVARCHAR(500)    NULL,
    [Order]     INT              NOT NULL DEFAULT (0),
    IsDeleted   BIT              NOT NULL DEFAULT (0),
    CreatedAt   DATETIME         NOT NULL,
    UpdatedAt   DATETIME         NULL
);
GO

-- Không có tên trùng nhau trong các bản ghi chưa xoá.
CREATE UNIQUE INDEX UX_EducationLevel_Name
    ON dbo.EducationLevel(Name)
    WHERE IsDeleted = 0;
GO