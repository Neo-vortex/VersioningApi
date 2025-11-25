# Version Manager API Documentation

This document describes how to use the Version Manager API to manage
**projects**, **environments**, and **semantic versions**.

------------------------------------------------------------------------

## Table of Contents

1.  Introduction
2.  Prerequisites
3.  Authentication
4.  API Endpoints
    -   Projects
    -   Environments
    -   Versions
5.  DTOs (Data Transfer Objects)
6.  Scenarios
7.  Step-by-Step Usage Guide

------------------------------------------------------------------------

## Introduction

The Version Manager API helps teams manage:

-   Projects\
-   Environments such as `dev`, `staging`, `production`\
-   Semantic versions (`major.minor.patch`) for each environment

It is designed for CI/CD pipelines, deployment systems, and
configuration management.

------------------------------------------------------------------------

## Prerequisites

### 1. Secret Authentication

Set an environment variable named `SECRET`:

``` bash
export SECRET=your-secret-key
```

All API requests must include:

    secret: your-secret-key

### 2. Technology Stack

-   ASP.NET Core backend
-   DapperContext for database handling
-   Scalar.AspNetCore for API schema documentation

------------------------------------------------------------------------

## Authentication

Requests require a `secret` header.

### Example

``` http
GET /api/projects HTTP/1.1
secret: your-secret-key
```

If the header is missing or incorrect:

``` json
{
  "status": 403,
  "message": "Forbidden: Invalid or missing 'secret' header."
}
```

------------------------------------------------------------------------

## API Endpoints

# Projects

------------------------------------------------------------------------

### Create Project

**POST** `/api/Project`

**Request Body (string JSON):**

``` json
"MyProject"
```

**Response**

``` json
{
  "projectId": 1,
  "projectName": "MyProject"
}
```

------------------------------------------------------------------------

### Get Project By ID

**GET** `/api/Project/{id}`

------------------------------------------------------------------------

### Get All Projects

**GET** `/api/Project`

------------------------------------------------------------------------

### Delete Project

**DELETE** `/api/Project/{id}`\
Returns: **204 No Content**

------------------------------------------------------------------------

### Create Environment (Via ProjectController)

**POST** `/api/Project/{projectId}/Environment`

**Request Body:**

``` json
"dev"
```

------------------------------------------------------------------------

### Get Environments (Via ProjectController)

**GET** `/api/Project/{projectId}/Environment`

------------------------------------------------------------------------

# Environments

This section corresponds to EnvironmentController.

Base route:

    /api/projects/{projectId}/environments

------------------------------------------------------------------------

### Get Environments

**GET** `/api/projects/{projectId}/environments`

------------------------------------------------------------------------

### Create Environment

**POST** `/api/projects/{projectId}/environments`

**Request Body**

``` json
{
  "name": "dev"
}
```

------------------------------------------------------------------------

### Update Environment

**PUT**\
`/api/projects/{projectId}/environments/{environmentId}`

------------------------------------------------------------------------

### Delete Environment

**DELETE**\
`/api/projects/{projectId}/environments/{environmentId}`

------------------------------------------------------------------------

# Versions

Base route:

    /api/projects/{projectId}/{environment}/version

------------------------------------------------------------------------

### Get Current Version

**GET** `/api/projects/{projectId}/{environment}/version`

------------------------------------------------------------------------

### Set Version

**PUT** `/api/projects/{projectId}/{environment}/version`

------------------------------------------------------------------------

### Increment Version

**POST** `/api/projects/{projectId}/{environment}/version/increment`

------------------------------------------------------------------------

### Decrement Version

**POST** `/api/projects/{projectId}/{environment}/version/decrement`

------------------------------------------------------------------------

## DTOs (Data Transfer Objects)

### CreateEnvironmentRequest

``` json
{
  "name": "string"
}
```

### UpdateEnvironmentRequest

``` json
{
  "name": "string"
}
```

### SetVersionRequest

``` json
{
  "major": 0,
  "minor": 0,
  "patch": 0
}
```

### IncrementVersionRequest

``` json
{
  "part": "major | minor | patch"
}
```

### DecrementVersionRequest

``` json
{
  "part": "major | minor | patch"
}
```

------------------------------------------------------------------------

## Scenarios

### Managing a Software With Dev and Staging Environments

1.  Create a project

``` http
POST /api/Project
"MySoftware"
```

2.  Create environments

``` http
POST /api/projects/1/environments
{ "name": "dev" }
```

``` http
POST /api/projects/1/environments
{ "name": "staging" }
```

3.  Set version for dev

``` http
PUT /api/projects/1/dev/version
{ "major": 1, "minor": 0, "patch": 0 }
```

4.  Increment version on staging

``` http
POST /api/projects/1/staging/version/increment
{ "part": "minor" }
```

5.  Get version

``` http
GET /api/projects/1/staging/version
```

------------------------------------------------------------------------

## Step-by-Step Usage Guide

1.  Set your `SECRET` environment variable.
2.  Create a project.
3.  Add environments.
4.  Use Version endpoints to manage versions per environment.
5.  Integrate calls into CI/CD pipelines to automate version changes.

------------------------------------------------------------------------

## Contact

For issues or support, open an issue in the GitHub repository.
