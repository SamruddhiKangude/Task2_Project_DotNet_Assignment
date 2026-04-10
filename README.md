# Task2_Project_DotNet_Assignment
 This assignment evaluates your practical skills in .NET Core, SQL, and API development.

 # Student Management System

A full-stack Student Management System built as a technical assignment.

## Features Included
- **ASP.NET Core Web API 10.0** with Layered Architecture (Controllers, Services, Repositories)
- **React UI** built with Vite, featuring a premium glassmorphic "anti-gravity" modern aesthetic powered by Framer Motion.
- **Dockerized Setup** for both Database and API
- **JWT Authentication** (Secure API endpoints and login flow)
- **Global Exception Handling Middleware** for unified error responses
- **Serilog** for structural logging
- **Swagger UI** for API documentation
- **Unit Testing** with xUnit and Moq


---
## Repository link

GitHub Repository Link : https://github.com/SamruddhiKangude/Task2_Project_DotNet_Assignment

---

## 🚀 Setup & Execution

### Option 1: Running with Docker (Recommended)
This approach sets up both the SQL Server database and the .NET Web API in containers.

1. Ensure [Docker Desktop](https://www.docker.com/products/docker-desktop) is running.
2. In the root directory of the project, run:
   ```bash
   docker-compose up --build
   ```
3. The API will be available at `http://localhost:5000`. You can test Swagger at `http://localhost:5000/swagger`.
4. Run the React Frontend (in a separate terminal):
   ```bash
   cd StudentManagement-ui
   npm install
   npm run dev
   ```
5. The React application will start at `http://localhost:5173`. 

---

### Option 2: Running Locally (Manual Build)
If you don't use Docker, ensure you have **.NET 10.0 SDK**, **Node.js**, and a running **SQL Server** instance.

1. **Database Setup**
   - Open `StudentManagement.API/appsettings.json` and change the `DefaultConnection` string to point to your local SQL Server instance.

2. **Run Backend**
   ```bash
   cd StudentManagement.API
   dotnet restore
   dotnet run
   ```
   *Note: Entity Framework Code First relies on `context.Database.Migrate()` on startup, meaning the schema and a default `admin` user are created automatically.*

3. **Run Frontend** 
   ```bash
   cd StudentManagement-ui
   npm install
   npm run dev
   ```

---

## 🧪 Running Unit Tests
To run the included unit tests (which cover `StudentService`):
```bash
dotnet test
```
---

## 🔐 Default Credentials
To access the Dashboard through the React UI, use the default seeded credentials:
- **Username:** `admin`
- **Password:** `admin123`

---

## 🏗️ Architecture Design
- **API Project:** Contains Controllers, Middleware, and API configuration.
- **Core Library:** Contains Domain Models (`Student`, `User`), DTOs, and Service/Repository Interfaces.
- **Infrastructure Library:** Contains Entity Framework `AppDbContext`, and implementations of Repositories and Services.
- **Tests Project:** xUnit testing logic.
- **React UI:** Context API for Authentication handling, Axios interceptors, Framer Motion for premium UX.

--- 

## 🔄 Logic Flowchart ( flowchart TD )

```mermaid
flowchart TD

    A[Start] --> B[User Opens React UI]
    B --> C{Authenticated?}

    C -- No --> D[Enter Username and Password]
    D --> E[Send Login Request to API]
    E --> F{Valid Credentials?}

    F -- No --> G[Show Error Message]
    G --> D

    F -- Yes --> H[Receive JWT Token]
    H --> I[Store Token - LocalStorage]
    I --> J[Redirect to Dashboard]

    C -- Yes --> J

    J --> K[User Selects Action]

    K --> L1[Get All Students]
    K --> L2[Add Student]
    K --> L3[Update Student]
    K --> L4[Delete Student]

    %% GET FLOW
    L1 --> M1[Send GET Request with JWT]
    M1 --> N1[Controller]
    N1 --> O1[Service Layer]
    O1 --> P1[Repository Layer]
    P1 --> Q1[Database]
    Q1 --> R1[Return Student List]
    R1 --> S1[Display in UI]

    %% ADD FLOW
    L2 --> M2[Fill Student Form]
    M2 --> N2[Send POST Request]
    N2 --> O2[Validate Data]
    O2 --> P2[Service Layer]
    P2 --> Q2[Repository Layer]
    Q2 --> R2[Save to Database]
    R2 --> S2[Return Success Response]
    S2 --> T2[Update UI]

    %% UPDATE FLOW
    L3 --> M3[Edit Student Details]
    M3 --> N3[Send PUT Request]
    N3 --> O3[Validate Data]
    O3 --> P3[Service Layer]
    P3 --> Q3[Repository Layer]
    Q3 --> R3[Update Database]
    R3 --> S3[Return Updated Data]
    S3 --> T3[Refresh UI]

    %% DELETE FLOW
    L4 --> M4[Select Student]
    M4 --> N4[Send DELETE Request]
    N4 --> O4[Service Layer]
    O4 --> P4[Repository Layer]
    P4 --> Q4[Delete from Database]
    Q4 --> R4[Return Success Response]
    R4 --> S4[Update UI]

    %% ERROR HANDLING
    N1 --> X[Global Exception Middleware]
    N2 --> X
    N3 --> X
    N4 --> X

    X --> Y[Log Error - Serilog]
    Y --> Z[Return Standard Error Response]

    Z --> J

    S1 --> J
    T2 --> J
    T3 --> J
    S4 --> J

    J --> AA[End or Continue Using App]
```
