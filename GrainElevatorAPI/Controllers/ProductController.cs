using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }
    
    // POST: api/Product
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(ProductCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var newProduct = _mapper.Map<Product>(request);
                
            newProduct.CreatedAt = DateTime.UtcNow;
            newProduct.CreatedById = (int)HttpContext.Session.GetInt32("EmployeeId");
            
            var createdProduct = await _productService.AddProductAsync(newProduct);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, _mapper.Map<ProductDTO>(createdProduct));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // GET: api/Product
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var products = _productService.GetProducts(page, size);
            return Ok(_mapper.Map<IEnumerable<ProductDTO>>(products));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // GET: api/Product/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Продукт з ID {id} не знайдено.");
            }
            return Ok(_mapper.Map<ProductDTO>(product));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    // PUT: api/Product/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, ProductCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var productDb = await _productService.GetProductByIdAsync(id);
        
            productDb.Title = request.Title;
        
            var updatedProduct = await _productService.UpdateProductAsync(productDb);
        
            if (updatedProduct == null)
            {
                return NotFound($"Продукт з ID {id} не знайдений.");
            }

            return Ok(_mapper.Map<ProductDTO>(updatedProduct));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }

    
    // DELETE: api/Product/5
    [HttpDelete("{id}")]
    //[Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound($"Продукт з ID {{id}} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    // Patch: api/Product/5
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> SoftDeleteProduct(int id)
    {
        try
        {
            var productDb = await _productService.GetProductByIdAsync(id);
            
            productDb.Removed = true;
            productDb.RemovedAt = DateTime.UtcNow;
            productDb.RemovedById = (int)HttpContext.Session.GetInt32("EmployeeId");
            
            var removedProduct = await _productService.UpdateProductAsync(productDb);
            
            if (removedProduct == null)
            {
                return NotFound($"Продукт з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<ProductDTO>(removedProduct));
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    // Patch: api/Product/5
    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> RestoreRemovedProduct(int id)
    {
        try
        {
            var productDb = await _productService.GetProductByIdAsync(id);
            
            productDb.Removed = false;
            productDb.RestoredAt = DateTime.UtcNow;
            productDb.RestoreById = (int)HttpContext.Session.GetInt32("EmployeeId");
            
            var restoredProduct = await _productService.UpdateProductAsync(productDb);
            
            if (restoredProduct == null)
            {
                return NotFound($"Продукт з ID {id} не знайдено.");
            }

            return Ok(_mapper.Map<ProductDTO>(restoredProduct));
            
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }
    
    
    // GET: api/Product/search?title=Hortytsya
    [HttpGet("search")]
    public ActionResult<IEnumerable<Product>> SearchProducts(string title)
    {
        try
        {
            var products = _productService.SearchProduct(title);
            return Ok(_mapper.Map<IEnumerable<ProductDTO>>(products));
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    } 
}

