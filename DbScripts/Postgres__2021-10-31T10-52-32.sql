CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "AuthenticationProviders" (
        "Id" uuid NOT NULL,
        "Type" text NULL,
        "Enabled" boolean NOT NULL,
        "Name" text NULL,
        "DisplayName" text NULL,
        "Description" text NULL,
        "Parameters" text NULL,
        CONSTRAINT "PK_AuthenticationProviders" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "Roles" (
        "Id" uuid NOT NULL,
        "Name" text NULL,
        "DisplayName" text NULL,
        "Description" text NULL,
        "BuiltIn" boolean NOT NULL,
        "ClientId" uuid NOT NULL,
        CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Subject" text NULL,
        "UserName" text NULL,
        "Email" text NULL,
        "Password" text NULL,
        "Active" boolean NOT NULL,
        "FirstName" text NULL,
        "LastName" text NULL,
        "PhoneNumber" text NULL,
        "EmailConfirmed" boolean NOT NULL,
        "PhoneNumberConfirmed" boolean NOT NULL,
        "TwoFactorEnabled" boolean NOT NULL,
        "LockoutEnabled" boolean NOT NULL,
        "ConcurrencyStamp" text NULL,
        "SecurityCode" text NULL,
        "SecurityCodeExpirationDate" timestamp without time zone NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "WhoClients" (
        "Id" uuid NOT NULL,
        "ClientId" text NULL,
        "ClientSecret" text NULL,
        "ConcurrencyToken" text NULL,
        "ConsentType" text NULL,
        "DisplayName" text NULL,
        "DisplayNames" text NULL,
        "Permissions" text NULL,
        "Properties" text NULL,
        "Requirements" text NULL,
        "Type" text NULL,
        "AccessTokenLifeTime" integer NULL,
        "RefreshTokenLifeTime" integer NULL,
        "Description" text NULL,
        CONSTRAINT "PK_WhoClients" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "WhoScopes" (
        "Id" uuid NOT NULL,
        "ConcurrencyToken" text NULL,
        "Description" text NULL,
        "Descriptions" text NULL,
        "DisplayName" text NULL,
        "DisplayNames" text NULL,
        "Name" text NULL,
        "Properties" text NULL,
        "Resources" text NULL,
        CONSTRAINT "PK_WhoScopes" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "RoleUser" (
        "RolesId" uuid NOT NULL,
        "UsersId" uuid NOT NULL,
        CONSTRAINT "PK_RoleUser" PRIMARY KEY ("RolesId", "UsersId"),
        CONSTRAINT "FK_RoleUser_Roles_RolesId" FOREIGN KEY ("RolesId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_RoleUser_Users_UsersId" FOREIGN KEY ("UsersId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "UserClaims" (
        "Id" uuid NOT NULL,
        "Type" character varying(250) NOT NULL,
        "Value" character varying(250) NOT NULL,
        "ConcurrencyStamp" text NULL,
        "UserId" uuid NOT NULL,
        CONSTRAINT "PK_UserClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_UserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "ClientPostLogoutRedirectUri" (
        "Id" uuid NOT NULL,
        "PostLogoutRedirectUri" text NULL,
        "ClientId" uuid NOT NULL,
        CONSTRAINT "PK_ClientPostLogoutRedirectUri" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ClientPostLogoutRedirectUri_WhoClients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "WhoClients" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "ClientRedirectUri" (
        "Id" uuid NOT NULL,
        "RedirectUri" text NULL,
        "ClientId" uuid NOT NULL,
        CONSTRAINT "PK_ClientRedirectUri" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ClientRedirectUri_WhoClients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "WhoClients" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "WhoAuthorizations" (
        "Id" uuid NOT NULL,
        "ApplicationId" uuid NULL,
        "ConcurrencyToken" text NULL,
        "CreationDate" timestamp without time zone NULL,
        "Properties" text NULL,
        "Scopes" text NULL,
        "Status" text NULL,
        "Subject" text NULL,
        "Type" text NULL,
        CONSTRAINT "PK_WhoAuthorizations" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_WhoAuthorizations_WhoClients_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES "WhoClients" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE TABLE "WhoTokens" (
        "Id" uuid NOT NULL,
        "ApplicationId" uuid NULL,
        "AuthorizationId" uuid NULL,
        "ConcurrencyToken" text NULL,
        "CreationDate" timestamp without time zone NULL,
        "ExpirationDate" timestamp without time zone NULL,
        "Payload" text NULL,
        "Properties" text NULL,
        "RedemptionDate" timestamp without time zone NULL,
        "ReferenceId" text NULL,
        "Status" text NULL,
        "Subject" text NULL,
        "Type" text NULL,
        CONSTRAINT "PK_WhoTokens" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_WhoTokens_WhoAuthorizations_AuthorizationId" FOREIGN KEY ("AuthorizationId") REFERENCES "WhoAuthorizations" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_WhoTokens_WhoClients_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES "WhoClients" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE INDEX "IX_ClientPostLogoutRedirectUri_ClientId" ON "ClientPostLogoutRedirectUri" ("ClientId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE INDEX "IX_ClientRedirectUri_ClientId" ON "ClientRedirectUri" ("ClientId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE INDEX "IX_RoleUser_UsersId" ON "RoleUser" ("UsersId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE INDEX "IX_UserClaims_UserId" ON "UserClaims" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE UNIQUE INDEX "IX_Users_Subject" ON "Users" ("Subject");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE UNIQUE INDEX "IX_Users_UserName" ON "Users" ("UserName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE INDEX "IX_WhoAuthorizations_ApplicationId" ON "WhoAuthorizations" ("ApplicationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE INDEX "IX_WhoTokens_ApplicationId" ON "WhoTokens" ("ApplicationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    CREATE INDEX "IX_WhoTokens_AuthorizationId" ON "WhoTokens" ("AuthorizationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211031095124__2021-10-31T10-51-10') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211031095124__2021-10-31T10-51-10', '5.0.10');
    END IF;
END $EF$;
COMMIT;

