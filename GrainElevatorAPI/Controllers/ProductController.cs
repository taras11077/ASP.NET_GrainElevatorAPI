using AutoMapper;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.DTOs;
using GrainElevatorAPI.DTO.Requests.CreateRequests;
using GrainElevatorAPI.DTOs;
using GrainElevatorAPI.DTOs.Requests;
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
    
    private CancellationToken GetCancellationToken()
    {
        return HttpContext.RequestAborted;
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Laboratory,Accountant")]
    public async Task<ActionResult<ProductDto>> CreateProduct(ProductCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var createdById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            if (createdById <= 0)
                return Unauthorized(new { message = "Користувач не авторизований." });
            
            var newProduct = _mapper.Map<Product>(request);
            
            var createdProduct = await _productService.CreateProductAsync(newProduct, createdById, cancellationToken);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, _mapper.Map<ProductDto>(createdProduct));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при створенні Найменування продукції: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при створенні Найменування продукції: {ex.Message}");
        }
    }
    
    
    [HttpGet]
    [Authorize(Roles = "Admin,Laboratory,Technologist,Accountant,CEO")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var products = await _productService.GetProducts(page, size, cancellationToken);
            
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні всіх Найменувань продукції: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні всіх Найменувань продукції: {ex.Message}");
        }
    }
    
    
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Laboratory,Technologist,Accountant,CEO")]
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var product = await _productService.GetProductByIdAsync(id, cancellationToken);
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
    [Authorize(Roles = "Admin,Laboratory,Technologist,Accountant,CEO")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts(
        [FromQuery] string? title,
        [FromQuery] string? createdByName,
        [FromQuery] int page = 1,
        [FromQuery] int size = 10,
        [FromQuery] string? sortField = null,
        [FromQuery] string? sortOrder = null)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var (products, totalCount) = await _productService.SearchProductsAsync(
                title,
                createdByName,
                page, 
                size,
                sortField, sortOrder,
                cancellationToken);
            
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            Response.Headers.Append("X-Total-Count", totalCount.ToString());
            
            return Ok(productsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при отриманні Найменування продукції за назвою: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при отриманні Найменування продукції за назвою: {ex.Message}");
        }
    } 
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Laboratory,Accountant")]
    public async Task<IActionResult> UpdateProduct(int id, ProductCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        try
        {
            var cancellationToken = GetCancellationToken();
            var productDb = await _productService.GetProductByIdAsync(id, cancellationToken);
            if (productDb == null)
            {
                return NotFound($"Продукт з ID {id} не знайдений.");
            }
            
            productDb.Title = request.Title;
            var modifiedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var updatedProduct = await _productService.UpdateProductAsync(productDb, modifiedById, cancellationToken);
            
            return Ok(_mapper.Map<ProductDto>(updatedProduct));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при оновленні Найменування продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при оновленні Найменування продукції з ID {id}: {ex.Message}");
        }
    }
    
    
    [HttpPatch("{id}/soft-remove")]
    [Authorize(Roles = "Admin,Laboratory,Accountant")]
    public async Task<IActionResult> SoftDeleteProduct(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var productDb = await _productService.GetProductByIdAsync(id, cancellationToken);
            if (productDb == null)
            {
                return NotFound($"Продукт з ID {id} не знайдено.");
            }
            
            var removedById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var removedProduct = await _productService.SoftDeleteProductAsync(productDb, removedById, cancellationToken);
            
            return Ok(_mapper.Map<ProductDto>(removedProduct));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при soft-видаленні Найменування продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при видаленні Найменування продукції з ID {id}: {ex.Message}");
        }
    }
    

    [HttpPatch("{id}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RestoreRemovedProduct(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var productDb = await _productService.GetProductByIdAsync(id, cancellationToken);
            if (productDb == null)
            {
                return NotFound($"Продукт з ID {id} не знайдено.");
            }
            
            var restoredById = HttpContext.Session.GetInt32("EmployeeId").GetValueOrDefault();
            var restoredProduct = await _productService.RestoreRemovedProductAsync(productDb, restoredById, cancellationToken);
            
            return Ok(_mapper.Map<ProductDto>(restoredProduct));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Внутрішня помилка сервера при відновленні Найменування продукції з ID {id}: {ex.Message}");
            return StatusCode(500, $"Внутрішня помилка сервера при відновленні Найменування продукції з ID {id}: {ex.Message}");
        }
    }
    
    [HttpDelete("{id}/hard-remove")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var cancellationToken = GetCancellationToken();
            var success = await _productService.DeleteProductAsync(id, cancellationToken);
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

