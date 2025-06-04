# 📝 Task Management System

A full-stack backend project built with **ASP.NET Core**, providing secure user registration, login, and task management capabilities. The system is integrated with a separate **Notification API**, which receives HTTP requests for task creation and updates.

## 📌 Features

### ✅ Authentication & Authorization
- User registration and login using **ASP.NET Core Identity**.
- Secured endpoints via **JWT (JSON Web Token)** authentication.
- Each user can only access and manage their own tasks.

### ✅ Task Management (CRUD)
- **Create** a new task.
- **Read** tasks with optional **pagination** for better performance.
- **Update** existing tasks.
- **Delete** tasks.
- Users can only perform operations on **their own tasks**.

### 🔔 Notification Integration
- A separate **Notification API** is implemented.
- The **Task API sends HTTP requests** to the Notification API whenever:
  - A task is **created**.
  - A task is **updated**.
- This simulates a decoupled microservice-style communication.

---

## 🛠️ Technologies Used

- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core**
- **JWT Authentication**
- **ASP.NET Core Identity**
- **HttpClient** for inter-service communication
- **SQL Server** (or local DB for development)
- **Swagger** for API documentation and testing

---


---

## 🔐 How JWT Authentication Works

- After registration/login, users receive a **JWT token**.
- This token must be included in the `Authorization` header as `Bearer <token>` in subsequent API requests.
- The backend verifies the token to authenticate users and determine access.

---

## 🔗 Endpoints Overview

### 🚪 Auth (AccountController)
| Method | Endpoint           | Description           |
|--------|--------------------|-----------------------|
| POST   | `/api/account/register` | Register a new user   |
| POST   | `/api/account/login`    | Log in and receive JWT token |

### 📋 Task Management (TaskController)
| Method | Endpoint           | Description                      |
|--------|--------------------|----------------------------------|
| GET    | `/api/tasks`       | Get all user tasks (paginated)  |
| GET    | `/api/tasks/{id}`  | Get a specific task             |
| POST   | `/api/tasks`       | Create a new task               |
| PUT    | `/api/tasks/{id}`  | Update an existing task         |
| DELETE | `/api/tasks/{id}`  | Delete a task                   |

> Each request must be authenticated with a valid JWT token.

---

## 🚨 Notification API (Overview)

- A lightweight API running separately.
- Listens for incoming HTTP POST requests from the Task API.
- Logs or processes task creation/update events.

---

## 📄 Example Request (Task Creation)

```http
POST /api/tasks
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6...
Content-Type: application/json

{
  "title": "Complete README file",
  "description": "Add project description and usage to README",
  "dueDate": "2025-06-10T23:59:00"
}

