﻿USE PTS;
GO

SET IDENTITY_INSERT tblCustomer ON;

INSERT INTO tblCustomer(Id, [Login], [Password], Name, Surname, Mode)
VALUES	(1, 'roman.vantukh@gmail.com', '8eb65b037534af3177bb9dd4c1e38d29b5651d09cbb220a25abfa6c8cde73966', 'Roman', 'Vantukh', 0), --RomanVantukh123
		(2, 'andriy.moroz@yahoo.com', '793843bfd647d6c30122c8889cba3fb9e80012d6f4084720c71bdd2af82660e3', 'Andriy', 'Moroz', 0), --AndriyMoroz123
		(3, 'marta.bila@gmail.com', '0f1f7d4e3021679370a1eef573c5ca205b47a0e5795d1dc0152bb65236fbe7fc', 'Marta', 'Bila', 1),  --MartaBila123
		(4, 'ivan.kozak@ua.com', 'f4a2043159ead1bf29d8d3c2d5903930548d15f20c814fb51119fdcd333ddda1', 'Ivan', 'Kozak', 1),  --IvanKozak123
		(5, 'orest.kit@gmail.com', '5180c91d187fb7712d00d757cf7fe34ca7990b1ec0d16811a9a172c0b0649570', 'Orest', 'Kit', 1) --OrestKit123

SET IDENTITY_INSERT tblCustomer OFF;

SET IDENTITY_INSERT tblRoute ON;

INSERT INTO tblRoute(Id, Number, Distance, Price, Duration, CustomerId, Deleted)
Values	(1, '1a', 6, 4, 25, 1, 0),
		(2, '3a', 4, 4, 35, 2, 0),
		(3, '4a', 10, 4, 40, 1, 0),
		(4, '5a', 12, 4, 43, 2, 0),
		(5, '47a', 15, 4, 38, 2, 0),
		(6, '31', 12, 4, 35, 1, 0),
		(7, '41', 14, 4, 50, 2, 0),
		(8, '51', 15, 4, 55, 2, 0),
		(9, '127', 14, 11, 60, 2, 0),
		(10, '151', 35, 12, 55, 1, 0)

SET IDENTITY_INSERT tblRoute OFF;

SET IDENTITY_INSERT tblBus ON;

INSERT INTO tblBus(Id, Number, RouteId, Model, CustomerId, Deleted)
VALUES	(1, 'BC 3402 BC', 1, 'Man', 1, 0),
		(2, 'BC 4452 BX', 1, 'Man', 2, 0),
		(3, 'BC 1982 BC', 1, 'LAZ', 2, 0),
		(4, 'BC 9502 BX', 2, 'LAZ', 1, 0),
		(5, 'BC 4572 BC', 2, 'LAZ', 1, 0),
		(6, 'BC 1502 BC', 2, 'LAZ', 2, 0),
		(7, 'BC 1212 BV', 3, 'Mersedes', 1, 0),
		(8, 'BC 3245 BX', 3, 'LAZ', 1, 0),
		(9, 'BC 9876 BC', 4, 'Man', 1, 0),
		(10, 'BC 7609 AO', 4, 'LAZ', 1, 0),
	    (11, 'BC 1385 AA', 4, 'LAZ', 2, 0),
	    (12, 'BC 1212 BC', 4, 'Man', 2, 0),
	    (13, 'BC 3415 BO', 5, 'LAZ', 1, 0),
	    (14, 'BC 4581 CO', 5, 'LAZ', 1, 0),
	    (15, 'BC 3212 BE', 5, 'LAZ', 1, 0),
	    (16, 'BC 5509 BX', 6, 'LAZ', 2, 0),
	    (17, 'BC 3214 BX', 6, 'LAZ', 1, 0),
	    (18, 'BC 3434 BO', 6, 'Man', 1, 0),
	    (19, 'BC 6717 BI', 7, 'LAZ', 2, 0),
	    (20, 'BC 0909 BI', 7, 'Man', 2, 0),
	    (21, 'BC 0049 BC', 7, 'Mersedes', 1, 0),
	    (22, 'BC 0988 BI', 8, 'Man', 1, 0),
	    (23, 'BC 4513 BI', 8, 'LAZ', 2, 0),
	    (24, 'BC 1120 BX', 8, 'Man', 2, 0),
	    (25, 'BC 3410 BI', 9, 'LAZ', 1, 0),
	    (26, 'BC 1267 BO', 9, 'Man', 1, 0),
	    (27, 'BC 3209 BX', 9, 'LAZ', 1 ,0),
	    (28, 'BC 5757 BI', 10, 'LAZ', 2, 0),
	    (29, 'BC 3342 BO', 10, 'Man', 2, 0),
	    (30, 'BC 4432 BE', 10, 'LAZ', 2, 0)

SET IDENTITY_INSERT tblBus OFF;

SET IDENTITY_INSERT tblDriver ON;

INSERT INTO tblDriver(Id, BusId, Name, Surname, CustomerId, Deleted)
VALUES	(1, 1, 'Bohdan', 'Shevchenko', 2, 0),
		(2, 1, 'Ivan', 'Bodnar', 2, 0),
		(3, 2, 'Oleh', 'Biluy', 1, 0),
		(4, 2, 'Andriy', 'Bilan', 2, 0),
		(5, 3, 'Oleh', 'Kobryn', 1, 0),
		(6, 3, 'Oleksandr', 'Baluk', 1, 0),
		(7, 4, 'Bohdan', 'Parta', 1, 0),
		(8, 4, 'Anatoly', 'Velozu', 2, 0),
		(9, 5, 'Andriy', 'Tsyrkul', 1, 0),
		(10, 5, 'Oleh', 'Baran', 2, 0),
		(11, 6, 'Vitalii', 'Koza', 2, 0),
		(12, 6, 'Tymofii', 'Nis', 2, 0),
		(13, 7, 'Vladyslav', 'Varenyk', 1, 0),
		(14, 7, 'Nazar', 'Strukhal', 1, 0),
		(15, 8, 'Taras', 'Tymofiichuk', 1, 0),
		(16, 8, 'Myroslav', 'Tereblia', 2, 0),
		(17, 9, 'Yaroslav', 'Ivanenko', 1, 0),
		(18, 9, 'Sviatoslav', 'Petrenko', 1, 0),
		(19, 10, 'Vasyl', 'Kopytko', 2, 0),
		(20, 10, 'Roman', 'Vasula', 1, 0),
		(21, 11, 'Andrii', 'Stetsyshyn', 2, 0),
		(22, 11, 'Bohdan', 'Moskal', 2, 0),
		(23, 12, 'Oleksii', 'Hroisman', 2, 0),
		(24, 12, 'Yurii', 'Nedilko', 1, 0),
		(25, 13, 'Oleh', 'Koval', 1, 0),
		(26, 13, 'Vitalii', 'Shpyk', 1, 0),
		(27, 14, 'Tymofii', 'Sobaka', 2, 0),
		(28, 14, 'Vladyslav', 'Vasyl', 2, 0),
		(29, 15, 'Nazar', 'Kortan', 2, 0),
		(30, 15, 'Taras', 'Vasula', 1, 0),
		(31, 16, 'Myroslav', 'Brehinets', 2, 0),
		(32, 16, 'Yaroslav', 'Zinkiv', 1, 0),
		(33, 17, 'Sviatoslav', 'Demko', 2, 0),
		(34, 17, 'Vasyl', 'Demchyna', 1, 0),
		(35, 18, 'Roman', 'Senyk', 2, 0),
		(36, 18, 'Andrii', 'Tsokalo', 1, 0),
		(37, 19, 'Bohdan', 'Moroz', 2, 0),
		(38, 19, 'Oleksii', 'Kozak', 2, 0),
		(39, 20, 'Yurii', 'Vikhola', 1, 0),
		(40, 20, 'Oleh', 'Perekotypole', 1, 0),
		(41, 21, 'Vitalii', 'Miroshnychenko', 2, 0),
		(42, 21, 'Tymofii', 'Mark', 2, 0),
		(43, 22, 'Vladyslav', 'Hnat', 1, 0),
		(44, 22, 'Nazar', 'Vehera', 2, 0),
		(45, 23, 'Taras', 'Potereiko', 1, 0),
		(46, 23, 'Myroslav', 'Matsiuk', 1, 0),
		(47, 24, 'Yaroslav', 'Berezen', 1, 0),
		(48, 24, 'Sviatoslav', 'Vasko', 1, 0),
		(49, 25, 'Vasyl', 'Viter', 2, 0),
		(50, 25, 'Roman', 'Pyrih', 2, 0),
		(51, 26, 'Andrii', 'Loza', 2, 0),
		(52, 26, 'Bohdan', 'Varvanets', 2, 0),
		(53, 27, 'Oleksii', 'Tomashov', 2, 0),
		(54, 27, 'Yurii', 'Urahan', 1, 0),
		(55, 28, 'Oleh', 'Romanchak', 1, 0),
		(56, 28, 'Vitalii', 'Ivanets', 1, 0),
		(57, 29, 'Tymofii', 'Tymoshenko', 2, 0),
		(58, 29, 'Vladyslav', 'Samsonenko', 2, 0),
		(59, 30, 'Nazar', 'Melnyk', 2, 0),
		(60, 30, 'Taras', 'Paslavskyi', 1, 0)

SET IDENTITY_INSERT tblDriver OFF;

SET IDENTITY_INSERT tblBusStation ON;

INSERT INTO tblBusStation(Id, Name, Deleted)
VALUES	(1, 'Svobody Sqr', 0),
		(2, 'Chornovola Str', 0),
		(3, 'Franka Str', 0),
		(4, 'Mytna Sqr', 0),
		(5, 'Lypynskoho Str', 0),
		(6, 'Halytske Perehkrestia', 0),
		(7, 'Bandery Str', 0),
		(8, 'Railway station', 0),
		(9, 'Lychakivska Str', 0),
		(10, 'Khmelnytskoho Str', 0),
		(11, 'Pasichna Str', 0),
		(12, 'Zelena Str', 0),
		(13, 'Doroshenka Str', 0),
		(14, 'Hrinchenka Str', 0),
		(15, 'Mazepy Str', 0),
		(16, 'Liubinska Str', 0),
		(17, 'Heroiv UPA Str', 0),
		(18, 'Patona Str', 0),
		(19, 'Stryiska Str', 0),
		(21, 'Chervonoi Kalyny Str', 0),
		(22, 'Stusa Str', 0),
		(23, 'Vynnyky', 0),
		(24, 'Sykhivska Str', 0),
		(25, 'Chuprynky Str', 0),
		(26, 'Kniahyni Olhy Str', 0),
		(27, 'Naukova Str', 0),
		(28, 'Volodymyra Velykoho Str', 0),
		(29, 'Doroshiv', 0),
		(30, 'Kulykiv', 0),
		(31, 'Soposhyn', 0),
		(32, 'Zhovkva', 0),
		(33, 'Nove Selo', 0),
		(34, 'Stroniatyn', 0),
		(35, 'Rizni', 0),
		(36, 'Horodotska', 0)

SET IDENTITY_INSERT tblBusStation OFF;

SET IDENTITY_INSERT tblRouteBusStation ON;

INSERT INTO tblRouteBusStation(Id, RouteId, BusStationId, OrderNumber, Deleted)
VALUES	(1, 1, 1, 2, 0),
		(2, 1, 2, 3, 0),
		(3, 1, 4, 1, 0),
		(4, 1, 5, 4, 0),
		(5, 1, 10, 5, 0),
		(6, 1, 6, 6, 0),
		(7, 2, 35, 1, 0),
		(8, 2, 1, 2, 0),
		(9, 2, 4, 3, 0),
		(10, 2, 12, 4, 0),
		(11, 2, 19, 5, 0),
		(12, 3, 35, 1, 0),
		(13, 3, 1, 2, 0),
		(14, 3, 3, 3, 0),
		(15, 3, 22, 4, 0),
		(16, 3, 21, 5, 0),
		(17, 4, 35, 1, 0),
		(18, 4, 1, 2, 0),
		(19, 4, 4, 3, 0),
		(20, 4, 9, 4, 0),
		(21, 4, 23, 5, 0),
		(22, 5, 35, 1, 0),
		(23, 5, 1, 2, 0),
		(24, 5, 4, 3, 0),
		(25, 5, 9, 4, 0),
		(26, 5, 11, 5, 0),
		(27, 5, 12, 6, 0),
		(28, 5, 21, 7, 0),
		(29, 6, 8, 1, 0),
		(30, 6, 36, 2, 0),
		(31, 6, 2, 3, 0),
		(32, 6, 15, 4, 0),
		(33, 6, 14, 5, 0),
		(34, 6, 10, 6, 0),
		(35, 6, 6, 7, 0),
		(36, 7, 24, 1, 0),
		(37, 7, 21, 2, 0),
		(38, 7, 27, 3, 0),
		(39, 7, 26, 4, 0),
		(40, 7, 36, 5, 0),
		(41, 7, 2, 6, 0),
		(42, 7, 15, 7, 0),
		(43, 7, 14, 8, 0),
		(44, 7, 10, 9, 0),
		(45, 7, 6, 10, 0),
		(46, 8, 27, 1, 0),
		(47, 8, 26, 2, 0),
		(48, 8, 28, 3, 0),
		(49, 8, 16, 4, 0),
		(50, 8, 36, 5, 0),
		(51, 8, 2, 6, 0),
		(52, 8, 15, 7, 0),
		(53, 8, 14, 8, 0),
		(54, 8, 10, 9, 0),
		(55, 8, 6, 10, 0),
		(56, 9, 2, 1, 0),
		(57, 9, 5, 2, 0),
		(58, 9, 10, 3, 0),
		(59, 9, 6, 4, 0),
		(60, 9, 29, 5, 0),
		(61, 9, 30, 6, 0),
		(62, 9, 33, 7, 0),
		(63, 9, 34, 8, 0),
		(64, 10, 2, 1, 0),
		(65, 10, 5, 2, 0),
		(66, 10, 10, 3, 0),
		(67, 10, 6, 4, 0),
		(68, 10, 29, 5, 0),
		(69, 10, 30, 6, 0),
		(70, 10, 31, 7, 0),
		(71, 10, 32, 8, 0)

SET IDENTITY_INSERT tblRouteBusStation OFF;

SET IDENTITY_INSERT tblTimeTable ON;

INSERT INTO tblTimeTable(Id, BusId, DepartureTime, Deleted)
VALUES	(1, 1, '07:00:00', 0),
		(2, 1, '10:00:00', 0),
		(3, 1, '13:00:00', 0),
		(4, 1, '16:00:00', 0),
		(5, 1, '19:00:00', 0),
		(6, 2, '08:30:00', 0),
		(7, 2, '11:30:00', 0),
		(8, 2, '14:30:00', 0),
		(9, 2, '17:30:00', 0),
		(10, 2, '20:30:00', 0),
		(11, 3, '06:45:00', 0),
		(12, 3, '10:15:00', 0),
		(13, 3, '14:10:00', 0),
		(14, 3, '16:50:00', 0),
		(15, 3, '21:05:00', 0),
		(16, 4, '08:25:00', 0),
		(17, 4, '12:35:00', 0),
		(18, 4, '15:30:00', 0),
		(19, 4, '18:55:00', 0),
		(20, 4, '19:45:00', 0),

		(21, 5, '06:30:00', 0),
		(22, 5, '09:15:00', 0),
		(23, 5, '14:30:00', 0),
		(24, 5, '17:10:00', 0),
		(25, 5, '20:10:00', 0),
		(26, 6, '07:43:00', 0),
		(27, 6, '11:15:00', 0),
		(28, 6, '15:50:00', 0),
		(29, 6, '19:30:00', 0),
		(30, 6, '21:40:00', 0),
		(31, 7, '08:00:00', 0),
		(32, 7, '12:55:00', 0),
		(33, 7, '16:10:00', 0),
		(34, 7, '19:20:00', 0),
		(35, 7, '21:30:00', 0),
		(36, 8, '06:20:00', 0),
		(37, 8, '09:50:00', 0),
		(38, 8, '14:00:00', 0),
		(39, 8, '18:10:00', 0),
		(40, 8, '20:40:00', 0),

		(41, 9, '07:10:00', 0),
		(42, 9, '10:15:00', 0),
		(43, 9, '13:30:00', 0),
		(44, 9, '17:50:00', 0),
		(45, 9, '20:10:00', 0),
		(46, 10, '08:15:00', 0),
		(47, 10, '11:50:00', 0),
		(48, 10, '15:55:00', 0),
		(49, 10, '18:30:00', 0),
		(50, 10, '21:20:00', 0),
		(51, 11, '08:10:00', 0),
		(52, 11, '11:30:00', 0),
		(53, 11, '15:45:00', 0),
		(54, 11, '17:55:00', 0),
		(55, 11, '20:05:00', 0),
		(56, 12, '07:00:00', 0),
		(57, 12, '10:00:00', 0),
		(58, 12, '13:00:00', 0),
		(59, 12, '16:00:00', 0),
		(60, 12, '21:00:00', 0),

		(61, 13, '07:10:00', 0),
		(62, 13, '10:20:00', 0),
		(63, 13, '14:30:00', 0),
		(64, 13, '17:10:00', 0),
		(65, 13, '20:40:00', 0),
		(66, 14, '08:30:00', 0),
		(67, 14, '12:35:00', 0),
		(68, 14, '16:35:00', 0),
		(69, 14, '18:10:00', 0),
		(70, 14, '21:00:00', 0),
		(71, 15, '08:10:00', 0),
		(72, 15, '12:20:00', 0),
		(73, 15, '15:45:00', 0),
		(74, 15, '18:00:00', 0),
		(75, 15, '20:30:00', 0),
		(76, 16, '07:05:00', 0),
		(77, 16, '11:10:00', 0),
		(78, 16, '13:40:00', 0),
		(79, 16, '17:20:00', 0),
		(80, 16, '21:10:00', 0),

		(81, 17, '07:25:00', 0),
		(82, 17, '12:10:00', 0),
		(83, 17, '15:20:00', 0),
		(84, 17, '17:40:00', 0),
		(85, 17, '20:00:00', 0),
		(86, 18, '08:00:00', 0),
		(87, 18, '10:10:00', 0),
		(88, 18, '14:15:00', 0),
		(89, 18, '16:40:00', 0),
		(90, 18, '18:55:00', 0),
		(91, 19, '07:00:00', 0),
		(92, 19, '10:00:00', 0),
		(93, 19, '14:00:00', 0),
		(94, 19, '17:00:00', 0),
		(95, 19, '20:00:00', 0),
		(96, 20, '08:00:00', 0),
		(97, 20, '11:00:00', 0),
		(98, 20, '15:00:00', 0),
		(99, 20, '18:00:00', 0),
		(100, 20, '21:00:00', 0),

		(101, 21, '07:00:00', 0),
		(102, 21, '10:00:00', 0),
		(103, 21, '13:00:00', 0),
		(104, 21, '16:00:00', 0),
		(105, 21, '19:00:00', 0),
		(106, 22, '08:30:00', 0),
		(107, 22, '11:30:00', 0),
		(108, 22, '14:30:00', 0),
		(109, 22, '17:30:00', 0),
		(110, 22, '20:30:00', 0),
		(111, 23, '06:45:00', 0),
		(112, 23, '10:15:00', 0),
		(113, 23, '14:10:00', 0),
		(114, 23, '16:50:00', 0),
		(115, 23, '21:05:00', 0),
		(116, 24, '08:25:00', 0),
		(117, 24, '12:35:00', 0),
		(118, 24, '15:30:00', 0),
		(119, 24, '18:55:00', 0),
		(120, 24, '19:45:00', 0),
		 
		(121, 25, '07:25:00', 0),
		(122, 25, '12:10:00', 0),
		(123, 25, '15:20:00', 0),
		(124, 25, '17:40:00', 0),
		(125, 25, '20:00:00', 0),
		(126, 26, '08:00:00', 0),
		(127, 26, '10:10:00', 0),
		(128, 26, '14:15:00', 0),
		(129, 26, '16:40:00', 0),
		(130, 26, '18:55:00', 0),
		(131, 27, '07:25:00', 0),
		(132, 27, '12:10:00', 0),
		(133, 27, '15:20:00', 0),
		(134, 27, '17:40:00', 0),
		(135, 27, '20:00:00', 0),
		(136, 28, '08:00:00', 0),
		(137, 28, '10:10:00', 0),
		(138, 28, '14:15:00', 0),
		(139, 28, '16:40:00', 0),
		(140, 28, '18:55:00', 0),
		 
		(141, 29, '07:10:00', 0),
		(142, 29, '10:15:00', 0),
		(143, 29, '13:30:00', 0),
		(144, 29, '17:50:00', 0),
		(145, 29, '20:10:00', 0),
		(146, 30, '08:15:00', 0),
		(147, 30, '11:50:00', 0),
		(148, 30, '15:55:00', 0),
		(149, 30, '18:30:00', 0),
		(150, 30, '21:20:00', 0)

SET IDENTITY_INSERT tblTimeTable OFF;