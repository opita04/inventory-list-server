# Technical Design: Dynamic Inventory Templates & Persistent Layouts

## Goal
The goal is to modernize the Inventory List application by replacing the hardcoded column system with a dynamic, template-based approach and adding persistent column width settings—all while maintaining the "generic for any server" portability of SQLite.

## Core Features

### 1. Unified Storage (Generic & Portable)
All data, including templates and layout settings, will continue to reside in the single `inventory.db` file.
*   **New Migration**: Add `InventoryTemplates` table.
*   **Update**: Link `InventoryGroups` to `InventoryTemplateId`.
*   **Persistence**: Add a `ColumnSettingsJson` field to `InventoryGroups` to store per-group column widths.

### 2. Template Management System
*   **InventoryTemplate Model**:
    | Field | Type | Description |
    |-------|------|-------------|
    | Id | int | Primary Key |
    | Name | string | e.g., "Server", "Workstation" |
    | MappingsJson | string | JSON array of `{ Header: string, Property: string }` |
*   **UI**: A new management interface to create, edit, and delete templates.

### 3. Modern Compact View
*   **Styling**:
    *   Transition to a "Compact" grid with reduced row height and font sizes (Inter/Roboto).
    *   Zebra-striping for readability.
    *   Sticky headers for easy scrolling.
*   **Auto-Fit & Resizing**:
    *   Columns will auto-fit content by default.
    *   Manual resize handles will trigger an update to the `ColumnSettingsJson` in the database for persistence.

## Technical Components

### Frontend: Interactive Grid
*   The `Inventory.razor` page will be updated to render `GridColumn` components dynamically by iterating over the `MappingsJson` from the group's assigned template.
*   Use Blazor JS Interop to capture column resize events and sync them back to the server.

### Backend: Service Layer
*   `InventoryService`: Will handle CRUD for templates and groups.
*   `ExcelPasteParser`: Will be updated to use the template mappings instead of a hardcoded array.

## Verification Plan
*   **Manual**: Create a "Printer" template with 3 columns. Assign it to a group. Verify the grid only shows those 3 columns.
*   **Manual**: Resize a column, refresh the page, and verify the width is preserved.
*   **Cross-Server**: Move the `inventory.db` to a different machine and verify the settings persist.
