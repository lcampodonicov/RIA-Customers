
---

# RIA Customers – REST API

## Overview

**RIA Customers** is a RESTful API designed to manage customer records in a database. It allows the creation and retrieval of customer data with validation rules and sorting. The project is built with **.NET 6** and **Entity Framework Core**, and it includes a minimal frontend for interacting with the API.

## Features

* **UI /Customer/Index**
   Small UI with a button that generates 10 customers as required, and a button to reset the database.
* **POST /Customer**
  Create one or more customer records with validation.
* **GET /Customer**
  Retrieve all customer records sorted alphabetically by first name, then last name.
* **Validations**:

  * Required fields: `Id`, `FirstName`, `LastName`, `Age`.
  * IDs must be unique and not duplicated in the payload.
  * Age must be greater than 18.
  * IDs must not already exist in the database.
  * IDs must not be duplicated within the same input

## Tech Stack

* **Backend**: ASP.NET Core 6
* **Database**: SQLite
* **ORM**: Entity Framework Core
* **Frontend**: Minimal UI using Bootstrap 5, plain HTML/CSS/JS

## Prerequisites

* [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download)
* [Docker](https://www.docker.com/) (optional, for containerized deployment)

## Setup Instructions

### 🖥️ Run locally (no Docker)

```bash
git clone https://github.com/lcampodonicov/RIA-Customers
cd RIA-Customers
dotnet restore
dotnet build
dotnet run
```

The API will be available at `http://localhost:5000` (or another port if configured).

### 🐳 Run in Docker

You can build and run the project in a Docker container:

```bash
docker build -f riacustomers.rest.dockerfile -t ria-customers .
docker run -p 8080:8080 ria-customers
```

Access it via `http://localhost:8080`.

## Usage

You can use tools like Postman or curl to interact with the API.

There is also a minimal UI at /Customer/Index that generates 10 users with random values as stated in the requirements.

### POST /Customer

Submit a list of customers:

```json
{
  "Customers": [
    {
      "Id": 1,
      "FirstName": "Sara",
      "LastName": "Lane",
      "Age": 32
    }
  ]
}
```

### GET /Customers

Returns a sorted list of all registered customers.

## Notes

* Pagination was considered but intentionally omitted for brevity.
* Authentication and login were not included due to time constraints and limited added value for this assignment.

## Development Log

The full task breakdown with timestamps is available in the `Tasks.todo` file under the HumanLayer/ folder

## Possible Future Enhancements

* Add JWT authentication
* Add pagination to the `/Customers` GET endpoint
* Improved error handling middleware
* Frontend enhancements with dynamic tables or charts

## License

This project is open-source and available under the MIT License.

