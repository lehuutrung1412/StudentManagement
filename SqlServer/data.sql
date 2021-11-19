-- USE TEMP
-- DROP DATABASE StudentManagement
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
  DayOfBirth DATETIME,
  Gender INT,
  Email NVARCHAR(MAX),
  PhoneNumber NVARCHAR(MAX),
  Online BIT DEFAULT 0,
  -- 1: online, 0: offline
  Roles INT,
  IdFaculty UNIQUEIDENTIFIER NOT NULL,
  IdAvatar UNIQUEIDENTIFIER NOT NULL,

)
GO

CREATE TABLE Parent
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  NameDad NVARCHAR(MAX),
  NameMom NVARCHAR(MAX),
  AddressDad NVARCHAR(MAX),
  AddressMom NVARCHAR(MAX),
  PhoneDad NVARCHAR(MAX),
  PhoneMom NVARCHAR(MAX),
  JobDad NVARCHAR(MAX),
  JobMom NVARCHAR(MAX),
)
GO

CREATE TABLE Student
(
  Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
  IdTrainingForm UNIQUEIDENTIFIER NOT NULL,
  Status INT DEFAULT 1,
  -- 1: còn học, 0: đã tốt nghiệp
  IdUsers UNIQUEIDENTIFIER,
  IdParent UNIQUEIDENTIFIER
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
  IdTrainingForm UNIQUEIDENTIFIER NOT NULL,
  IdFaculty UNIQUEIDENTIFIER NOT NULL,
  IsDeleted BIT DEFAULT 0,

  PRIMARY KEY (IdTrainingForm,IdFaculty),
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
  Id UNIQUEIDENTIFIER PRIMARY KEY,
  DisplayName NVARCHAR(MAX),
  Content NVARCHAR(MAX),
  CreatedAt DateTime,
  IdPoster UNIQUEIDENTIFIER NOT NULL,
  IdFolder UNIQUEIDENTIFIER,
  IdSubjectClass UNIQUEIDENTIFIER NOT NULL,
)
GO


CREATE TABLE Folder
(
  Id UNIQUEIDENTIFIER PRIMARY KEY,
  DisplayName NVARCHAR(MAX),
  IdSubjectClass UNIQUEIDENTIFIER NOT NULL,
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
  Id UNIQUEIDENTIFIER PRIMARY KEY,
  Image VARBINARY(max),
)
GO



-- foreign key
ALTER TABLE  Users
ADD FOREIGN KEY (IdFaculty) REFERENCES Faculty(Id),
FOREIGN KEY (IdAvatar) REFERENCES DatabaseImageTable(Id)
GO



ALTER TABLE  Student
ADD FOREIGN KEY(IdTrainingForm) REFERENCES TrainingForm(Id),
FOREIGN KEY(IdUsers) REFERENCES Users(Id),
FOREIGN KEY(IdParent) REFERENCES Parent(Id)
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
FOREIGN KEY(IdThumbnail) REFERENCES DatabaseImageTable(Id)
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
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id)
GO

ALTER TABLE AbsentCalendar ADD
FOREIGN KEY (IdSubjectClass) REFERENCES SubjectClass(Id)
GO
