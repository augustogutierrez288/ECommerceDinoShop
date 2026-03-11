# ECommerce DinoShop 🦖🛒

Welcome to the **ECommerce DinoShop** repository! 

This project is a comprehensive e-commerce solution developed as an academic assignment to pass the **Systems Methodology** (Metodología de Sistemas) course in the **University Degree in Programming** (Tecnicatura Universitaria en Programación) at **UTN - Facultad Regional Tucumán (UTN-FRT)**.

## 🎯 Project Objective

The main purpose of this application is purely academic and demonstrative. It serves as practical evidence of the knowledge acquired during the degree, applying software development methodologies, software architecture, design patterns, and best practices using the **.NET** ecosystem.

## 🏗️ Architecture and Solution Layers

The project is structured using an **N-Tier Architecture** to separate concerns, facilitate maintenance, scalability, and testing. Below is the role of each project within the solution:

### 1. `ECommerceDinoShop.Model` (Domain Layer)
Contains the fundamental business entities (e.g., `User`, `Product`, `Category`, `Order`, `OrderDetail`). These classes represent the data structure and are directly mapped to the database.

### 2. `ECommerceDinoShop.DTO` (Data Transfer Objects)
This layer defines the data transfer objects. Its function is to isolate the domain models from the presentation/API layer. It contains specific classes for sending or receiving structured information (e.g., `LoginDTO`, `CartDTO`, and integrations like `MercadoPagoData`), preventing direct exposure of the database structure and optimizing network payloads.

### 3. `ECommerceDinoShop.Repository` (Data Access Layer)
Exclusively responsible for data persistence and retrieval. 
- Uses **Entity Framework Core** through the `DbdinoShopContext`.
- Implements the **Repository Pattern** (like `IGenericRepository` and `IOrderRepository`) to abstract database queries, centralizing CRUD operations and keeping the rest of the application agnostic to the underlying database engine.

### 4. `ECommerceDinoShop.Service` (Business Logic Layer)
The heart of the application. Here, contracts (`Interfaces`) and business logic (`IProductService`, `IOrderService`, `IPaymentService`, etc.) are defined and implemented. This layer consumes repositories, applies business rules (such as shipping calculations, stock validation, or payment processing), and returns results to the API.

### 5. `ECommerceDinoShop.Utilities` (Cross-cutting & Utilities Layer)
Contains tools and configurations that span the entire solution. Primarily houses the **AutoMapper Profiles** (`AutoMapperProfile.cs`), which automatically transform domain objects (`Models`) to transfer objects (`DTOs`) and vice versa.

### 6. `ECommerceDinoShop.API` (Backend Presentation Layer)
The entry point to the backend services. It exposes a set of RESTful endpoints through Controllers (like `ProductController`, `OrderController`, `PaymentController`). It receives HTTP requests from the client, delegates processing to the Service layer, and returns JSON responses.

### 7. `ECommerceDinoShop.WebAssembly` (Frontend Presentation Layer)
A Single Page Application (SPA) developed with **Blazor WebAssembly**. 
It runs directly in the client's browser using C# and .NET. It features components for the virtual store (`Catalog`, `Cart`, `Checkout`) and an administration panel (`Dashboard`, `Products`, `Orders`, `Users`). This layer interacts with the backend by consuming the REST API.

## 🚀 Technologies Used

* **Backend:** C#, .NET 9 (or higher), ASP.NET Core Web API.
* **Frontend:** Blazor WebAssembly, HTML/CSS, Bootstrap.
* **Data Access:** Entity Framework Core.
* **Object Mapping:** AutoMapper.
* **Integrations:** Mercado Pago (Webhooks and payment processing).

## 🧑‍💻 Author

Developed by **Augusto Gutierrez**, student of the University Degree in Programming (UTN-FRT).
