--CREATE DATABASE PTS;
--GO

USE PTS;
GO

CREATE TABLE tblCustomer
(
	Id INT NOT NULL IDENTITY(1, 1),
	[Login] VARCHAR(50) NOT NULL,
	[Password] VARCHAR(64) NOT NULL,
	Name NVARCHAR(50) NOT NULL,
	Surname NVARCHAR(50) NOT NULL,
	Mode INT NOT NULL,
	CONSTRAINT PK_tblCustomers_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_tblCustomers_Login UNIQUE([Login]),
);

CREATE TABLE tblRoute
(
	Id INT NOT NULL IDENTITY(1, 1),
	Number NVARCHAR(50) NOT NULL,
	Distance INT NOT NULL,
	Price INT NOT NULL,
	Duration INT NOT NULL,
	CustomerId INT NOT NULL,
	Deleted BIT NOT NULL,
	CONSTRAINT PK_tblRoute_Id PRIMARY KEY(Id),
	CONSTRAINT FK_tblRoute_CustomerId_tblCustomer FOREIGN KEY(CustomerId) REFERENCES tblCustomer(Id),
	CONSTRAINT UQ_tblRoute_Number UNIQUE(Number),
	CONSTRAINT CK_tblRoute_Distance CHECK(Distance > 0),
	CONSTRAINT CK_tblRoute_Price CHECK(Price > 0),
	CONSTRAINT CK_tblRoute_Duration CHECK(Duration > 0)
);

CREATE TABLE tblBus
(
	Id INT NOT NULL IDENTITY(1, 1),
	Number NVARCHAR(50) NOT NULL,
	RouteId INT NOT NULL,
	Model NVARCHAR(50) NOT NULL,
	CustomerId INT NOT NULL,
	Deleted BIT NOT NULL,
	CONSTRAINT PK_tblBus_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_tblBus_Number UNIQUE(Number),
	CONSTRAINT FK_tblBus_RouteId_tblRoute FOREIGN KEY(RouteId) REFERENCES tblRoute(Id),
	CONSTRAINT FK_tblBus_CustomerId_tblCustomer FOREIGN KEY(CustomerId) REFERENCES tblCustomer(Id)
);

CREATE TABLE tblDriver
(
	Id INT NOT NULL IDENTITY(1, 1),
	BusId INT NOT NULL,
	Name NVARCHAR(50) NOT NULL,
	Surname NVARCHAR(50) NOT NULL,
	CustomerId INT NOT NULL,
	Deleted BIT NOT NULL,
	CONSTRAINT PK_tblDriver_Id PRIMARY KEY(Id),
	CONSTRAINT FK_tblDriver_BusId_tblBus FOREIGN KEY(BusId) REFERENCES tblBus(Id),
	CONSTRAINT FK_tblDriver_CustomerId_tblCustomer FOREIGN KEY(CustomerId) REFERENCES tblCustomer(Id)
);

CREATE TABLE tblBusStation
(
	Id INT NOT NULL IDENTITY(1, 1),
	Name NVARCHAR(50) NOT NULL,
	Deleted BIT NOT NULL,
	CONSTRAINT PK_tblBusStation_Id PRIMARY KEY(Id),
	CONSTRAINT UQ_tblBusStation_Name UNIQUE(Name)
);

CREATE TABLE tblRouteBusStation
(
	Id INT NOT NULL IDENTITY(1, 1),
	RouteId INT NOT NULL,
	BusStationId INT NOT NULL,
	OrderNumber INT NOT NULL,
	Deleted BIT NOT NULL,
	CONSTRAINT PK_tblRouteBusStation_Id PRIMARY KEY(Id),
	CONSTRAINT FK_tblRouteBusStation_BusStationId_tblBusStaion FOREIGN KEY(BusStationId) REFERENCES tblBusStation(Id),
	CONSTRAINT FK_tblRouteBusStation_RouteId_tblRoute FOREIGN KEY(RouteId) REFERENCES tblRoute(Id),
	CONSTRAINT CK_tblRouteBusStation_OrderNumber CHECK(OrderNumber >= 0)
);

CREATE TABLE tblTimeTable
(
	Id INT NOT NULL IDENTITY(1, 1),
	BusId INT NOT NULL,
	DepartureTime TIME NOT NULL,
	Deleted BIT NOT NULL,
	CONSTRAINT PK_tblTimeTable_Id PRIMARY KEY(Id),
	CONSTRAINT FK_tblTimeTable_BusId_tblRoute FOREIGN KEY(BusId) REFERENCES tblBus(Id)
);