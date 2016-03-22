USE PTS;
GO

DROP PROCEDURE spDeleteBus;

DROP PROCEDURE spDeleteDriver;

DROP PROCEDURE spDeleteRoute;

DROP PROCEDURE spDeleteRouteStationRelation;

DROP PROCEDURE spDeleteStation;

DROP PROCEDURE spDeleteTimeTable;

DROP PROCEDURE spGetBusesByRoute;

DROP PROCEDURE spGetBusesBySearch;

DROP PROCEDURE spGetBusesByStation;

DROP PROCEDURE spGetBusNumbers;

DROP PROCEDURE spGetDriversBySchedule;

DROP PROCEDURE spGetDriversBySearch;

DROP PROCEDURE spGetRouteNumber;

DROP PROCEDURE spGetRoutesBySearch;

DROP PROCEDURE spGetScheduleByBus;

DROP PROCEDURE spGetScheduleByDriver;

DROP PROCEDURE spGetScheduleByRoute;

DROP PROCEDURE spGetScheduleBySearch;

DROP PROCEDURE spGetScheduleByStation;

DROP PROCEDURE spGetStationName;

DROP PROCEDURE spGetStationsByBus;

DROP PROCEDURE spGetStationsByDriver;

DROP PROCEDURE spGetStationsByRoute;

DROP PROCEDURE spGetStationsBySchedule;

DROP PROCEDURE spGetStationsBySearch;

DROP PROCEDURE spInsertBus;

DROP PROCEDURE spInsertDriver;

DROP PROCEDURE spInsertRoute;

DROP PROCEDURE spInsertRouteStationRelation;

DROP PROCEDURE spInsertStation;

DROP PROCEDURE spInsertTimeTable;

DROP PROCEDURE spLogIn;

DROP PROCEDURE spSelectAllBuses;

DROP PROCEDURE spSelectAllDrivers;

DROP PROCEDURE spSelectAllRoutes;

DROP PROCEDURE spSelectAllStations;

DROP PROCEDURE spSelectAllTimeTable;

DROP PROCEDURE spUpdateRoute;

DROP FUNCTION dbo.fnInTimeRange;

DROP TABLE tblTimeTable;

DROP TABLE tblRouteBusStation;

DROP TABLE tblBusStation;

DROP TABLE tblDriver;

DROP TABLE tblBus;

DROP TABLE tblRoute;

DROP TABLE tblCustomer;