# Inventory List

A modern, streamlined inventory management system built with Blazor Server and .NET 8.

## 🚀 Features

-   **Inline Grid Editing**: Data-entry simplified. Edit any cell directly in the grid; changes are automatically persisted to the database on blur or enter.
-   **Collapsible Sidebar**: Maximize your workspace with a smooth, collapsible navigation menu (mini-sidebar mode).
-   **Row Management**: Effortlessly remove rows with a single click. Includes immediate visual feedback and database synchronization.
-   **Excel-Style Copy**: Copy the entire grid to your clipboard in tab-delimited format, ready to paste directly into Excel.
-   **Audit System**: Built-in change tracking that logs all modifications and batch additions for data integrity.
-   **History Viewing**: Dedicated history dashboard to review system logs and audit trails.

## 🛠️ Tech Stack

-   **Frontend**: Blazor Server (.NET 8)
-   **Style**: Vanilla CSS (Modern Compact)
-   **Database**: SQLite (Local & Portable)
-   **ORM**: Entity Framework Core

## 🏁 Getting Started

### Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation & Run

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/opita04/inventory-list-server.git
    cd inventory-list-server
    ```

2.  **Restore dependencies**:
    ```bash
    dotnet restore
    ```

3.  **Database Setup**:
    The application uses **SQLite** for local development. The database file (`inventory.db`) will be automatically initialized upon the first run.

4.  **Run the application**:
    ```bash
    dotnet run --project InventoryList
    ```

---

*Designed with Intentional Minimalism by the Frontend Architecture Team.*
