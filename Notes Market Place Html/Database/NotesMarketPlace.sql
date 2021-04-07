USE [master]
GO
/****** Object:  Database [NotesMarketPLace]    Script Date: 03-04-2021 16:04:01 ******/
CREATE DATABASE [NotesMarketPLace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NotesMarketPLace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPLace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NotesMarketPLace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPLace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [NotesMarketPLace] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NotesMarketPLace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NotesMarketPLace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NotesMarketPLace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NotesMarketPLace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET  ENABLE_BROKER 
GO
ALTER DATABASE [NotesMarketPLace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NotesMarketPLace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET RECOVERY FULL 
GO
ALTER DATABASE [NotesMarketPLace] SET  MULTI_USER 
GO
ALTER DATABASE [NotesMarketPLace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NotesMarketPLace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NotesMarketPLace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NotesMarketPLace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NotesMarketPLace] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NotesMarketPLace] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'NotesMarketPLace', N'ON'
GO
ALTER DATABASE [NotesMarketPLace] SET QUERY_STORE = OFF
GO
USE [NotesMarketPLace]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__Countrie__3214EC2773C3EC15] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownloads] [bit] NOT NULL,
	[AttechmentPath] [varchar](max) NULL,
	[IsAttechmentDownloads] [bit] NOT NULL,
	[AttechmentDownloadDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
 CONSTRAINT [PK__Download__3214EC2733439BE2] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__NoteCate__3214EC272E92D6B5] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__NoteType__3214EC27A910C210] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReferenceData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[value] [varchar](100) NOT NULL,
	[DataValur] [varchar](100) NOT NULL,
	[RefCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__Referenc__3214EC27F1BF1D0E] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotes]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[PublishedDate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[NoteType] [int] NULL,
	[NumberofPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[Country] [int] NULL,
	[University] [varchar](200) NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__SellerNo__3214EC2766ED2CC1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesAttechment]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesAttechment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK__SellerNo__3214EC271AB346FA] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReportedIssues]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReportedIssues](
	[ID] [int] NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReviews]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReviews](
	[ID] [int] NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewByID] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfiguration]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfiguration](
	[ID] [int] NOT NULL,
	[Key] [varchar](100) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](100) NOT NULL,
	[Password] [varchar](24) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[ActivationCode] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__Users__3214EC27D0ADED0F] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [varchar](100) NULL,
	[Phone number-Country Code] [varchar](5) NOT NULL,
	[phonenumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[AddressLine1] [varchar](100) NOT NULL,
	[AddressLine2] [varchar](100) NOT NULL,
	[City] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[ZipCode] [varchar](50) NOT NULL,
	[Country] [varchar](50) NOT NULL,
	[University] [varchar](100) NULL,
	[Collage] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
 CONSTRAINT [PK__UserProf__3214EC2768B48559] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 03-04-2021 16:04:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[MidifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (1, N'India', N'123', CAST(N'2021-03-22T10:42:53.497' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (2, N'England', N'456', CAST(N'2021-03-22T10:43:27.833' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (1, N'IT', N'This Category is belong to IT', CAST(N'2021-03-22T10:39:39.253' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (2, N'CA', N'This Category is belong to IT', CAST(N'2021-03-22T10:40:11.907' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (3, N'MBA', N'This Category is belong to IT', CAST(N'2021-03-22T10:40:43.750' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (4, N'Science', N'This is the Science Category', CAST(N'2021-03-26T17:48:27.697' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (5, N'Science', N'This is the Science Category', CAST(N'2021-03-26T17:54:14.533' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (6, N'Commerce', N'This is the Commerce Category', CAST(N'2021-03-26T17:54:14.533' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (7, N'Social', N'This is the Social Category', CAST(N'2021-03-26T17:54:14.533' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (8, N'Lorem', N'This is the Dummy Category', CAST(N'2021-03-26T17:54:14.537' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (1, N'handWritten Books', N'this book written by hand', CAST(N'2021-03-22T10:35:38.360' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (2, N'University Notes', N'this is the University notes', CAST(N'2021-03-22T10:37:07.627' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (3, N'SelfWritten Novel', N'this is the University notes', CAST(N'2021-03-22T10:37:48.803' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ReferenceData] ON 

INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (1, N'Male', N'M', N'Gender', CAST(N'2021-03-26T17:10:36.603' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (2, N'Female', N'Fe', N'Gender', CAST(N'2021-03-26T17:10:36.833' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (3, N'Unknown', N'U', N'Gender', CAST(N'2021-03-26T17:10:36.833' AS DateTime), NULL, NULL, NULL, 0)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (4, N'Paid', N'P', N'Selling Mode', CAST(N'2021-03-26T17:10:36.833' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (5, N'Free', N'P', N'Selling Mode', CAST(N'2021-03-26T17:10:36.833' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (6, N'Draft', N'Draft', N'NotesStatus', CAST(N'2021-03-26T17:10:36.837' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (7, N'Submitted For Review', N'Submitted For Review', N'NotesStatus', CAST(N'2021-03-26T17:10:36.837' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (8, N'In Review', N'In Review', N'NotesStatus', CAST(N'2021-03-26T17:10:36.837' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (9, N'Published', N'Published', N'NotesStatus', CAST(N'2021-03-26T17:10:36.837' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (10, N'Rejected', N'Rejected', N'NotesStatus', CAST(N'2021-03-26T17:10:36.837' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [value], [DataValur], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (11, N'Removed', N'Removed', N'NotesStatus', CAST(N'2021-03-26T17:10:36.837' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[ReferenceData] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotes] ON 

INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [Country], [University], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (1, 1, 6, NULL, NULL, NULL, N'Computer Science', 4, N'~/Members/1/1/DisplayPicture/20210403-computer-science.png', 2, 200, N'hello this is the Science book', 1, N'GEC Rajkot', N'B.E', N'21061205', N'Anant', 0, NULL, N'~/Members/1/1/NotesPreview/20210403-sample.pdf', CAST(N'2021-04-03T10:57:31.463' AS DateTime), 1, CAST(N'2021-04-03T10:57:31.463' AS DateTime), 1, 0)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [Country], [University], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (2, 1, 9, NULL, NULL, NULL, N'G.S.T', 2, N'~/Members/1/2/DisplayPicture/20210403-search1.png', 2, 300, N'this is the G.S.T book', 1, N'GEC Rajkot', N'B.com', N'21061205', N'harshil', 1, CAST(500 AS Decimal(18, 0)), N'~/Members/1/2/NotesPreview/20210403-DDBMS_Unit 1_Assignment.pdf', CAST(N'2021-04-03T11:03:25.583' AS DateTime), 1, CAST(N'2021-04-03T11:03:25.583' AS DateTime), 1, 0)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [Country], [University], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (3, 1, 6, NULL, NULL, NULL, N'Social Studies', 7, N'~/Members/1/3/DisplayPicture/20210403-search3.png', 2, 200, N'This is the Social Studies book', 1, N'GEC Rajkot', N'B.com', N'21061205', N'Anant', 0, NULL, N'~/Members/1/3/NotesPreview/20210403-sample.pdf', CAST(N'2021-04-03T15:29:19.777' AS DateTime), 1, CAST(N'2021-04-03T15:29:19.777' AS DateTime), 1, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [Country], [University], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (4, 1, 6, NULL, NULL, NULL, N'Account', 6, N'~/Members/1/4/DisplayPicture/20210403-search4.png', 2, 200, N'this is the account book', 1, N'GEC Rajkot', N'B.com', N'21061205', N'harshil', 1, CAST(500 AS Decimal(18, 0)), N'~/Members/1/4/NotesPreview/20210403-sample.pdf', CAST(N'2021-04-03T15:31:03.747' AS DateTime), 1, CAST(N'2021-04-03T15:31:03.747' AS DateTime), 1, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [Country], [University], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (5, 1, 6, NULL, NULL, NULL, N'Lorem1', 8, N'~/Members/1/5/DisplayPicture/20210403-search5.png', 2, 300, N'this is the lorem', 1, N'GEC Rajkot', N'Lorem', N'21061205', N'Anant', 1, CAST(100 AS Decimal(18, 0)), N'~/Members/1/5/NotesPreview/20210403-sample.pdf', CAST(N'2021-04-03T15:32:39.713' AS DateTime), 1, CAST(N'2021-04-03T15:32:39.713' AS DateTime), 1, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [Country], [University], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (6, 1, 7, NULL, NULL, NULL, N'AI', 1, N'~/Members/1/6/DisplayPicture/20210403-computer-science.png', 2, 200, N'this is the AI book', 2, N'GEC Rajkot', N'B.E', N'abc12', N'harshil', 1, CAST(250 AS Decimal(18, 0)), N'~/Members/1/6/NotesPreview/20210403-sample.pdf', CAST(N'2021-04-03T15:36:39.320' AS DateTime), 1, CAST(N'2021-04-03T15:36:39.320' AS DateTime), 1, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionBy], [AdminRemarks], [PublishedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [Country], [University], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (7, 1, 7, NULL, NULL, NULL, N'G.S.T', 2, N'~/Members/1/7/DisplayPicture/20210403-search6.png', 2, 123, N'this is the G.S.T subject', 1, N'GEC Rajkot', N'B.com', N'abc12', N'Anant', 0, NULL, N'~/Members/1/7/NotesPreview/20210403-sample.pdf', CAST(N'2021-04-03T15:38:05.597' AS DateTime), 1, CAST(N'2021-04-03T15:38:05.597' AS DateTime), 1, 1)
SET IDENTITY_INSERT [dbo].[SellerNotes] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesAttechment] ON 

INSERT [dbo].[SellerNotesAttechment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (1, 1, N'sample.pdf', N'~/Members/1/1/Attechment/', CAST(N'2021-04-03T10:57:40.340' AS DateTime), 1, CAST(N'2021-04-03T10:57:40.340' AS DateTime), 1, 0)
INSERT [dbo].[SellerNotesAttechment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (2, 2, N'sample.pdf', N'~/Members/1/2/Attechment/', CAST(N'2021-04-03T11:03:26.237' AS DateTime), 1, CAST(N'2021-04-03T11:03:26.237' AS DateTime), 1, 0)
INSERT [dbo].[SellerNotesAttechment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (3, 3, N'sample.pdf', N'~/Members/1/3/Attechment/', CAST(N'2021-04-03T15:29:21.850' AS DateTime), 1, CAST(N'2021-04-03T15:29:21.850' AS DateTime), 1, 0)
INSERT [dbo].[SellerNotesAttechment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (4, 4, N'sample.pdf', N'~/Members/1/4/Attechment/', CAST(N'2021-04-03T15:31:03.977' AS DateTime), 1, CAST(N'2021-04-03T15:31:03.977' AS DateTime), 1, 0)
INSERT [dbo].[SellerNotesAttechment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (5, 5, N'sample.pdf', N'~/Members/1/5/Attechment/', CAST(N'2021-04-03T15:32:39.803' AS DateTime), 1, CAST(N'2021-04-03T15:32:39.803' AS DateTime), 1, 0)
INSERT [dbo].[SellerNotesAttechment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (6, 6, N'sample.pdf', N'~/Members/1/6/Attechment/', CAST(N'2021-04-03T15:36:40.043' AS DateTime), 1, CAST(N'2021-04-03T15:36:40.043' AS DateTime), 1, 0)
INSERT [dbo].[SellerNotesAttechment] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (7, 7, N'sample.pdf', N'~/Members/1/7/Attechment/', CAST(N'2021-04-03T15:38:05.660' AS DateTime), 1, CAST(N'2021-04-03T15:38:05.660' AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[SellerNotesAttechment] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive], [ActivationCode]) VALUES (1, 1, N'Harshil', N'Khandhar', N'harshilkhandhar7@gmail.com', N'12345678', 1, CAST(N'2021-03-23T09:46:03.663' AS DateTime), NULL, CAST(N'2021-04-03T11:27:09.513' AS DateTime), NULL, 0, N'17899d8b-03cc-4e25-bebb-ca0bd2966315')
INSERT [dbo].[User] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive], [ActivationCode]) VALUES (2, 1, N'Anant', N'Mehta', N's.khandhar1965@gmail.com', N'123456', 0, CAST(N'2021-03-23T09:57:54.433' AS DateTime), NULL, NULL, NULL, 0, N'95bded13-0903-4cb5-8c78-7b8fa9f50600')
SET IDENTITY_INSERT [dbo].[User] OFF
GO
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [MidifiedBy], [IsActive]) VALUES (1, N'User', N'this is the user profile role id', CAST(N'2021-03-17T16:26:06.650' AS DateTime), NULL, CAST(N'2021-03-17T16:26:06.650' AS DateTime), NULL, 1)
GO
ALTER TABLE [dbo].[Countries] ADD  CONSTRAINT [DF_Countries_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Countries] ADD  CONSTRAINT [DF_Countries_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Downloads] ADD  CONSTRAINT [DF_Downloads_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [DF_NoteCategories_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NoteCategories] ADD  CONSTRAINT [DF_NoteCategories_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [DF_NoteTypes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NoteTypes] ADD  CONSTRAINT [DF_NoteTypes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  CONSTRAINT [DF_ReferenceData_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  CONSTRAINT [DF_ReferenceData_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  CONSTRAINT [DF_SellerNotes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  CONSTRAINT [DF_SellerNotes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesAttechment] ADD  CONSTRAINT [DF_SellerNotesAttechment_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotesAttechment] ADD  CONSTRAINT [DF__SellerNot__IsAct__300424B4]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] ADD  CONSTRAINT [DF_SellerNotesReportedIssues_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotesReviews] ADD  CONSTRAINT [DF_SellerNotesReviews_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotesReviews] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SystemConfiguration] ADD  CONSTRAINT [DF_SystemConfiguration_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SystemConfiguration] ADD  CONSTRAINT [DF_SystemConfiguration_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF__Users__IsEmailVe__24927208]  DEFAULT ((0)) FOR [IsEmailVerified]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF__Users__IsActive__25869641]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserProfile] ADD  CONSTRAINT [DF_UserProfile_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [DF_UserRoles_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_SellerNotes]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_SellerNotesWithUsreTable] FOREIGN KEY([Seller])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_SellerNotesWithUsreTable]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_UsersDownloader] FOREIGN KEY([Downloader])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_UsersDownloader]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Countries] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Countries]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteCategories] FOREIGN KEY([Category])
REFERENCES [dbo].[NoteCategories] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteCategories]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteTypes] FOREIGN KEY([NoteType])
REFERENCES [dbo].[NoteTypes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteTypes]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_ReferenceData] FOREIGN KEY([Status])
REFERENCES [dbo].[ReferenceData] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_ReferenceData]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users] FOREIGN KEY([SellerID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users1] FOREIGN KEY([ActionBy])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users1]
GO
ALTER TABLE [dbo].[SellerNotesAttechment]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesAttechment_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesAttechment] CHECK CONSTRAINT [FK_SellerNotesAttechment_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Downloads] FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[Downloads] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Downloads]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Users] FOREIGN KEY([ReviewByID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Users]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Downloads] FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[Downloads] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Downloads]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Users] FOREIGN KEY([ReviewByID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Users]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [Users_RoleID_FK] FOREIGN KEY([RoleID])
REFERENCES [dbo].[UserRoles] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [Users_RoleID_FK]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_ReferenceData] FOREIGN KEY([Gender])
REFERENCES [dbo].[ReferenceData] ([ID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_ReferenceData]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_Users]
GO
USE [master]
GO
ALTER DATABASE [NotesMarketPLace] SET  READ_WRITE 
GO
