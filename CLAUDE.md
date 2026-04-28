# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```bash
dotnet restore                                              # Restore NuGet packages
dotnet build app_agenda.slnx                               # Build entire solution
dotnet run --project app_agenda/app_agenda.UI.csproj       # Run the application
dotnet build app_agenda.slnx --configuration Release       # Release build
dotnet test                                                # Run tests (project is empty for now)
```

**Prerequisite:** SQL Server Express must be running locally. The connection string targets `.\SQLEXPRESS`, database `agenda_bd`. EF Core migrations create the schema on first run.

## Architecture

Three-project solution (`app_agenda.slnx`):

| Project | Role |
|---|---|
| `app_agenda.Data/` | EF Core models, `AgendaContext`, migrations |
| `app_agenda/` | WinForms UI, services, entry point |
| `app_agenda.UI.Test/` | Test project (empty, infrastructure only) |

### Data Layer (`app_agenda.Data/`)

- `AgendaContext.cs` — single DbContext; applies a global query filter `IsDeleted == false` on all entities (soft-delete pattern). All deletions must set `IsDeleted = true` rather than calling `Remove()`.
- Models: `User`, `Contact`, `Category`, `TodoItem`. All are scoped per-user: `Category`, `Contact`, and `TodoItem` have a `UserId` FK.
- `Category` seeds 4 defaults per user (*Personal, Trabajo, Familia, General*) via `CategoryService`.

### Service Layer (`app_agenda/Services/`)

Each service creates its own `AgendaContext` instance per operation (using-statement pattern — no shared context).

- `AuthService` — login/register with BCrypt hashing.
- `CategoryService` — CRUD + auto-create default categories.
- `ContactService` — CRUD, filter by category/favorites, search by name or phone.
- `TodoService` — CRUD, implements `Aplicada1.Core.IService<T>` (educational library contract).

### UI Layer (`app_agenda/`)

Entry point: `Program.cs` → `new LoginForm()`. `MainForm` is instantiated by `LoginForm` after successful auth, receiving `userId` and `username`.

**MainForm layout:**
```
Sidebar (250px, #249EA0) | Header (60px, white)
  Logo + title           |   Welcome label + Logout
  Category buttons       |
  Todos / Favoritos btns | Content panel (#F0F2F5)
  Add Category btn       |   ContactCards or TodoItemControls
```

- Sidebar category buttons are generated dynamically from the database.
- Content panel is cleared and repopulated when the user switches views.
- `ContactCard` (UserControl) — card per contact; double-click opens `ContactForm` for editing.
- `TodoItemControl` (UserControl) — renders a single todo with a checkbox.
- Popup dialogs live in `Popups/`: `ContactForm` (add/edit contact), `AddCategoryForm` (new category with icon picker).

## Key Conventions

- **UI language:** Spanish throughout (labels, comments, form text).
- **Color scheme:** Teal `#249EA0` sidebar, `#F0F2F5` content background, `#8DB3E2` contact cards.
- **Icons:** FontAwesome.Sharp — use `IconChar` enum values for any new icon buttons.
- **Password hashing:** Always use `BCrypt.Net.BCrypt` via `AuthService`; never store plaintext passwords.
- **Soft delete:** Set `entity.IsDeleted = true` and call `SaveChanges()`; never call `context.Remove()` on user-facing entities.

## CI/CD

GitHub Actions (`.github/workflows/main.yml`) triggers on push/PR to `main`/`master`: restore → build (Release) → test on `windows-latest` with .NET 10.0 prerelease.
