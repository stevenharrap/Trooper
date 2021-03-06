USE [master]
GO
/****** Object:  Database [TrooperTestStates2]    Script Date: 21/10/2015 11:10:16 PM ******/
CREATE DATABASE [TrooperTestStates2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TrooperTestStates2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TrooperTestStates2.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TrooperTestStates2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TrooperTestStates2_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TrooperTestStates2] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TrooperTestStates2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TrooperTestStates2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET ARITHABORT OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TrooperTestStates2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TrooperTestStates2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TrooperTestStates2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TrooperTestStates2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TrooperTestStates2] SET  MULTI_USER 
GO
ALTER DATABASE [TrooperTestStates2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TrooperTestStates2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TrooperTestStates2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TrooperTestStates2] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TrooperTestStates2] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TrooperTestStates2', N'ON'
GO
USE [TrooperTestStates2]
GO
/****** Object:  Table [dbo].[Outcome]    Script Date: 21/10/2015 11:10:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Outcome](
	[Action] [varchar](50) NOT NULL,
	[Result] [varchar](50) NOT NULL,
	[NeedsItems] [bit] NULL,
	[Pass] [bit] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Parameter]    Script Date: 21/10/2015 11:10:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Parameter](
	[Action] [varchar](50) NOT NULL,
	[Property] [varchar](50) NOT NULL,
	[State] [varchar](50) NOT NULL,
	[NeedsItems] [bit] NULL,
	[Fault] [bit] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[System]    Script Date: 21/10/2015 11:10:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[System](
	[State] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Outcome] ([Action], [Result], [NeedsItems], [Pass]) VALUES (N'Adding', N'ReportsOkAndIsAdded', 0, 1)
INSERT [dbo].[Outcome] ([Action], [Result], [NeedsItems], [Pass]) VALUES (N'All', N'ReportsErrorAndNoChange', NULL, 0)
INSERT [dbo].[Outcome] ([Action], [Result], [NeedsItems], [Pass]) VALUES (N'Adding', N'ReportsOkAndIsAddedAndOthersUnchanged', 1, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'Adding', N'Item', N'IsValidNew', NULL, 0)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'Adding', N'Item', N'IsInvalidNew', NULL, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'Adding', N'Item', N'IsValidExists', 1, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'Adding', N'Item', N'IsInvalidExists', 1, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'All', N'Identity', N'IsAllowed', NULL, 0)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'All', N'Identity', N'IsDenied', NULL, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'All', N'Identity', N'IsInvalid', NULL, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'AllValidAllNew', NULL, 0)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'AllInvalidAllNew', NULL, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'MixedValidityAllNew', NULL, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'AllValidAllExist', 1, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'AllInvalidAllExist', 1, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'MixedValidityAllExist', 1, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'AllValidMixedExistance', 1, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'AllInvalidMixedExistance', 1, 1)
INSERT [dbo].[Parameter] ([Action], [Property], [State], [NeedsItems], [Fault]) VALUES (N'AddingSome', N'Items', N'MixedValidityMixedExistance', 1, 1)
INSERT [dbo].[System] ([State]) VALUES (N'IsEmpty')
INSERT [dbo].[System] ([State]) VALUES (N'HasItems')
USE [master]
GO
ALTER DATABASE [TrooperTestStates2] SET  READ_WRITE 
GO
