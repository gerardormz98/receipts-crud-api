# Receipts CRUD Web API

**Receipts CRUD Web API** is a REST service that provides an interface to interact with the database of the "Receipts CRUD" app. This service allows the client to authenticate users through JWT, edit suppliers catalog, and create, update and delete receipts.

## Technologies

 - **.NET Core 5 Web API**: .NET Framework was used to build the REST service with C# programming language.
 - **Entity Framework Core**: I used the .NET ORM with "Code First" alternative.
 - **Firebase Authentication**: Auth provider by Google. I used Firebase to safely handle passwords and user sessions.
 - **SQLite**: Database engine. As this is a fake project, I decided to use SQLite to avoid additional hosting.
 - **Azure App Services**: This service is hosted in an Azure free development server, so it can be accessed from anywhere in the world.
 - **JSON Web Tokens**: I used JWT to handle user authentication. Tokens are provided by Firebase.

## Endpoints

All the endpoints of this service require authentication (except for the login). The request must include the header "Authorization" with the value "Bearer " + the token obtained from the login.

Also, some endpoints require that the authenticated user is the owner of the object to modify. Otherwise, they will return a 401 error (Unauthorized).

Base URL: https://simple-crud-web-api.azurewebsites.net/api

> Login

|Method|URL|Description|Requires Admin Role|
|--|--|--|--|
|GET|/login|Authenticate user with email and password.|No|

> Users

|Method|URL|Description|Requires Admin Role|
|--|--|--|--|
|GET|/users|Get all the users|Yes
|GET|/users/{id}|Get a user by ID|No
|POST|/users|Create a new user|Yes
|PUT|/users/{id}|Update an existing user by ID|Yes
|DELETE|/users/{id}|Delete a user by ID|Yes
|POST|/users/reset?email={email}|Send an email with the link to reset the user's password|No

> Receipts

|Method|URL|Description|Requires Admin Role|
|--|--|--|--|
|GET|/receipts|Get all the receipts of the current user|No
|GET|/receipts/{id}|Get a receipt by ID|No
|POST|/receipts|Create a new receipt|No
|PUT|/receipts/{id}|Update an existing receipt by ID|No  
|DELETE|/receipts/{id}|Delete a receipt by ID|No

> Suppliers

|Method|URL|Description|Requires Admin Role|
|--|--|--|--|
|GET|/suppliers|Get all the suppliers|No
|GET|/suppliers/{id}|Get a supplier by ID|Yes
|POST|/suppliers|Create a new supplier|Yes
|PUT|/suppliers/{id}|Update an existing supplier by ID|Yes
|DELETE|/suppliers/{id}|Delete a supplier by ID|Yes
