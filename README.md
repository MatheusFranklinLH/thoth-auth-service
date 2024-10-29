# Thoth Authentication Service

Thoth is an authentication service developed to provide a robust solution for handling users, roles, permissions, and multi-tenant support. This project was initially created to meet my own needs for a flexible, standalone authentication service for my upcoming side projects. Additionally, I leveraged this project to solidify my understanding of **Domain Driven Design (DDD)** principles and **unit testing** practices, which is why the repository may be more verbose and include more files than strictly necessary.

The project’s name, **Thoth**, is inspired by the Egyptian god of knowledge, writing, and wisdom, honoring the deity’s association with the secure and organized handling of information.

## Technologies Used

This project uses the following technologies and libraries:

- **.NET 8** - Core framework for building and running the service
- **Flunt** - For handling validation and notifications
- **Entity Framework Core** - ORM for handling database operations
- **PostgreSQL** - Database used for persistence
- **Domain Driven Design (DDD)** - Architectural approach for organizing the codebase
- **xUnit** - Testing framework for unit tests
- **Moq** - For mocking dependencies in unit tests
- **HTTP Files** - For testing API endpoints with predefined requests

## Multi-Tenant Structure

The service is designed to support a **multi-tenant architecture**, with **Organization** being the primary multi-tenant entity. Each user is associated with an organization, enabling role-based and permission-based access control within the scope of a tenant. This setup ensures that each organization’s data is isolated, enhancing security and data integrity.

## Project Structure

The repository follows **Domain Driven Design (DDD)** principles, dividing the codebase into organized layers:

- **Domain**: Contains the core business logic, including entities, value objects, and service interfaces.
- **Infrastructure**: Manages data access, database migrations, and other integrations, including PostgreSQL and EF Core configurations.
- **API**: Exposes the endpoints for the authentication service.

## How to Run the Project

### Prerequisites

- **.NET 8 SDK** - Ensure that .NET version 8 is installed on your machine.
- **PostgreSQL** - Ensure that PostgreSQL is set up and running.

### Setup Steps

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/thoth-auth-service.git
   cd thoth-auth-service

2. **Run the Database Script**:

    Locate the dbScript.sql file in the Thoth.Infrastructure folder.
    Execute the SQL script in your PostgreSQL database to set up the initial schema.

3. **Apply Migrations**:

    Run the following commands to apply any outstanding migrations:
	```bash
		dotnet ef database update --project Thoth.Infrastructure --startup-project Thoth.API

4. **Run the Application**:
	```bash
   	 dotnet run --project Thoth.API

### Testing with .http Files

HTTP files are provided in the project to facilitate testing of API endpoints. Open these files in your preferred IDE (e.g., VS Code) to execute predefined requests and validate the service’s functionality.

## Why Thoth?

Named after the Egyptian god Thoth, the project reflects the ideals of structured, reliable information management and is a fitting namesake for an authentication and authorization service.
