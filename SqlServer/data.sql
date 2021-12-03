-- USE TEMP
--  DROP DATABASE StudentManagement
CREATE DATABASE StudentManagement
GO

USE StudentManagement
GO

CREATE TABLE Users
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  Username NVARCHAR(MAX),
  Password NVARCHAR(MAX),
  DisplayName NVARCHAR(MAX),
  --DayOfBirth DATETIME,
  --Gender INT,
  --Email NVARCHAR(MAX),
  --PhoneNumber NVARCHAR(MAX),
  Online BIT DEFAULT 0,
  -- 1: online, 0: offline
  IdUserRole UNIQUEIDENTIFIER NOT NULL,
  IdFaculty UNIQUEIDENTIFIER NOT NULL,
  IdAvatar UNIQUEIDENTIFIER NOT NULL,
)
GO

CREATE TABLE UserRole
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  Role NVARCHAR(MAX),
)
GO

--CREATE TABLE UserInfo
--(
--	Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
--	InfoName NVARCHAR(MAX) NOT NULL,
--	Type INT, --0: textbox 1:datepicker 2:combobox
--)
--GO

CREATE TABLE UserRole_UserInfo
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdRole UNIQUEIDENTIFIER NOT NULL,
  InfoName NVARCHAR(MAX) NOT NULL,
  Type INT,
  IsEnable BIT,
  -- 1: có thể chỉnh sửa 0: không thể
  IsDeleted BIT
  --1: bị xoá --0: còn
)
GO

CREATE TABLE User_UserRole_UserInfo
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdUser UNIQUEIDENTIFIER NOT NULL,
  IdUserRole_Info UNIQUEIDENTIFIER NOT NULL,
  Content NVARCHAR(MAX),
)
GO

CREATE TABLE UserRole_UserInfoItem --Cho UserInfo dạng combobox
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdUserRole_Info UNIQUEIDENTIFIER NOT NULL,
  Content NVARCHAR(MAX),
)
GO

--CREATE TABLE Parent
--(
--  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
--  NameDad NVARCHAR(MAX),
--  NameMom NVARCHAR(MAX),
--  AddressDad NVARCHAR(MAX),
--  AddressMom NVARCHAR(MAX),
--  PhoneDad NVARCHAR(MAX),
--  PhoneMom NVARCHAR(MAX),
--  JobDad NVARCHAR(MAX),
--  JobMom NVARCHAR(MAX),
--)
--GO


CREATE TABLE Student
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdTrainingForm UNIQUEIDENTIFIER NOT NULL,
  Status INT DEFAULT 1,
  -- 1: còn học, 0: đã tốt nghiệp
  IdUsers UNIQUEIDENTIFIER
  --IdParent UNIQUEIDENTIFIER
)
GO

CREATE TABLE Teacher
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdUsers UNIQUEIDENTIFIER
)
GO

CREATE TABLE Admin
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdUsers UNIQUEIDENTIFIER
)
GO

CREATE TABLE Faculty
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  DisplayName NVARCHAR(MAX),
  IsDeleted BIT DEFAULT 0,
  FoundationDay DateTime,
)
GO

CREATE TABLE TrainingForm
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  DisplayName NVARCHAR(MAX),
  IsDeleted BIT DEFAULT 0,
)
GO

CREATE TABLE Faculty_TrainingForm
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdTrainingForm UNIQUEIDENTIFIER NOT NULL,
  IdFaculty UNIQUEIDENTIFIER NOT NULL,
  IsDeleted BIT DEFAULT 0,
)
GO

CREATE TABLE StudyResult
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdStudent UNIQUEIDENTIFIER NOT NULL,
  IdSubjectClass UNIQUEIDENTIFIER NOT NULL,
  -- ProcessScore FLOAT,
  -- MidTermScore FLOAT,
  -- FinalTermScore FLOAT,
  -- PracticeScore FLOAT,
  -- AverageScore FLOAT,
  -- ProcessScorePercentage FLOAT,
  -- MidTermScorePercentage FLOAT,
  -- FinalTermScorePercentage FLOAT,
  -- PracticeScorePercentage FLOAT,
)
GO

CREATE TABLE ComponentScore
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdSubjectClass UNIQUEIDENTIFIER NOT NULL,
  DisplayName NVARCHAR(MAX),
  ContributePercent FLOAT,
)
GO

CREATE TABLE DetailScore
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdStudent UNIQUEIDENTIFIER NOT NULL,
  IdComponentScore UNIQUEIDENTIFIER NOT NULL,
  Score FLOAT,
)
GO

CREATE TABLE Semester
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  Batch NVARCHAR(MAX),
  DisplayName NVARCHAR(MAX),
  CourseRegisterStatus INT DEFAULT 0,
)
GO

CREATE TABLE Class
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdTrainingForm UNIQUEIDENTIFIER NOT NULL,
  IdFaculty UNIQUEIDENTIFIER NOT NULL,
  DisplayName NVARCHAR(MAX),
  IdTeacher UNIQUEIDENTIFIER NOT NULL,
  IsDeleted BIT DEFAULT 0,
  IdThumbnail UNIQUEIDENTIFIER NOT NULL,
)
GO

CREATE TABLE Subject
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  Credit NVARCHAR(MAX) NOT NULL,
  DisplayName NVARCHAR(MAX),
  Code NVARCHAR(MAX),
  Describe NVARCHAR(MAX),
  IsDeleted BIT DEFAULT 0,
)
GO

CREATE TABLE SubjectClass
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdSubject UNIQUEIDENTIFIER NOT NULL,
  StartDate DateTime,
  EndDate DateTime,
  IdSemester UNIQUEIDENTIFIER,
  Period NVARCHAR(MAX) NOT NULL,
  WeekDay NVARCHAR(MAX) NOT NULL,
  IdThumbnail UNIQUEIDENTIFIER NOT NULL,
  IdTrainingForm UNIQUEIDENTIFIER NOT NULL,
  Code NVARCHAR(MAX) NOT NULL,
  NumberOfStudents INT NOT NULL,
  MaxNumberOfStudents INT NOT NULL,
)
GO

CREATE TABLE Examination
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdSubjectClass UNIQUEIDENTIFIER ,
  ExamName NVARCHAR(MAX),
  WeekDay NVARCHAR(MAX) NOT NULL,
  Period NVARCHAR(MAX) NOT NULL,

)
GO

CREATE TABLE Teacher_SubjectClass
(
  IdSubjectClass UNIQUEIDENTIFIER ,
  IdTeacher UNIQUEIDENTIFIER ,

  PRIMARY KEY 
    (IdSubjectClass,IdTeacher),
)
GO

CREATE TABLE CourseRegister
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  Status INT DEFAULT 0,
  -- 0:Chưa đăng ký || 1:Đã đăng ký || 2:Đang chờ duyệt
  IdStudent UNIQUEIDENTIFIER NOT NULL,
  IdSubjectClass UNIQUEIDENTIFIER NOT NULL,
  IdSemester UNIQUEIDENTIFIER NOT NULL,
)
GO

CREATE TABLE TrainingScore
(
  Id UNIQUEIDENTIFIER PRIMARY KEY,
  Score FLOAT DEFAULT 0,
  IdSemester UNIQUEIDENTIFIER NOT NULL,
  IdStudent UNIQUEIDENTIFIER NOT NULL,
)
GO

CREATE TABLE Document
(
  Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  DisplayName NVARCHAR(MAX),
  Content NVARCHAR(MAX),
  CreatedAt DateTime,
  IdPoster UNIQUEIDENTIFIER NOT NULL,
  IdFolder UNIQUEIDENTIFIER,
  IdSubjectClass UNIQUEIDENTIFIER NOT NULL,
  Size BIGINT
)
GO

CREATE TABLE Folder
(
  Id UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
  DisplayName NVARCHAR(MAX),
  CreatedAt DateTime,
  IdSubjectClass UNIQUEIDENTIFIER NOT NULL,
  IdPoster UNIQUEIDENTIFIER DEFAULT NEWID()
)
GO

CREATE TABLE AbsentCalendar
(
  Id UNIQUEIDENTIFIER PRIMARY KEY,
  IdSubjectClass UNIQUEIDENTIFIER NOT NULL,
  Date DateTime,
  Type INT,
)
GO

CREATE TABLE DatabaseImageTable
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  Image VARBINARY(max),
)
GO

CREATE TABLE Notification
(
	Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	Topic NVARCHAR(MAX),
	Content NVARCHAR(MAX),
	Time DateTime,
	IdNotificationType UNIQUEIDENTIFIER,
	IdPoster UNIQUEIDENTIFIER NOT NULL,
	IdSubjectClass UNIQUEIDENTIFIER,	
)
CREATE TABLE NotificationType
(
	Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	Content NVARCHAR(MAX),
)
CREATE TABLE NotificationInfo
(
	Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	IdNotification UNIQUEIDENTIFIER NOT NULL,
	IdUserReceiver UNIQUEIDENTIFIER NOT NULL,
	IsRead BIT DEFAULT 0, 
)
CREATE TABLE NotificationComment
(
	Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	IdUserComment UNIQUEIDENTIFIER NOT NULL,
	IdNotification UNIQUEIDENTIFIER NOT NULL,
	Content NVARCHAR(MAX),
	Time DateTime,
)
CREATE TABLE NotificationImages
(
	Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
	IdNotification UNIQUEIDENTIFIER NOT NULL,
	IdDatabaseImageTable UNIQUEIDENTIFIER NOT NULL,
)



-- foreign key
ALTER TABLE  Users
ADD FOREIGN KEY (IdFaculty) REFERENCES Faculty(Id),
FOREIGN KEY (IdAvatar) REFERENCES DatabaseImageTable(Id),
FOREIGN KEY (IdUserRole) REFERENCES UserRole(Id)
GO

ALTER TABLE UserRole_UserInfo
ADD FOREIGN KEY (IdRole) REFERENCES UserRole(Id)
GO

ALTER TABLE User_UserRole_UserInfo
ADD FOREIGN KEY (IdUser) REFERENCES Users(Id),
FOREIGN KEY (IdUserRole_Info) REFERENCES UserRole_UserInfo(Id)
GO

ALTER TABLE UserRole_UserInfoItem
ADD FOREIGN KEY (IdUserRole_Info) REFERENCES UserRole_UserInfo(Id)
GO

ALTER TABLE  Student
ADD FOREIGN KEY(IdTrainingForm) REFERENCES TrainingForm(Id),
FOREIGN KEY(IdUsers) REFERENCES Users(Id)
--FOREIGN KEY(IdParent) REFERENCES Parent(Id)
GO



ALTER TABLE  Teacher
ADD FOREIGN KEY(IdUsers) REFERENCES Users(Id)
GO



ALTER TABLE  Admin
ADD FOREIGN KEY(IdUsers) REFERENCES Users(Id)
GO



ALTER TABLE  Faculty_TrainingForm
ADD FOREIGN KEY(IdTrainingForm) REFERENCES TrainingForm(Id),
 FOREIGN KEY(IdFaculty) REFERENCES Faculty(Id)
 GO



ALTER TABLE  StudyResult
ADD FOREIGN KEY(IdStudent) REFERENCES Student(Id),
 FOREIGN KEY(IdSubjectClass) REFERENCES SubjectClass(Id)
 GO



ALTER TABLE  Class
ADD FOREIGN KEY(IdTrainingForm) REFERENCES TrainingForm(Id),
 FOREIGN KEY(IdFaculty) REFERENCES Faculty(Id),
 FOREIGN KEY(IdTeacher) REFERENCES Teacher(Id),
 FOREIGN KEY(IdThumbnail) REFERENCES DatabaseImageTable(Id)
 GO



ALTER TABLE SubjectClass ADD
FOREIGN KEY (IdSubject) REFERENCES Subject(Id),
FOREIGN KEY (IdSemester) REFERENCES Semester(Id),
FOREIGN KEY(IdThumbnail) REFERENCES DatabaseImageTable(Id),
FOREIGN KEY (IdTrainingForm) REFERENCES TrainingForm(Id)
GO



ALTER TABLE Examination ADD
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id)
GO



ALTER TABLE Teacher_SubjectClass ADD
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id),
FOREIGN KEY (IdTeacher) REFERENCES Teacher(Id)
GO



ALTER TABLE CourseRegister ADD
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id),
FOREIGN KEY (IdStudent) REFERENCES Student(Id),
FOREIGN KEY (IdSemester) REFERENCES Semester(Id)
GO



ALTER TABLE TrainingScore ADD
FOREIGN KEY (IdStudent) REFERENCES Student(Id),
FOREIGN KEY (IdSemester) REFERENCES Semester(Id)
GO


ALTER TABLE ComponentScore ADD
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id)
GO


ALTER TABLE DetailScore ADD
FOREIGN KEY (IdStudent) REFERENCES Student(Id),
FOREIGN KEY (IdComponentScore) REFERENCES ComponentScore(Id)
GO

ALTER TABLE Document ADD
FOREIGN KEY (IdPoster) REFERENCES Users(Id),
FOREIGN KEY (IdFolder) REFERENCES Folder(Id),
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id)
GO


ALTER TABLE Folder ADD
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id),
FOREIGN KEY (IdPoster) REFERENCES Users(Id)
GO

ALTER TABLE AbsentCalendar ADD
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id)
GO

ALTER TABLE Notification ADD
FOREIGN KEY (IdPoster) REFERENCES Users(Id),
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id)
GO

ALTER TABLE Notification ADD
FOREIGN KEY (IdPoster) REFERENCES Users(Id),
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id),
FOREIGN KEY (IdNotificationType) REFERENCES NotificationType(Id)
GO

ALTER TABLE NotificationInfo ADD
FOREIGN KEY (IdNotification) REFERENCES Notification(Id),
FOREIGN KEY (IdUserReceiver) REFERENCES Users(Id)
GO

ALTER TABLE NotificationComment ADD
FOREIGN KEY (IdUserComment) REFERENCES Users(Id),
FOREIGN KEY (IdNotification) REFERENCES Notification(Id)
GO


ALTER TABLE NotificationImages ADD
FOREIGN KEY (IdNotification) REFERENCES Notification(Id),
FOREIGN KEY (IdDatabaseImageTable) REFERENCES DatabaseImageTable(Id)
GO


--INSERT
INSERT INTO dbo.Subject
  (Code, DisplayName, Credit, Describe)
VALUES
  (N'CS106', N'Trí tuệ Nhân tạo', 4, N''),
  (N'CS116', N'Lập trình Python cho Máy học', 4, N''),
  (N'IT008', N'Lập trình Trực quan', 4, N''),
  (N'CS336', N'Truy vấn thông tin đa phương tiện', 4, N'')
GO

INSERT INTO dbo.Semester
  (DisplayName, Batch, CourseRegisterStatus)
VALUES
  (N'Học kỳ 1', N'2019-2020', 0),
  (N'Học kỳ 2', N'2019-2020', 0),
  (N'Học kỳ 1', N'2020-2021', 0)
GO


INSERT INTO dbo.UserRole
  (Role)
VALUES
  (N'Học sinh'),
  (N'Giáo viên'),
  (N'Admin')
GO

INSERT INTO dbo.NotificationType
  (Content)
VALUES
  (N'Thông báo chung'),
  (N'Thông báo sinh viên'),
  (N'Thông báo giáo viên'),
  (N'Thông báo Admin')
GO

INSERT INTO DatabaseImageTable
 (Image)
values
 ( (SELECT *
   FROM OPENROWSET(BULK N'C:\Users\vinhq\Downloads\257208768_2117614618377866_2246121709195565683_n.jpg', SINGLE_BLOB) as T1))
-- INSERT INTO DatabaseImageTable
--   (Image)
-- values
--   ( (SELECT *
--     FROM OPENROWSET(BULK N'C:\Users\DELL\Downloads\france-in-pictures-beautiful-places-to-photograph-eiffel-tower.jpg', SINGLE_BLOB) as T1))
-- INSERT INTO Faculty
--   (DisplayName)
-- values(N'TestFaculty')
-- GO
--INSERT INTO DatabaseImageTable
--  (Image)
--values
--  ( (SELECT *
--    FROM OPENROWSET(BULK N'C:\Users\Trung\Downloads\a.png', SINGLE_BLOB) as T1))
--INSERT INTO Faculty
--  (DisplayName)
--values(N'TestFaculty')
--GO

INSERT INTO dbo.DatabaseImageTable
  (Id ,Image)
-- SELECT '52FD8086-5BD4-4365-9260-ADA8B326873C',* FROM OPENROWSET( Bulk 'C:\Users\DELL\Downloads\Picture\cat.1002.jpg', SINGLE_BLOB) rs
SELECT '52FD8086-5BD4-4365-9260-ADA8B326873C', *
FROM OPENROWSET( Bulk 'C:\Users\vinhq\Downloads\257208768_2117614618377866_2246121709195565683_n.jpg', SINGLE_BLOB) rs
-- FROM OPENROWSET( Bulk 'C:\Users\Trung\Downloads\a.png', SINGLE_BLOB) rs

INSERT INTO dbo.TrainingForm
  (Id, DisplayName)
VALUES
  ('52DF1714-C81F-42C2-8C64-8D744D787E0C', N'Cử nhân Tài năng')

INSERT INTO dbo.Faculty
  (Id, DisplayName)
VALUES
  ('3BADC66B-382B-4F35-A96C-B9B546FF98AD', N'Khoa học Máy tính')
GO

CREATE PROC USP_InsertUserWithRole
  @Role NVARCHAR(100),
  @Faculty NVARCHAR(100)
AS
BEGIN
  DECLARE @IdRole UNIQUEIDENTIFIER
  SET @IdRole = (Select id
  from UserRole
  Where Role = @Role)

  DECLARE @IdFaculty UNIQUEIDENTIFIER
  SET @IdFaculty = (Select id
  from Faculty
  Where DisplayName = @Faculty)

  DECLARE @IdAvatar UNIQUEIDENTIFIER
  SET @IdAvatar = (Select TOp 1
    (Id)
  From DatabaseImageTable)

  INSERT INTO Users
    (Username, Password, DisplayName,IdUserRole,IdFaculty,IdAvatar)
  values
    ('Admin', '1', 'Admin', @IdRole, @IdFaculty, @IdAvatar)
END
GO

USP_InsertUserWithRole @Role = 'Admin' , @Faculty = N'Khoa học Máy tính'
GO
-- select *
-- from Users
-- select *
-- from UserRole
-- select *
-- from UserRole_UserInfo
-- select *
-- from User_UserRole_UserInfo
-- select *
-- from Faculty


INSERT INTO dbo.Student
  (IdTrainingForm)
VALUES
  ('52DF1714-C81F-42C2-8C64-8D744D787E0C')



-- select * from Users
-- select * from Document
-- select * from Folder

-- delete from folder

--Insert into TrainingForm (Id, displayname) values (NEWID(), 'Oke')

BEGIN
	--DECLARE @IdFaculty UNIQUEIDENTIFIER
	--SET @IdFaculty = (Select id from Faculty Where DisplayName = 'TestFaculty')

	DECLARE @IdSubject UNIQUEIDENTIFIER
	SET @IdSubject = (Select id from Subject Where Code = N'CS106') 

	--DECLARE @IdTrainingForm UNIQUEIDENTIFIER
	--SET @IdTrainingForm = (Select id from TrainingForm Where DisplayName = 'Oke')

	DECLARE @IdAvatar UNIQUEIDENTIFIER
	SET @IdAvatar = (Select TOp 1(Id) From DatabaseImageTable)

	INSERT INTO SubjectClass(Id, IdSubject, Period, Weekday, IdThumbnail, IdTrainingForm, Code, NumberOfStudents, MaxNumberOfStudents) 
	values ('00000000-0000-0000-0000-000000000000', @IdSubject, '123', '4', @IdAvatar, '52DF1714-C81F-42C2-8C64-8D744D787E0C', 'Hi', 100, 100)
END

