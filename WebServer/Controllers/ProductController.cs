using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/Products")]
public class ProductsController : ControllerBase
{
    IDataService _dataService;
    private readonly LinkGenerator _linkGenerator;

    public ProductsController(
        IDataService dataService,
        LinkGenerator linkGenerator)
    {
        _dataService = dataService;
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        var Products = _dataService?
            .GetProducts()
            .Select(CreateProductModel);
        return Ok(Products);
    }

    [HttpGet("{id}", Name = nameof(GetProduct))]
    public IActionResult GetProduct(int id)
    {
        var Product = _dataService.GetProduct(id);

        if (Product == null)
        {
            return NotFound();
        }
        var model = CreateProductModel(Product);

        return Ok(model);
    }

    private ProductModel? CreateProductModel(ProductDTO? Product)
    {
        if(Product == null)
        {
            return null;
        }

        var model = Product.Adapt<ProductModel>();
        model.Url = GetUrl(Product.Id);

        return model;
    }

    private string? GetUrl(int id)
    {
        return _linkGenerator.GetUriByName(HttpContext, nameof(GetProduct), new { id });
    }
}
