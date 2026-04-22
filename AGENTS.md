# AGENTS.md

## Build & Run

```bash
dotnet build app_agenda.slnx
dotnet run --project app_agenda/app_agenda.UI.csproj
```

## Architecture

- **Solution**: 3 projects in `app_agenda.slnx`
  - `app_agenda.Data` - EF Core data layer (SQL Server)
  - `app_agenda.UI` - WinForms UI (executable)
  - `app_agenda.UI.Test` - Empty test project (no test framework)

- **Database**: SQL Express (`.\SQLEXPRESS`), database `agenda_bd`, uses soft-delete (global query filter on `IsDeleted`)

- **Entry point**: `app_agenda/Program.cs` runs `MainForm` (not `LoginForm`)

## Testing

Test project is empty - add a test framework (e.g., xUnit, NUnit) before writing tests.