# Server Inventory App Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a Blazor .NET 8 server inventory app with SQL Server, inventory groups, bulk Excel paste, and a filterable/sortable grid using Blazor.Bootstrap.

**Architecture:** EF Core SQL Server backend with two entities (InventoryGroup, InventoryItem). Blazor Server interactive components with a sidebar for group navigation and a main grid area. Bulk paste modal parses tab-delimited Excel clipboard data with preview before save.

**Tech Stack:** .NET 8, Blazor Server, EF Core 8 (SQL Server), Blazor.Bootstrap 3.x

---

### Task 1: Models

**Files:**
- Create: `Models/InventoryGroup.cs`
- Create: `Models/InventoryItem.cs`

- [ ] **Step 1:** Create InventoryGroup model with Id, Name, Description, CreatedOn
- [ ] **Step 2:** Create InventoryItem model with all 19 specified columns + Id + GroupId FK

### Task 2: DbContext + Configuration

**Files:**
- Create: `Data/InventoryDbContext.cs`
- Modify: `appsettings.json` — add ConnectionStrings
- Modify: `Program.cs` — register DbContext and Blazor.Bootstrap

- [ ] **Step 1:** Create InventoryDbContext with DbSets
- [ ] **Step 2:** Add connection string to appsettings.json
- [ ] **Step 3:** Register DbContext and BlazorBootstrap in Program.cs

### Task 3: Parsing Service

**Files:**
- Create: `Services/ExcelPasteParser.cs`

- [ ] **Step 1:** Create static parser that splits by newlines then tabs, maps to InventoryItem list

### Task 4: App.razor + _Imports.razor + CSS setup for Blazor.Bootstrap

**Files:**
- Modify: `Components/App.razor` — add Blazor.Bootstrap CSS/JS
- Modify: `Components/_Imports.razor` — add using statements

- [ ] **Step 1:** Add Blazor.Bootstrap CDN links and BlazorBootstrap icons
- [ ] **Step 2:** Add @using BlazorBootstrap to _Imports.razor

### Task 5: Layout — Sidebar + Main Area

**Files:**
- Modify: `Components/Layout/MainLayout.razor` — custom sidebar layout
- Modify: `Components/Layout/MainLayout.razor.css` — fluid layout styles

- [ ] **Step 1:** Replace default layout with sidebar (group list) + main content area

### Task 6: Inventory Page — Grid + Bulk Add Modal

**Files:**
- Create: `Components/Pages/Inventory.razor` — main page with grid, modal, sidebar interaction

- [ ] **Step 1:** Build sidebar group list with add/select
- [ ] **Step 2:** Build Blazor.Bootstrap Grid with all columns, sorting, filtering
- [ ] **Step 3:** Build Bulk Add modal with header row, textarea, parse, preview table, save
- [ ] **Step 4:** Wire up EF Core CRUD operations

### Task 7: EF Core Migration

- [ ] **Step 1:** Create initial migration
- [ ] **Step 2:** Verify build succeeds
