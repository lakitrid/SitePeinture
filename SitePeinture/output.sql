CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK_HistoryRow" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

CREATE TABLE "AspNetRoles" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_IdentityRole" PRIMARY KEY,
    "ConcurrencyStamp" TEXT,
    "Name" TEXT,
    "NormalizedName" TEXT
);

CREATE TABLE "AspNetUsers" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_User" PRIMARY KEY,
    "AccessFailedCount" INTEGER NOT NULL,
    "ConcurrencyStamp" TEXT,
    "Email" TEXT,
    "EmailConfirmed" INTEGER NOT NULL,
    "LockoutEnabled" INTEGER NOT NULL,
    "LockoutEnd" TEXT,
    "NormalizedEmail" TEXT,
    "NormalizedUserName" TEXT,
    "PasswordHash" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" INTEGER NOT NULL,
    "SecurityStamp" TEXT,
    "TwoFactorEnabled" INTEGER NOT NULL,
    "UserName" TEXT
);

CREATE TABLE "AspNetRoleClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_IdentityRoleClaim<string>" PRIMARY KEY AUTOINCREMENT,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "FK_IdentityRoleClaim<string>_IdentityRole_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserClaims" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_IdentityUserClaim<string>" PRIMARY KEY AUTOINCREMENT,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "FK_IdentityUserClaim<string>_User_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT,
    "UserId" TEXT NOT NULL,
    CONSTRAINT "PK_IdentityUserLogin<string>" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_IdentityUserLogin<string>_User_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "AspNetUserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    CONSTRAINT "PK_IdentityUserRole<string>" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_IdentityUserRole<string>_IdentityRole_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_IdentityUserRole<string>_User_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

CREATE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");

CREATE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");

CREATE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20151202221457_FirstMigration', '7.0.0-rc1-16348');


