# StudyPlanner Web App

> A web application for students to organize tasks, exams, and projects.

---

## Description

**StudyPlanner** helps students manage their academic workflow by organizing tasks, exams, and categories, with features for progress tracking and time management.

---

## Main Features

* Create, edit, and delete tasks with priorities and subtasks
* Manage exams with date, time, and location
* Categorize tasks by subject
* Dashboard with real-time statistics and reminders
* Calendar with all tasks and exams
* Statistics and charts powered by Chart.js
* Pomodoro timer for focused work sessions
* Filtering, searching, and pagination
* Admin panel for user management

---

## User Roles

| Role            | Access             |
| --------------- | ------------------ |
| Guest           | Limited access     |
| Registered User | Full access        |
| Administrator   | Admin panel access |

---

## Technologies

| Technology                | Purpose               |
| ------------------------- | --------------------- |
| ASP.NET Core 6.0          | MVC Framework         |
| Entity Framework Core 6.0 | ORM                   |
| Microsoft SQL Server      | Database              |
| ASP.NET Identity          | Authentication        |
| Bootstrap 5               | UI Framework          |
| Chart.js                  | Charts and Statistics |
| FullCalendar              | Calendar              |
| NUnit                     | Unit Testing          |

---

## Project Structure

```text
StudyPlanner/
├── StudyPlanner.Web             # ASP.NET Core MVC
├── StudyPlanner.Core            # Entity Models, Interfaces
├── StudyPlanner.Infrastructure  # EF Core, Services, Migrations
└── StudyPlanner.Tests           # NUnit Unit Tests
```

---

## Installation

1. Clone the repository

```bash
git clone https://github.com/BoyanTsarski/StudyPlanner-Web-App.git
```

2. Configure the connection string in `appsettings.json`

3. Run the database migration

```bash
Update-Database -StartupProject "StudyPlanner WebApp"
```

4. Start the project with `F5`

---

## Sample Admin Account

**Email:** [admin@studyplanner.com](mailto:admin@studyplanner.com)
**Password:** Admin123!

