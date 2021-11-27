CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "AuthenticationProviders" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_AuthenticationProviders" PRIMARY KEY,
    "Type" TEXT NULL,
    "Enabled" INTEGER NOT NULL,
    "Name" TEXT NULL,
    "DisplayName" TEXT NULL,
    "Description" TEXT NULL,
    "Parameters" TEXT NULL
);

CREATE TABLE "Roles" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Roles" PRIMARY KEY,
    "Name" TEXT NULL,
    "DisplayName" TEXT NULL,
    "Description" TEXT NULL,
    "BuiltIn" INTEGER NOT NULL,
    "ClientId" TEXT NOT NULL
);

CREATE TABLE "Users" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Users" PRIMARY KEY,
    "Subject" TEXT NULL,
    "UserName" TEXT NULL,
    "Email" TEXT NULL,
    "Password" TEXT NULL,
    "Active" INTEGER NOT NULL,
    "FirstName" TEXT NULL,
    "LastName" TEXT NULL,
    "PhoneNumber" TEXT NULL,
    "EmailConfirmed" INTEGER NOT NULL,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "SecurityCode" TEXT NULL,
    "SecurityCodeExpirationDate" TEXT NOT NULL
);

CREATE TABLE "WhoClients" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_WhoClients" PRIMARY KEY,
    "ClientId" TEXT NULL,
    "ClientSecret" TEXT NULL,
    "ConcurrencyToken" TEXT NULL,
    "ConsentType" TEXT NULL,
    "DisplayName" TEXT NULL,
    "DisplayNames" TEXT NULL,
    "Permissions" TEXT NULL,
    "Properties" TEXT NULL,
    "Requirements" TEXT NULL,
    "Type" TEXT NULL,
    "AccessTokenLifeTime" INTEGER NULL,
    "RefreshTokenLifeTime" INTEGER NULL,
    "Description" TEXT NULL
);

CREATE TABLE "WhoScopes" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_WhoScopes" PRIMARY KEY,
    "ConcurrencyToken" TEXT NULL,
    "Description" TEXT NULL,
    "Descriptions" TEXT NULL,
    "DisplayName" TEXT NULL,
    "DisplayNames" TEXT NULL,
    "Name" TEXT NULL,
    "Properties" TEXT NULL,
    "Resources" TEXT NULL
);

CREATE TABLE "RoleUser" (
    "RolesId" TEXT NOT NULL,
    "UsersId" TEXT NOT NULL,
    CONSTRAINT "PK_RoleUser" PRIMARY KEY ("RolesId", "UsersId"),
    CONSTRAINT "FK_RoleUser_Roles_RolesId" FOREIGN KEY ("RolesId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_RoleUser_Users_UsersId" FOREIGN KEY ("UsersId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "UserClaims" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_UserClaims" PRIMARY KEY,
    "Type" TEXT NOT NULL,
    "Value" TEXT NOT NULL,
    "ConcurrencyStamp" TEXT NULL,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientPostLogoutRedirectUri" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ClientPostLogoutRedirectUri" PRIMARY KEY,
    "PostLogoutRedirectUri" TEXT NULL,
    "ClientId" TEXT NOT NULL,
    CONSTRAINT "FK_ClientPostLogoutRedirectUri_WhoClients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "WhoClients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "ClientRedirectUri" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_ClientRedirectUri" PRIMARY KEY,
    "RedirectUri" TEXT NULL,
    "ClientId" TEXT NOT NULL,
    CONSTRAINT "FK_ClientRedirectUri_WhoClients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "WhoClients" ("Id") ON DELETE CASCADE
);

CREATE TABLE "WhoAuthorizations" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_WhoAuthorizations" PRIMARY KEY,
    "ApplicationId" TEXT NULL,
    "ConcurrencyToken" TEXT NULL,
    "CreationDate" TEXT NULL,
    "Properties" TEXT NULL,
    "Scopes" TEXT NULL,
    "Status" TEXT NULL,
    "Subject" TEXT NULL,
    "Type" TEXT NULL,
    CONSTRAINT "FK_WhoAuthorizations_WhoClients_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES "WhoClients" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "WhoTokens" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_WhoTokens" PRIMARY KEY,
    "ApplicationId" TEXT NULL,
    "AuthorizationId" TEXT NULL,
    "ConcurrencyToken" TEXT NULL,
    "CreationDate" TEXT NULL,
    "ExpirationDate" TEXT NULL,
    "Payload" TEXT NULL,
    "Properties" TEXT NULL,
    "RedemptionDate" TEXT NULL,
    "ReferenceId" TEXT NULL,
    "Status" TEXT NULL,
    "Subject" TEXT NULL,
    "Type" TEXT NULL,
    CONSTRAINT "FK_WhoTokens_WhoAuthorizations_AuthorizationId" FOREIGN KEY ("AuthorizationId") REFERENCES "WhoAuthorizations" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_WhoTokens_WhoClients_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES "WhoClients" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_ClientPostLogoutRedirectUri_ClientId" ON "ClientPostLogoutRedirectUri" ("ClientId");

CREATE INDEX "IX_ClientRedirectUri_ClientId" ON "ClientRedirectUri" ("ClientId");

CREATE INDEX "IX_RoleUser_UsersId" ON "RoleUser" ("UsersId");

CREATE INDEX "IX_UserClaims_UserId" ON "UserClaims" ("UserId");

CREATE UNIQUE INDEX "IX_Users_Subject" ON "Users" ("Subject");

CREATE UNIQUE INDEX "IX_Users_UserName" ON "Users" ("UserName");

CREATE INDEX "IX_WhoAuthorizations_ApplicationId" ON "WhoAuthorizations" ("ApplicationId");

CREATE INDEX "IX_WhoTokens_ApplicationId" ON "WhoTokens" ("ApplicationId");

CREATE INDEX "IX_WhoTokens_AuthorizationId" ON "WhoTokens" ("AuthorizationId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20211031095118__2021-10-31T10-51-10', '5.0.10');

COMMIT;

