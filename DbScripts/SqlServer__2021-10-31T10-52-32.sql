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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [AuthenticationProviders] (
        [Id] uniqueidentifier NOT NULL,
        [Type] nvarchar(max) NULL,
        [Enabled] bit NOT NULL,
        [Name] nvarchar(max) NULL,
        [DisplayName] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [Parameters] nvarchar(max) NULL,
        CONSTRAINT [PK_AuthenticationProviders] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [Roles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NULL,
        [DisplayName] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [BuiltIn] bit NOT NULL,
        [ClientId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [Subject] nvarchar(450) NULL,
        [UserName] nvarchar(450) NULL,
        [Email] nvarchar(max) NULL,
        [Password] nvarchar(max) NULL,
        [Active] bit NOT NULL,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnabled] bit NOT NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [SecurityCode] nvarchar(max) NULL,
        [SecurityCodeExpirationDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [WhoClients] (
        [Id] uniqueidentifier NOT NULL,
        [ClientId] nvarchar(max) NULL,
        [ClientSecret] nvarchar(max) NULL,
        [ConcurrencyToken] nvarchar(max) NULL,
        [ConsentType] nvarchar(max) NULL,
        [DisplayName] nvarchar(max) NULL,
        [DisplayNames] nvarchar(max) NULL,
        [Permissions] nvarchar(max) NULL,
        [Properties] nvarchar(max) NULL,
        [Requirements] nvarchar(max) NULL,
        [Type] nvarchar(max) NULL,
        [AccessTokenLifeTime] int NULL,
        [RefreshTokenLifeTime] int NULL,
        [Description] nvarchar(max) NULL,
        CONSTRAINT [PK_WhoClients] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [WhoScopes] (
        [Id] uniqueidentifier NOT NULL,
        [ConcurrencyToken] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [Descriptions] nvarchar(max) NULL,
        [DisplayName] nvarchar(max) NULL,
        [DisplayNames] nvarchar(max) NULL,
        [Name] nvarchar(max) NULL,
        [Properties] nvarchar(max) NULL,
        [Resources] nvarchar(max) NULL,
        CONSTRAINT [PK_WhoScopes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [RoleUser] (
        [RolesId] uniqueidentifier NOT NULL,
        [UsersId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_RoleUser] PRIMARY KEY ([RolesId], [UsersId]),
        CONSTRAINT [FK_RoleUser_Roles_RolesId] FOREIGN KEY ([RolesId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RoleUser_Users_UsersId] FOREIGN KEY ([UsersId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [UserClaims] (
        [Id] uniqueidentifier NOT NULL,
        [Type] nvarchar(250) NOT NULL,
        [Value] nvarchar(250) NOT NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [ClientPostLogoutRedirectUri] (
        [Id] uniqueidentifier NOT NULL,
        [PostLogoutRedirectUri] nvarchar(max) NULL,
        [ClientId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ClientPostLogoutRedirectUri] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientPostLogoutRedirectUri_WhoClients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [WhoClients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [ClientRedirectUri] (
        [Id] uniqueidentifier NOT NULL,
        [RedirectUri] nvarchar(max) NULL,
        [ClientId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ClientRedirectUri] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientRedirectUri_WhoClients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [WhoClients] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [WhoAuthorizations] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicationId] uniqueidentifier NULL,
        [ConcurrencyToken] nvarchar(max) NULL,
        [CreationDate] datetime2 NULL,
        [Properties] nvarchar(max) NULL,
        [Scopes] nvarchar(max) NULL,
        [Status] nvarchar(max) NULL,
        [Subject] nvarchar(max) NULL,
        [Type] nvarchar(max) NULL,
        CONSTRAINT [PK_WhoAuthorizations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WhoAuthorizations_WhoClients_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [WhoClients] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE TABLE [WhoTokens] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicationId] uniqueidentifier NULL,
        [AuthorizationId] uniqueidentifier NULL,
        [ConcurrencyToken] nvarchar(max) NULL,
        [CreationDate] datetime2 NULL,
        [ExpirationDate] datetime2 NULL,
        [Payload] nvarchar(max) NULL,
        [Properties] nvarchar(max) NULL,
        [RedemptionDate] datetime2 NULL,
        [ReferenceId] nvarchar(max) NULL,
        [Status] nvarchar(max) NULL,
        [Subject] nvarchar(max) NULL,
        [Type] nvarchar(max) NULL,
        CONSTRAINT [PK_WhoTokens] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_WhoTokens_WhoAuthorizations_AuthorizationId] FOREIGN KEY ([AuthorizationId]) REFERENCES [WhoAuthorizations] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_WhoTokens_WhoClients_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [WhoClients] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE INDEX [IX_ClientPostLogoutRedirectUri_ClientId] ON [ClientPostLogoutRedirectUri] ([ClientId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE INDEX [IX_ClientRedirectUri_ClientId] ON [ClientRedirectUri] ([ClientId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE INDEX [IX_RoleUser_UsersId] ON [RoleUser] ([UsersId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE INDEX [IX_UserClaims_UserId] ON [UserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Users_Subject] ON [Users] ([Subject]) WHERE [Subject] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Users_UserName] ON [Users] ([UserName]) WHERE [UserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE INDEX [IX_WhoAuthorizations_ApplicationId] ON [WhoAuthorizations] ([ApplicationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE INDEX [IX_WhoTokens_ApplicationId] ON [WhoTokens] ([ApplicationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    CREATE INDEX [IX_WhoTokens_AuthorizationId] ON [WhoTokens] ([AuthorizationId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20211031095129__2021-10-31T10-51-10')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20211031095129__2021-10-31T10-51-10', N'5.0.10');
END;
GO

COMMIT;
GO

