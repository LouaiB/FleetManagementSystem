delete from DeliverySummaries
delete from Deliveries
SET IDENTITY_INSERT [dbo].[Deliveries] ON
INSERT INTO [dbo].[Deliveries] ( [Id], [Time], [CompanyId], [ClientId], [VehicleId], [DriverId], [SourceLongtitude], [SourceLatitude], [SourceCity], [DestinationLongtitude], 
[DestinationLatitude], [DestinationCity], [Quantity], [Answered], [Started], [Finished], [OptimalDistance], [OptimalTime], [OptimalFuelConsumption]) 
VALUES (1 , N'2018-01-1 23:18:42', 1, 1, 1, 1, 35.700399804687493, 33.468322237023038, N'Beqaa', 36.282675195312493, 
 34.316430542272371, N'Hermel', 4, 1, 1,1, 20, 20, 150)
INSERT INTO [dbo].[Deliveries] ( [Id],[Time], [CompanyId], [ClientId], [VehicleId], [DriverId], [SourceLongtitude], [SourceLatitude], [SourceCity], [DestinationLongtitude], 
[DestinationLatitude], [DestinationCity], [Quantity], [Answered], [Started], [Finished], [OptimalDistance], [OptimalTime], [OptimalFuelConsumption]) 
VALUES (2, N'2018-01-3 23:18:42', 1, 1, 1, 1, 35.700399804687493, 33.468322237023038, N'Beqaa', 36.282675195312493, 
 34.316430542272371, N'Hermel', 4, 1, 1,1, 20, 20, 140)
INSERT INTO [dbo].[Deliveries] ( [Id], [Time], [CompanyId], [ClientId], [VehicleId], [DriverId], [SourceLongtitude], [SourceLatitude], [SourceCity], [DestinationLongtitude], 
[DestinationLatitude], [DestinationCity], [Quantity], [Answered], [Started], [Finished], [OptimalDistance], [OptimalTime], [OptimalFuelConsumption]) 
VALUES (3, N'2018-01-5 23:18:42', 1, 1, 1, 1, 35.700399804687493, 33.468322237023038, N'Beqaa', 36.282675195312493, 
 34.316430542272371, N'Hermel', 4, 1, 1,1, 20, 20, 145)
INSERT INTO [dbo].[Deliveries] ( [Id],[Time], [CompanyId], [ClientId], [VehicleId], [DriverId], [SourceLongtitude], [SourceLatitude], [SourceCity], [DestinationLongtitude], 
[DestinationLatitude], [DestinationCity], [Quantity], [Answered], [Started], [Finished], [OptimalDistance], [OptimalTime], [OptimalFuelConsumption]) 
VALUES ( 4,N'2018-01-7 23:18:42', 1, 1, 1, 1, 35.700399804687493, 33.468322237023038, N'Beqaa', 36.282675195312493, 
 34.316430542272371, N'Hermel', 4, 1, 1,1, 20, 20, 160)
INSERT INTO [dbo].[Deliveries] ( [Id], [Time], [CompanyId], [ClientId], [VehicleId], [DriverId], [SourceLongtitude], [SourceLatitude], [SourceCity], [DestinationLongtitude], 
[DestinationLatitude], [DestinationCity], [Quantity], [Answered], [Started], [Finished], [OptimalDistance], [OptimalTime], [OptimalFuelConsumption]) 
VALUES ( 5,N'2018-1-10 23:18:42', 1, 1, 1, 1, 35.700399804687493, 33.468322237023038, N'Beqaa', 36.282675195312493, 
 34.316430542272371, N'Hermel', 4, 1, 1,1, 20, 20, 170)
INSERT INTO [dbo].[Deliveries] ( [Id], [Time], [CompanyId], [ClientId], [VehicleId], [DriverId], [SourceLongtitude], [SourceLatitude], [SourceCity], [DestinationLongtitude], 
[DestinationLatitude], [DestinationCity], [Quantity], [Answered], [Started], [Finished], [OptimalDistance], [OptimalTime], [OptimalFuelConsumption]) 
VALUES (6, N'2019-1-1 23:18:42', 1, 1, 1, 1, 35.700399804687493, 33.468322237023038, N'Beqaa', 36.282675195312493, 
 34.316430542272371, N'Hermel', 4, 1, 1,1, 20, 20, 150)
 SET IDENTITY_INSERT [dbo].[Deliveries] OFF




 

delete from DeliverySummaries
INSERT INTO [dbo].[DeliverySummaries] ([DeliveryId], [StartTime], [EndTime], [StartFuelLevel], [EndFuelLevel], [StartOdometer], [EndOdometer], [HarshAccelerationAndDeceleration], [HarshBreakingsRate], [HardCorneringRate], [SpeedingsRate], [SeatBeltRate], [OverRevving], [OnTimeDeliveryRate], [FuelConsumptionRate], [Idling], [PerformanceScore], [ComplianceScore], [SafetyScore]) 
VALUES (1,'2018/1/1', '2018/1/1',200, 0, 100, 120, 5, 5, 5, 5, 5, 5, 5, 5, 5,1,2.4, 2.2)
INSERT INTO [dbo].[DeliverySummaries] ([DeliveryId], [StartTime], [EndTime], [StartFuelLevel], [EndFuelLevel], [StartOdometer], [EndOdometer], [HarshAccelerationAndDeceleration], [HarshBreakingsRate], [HardCorneringRate], [SpeedingsRate], [SeatBeltRate], [OverRevving], [OnTimeDeliveryRate], [FuelConsumptionRate], [Idling], [PerformanceScore], [ComplianceScore], [SafetyScore]) 
VALUES (2,'2018/3/1', '2018/3/1',200, 10, 100, 120, 5, 5, 5, 5, 5, 5, 5, 5, 5,2,2.9, 2.5)
INSERT INTO [dbo].[DeliverySummaries] ([DeliveryId], [StartTime], [EndTime], [StartFuelLevel], [EndFuelLevel], [StartOdometer], [EndOdometer], [HarshAccelerationAndDeceleration], [HarshBreakingsRate], [HardCorneringRate], [SpeedingsRate], [SeatBeltRate], [OverRevving], [OnTimeDeliveryRate], [FuelConsumptionRate], [Idling], [PerformanceScore], [ComplianceScore], [SafetyScore]) 
VALUES (3,'2018/5/1', '2018/5/1',200, 15, 100, 120, 5, 5, 5, 5, 5, 5, 5, 5, 5,2.5,3.2, 2.9)
INSERT INTO [dbo].[DeliverySummaries] ([DeliveryId], [StartTime], [EndTime], [StartFuelLevel], [EndFuelLevel], [StartOdometer], [EndOdometer], [HarshAccelerationAndDeceleration], [HarshBreakingsRate], [HardCorneringRate], [SpeedingsRate], [SeatBeltRate], [OverRevving], [OnTimeDeliveryRate], [FuelConsumptionRate], [Idling], [PerformanceScore], [ComplianceScore], [SafetyScore]) 
VALUES (4,'2018/7/1', '2018/7/1',200, 35, 100, 120, 5, 5, 5, 5, 5, 5, 5, 5, 5,3,3.5, 3.6)
INSERT INTO [dbo].[DeliverySummaries] ([DeliveryId], [StartTime], [EndTime], [StartFuelLevel], [EndFuelLevel], [StartOdometer], [EndOdometer], [HarshAccelerationAndDeceleration], [HarshBreakingsRate], [HardCorneringRate], [SpeedingsRate], [SeatBeltRate], [OverRevving], [OnTimeDeliveryRate], [FuelConsumptionRate], [Idling], [PerformanceScore], [ComplianceScore], [SafetyScore]) 
VALUES (5,'2018/10/1', '2018/10/1',200, 20, 100, 120, 5, 5, 5, 5, 5, 5, 5, 5, 5,4,3.4, 3.8)
INSERT INTO [dbo].[DeliverySummaries] ([DeliveryId], [StartTime], [EndTime], [StartFuelLevel], [EndFuelLevel], [StartOdometer], [EndOdometer], [HarshAccelerationAndDeceleration], [HarshBreakingsRate], [HardCorneringRate], [SpeedingsRate], [SeatBeltRate], [OverRevving], [OnTimeDeliveryRate], [FuelConsumptionRate], [Idling], [PerformanceScore], [ComplianceScore], [SafetyScore]) 
VALUES (6,'2019/1/1', '2019/1/1',200, 145, 100, 120, 5, 5, 5, 5, 5, 5, 5, 5, 5,4.5,4.4, 5)
-- [HarshAccelerationAndDeceleration], [HarshBreakingsRate], [HardCorneringRate], [SpeedingsRate], [SeatBeltRate], [OverRevving], [OnTimeDeliveryRate], [FuelConsumptionRate], [Idling], [PerformanceScore], [ComplianceScore], [SafetyScore]) 