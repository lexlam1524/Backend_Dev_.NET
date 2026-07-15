# Robot Controller API

ASP.NET Core Web API for managing robot commands, maps, and authenticated users. The project demonstrates C# backend development, REST endpoints, PostgreSQL data access, dependency injection, Basic Authentication, role-based authorization, and Swagger/OpenAPI documentation.

## Tech Stack

- .NET 8 / ASP.NET Core Web API
- C#
- PostgreSQL
- Npgsql
- Entity Framework Core model mapping
- Basic Authentication
- Swagger / Swashbuckle

## Features

- CRUD endpoints for robot commands
- CRUD endpoints for maps
- User lookup and user management endpoints
- Password hashing with `PasswordHasher<UserModel>`
- Basic Authentication handler
- Role-based authorization policies for `Admin` and `User`
- Swagger UI for browser-based API testing

## Project Structure

- `Controllers/` - API controllers for maps, robot commands, and users
- `Models/` - request and entity models
- `Persistence/` - PostgreSQL data access and EF Core context
- `Authentication/` - Basic Authentication handler
- `41p-schema.sql` - database schema dump for the map and robot command tables
- `Program.cs` - dependency injection, authentication, authorization, and Swagger setup

## Prerequisites

- .NET 8 SDK
- PostgreSQL
- A local PostgreSQL database named `sit331`, or your own database name in the connection string

## Configuration

The application reads the database connection string from `ConnectionStrings:RobotDatabase`.

The committed `appsettings.json` contains a placeholder only:

```json
"ConnectionStrings": {
  "RobotDatabase": "Host=localhost;Database=sit331;Username=postgres;Password=YOUR_LOCAL_PASSWORD"
}
```

Do not commit your real database password. For local development, set the connection string with an environment variable before running the app.

PowerShell example:

```powershell
$env:ConnectionStrings__RobotDatabase="Host=localhost;Database=sit331;Username=postgres;Password=your_password"
dotnet run
```

The double underscore `__` maps to `:` in ASP.NET Core configuration, so `ConnectionStrings__RobotDatabase` becomes `ConnectionStrings:RobotDatabase`.

## Database Setup

Create a PostgreSQL database named `sit331`, then run the schema script:

```powershell
psql -U postgres -d sit331 -f 41p-schema.sql
```

Current note: `41p-schema.sql` contains the `map` and `robotcommand` tables. The authentication and user endpoints also expect a `users` table with columns used by `UserADO.cs`: `id`, `email`, `firstname`, `lastname`, `passwordhash`, `description`, `role`, `createddate`, and `modifieddate`.

## Run the API

```powershell
dotnet restore
dotnet build
dotnet run
```

The default development URLs are configured in `Properties/launchSettings.json`:

- `https://localhost:7253`
- `http://localhost:5239`

## Swagger UI

When running in Development mode, open:

```text
https://localhost:7253/swagger
```

Swagger lists the available endpoints and includes Basic Authentication support through the `Authorize` button.

## Main Endpoints

- `GET /api/robot-commands`
- `GET /api/robot-commands/move`
- `POST /api/robot-commands`
- `GET /api/maps`
- `GET /api/maps/square`
- `GET /api/maps/{id}/{x}-{y}`
- `GET /api/users`
- `GET /api/users/admin`

Some endpoints require a valid Basic Auth user and role.

## Build Status

The project currently builds with warnings:

```powershell
dotnet build
```

Known remaining cleanup items:

- Upgrade vulnerable/outdated NuGet packages
- Replace obsolete `ISystemClock` usage in the authentication handler
- Clean nullable reference warnings
- Add automated tests
- Add a complete user-table setup/seed script
