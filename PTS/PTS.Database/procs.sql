USE PTS;
GO

CREATE PROCEDURE spLogIn
	@login VARCHAR(50),
	@password VARCHAR(64)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, Name, Surname, Mode
	FROM tblCustomer
	WHERE [Login] = @login AND [Password] = @password;
END
GO

CREATE FUNCTION fnInTimeRange(@from TIME, @to TIME, @time TIME)
RETURNS BIT
AS
BEGIN
    DECLARE @result BIT;
	IF @from < @to
	BEGIN
		IF (@time >= @from AND @time <= @to)
		BEGIN
			SET @result = 1;
		END
		ELSE
		BEGIN
			SET @result = 0;
		END
	END
	ELSE
	BEGIN
		IF (@time > @to AND @time < @from)
		BEGIN
			SET @result = 0;
		END
		ELSE
		BEGIN
			SET @result = 1;
		END
	END

    RETURN @result;
END;
GO

--select all

CREATE PROCEDURE spSelectAllDrivers
AS
BEGIN
	SET NOCOUNT ON;

	SELECT d.Id, d.Name, d.Surname, b.Number as BusNumber, r.Number as RouteNumber, c.Name + ' ' + c.Surname as Customer
	FROM tblDriver d
	INNER JOIN tblBus as b
	ON d.BusId = b.Id
	INNER JOIN tblRoute r
	ON r.Id = b.RouteId
	INNER JOIN tblCustomer c
	ON c.Id = d.CustomerId
	WHERE d.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0;
END
GO

CREATE PROCEDURE spSelectAllStations
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT Id, Name
	FROM tblBusStation
	WHERE Deleted = 0;
END
GO

CREATE PROCEDURE spSelectAllRoutes
AS
BEGIN
	SET NOCOUNT ON;

	SELECT r.Id, r.Number, r.Distance, r.Price, r.Duration, c.Name +' '+ c.Surname as Customer 
	FROM tblRoute as r
	INNER JOIN tblCustomer as c
	ON c.Id = r.CustomerId
	WHERE r.Deleted = 0 
	ORDER BY r.Id;
END
GO

CREATE PROCEDURE spSelectAllBuses
AS
BEGIN
	SET NOCOUNT ON;

	SELECT b.Id, b.Number as BusNumber, r.Number as RouteNumber, b.Model, c.Name +' '+ c.Surname as Customer
	FROM tblBus as b
	INNER JOIN tblCustomer as c
	ON c.Id = b.CustomerId
	INNER JOIN tblRoute as r
	ON b.RouteId = r.Id
	WHERE b.Deleted = 0 AND r.Deleted = 0
	ORDER BY b.Id;
END
GO

CREATE PROCEDURE spSelectAllTimeTable
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime
	FROM tblTimeTable as t
	INNER JOIN tblBus as b
	ON t.BusId = b.Id
	INNER JOIN tblRoute as r
	ON b.RouteId = r.Id
	WHERE (t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0);
END
GO

--insert

CREATE PROCEDURE spInsertRoute
	@number NVARCHAR(50),
	@distance INT,
	@price INT,
	@duration INT,
	@customerId INT,
	@id INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @count INT = 0;
	SELECT @count = COUNT(Number) FROM tblRoute WHERE Number = @number AND Deleted = 0;
	
	BEGIN TRY
	IF @count > 0
		THROW 60001, 'Number is not unique', 1;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;

	SELECT @count = COUNT(Number) FROM tblRoute WHERE Number = @number AND Deleted = 1;

	IF @count = 0
	BEGIN
		INSERT INTO tblRoute(Number, Distance, Price, Duration, CustomerId, Deleted)
		VALUES(@number, @distance, @price, @duration, @customerId, 0);
		SET @id = SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE tblRoute
		SET Deleted = 0,
			Distance = @distance,
			Price = @price,
			CustomerId = @customerId,
			Duration = @duration
		WHERE Number = @number;
		SELECT @id = Id FROM tblRoute WHERE Number = @number;
	END
END
GO

CREATE PROCEDURE spInsertBus
	@busNumber NVARCHAR(50),
	@routeNumber NVARCHAR(50),
	@model NVARCHAR(50),
	@customerId INT,
	@id INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @routeCount INT = 0;

	SELECT @routeCount = COUNT(Number) FROM tblRoute WHERE Number = @routeNumber AND Deleted = 0;

	BEGIN TRY
	IF @routeCount = 0
		THROW 60000, 'Route number doesn"t exist', 1;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;

	DECLARE @routeId INT;

	SELECT @routeId = Id FROM tblRoute WHERE Number = @routeNumber;

	DECLARE @busCount INT = 0;
	SELECT @busCount = COUNT(Number) FROM tblBus WHERE Number = @busNumber AND Deleted = 0;
	
	BEGIN TRY
	IF @busCount > 0
		THROW 60001, 'Bus number is not unique', 1;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;

	SELECT @busCount = COUNT(Number) FROM tblBus WHERE Number = @busNumber AND Deleted = 1;

	IF @busCount = 0
	BEGIN
		INSERT INTO tblBus(Number, RouteId, Model, CustomerId, Deleted)
		VALUES(@busNumber, @routeId, @model, @customerId, 0);
		SET @id = SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE tblBus
		SET Deleted = 0,
			RouteId = @routeId,
			Model = @model,
			CustomerId = @customerId
		WHERE Number = @busNumber;
		SELECT @id = Id FROM tblBus WHERE Number = @busNumber;
	END
END
GO

CREATE PROCEDURE spInsertDriver
	@busNumber NVARCHAR(50),
	@name NVARCHAR(50),
	@surname NVARCHAR(50),
	@customerId INT,
	@id INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @busCount INT = 0;

	SELECT @busCount = COUNT(Number) FROM tblBus WHERE Number = @busNumber AND Deleted = 0;

	BEGIN TRY
	IF @busCount = 0
		THROW 60000, 'Bus number doesn"t exist', 1;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;

	DECLARE @busId INT;

	SELECT @busId = Id FROM tblBus WHERE Number = @busNumber;

	INSERT INTO tblDriver(BusId, Name, Surname, CustomerId, Deleted)
	VALUES(@busId, @name, @surname, @customerId, 0);

	SET @id = SCOPE_IDENTITY();
	
END
GO

CREATE PROCEDURE spInsertStation
	@name NVARCHAR(50),
	@id INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @stationCount INT = 0;

	SELECT @stationCount = COUNT(Name) FROM tblBusStation WHERE Name = @name AND Deleted = 0;
	
	BEGIN TRY
	IF @stationCount > 0
		THROW 60001, 'Station name is not unique', 1;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;

	SELECT @stationCount = COUNT(Name) FROM tblBusStation WHERE Name = @name AND Deleted = 1;

	IF @stationCount = 0
	BEGIN
		INSERT INTO tblBusStation(Name, Deleted)
		VALUES (@name, 0);
		SET @id = SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE tblBusStation
		SET Deleted = 0
		WHERE Name = @name;
		SELECT @id = Id FROM tblBusStation WHERE Name = @name;
	END
END
GO

CREATE PROCEDURE spInsertTimeTable
	@busNumber NVARCHAR(50),
	@departureTime TIME,
	@id INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @busCount INT = 0;

	SELECT @busCount = COUNT(Number) FROM tblBus WHERE Number = @busNumber AND Deleted = 0;

	BEGIN TRY
	IF @busCount = 0
		THROW 60000, 'Bus number doesn"t exist', 1;
	END TRY
	BEGIN CATCH
		THROW;
	END CATCH;

	DECLARE @busId INT;

	SELECT @busId = Id FROM tblBus WHERE Number = @busNumber;

	INSERT INTO tblTimeTable(BusId,DepartureTime, Deleted)
	VALUES(@busId, @departureTime, 0);

	SET @id = SCOPE_IDENTITY();
	
END
GO

CREATE PROCEDURE spInsertRouteStationRelation
	@routeNumber NVARCHAR(50),
	@stationName NVARCHAR(50),
	@orderNumber INT,
	@id INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @routeId INT;
	SELECT @routeId = Id FROM tblRoute WHERE Number = @routeNumber;

	DECLARE @stationId INT;
	SELECT @stationId = Id FROM tblBusStation WHERE Name = @stationName;

	INSERT INTO tblRouteBusStation(RouteId, BusStationId, OrderNumber, Deleted)
	VALUES (@routeId, @stationId, @orderNumber, 0);

	SET @id = SCOPE_IDENTITY();
END
GO

--update

CREATE PROCEDURE spUpdateRoute
	@id INT,
	@customerId INT,
	@number NVARCHAR(50) = NULL,
	@distance INT = NULL,
	@price INT = NULL,
	@duration INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @number IS NOT NULL
	BEGIN	
		DECLARE @count INT;
		SELECT @count = COUNT(Number) FROM tblRoute WHERE Number = @number AND Deleted = 0 AND Id <> @id;
		BEGIN TRY
		IF @count > 0
			THROW 60000, 'Route number is not unique', 1;
		END TRY
		BEGIN CATCH
			THROW;
		END CATCH;

		UPDATE tblRoute
		SET Number = @number,
			CustomerId = @customerId
		WHERE Id = @id AND Deleted = 0;
	END

	IF @distance IS NOT NULL AND @distance > 0
	BEGIN	
		UPDATE tblRoute
		SET Distance = @distance,
			CustomerId = @customerId
		WHERE Id = @id AND Deleted = 0;
	END

	IF @distance IS NOT NULL AND @price > 0
	BEGIN	
		UPDATE tblRoute
		SET Price = @price,
			CustomerId = @customerId
		WHERE Id = @id AND Deleted = 0;
	END

	IF @duration IS NOT NULL AND @duration > 0
	BEGIN	
		UPDATE tblRoute
		SET Duration = @duration,
			CustomerId = @customerId
		WHERE Id = @id AND Deleted = 0;
	END

END
GO

--delete

CREATE PROCEDURE spDeleteRoute
	@id INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tblRoute 
	SET Deleted = 1 
	WHERE Id = @id;

	UPDATE tblBus
	SET Deleted = 1
	WHERE RouteId = @id;

	UPDATE tblDriver
	SET Deleted = 1
	WHERE BusId IN (SELECT Id FROM tblBus WHERE RouteId = @id);

	UPDATE tblRouteBusStation
	SET Deleted = 1
	WHERE RouteId = @id;

	UPDATE tblTimeTable
	SET Deleted = 1
	WHERE BusId IN (SELECT Id FROM tblBus WHERE RouteId = @id);
END
GO

CREATE PROCEDURE spDeleteBus
	@busId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tblBus
	SET Deleted = 1
	WHERE Id = @busId;

	UPDATE tblDriver
	SET Deleted = 1
	WHERE BusId = @busId;

	UPDATE tblTimeTable
	SET Deleted = 1
	WHERE BusId = @busId;
END
GO

CREATE PROCEDURE spDeleteDriver
	@driverId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tblDriver
	SET Deleted = 1
	WHERE Id = @driverId;

END
GO

CREATE PROCEDURE spDeleteStation
	@stationId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tblBusStation
	SET Deleted = 1
	WHERE Id = @stationId;

	UPDATE tblRouteBusStation
	SET Deleted = 1
	WHERE BusStationId = @stationId;
END
GO

CREATE PROCEDURE spDeleteTimeTable
	@timeTableId INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tblTimeTable
	SET Deleted = 1
	WHERE Id = @timeTableId;
END
GO

CREATE PROCEDURE spDeleteRouteStationRelation
	@name NVARCHAR(50),
	@routeId INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @id INT;

	SELECT @id = Id FROM tblBusStation WHERE Name = @name AND Deleted = 0;

	UPDATE tblRouteBusStation
	SET Deleted = 1
	WHERE BusStationId = @Id AND RouteId = RouteId;
END
GO

--lists for comboBoxes

CREATE PROCEDURE spGetStationName
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Name FROM tblBusStation WHERE Deleted = 0 ORDER BY Name;
END
GO

CREATE PROCEDURE spGetRouteNumber
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Number FROM tblRoute WHERE Deleted = 0 ORDER BY Number;
END
GO

CREATE PROCEDURE spGetBusNumbers
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Number FROM tblBus WHERE Deleted = 0 ORDER BY Number;
END
GO

--get by

CREATE PROCEDURE spGetStationsByRoute
	@routeId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.Id, s.Name
	FROM tblBusStation as s 
	INNER JOIN tblRouteBusStation as rbs
	ON rbs.BusStationId = s.Id
	INNER JOIN tblRoute as r
	ON r.Id = rbs.RouteId
	WHERE r.Id = @routeId AND s.Deleted = 0 AND rbs.Deleted = 0 AND r.Deleted = 0
	ORDER BY rbs.OrderNumber;
END
GO

CREATE PROCEDURE spGetScheduleByRoute
	@routeId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime 
	FROM tblTimeTable as t
	INNER JOIN tblBus as b
	ON b.Id = t.BusId
	INNER JOIN tblRoute as r
	ON r.Id = b.RouteId
	WHERE r.Id = @routeId AND t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0;
END
GO

CREATE PROCEDURE spGetBusesByRoute
	@routeId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT b.Id, b.Number as BusNumber, r.Number as RouteNumber, b.Model, c.Name + ' ' + c.Surname as Customer 
	FROM tblBus as b
	INNER JOIN tblRoute as r 
	ON b.RouteId = r.Id
	INNER JOIN tblCustomer as c
	ON b.CustomerId = c.Id
	WHERE RouteId = @routeId AND b.Deleted = 0 AND r.Deleted = 0;
END
GO

CREATE PROCEDURE spGetScheduleByBus
	@busId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime 
	FROM tblTimeTable as t
	INNER JOIN tblBus as b
	ON b.Id = t.BusId
	INNER JOIN tblRoute as r
	ON r.Id = b.RouteId
	WHERE b.Id = @busId AND t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0;
END
GO

CREATE PROCEDURE spGetStationsByBus
	@busId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.Id, s.Name
	FROM tblBusStation as s 
	INNER JOIN tblRouteBusStation as rbs
	ON rbs.BusStationId = s.Id
	INNER JOIN tblRoute as r
	ON r.Id = rbs.RouteId
	INNER JOIN tblBus as b
	ON b.RouteId = r.Id
	WHERE b.Id = @busId AND s.Deleted = 0 AND rbs.Deleted = 0 AND r.Deleted = 0 AND b.Deleted = 0
	ORDER BY rbs.OrderNumber;
END
GO

CREATE PROCEDURE spGetStationsBySchedule
	@scheduleId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.Id, s.Name
	FROM tblBusStation as s 
	INNER JOIN tblRouteBusStation as rbs
	ON rbs.BusStationId = s.Id
	INNER JOIN tblRoute as r
	ON r.Id = rbs.RouteId
	INNER JOIN tblBus as b
	ON b.RouteId = r.Id
	INNER JOIN tblTimeTable as t
	ON t.BusId = b.Id
	WHERE t.Id = @scheduleId AND s.Deleted = 0 AND rbs.Deleted = 0 AND r.Deleted = 0 AND b.Deleted = 0 AND t.Deleted = 0
	ORDER BY rbs.OrderNumber;
END
GO

CREATE PROCEDURE spGetDriversBySchedule
	@scheduleId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT d.Id, d.Name, d.Surname, b.Number as BusNumber, r.Number as RouteNumber, c.Name + ' ' + c.Surname as Customer
	FROM tblDriver as d 
	INNER JOIN tblBus as b
	ON b.Id = d.BusId
	INNER JOIN tblTimeTable as t
	ON t.BusId = b.Id
	INNER JOIN tblRoute as r
	ON b.RouteId = r.Id
	INNER JOIN tblCustomer as c
	ON d.CustomerId = c.Id
	WHERE t.Id = @scheduleId AND d.Deleted = 0 AND b.Deleted = 0 AND t.Deleted = 0 AND r.Deleted = 0;
END
GO

CREATE PROCEDURE spGetScheduleByStation
	@stationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime 
	FROM tblTimeTable as t
	INNER JOIN tblBus as b
	ON b.Id = t.BusId
	INNER JOIN tblRoute as r
	ON r.Id = b.RouteId
	INNER JOIN tblRouteBusStation as rbs
	ON rbs.RouteId = r.Id
	INNER JOIN tblBusStation as s
	ON s.Id = rbs.BusStationId
	WHERE s.Id = @stationId AND t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 AND rbs.Deleted = 0 AND s.Deleted = 0;
END
GO

CREATE PROCEDURE spGetBusesByStation
	@stationId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT b.Id, b.Number as BusNumber, r.Number as RouteNumber, b.Model, c.Name + ' ' + c.Surname as Customer 
	FROM tblBus as b
	INNER JOIN tblRoute as r 
	ON b.RouteId = r.Id
	INNER JOIN tblRouteBusStation as rbs
	ON rbs.RouteId = r.Id
	INNER JOIN tblBusStation as s
	ON s.Id = rbs.BusStationId
	INNER JOIN tblCustomer as c
	ON b.CustomerId = c.Id
	WHERE s.Id = @stationId AND b.Deleted = 0 AND r.Deleted = 0 AND rbs.Deleted = 0 AND s.Deleted = 0;
END
GO

CREATE PROCEDURE spGetScheduleByDriver
	@driverId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime 
	FROM tblTimeTable as t
	INNER JOIN tblBus as b
	ON b.Id = t.BusId
	INNER JOIN tblDriver as d
	ON d.BusId = b.Id
	INNER JOIN tblRoute as r
	ON r.Id = b.RouteId
	WHERE d.Id = @driverId AND t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 AND d.Deleted = 0;
END
GO

CREATE PROCEDURE spGetStationsByDriver
	@driverId INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT s.Id, s.Name
	FROM tblBusStation as s 
	INNER JOIN tblRouteBusStation as rbs
	ON rbs.BusStationId = s.Id
	INNER JOIN tblRoute as r
	ON r.Id = rbs.RouteId
	INNER JOIN tblBus as b
	ON b.RouteId = r.Id
	INNER JOIN tblDriver as d
	ON d.BusId = b.Id
	WHERE d.Id = @driverId AND s.Deleted = 0 AND rbs.Deleted = 0 AND r.Deleted = 0 AND b.Deleted = 0 AND d.Deleted = 0
	ORDER BY rbs.OrderNumber;
END
GO

--search

CREATE PROCEDURE spGetRoutesBySearch
	@stationName NVARCHAR(50) = NULL,
	@maxPrice INT = NULL,
	@maxDuration INT = NULL
AS
BEGIN
	IF @maxPrice IS NULL
	BEGIN
		SELECT @maxPrice = MAX(Price) + 1 FROM tblRoute WHERE Deleted = 0;
	END

	IF @maxDuration IS NULL
	BEGIN
		SELECT @maxDuration = MAX(Duration) + 1 FROM tblRoute WHERE Deleted = 0;
	END

	IF @stationName IS NULL
	BEGIN
		SELECT r.Id, r.Number, r.Distance, r.Price, r.Duration, c.Name + ' ' + c.Surname as Customer
		FROM tblRoute as r
		INNER JOIN tblCustomer as c
		ON c.Id = r.CustomerId
		WHERE r.Price <= @maxPrice AND r.Duration <= @maxDuration AND r.Deleted = 0;
	END
	ELSE
	BEGIN
		SELECT r.Id, r.Number, r.Distance, r.Price, r.Duration, c.Name + ' ' + c.Surname as Customer
		FROM tblRoute as r
		INNER JOIN tblRouteBusStation as rs
		ON r.Id = rs.RouteId
		INNER JOIN tblBusStation as s
		ON rs.BusStationId = s.Id
		INNER JOIN tblCustomer as c
		ON r.CustomerId = c.Id
		WHERE r.Price <= @maxPrice AND r.Duration <= @maxDuration AND r.Deleted = 0 AND rs.Deleted = 0 AND s.Deleted = 0 AND s.Name = @stationName;
	END
END
GO

CREATE PROCEDURE spGetBusesBySearch
	@stationName NVARCHAR(50) = NULL,
	@routeNumber NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @stationName IS NOT NULL AND @routeNumber IS NOT NULL
	BEGIN
		SELECT b.Id, b.Number as BusNumber, r.Number as RouteNumber, b.Model, c.Name + ' ' + c.Surname as Customer
		FROM tblBus as b
		INNER JOIN tblRoute as r
		ON b.RouteId = r.Id
		INNER JOIN tblRouteBusStation as rbs
		ON r.Id = rbs.RouteId
		INNER JOIN tblBusStation as s
		ON rbs.BusStationId = s.Id
		INNER JOIN tblCustomer as c
		ON b.CustomerId = c.Id
		WHERE (b.Deleted = 0 AND r.Deleted = 0 AND rbs.Deleted = 0 AND s.Deleted = 0 AND
			   s.Name = @stationName AND r.Number = @routeNumber);
	END

	IF @stationName IS NULL AND @routeNumber IS NOT NULL
	BEGIN
		SELECT b.Id, b.Number as BusNumber, r.Number as RouteNumber, b.Model, c.Name + ' ' + c.Surname as Customer
		FROM tblBus as b
		INNER JOIN tblRoute as r
		ON b.RouteId = r.Id
		INNER JOIN tblCustomer as c
		ON b.CustomerId = c.Id
		WHERE b.Deleted = 0 AND r.Deleted = 0 AND r.Number = @routeNumber;
	END

	IF @stationName IS NOT NULL AND @routeNumber IS NULL
	BEGIN
		SELECT b.Id, b.Number as BusNumber, r.Number as RouteNumber, b.Model, c.Name + ' ' + c.Surname as Customer
		FROM tblBus as b
		INNER JOIN tblRoute as r
		ON b.RouteId = r.Id
		INNER JOIN tblRouteBusStation as rbs
		ON r.Id = rbs.RouteId
		INNER JOIN tblBusStation as s
		ON rbs.BusStationId = s.Id
		INNER JOIN tblCustomer as c
		ON b.CustomerId = c.Id
		WHERE b.Deleted = 0 AND r.Deleted = 0 AND rbs.Deleted = 0 AND s.Deleted = 0 AND s.Name = @stationName;
	END

	IF @stationName IS NULL AND @routeNumber IS NULL
	BEGIN
		SELECT b.Id, b.Number as BusNumber, r.Number as RouteNumber, b.Model, c.Name +' '+ c.Surname as Customer
		FROM tblBus as b
		INNER JOIN tblCustomer as c
		ON c.Id = b.CustomerId
		INNER JOIN tblRoute as r
		ON b.RouteId = r.Id
		WHERE b.Deleted = 0 
		ORDER BY b.Id;
	END
END
GO

CREATE PROCEDURE spGetDriversBySearch
	@stationName NVARCHAR(50) = NULL,
	@routeNumber NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @stationName IS NOT NULL AND @routeNumber IS NOT NULL
	BEGIN
		SELECT d.Id, d.Name, d.Surname, b.Number as BusNumber, r.Number as RouteNumber, c.Name + ' ' + c.Surname as Customer
		FROM tblDriver as d
		INNER JOIN tblBus as b
		ON d.BusId = b.Id
		INNER JOIN tblRoute as r
		ON r.Id = b.RouteId
		INNER JOIN tblRouteBusStation as rbs
		ON rbs.RouteId = r.Id
		INNER JOIN tblBusStation as s
		ON rbs.BusStationId = s.Id
		INNER JOIN tblCustomer as c
		ON c.Id = d.CustomerId
		WHERE (d.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 AND rbs.Deleted = 0 AND 
			   s.Deleted = 0 AND s.Name = @stationName AND r.Number = @routeNumber);
	END

	IF @stationName IS NULL AND @routeNumber IS NOT NULL
	BEGIN
		SELECT d.Id, d.Name, d.Surname, b.Number as BusNumber, r.Number as RouteNumber, c.Name + ' ' + c.Surname as Customer
		FROM tblDriver as d
		INNER JOIN tblBus as b
		ON d.BusId = b.Id
		INNER JOIN tblRoute as r
		ON r.Id = b.RouteId
		INNER JOIN tblCustomer as c
		ON c.Id = d.CustomerId
		WHERE (d.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 AND r.Number = @routeNumber);
	END

	IF @stationName IS NOT NULL AND @routeNumber IS NULL
	BEGIN
		SELECT d.Id, d.Name, d.Surname, b.Number as BusNumber, r.Number as RouteNumber, c.Name + ' ' + c.Surname as Customer
		FROM tblDriver as d
		INNER JOIN tblBus as b
		ON d.BusId = b.Id
		INNER JOIN tblRoute as r
		ON r.Id = b.RouteId
		INNER JOIN tblRouteBusStation as rbs
		ON rbs.RouteId = r.Id
		INNER JOIN tblBusStation as s
		ON rbs.BusStationId = s.Id
		INNER JOIN tblCustomer as c
		ON c.Id = d.CustomerId
		WHERE (d.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 AND rbs.Deleted = 0 AND 
			   s.Deleted = 0 AND s.Name = @stationName);
	END

	IF @stationName IS NULL AND @routeNumber IS NULL
	BEGIN
		SELECT d.Id, d.Name, d.Surname, b.Number as BusNumber, r.Number as RouteNumber, c.Name + ' ' + c.Surname as Customer
		FROM tblDriver as d
		INNER JOIN tblBus as b
		ON d.BusId = b.Id
		INNER JOIN tblRoute as r
		ON r.Id = b.RouteId
		INNER JOIN tblCustomer as c
		ON c.Id = d.CustomerId
		WHERE (d.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0);
	END
END
GO

CREATE PROCEDURE spGetStationsBySearch
	@routeNumber NVARCHAR(50) = NULL,
	@busNumber NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @busNumber IS NOT NULL AND @routeNumber IS NOT NULL
	BEGIN
		SELECT s.Id, s.Name
		FROM tblBusStation as s
		INNER JOIN tblRouteBusStation as rbs
		ON s.Id = rbs.BusStationId
		INNER JOIN tblRoute as r
		ON r.Id = rbs.RouteId
		INNER JOIN tblBus as b
		ON b.RouteId = r.Id
		WHERE (s.Deleted = 0 AND rbs.Deleted = 0 AND r.Deleted = 0 AND b.Deleted = 0 AND 
			   b.Number = @busNumber AND r.Number = @routeNumber)
		ORDER BY rbs.OrderNumber;
	END

	IF @busNumber IS NULL AND @routeNumber IS NOT NULL
	BEGIN
		SELECT s.Id, s.Name
		FROM tblBusStation as s
		INNER JOIN tblRouteBusStation as rbs
		ON s.Id = rbs.BusStationId
		INNER JOIN tblRoute as r
		ON r.Id = rbs.RouteId
		WHERE (s.Deleted = 0 AND rbs.Deleted = 0 AND r.Deleted = 0 AND r.Number = @routeNumber)
		ORDER BY rbs.OrderNumber;
	END

	IF @busNumber IS NOT NULL AND @routeNumber IS NULL
	BEGIN
		SELECT s.Id, s.Name
		FROM tblBusStation as s
		INNER JOIN tblRouteBusStation as rbs
		ON s.Id = rbs.BusStationId
		INNER JOIN tblRoute as r
		ON r.Id = rbs.RouteId
		INNER JOIN tblBus as b
		ON b.RouteId = r.Id
		WHERE (s.Deleted = 0 AND rbs.Deleted = 0 AND r.Deleted = 0 AND b.Deleted = 0 AND b.Number = @busNumber)
		ORDER BY rbs.OrderNumber;
	END

	IF @busNumber IS NULL AND @routeNumber IS NULL
	BEGIN
		SELECT s.Id, s.Name
		FROM tblBusStation as s
		WHERE s.Deleted = 0;
	END
END
GO

CREATE PROCEDURE spGetScheduleBySearch
	@stationName NVARCHAR(50) = NULL,
	@routeNumber NVARCHAR(50) = NULL,
	@from TIME = NULL,
	@to TIME = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @from IS NULL
	BEGIN
		SET @from = '00:00:00';
	END

	IF @to IS NULL
	BEGIN
		SET @to = '23:59:59';
	END

	IF @stationName IS NOT NULL AND @routeNumber IS NOT NULL
	BEGIN
		SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime
		FROM tblTimeTable as t
		INNER JOIN tblBus as b
		ON t.BusId = b.Id
		INNER JOIN tblRoute as r
		ON r.Id = b.RouteId
		INNER JOIN tblRouteBusStation as rbs
		ON rbs.RouteId = r.Id
		INNER JOIN tblBusStation as s
		ON rbs.BusStationId = s.Id
		WHERE (t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 AND rbs.Deleted = 0 AND s.Deleted = 0 
			   AND s.Name = @stationName AND r.Number = @routeNumber AND dbo.fnInTimeRange(@from, @to, t.DepartureTime) = 1);
	END

	IF @stationName IS NULL AND @routeNumber IS NOT NULL
	BEGIN
		SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime
		FROM tblTimeTable as t
		INNER JOIN tblBus as b
		ON t.BusId = b.Id
		INNER JOIN tblRoute as r
		ON r.Id = b.RouteId
		WHERE (t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 
			   AND r.Number = @routeNumber AND dbo.fnInTimeRange(@from, @to, t.DepartureTime) = 1);
	END

	IF @stationName IS NOT NULL AND @routeNumber IS NULL
	BEGIN
		SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime
		FROM tblTimeTable as t
		INNER JOIN tblBus as b
		ON t.BusId = b.Id
		INNER JOIN tblRoute as r
		ON r.Id = b.RouteId
		INNER JOIN tblRouteBusStation as rbs
		ON rbs.RouteId = r.Id
		INNER JOIN tblBusStation as s
		ON rbs.BusStationId = s.Id
		WHERE (t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 AND rbs.Deleted = 0 AND s.Deleted = 0 
			   AND s.Name = @stationName AND dbo.fnInTimeRange(@from, @to, t.DepartureTime) = 1);
	END

	IF @stationName IS NULL AND @routeNumber IS NULL
	BEGIN
		SELECT t.Id, r.Number as RouteNumber, t.DepartureTime, r.Duration, DATEADD(MINUTE, r.Duration, t.DepartureTime) as ArrivalTime
		FROM tblTimeTable as t
		INNER JOIN tblBus as b
		ON t.BusId = b.Id
		INNER JOIN tblRoute as r
		ON r.Id = b.RouteId
		WHERE (t.Deleted = 0 AND b.Deleted = 0 AND r.Deleted = 0 AND dbo.fnInTimeRange(@from, @to, t.DepartureTime) = 1);
	END
END
GO