# Webshop management application
> It's an application for managing a webshop

## General info
The application is designed to manage an online shop. It allows you to manage employees, categories, products and also to view customer details. Two panels are available, for administrators and employees. As an administrator you can manage employees, view detailed customer data. As an employee you have a panel to manage products, categories and customer orders. Customers can browse products on the homepage and go to explore specific products through categories. By clicking on a product, customers can see its details and add it to their shopping cart.

**The application was developed based on Clean Architecture pattern.**

## Online version (Oracle Cloud + nginx)
Default administration account -
Email: admin@admin.com Password: Pa$$w0rd!

## Technologies
* .NET 5
* Entity Framework Core 5
* AutoMapper
* XUnit
* Moq
* Fluent Validation
* MSSQL
* Bootstrap 4
* LINQ
* ASP.NET Core
* HTML, CSS
* Dependency Injection
* WebApi

## Features
### Administration panel:
* Employee Management - Employee overview, possibility to add, edit data and delete employees.
* Customer Management - Customer overview, checking of details (recent orders, current items in the shopping cart) and possibility to delete customers.
### Employee Panel:
* Category Management - Category overview, possibility to add, edit and delete categories.
* Product Management - Product overview, allows inventory management including adding, editing and deleting products.
* Order Management - Order overview, possibility of viewing order details and deleting it.
### Customer functionalities
* Browsing products, viewing product details, adding to shopping cart.
* Shopping Cart - Display of cart contents, increase, decrease quantity of item, deleting items.
* Order History - The customer can view their previous orders.
