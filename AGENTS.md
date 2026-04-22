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
  - `app_agenda.UI.Test` - Empty test project

- **Database**: SQL Express (`.\SQLEXPRESS`), database `agenda_bd`, uses soft-delete (global query filter on `IsDeleted`)

- **Entry point**: `app_agenda/Program.cs` runs `LoginForm()` (MainForm is commented out on line 17)

- **Services** (`app_agenda/Services`):
  - `AuthService.cs` - Login/Register
  - `CategoryService.cs` - CRUD + EnsureDefaultCategories(Personal/Trabajo/Familia/General)
  - `ContactService.cs` - CRUD + filtering by category/favorites
  - `TodoService.cs` - CRUD for TodoItems

- **UserControls** (`app_agenda/UserControls`):
  - `ContactCard.cs` - Contact display card (#8DB3E2)
  - `TodoItemControl.cs` - Todo item with checkbox

- **Popups** (`app_agenda/Popups`):
  - `AddCategoryForm.cs` - Add new category with icon picker
  - `ContactForm.cs` - Add/Edit contact

- **MainForm Layout**:
  - `pnlSidebar` (250px, #249EA0) - Logo, Categories dynamic, Todos/Favoritos/ToDo buttons
  - `pnlHeader` (60px, White) - Welcome label + Logout
  - `pnlContent` (#F0F2F5) - Contact cards or Todo list

## Testing

Test project is empty - add a test framework (e.g., xUnit, NUnit) before writing tests.