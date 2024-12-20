USE [SamstedTest]
GO
/****** Object:  Table [dbo].[CourseRooms]    Script Date: 17-12-2024 21:49:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseRooms](
	[CourseRoomID] [int] IDENTITY(1,1) NOT NULL,
	[CourseRoomName] [nvarchar](50) NOT NULL,
	[EventPackage] [nvarchar](100) NOT NULL,
	[TotalPrice] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CourseRoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 17-12-2024 21:49:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[TLF] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReservationCourseRoom]    Script Date: 17-12-2024 21:49:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReservationCourseRoom](
	[ReservationID] [int] NOT NULL,
	[CourseRoomID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ReservationID] ASC,
	[CourseRoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReservationRoom]    Script Date: 17-12-2024 21:49:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReservationRoom](
	[RoomID] [int] NOT NULL,
	[ReservationID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC,
	[ReservationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservations]    Script Date: 17-12-2024 21:49:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservations](
	[ReservationID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[Created] [datetime] NULL,
	[Updated] [datetime] NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[BookingType] [nvarchar](50) NULL,
	[TotalAmount] [decimal](10, 2) NOT NULL,
	[Status] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ReservationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 17-12-2024 21:49:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[RoomID] [int] IDENTITY(1,1) NOT NULL,
	[RoomName] [nvarchar](100) NOT NULL,
	[RoomTypeID] [int] NOT NULL,
	[Status] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoomType]    Script Date: 17-12-2024 21:49:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoomType](
	[RoomTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PricePerNight] [decimal](10, 2) NOT NULL,
	[Capacity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CourseRooms] ON 

INSERT [dbo].[CourseRooms] ([CourseRoomID], [CourseRoomName], [EventPackage], [TotalPrice]) VALUES (1, N'Formiddagspakke', N'Lokale og spisning', CAST(325.00 AS Decimal(10, 2)))
INSERT [dbo].[CourseRooms] ([CourseRoomID], [CourseRoomName], [EventPackage], [TotalPrice]) VALUES (2, N'Kvik mødepakke frokost', N'Lokale og spisning', CAST(185.00 AS Decimal(10, 2)))
INSERT [dbo].[CourseRooms] ([CourseRoomID], [CourseRoomName], [EventPackage], [TotalPrice]) VALUES (3, N'Dagspakke', N'Lokale og spisning', CAST(495.00 AS Decimal(10, 2)))
INSERT [dbo].[CourseRooms] ([CourseRoomID], [CourseRoomName], [EventPackage], [TotalPrice]) VALUES (4, N'Udvidet dagspakke', N'Lokale og spisning', CAST(795.00 AS Decimal(10, 2)))
INSERT [dbo].[CourseRooms] ([CourseRoomID], [CourseRoomName], [EventPackage], [TotalPrice]) VALUES (5, N'Eftermiddagspakke', N'Lokale og spisning', CAST(325.00 AS Decimal(10, 2)))
INSERT [dbo].[CourseRooms] ([CourseRoomID], [CourseRoomName], [EventPackage], [TotalPrice]) VALUES (6, N'Kvik mødepakke eftermiddag', N'Lokale og spisning', CAST(135.00 AS Decimal(10, 2)))
INSERT [dbo].[CourseRooms] ([CourseRoomID], [CourseRoomName], [EventPackage], [TotalPrice]) VALUES (7, N'Aftenpakke', N'Lokale og spisning', CAST(425.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[CourseRooms] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 

INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email], [TLF]) VALUES (1, N'Thomas', N'Blachman', N'blachman@gmail.com', N'31889910')
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email], [TLF]) VALUES (2, N'Simon', N'Test', N'test@gmail.com', N'99112233')
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email], [TLF]) VALUES (5, N'Test1', N'Test1', N'test1@gmail.com', N'33991100')
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email], [TLF]) VALUES (7, N'test3', N'test3', N'gnanhanha@gmail.com', N'928190281')
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email], [TLF]) VALUES (9, N'Simon', N'Th', N'th@gmail.com', N'11111111')
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email], [TLF]) VALUES (11, N'nhgan', N'bhanmhnba', N'nbhan@gmail.com', N'92718271')
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Reservations] ON 

INSERT [dbo].[Reservations] ([ReservationID], [CustomerID], [Created], [Updated], [StartDate], [EndDate], [BookingType], [TotalAmount], [Status]) VALUES (12, 1, CAST(N'2024-12-13T22:22:26.507' AS DateTime), CAST(N'2024-12-13T22:22:26.507' AS DateTime), CAST(N'2024-12-13T22:22:14.240' AS DateTime), CAST(N'2024-12-14T22:22:14.240' AS DateTime), N'Ferielejlighed, Udvidet dagspakke', CAST(2095.00 AS Decimal(10, 2)), N'Booked')
INSERT [dbo].[Reservations] ([ReservationID], [CustomerID], [Created], [Updated], [StartDate], [EndDate], [BookingType], [TotalAmount], [Status]) VALUES (13, 1, CAST(N'2024-12-13T22:22:33.260' AS DateTime), CAST(N'2024-12-13T22:22:33.260' AS DateTime), CAST(N'2024-12-13T22:22:14.240' AS DateTime), CAST(N'2024-12-14T22:22:14.240' AS DateTime), N'Ferielejlighed, Kvik mødepakke frokost', CAST(1485.00 AS Decimal(10, 2)), N'Booked')
INSERT [dbo].[Reservations] ([ReservationID], [CustomerID], [Created], [Updated], [StartDate], [EndDate], [BookingType], [TotalAmount], [Status]) VALUES (14, 1, CAST(N'2024-12-13T22:22:39.420' AS DateTime), CAST(N'2024-12-13T22:22:39.420' AS DateTime), CAST(N'2024-12-13T22:22:14.240' AS DateTime), CAST(N'2024-12-14T22:22:14.240' AS DateTime), N'Ferielejlighed, Kvik mødepakke frokost', CAST(1485.00 AS Decimal(10, 2)), N'Booked')
INSERT [dbo].[Reservations] ([ReservationID], [CustomerID], [Created], [Updated], [StartDate], [EndDate], [BookingType], [TotalAmount], [Status]) VALUES (15, 5, CAST(N'2024-12-15T14:11:23.380' AS DateTime), CAST(N'2024-12-15T14:11:23.380' AS DateTime), CAST(N'2024-12-15T14:04:08.020' AS DateTime), CAST(N'2024-12-16T14:04:08.020' AS DateTime), N'Ferielejlighed, Kvik mødepakke frokost', CAST(1485.00 AS Decimal(10, 2)), N'Booked')
INSERT [dbo].[Reservations] ([ReservationID], [CustomerID], [Created], [Updated], [StartDate], [EndDate], [BookingType], [TotalAmount], [Status]) VALUES (16, 2, CAST(N'2024-12-16T20:17:35.407' AS DateTime), CAST(N'2024-12-16T20:17:35.407' AS DateTime), CAST(N'2024-12-16T20:17:28.743' AS DateTime), CAST(N'2024-12-17T20:17:28.743' AS DateTime), N'Ferielejlighed, Dagspakke', CAST(1795.00 AS Decimal(10, 2)), N'Booked')
SET IDENTITY_INSERT [dbo].[Reservations] OFF
GO
SET IDENTITY_INSERT [dbo].[Rooms] ON 

INSERT [dbo].[Rooms] ([RoomID], [RoomName], [RoomTypeID], [Status]) VALUES (1, N'Ferielejlighed', 1, N'Available')
INSERT [dbo].[Rooms] ([RoomID], [RoomName], [RoomTypeID], [Status]) VALUES (2, N'Dobbeltværelse', 2, N'Available')
INSERT [dbo].[Rooms] ([RoomID], [RoomName], [RoomTypeID], [Status]) VALUES (3, N'Familieværelse', 3, N'Available')
SET IDENTITY_INSERT [dbo].[Rooms] OFF
GO
SET IDENTITY_INSERT [dbo].[RoomType] ON 

INSERT [dbo].[RoomType] ([RoomTypeID], [Name], [PricePerNight], [Capacity]) VALUES (1, N'Ferielejlighed', CAST(1300.00 AS Decimal(10, 2)), 4)
INSERT [dbo].[RoomType] ([RoomTypeID], [Name], [PricePerNight], [Capacity]) VALUES (2, N'Dobbeltværelse', CAST(1000.00 AS Decimal(10, 2)), 2)
INSERT [dbo].[RoomType] ([RoomTypeID], [Name], [PricePerNight], [Capacity]) VALUES (3, N'Familieværelse', CAST(1200.00 AS Decimal(10, 2)), 4)
SET IDENTITY_INSERT [dbo].[RoomType] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__CourseRo__AE2DACBD10B07FFD]    Script Date: 17-12-2024 21:49:32 ******/
ALTER TABLE [dbo].[CourseRooms] ADD UNIQUE NONCLUSTERED 
(
	[CourseRoomName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Customer__A9D105343572DD2B]    Script Date: 17-12-2024 21:49:32 ******/
ALTER TABLE [dbo].[Customers] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Rooms__6B500B556177E18C]    Script Date: 17-12-2024 21:49:32 ******/
ALTER TABLE [dbo].[Rooms] ADD UNIQUE NONCLUSTERED 
(
	[RoomName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Reservations] ADD  DEFAULT (getdate()) FOR [Created]
GO
ALTER TABLE [dbo].[Reservations] ADD  DEFAULT (getdate()) FOR [Updated]
GO
ALTER TABLE [dbo].[Reservations] ADD  DEFAULT ('Booked') FOR [Status]
GO
ALTER TABLE [dbo].[Rooms] ADD  DEFAULT ('Available') FOR [Status]
GO
ALTER TABLE [dbo].[ReservationCourseRoom]  WITH CHECK ADD FOREIGN KEY([CourseRoomID])
REFERENCES [dbo].[CourseRooms] ([CourseRoomID])
GO
ALTER TABLE [dbo].[ReservationCourseRoom]  WITH CHECK ADD FOREIGN KEY([ReservationID])
REFERENCES [dbo].[Reservations] ([ReservationID])
GO
ALTER TABLE [dbo].[ReservationRoom]  WITH CHECK ADD FOREIGN KEY([ReservationID])
REFERENCES [dbo].[Reservations] ([ReservationID])
GO
ALTER TABLE [dbo].[ReservationRoom]  WITH CHECK ADD FOREIGN KEY([RoomID])
REFERENCES [dbo].[Rooms] ([RoomID])
GO
ALTER TABLE [dbo].[Reservations]  WITH CHECK ADD FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([CustomerID])
GO
ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD FOREIGN KEY([RoomTypeID])
REFERENCES [dbo].[RoomType] ([RoomTypeID])
GO
