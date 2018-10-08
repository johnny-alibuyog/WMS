
    alter table ampedbizdb.Returns 
        add column TotalReturned_Amount DECIMAL(19,5);

    alter table ampedbizdb.Returns 
        add column TotalReturned_CurrencyId VARCHAR(30);

    create table ampedbizdb.ReturnItemBases (
        ReturnItemBaseId CHAR(36) not null,
       ProductId CHAR(36) not null,
       ReasonId VARCHAR(15) not null,
       Quantity_Value DECIMAL(19,5),
       Quantity_UnitId VARCHAR(255),
       Standard_Value DECIMAL(19,5),
       Standard_UnitId VARCHAR(255),
       QuantityStandardEquivalent_Value DECIMAL(19,5),
       QuantityStandardEquivalent_UnitId VARCHAR(255),
       Returned_Amount DECIMAL(19,5),
       Returned_CurrencyId VARCHAR(30),
       primary key (ReturnItemBaseId)
    ) ENGINE=InnoDB;

    alter table ampedbizdb.OrderReturns 
        add column ReturnItemBaseId CHAR(36);

    alter table ampedbizdb.ReturnItems 
        add column ReturnItemBaseId CHAR(36);

    alter table ampedbizdb.Returns 
        add index (TotalReturned_CurrencyId), 
        add constraint FK_Return_TotalReturned_Currency 
        foreign key (TotalReturned_CurrencyId) 
        references ampedbizdb.Currencies (CurrencyId);

    create index IDX_Return_TotalReturned_Currency on ampedbizdb.Returns (TotalReturned_CurrencyId);

    alter table ampedbizdb.ReturnItemBases 
        add index (ProductId), 
        add constraint FK_ReturnItemBases_Product 
        foreign key (ProductId) 
        references ampedbizdb.Products (ProductId);

    alter table ampedbizdb.ReturnItemBases 
        add index (ReasonId), 
        add constraint FK_ReturnItemBases_Reason 
        foreign key (ReasonId) 
        references ampedbizdb.ReturnReasons (ReturnReasonId);

    alter table ampedbizdb.ReturnItemBases 
        add index (Quantity_UnitId), 
        add constraint FK_ReturnItemBase_Quantity_Unit 
        foreign key (Quantity_UnitId) 
        references ampedbizdb.UnitOfMeasures (UnitOfMeasureId);

    alter table ampedbizdb.ReturnItemBases 
        add index (Standard_UnitId), 
        add constraint FK_ReturnItemBase_Standard_Unit 
        foreign key (Standard_UnitId) 
        references ampedbizdb.UnitOfMeasures (UnitOfMeasureId);

    alter table ampedbizdb.ReturnItemBases 
        add index (QuantityStandardEquivalent_UnitId), 
        add constraint FK_ReturnItemBase_QuantityStandardEquivalent_Unit 
        foreign key (QuantityStandardEquivalent_UnitId) 
        references ampedbizdb.UnitOfMeasures (UnitOfMeasureId);

    alter table ampedbizdb.ReturnItemBases 
        add index (Returned_CurrencyId), 
        add constraint FK_ReturnItemBase_Returned_Currency 
        foreign key (Returned_CurrencyId) 
        references ampedbizdb.Currencies (CurrencyId);

    create index IDX_ReturnItemBases_Product on ampedbizdb.ReturnItemBases (ProductId);

    create index IDX_ReturnItemBases_Reason on ampedbizdb.ReturnItemBases (ReasonId);

    create index IDX_ReturnItemBase_Quantity_Unit on ampedbizdb.ReturnItemBases (Quantity_UnitId);

    create index IDX_ReturnItemBase_Standard_Unit on ampedbizdb.ReturnItemBases (Standard_UnitId);

    create index IDX_ReturnItemBase_QuantityStandardEquivalent_Unit on ampedbizdb.ReturnItemBases (QuantityStandardEquivalent_UnitId);

    create index IDX_ReturnItemBase_Returned_Currency on ampedbizdb.ReturnItemBases (Returned_CurrencyId);

    alter table ampedbizdb.OrderReturns 
        add index (ReturnItemBaseId), 
        add constraint FK_OrderReturns_ReturnItemBase 
        foreign key (ReturnItemBaseId) 
        references ampedbizdb.ReturnItemBases (ReturnItemBaseId);

    create index IDX_OrderReturns_ReturnItemBase on ampedbizdb.OrderReturns (ReturnItemBaseId);

    alter table ampedbizdb.ReturnItems 
        add index (ReturnItemBaseId), 
        add constraint FK_ReturnItems_ReturnItemBase 
        foreign key (ReturnItemBaseId) 
        references ampedbizdb.ReturnItemBases (ReturnItemBaseId);

    create index IDX_ReturnItems_ReturnItemBase on ampedbizdb.ReturnItems (ReturnItemBaseId);