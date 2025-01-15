using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Controllers;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.Models;

public class ProductsControllerTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;
    private readonly ApplicationDbContext _dbContext;

    public ProductsControllerTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _dbContext = new ApplicationDbContext(_dbContextOptions);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        _dbContext.Products.AddRange(new List<Product>
        {
            new Product { Code = "1234-5678", Name = "Test Product 1", Price = 10.99M },
            new Product { Code = "8765-4321", Name = "Test Product 2", Price = 5.49M }
        });
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task GetAllProducts_ReturnsAllProducts()
    {
        var controller = new ProductsController(_dbContext);

        var result = await controller.GetAllProducts();

        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = actionResult.Value;

        Assert.NotNull(response);
    }

    [Fact]
    public async Task GetProductByCode_ReturnsProduct()
    {
        var controller = new ProductsController(_dbContext);
        var productCode = "1234-5678";

        var result = await controller.GetProductByCode(productCode);

        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var product = Assert.IsType<Product>(actionResult.Value);
        Assert.Equal(productCode, product.Code);
    }

    [Fact]
    public async Task GetProductByCode_ReturnsNotFound()
    {
        var controller = new ProductsController(_dbContext);
        var productCode = "9999-9999";

        var result = await controller.GetProductByCode(productCode);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateProduct_AddsProduct()
    {
        var controller = new ProductsController(_dbContext);
        var newProduct = new Product
        {
            Code = "1111-2222",
            Name = "New Product",
            Price = 20.00M
        };

        var result = await controller.CreateProduct(newProduct);

        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var product = Assert.IsType<Product>(actionResult.Value);
        Assert.Equal("1111-2222", product.Code);
        Assert.Equal(3, _dbContext.Products.Count());
    }

    [Fact]
    public async Task DeleteProduct_RemovesProduct()
    {
        var controller = new ProductsController(_dbContext);
        var productCode = "1234-5678";

        var result = await controller.DeleteProduct(productCode);

        Assert.IsType<NoContentResult>(result);
        Assert.Null(_dbContext.Products.Find(productCode));
    }
}
