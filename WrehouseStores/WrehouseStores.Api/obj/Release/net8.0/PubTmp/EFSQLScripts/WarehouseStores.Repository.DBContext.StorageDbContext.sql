IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Customers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [CommercialRegister] int NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Departments] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Employees] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Job] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Floor] (
        [Id] int NOT NULL IDENTITY,
        [FloorNumber] int NOT NULL,
        CONSTRAINT [PK_Floor] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Priority] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Priority] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Shelf] (
        [Id] int NOT NULL IDENTITY,
        [ShelfNumber] int NOT NULL,
        CONSTRAINT [PK_Shelf] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Status] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Status] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Storage] (
        [Id] int NOT NULL IDENTITY,
        [Phone] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Location] nvarchar(max) NOT NULL,
        [Supervisor] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Storage] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Suppliers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Suppliers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [CategoryId] int NULL,
        [Unit] nvarchar(max) NOT NULL,
        [Notes] nvarchar(max) NULL,
        [ExpiryDate] datetime2 NOT NULL,
        [AddDate] datetime2 NOT NULL,
        [Quantity] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [SupplyFromWarehouses] (
        [Id] int NOT NULL IDENTITY,
        [AvailabilityDate] datetime2 NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [EmployeeId] int NOT NULL,
        [EmployeesId] int NULL,
        [PriorityId] int NOT NULL,
        [DepartmentId] int NOT NULL,
        [DepartmentsId] int NULL,
        CONSTRAINT [PK_SupplyFromWarehouses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SupplyFromWarehouses_Departments_DepartmentsId] FOREIGN KEY ([DepartmentsId]) REFERENCES [Departments] ([Id]),
        CONSTRAINT [FK_SupplyFromWarehouses_Employees_EmployeesId] FOREIGN KEY ([EmployeesId]) REFERENCES [Employees] ([Id]),
        CONSTRAINT [FK_SupplyFromWarehouses_Priority_PriorityId] FOREIGN KEY ([PriorityId]) REFERENCES [Priority] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [CustomerOrders] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [AvailabilityDate] datetime2 NOT NULL,
        [DepartmentId] int NULL,
        [ReasonOfOrder] nvarchar(max) NULL,
        [Weight] decimal(18,2) NOT NULL,
        [Time] datetime2 NOT NULL,
        [StatusId] int NULL,
        [Notes] nvarchar(max) NULL,
        [Phone] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        CONSTRAINT [PK_CustomerOrders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CustomerOrders_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]),
        CONSTRAINT [FK_CustomerOrders_Status_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Status] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [PurchaseOrders] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NULL,
        [DepartmentId] int NULL,
        [DateOfNeed] datetime2 NOT NULL,
        [AvailabilityDate] datetime2 NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [Weight] decimal(18,2) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PriorityId] int NULL,
        [StatusId] int NULL,
        [Phone] nvarchar(max) NULL,
        CONSTRAINT [PK_PurchaseOrders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PurchaseOrders_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]),
        CONSTRAINT [FK_PurchaseOrders_Priority_PriorityId] FOREIGN KEY ([PriorityId]) REFERENCES [Priority] ([Id]),
        CONSTRAINT [FK_PurchaseOrders_Status_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Status] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [ReadyOrders] (
        [Id] int NOT NULL IDENTITY,
        [Notes] nvarchar(max) NOT NULL,
        [Date] datetime2 NOT NULL,
        [Time] time NOT NULL,
        [StatusId] int NOT NULL,
        CONSTRAINT [PK_ReadyOrders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ReadyOrders_Status_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Status] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [CategoriesStorage] (
        [CategoriesId] int NOT NULL,
        [StoragesId] int NOT NULL,
        CONSTRAINT [PK_CategoriesStorage] PRIMARY KEY ([CategoriesId], [StoragesId]),
        CONSTRAINT [FK_CategoriesStorage_Categories_CategoriesId] FOREIGN KEY ([CategoriesId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CategoriesStorage_Storage_StoragesId] FOREIGN KEY ([StoragesId]) REFERENCES [Storage] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [DepartmentsStorage] (
        [DepartmentsId] int NOT NULL,
        [StoragesId] int NOT NULL,
        CONSTRAINT [PK_DepartmentsStorage] PRIMARY KEY ([DepartmentsId], [StoragesId]),
        CONSTRAINT [FK_DepartmentsStorage_Departments_DepartmentsId] FOREIGN KEY ([DepartmentsId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_DepartmentsStorage_Storage_StoragesId] FOREIGN KEY ([StoragesId]) REFERENCES [Storage] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [FloorStorage] (
        [FloorsId] int NOT NULL,
        [StoragesId] int NOT NULL,
        CONSTRAINT [PK_FloorStorage] PRIMARY KEY ([FloorsId], [StoragesId]),
        CONSTRAINT [FK_FloorStorage_Floor_FloorsId] FOREIGN KEY ([FloorsId]) REFERENCES [Floor] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_FloorStorage_Storage_StoragesId] FOREIGN KEY ([StoragesId]) REFERENCES [Storage] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [ShelfStorage] (
        [ShelvesId] int NOT NULL,
        [StoragesId] int NOT NULL,
        CONSTRAINT [PK_ShelfStorage] PRIMARY KEY ([ShelvesId], [StoragesId]),
        CONSTRAINT [FK_ShelfStorage_Shelf_ShelvesId] FOREIGN KEY ([ShelvesId]) REFERENCES [Shelf] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ShelfStorage_Storage_StoragesId] FOREIGN KEY ([StoragesId]) REFERENCES [Storage] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [Bill] (
        [Id] int NOT NULL IDENTITY,
        [Phone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Date] datetime2 NOT NULL,
        [Weight] decimal(18,2) NOT NULL,
        [PriorityId] int NULL,
        [SupplierId] int NULL,
        [Notes] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [StatusId] int NULL,
        [DepartmentId] int NULL,
        [CustomerId] int NOT NULL,
        CONSTRAINT [PK_Bill] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Bill_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Bill_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]),
        CONSTRAINT [FK_Bill_Priority_PriorityId] FOREIGN KEY ([PriorityId]) REFERENCES [Priority] ([Id]),
        CONSTRAINT [FK_Bill_Status_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [Status] ([Id]),
        CONSTRAINT [FK_Bill_Suppliers_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [SupplyFromInventoryToSales] (
        [Id] int NOT NULL IDENTITY,
        [Description] nvarchar(max) NULL,
        [Quantity] decimal(18,2) NULL,
        [Notes] nvarchar(max) NULL,
        [DateOfSupply] datetime2 NULL,
        [ExpectedDeliveryDate] datetime2 NULL,
        [ProductId] int NOT NULL,
        [DepartmentId] int NOT NULL,
        CONSTRAINT [PK_SupplyFromInventoryToSales] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_SupplyFromInventoryToSales_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SupplyFromInventoryToSales_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [CustomerOrdersProducts] (
        [CustomerOrdersId] int NOT NULL,
        [ProductsId] int NOT NULL,
        CONSTRAINT [PK_CustomerOrdersProducts] PRIMARY KEY ([CustomerOrdersId], [ProductsId]),
        CONSTRAINT [FK_CustomerOrdersProducts_CustomerOrders_CustomerOrdersId] FOREIGN KEY ([CustomerOrdersId]) REFERENCES [CustomerOrders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CustomerOrdersProducts_Products_ProductsId] FOREIGN KEY ([ProductsId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductsPurchaseOrders] (
        [ProductsId] int NOT NULL,
        [PurchaseOrdersId] int NOT NULL,
        CONSTRAINT [PK_ProductsPurchaseOrders] PRIMARY KEY ([ProductsId], [PurchaseOrdersId]),
        CONSTRAINT [FK_ProductsPurchaseOrders_Products_ProductsId] FOREIGN KEY ([ProductsId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProductsPurchaseOrders_PurchaseOrders_PurchaseOrdersId] FOREIGN KEY ([PurchaseOrdersId]) REFERENCES [PurchaseOrders] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [CustomerOrdersReadyOrders] (
        [CustomerOrdersId] int NOT NULL,
        [ReadyOrdersId] int NOT NULL,
        CONSTRAINT [PK_CustomerOrdersReadyOrders] PRIMARY KEY ([CustomerOrdersId], [ReadyOrdersId]),
        CONSTRAINT [FK_CustomerOrdersReadyOrders_CustomerOrders_CustomerOrdersId] FOREIGN KEY ([CustomerOrdersId]) REFERENCES [CustomerOrders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_CustomerOrdersReadyOrders_ReadyOrders_ReadyOrdersId] FOREIGN KEY ([ReadyOrdersId]) REFERENCES [ReadyOrders] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [BillProducts] (
        [BillsId] int NOT NULL,
        [ProductsId] int NOT NULL,
        CONSTRAINT [PK_BillProducts] PRIMARY KEY ([BillsId], [ProductsId]),
        CONSTRAINT [FK_BillProducts_Bill_BillsId] FOREIGN KEY ([BillsId]) REFERENCES [Bill] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_BillProducts_Products_ProductsId] FOREIGN KEY ([ProductsId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [ReceivedOrders] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Notes] nvarchar(max) NOT NULL,
        [Date] datetime2 NOT NULL,
        [Time] nvarchar(max) NOT NULL,
        [DepartmentId] int NULL,
        [StorageId] int NULL,
        [Weight] decimal(18,2) NOT NULL,
        [PurchaseOrderId] int NULL,
        [BillId] int NOT NULL,
        CONSTRAINT [PK_ReceivedOrders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ReceivedOrders_Bill_BillId] FOREIGN KEY ([BillId]) REFERENCES [Bill] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ReceivedOrders_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ReceivedOrders_PurchaseOrders_PurchaseOrderId] FOREIGN KEY ([PurchaseOrderId]) REFERENCES [PurchaseOrders] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ReceivedOrders_Storage_StorageId] FOREIGN KEY ([StorageId]) REFERENCES [Storage] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE TABLE [ProductsReceivedOrders] (
        [ProductsId] int NOT NULL,
        [ReceivedOrdersId] int NOT NULL,
        CONSTRAINT [PK_ProductsReceivedOrders] PRIMARY KEY ([ProductsId], [ReceivedOrdersId]),
        CONSTRAINT [FK_ProductsReceivedOrders_Products_ProductsId] FOREIGN KEY ([ProductsId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProductsReceivedOrders_ReceivedOrders_ReceivedOrdersId] FOREIGN KEY ([ReceivedOrdersId]) REFERENCES [ReceivedOrders] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Bill_CustomerId] ON [Bill] ([CustomerId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Bill_DepartmentId] ON [Bill] ([DepartmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Bill_PriorityId] ON [Bill] ([PriorityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Bill_StatusId] ON [Bill] ([StatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Bill_SupplierId] ON [Bill] ([SupplierId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_BillProducts_ProductsId] ON [BillProducts] ([ProductsId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CategoriesStorage_StoragesId] ON [CategoriesStorage] ([StoragesId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CustomerOrders_DepartmentId] ON [CustomerOrders] ([DepartmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CustomerOrders_StatusId] ON [CustomerOrders] ([StatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CustomerOrdersProducts_ProductsId] ON [CustomerOrdersProducts] ([ProductsId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_CustomerOrdersReadyOrders_ReadyOrdersId] ON [CustomerOrdersReadyOrders] ([ReadyOrdersId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_DepartmentsStorage_StoragesId] ON [DepartmentsStorage] ([StoragesId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_FloorStorage_StoragesId] ON [FloorStorage] ([StoragesId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductsPurchaseOrders_PurchaseOrdersId] ON [ProductsPurchaseOrders] ([PurchaseOrdersId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ProductsReceivedOrders_ReceivedOrdersId] ON [ProductsReceivedOrders] ([ReceivedOrdersId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrders_DepartmentId] ON [PurchaseOrders] ([DepartmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrders_PriorityId] ON [PurchaseOrders] ([PriorityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_PurchaseOrders_StatusId] ON [PurchaseOrders] ([StatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ReadyOrders_StatusId] ON [ReadyOrders] ([StatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ReceivedOrders_BillId] ON [ReceivedOrders] ([BillId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ReceivedOrders_DepartmentId] ON [ReceivedOrders] ([DepartmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_ReceivedOrders_PurchaseOrderId] ON [ReceivedOrders] ([PurchaseOrderId]) WHERE [PurchaseOrderId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ReceivedOrders_StorageId] ON [ReceivedOrders] ([StorageId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_ShelfStorage_StoragesId] ON [ShelfStorage] ([StoragesId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SupplyFromInventoryToSales_DepartmentId] ON [SupplyFromInventoryToSales] ([DepartmentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SupplyFromInventoryToSales_ProductId] ON [SupplyFromInventoryToSales] ([ProductId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SupplyFromWarehouses_DepartmentsId] ON [SupplyFromWarehouses] ([DepartmentsId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SupplyFromWarehouses_EmployeesId] ON [SupplyFromWarehouses] ([EmployeesId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_SupplyFromWarehouses_PriorityId] ON [SupplyFromWarehouses] ([PriorityId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708092521_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240708092521_InitialCreate', N'8.0.6');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708095742_UpdateTimeType'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ReadyOrders]') AND [c].[name] = N'Time');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [ReadyOrders] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [ReadyOrders] ALTER COLUMN [Time] nvarchar(max) NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240708095742_UpdateTimeType'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240708095742_UpdateTimeType', N'8.0.6');
END;
GO

COMMIT;
GO

