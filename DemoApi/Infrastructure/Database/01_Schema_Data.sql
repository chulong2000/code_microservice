USE [DemoEducationLevelDb]
GO
/****** Object:  Table [dbo].[EducationLevel]    Script Date: 09/09/2026 9:57:33 SA ******/
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
	[ParentId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationLevelSalaryCoefficient]    Script Date: 09/09/2026 9:57:33 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationLevelSalaryCoefficient](
	[Id] [uniqueidentifier] NOT NULL,
	[EducationLevelId] [uniqueidentifier] NOT NULL,
	[BaseCoefficient] [decimal](5, 2) NULL,
	[AllowancePercentage] [decimal](5, 2) NOT NULL,
	[EffectiveFrom] [datetime] NOT NULL,
	[Notes] [nvarchar](200) NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UpdatedAt] [datetime] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobApplication]    Script Date: 09/09/2026 9:57:33 SA ******/
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
/****** Object:  Table [dbo].[JobPosition]    Script Date: 09/09/2026 9:57:33 SA ******/
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
	[ParentId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MyEmployees]    Script Date: 09/09/2026 9:57:33 SA ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyEmployees](
	[EmployeeID] [smallint] NOT NULL,
	[FirstName] [nvarchar](30) NOT NULL,
	[LastName] [nvarchar](40) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[DeptID] [smallint] NOT NULL,
	[ManagerID] [int] NULL,
 CONSTRAINT [PK_EmployeeID] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'8cd1319d-31ca-4185-ab80-022aeb3d7de9', N'Định hướng dưỡng sinh', N'test_abccc', 3, 0, CAST(N'2026-09-08T14:59:59.510' AS DateTime), NULL, N'a0696403-f831-461a-b03f-3ecd07ab2cac')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'bcece7c5-8ae1-410f-935e-05fe6e7c0f46', N'Trung cấp', N'Test_abcccc66', 0, 1, CAST(N'2026-08-26T09:28:14.867' AS DateTime), CAST(N'2026-08-26T09:28:22.400' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'b7cd1c4d-8157-4e30-8503-08a97660787a', N'Khối bác sĩ 2', N'string', 3, 0, CAST(N'2026-09-08T14:46:29.987' AS DateTime), CAST(N'2026-09-09T09:45:42.293' AS DateTime), N'4f5a79b7-2c84-48ba-ae75-965cfd3aac91')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'2a032bb5-f810-4650-a3fd-0c1060d058c2', N'Định hướng châm cứu', N'test_abccc', 3, 0, CAST(N'2026-09-08T14:59:50.380' AS DateTime), NULL, N'a0696403-f831-461a-b03f-3ecd07ab2cac')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'f1a12e19-91a4-4d10-b12d-19ec7f35a467', N'Bác sĩ răng hàm mặt', N'test_abccc', 3, 0, CAST(N'2026-09-08T14:57:40.907' AS DateTime), NULL, N'b7cd1c4d-8157-4e30-8503-08a97660787a')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'aab37877-e299-4c7e-b49c-2aa8897adca9', N'cử nhân', N'string_abccc', 0, 1, CAST(N'2026-08-12T09:22:36.740' AS DateTime), CAST(N'2026-08-25T08:45:12.833' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'3fff0d43-0ca1-494f-a127-31b6e64fcfc2', N'Nhân bản đang test0', N'.', 0, 0, CAST(N'2026-08-27T17:11:56.123' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'282b8e23-2ff1-4acb-a08b-36e967047ada', N'Trung cấp', NULL, 0, 1, CAST(N'2026-08-26T09:10:42.133' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'6054a6dd-b1c6-4445-860c-3ba19ca0c297', N'Bác sĩ y học dự phòng', N'test_abccc', 3, 0, CAST(N'2026-09-08T14:57:12.670' AS DateTime), NULL, N'b7cd1c4d-8157-4e30-8503-08a97660787a')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'a6ef7f71-a2ab-4edf-99dd-3c4d598637eb', N'Nhân bản đang test', N',', 0, 0, CAST(N'2026-08-27T17:11:31.757' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'c0bac7a6-1673-41e8-970d-3e923e0e26cc', N'Định hướng ngoại khoa', N'test_abccc', 3, 1, CAST(N'2026-09-08T14:58:43.667' AS DateTime), CAST(N'2026-09-08T16:35:48.440' AS DateTime), N'51c29347-2d3a-4ddd-8bea-a47349da506a')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'a0696403-f831-461a-b03f-3ecd07ab2cac', N'Bác sĩ y học cổ truyền', N'test_abccc', 3, 0, CAST(N'2026-09-08T14:55:44.967' AS DateTime), NULL, N'b7cd1c4d-8157-4e30-8503-08a97660787a')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'c4b8cfc2-d3cd-4db3-883c-5827d793ab76', N'Trung cấp', N'test_abccc_00000', 5, 1, CAST(N'2026-08-25T08:45:04.023' AS DateTime), CAST(N'2026-08-25T08:45:26.530' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'59f0c288-c909-44ec-9e77-68aebd0088d9', N'Định hướng nội khoa', N'test_abccc', 3, 1, CAST(N'2026-09-08T14:58:31.567' AS DateTime), CAST(N'2026-09-08T16:35:48.440' AS DateTime), N'51c29347-2d3a-4ddd-8bea-a47349da506a')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'dd899645-cc59-4383-8e17-7229066399f1', N'string_abcccccccc', N'string', 0, 0, CAST(N'2026-09-07T09:18:53.213' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'5c8d21bf-5a9c-47f6-b305-8a25c0852085', N'string_abcccccc', N'string', 0, 0, CAST(N'2026-09-04T09:21:44.757' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', N'đại học', N'string123', 0, 0, CAST(N'2026-08-12T08:46:30.687' AS DateTime), CAST(N'2026-08-26T09:55:13.843' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'9d1ddae4-8f88-4684-a194-945a1faf485e', N'giáo sư', N'string', 0, 1, CAST(N'2026-08-12T09:22:46.217' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'335b533b-951b-4486-9584-95b2af501574', N'Cao Đẳng', N'test_abccccc', 0, 1, CAST(N'2026-08-26T09:29:05.850' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'4f5a79b7-2c84-48ba-ae75-965cfd3aac91', N'Cử nhân123', N'abcccc,', 0, 0, CAST(N'2026-08-26T09:55:08.790' AS DateTime), CAST(N'2026-08-27T17:12:28.163' AS DateTime), NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'ea870002-2b05-4ad7-98e1-97e25e13a2b1', N'Định hướng phục hình', N'test_abccc', 3, 0, CAST(N'2026-09-08T15:00:48.963' AS DateTime), NULL, N'f1a12e19-91a4-4d10-b12d-19ec7f35a467')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'51c29347-2d3a-4ddd-8bea-a47349da506a', N'Bác sĩ đa khoa', N'test_abccc', 3, 1, CAST(N'2026-09-08T14:55:14.630' AS DateTime), CAST(N'2026-09-08T16:35:48.440' AS DateTime), N'b7cd1c4d-8157-4e30-8503-08a97660787a')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'd2862c90-fcb8-4224-9252-a49935b6ded0', N'Định hướng sản phụ khoa', N'test_abccc', 3, 1, CAST(N'2026-09-08T14:58:54.220' AS DateTime), CAST(N'2026-09-08T16:35:48.440' AS DateTime), N'51c29347-2d3a-4ddd-8bea-a47349da506a')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'34c6e98b-88d8-442f-99bf-b5d0905f14d4', N'string', N'string', 0, 0, CAST(N'2026-08-26T13:18:15.307' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'15a03330-af58-403e-be72-bcf5e09a7f91', N'Trung cấp', N'test_abcccccc', 0, 1, CAST(N'2026-08-26T09:28:45.573' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'4f2f20c2-3713-4acd-b2ac-d4c5722f5888', N'test_abcc', N'string', 0, 1, CAST(N'2026-08-22T11:27:50.087' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'04e043c3-fb44-4ead-8088-dd19207aa0b1', N'Định hướng chỉnh nha', N'test_abccc', 3, 0, CAST(N'2026-09-08T15:00:37.230' AS DateTime), NULL, N'f1a12e19-91a4-4d10-b12d-19ec7f35a467')
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'36716d4d-f685-4a91-b389-e60e8d698728', N'Tiến sĩ', N'test_abccc', 0, 1, CAST(N'2026-08-25T11:27:11.307' AS DateTime), NULL, NULL)
INSERT [dbo].[EducationLevel] ([Id], [Name], [Description], [Order], [IsDeleted], [CreatedAt], [UpdatedAt], [ParentId]) VALUES (N'317d8940-2ed3-4698-9ac4-f99e591d2b52', N'Định hướng nhi khoa', N'test_abccc', 3, 1, CAST(N'2026-09-08T14:59:01.793' AS DateTime), CAST(N'2026-09-08T16:35:48.440' AS DateTime), N'51c29347-2d3a-4ddd-8bea-a47349da506a')
GO
INSERT [dbo].[EducationLevelSalaryCoefficient] ([Id], [EducationLevelId], [BaseCoefficient], [AllowancePercentage], [EffectiveFrom], [Notes], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'ac97283a-8308-42f8-b054-3b793900762e', N'15a03330-af58-403e-be72-bcf5e09a7f91', CAST(0.24 AS Decimal(5, 2)), CAST(0.12 AS Decimal(5, 2)), CAST(N'2026-08-28T00:00:00.000' AS DateTime), N'abcc', CAST(N'2026-08-26T09:49:44.760' AS DateTime), NULL, 0)
INSERT [dbo].[EducationLevelSalaryCoefficient] ([Id], [EducationLevelId], [BaseCoefficient], [AllowancePercentage], [EffectiveFrom], [Notes], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'c7290a4e-61c5-4765-af9d-4ae1ce2205d9', N'335b533b-951b-4486-9584-95b2af501574', CAST(0.33 AS Decimal(5, 2)), CAST(0.15 AS Decimal(5, 2)), CAST(N'2026-08-26T00:00:00.000' AS DateTime), N'abccc', CAST(N'2026-08-26T09:45:22.650' AS DateTime), NULL, 0)
INSERT [dbo].[EducationLevelSalaryCoefficient] ([Id], [EducationLevelId], [BaseCoefficient], [AllowancePercentage], [EffectiveFrom], [Notes], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'5ae5a0a5-3093-4dcc-8687-c5db8a9406ba', N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', CAST(0.45 AS Decimal(5, 2)), CAST(0.37 AS Decimal(5, 2)), CAST(N'2026-08-20T00:00:00.000' AS DateTime), NULL, CAST(N'2026-08-27T17:34:26.280' AS DateTime), CAST(N'2026-08-28T14:14:42.500' AS DateTime), 0)
GO
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'044084fd-8682-4014-be22-11f0dfa06893', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Khánh Tùng', N'tungkh@gmail.com', N'097122222', CAST(N'2026-08-18T16:53:46.910' AS DateTime), N'Male', N'abc.url.com', N'string', 5, CAST(N'2026-08-18T09:53:46.910' AS DateTime), CAST(N'2026-08-18T09:53:46.910' AS DateTime), NULL, 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'b39fb7b0-b53d-4885-8179-07166d0b2ceb', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Nguyễn Cường', N'tungkh@gmail.com', N'097122222', CAST(N'2026-08-18T16:54:04.773' AS DateTime), N'Male', N'abc.url.com', N'string', 5, CAST(N'2026-08-18T09:54:04.770' AS DateTime), CAST(N'2026-08-18T09:54:04.770' AS DateTime), NULL, 1)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'483e9b92-bdb0-4957-a09b-77e140eea910', N'cdc797e1-f10e-41aa-ac99-740a7f2b0e73', N'Phan Huy', N'huynk@gmail.com', N'9292822', CAST(N'2026-09-12T17:15:00.000' AS DateTime), N'Male', N'abc.url.com', NULL, 6, CAST(N'2026-08-20T10:05:00.000' AS DateTime), CAST(N'2026-08-20T10:05:00.000' AS DateTime), CAST(N'2026-08-20T10:05:00.000' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'839e6cd3-82a9-4c58-b68b-13c7080521c2', N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'khanh pham', N'khanhpham@gmail.com', N'string', CAST(N'2026-08-21T02:01:25.517' AS DateTime), N'string', N'string', N'string', 5, CAST(N'2026-08-21T09:02:00.143' AS DateTime), CAST(N'2026-08-21T09:02:00.143' AS DateTime), NULL, 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'30dafe16-a3b8-437c-a899-2ad293319224', N'30dafe16-a3b8-437c-a899-2ad293319224', N'Khánh Phạm', N'phamvan@gmail.com', N'095622228', CAST(N'2026-08-05T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 5, CAST(N'2026-08-25T11:16:34.047' AS DateTime), CAST(N'2026-08-25T18:16:34.470' AS DateTime), CAST(N'2026-08-25T18:16:39.530' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'91bdf404-9ac7-42d8-839c-983a56dd171f', N'2a8ce682-955a-406f-8c9a-b4795074f2c3', N'Phan Giang', N'phamvan@gmail.com', N'095622226', CAST(N'2026-08-20T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 6, CAST(N'2026-08-25T19:03:41.453' AS DateTime), CAST(N'2026-08-26T09:03:41.453' AS DateTime), CAST(N'2026-08-27T17:13:25.537' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'f41ff826-1f6b-4a9c-bedb-d2e0d38ae3da', N'f41ff826-1f6b-4a9c-bedb-d2e0d38ae3da', N'Cao Văn Nam', N'namvawn@gmail.com', N'094444444', CAST(N'2026-08-19T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 5, CAST(N'2026-08-25T09:37:51.007' AS DateTime), CAST(N'2026-08-25T16:37:51.800' AS DateTime), CAST(N'2026-08-25T17:43:26.503' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'41b89322-be18-42e2-88e1-129c7f190c18', N'41b89322-be18-42e2-88e1-129c7f190c18', N'Lương Huy', N'huyluong@gmail.com', N'09562222', CAST(N'2026-08-26T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 5, CAST(N'2026-08-25T09:47:02.657' AS DateTime), CAST(N'2026-08-25T16:47:02.657' AS DateTime), CAST(N'2026-08-25T17:42:32.757' AS DateTime), 0)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'c6af9a6e-2fcf-4e28-bb2d-f253b2f5c2db', N'2a8ce682-955a-406f-8c9a-b4795074f2c3', N'Huy Phan', N'phanhuy@gmail.com', N'095622226', CAST(N'2026-08-27T00:00:00.000' AS DateTime), N'Male', N'cv.com.url', N'abcccc', 5, CAST(N'2026-08-26T01:30:55.017' AS DateTime), CAST(N'2026-08-26T08:30:55.150' AS DateTime), CAST(N'2026-08-26T08:31:01.193' AS DateTime), 1)
INSERT [dbo].[JobApplication] ([Id], [JobPositionId], [FullName], [Email], [PhoneNumber], [DateOfBirth], [Gender], [CvFileUrl], [CoverLetter], [YearsOfExperience], [AppliedAt], [CreatedAt], [UpdatedAt], [IsDeleted]) VALUES (N'9c5b94f4-4d30-4bb8-ac24-eb802c5c0e34', N'9c5b94f4-4d30-4bb8-ac24-eb802c5c0e34', N'Chu Liên_3444', N'liên@gmail.com', N'09562222', CAST(N'2026-08-26T00:00:00.000' AS DateTime), N'Female', N'cv.com.url', N'abccc', 6, CAST(N'2026-08-25T10:44:44.063' AS DateTime), CAST(N'2026-08-25T17:44:44.620' AS DateTime), CAST(N'2026-08-25T17:44:55.520' AS DateTime), 0)
GO
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'cf608c5c-1e3c-4c71-bb3e-029e398e8e53', N'Điều dưỡng nội khoa', N'Khoa Nội', 6, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-09-09T09:14:53.720' AS DateTime), CAST(N'2026-09-09T09:34:56.860' AS DateTime), 1, N'98a6b7a1-508e-4683-bdc3-2e120dddceba')
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'994aff68-bfd6-48a9-bd36-09a82fdf5409', N'Bác sĩ', N'Phòng khám chuyên khoa', 5, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-08-15T08:37:27.440' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'74411bb6-07c6-498e-992a-259160f9f695', N'Phó Giám Đốc bệnh viện', N'Ban Giám Đốc', 7, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-09-09T09:15:39.963' AS DateTime), CAST(N'2026-09-09T09:33:54.660' AS DateTime), 0, N'ac67ffe8-bed1-417c-949f-4599b50abff5')
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'98a6b7a1-508e-4683-bdc3-2e120dddceba', N'Trưởng khoa nội', N'Khoa Nội', 6, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-09-09T09:13:25.440' AS DateTime), CAST(N'2026-09-09T09:34:56.860' AS DateTime), 1, N'ac67ffe8-bed1-417c-949f-4599b50abff5')
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'ac67ffe8-bed1-417c-949f-4599b50abff5', N'Giám đốc bênh viện', N'Ban Giám Đốc', 6, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-09-08T17:51:57.017' AS DateTime), NULL, 0, NULL)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'cf48499d-f8d0-44f2-8b5c-594cd7031731', N'Nhà thiết kế', N'Khoa thiết kế', 5, N'3fff0d43-0ca1-494f-a127-31b6e64fcfc2', 1, CAST(N'2026-08-25T16:31:39.247' AS DateTime), CAST(N'2026-08-27T17:13:43.897' AS DateTime), 1, NULL)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'cdc797e1-f10e-41aa-ac99-740a7f2b0e73', N'kỹ sư xây dựng_2', N'Khoa xây dựng_4', 7, N'9d1ddae4-8f88-4684-a194-945a1faf485e', 0, CAST(N'2026-08-18T07:33:07.647' AS DateTime), CAST(N'2026-08-25T16:30:30.923' AS DateTime), 1, NULL)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'2d2bbacb-c5be-4f95-9c51-824607816475', N'Bác sĩ nội khoa', N'Khoa Nội', 6, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-09-09T09:14:09.087' AS DateTime), CAST(N'2026-09-09T09:34:56.860' AS DateTime), 1, N'98a6b7a1-508e-4683-bdc3-2e120dddceba')
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'2a8ce682-955a-406f-8c9a-b4795074f2c3', N'Lập trình viên', N'Khoa CNTT', 5, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-08-25T16:19:57.520' AS DateTime), CAST(N'2026-08-27T17:14:46.677' AS DateTime), 1, NULL)
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'f4943d8b-8ccb-4a25-8f0c-cd726410d390', N'Bác sĩ khoa ngoại', N'Khoa Ngoại', 6, N'67feba50-7b7d-4ffb-a771-8f8e16b429f6', 1, CAST(N'2026-09-09T09:16:49.980' AS DateTime), NULL, 0, N'74411bb6-07c6-498e-992a-259160f9f695')
INSERT [dbo].[JobPosition] ([Id], [Title], [Department], [OpenSlots], [MinimumEducationLevelId], [IsOpen], [CreatedAt], [UpdatedAt], [IsDeleted], [ParentId]) VALUES (N'a5a55c7f-ebb8-4744-ab4d-fbf240fcb866', N'kỹ sư nông nghiệp', N'Khoa Nông Nghiệp', 4, N'9d1ddae4-8f88-4684-a194-945a1faf485e', 0, CAST(N'2026-08-25T08:52:25.900' AS DateTime), CAST(N'2026-08-25T16:30:03.563' AS DateTime), 1, NULL)
GO
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (1, N'Ken', N'Sánchez', N'Chief Executive Officer', 16, NULL)
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (16, N'David', N'Bradley', N'Marketing Manager', 4, 273)
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (23, N'Mary', N'Gibson', N'Marketing Specialist', 4, 16)
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (273, N'Brian', N'Welcker', N'Vice President of Sales', 3, 1)
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (274, N'Stephen', N'Jiang', N'North American Sales Manager', 3, 273)
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (275, N'Michael', N'Blythe', N'Sales Representative', 3, 274)
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (276, N'Linda', N'Mitchell', N'Sales Representative', 3, 274)
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (285, N'Syed', N'Abbas', N'Pacific Sales Manager', 3, 273)
INSERT [dbo].[MyEmployees] ([EmployeeID], [FirstName], [LastName], [Title], [DeptID], [ManagerID]) VALUES (286, N'Lynn', N'Tsoflias', N'Sales Representative', 3, 285)
GO
ALTER TABLE [dbo].[EducationLevel] ADD  DEFAULT ((0)) FOR [Order]
GO
ALTER TABLE [dbo].[EducationLevel] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[EducationLevel]  WITH CHECK ADD FOREIGN KEY([ParentId])
REFERENCES [dbo].[EducationLevel] ([Id])
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
ALTER TABLE [dbo].[EducationLevel]  WITH CHECK ADD  CONSTRAINT [CK_EducationLevel_Order] CHECK  (([Order]<=(5)))
GO
ALTER TABLE [dbo].[EducationLevel] CHECK CONSTRAINT [CK_EducationLevel_Order]
GO
