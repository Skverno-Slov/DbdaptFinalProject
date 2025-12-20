USE [master]
GO
/****** Object:  Database [dbdaptFinalProject]    Script Date: 20.12.2025 4:20:46 ******/
CREATE DATABASE [dbdaptFinalProject]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'dbdaptFinalProject', FILENAME = N'C:\Users\THUNDER\dbdaptFinalProject.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'dbdaptFinalProject_log', FILENAME = N'C:\Users\THUNDER\dbdaptFinalProject_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [dbdaptFinalProject] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [dbdaptFinalProject].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [dbdaptFinalProject] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET ARITHABORT OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [dbdaptFinalProject] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [dbdaptFinalProject] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET  DISABLE_BROKER 
GO
ALTER DATABASE [dbdaptFinalProject] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [dbdaptFinalProject] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [dbdaptFinalProject] SET  MULTI_USER 
GO
ALTER DATABASE [dbdaptFinalProject] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [dbdaptFinalProject] SET DB_CHAINING OFF 
GO
ALTER DATABASE [dbdaptFinalProject] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [dbdaptFinalProject] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [dbdaptFinalProject] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [dbdaptFinalProject] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [dbdaptFinalProject] SET QUERY_STORE = OFF
GO
USE [dbdaptFinalProject]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[OrderedPrice] [decimal](8, 2) NOT NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufacturer]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufacturer](
	[ManufacturerId] [int] IDENTITY(1,1) NOT NULL,
	[Manufacturer] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Manufacturer] PRIMARY KEY CLUSTERED 
(
	[ManufacturerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrderDate] [date] NOT NULL,
	[DeliveryDate] [date] NOT NULL,
	[UserId] [int] NOT NULL,
	[ReceiveCode] [smallint] NOT NULL,
	[StatusId] [tinyint] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person](
	[PersonId] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [nvarchar](6) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[UnitId] [tinyint] NOT NULL,
	[Price] [decimal](8, 2) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[ManufacturerId] [int] NOT NULL,
	[CategoryId] [tinyint] NOT NULL,
	[Discount] [tinyint] NOT NULL,
	[StoredQuantity] [int] NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Photo] [nvarchar](50) NULL,
 CONSTRAINT [PK_Product_1] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Unit]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Unit](
	[UnitId] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED 
(
	[UnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 20.12.2025 4:20:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[HashPassword] [nvarchar](100) NOT NULL,
	[RoleId] [tinyint] NOT NULL,
	[PersonId] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 
GO
INSERT [dbo].[Category] ([CategoryId], [Name]) VALUES (1, N'Женская обувь')
GO
INSERT [dbo].[Category] ([CategoryId], [Name]) VALUES (2, N'Мужская обувь')
GO
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Item] ON 
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (39, 1, 1, 2, CAST(4990.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (40, 1, 2, 2, CAST(3244.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (41, 2, 3, 1, CAST(4499.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (42, 3, 5, 10, CAST(3800.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (43, 4, 7, 5, CAST(2700.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (44, 4, 8, 4, CAST(1890.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (45, 4, 4, 1, CAST(5900.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (46, 5, 1, 2, CAST(4990.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (47, 5, 2, 2, CAST(3244.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (48, 6, 3, 1, CAST(4499.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (49, 6, 4, 1, CAST(5900.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (50, 7, 5, 3, CAST(3800.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (51, 7, 6, 10, CAST(4100.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (52, 8, 7, 5, CAST(2700.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (53, 8, 8, 4, CAST(1890.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (54, 9, 9, 5, CAST(4300.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (55, 9, 10, 1, CAST(2800.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (56, 9, 6, 1, CAST(4100.00 AS Decimal(8, 2)))
GO
INSERT [dbo].[Item] ([ItemId], [OrderId], [ProductId], [Quantity], [OrderedPrice]) VALUES (57, 10, 11, 5, CAST(2156.00 AS Decimal(8, 2)))
GO
SET IDENTITY_INSERT [dbo].[Item] OFF
GO
SET IDENTITY_INSERT [dbo].[Manufacturer] ON 
GO
INSERT [dbo].[Manufacturer] ([ManufacturerId], [Manufacturer]) VALUES (1, N'Kari')
GO
INSERT [dbo].[Manufacturer] ([ManufacturerId], [Manufacturer]) VALUES (2, N'Marco Tozzi')
GO
INSERT [dbo].[Manufacturer] ([ManufacturerId], [Manufacturer]) VALUES (3, N'Рос')
GO
INSERT [dbo].[Manufacturer] ([ManufacturerId], [Manufacturer]) VALUES (4, N'Rieker')
GO
INSERT [dbo].[Manufacturer] ([ManufacturerId], [Manufacturer]) VALUES (5, N'Alessio Nesca')
GO
INSERT [dbo].[Manufacturer] ([ManufacturerId], [Manufacturer]) VALUES (6, N'CROSBY')
GO
SET IDENTITY_INSERT [dbo].[Manufacturer] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (1, CAST(N'2025-04-20' AS Date), CAST(N'2025-04-20' AS Date), 4, 901, 1)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (2, CAST(N'2022-09-28' AS Date), CAST(N'2025-04-21' AS Date), 1, 902, 1)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (3, CAST(N'2025-03-21' AS Date), CAST(N'2025-04-22' AS Date), 2, 903, 1)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (4, CAST(N'2025-02-20' AS Date), CAST(N'2025-04-23' AS Date), 3, 904, 1)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (5, CAST(N'2025-03-17' AS Date), CAST(N'2025-04-24' AS Date), 4, 905, 1)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (6, CAST(N'2025-03-01' AS Date), CAST(N'2025-04-25' AS Date), 1, 906, 1)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (7, CAST(N'2025-03-02' AS Date), CAST(N'2025-04-26' AS Date), 2, 907, 1)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (8, CAST(N'2025-03-31' AS Date), CAST(N'2025-04-27' AS Date), 3, 908, 2)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (9, CAST(N'2025-04-02' AS Date), CAST(N'2025-04-28' AS Date), 4, 909, 2)
GO
INSERT [dbo].[Order] ([OrderId], [OrderDate], [DeliveryDate], [UserId], [ReceiveCode], [StatusId]) VALUES (10, CAST(N'2025-04-03' AS Date), CAST(N'2025-04-29' AS Date), 4, 910, 2)
GO
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
SET IDENTITY_INSERT [dbo].[Person] ON 
GO
INSERT [dbo].[Person] ([PersonId], [LastName], [FirstName], [MiddleName]) VALUES (1, N'Никифорова', N'Весения', N'Николаевна')
GO
INSERT [dbo].[Person] ([PersonId], [LastName], [FirstName], [MiddleName]) VALUES (2, N'Сазонов', N'Руслан', N'Германович')
GO
INSERT [dbo].[Person] ([PersonId], [LastName], [FirstName], [MiddleName]) VALUES (3, N'Одинцов', N'Серафим', N'Артёмович')
GO
INSERT [dbo].[Person] ([PersonId], [LastName], [FirstName], [MiddleName]) VALUES (4, N'Степанов', N'Михаил', N'Артёмович')
GO
INSERT [dbo].[Person] ([PersonId], [LastName], [FirstName], [MiddleName]) VALUES (5, N'Ворсин', N'Петр', N'Евгеньевич')
GO
INSERT [dbo].[Person] ([PersonId], [LastName], [FirstName], [MiddleName]) VALUES (6, N'Старикова', N'Елена', N'Павловна')
GO
INSERT [dbo].[Person] ([PersonId], [LastName], [FirstName], [MiddleName]) VALUES (7, N'Михайлюк', N'Анна', N'Вячеславовна')
GO
INSERT [dbo].[Person] ([PersonId], [LastName], [FirstName], [MiddleName]) VALUES (8, N'Ситдикова', N'Елена', N'Анатольевна')
GO
SET IDENTITY_INSERT [dbo].[Person] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (1, N'А112Т4', N'Ботинки', 1, CAST(4990.00 AS Decimal(8, 2)), 1, 1, 1, 3, 6, N'Женские Ботинки демисезонные kari', N'1.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (2, N'F635R4', N'Ботинки', 1, CAST(3244.00 AS Decimal(8, 2)), 2, 2, 1, 2, 13, N'Ботинки Marco Tozzi женские демисезонные, размер 39, цвет бежевый', N'2.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (3, N'H782T5', N'Туфли', 1, CAST(4499.00 AS Decimal(8, 2)), 1, 1, 2, 5, 5, N'Туфли kari мужские классика MYZ21AW-450A, размер 43, цвет: черный', N'3.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (4, N'G783F5', N'Ботинки', 1, CAST(5900.00 AS Decimal(8, 2)), 1, 3, 2, 2, 8, N'Мужские ботинки Рос-Обувь кожаные с натуральным мехом', N'4.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (5, N'J384T6', N'Ботинки', 1, CAST(3800.00 AS Decimal(8, 2)), 2, 4, 2, 0, 16, N'B3430/14 Полуботинки мужские Rieker', N'5.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (6, N'D572U8', N'Кроссовки', 1, CAST(4100.00 AS Decimal(8, 2)), 2, 3, 2, 3, 0, N'129615-4 Кроссовки мужские', N'6.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (7, N'F572H7', N'Туфли', 1, CAST(2700.00 AS Decimal(8, 2)), 1, 2, 1, 2, 14, N'Туфли Marco Tozzi женские летние, размер 39, цвет черный', N'7.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (8, N'D329H3', N'Полуботинки', 1, CAST(1890.00 AS Decimal(8, 2)), 2, 5, 1, 0, 4, N'Полуботинки Alessio Nesca женские 3-30797-47, размер 37, цвет: бордовый', N'8.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (9, N'B320R5', N'Туфли', 1, CAST(4300.00 AS Decimal(8, 2)), 1, 4, 1, 2, 6, N'Туфли Rieker женские демисезонные, размер 41, цвет коричневый', N'9.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (10, N'G432E4', N'Туфли', 1, CAST(2800.00 AS Decimal(8, 2)), 1, 1, 1, 0, 15, N'Туфли kari женские TR-YR-413017, размер 37, цвет: черный', N'10.jpg')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (11, N'S213E3', N'Полуботинки', 1, CAST(2156.00 AS Decimal(8, 2)), 2, 6, 2, 0, 6, N'407700/01-01 Полуботинки мужские CROSBY', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (12, N'E482R4', N'Полуботинки', 1, CAST(1800.00 AS Decimal(8, 2)), 1, 1, 1, 2, 14, N'Полуботинки kari женские MYZ20S-149, размер 41, цвет: черный', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (13, N'S634B5', N'Кеды', 1, CAST(5500.00 AS Decimal(8, 2)), 2, 6, 2, 3, 0, N'Кеды Caprice мужские демисезонные, размер 42, цвет черный', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (14, N'K345R4', N'Полуботинки', 1, CAST(2100.00 AS Decimal(8, 2)), 2, 6, 2, 2, 3, N'407700/01-02 Полуботинки мужские CROSBY', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (15, N'O754F4', N'Туфли', 1, CAST(5400.00 AS Decimal(8, 2)), 2, 2, 1, 4, 18, N'Туфли женские демисезонные Rieker артикул 55073-68/37', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (16, N'G531F4', N'Ботинки', 1, CAST(6600.00 AS Decimal(8, 2)), 1, 1, 1, 0, 9, N'Ботинки женские зимние ROMER арт. 893167-01 Черный', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (17, N'J542F5', N'Тапочки', 1, CAST(500.00 AS Decimal(8, 2)), 1, 1, 2, 0, 0, N'Тапочки мужские Арт.70701-55-67', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (18, N'B431R5', N'Ботинки', 1, CAST(2700.00 AS Decimal(8, 2)), 2, 4, 2, 2, 5, N'Мужские кожаные ботинки/мужские ботинки', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (19, N'P764G4', N'Туфли', 1, CAST(6800.00 AS Decimal(8, 2)), 1, 6, 1, 10, 15, N'Туфли женские, ARGO', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (20, N'C436G5', N'Ботинки', 1, CAST(10200.00 AS Decimal(8, 2)), 1, 5, 1, 0, 0, N'Ботинки женские, ARGO, размер 40', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (21, N'F427R5', N'Ботинки', 1, CAST(11800.00 AS Decimal(8, 2)), 2, 4, 1, 15, 0, N'Ботинки на молнии с декоративной пряжкой FRAU', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (22, N'N457T5', N'Полуботинки', 1, CAST(4600.00 AS Decimal(8, 2)), 1, 6, 1, 3, 13, N'Полуботинки Ботинки черные зимние, мех', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (23, N'D364R4', N'Туфли', 1, CAST(12400.00 AS Decimal(8, 2)), 1, 1, 1, 0, 5, N'Туфли Luiza Belly женские Kate-lazo черные из натуральной замши', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (24, N'S326R5', N'Тапочки', 1, CAST(9900.00 AS Decimal(8, 2)), 2, 6, 2, 3, 15, N'Мужские кожаные тапочки "Профиль С.Дали" ', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (25, N'L754R4', N'Полуботинки', 1, CAST(1700.00 AS Decimal(8, 2)), 1, 1, 1, 2, 7, N'Полуботинки kari женские WB2020SS-26, размер 38, цвет: черный', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (26, N'M542T5', N'Кроссовки', 1, CAST(2800.00 AS Decimal(8, 2)), 2, 2, 2, 20, 3, N'Кроссовки мужские TOFA', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (27, N'D268G5', N'Туфли', 1, CAST(4399.00 AS Decimal(8, 2)), 2, 4, 1, 0, 12, N'Туфли Rieker женские демисезонные, размер 36, цвет коричневый', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (28, N'T324F5', N'Сапоги', 1, CAST(4699.00 AS Decimal(8, 2)), 1, 6, 1, 2, 5, N'Сапоги замша Цвет: синий', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (29, N'K358H6', N'Тапочки', 1, CAST(599.00 AS Decimal(8, 2)), 1, 4, 2, 20, 2, N'Тапочки мужские син р.41', N'')
GO
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [UnitId], [Price], [SupplierId], [ManufacturerId], [CategoryId], [Discount], [StoredQuantity], [Description], [Photo]) VALUES (30, N'H535R5', N'Ботинки', 1, CAST(2300.00 AS Decimal(8, 2)), 2, 4, 1, 0, 7, N'Женские Ботинки демисезонные', N'')
GO
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
GO
INSERT [dbo].[Role] ([RoleId], [Name]) VALUES (1, N'Администратор')
GO
INSERT [dbo].[Role] ([RoleId], [Name]) VALUES (2, N'Менеджер')
GO
INSERT [dbo].[Role] ([RoleId], [Name]) VALUES (3, N'Клиент')
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
SET IDENTITY_INSERT [dbo].[Status] ON 
GO
INSERT [dbo].[Status] ([StatusId], [Name]) VALUES (1, N'Завершен')
GO
INSERT [dbo].[Status] ([StatusId], [Name]) VALUES (2, N'Новый ')
GO
SET IDENTITY_INSERT [dbo].[Status] OFF
GO
SET IDENTITY_INSERT [dbo].[Supplier] ON 
GO
INSERT [dbo].[Supplier] ([SupplierId], [Name]) VALUES (1, N'Kari')
GO
INSERT [dbo].[Supplier] ([SupplierId], [Name]) VALUES (2, N'Обувь для вас')
GO
SET IDENTITY_INSERT [dbo].[Supplier] OFF
GO
SET IDENTITY_INSERT [dbo].[Unit] ON 
GO
INSERT [dbo].[Unit] ([UnitId], [Name]) VALUES (1, N'шт.')
GO
SET IDENTITY_INSERT [dbo].[Unit] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (1, N'94d5ous@gmail.com', N'$2a$12$X33dpStLC7oczKz4ku0DF..Ehax/fa3B4RCxMjYL0aoGISFLjRarO', 1, 1)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (2, N'uth4iz@mail.com', N'$2a$12$z1g6wv6P.pwQSmkAz5/HHeXT5Fk/Olmbtm5a5LZN9HTGHbVVXD3t2', 1, 2)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (3, N'yzls62@outlook.com', N'$2a$12$y99zGqXWR6znynTW/elUlebZ.O9eVuxyyG0K7IonqSNZYmfhcdxu2', 1, 3)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (4, N'1diph5e@tutanota.com', N'$2a$12$JPFzhVQMM2YKhj9NimZAEeWEvEtvyjnKjpfQ/fBBIcnR0mVRoZ99S', 2, 4)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (5, N'tjde7c@yahoo.com', N'$2a$12$Aw7KWfPW1fHBGzYqhA4PMun9pkqZlkgF7CS6v3jG/x/AWU4OhBLHG', 2, 5)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (6, N'wpmrc3do@tutanota.com', N'$2a$12$xfcxm3Wof6P35/LmTqi/cemEJtn8jxB2Sm3uns2kv0/jkI8HycEPK', 2, 6)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (7, N'5d4zbu@tutanota.com', N'$2a$12$hVg6krpsmDoQOQbBVNo3wulFZsThsYzgeudaGpKcpLlKRcrtH0Dtq', 3, 7)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (8, N'ptec8ym@yahoo.com', N'$2a$12$GBBDSmZmP7pFvxzo9PRE/u50R2m.TJTmPQVxchCQ/TizLOOyojW2G', 3, 8)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (9, N'1qz4kw@mail.com', N'$2a$12$VOj71r92DrnzApXkOg6CxuPvjDNNOXaCeq0p8GjVbZKXREl72fXr2', 3, 5)
GO
INSERT [dbo].[User] ([UserId], [Login], [HashPassword], [RoleId], [PersonId]) VALUES (10, N'4np6se@mail.com', N'$2a$12$l0CBsCc5X1TaLnvVXG8QHeJfNHxG1saXN4RYKXjJZq7KCRnhSiBnC', 3, 6)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_User_Login]    Script Date: 20.12.2025 4:20:46 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [UQ_User_Login] UNIQUE NONCLUSTERED 
(
	[Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Item] ADD  CONSTRAINT [DF_Item_Quantity]  DEFAULT ((1)) FOR [Quantity]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([OrderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Order]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Product]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Status]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_User]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Manufacturer] FOREIGN KEY([ManufacturerId])
REFERENCES [dbo].[Manufacturer] ([ManufacturerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Manufacturer]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Supplier] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([SupplierId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Supplier]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Unit] FOREIGN KEY([UnitId])
REFERENCES [dbo].[Unit] ([UnitId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Unit]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Person] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Person]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
USE [master]
GO
ALTER DATABASE [dbdaptFinalProject] SET  READ_WRITE 
GO
