# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project

RateYourMusic-style API: ASP.NET Core 10 Web API (`net10.0`) over PostgreSQL via EF Core/Npgsql. Single project, `RymCloneApi.csproj`, all code under `src/` (namespaces literally include the folder: `RymCloneApi.src.*`).

## Commands

```powershell
dotnet tool restore                      # installs local tools: dotnet-ef, csharpier (manifest: dotnet-tools.json at repo root)
docker compose up -d                     # postgres:18 on :5432 (user/pass: docker/docker123)
dotnet ef database update                # apply migrations (nothing migrates automatically at startup)
dotnet run                               # http://localhost:5221
dotnet run --launch-profile https        # https://localhost:7229
dotnet build
dotnet csharpier format .                # formatting (honors .editorconfig: 2-space indent for .cs)
```

Migrations live in `src/Persistence/Migrations`, not the default location:

```powershell
dotnet ef migrations add <Name> -o src/Persistence/Migrations
```

`dotnet ef` works through `DesignTimeDbContextFactory`, which builds its own connection string — it must stay in sync with `AppDbContext.GetDbConectionString()`.

There is no test project in this repo.

Dev-only endpoints: Scalar API reference at `/api-docs`; `GET /healthcheck` is always available.

## Configuration

Config comes from a `.env` file (see `.env.example`), **not** from `appsettings.json` connection strings. `DotEnv.AutoConfig()` runs at the top of `Program.cs` and values are read through `EnvProvider.Instance` (a singleton dotenv `EnvReader`): `DB_HOST`, `DB_USERNAME`, `DB_PASSWORD`, `DB_DATABASE`, `APP_INSIGHTS_TELEMETRY_CONNECTION_STRING`. `.env.example` defaults to a local `postgres/sa` server; the docker-compose service uses `docker/docker123`, so pick one and make `.env` match.

## Architecture

Request flow: `Controller → Repository (queries only) → IUnitOfWork.Commit() → EF Core`.

**Routing.** All controllers derive from `ApplicationV1Controller`, which holds `[Route("/api/v1")]`. Action attributes carry the full relative path (`[HttpGet("albums/{id:int:min(1)}")]` → `/api/v1/albums/{id}`).

**Repositories.** Generic abstract `Repository<TEntity>` (`src/Persistence/Repositories/Repository.cs`) provides Get/GetMultiple/GetAll/Create/Update/Delete, with `params Expression<Func<T, object>>[] includes` overloads powered by the `IncludeMultiple` extension. Per-entity repositories subclass it and add specific queries (e.g. `AlbumsRepository.GetMostRecentAlbumAsync`). Repositories never call `SaveChanges` — controllers must `await _unitOfWork.Commit()` (which no-ops when the change tracker sees no changes). Each new repository interface/impl needs a `builder.Services.AddScoped` line in `Program.cs`.

**Entities & mapping.** Entities in `src/Domain/Entities` inherit `Entity`, whose constructor stamps `CreatedAt`/`UpdatedAt` (nothing refreshes `UpdatedAt` on save). Schema is defined by `IEntityTypeConfiguration` classes in `src/Persistence/Configurations`, auto-applied via `ApplyConfigurationsFromAssembly` — tables and columns are explicitly mapped to snake_case there, so the DataAnnotations on entities are decorative for schema purposes. Entity ⇄ DTO conversion is done by hand-written static extension methods in `src/Controllers/v1/DTOs/Extensions`; there is no AutoMapper.

**Validation.** Automatic ModelState validation is disabled (`SuppressModelStateInvalidFilter = true`), so invalid payloads do *not* auto-return 400. Validation is explicit: FluentValidation validators in `src/Domain/Validators`, invoked in the controller with `new XValidator().ValidateAndThrow(entity)`.

**Errors.** `IExceptionHandler` implementations registered in `Program.cs` (order matters; each returns `false` when the exception type doesn't match). Domain code throws `HttpException` subclasses — `NotFoundException`, `UnprocessableEntityException`, `InternalServerErrorException` — each with a matching handler that writes the project's own `src/Domain/ProblemDetails` as JSON (not the framework `ProblemDetails`). `DbUpdateException` from FK-restricted deletes is caught in controllers and rethrown as `UnprocessableEntityException`.

**JSON.** Both `System.Text.Json` (cycles ignored, fields included) and Newtonsoft are registered; Newtonsoft exists for JSON Patch. PATCH endpoints take `JsonPatchDocument<UpdateXRequestDTO>` with `[Consumes("application/json-patch+json")]`, apply the patch to the DTO, then re-resolve related entities before validating and committing.

**Seeding.** In Development only, `Program.cs` runs `AppDbContextInitializer.Seed()`, which calls `InitialDevSeeds` (Bogus-generated albums/users/reviews) — and only when every seeded table is empty.
