# RestApiChallenge

This project is designed as a book management system REST API. Users can register, log in, add, update, or delete books. They can also query the books added by a user.

## Technologies

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- JWT Authentication
- Swagger

## Installation

Clone the project to your local machine:

```bash
git clone https://github.com/IsmailCanMutlu/RestApiChallenge.git
```
Navigate to the project directory and install dependencies:

```bash
cd RestApiChallenge
dotnet restore
```
Create the database and apply migrations:

```bash
dotnet ef database update
```
Run the application:

```bash
dotnet run
```
The application will be running at https://localhost:5001 and http://localhost:5000. You can access the Swagger UI at https://localhost:5001/swagger.

## Usage

User Operations
User Registration: POST /api/User/register
User Login: POST /api/User/login

Book Operations
List All Books: GET /api/Book
Add Book: POST /api/Book
Update Book: PUT /api/Book/{id}
Delete Book: DELETE /api/Book/{id}
List Books by User: GET /api/Book/user/{userId}

##License
This project is licensed under the MIT License.


