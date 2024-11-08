# Game Library Project

This project is an ASP.NET Core MVC web application that manages a library of video games. It demonstrates various design patterns, integrates Entity Framework Core with a SQL Server database, and allows CRUD operations with sorting and simple validation. Created for the Software Technologies 2 course @ Plovdiv University "Paisii Hilendarski".

## Table of Contents

- [Technologies Used](#technologies-used)
- [Project Setup](#project-setup)
- [Database Structure](#database-structure)
- [Design Patterns Used](#design-patterns-used)
- [Features](#features)
- [How to Use](#how-to-use)

---

## Technologies Used

- **ASP.NET Core MVC** - Web framework for building MVC applications
- **Entity Framework Core** - ORM for database interactions
- **Microsoft SQL Server** - Database for storing game records
- **Bootstrap** - Front-end framework for styling
- **C#** - Programming language

---

## Project Setup

### Prerequisites

- Visual Studio 2019 or later
- .NET 8 SDK
- Microsoft SQL Server or SQL Server Express

### Installation Steps

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/GameLibraryProject.git
   ```

2. **Open the Project in Visual Studio**.

3. **Configure the Database Connection**:
   - Open `appsettings.json`.
   - Update the connection string in `ConnectionStrings:GameLibraryDatabase` to match your SQL Server setup.
   
4. **Apply Migrations**:
   - Open the **Package Manager Console** in Visual Studio.
   - Run the following commands to create the database and tables:
     ```bash
     Add-Migration InitialCreate
     Update-Database
     ```

5. **Run the Application**:
   - Press `F5` in Visual Studio to start the application.

---

## Database Structure

The `Game` model represents the structure of the `Games` table in SQL Server.

- **ID** (int): Primary Key
- **Title** (string): Title of the game, required with a max length of 100 characters
- **Publisher** (string): Publisher of the game, required
- **Developer** (string): Developer of the game, required
- **ReleaseDate** (DateTime): Release date of the game, required
- **Genre** (string): Genre of the game, required with a max length of 50 characters

---

## Design Patterns Used

The project demonstrates the following design patterns:

1. **Singleton Pattern**:
   - Used for the `GameNotifier` service, ensuring a single instance for notifying observers about changes to game entities.

2. **Factory Pattern**:
   - Used to create instances of different game categories (e.g., Action, RPG, Puzzle) based on user input.

3. **Strategy Pattern**:
   - Applied in the `Index` action to sort the games list by different criteria (e.g., by Title, Release Date, Genre) in ascending or descending order.

4. **Observer Pattern**:
   - Observers are notified when game entities are added, updated, or deleted. Example observers include logging updates and sending notifications.

---

## Features

- **CRUD Operations**: Create, Read, Update, and Delete games.
- **Sorting**: Sort games by title, release date, or genre in both ascending and descending order.
- **Validation**: Data annotations validate user input (e.g., required fields, maximum string length).
- **Design Patterns**: Implements Singleton, Factory, Strategy, and Observer patterns to enhance code organization and extensibility.
- **Responsive UI**: Styled with Bootstrap for a clean and responsive interface.

---

## How to Use

1. **Home Page**: Displays a list of games with options to sort by title, release date, or genre.
2. **Add Game**: Click on **Add New Game** to add a new game to the library. Ensure all required fields are filled.
3. **Edit Game**: Click on **Edit** next to a game to update its details.
4. **Delete Game**: Click on **Delete** to remove a game from the library.
5. **Sort Games**: Click on column headers to toggle between ascending and descending sorting.

---

## License

This project is open-source and available under the MIT License.
