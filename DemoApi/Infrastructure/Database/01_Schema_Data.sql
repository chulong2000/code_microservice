USE [DemoEducationLevelDb]
GO
/****** Object:  Table [dbo].[EducationLevel]    Script Date: 27/08/2026 3:45:12 CH ******/
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
/****** Object:  Table [dbo].[EducationLevelSalaryCoefficient]    Script Date: 27/08/2026 3:45:12 CH ******/
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
/****** Object:  Table [dbo].[JobApplication]    Script Date: 27/08/2026 3:45:12 CH ******/
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
/****** Object:  Table [dbo].[JobPosition]    Script Date: 27/08/2026 3:45:12 CH ******/
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
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'bcece7c5-8ae1-410f-935e-05fe6e7c0f46', N'Trung cấp', N'Test_abcccc66', 0, 1, CAST(N'2026-08-26T09:28:14.867' AS DateTime), CAST(N'2026-08-26T09:28:22.400' AS DateTime))
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'aab37877-e299-4c7e-b49c-2aa8897adca9', N'cử nhân', N'string_abccc', 0, 1, CAST(N'2026-08-12T09:22:36.740' AS DateTime), CAST(N'2026-08-25T08:45:12.833' AS DateTime))
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'282b8e23-2ff1-4acb-a08b-36e967047ada', N'Trung cấp', NULL, 0, 1, CAST(N'2026-08-26T09:10:42.133' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'c4b8cfc2-d3cd-4db3-883c-5827d793ab76', N'Trung cấp', N'test_abccc_00000', 5, 1, CAST(N'2026-08-25T08:45:04.023' AS DateTime), CAST(N'2026-08-25T08:45:26.530' AS DateTime))
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', N'đại học', N'string123', 0, 0, CAST(N'2026-08-12T08:46:30.687' AS DateTime), CAST(N'2026-08-26T09:55:13.843' AS DateTime))
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'9d1ddae4-8f88-4684-a194-945a1faf485e', N'giáo sư', N'string', 0, 1, CAST(N'2026-08-12T09:22:46.217' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'335b533b-951b-4486-9584-95b2af501574', N'Cao Đẳng', N'test_abccccc', 0, 0, CAST(N'2026-08-26T09:29:05.850' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'4f5a79b7-2c84-48ba-ae75-965cfd3aac91', N'Cử nhân', N'abcccc', 0, 0, CAST(N'2026-08-26T09:55:08.790' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'34c6e98b-88d8-442f-99bf-b5d0905f14d4', N'string', N'string', 0, 0, CAST(N'2026-08-26T13:18:15.307' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'15a03330-af58-403e-be72-bcf5e09a7f91', N'Trung cấp', N'test_abcccccc', 0, 1, CAST(N'2026-08-26T09:28:45.573' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'4f2f20c2-3713-4acd-b2ac-d4c5722f5888', N'test_abcc', N'string', 0, 1, CAST(N'2026-08-22T11:27:50.087' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt]) VALUES (N'36716d4d-f685-4a91-b389-e60e8d698728', N'Tiến sĩ', N'test_abccc', 0, 1, CAST(N'2026-08-25T11:27:11.307' AS DateTime), NULL)
GO
INSERT [dbo].[EducationLevelSalaryCoefficient] ([Id], [EducationLevelId], [BaseCoefficient], [AllowancePercentage], [EffectiveFrom], [Notes], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'ac97283a-8308-42f8-b054-3b793900762e', N'15a03330-af58-403e-be72-bcf5e09a7f91', CAST(0.24 AS Decimal(5, 2)), CAST(0.12 AS Decimal(5, 2)), CAST(N'2026-08-28T00:00:00.000' AS DateTime), N'abcc', CAST(N'2026-08-26T09:49:44.760' AS DateTime), NULL, 0)
INSERT [dbo].[EducationLevelSalaryCoefficient] ([Id], [EducationLevelId], [BaseCoefficient], [AllowancePercentage], [EffectiveFrom], [Notes], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'c7290a4e-61c5-4765-af9d-4ae1ce2205d9', N'335b533b-951b-4486-9584-95b2af501574', CAST(0.33 AS Decimal(5, 2)), CAST(0.15 AS Decimal(5, 2)), CAST(N'2026-08-26T00:00:00.000' AS DateTime), N'abccc', CAST(N'2026-08-26T09:45:22.650' AS DateTime), NULL, 0)
INSERT [dbo].[EducationLevelSalaryCoefficient] ([Id], [EducationLevelId], [BaseCoefficient], [AllowancePercentage], [EffectiveFrom], [Notes], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'9eca947f-86af-49eb-bd44-6474e4145ee8', N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', CAST(0.24 AS Decimal(5, 2)), CAST(0.25 AS Decimal(5, 2)), CAST(N'2026-08-25T00:00:00.000' AS DateTime), N'test_abccccc', CAST(N'2026-08-25T14:30:23.247' AS DateTime), CAST(N'2026-08-25T15:19:40.807' AS DateTime), 0)
GO
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'044084fd-8682-4014-be22-11f0dfa06893', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Khánh Tùng', N'tungkh@gmail.com', N'097122222', CAST(N'2026-08-18T16:53:46.910' AS DateTime), N'Male', N'abc.url.com', N'string', 5, CAST(N'2026-08-18T09:53:46.910' AS DateTime), CAST(N'2026-08-18T09:53:46.910' AS DateTime), NULL, 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'b39fb7b0-b53d-4885-8179-07166d0b2ceb', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Nguyễn Cường', N'tungkh@gmail.com', N'097122222', CAST(N'2026-08-18T16:54:04.773' AS DateTime), N'Male', N'abc.url.com', N'string', 5, CAST(N'2026-08-18T09:54:04.770' AS DateTime), CAST(N'2026-08-18T09:54:04.770' AS DateTime), NULL, 1)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'483e9b92-bdb0-4957-a09b-77e140eea910', N'cdc797e1-f10e-41aa-ac99-740a7f2b0e73', N'Phan Huy', N'huynk@gmail.com', N'9292822', CAST(N'2026-09-12T17:15:00.000' AS DateTime), N'Male', N'abc.url.com', NULL, 6, CAST(N'2026-08-20T10:05:00.000' AS DateTime), CAST(N'2026-08-20T10:05:00.000' AS DateTime), CAST(N'2026-08-20T10:05:00.000' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'839e6cd3-82a9-4c58-b68b-13c7080521c2', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'khanh pham', N'khanhpham@gmail.com', N'string', CAST(N'2026-08-21T02:01:25.517' AS DateTime), N'string', N'string', N'string', 5, CAST(N'2026-08-21T09:02:00.143' AS DateTime), CAST(N'2026-08-21T09:02:00.143' AS DateTime), NULL, 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'30dafe16-a3b8-437c-a899-2ad293319224', N'30dafe16-a3b8-437c-a899-2ad293319224', N'Khánh Phạm', N'phamvan@gmail.com', N'095622228', CAST(N'2026-08-05T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 5, CAST(N'2026-08-25T11:16:34.047' AS DateTime), CAST(N'2026-08-25T18:16:34.470' AS DateTime), CAST(N'2026-08-25T18:16:39.530' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'91bdf404-9ac7-42d8-839c-983a56dd171f', N'2a8ce682-955a-406f-8c9a-b4795074f2c3', N'Phan Giang', N'phamvan@gmail.com', N'095622226', CAST(N'2026-08-20T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 6, CAST(N'2026-08-26T02:03:41.453' AS DateTime), CAST(N'2026-08-26T09:03:41.453' AS DateTime), CAST(N'2026-08-26T09:03:55.817' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'f41ff826-1f6b-4a9c-bedb-d2e0d38ae3da', N'f41ff826-1f6b-4a9c-bedb-d2e0d38ae3da', N'Cao Văn Nam', N'namvawn@gmail.com', N'094444444', CAST(N'2026-08-19T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 5, CAST(N'2026-08-25T09:37:51.007' AS DateTime), CAST(N'2026-08-25T16:37:51.800' AS DateTime), CAST(N'2026-08-25T17:43:26.503' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'41b89322-be18-42e2-88e1-129c7f190c18', N'41b89322-be18-42e2-88e1-129c7f190c18', N'Lương Huy', N'huyluong@gmail.com', N'09562222', CAST(N'2026-08-26T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 5, CAST(N'2026-08-25T09:47:02.657' AS DateTime), CAST(N'2026-08-25T16:47:02.657' AS DateTime), CAST(N'2026-08-25T17:42:32.757' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'c6af9a6e-2fcf-4e28-bb2d-f253b2f5c2db', N'2a8ce682-955a-406f-8c9a-b4795074f2c3', N'Huy Phan', N'phanhuy@gmail.com', N'095622226', CAST(N'2026-08-27T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 5, CAST(N'2026-08-26T01:30:55.017' AS DateTime), CAST(N'2026-08-26T08:30:55.150' AS DateTime), CAST(N'2026-08-26T08:31:01.193' AS DateTime), 1)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'9c5b94f4-4d30-4bb8-ac24-eb802c5c0e34', N'9c5b94f4-4d30-4bb8-ac24-eb802c5c0e34', N'Chu Liên_3444', N'liên@gmail.com', N'09562222', CAST(N'2026-08-26T00:00:00.000' AS DateTime), N'Female', N'cv.com.url', N'abccc', 6, CAST(N'2026-08-25T10:44:44.063' AS DateTime), CAST(N'2026-08-25T17:44:44.620' AS DateTime), CAST(N'2026-08-25T17:44:55.520' AS DateTime), 0)
GO
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Bác sĩ', N'Phòng khám chuyên khoa', 5, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-08-15T08:37:27.440' AS DateTime), NULL, 1)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'cf48499d-f8d0-44f2-8b5c-594cd7031731', N'Nhà thiết kế', N'Khoa thiết kế', 5, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-08-25T16:31:39.247' AS DateTime), NULL, 0)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'cdc797e1-f10e-41aa-ac99-740a7f2b0e73', N'kỹ sư xây dựng_2', N'Khoa xây dựng_4', 7, N'9d1ddae4-8f88-4684-a194-945a1faf485e', 0, CAST(N'2026-08-18T07:33:07.647' AS DateTime), CAST(N'2026-08-25T16:30:30.923' AS DateTime), 1)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'2a8ce682-955a-406f-8c9a-b4795074f2c3', N'Lập trình viên', N'Khoa CNTT', 5, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-08-25T16:19:57.520' AS DateTime), NULL, 0)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'a5a55c7f-ebb8-4744-ab4d-fbf240fcb866', N'kỹ sư nông nghiệp', N'Khoa Nông Nghiệp', 4, N'9d1ddae4-8f88-4684-a194-945a1faf485e', 0, CAST(N'2026-08-25T08:52:25.900' AS DateTime), CAST(N'2026-08-25T16:30:03.563' AS DateTime), 1)
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
﻿
