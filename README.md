# 📚 StudyPlanner Web App

> Уеб приложение за ученици и студенти за организиране на задачи, изпити и проекти.

---

## 📖 Описание

**StudyPlanner** помага на студентите да управляват учебния си процес чрез организиране на задачи, изпити и категории, с възможност за проследяване на напредъка и планиране на времето.

---

## ✨ Основни функции

- ✅ Създаване, редактиране и изтриване на задачи с приоритети и подзадачи
- 📅 Управление на изпити с дата, час и зала
- 🏷️ Категоризиране на задачи по предмети
- 📊 Dashboard с реални статистики и напомняния
- 🗓️ Календар с всички задачи и изпити
- 📈 Статистики с графики (Chart.js)
- ⏱️ Pomodoro таймер за фокусирана работа
- 🔍 Филтриране, търсене и странициране
- 🔐 Admin панел за управление на потребители

---

## 👥 Потребители

| Роля | Достъп |
|------|--------|
| 👤 Гост | Ограничен достъп |
| 🎓 Регистриран | Пълен достъп |
| 🛡️ Администратор | Admin панел |

---

## 🛠️ Технологии

| Технология | Употреба |
|-----------|---------|
| ASP.NET Core 6.0 | MVC Framework |
| Entity Framework Core 6.0 | ORM |
| Microsoft SQL Server | База данни |
| ASP.NET Identity | Автентикация |
| Bootstrap 5 | UI Framework |
| Chart.js | Графики |
| FullCalendar | Календар |
| NUnit | Unit тестове |

---

## 📁 Структура на проекта
StudyPlanner/
├── 🌐 StudyPlanner.Web             # ASP.NET Core MVC
├── 📦 StudyPlanner.Core            # Entity модели, интерфейси
├── 🔧 StudyPlanner.Infrastructure  # EF Core, Services, Migrations
└── 🧪 StudyPlanner.Tests           # NUnit unit тестове

---

## 🚀 Инсталация

1. Клонирай репото
```bash
git clone https://github.com/BoyanTsarski/StudyPlanner-Web-App.git
```
2. Настрой connection string в `appsettings.json`
3. Изпълни миграцията
```bash
Update-Database -StartupProject "StudyPlanner WebApp"
```
4. Стартирай проекта с `F5`

---

## 🔑 Примерен Admin достъп
Email:  admin@studyplanner.com
Парола: Admin123!
