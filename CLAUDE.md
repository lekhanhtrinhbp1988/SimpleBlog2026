# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Current state

This repository is a freshly generated skeleton. The only application code is what `dotnet new mvc` and `dotnet new xunit` produced (`HomeController`, `ErrorViewModel`, an empty `UnitTest1`). There is no DbContext, entity, migration, service layer, or real test yet, so there are no established patterns to follow beyond stock ASP.NET Core MVC.

## Commands

Run from the repository root; `dotnet` picks up `SimpleBlog.slnx` automatically.

```powershell
dotnet build
dotnet test
dotnet test --filter "FullyQualifiedName~SimpleBlog.Tests.UnitTest1.Test1"   # single test
dotnet test --filter "FullyQualifiedName~SomeTestClass"                      # single class
dotnet run --project src/SimpleBlog.Web                                      # http://localhost:5227
dotnet run --project src/SimpleBlog.Web --launch-profile https               # https://localhost:7276
```

EF Core CLI is a local tool pinned in `dotnet-tools.json` at the repository root (not in `.config/`). Restore it once, then call it through `dotnet`:

```powershell
dotnet tool restore
dotnet ef migrations add <Name> --project src/SimpleBlog.Web
dotnet ef database update --project src/SimpleBlog.Web
```

These `ef` commands will fail until a DbContext exists and is registered in `Program.cs`.

## Toolchain

- `global.json` pins SDK `10.0.201` with no `rollForward`, so that exact feature band must be installed.
- Both projects target `net10.0` with nullable reference types and implicit usings enabled.
- The solution file is `SimpleBlog.slnx` (the XML format that SDK 10 creates by default), not a `.sln`.
- Tests use xUnit v2 (`xunit` 2.9.3) with a global `using Xunit`.
- `.gitattributes` stores text files with LF in the repository; Git checks them out with the platform's line ending. `.editorconfig` sets UTF-8 without BOM, except `.cshtml`, which is UTF-8 with BOM so Vietnamese text survives every tool. Run `dotnet format` before committing; CI fails on `dotnet format --verify-no-changes`.

## Git and CI

- The main branch is `main`, hosted at `github.com/lekhanhtrinhbp1988/SimpleBlog2026`. Changes reach `main` only through pull requests; feature branches are named `feature/<feature>` in English kebab-case.
- `.github/workflows/ci.yml` runs `dotnet build` and `dotnet test` on Ubuntu for every push to `main` and every pull request into it. It does not provide SQL Server LocalDB, so tests that need a database will need a different setup there.
- `.claude/settings.json` denies `git push` to agents; the user pushes.

## Structure

- `src/SimpleBlog.Web`: ASP.NET Core MVC app, no authentication. `Program.cs` uses the minimal hosting model with the conventional `{controller=Home}/{action=Index}/{id?}` route and `MapStaticAssets`.
- `tests/SimpleBlog.Tests`: xUnit project that references the Web project and has `Microsoft.AspNetCore.Mvc.Testing`, so integration tests can use `WebApplicationFactory<Program>`.

## Database

- The Web project references `Microsoft.EntityFrameworkCore.SqlServer` and `Microsoft.EntityFrameworkCore.Design`, but nothing uses them yet.
- The connection string `DefaultConnection` exists only in `src/SimpleBlog.Web/appsettings.Development.json` and points at SQL Server LocalDB: `(localdb)\MSSQLLocalDB`, database `SimpleBlog`. `appsettings.json` has no connection string, so any non-Development environment has none.
- Only run `ef` or SQL commands against `(localdb)\MSSQLLocalDB`.
