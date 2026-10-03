# EventsHub - Project Structure

## 1. Overview

EventsHub is a backend application for managing events and their related operations,
including event categories, halls, ticket types, ticket reservations, orders, payments, and user accounts.

The project is organized into multiple layers to separate API responsibilities, 
business logic, domain models, and infrastructure concerns.

API — Handles HTTP requests and exposes application endpoints.
Application — Contains application services, business workflows, DTOs, and validation.
Domain — Defines the core entities, interfaces, enums, and domain exceptions.
Infrastructure — Implements persistence, repositories, database configuration, email delivery, and payment integrations.

## 2. Solution Structure

EventsHub/ 
│ 
├── API/ 
│   ├── Controllers/ 
│   ├── DTOs/ 
│   ├── Extensions/
│   ├── Program.cs 
│   └── appsettings.json 
│ 
├── Application/ 
│   ├── CartItems/ 
│   ├── Carts/ 
│   ├── Email/ 
│   ├── EventCategories/ 
│   ├── EventOccurences/ 
│   ├── Events/ 
│   ├── Halls/ 
│   ├── OrderItems/ 
│   ├── Orders/ 
│   ├── Payments/ 
│   ├── Refresh Token/ 
│   ├── ResetPasswordToken/ 
│   ├── Tickets/ 
│   ├── TicketTypes/ 
│   ├── Users/ 
│   └── Vertification/ 

├── Domain/ 
│   ├── Common/ 
│   ├── Contracts/ 
│   ├── Entities/ 
│   ├── Enums/ 
│   ├── Exceptions/ 
│   └── Interfaces/ 

├── Infrastructure/ 
│   ├── Configurations/ 
│   ├── DbContexts/ 
│   ├── Email/ 
│   ├── Payments/ 
│   └── Repositories/ 
|
└── EventsHub.sln

## 3. Project Responsibilities

### API

The API project is the entry point of the application.

Its responsibilities include:

  - Exposing REST endpoints through controllers.
  - Receiving and processing HTTP requests.
  - Handling authentication-related endpoints.
  - Defining API request and response models.
  - Configuring middleware and application startup.
  - Configuring CORS and rate limiting.

Important areas:

Component	| Responsibility
------------+----------------------------------------------------------------------------------------
Controllers	| Expose endpoints for users, events, categories, authentication, email, and verification.
DTOs	    | Define API-specific request and response models.
Extensions	| Provide reusable API configuration, including CORS and rate limiting.
Program.cs	| Configure and start the application.


### Application


The Application project contains the main application logic.

It organizes functionality by feature, including:

Events and event categories
Halls and event occurrences
Ticket types and ticket generation
Carts and cart items
Orders and checkout
Payments
User management
Authentication-related workflows
Email and verification

It also contains:

Service interfaces and implementations
Request and response DTOs
Input validation
Mapping profiles
Application dependency injection configuration

### Domain

The Domain project contains the core business models and abstractions.

Its main responsibilities include:

Defining entities such as User, Event, Hall, Ticket, Order, Payment, and Cart.
Defining enums for statuses and roles.
Declaring repository interfaces.
Defining contracts for payment-related operations.
Defining domain exceptions.
Providing shared domain abstractions.

The Domain layer defines what the application works with 
without containing the concrete database repository implementations.

### Infrastructure

The Infrastructure project provides concrete implementations for external and persistence-related concerns.

Its responsibilities include:

Database context and entity configurations.
Repository implementations.
Unit of Work implementation.
Email sending.
Paymob payment integration.
Payment HMAC validation.
Infrastructure dependency injection.

## 4. Main Business Modules

4. Main Functional Areas

Based on the current solution structure, EventsHub contains the following functional areas:

Area	         |  Main responsibilities
-----------------+----------------------------------------------------------------------------------------
Event Management |  Create, update, and retrieve events.
-----------------+----------------------------------------------------------------------------------------
Event Categories |  Organize events into categories.
-----------------+----------------------------------------------------------------------------------------
Halls			 |  
and 	         |  Manage event venues and scheduled occurrences.
Occurrences		 |  
-----------------+----------------------------------------------------------------------------------------
Ticket Types	 |  Define ticket categories and their details.
-----------------+----------------------------------------------------------------------------------------
Cart             |  Manage selected items before checkout.
-----------------+----------------------------------------------------------------------------------------
Orders			 |  Handle order creation, checkout, and queries.
-----------------+----------------------------------------------------------------------------------------
Tickets			 |  Generate and manage tickets.
-----------------+----------------------------------------------------------------------------------------
Payments		 |  Handle payment operations and Paymob integration.
-----------------+----------------------------------------------------------------------------------------
User Management	 |  Manage user information and profiles.
-----------------+----------------------------------------------------------------------------------------
Authentication	 |  Handle login, logout, token refresh, email verification, and password reset workflows.
-----------------+----------------------------------------------------------------------------------------
Email			 |  Support application email delivery and verification messages.


## 5. Architecture and Request Flow

The general architectural flow is:

Client
  |
  v
API Controllers
  |
  v
Application Services
  |
  v
Domain Interfaces
  |
  v
Infrastructure Implementations
  |
  v
Database / External Services

The API receives requests and delegates application operations to the Application layer.

Application services coordinate the required operations and use domain abstractions to access persistence or other capabilities.

Infrastructure provides the concrete implementations for database access, email delivery, and payment integration.
