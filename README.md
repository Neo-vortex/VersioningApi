
#Documentation

This document outlines the details, usage, and scenarios of the API. It provides sample requests and responses, step-by-step guides, and detailed explanations of how to use the API effectively.

---

## Table of Contents

1. [Introduction](#introduction)
2. [Prerequisites](#prerequisites)
3. [Authentication](#authentication)
4. [API Endpoints](#api-endpoints)
   - [Projects](#projects)
   - [Environments](#environments)
   - [Versions](#versions)
5. [DTOs (Data Transfer Objects)](#dtos)
6. [Scenarios](#scenarios)
   - [Managing a Software with Dev and Staging Environments](#managing-a-software-with-dev-and-staging-environments)
7. [Step-by-Step Usage Guide](#step-by-step-usage-guide)

---

## Introduction

The **Version Manager API** allows you to manage projects, environments, and versions in a streamlined way. This API is designed for teams who need structured version control and environment management for their applications.

---

## Prerequisites

1. **Environment Configuration**:
   - The API uses a `secret` header for authentication. You must set an environment variable `SECRET` with your chosen secret key. If not set, the default value is `"changeme"`.
2. **Framework**:
   - ASP.NET Core backend.
3. **Dependencies**:
   - Install necessary packages for `DapperContext`, `Scalar.AspNetCore`, and services.

---

## Authentication

The API uses a custom header `secret` for authentication. Requests must include this header to access protected endpoints.

### Example:
```http
GET /api/projects HTTP/1.1
Host: yourdomain.com
secret: your-secret-key
```

If the header is missing or invalid, the API will respond with:
```json
{
  "status": 403,
  "message": "Forbidden: Invalid or missing 'secret' header."
}
```

Set the environment variable in your application using:
```bash
export SECRET=your-secret-key
```

---

## API Endpoints

### Projects

#### Create a Project
**Endpoint**: `POST /api/projects`  
**Request Body**:
```json
{
  "name": "MySoftware",
  "environments": ["dev", "staging"]
}
```
**Response**:
```json
{
  "projectId": 1,
  "projectName": "MySoftware"
}
```

#### Get All Projects
**Endpoint**: `GET /api/projects`  
**Response**:
```json
[
  {
    "projectId": 1,
    "projectName": "MySoftware"
  }
]
```

#### Delete a Project
**Endpoint**: `DELETE /api/projects/{id}`  
**Response**: `204 No Content`

---

### Environments

#### Create an Environment
**Endpoint**: `POST /api/projects/{projectId}/environments`  
**Request Body**:
```json
{
  "name": "dev"
}
```
**Response**:
```json
{
  "environmentId": 1,
  "name": "dev"
}
```

#### Get Environments for a Project
**Endpoint**: `GET /api/projects/{projectId}/environments`  
**Response**:
```json
[
  {
    "environmentId": 1,
    "name": "dev"
  },
  {
    "environmentId": 2,
    "name": "staging"
  }
]
```

---

### Versions

#### Get Current Version
**Endpoint**: `GET /api/projects/{projectId}/{environment}/version`  
**Response**:
```json
{
  "projectId": 1,
  "environment": "dev",
  "version": "1.0.0"
}
```

#### Set Version
**Endpoint**: `PUT /api/projects/{projectId}/{environment}/version`  
**Request Body**:
```json
{
  "major": 1,
  "minor": 1,
  "patch": 0
}
```
**Response**:
```json
{
  "message": "Version updated successfully."
}
```

---

## DTOs (Data Transfer Objects)

### CreateEnvironmentRequest
```json
{
  "name": "string"
}
```

### CreateProjectRequest
```json
{
  "name": "string",
  "environments": ["string"]
}
```

### SetVersionRequest
```json
{
  "major": "int",
  "minor": "int",
  "patch": "int"
}
```

---

## Scenarios

### Managing a Software with Dev and Staging Environments

#### Scenario:
You have a software project with two environments: `dev` and `staging`. You want to use the API to manage these environments and automate version updates during CI/CD.

**Steps**:
1. **Create the Project**:
   ```http
   POST /api/projects
   ```
   **Request Body**:
   ```json
   {
     "name": "MySoftware",
     "environments": ["dev", "staging"]
   }
   ```

2. **Get the Project's Environments**:
   ```http
   GET /api/projects/1/environments
   ```

3. **Set the Version for `dev` Environment**:
   ```http
   PUT /api/projects/1/dev/version
   ```
   **Request Body**:
   ```json
   {
     "major": 1,
     "minor": 0,
     "patch": 0
   }
   ```

4. **Increment Version for `staging` During Deployment**:
   ```http
   POST /api/projects/1/staging/version/increment
   ```
   **Request Body**:
   ```json
   {
     "part": "minor"
   }
   ```

5. **Get the Updated Version for `staging`**:
   ```http
   GET /api/projects/1/staging/version
   ```

---

## Step-by-Step Usage Guide

1. Configure your `SECRET` key in the environment:
   ```bash
   export SECRET=your-secret-key
   ```

2. Create a new project with environments.

3. Use the API to manage versions across environments during CI/CD.

---

## Contact

For any issues or questions, feel free to open an issue on the GitHub repository.

