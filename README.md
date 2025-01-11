# Minimal API - .NET 8

This project is a Minimal API built using .NET 8, designed with a focus on simplicity and performance. Below are the key features and technologies used:

## Features

- **Entity Framework Core (EF Core):**
    - Enables efficient database access and management.

- **Filters:**
    - Custom filters for request validation and response processing.

- **Extension Methods:**
    - Enhances code reusability and readability by adding reusable methods.

- **AutoMapper:**
    - Simplifies object-to-object mapping.

- **Swagger Documentation:**
    - Integrated Swagger for API documentation, making the endpoints easy to explore and test.

- **Basic Authorization with JWT:**
    - Implements JSON Web Tokens (JWT) for secure authentication and authorization.

- **Microsoft.Identity:**
    - Leverages Microsoft.Identity for handling authentication.

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd <repository-folder>
   ```

2. **Set up the database:**
    - Update the connection string in `appsettings.json`.
    - Apply migrations:
      ```bash
      dotnet ef database update
      ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access Swagger UI:**
    - Navigate to `http://localhost:<port>/swagger` in your browser to explore the API.

## Configuration

- **JWT Configuration:**
    - Update the JWT settings in `appsettings.json` (e.g., `ValidAudiences`, `ValidIssuer`).

- **EF Core Migrations:**
    - To create a new migration, run:
      ```bash
      dotnet ef migrations add <MigrationName>
      ```

## Dependencies

- **.NET 8 SDK**
- **Entity Framework Core**
- **AutoMapper**
- **Swashbuckle (Swagger)**
- **Microsoft.Identity**

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

---

Feel free to contribute and suggest improvements!

