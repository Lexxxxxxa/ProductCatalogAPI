# ProductCatalogAPI

## Technologies

- **C# (.NET 8+)**
- **ASP.NET Core**
- **PostgreSQL**
- **Entity Framework Core**
- **Swagger** for API documentation
- **xUnit** for testing

## API Features

- **Add a new product**
- **View a list of products** (sorting, filtering, pagination)
- **Get product details by code**
- **Update product information**
- **Delete a product**

## How to Run the Project

### 1. Install Required Software

- **.NET SDK** (8+)
- **PostgreSQL**
- Any IDE, like Visual Studio.

### 2. Clone the Project

```bash
git clone https://github.com/Lexxxxxxa/ProductCatalogAPI.git
cd ProductCatalogAPI
```

### 3. Configure the Database

1. Create a database in PostgreSQL (e.g., `ProductCatalog`).
2. Update `appsettings.json`:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=ProductCatalog;Username=yourusername;Password=yourpassword"
   }
   ```

### 4. Apply Migrations

```bash
dotnet ef database update
```

### 5. Run the Project

```bash
dotnet run
```

The API will be available at: `https://localhost:{port}`

### 6. Open Swagger

Go to `https://localhost:{port}/swagger` to view API documentation

## Tests

1. Open the Test Explorer in Visual Studio
2. Run the tests

