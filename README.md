# 🏠 Real Estate Management System

[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-512BD4?logo=.net&logoColor=white&style=for-the-badge)](https://dotnet.microsoft.com/en-us/apps/aspnet)
[![C#](https://img.shields.io/badge/C%23-13.0-239120?logo=c-sharp&logoColor=white&style=for-the-badge)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Microsoft SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-2022-CC2927?logo=microsoft-sql-server&logoColor=white&style=for-the-badge)](https://www.microsoft.com/en-us/sql-server/)
[![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?logo=microsoft&logoColor=white&style=for-the-badge)](https://learn.microsoft.com/en-us/ef/core/)
[![GitHub](https://img.shields.io/badge/GitHub-Repository-181717?logo=github&logoColor=white&style=for-the-badge)](https://github.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

A robust, enterprise-grade **Real Estate Management System** built with **ASP.NET Core 9.0 MVC**, utilizing a modular Architecture with **MVC Areas** and the **Generic Repository Pattern**. This application provides a comprehensive platform for managing property listings, organizing location-based real estate data, maintaining user-specific favorite lists, and offering secure, multi-tier user registration and authentication workflows.

---

## 📖 Table of Contents
- [Architecture & Design Patterns](#-architecture--design-patterns)
- [Technologies & Frameworks](#-technologies--frameworks)
- [Key Features](#-key-features)
- [Project Structure](#-project-structure)
- [Database Configuration & Seeding](#-database-configuration--seeding)
- [Installation & Setup](#-installation--setup)
- [Running the Project](#-running-the-project)
- [API Documentation (Scalar/OpenAPI)](#-api-documentation-scalaropenapi)
- [📸 Screenshots](#-screenshots)
- [🎥 Demo](#-demo)
- [🚀 Future Improvements](#-future-improvements)
- [📄 License](#-license)

---

## 🏛️ Architecture & Design Patterns

The project follows a clean **N-Tier architectural pattern** optimized for web applications using ASP.NET Core MVC. This separation guarantees testability, maintainability, and clean separation of concerns:

1. **Presentation Layer (MVC Areas)**:
   - Organized into modular **Areas** (`Admin`, `Customer`, `Identity`) to segregate administrative controls, customer interactions, and account management views.
2. **Business Logic & Mappings**:
   - Leverages **Mapster** for high-performance, memory-efficient object-to-object data mappings between database models and clean ViewModels.
3. **Data Access Layer (Repository Pattern)**:
   - Decoupled from direct EF Core context calls through a generic **IRepository<TEntity>** and EF-based **Repository<TEntity>**.
   - Implements transactional synchronization via unit of work principles, facilitating synchronous and asynchronous operations (`CommitChange`, `CommitAsync`).
4. **Identity & Security Layer**:
   - Built on top of **Microsoft.AspNetCore.Identity.EntityFrameworkCore**, supporting robust role-based access control (RBAC) with preconfigured system roles (`SuperAdmin`, `Admin`, `employee`, `customer`).

---

## 🛠️ Technologies & Frameworks

| Category | Technology | Description |
| :--- | :--- | :--- |
| **Core Framework** | .NET 9.0 | High-performance, cross-platform app framework. |
| **Web UI** | ASP.NET Core MVC | Model-View-Controller architecture with Razor Views. |
| **Database ORM** | EF Core 9.0 | Object-Relational Mapper for .NET. |
| **Database Engine**| MS SQL Server | Relational database storage. |
| **Authentication** | ASP.NET Core Identity | Secure membership, hashing, and token generation. |
| **Object Mapper**  | Mapster 7.4.0 | Fast, type-safe data mapper. |
| **API Docs**       | Scalar & OpenAPI | Modern, interactive API reference docs (replacement for Swagger). |
| **Email Service**  | SMTP client | Integrated for OTP code delivery and verification links. |

---

## ✨ Key Features

### 🛡️ User Authentication & Profile Security
- **Multi-Role User Accounts**: Managed using Roles (`SuperAdmin`, `Admin`, `employee`, `customer`).
- **Email Verification**: Token-based confirmation email dispatch on new registration.
- **OTP Password Recovery**: Secure, time-sensitive (10-minute expiry) One-Time Password verification via SMTP mail to trigger password resets.
- **User Profiles**: Interactive profile manager to edit full name, phone number, address, and profile settings.

### 🏢 Real Estate Property CRUD
- **Comprehensive Listings**: Create, edit, preview, and delete properties (accessible to authorized users).
- **Sub-Image Galleries**: Supports uploading a main image and multiple sub-images for high-resolution galleries.
- **Geographic Information**: Binds physical location data (City, Street, Country, Latitude, Longitude) directly to properties.

### 🔍 Advanced Filtering & Sorting
- **Property Type Filters**: Categorized by type (Apartment, Villa, Office, Palace, Chalet).
- **Price Range Search**: Instant filtering across price categories (Low, Medium, High).
- **Pagination**: Grid-based layout with clean server-side pagination supporting 12 items per page.

### 💖 Customer Favorites
- **Wishlist Management**: Logged-in customers can bookmark/favorite properties.
- **My Favorites**: Dashboard to view, visit, or remove saved properties instantly.

---

## 📁 Project Structure

```
Real_Estate/                         # Root Solution Directory
├── Real_Estate.sln                  # Visual Studio Solution File
└── Real_Estate/                     # Main Web Project Directory
    ├── Areas/                       # MVC Areas (Modular components)
    │   ├── Admin/                   # Administrative Area
    │   │   ├── Controllers/         # PropertyController.cs (CRUD, Dashboard)
    │   │   └── Views/               # Property forms, details, dashboard index
    │   ├── Customer/                # Customer Area
    │   │   ├── Controllers/         # FavoriteController.cs (Wishlists)
    │   │   └── Views/               # Favorite list view page
    │   └── Identity/                # Identity & Accounts Area
    │       ├── Controllers/         # AccountController, ProfileController, HomeController
    │       └── Views/               # Login, Register, Forget Password, Verification views
    ├── DataAccess/                  # Database Context & Migrations Setup
    │   └── ApplicationDbContext.cs  # Entity configurations and DbSet registrations
    ├── DI_Service/                  # Centralized Service Registrations
    │   └── AddService.cs            # Custom services injection extension method
    ├── Email_Service/               # SMTP Client Configurations
    │   └── EmailSender.cs           # Sends registration tokens and OTP codes
    ├── Migrations/                  # EF Core database migrations history files
    ├── Models/                      # Model Layer
    │   ├── ViewModel/               # VM classes (LoginVM, ProfileVM, RegisterVM, etc.)
    │   ├── BaseEntity.cs            # Shared ID and base fields
    │   ├── Favorite.cs              # Saved property relationship
    │   ├── Location.cs              # Geographic location attributes
    │   ├── Property.cs              # Core Property specifications
    │   ├── PropertyLocVM.cs         # Combined Property and Location ViewModel
    │   ├── PropertySubImage.cs      # Sub-images model for property galleries
    │   ├── User.cs                  # Custom identity user implementation
    │   └── UserOtp.cs               # Model to check and validate account OTP codes
    ├── Repository/                  # Generic Repository Implementation
    │   ├── IRepository.cs           # Generic data operations signatures
    │   └── Repository.cs            # Concrete DbContext wrapper logic
    ├── Utilities/                   # System helpers & initializers
    │   ├── DBInitilizer/            # Seeds roles and SuperAdmin credentials
    │   └── SD.cs                    # Static Details (Role constant definitions)
    ├── wwwroot/                     # Static Web Assets (images, css, js)
    ├── appsettings.json             # Global application configuration settings
    └── Program.cs                   # Application entry point and pipelines configuration
```

---

## 💾 Database Configuration & Seeding

### 1. Connection String Config
Open `appsettings.json` and configure the `"DefaultConnection"` connection string to point to your MS SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=real_estate_g10;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

### 2. Auto-Migration & Data Seeding
The application is preconfigured to apply migrations automatically on startup and seed roles and accounts:
- Creates the system roles: **SuperAdmin**, **Admin**, **employee**, **customer**.
- Seeds a default SuperAdmin account if it doesn't already exist.

🔑 **Sample Credentials (SuperAdmin)**:
* **Username / Email**: `SuperAdmin@gmail.com`
* **Password**: `SuperAdmin123@`
* **Pre-assigned Role**: `SuperAdmin`

---

## ⚙️ Installation & Setup

Before running the application, make sure you have the following prerequisites installed:
* [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (with *ASP.NET and web development* workload)
* [MS SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express, LocalDB, or Developer Edition)

Follow these steps to set up the project locally:

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd Real_Estate
   ```

2. **Restore local .NET tools**
   This restores the configured tools (like `dotnet-ef` CLI tool):
   ```bash
   dotnet tool restore
   ```

3. **Restore project dependencies**
   ```bash
   dotnet restore
   ```

4. **Apply EF Core Migrations**
   To update your database using the latest migrations, run either of the following commands:
   * **Via dotnet CLI (Recommended)**:
     ```bash
     dotnet ef database update --project Real_Estate
     ```
   * **Via Package Manager Console in Visual Studio**:
     ```powershell
     Update-Database
     ```

---

## 🏃 Running the Project

1. **Open the solution** in Visual Studio 2022 by double-clicking `Real_Estate.sln`.

2. **Verify appsettings.json**: Ensure the connection string and `EmailSettings` parameters are correct.

3. **Build the project** by pressing `Ctrl + Shift + B` or running:
   ```bash
   dotnet build
   ```

4. **Run the project** using:
   - **IIS Express** or the **Web Profile** inside Visual Studio by pressing `F5`.
   - **dotnet CLI** from the command line:
     ```bash
     dotnet run --project Real_Estate
     ```

5. **Open your browser** and navigate to:
   ```
   https://localhost:7198
   ```
   *(Note: Verify the actual port in `Properties/launchSettings.json` or your console output).*

---

## 🔌 API Documentation (Scalar/OpenAPI)

The application includes configured packages for **Scalar API Reference** and **OpenAPI**.

If you wish to expose and browse API endpoints:
1. Ensure the following configurations are added to `Program.cs`:
   ```csharp
   builder.Services.AddOpenApi();
   // ...
   app.MapOpenApi();
   app.MapScalarApiReference();
   ```
2. Run the application and navigate to the Scalar interactive UI:
   ```
   https://localhost:xxxx/scalar/v1
   ```
   *(This provides a beautiful alternative to Swagger to test real-estate endpoints, endpoints documentation, and authentication tokens).*

---

## 📸 Screenshots

### Home Page
![Home](images/home.png)

### Dashboard
![Dashboard](images/dashboard.png)

### Login
![Login](images/login.png)

### Details
![Details](images/details.png)

### Manage Properties
![Manage Properties](images/manage_properties.png)

### Favorites
![Favorites](images/favorites.png)

---

## 🎥 Demo

![Demo](images/demo.gif)

The interactive GIF demo above showcases:
* **Secure Login**: Sign-in process for the default SuperAdmin account.
* **CRUD Operations**: Dynamically adding, editing, and deleting real estate listings.
* **Search & Filter**: Real-time location & price range filtration.
* **Visual Dashboard**: Live count overview of the properties in the system.
* **Favorites System**: Bookmarking properties to check out later.
* **Logout**: Gracefully ending the session.

---

## 🚀 Future Improvements

To transition this project into a commercial enterprise application, the following features are planned:
- [ ] **Google Maps API Integration**: Display real-estate listings directly on an interactive map.
- [ ] **Advanced File Hosting**: Store property images on cloud storage solutions like AWS S3 or Azure Blob Storage.
- [ ] **Payment Gateways**: Integrate Stripe or PayPal payments for subscription fees and premium listings.
- [ ] **Real-time Chat**: Implement SignalR for real-time messaging between sellers, agents, and buyers.
- [ ] **AI-Powered Recommendations**: Recommend properties to users based on search behavior and favorites list.

---

## 📄 License

Distributed under the **MIT License**. See `LICENSE` for more information.

---

<p align="center">
  Made with ❤️ by .NET Developers
</p>
