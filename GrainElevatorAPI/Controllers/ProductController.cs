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
    public async Task<ActionResult<Product>> PostSupplier(ProductCreateRequest request)
    {
        try
        {
            var createdProduct = await _productService.AddProductAsync(request.Title);
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
                return NotFound($"Роль з ID {id} не знайдено.");
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
    public async Task<IActionResult> PutSupplier(int id, ProductDTO productDto)
    {
        if (id != productDto.Id)
        {
            return BadRequest();
        }
        
        try
        {
            var productDb = await _productService.GetProductByIdAsync(id);
        
            productDb.Title = productDto.Title;
        
            var updatedProduct = await _productService.UpdateProductAsync(productDb);
        
            if (updatedProduct == null)
            {
                return NotFound($"Співробітник з ID {id} не знайдений.");
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
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound($"Роль з ID {{id}} не знайдено.");
            }

            return NoContent();
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

