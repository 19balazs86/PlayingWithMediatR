using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlayingWithMediatR.Entities;
using PlayingWithMediatR.Exceptions;
using PlayingWithMediatR.MediatR;

namespace PlayingWithMediatR.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase
  {
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet]
    public Task<IEnumerable<Product>> Get()
    {
      return _mediator.Send(new GetAllProduct());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> Get(int id)
    {
      Product product = await _mediator.Send(new GetProductById { Id = id });

      if (product == null)
        return NotFound(new { Message = $"Product({id}) is not found." });

      return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Post([FromBody] CreateProduct createProduct)
    {
      // RequestValidationBehavior (using FluentValidation) throws an exception, if we have an invalid object.
      Product product = await _mediator.Send(createProduct);

      return CreatedAtAction(nameof(Get), new { product.Id }, product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        await _mediator.Send(new DeleteProduct { Id = id });

        return NoContent();
      }
      catch (DeleteProductException ex)
      {
        ex.LogErrorIfSo($"Could not delete the product({id}).");

        throw;
      }
    }
  }
}
