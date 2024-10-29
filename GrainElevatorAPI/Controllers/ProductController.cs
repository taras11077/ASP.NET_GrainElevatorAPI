using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrainElevatorAPI.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, IMapper mapper, ILogger<ProductController> logger)
    {
        _productService = productService;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpPost]
    //[Authorize(Roles = "admin")]
    public async Task<ActionResult<Product>> CreateProduct(ProductCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var newProduct = _mapper.Map<Product>(request);
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            
            var createdProduct = await _productService.CreateProductAsync(newProduct, createdById);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, _mapper.Map<ProductDto>(createdProduct));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Найменування продукції: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Найменування продукції: {ex.Message}");
        }
    }
    
    
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var products = _productService.GetProducts(page, size);
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Найменувань продукції: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Найменувань продукції: {ex.Message}");
        }
    }
    
    
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
            return Ok(_mapper.Map<ProductDto>(product));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Найменування продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Найменування продукції з ID {id}: {ex.Message}");
        }
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Product>> SearchProducts(string title)
    {
        try
        {
            var products = _productService.SearchProduct(title);
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Найменування продукції за назвою: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Найменування продукції за назвою: {ex.Message}");
        }
    } 
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var productDb = await _productService.GetProductByIdAsync(id);
            if (productDb == null)
            {
                return NotFound($"Продукт з ID {id} не знайдений.");
            }
            
            productDb.Title = request.Title;
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedProduct = await _productService.UpdateProductAsync(productDb, modifiedById);
            
            return Ok(_mapper.Map<ProductDto>(updatedProduct));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні Найменування продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Найменування продукції з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> SoftDeleteProduct(int id)
    {
        try
        {
            var productDb = await _productService.GetProductByIdAsync(id);
            if (productDb == null)
            {
                return NotFound($"Продукт з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedProduct = await _productService.SoftDeleteProductAsync(productDb, removedById);
            
            return Ok(_mapper.Map<ProductDto>(removedProduct));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Найменування продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Найменування продукції з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    //[Authorize(Roles = "admin, laboratory")]
    public async Task<IActionResult> RestoreRemovedProduct(int id)
    {
        try
        {
            var productDb = await _productService.GetProductByIdAsync(id);
            if (productDb == null)
            {
                return NotFound($"Продукт з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredProduct = await _productService.RestoreRemovedProductAsync(productDb, restoredById);
            
            return Ok(_mapper.Map<ProductDto>(restoredProduct));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Найменування продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Найменування продукції з ID {id}: {ex.Message}");
        }
    }
    
    [HttpDelete("{id}/hard-remove")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound($"Продукт з ID {id} не знайдено.");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при hard-видаленні Найменування продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при hard-видаленні Найменування продукції з ID {id}: {ex.Message}");
        }
    }
    
}

