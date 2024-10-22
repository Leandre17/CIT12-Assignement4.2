using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;

    public ProductsController(IDataService dataService, LinkGenerator linkGenerator)
    {
        _dataService = dataService;
        _linkGenerator = linkGenerator;
    }

    [HttpGet("{id}", Name = nameof(GetProduct))]
    public IActionResult GetProduct(int id)
    {
        var Product = _dataService.GetProduct(id);

        if (Product == null)
            return NotFound();
        var model = CreateProductModel(Product);
        return Ok(model);
    }

    [HttpGet("category/{id}", Name = nameof(GetProductByCategory))]
    public IActionResult GetProductByCategory(int id)
    {
        var Product = _dataService.GetProductByCategory(id);

        if (Product == null || !Product.Any())
            return NotFound(new List<ProductModel>());
        var models = CreateProductModels(Product);
        return Ok(models);
    }

    [HttpGet(Name = nameof(GetProductsByName))]
    public IActionResult GetProductsByName([FromQuery] string name)
    {
        var Product = _dataService.GetProductByName(name);

        if (Product == null || !Product.Any())
            return NotFound(new List<ProductModel>());
        var models = CreateProductModels(Product);
        return Ok(models);
    }

    private ProductModel? CreateProductModel(ProductDTO? Product)
    {
        if (Product == null)
            return null;
        var model = Product.Adapt<ProductModel>();
        model.Url = GetUrl(Product.Id);
        return model;
    }

    private List<ProductModel> CreateProductModels(IList<ProductDTO> products)
    {
        if (products == null || !products.Any())
            return new List<ProductModel>();
        var models = products.Select(product =>
            {
                var model = product.Adapt<ProductModel>();
                model.Url = GetUrl(product.Id);
                return model;
            }).ToList();
        return models;
    }

    private string? GetUrl(int id)
    {
        return _linkGenerator.GetUriByName(HttpContext, nameof(GetProduct), new { id });
    }
}
