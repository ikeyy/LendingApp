---
# 🏦 LendingApp
---

## 🛠 Prerequisites

Ensure you have the following installed and configured before running the application:

1. **Node.js: ** [Download and install here](https://nodejs.org/).
2. **Angular CLI: ** Run the following command in your terminal:
```bash
npm install -g @angular/cli

```


3. **SQL Database: ** Import the `LenderAppData.bacpac` file into your SQL Server instance.
4. **Postman: ** Import `LendingApp.postman_collection.json` to access pre-configured API endpoints.

---

## ⚙️ Configuration & Setup

To ensure the frontend and backend communicate correctly, you must align the following settings:

### 1. Frontend Configuration

In the Angular project, locate `src/environments/environment.development.ts`:

* Update the `apiURL` to match your **.NET Core** localhost URL.

### 2. Backend Configuration

In the .NET project, locate `appSettings.development.json`:

* **CORS:** Update `AllowedOrigins` to match your **Angular** localhost URL (e.g., `http://localhost:4200`).
* **Database:** Update the Connection String/Server Name to match your local SQL Server credentials.

---

## 🚀 Running the App

### Start the Backend

1. Open the .NET Core solution via Visual Studio.
2. Run the application

### Start the Frontend

1. Open a terminal in the Angular project folder via Visual Studio Code
2. Execute:
```bash
ng serve

```


3. Open your browser to the localhost link provided in the terminal.

### Initiate Loan Process

Run `POST {baseUrl}/api/LoanRequest` (via Postman or Swagger) to receive the redirection link for the loan application.

---

## 📂 API Reference Samples

### Blacklist Management

Used to restrict specific email domains or mobile numbers.

| Type | Sample Payload |
| --- | --- |
| **Domain** | `{ "Type": "Domain", "Value": "gmail.com" }` |
| **Mobile** | `{ "Type": "Mobile", "Value": "09099099909" }` |

### Product Data

```json
{
  "DisplayName": "Product A",
  "InterestRate": 0.0599,
  "InterestFreeMonths": "all",
  "MinTermMonths": 2,
  "MaxTermMonths": 60,
  "MinLoanAmount": 500.00,
  "MaxLoanAmount": 15000.00,
  "DefaultTermMonths": 2,
  "Description": "Interest-free for the entire loan period"
}

```

> **Note:** `InterestFreeMonths` must be a **string**. Use `"all"` for the full term, or a specific number (e.g., `"2"`) for limited months.

---

## 🚀 Future Improvements

* **Database:** Normalization of remaining SQL tables.
* **Features:** Creation of a CRUD API for Fee management.
* **Testing:** Expanded Automated Unit Testing coverage beyond Loan Calculations.
* **Architecture:** Implementation of a Generic Repository pattern within the Infrastructure Layer.
* **Security:** Implementation of Authentication for API Call

---
