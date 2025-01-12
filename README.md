# 🌟 Library Management System (LMS)

Welcome to the **Library Management System (LMS)** repository! This project is developed using the **ASP.NET MVC framework**, designed to streamline essential library operations for university librarians. 

---

## 📚 **Overview**
The LMS project simplifies library workflows by automating:
- 📖 Book inventory management
- 🔄 Borrowing/returning transactions
- 💰 Fine calculation for overdue books

The system is tailored for **librarians** as the primary users, ensuring a user-friendly and efficient experience through modern web technologies.

---

## 🛠 **Key Features**

### ✅ **Core Functionalities**
1. **User Authentication**  
   🔒 Secure login system with role-based access control.

2. **Book Management**  
   📘 Add, edit, search, and delete book records.  
   📊 Real-time updates on book availability.

3. **Borrow/Return Management**  
   🔄 Record borrowing/returning transactions.  
   💰 Automatic overdue fine calculation.

4. **Dashboard Analytics**  
   📈 Visual statistics for library performance.  
   🗂 Charts for borrowing trends by author, category, and time.

5. **Responsive Design**  
   📱 Optimized for desktops, tablets, and mobile devices.

---

## 🚀 **Technologies Used**

- **Backend Framework:** ASP.NET MVC
- **Database:** Microsoft SQL Server
- **Frontend:** Razor Pages, HTML, CSS, JavaScript
- **Tools:** Visual Studio, Entity Framework

---

## 🔧 **Setup Instructions**

### Prerequisites
- Microsoft Visual Studio
- Microsoft SQL Server
- .NET Framework

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/Kyubikk/LMS.git
   ```

2. Open the project in Visual Studio.

3. Configure the database connection in the `appsettings.json` file.

4. Apply database migrations:
   ```bash
   Update-Database
   ```

5. Build and run the application:
   ```bash
   dotnet run
   ```

6. Access the application in your browser at `http://localhost:<port>`.

---

## ⚙️ **Detailed Features**

### 📘 **Book Management**
- Add and edit book details (e.g., title, author, category, ISBN).
- Delete books without active borrowing transactions.
- Search books by multiple criteria (e.g., title, author, category).

### 🔄 **Borrow/Return Transactions**
- Record borrowing with due dates.
- Update return status and calculate fines for overdue books.
- Validate university email addresses for borrowers.

### 📊 **Dashboard Analytics**
- Key statistics and trends visualized with:
  - 📊 Pie charts for book categories.
  - 📈 Bar charts for books by author.
  - 📉 Line charts for borrowing trends over time.

---

## 🛡 **Security Measures**

- 🔐 Password encryption using SHA-256.
- ⏲ Session timeouts after 15 minutes of inactivity.
- ✅ Input validation to prevent invalid data submissions.

---
