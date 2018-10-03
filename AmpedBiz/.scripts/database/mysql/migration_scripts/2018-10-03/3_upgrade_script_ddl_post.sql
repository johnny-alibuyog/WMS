ALTER TABLE `returns` DROP FOREIGN KEY `FK_Return_Total_Currency`;

ALTER TABLE `returns` DROP INDEX `IDX_Return_Total_Currency`;
ALTER TABLE `returns` DROP INDEX `Total_CurrencyId`;

ALTER TABLE `returns` DROP COLUMN `Total_CurrencyId`;
ALTER TABLE `returns` DROP COLUMN `Total_Amount`;



ALTER TABLE `returnitems` DROP PRIMARY KEY, ADD PRIMARY KEY(`ReturnItemBaseId`);
ALTER TABLE `returnitems` DROP FOREIGN KEY `FK_ReturnItem_QuantityStandardEquivalent_Unit`;
ALTER TABLE `returnitems` DROP FOREIGN KEY `FK_ReturnItem_Quantity_Unit`;
ALTER TABLE `returnitems` DROP FOREIGN KEY `FK_ReturnItem_Standard_Unit`;
ALTER TABLE `returnitems` DROP FOREIGN KEY `FK_ReturnItem_TotalPrice_Currency`;
ALTER TABLE `returnitems` DROP FOREIGN KEY `FK_ReturnItems_Product`;
ALTER TABLE `returnitems` DROP FOREIGN KEY `FK_ReturnItems_ReturnReason`;

ALTER TABLE `returnitems` DROP INDEX `IDX_ReturnItems_Product`;
ALTER TABLE `returnitems` DROP INDEX `IDX_ReturnItems_ReturnReason`;
ALTER TABLE `returnitems` DROP INDEX `IDX_ReturnItem_Quantity_Unit`;
ALTER TABLE `returnitems` DROP INDEX `IDX_ReturnItem_Standard_Unit`;
ALTER TABLE `returnitems` DROP INDEX `IDX_ReturnItem_QuantityStandardEquivalent_Unit`;
ALTER TABLE `returnitems` DROP INDEX `IDX_ReturnItem_TotalPrice_Currency`;
ALTER TABLE `returnitems` DROP INDEX `ProductId`;
ALTER TABLE `returnitems` DROP INDEX `ReturnReasonId`;
ALTER TABLE `returnitems` DROP INDEX `Quantity_UnitId`;
ALTER TABLE `returnitems` DROP INDEX `Standard_UnitId`;
ALTER TABLE `returnitems` DROP INDEX `QuantityStandardEquivalent_UnitId`;
ALTER TABLE `returnitems` DROP INDEX `TotalPrice_CurrencyId`;

ALTER TABLE `returnitems` DROP COLUMN `ReturnItemId`;
ALTER TABLE `returnitems` DROP COLUMN `ProductId`;
ALTER TABLE `returnitems` DROP COLUMN `ReturnReasonId`;
ALTER TABLE `returnitems` DROP COLUMN `Quantity_Value`;
ALTER TABLE `returnitems` DROP COLUMN `Quantity_UnitId`;
ALTER TABLE `returnitems` DROP COLUMN `Standard_Value`;
ALTER TABLE `returnitems` DROP COLUMN `Standard_UnitId`;
ALTER TABLE `returnitems` DROP COLUMN `QuantityStandardEquivalent_Value`;
ALTER TABLE `returnitems` DROP COLUMN `QuantityStandardEquivalent_UnitId`;
ALTER TABLE `returnitems` DROP COLUMN `TotalPrice_Amount`;
ALTER TABLE `returnitems` DROP COLUMN `TotalPrice_CurrencyId`;



ALTER TABLE `orderreturns` DROP PRIMARY KEY, ADD PRIMARY KEY(`ReturnItemBaseId`);
ALTER TABLE `orderreturns` DROP FOREIGN KEY `FK_OrderReturn_QuantityStandardEquivalent_Unit`;
ALTER TABLE `orderreturns` DROP FOREIGN KEY `FK_OrderReturn_Quantity_Unit`;
ALTER TABLE `orderreturns` DROP FOREIGN KEY `FK_OrderReturn_Returned_Currency`;
ALTER TABLE `orderreturns` DROP FOREIGN KEY `FK_OrderReturn_Standard_Unit`;
ALTER TABLE `orderreturns` DROP FOREIGN KEY `FK_OrderReturns_Product`;
ALTER TABLE `orderreturns` DROP FOREIGN KEY `FK_OrderReturns_Reason`;
ALTER TABLE `orderreturns` DROP FOREIGN KEY `FK_OrderReturns_ReturnItemBase`;
ALTER TABLE `orderreturns` DROP FOREIGN KEY `FK_Returns_Order`;

ALTER TABLE `orderreturns` DROP INDEX `IDX_OrderReturns_Product`;
ALTER TABLE `orderreturns` DROP INDEX `IDX_OrderReturns_Reason`;
ALTER TABLE `orderreturns` DROP INDEX `IDX_OrderReturn_Quantity_Unit`;
ALTER TABLE `orderreturns` DROP INDEX `IDX_OrderReturn_Standard_Unit`;
ALTER TABLE `orderreturns` DROP INDEX `IDX_OrderReturn_QuantityStandardEquivalent_Unit`;
ALTER TABLE `orderreturns` DROP INDEX `IDX_OrderReturn_Returned_Currency`;
ALTER TABLE `orderreturns` DROP INDEX `ProductId`;
ALTER TABLE `orderreturns` DROP INDEX `ReasonId`;
ALTER TABLE `orderreturns` DROP INDEX `Quantity_UnitId`;
ALTER TABLE `orderreturns` DROP INDEX `Standard_UnitId`;
ALTER TABLE `orderreturns` DROP INDEX `QuantityStandardEquivalent_UnitId`;
ALTER TABLE `orderreturns` DROP INDEX `Returned_CurrencyId`;

ALTER TABLE `orderreturns` DROP COLUMN `OrderReturnId`;
ALTER TABLE `orderreturns` DROP COLUMN `ProductId`;
ALTER TABLE `orderreturns` DROP COLUMN `ReasonId`;
ALTER TABLE `orderreturns` DROP COLUMN `Quantity_Value`;
ALTER TABLE `orderreturns` DROP COLUMN `Quantity_UnitId`;
ALTER TABLE `orderreturns` DROP COLUMN `Standard_Value`;
ALTER TABLE `orderreturns` DROP COLUMN `Standard_UnitId`;
ALTER TABLE `orderreturns` DROP COLUMN `QuantityStandardEquivalent_Value`;
ALTER TABLE `orderreturns` DROP COLUMN `QuantityStandardEquivalent_UnitId`;
ALTER TABLE `orderreturns` DROP COLUMN `Returned_Amount`;
ALTER TABLE `orderreturns` DROP COLUMN `Returned_CurrencyId`;
