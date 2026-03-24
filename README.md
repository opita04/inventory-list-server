# Inventory List Server

A high-performance **Blazor Server** application designed for efficient server inventory management. Built on **.NET 8**, it features a robust SQL Server backend, interactive UI components, and a specialized bulk-paste system for Excel data.

## 🚀 Features

-   **Interactive Grid**: Real-time filtering, sorting, and pagination powered by `Blazor.Bootstrap`.
-   **Excel Bulk Paste**: Unique tab-delimited parser to import server lists directly from Excel spreadsheets with a preview step.
-   **Group Management**: Organize inventory into logical groups with a dynamic sidebar navigation.
-   **Modern Architecture**: Clean separation of concerns with EF Core 8 and a sleek, responsive design.

## 🛠️ Tech Stack

-   **Frontend**: Blazor Server, Blazor.Bootstrap 3.x
-   **Backend**: .NET 8, Entity Framework Core 8
-   **Database**: SQL Server
-   **Design**: Vanilla CSS with a focus on visual hierarchy and whitespace.

## 📋 Getting Started

### Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   SQL Server (LocalDB or full instance)

### Installation

1.  **Clone the repository**:
    ```bash
    git clone https://github.com/opita04/inventory-list-server.git
    cd inventory-list-server
    ```

2.  **Configure Database**:
    Update the connection string in `InventoryList/appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InventoryDb;Trusted_Connection=True;MultipleActiveResultSets=true"
    }
    ```

3.  **Apply Migrations**:
    ```bash
    dotnet ef database update --project InventoryList
    ```

4.  **Run the application**:
    ```bash
    dotnet run --project InventoryList
    ```

## 🏗️ Project Structure

-   `/InventoryList`: Core Blazor Server application.
    -   `/Components`: UI components and pages.
    -   `/Models`: Domain entities (`InventoryGroup`, `InventoryItem`).
    -   `/Services`: Business logic, including the Excel paste parser.
    -   `/Data`: EF Core DbContext and migrations.
-   `/docs`: Documentation and implementation plans.

---
*Created with ANTIGRAVITY — Senior Frontend Architect & Avant-Garde UI Designer.*
