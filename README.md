# CustomerTestApp

CustomerTestApp is a WPF/MVVM/Microservice application for managing customers. It includes a stylized list of customers and a customer editor, with a backend powered by gRPC and ASP.NET Core.

## Features

- Add, update, delete, and filter customers.
- Split view with a list of customers and an editor for customer details.
- Validation for customer properties.
- Logging with Serilog.
- gRPC communication between the desktop application and the microservice.
- Entity Framework for database operations.

## Setup Instructions

### Prerequisites

- .NET 8.0
- Visual Studio 2022
- SQLite

Sure, here it is formatted with bullet points for easy copy and paste:

### Clone the Repository
- git clone https://github.com/yourusername/CustomerTestApp.git
- cd CustomerTestApp

### Setting Up the Database
- Navigate to the `CustomerService` directory.
- Ensure SQLite is being used and the connection string is correct in `appsettings.json`:
  
  {
    "ConnectionStrings": {
      "DefaultConnection": "Data Source=customers.db"
    },
  }
  
- Apply migrations and update the database:
  - dotnet ef migrations add InitialCreate
  - dotnet ef database update

### Running the Application

#### Running the Customer Service
- Navigate to the `CustomerService` directory.
- Run the service: `dotnet run`
- The service will start and listen on the configured port.

#### Running the WPF Application
- Open the solution in Visual Studio.
- Set `CustomerTestApp` as the startup project.
- Run the application from Visual Studio.

### Usage
- **Add Customer**: Click on the "Add Customer" button to add a new customer.
- **Edit Customer**: Select a customer from the list to edit their details in the editor.
- **Delete Customer**: Click on the "Remove" button next to a customer to delete them.
- **Filter Customers**: Use the TextBox and ComboBox to filter customers by name or email.

### Development Guidelines

#### WPF
- **No Code-Behind**: Avoid using code behind in *.xaml.cs files.
- **Styles**: Implement and apply styles to avoid repeating markup elements. Choose between implicit and explicit styles optimally.
- **Split Views**: Split the main view into two parts: a list of customers and a customer editor. Make them resizable with minimum sizes.
- **Commands**: Implement commands for adding and removing customers. Use Messenger for communication between view models.
- **View Models**: Use view models for binding, inheriting from a base view model. Avoid using models directly for binding.
- **Filtering**: Add filtering functionality for the customer list based on name or email.
- **Editing**: Ensure that editing a customer does not update the list until saved. Validate input fields for discount and other properties.

#### ASP.NET
- **gRPC**: Use gRPC for communication. Implement streaming calls where needed.
- **CRUD Operations**: Implement CRUD operations and filtration in the gRPC service.
- **Logging**: Use Serilog for logging. Add a custom interceptor for logging.
- **Thread Safety**: Ensure all service methods are thread-safe. Use repository pattern to manage data access.
- **Dependency Injection**: Use dependency injection for all services.
- **Metadata**: Pass the calling application name to the service side and log it.


