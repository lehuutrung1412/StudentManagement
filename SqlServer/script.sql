/****** Object:  Database [StudentManagement]    Script Date: 21/12/2021 9:41:31 AM ******/
CREATE DATABASE [StudentManagement]  (EDITION = 'GeneralPurpose', SERVICE_OBJECTIVE = 'GP_Gen5_2', MAXSIZE = 32 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS;
GO
ALTER DATABASE [StudentManagement] SET COMPATIBILITY_LEVEL = 150
GO
ALTER DATABASE [StudentManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StudentManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StudentManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StudentManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StudentManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [StudentManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StudentManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StudentManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StudentManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StudentManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StudentManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StudentManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StudentManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StudentManagement] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [StudentManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StudentManagement] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [StudentManagement] SET  MULTI_USER 
GO
ALTER DATABASE [StudentManagement] SET ENCRYPTION ON
GO
ALTER DATABASE [StudentManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [StudentManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  User [client]    Script Date: 21/12/2021 9:41:32 AM ******/
CREATE USER [client] FOR LOGIN [StumanApp] WITH DEFAULT_SCHEMA=[dbo]
GO
sys.sp_addrolemember @rolename = N'db_datareader', @membername = N'client'
GO
sys.sp_addrolemember @rolename = N'db_datawriter', @membername = N'client'
GO
/****** Object:  Table [dbo].[AbsentCalendar]    Script Date: 21/12/2021 9:41:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbsentCalendar](
	[Id] [uniqueidentifier] NOT NULL,
	[IdSubjectClass] [uniqueidentifier] NULL,
	[Period] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
	[Type] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[Id] [uniqueidentifier] NOT NULL,
	[IdUsers] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Id] [uniqueidentifier] NOT NULL,
	[IdTrainingForm] [uniqueidentifier] NULL,
	[IdFaculty] [uniqueidentifier] NULL,
	[DisplayName] [nvarchar](max) NULL,
	[IdTeacher] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NULL,
	[IdThumbnail] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ComponentScore]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComponentScore](
	[Id] [uniqueidentifier] NOT NULL,
	[IdSubjectClass] [uniqueidentifier] NULL,
	[DisplayName] [nvarchar](max) NULL,
	[ContributePercent] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseRegister]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseRegister](
	[Id] [uniqueidentifier] NOT NULL,
	[Status] [int] NULL,
	[IdStudent] [uniqueidentifier] NULL,
	[IdSubjectClass] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DatabaseImageTable]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DatabaseImageTable](
	[Id] [uniqueidentifier] NOT NULL,
	[Image] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetailScore]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetailScore](
	[Id] [uniqueidentifier] NOT NULL,
	[IdStudent] [uniqueidentifier] NULL,
	[IdComponentScore] [uniqueidentifier] NULL,
	[Score] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document](
	[Id] [uniqueidentifier] NOT NULL,
	[DisplayName] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[IdPoster] [uniqueidentifier] NULL,
	[IdFolder] [uniqueidentifier] NULL,
	[IdSubjectClass] [uniqueidentifier] NULL,
	[Size] [bigint] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Examination]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Examination](
	[Id] [uniqueidentifier] NOT NULL,
	[IdSubjectClass] [uniqueidentifier] NULL,
	[ExamName] [nvarchar](max) NULL,
	[WeekDay] [nvarchar](max) NOT NULL,
	[Period] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Faculty]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faculty](
	[Id] [uniqueidentifier] NOT NULL,
	[DisplayName] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[FoundationDay] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Faculty_TrainingForm]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Faculty_TrainingForm](
	[Id] [uniqueidentifier] NOT NULL,
	[IdTrainingForm] [uniqueidentifier] NULL,
	[IdFaculty] [uniqueidentifier] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Folder]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Folder](
	[Id] [uniqueidentifier] NOT NULL,
	[DisplayName] [nvarchar](max) NULL,
	[CreatedAt] [datetime] NULL,
	[IdSubjectClass] [uniqueidentifier] NULL,
	[IdPoster] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [uniqueidentifier] NOT NULL,
	[Topic] [nvarchar](max) NULL,
	[Content] [nvarchar](max) NULL,
	[Time] [datetime] NULL,
	[IdNotificationType] [uniqueidentifier] NULL,
	[IdPoster] [uniqueidentifier] NULL,
	[IdSubjectClass] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationComment]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationComment](
	[Id] [uniqueidentifier] NOT NULL,
	[IdUserComment] [uniqueidentifier] NULL,
	[IdNotification] [uniqueidentifier] NULL,
	[Content] [nvarchar](max) NULL,
	[Time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationImages]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationImages](
	[Id] [uniqueidentifier] NOT NULL,
	[IdNotification] [uniqueidentifier] NULL,
	[IdDatabaseImageTable] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationInfo]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[IdNotification] [uniqueidentifier] NULL,
	[IdUserReceiver] [uniqueidentifier] NULL,
	[IsRead] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationType](
	[Id] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OTP]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OTP](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Time] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Semester]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Semester](
	[Id] [uniqueidentifier] NOT NULL,
	[Batch] [nvarchar](max) NULL,
	[DisplayName] [nvarchar](max) NULL,
	[CourseRegisterStatus] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[Id] [uniqueidentifier] NOT NULL,
	[IdTrainingForm] [uniqueidentifier] NULL,
	[IdFaculty] [uniqueidentifier] NULL,
	[Status] [int] NULL,
	[IdUsers] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[Id] [uniqueidentifier] NOT NULL,
	[Credit] [int] NULL,
	[DisplayName] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[Describe] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubjectClass]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectClass](
	[Id] [uniqueidentifier] NOT NULL,
	[IdSubject] [uniqueidentifier] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[IdSemester] [uniqueidentifier] NULL,
	[Period] [nvarchar](max) NULL,
	[WeekDay] [int] NULL,
	[IdThumbnail] [uniqueidentifier] NULL,
	[IdTrainingForm] [uniqueidentifier] NULL,
	[Code] [nvarchar](max) NOT NULL,
	[NumberOfStudents] [int] NOT NULL,
	[MaxNumberOfStudents] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[Id] [uniqueidentifier] NOT NULL,
	[IdFaculty] [uniqueidentifier] NULL,
	[IdUsers] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher_SubjectClass]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher_SubjectClass](
	[IdSubjectClass] [uniqueidentifier] NOT NULL,
	[IdTeacher] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdSubjectClass] ASC,
	[IdTeacher] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingForm]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingForm](
	[Id] [uniqueidentifier] NOT NULL,
	[DisplayName] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrainingScore]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrainingScore](
	[Id] [uniqueidentifier] NOT NULL,
	[Score] [float] NULL,
	[IdSemester] [uniqueidentifier] NULL,
	[IdStudent] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_UserRole_UserInfo]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_UserRole_UserInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[IdUser] [uniqueidentifier] NULL,
	[IdUserRole_Info] [uniqueidentifier] NULL,
	[Content] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[Id] [uniqueidentifier] NOT NULL,
	[Role] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole_UserInfo]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole_UserInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[IdRole] [uniqueidentifier] NULL,
	[InfoName] [nvarchar](max) NOT NULL,
	[Type] [int] NULL,
	[IsEnable] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole_UserInfoItem]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole_UserInfoItem](
	[Id] [uniqueidentifier] NOT NULL,
	[IdUserRole_Info] [uniqueidentifier] NULL,
	[Content] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 21/12/2021 9:41:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[DisplayName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[IdOTP] [uniqueidentifier] NULL,
	[Online] [bit] NULL,
	[IdUserRole] [uniqueidentifier] NULL,
	[IdAvatar] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Admin] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Class] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Class] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ComponentScore] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[CourseRegister] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[CourseRegister] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[DatabaseImageTable] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[DetailScore] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Examination] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Faculty] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Faculty] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Faculty_TrainingForm] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Faculty_TrainingForm] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Folder] ADD  DEFAULT (newid()) FOR [IdPoster]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[NotificationComment] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[NotificationImages] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[NotificationInfo] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[NotificationInfo] ADD  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[NotificationType] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[OTP] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[OTP] ADD  DEFAULT (getdate()) FOR [Time]
GO
ALTER TABLE [dbo].[Semester] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Semester] ADD  DEFAULT ((0)) FOR [CourseRegisterStatus]
GO
ALTER TABLE [dbo].[Student] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[Subject] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Subject] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[SubjectClass] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[SubjectClass] ADD  DEFAULT ((0)) FOR [NumberOfStudents]
GO
ALTER TABLE [dbo].[SubjectClass] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Teacher] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[TrainingForm] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[TrainingForm] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[TrainingScore] ADD  DEFAULT ((0)) FOR [Score]
GO
ALTER TABLE [dbo].[User_UserRole_UserInfo] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserRole] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserRole_UserInfo] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[UserRole_UserInfoItem] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Online]
GO
ALTER TABLE [dbo].[AbsentCalendar]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[Admin]  WITH CHECK ADD FOREIGN KEY([IdUsers])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([IdFaculty])
REFERENCES [dbo].[Faculty] ([Id])
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([IdTeacher])
REFERENCES [dbo].[Teacher] ([Id])
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([IdThumbnail])
REFERENCES [dbo].[DatabaseImageTable] ([Id])
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD FOREIGN KEY([IdTrainingForm])
REFERENCES [dbo].[TrainingForm] ([Id])
GO
ALTER TABLE [dbo].[ComponentScore]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[CourseRegister]  WITH CHECK ADD FOREIGN KEY([IdStudent])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[CourseRegister]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[DetailScore]  WITH CHECK ADD FOREIGN KEY([IdComponentScore])
REFERENCES [dbo].[ComponentScore] ([Id])
GO
ALTER TABLE [dbo].[DetailScore]  WITH CHECK ADD FOREIGN KEY([IdStudent])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD FOREIGN KEY([IdFolder])
REFERENCES [dbo].[Folder] ([Id])
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD FOREIGN KEY([IdPoster])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[Examination]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[Faculty_TrainingForm]  WITH CHECK ADD FOREIGN KEY([IdFaculty])
REFERENCES [dbo].[Faculty] ([Id])
GO
ALTER TABLE [dbo].[Faculty_TrainingForm]  WITH CHECK ADD FOREIGN KEY([IdTrainingForm])
REFERENCES [dbo].[TrainingForm] ([Id])
GO
ALTER TABLE [dbo].[Folder]  WITH CHECK ADD FOREIGN KEY([IdPoster])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Folder]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([IdNotificationType])
REFERENCES [dbo].[NotificationType] ([Id])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([IdPoster])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([IdPoster])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[NotificationComment]  WITH CHECK ADD FOREIGN KEY([IdNotification])
REFERENCES [dbo].[Notification] ([Id])
GO
ALTER TABLE [dbo].[NotificationComment]  WITH CHECK ADD FOREIGN KEY([IdUserComment])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[NotificationImages]  WITH CHECK ADD FOREIGN KEY([IdDatabaseImageTable])
REFERENCES [dbo].[DatabaseImageTable] ([Id])
GO
ALTER TABLE [dbo].[NotificationImages]  WITH CHECK ADD FOREIGN KEY([IdNotification])
REFERENCES [dbo].[Notification] ([Id])
GO
ALTER TABLE [dbo].[NotificationInfo]  WITH CHECK ADD FOREIGN KEY([IdNotification])
REFERENCES [dbo].[Notification] ([Id])
GO
ALTER TABLE [dbo].[NotificationInfo]  WITH CHECK ADD FOREIGN KEY([IdUserReceiver])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([IdFaculty])
REFERENCES [dbo].[Faculty] ([Id])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([IdTrainingForm])
REFERENCES [dbo].[TrainingForm] ([Id])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD FOREIGN KEY([IdUsers])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[SubjectClass]  WITH CHECK ADD FOREIGN KEY([IdSemester])
REFERENCES [dbo].[Semester] ([Id])
GO
ALTER TABLE [dbo].[SubjectClass]  WITH CHECK ADD FOREIGN KEY([IdSubject])
REFERENCES [dbo].[Subject] ([Id])
GO
ALTER TABLE [dbo].[SubjectClass]  WITH CHECK ADD FOREIGN KEY([IdThumbnail])
REFERENCES [dbo].[DatabaseImageTable] ([Id])
GO
ALTER TABLE [dbo].[SubjectClass]  WITH CHECK ADD FOREIGN KEY([IdTrainingForm])
REFERENCES [dbo].[TrainingForm] ([Id])
GO
ALTER TABLE [dbo].[Teacher]  WITH CHECK ADD FOREIGN KEY([IdFaculty])
REFERENCES [dbo].[Faculty] ([Id])
GO
ALTER TABLE [dbo].[Teacher]  WITH CHECK ADD FOREIGN KEY([IdUsers])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Teacher_SubjectClass]  WITH CHECK ADD FOREIGN KEY([IdSubjectClass])
REFERENCES [dbo].[SubjectClass] ([Id])
GO
ALTER TABLE [dbo].[Teacher_SubjectClass]  WITH CHECK ADD FOREIGN KEY([IdTeacher])
REFERENCES [dbo].[Teacher] ([Id])
GO
ALTER TABLE [dbo].[TrainingScore]  WITH CHECK ADD FOREIGN KEY([IdSemester])
REFERENCES [dbo].[Semester] ([Id])
GO
ALTER TABLE [dbo].[TrainingScore]  WITH CHECK ADD FOREIGN KEY([IdStudent])
REFERENCES [dbo].[Student] ([Id])
GO
ALTER TABLE [dbo].[User_UserRole_UserInfo]  WITH CHECK ADD FOREIGN KEY([IdUser])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[User_UserRole_UserInfo]  WITH CHECK ADD FOREIGN KEY([IdUserRole_Info])
REFERENCES [dbo].[UserRole_UserInfo] ([Id])
GO
ALTER TABLE [dbo].[UserRole_UserInfo]  WITH CHECK ADD FOREIGN KEY([IdRole])
REFERENCES [dbo].[UserRole] ([Id])
GO
ALTER TABLE [dbo].[UserRole_UserInfoItem]  WITH CHECK ADD FOREIGN KEY([IdUserRole_Info])
REFERENCES [dbo].[UserRole_UserInfo] ([Id])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([IdAvatar])
REFERENCES [dbo].[DatabaseImageTable] ([Id])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([IdOTP])
REFERENCES [dbo].[OTP] ([Id])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([IdUserRole])
REFERENCES [dbo].[UserRole] ([Id])
GO
ALTER DATABASE [StudentManagement] SET  READ_WRITE 
GO
