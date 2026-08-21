USE [DemoEducationLevelDb]
GO
/****** Object:  Table [dbo].[EducationLevel]    Script Date: 8/21/2026 11:11:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationLevel](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Order] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationLevelSalaryCoefficient]    Script Date: 8/21/2026 11:11:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationLevelSalaryCoefficient](
	[Id] [uniqueidentifier] NOT NULL,
	[EducationLevelId] [uniqueidentifier] NOT NULL,
	[BaseCoefficient] [decimal](5, 2) NULL,
	[AllowancePercentage] [decimal](5, 2) NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[Notes] [nvarchar](500) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobApplication]    Script Date: 8/21/2026 11:11:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobApplication](
	[Id] [uniqueidentifier] NOT NULL,
	[JobPositionId] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](200) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[PhoneNumber] [varchar](100) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Gender] [varchar](120) NOT NULL,
	[CvFileUrl] [varchar](200) NOT NULL,
	[CoverLetter] [nvarchar](300) NULL,
	[YearsOfExperience] [int] NOT NULL,
	[AppliedAt] [datetime] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobPosition]    Script Date: 8/21/2026 11:11:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobPosition](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](150) NOT NULL,
	[Department] [nvarchar](150) NOT NULL,
	[OpenSlots] [int] NOT NULL,
	[MinimumEducationLevelId] [uniqueidentifier] NOT NULL,
	[IsOpen] [bit] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'aab37877-e299-4c7e-b49c-2aa8897adca9', N'cử nhân', N'string', 0, 0, CAST(N'2026-08-12T09:22:36.740' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', N'đại học', N'string', 0, 0, CAST(N'2026-08-12T08:46:30.687' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'9d1ddae4-8f88-4684-a194-945a1faf485e', N'giáo sư', N'string', 0, 0, CAST(N'2026-08-12T09:22:46.217' AS DateTime), NULL)
GO
INSERT [dbo].[EducationLevelSalaryCoefficient] ([Id], [EducationLevelId], [BaseCoefficient], [AllowancePercentage], [EffectiveFrom], [Notes], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'0a51a540-dc8b-4514-8cf1-b94526f1ff09', N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', CAST(3.45 AS Decimal(5, 2)), CAST(0.08 AS Decimal(5, 2)), CAST(N'2026-08-18T07:34:01.087' AS DateTime), N'test_abcc', CAST(N'2026-08-18T14:34:29.663' AS DateTime), NULL, 0)
GO
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'044084fd-8682-4014-be22-11f0dfa06893', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Khánh Tùng', N'tungkh@gmail.com', N'097122222', CAST(N'2026-08-18T16:53:46.910' AS DateTime), N'Male', N'abc.url.com', N'string', 5, CAST(N'2026-08-18T09:53:46.910' AS DateTime), CAST(N'2026-08-18T09:53:46.910' AS DateTime), NULL, 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'b39fb7b0-b53d-4885-8179-07166d0b2ceb', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Nguy?n Cư?ng', N'tungkh@gmail.com', N'097122222', CAST(N'2026-08-18T16:54:04.773' AS DateTime), N'Male', N'abc.url.com', N'string', 5, CAST(N'2026-08-18T09:54:04.770' AS DateTime), CAST(N'2026-08-18T09:54:04.770' AS DateTime), NULL, 1)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'483e9b92-bdb0-4957-a09b-77e140eea910', N'cdc797e1-f10e-41aa-ac99-740a7f2b0e73', N'Phan Huy', N'huynk@gmail.com', N'9292822', CAST(N'2026-09-12T17:15:00.000' AS DateTime), N'Male', N'abc.url.com', NULL, 6, CAST(N'2026-08-20T10:05:00.000' AS DateTime), CAST(N'2026-08-20T10:05:00.000' AS DateTime), CAST(N'2026-08-20T10:05:00.000' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'839e6cd3-82a9-4c58-b68b-13c7080521c2', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'khanh pham', N'', N'string', CAST(N'2026-08-21T02:01:25.517' AS DateTime), N'string', N'string', N'string', 5, CAST(N'2026-08-21T09:02:00.143' AS DateTime), CAST(N'2026-08-21T09:02:00.143' AS DateTime), NULL, 0)
GO
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Bác sĩ', N'Phòng khám chuyên khoa', 5, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-08-15T08:37:27.440' AS DateTime), NULL, 0)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'cdc797e1-f10e-41aa-ac99-740a7f2b0e73', N'kỹ sư xây dựng', N'Khoa xây dựng', 5, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-08-18T07:33:07.647' AS DateTime), NULL, 0)
GO
ALTER TABLE [dbo].[EducationLevel] ADD  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [dbo].[EducationLevel] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[EducationLevelSalaryCoefficient]  WITH CHECK ADD  CONSTRAINT [fk_Education_Level] FOREIGN KEY([EducationLevelId])
REFERENCES [dbo].[EducationLevel] ([Id])
GO
ALTER TABLE [dbo].[EducationLevelSalaryCoefficient] CHECK CONSTRAINT [fk_Education_Level]
GO
ALTER TABLE [dbo].[JobPosition]  WITH CHECK ADD  CONSTRAINT [fk_Job_Position] FOREIGN KEY([MinimumEducationLevelId])
REFERENCES [dbo].[EducationLevel] ([Id])
GO
ALTER TABLE [dbo].[JobPosition] CHECK CONSTRAINT [fk_Job_Position]
GO
