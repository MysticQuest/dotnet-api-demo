# ASP.NET API, Console App & DB

The project launches both a console application and an ASP.NET API that listens on localhost.
- The Views project in this case is not a front-end. It is the console app and just emulates a client that would otherwise hit the faceless API remotely. It provides a command-line interface for operations like printing, deleting, and retrieving records.
- The Services project includes an ASP.NET API app that hosts endpoints for CRUD operations, dynamically handling service types for different data models. It also includes the startup configuration for the application.
- The Models project contains data models that represent the structure of records stored in the database.
- The DataAccess project manages the Entity Framework Core DbContext and migrations for SQLite database interactions.

### Functionality:
The API can perform tasks such as pinging a domain or querying a remote API, capturing the response, and persisting it as an entry in the database. The user can interact with stored data through provided API endpoints, enabling operations such as retrieval, deletion, and listing of all entries. 

### Required:
In order to generate the sqlite database type the following command in the Package Manager Console:

```Update-Database -Project DataAccess -StartupProject Services```

### Notes:
The Controller and its API endpoints are used for both models and gets the correct type of service dynamically (for testing purposes).

### TO DO:
- Let the user pick any domain/API endpoint.
- Let the user make mass requests.
- Auto-clean db.
