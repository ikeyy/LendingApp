🏦 LendingApp

A full-stack lending application featuring an **Angular** frontend, a **.NET Core** backend, and **SQL Server** database integration.

---

## 🛠 Prerequisites

Ensure you have the following installed before proceeding:

1.  **Node.js:** [Download here](https://nodejs.org/)
2.  **Angular CLI:** Install via terminal:
    ```

[file-tag: code-generated-file-0-1771258482726569808]
bash
    npm install -g @angular/cli
    ```
3.  **SQL Server:** Prepared for `.bacpac` restoration.
4.  **Postman:** For API testing.

---

## 🚀 Installation & Setup

### 1. Database Setup
* Import the `LenderAppData.bacpac` file into your SQL Server instance to initialize the schema and data.

### 2. Postman Collection
* Import `LendingApp.postman_collection.json` into Postman to access pre-configured requests for Blacklisting and Product management.

### 3. Configuration & Alignment
To ensure the frontend and backend communicate correctly, update the following files:

* **Frontend:** In `environment.development.ts`, update the `apiURL` to match your .NET Core localhost URL.
* **Backend:** In `appSettings.development.json`, update `AllowedOrigins` to match your Angular localhost URL (default: `http://localhost:4200`).
* **Database:** In `appSettings.development.json`, update the connection string to match your local SQL Server credentials.

---

## 🖥️ Running the Application

### Backend (.NET Core)
1. Open the solution in Visual Studio or your preferred IDE.
2. Run the project (F5 or `dotnet run`).

### Frontend (Angular)
1. Open a terminal in the Angular project directory.
2. Run the development server:
   ```bash
   ng serve
