# FastLearn: An Online Learning Platform

## Overview
This repository contains the implementation of **FastLearn**, a web-based platform designed to enhance the educational experience by offering reliable learning resources and fostering collaboration among users. FastLearn supports multiple user roles, including Unregistered Users, Students, HelpingStudents, Professors, and Admins, each with tailored functionalities to ensure a robust and user-friendly learning environment.

The project includes features like progress tracking, course creation, one-on-one tutoring sessions, and comprehensive course management. The platform is built using modern technologies such as **ASP.NET**, **Entity Framework**, and **SQLite**.

A detailed project report, outlining the design, implementation, and results, is included in the repository under `Documentation/ThesisDocumentation.pdf`.

---

## Repository Structure
- **`Web`**: Contains the main application code, including controllers, views, and frontend components.
- **`Data`**: Handles the database structure and operations using Entity Framework with a Code-First approach.
- **`Documentation`**: Includes the project report and related files.

---

## Installation Instructions
To run the project, ensure you have the following dependencies installed:
- **ASP.NET**
- **Entity Framework**
- **SQLite**
- **Visual Studio 2022**

## Project Setup Instructions

### 1. Open the Project in Visual Studio
1. Launch **Visual Studio 2022**.
2. Open the solution file (`.sln`) located in the repository.

### 2. Restore Dependencies
1. Open **NuGet Package Manager** in Visual Studio:
   - Go to **Tools > NuGet Package Manager > Manage NuGet Packages for Solution**.
2. Restore all missing dependencies by clicking **Restore**.

### 3. Configure the Database
The project uses **SQLite** for the backend. To set it up:
1. Open the **Package Manager Console** in Visual Studio:
   - Go to **Tools > NuGet Package Manager > Package Manager Console**.
2. Run the following commands to apply database migrations:
   ```bash
   Add-Migration InitialCreate
   Update-Database
   ```
### 4. Run the Application
1. Set the `ProiectLicenta.Web` project as the **startup project**:
   - Right-click on the `ProiectLicenta.Web` project in **Solution Explorer**.
   - Select **Set as Startup Project**.

2. Run the application:
   - Press `F5` or click the **Run** button in Visual Studio.

### Accessing the Application
Once the application is running, open a web browser and navigate to the URL provided by Visual Studio (e.g., `http://localhost:5000`).

---

## Key Features
- **Role-based Access**:
  - **Unregistered Users**: Browse available courses and view public information.
  - **Students**: Enroll in courses, track progress, and leave reviews.
  - **HelpingStudents**: Offer assistance through one-on-one tutoring and earn platform points.
  - **Professors**: Create and manage courses with advanced tools like CKEditor for content formatting.
  - **Admins**: Oversee platform operations, review user activities, and manage roles.

- **Interactive Learning**:
  - Structured courses with chapters and subchapters.
  - Progress tracking with integrated quizzes and tests.

- **Collaboration**:
  - Real-time one-on-one chat functionality for peer support.

---

## Results
The project successfully demonstrates:
- A fully operational learning platform with robust user role management.
- Effective integration of modern technologies for scalability and maintainability.
- A novel approach to learning, emphasizing user collaboration and authentic content delivery.

---

## Detailed Report
For an in-depth explanation of the design, implementation, and testing of the platform, refer to the detailed report included in the repository:
**`Thesis Documentation.pdf`**

---

## License
This project is released under the MIT License. See the LICENSE file for details.

---
