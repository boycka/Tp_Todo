# Tp_TODO

An ASP.NET Core (.NET 9) web application implementing a simple TODO list with basic authentication, theming, and logging.

## Overview
- Architecture: Razor Pages-style views with MVC controllers and filters
- Target Framework: .NET 9
- Key Components:
  - `TodoController` with `Views/Todo/Index.cshtml` for listing and managing TODOs
  - `Views/User/Login.cshtml` for a simple login page
  - Filters:
    - `AuthFilter` for protecting routes
    - `ThemeFilter` for applying a UI theme
    - `LoggingFilter` for request/response logging
  - Services:
    - `IFileLog` and `FileLog` for file-based logging
  - Shared layout: `Views/Shared/_Layout.cshtml`
  - App startup: `Program.cs`

## What was implemented
- Basic TODO listing page rendered at `Views/Todo/Index.cshtml` handled by `TodoController`
- Simple login view at `Views/User/Login.cshtml`, with `AuthFilter` enforcing access control
- Theming support via `ThemeFilter` that adjusts styling in the layout
- Request logging via `LoggingFilter` backed by `FileLog` service writing to disk
- Common layout with navigation and theme hooks in `_Layout.cshtml`
- App bootstrapping and DI registrations in `Program.cs`

## Run the project
1. Ensure .NET 9 SDK is installed
2. From the solution directory, run:
   - `dotnet restore`
   - `dotnet build`
   - `dotnet run` (the app will start on the configured URL)

## Project structure (relevant parts)
- `Tp_TODO/Program.cs` – app configuration and service registrations
- `Tp_TODO/Controllers/TodoController.cs` – TODO endpoints
- `Tp_TODO/Views/Todo/Index.cshtml` – TODO list view
- `Tp_TODO/Views/User/Login.cshtml` – login view
- `Tp_TODO/Views/Shared/_Layout.cshtml` – shared layout
- `Tp_TODO/Filters/AuthFilter.cs` – authentication filter
- `Tp_TODO/Filters/ThemeFilter.cs` – theme filter
- `Tp_TODO/Filters/LoggingFilter.cs` – logging filter
- `Tp_TODO/Services/IFileLog.cs` & `Tp_TODO/Services/FileLog.cs` – logging service

## Notes
- Logs are written to a file via `FileLog`; ensure the app has write permissions to the log directory
- The filters demonstrate cross-cutting concerns without cluttering controllers/views
- The `.gitignore` excludes IDE folders, build outputs, and logs

