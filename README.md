# 📺 Habertürk TV Ratings Automation Project

This project is an end-to-end automation system developed for **Habertürk** to streamline the processing and presentation of daily television channel rating data. The system automatically retrieves rating reports sent via email, processes the data, and makes it accessible via a modern web interface.

---

## 🔍 Project Overview

The purpose of this project is to **automate the collection, processing, and display** of TV ratings data. The ratings data is received daily via email and must be parsed and stored for reporting and visualization.

This project consists of the following four components:

### 1. 🖥️ Console Application

* Retrieves rating data from incoming emails.
* Parses the relevant information.
* Inserts the parsed data into a **Microsoft SQL Server** database.

### 2. 🗄️ Database

* Stores all rating data and handles the processing logic.
* Contains **stored procedures** and algorithms to prepare the data for display, including:

  * Ranking and sorting
  * Daily comparisons
  * Color-coded classifications

### 3. 🔌 ASP.NET Core API

* Fetches processed data from the database.
* Formats and enhances data with sorting and color-coding logic.
* Provides RESTful endpoints for consumption by the UI.

### 4. 🌐 Angular Web Application

* Displays data through user-friendly tables and dashboards.
* Shows ratings with visual indicators (e.g., colors, rankings).
* Enables easy navigation and filtering of daily reports.

---

## 🛠️ Technologies Used

| Component   | Technology           |
| ----------- | -------------------- |
| Console App | .NET Core            |
| Database    | Microsoft SQL Server |
| Backend API | ASP.NET Core Web API |
| Frontend UI | Angular              |

---

## 🚀 Getting Started

### Prerequisites

* .NET SDK
* SQL Server
* Node.js & Angular CLI
* Access to the email account receiving the rating reports

### Setup Steps

1. **Database**

   * Set up the SQL Server and run the provided schema and stored procedures.
2. **Console Application**

   * Configure email credentials and database connection string.
   * Schedule the app to run daily (e.g., with Task Scheduler).
3. **API**

   * Configure database connection.
   * Run the API using `dotnet run`.
4. **Angular Frontend**

   * Run `npm install` and `ng serve`.

---

## 📊 Features

* Automated email parsing
* Daily TV ratings ingestion
* Color-coded, ranked data presentation
* Web-based UI for easy access
* Modular architecture for easy maintenance

---

## 📂 Project Structure

```
/HaberturkRatingsProject
├── ConsoleApp/           # Email processing and data ingestion
├── Database/             # SQL scripts and stored procedures
├── Api/                  # ASP.NET Core Web API
├── WebApp/               # Angular frontend
```

---

## 📄 License

This project is intended for internal use and may not be publicly licensed. Please contact the project maintainers for more information.
