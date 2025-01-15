using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ProductsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAllProducts(
        string? search = null,
        string? sortBy = "Name",
        bool descending = false,
        int page = 1,
        int pageSize = 10)
    {
        if (page < 1 || pageSize < 1)
        {
            return BadRequest("Page and pageSize must be greater than 0");
        }

        var query = _dbContext.Products.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));
        }

        query = sortBy?.ToLower() switch
        {
            "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "code" => descending ? query.OrderByDescending(p => p.Code) : query.OrderBy(p => p.Code),
            _ => descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name)
        };

        var totalItems = await query.CountAsync();
        var products = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new { p.Code, p.Name, p.Price })
            .ToListAsync();

        return Ok(new
        {
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            Data = products
        });
    }

    [HttpGet("{productCode}")]
    public async Task<ActionResult<Product>> GetProductByCode(string productCode)
    {
        var product = await _dbContext.Products.FindAsync(productCode);

        if (product == null)
        {
            return NotFound($"Product with this code '{productCode}' not found");
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product newProduct)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProductByCode), new { productCode = newProduct.Code }, newProduct);
    }

    [HttpPut("{productCode}")]
    public async Task<IActionResult> UpdateProduct(string productCode, Product updatedProduct)
    {
        if (productCode != updatedProduct.Code)
        {
            return BadRequest("Product code in the URL does not match the code in the request");
        }

        var existingProduct = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Code == productCode);
        if (existingProduct == null)
        {
            return NotFound($"Product with this code '{productCode}' not found");
        }

        _dbContext.Entry(updatedProduct).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExistsAsync(productCode))
            {
                return NotFound($"Product with this code '{productCode}' not found");
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{productCode}")]
    public async Task<IActionResult> DeleteProduct(string productCode)
    {
        var product = await _dbContext.Products.FindAsync(productCode);

        if (product == null)
        {
            return NotFound($"Product with this code '{productCode}' not found");
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ProductExistsAsync(string productCode)
    {
        return await _dbContext.Products.AnyAsync(product => product.Code == productCode);
    }
}
