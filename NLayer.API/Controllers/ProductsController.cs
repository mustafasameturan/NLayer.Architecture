using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Services;

namespace NLayer.API.Controllers;

public class ProductsController : CustomBaseController
{
    private readonly IMapper _mapper;
    private readonly IProductService _service;

    public ProductsController(IMapper mapper, IProductService productService)
    {
        _mapper = mapper;
        _service = productService;
    }

    [HttpGet("getProductsWithCategory")]
    public async Task<IActionResult> GetProductsWithCategory()
    {
        return CreateActionResult(await _service.GetProductsWithCategory());
    }
    
    [HttpGet]
    public async Task<IActionResult> All()
    {
        var products = await _service.GetAllAsync();
        var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());;
        return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDto));
    }
    
    //Not found filter class'ı attribute sınıfını miras almadığı için direkt attribute olarak kullanamıyoruz!
    [ServiceFilter(typeof(NotFoundFilter<Product>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        var productDto = _mapper.Map<ProductDto>(product);;
        return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productDto));
    }

    [HttpPost]
    public async Task<IActionResult> Save(ProductDto productDto)
    {
        var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
        var productsDto = _mapper.Map<ProductDto>(product);;
        return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productDto));
    }

    [HttpPost("saveRange")]
    public async Task<IActionResult> SaveRange(IEnumerable<ProductDto> productDto)
    {
        var products = _mapper.Map<IEnumerable<Product>>(productDto);
        await _service.AddRangeAsync(products);

        var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
        return CreateActionResult(CustomResponseDto<IEnumerable<ProductDto>>.Success(201, productsDto));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
    {
        await _service.UpdateAsync(_mapper.Map<Product>(productUpdateDto));
        return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        var product = await _service.GetByIdAsync(id);
        await _service.RemoveAsync(product);
        return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
    }
}