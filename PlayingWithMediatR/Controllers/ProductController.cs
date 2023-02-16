using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.MediatR;
using PlayingWithMediatR.Pagination;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace PlayingWithMediatR.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<PageResult<ProductDto>> Get(
        [FromQuery] GetAllProduct getAllProduct,
        CancellationToken cancelToken = default)
    {
        // CancellationToken is given by the framework. Default value makes your test easier.
        // You can pass this CancellationToken to the Mediator method.
        // In this case, if the caller cancel the request, you can stop your process.
        return await _mediator.Send(getAllProduct, cancelToken);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> Get(int id)
    {
        ProductDto product = await _mediator.Send(new GetProductById(id));

        if (product is null)
            return Problem(title: $"Product({id}) is not found.", statusCode: Status404NotFound);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Post([FromBody] CreateProduct createProduct)
    {
        // RequestValidationBehavior (using FluentValidation) throws an exception, if we have an invalid object.
        // Custom middleware will catch that exception.
        ProductDto product = await _mediator.Send(createProduct);

        return CreatedAtAction(nameof(Get), new { product.Id }, product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _mediator.Send(new DeleteProduct(id));

            return Ok();
        }
        catch (DeleteProductException ex)
        {
            ex.LogErrorIfSo($"Could not delete the product({id}).");

            throw; // Custom middleware will catch thit exception.
        }
    }
}
