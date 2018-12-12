﻿using System.Collections.Generic;
using System.Threading;
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
    public async Task<IEnumerable<ProductDto>> Get(CancellationToken cancelToken = default(CancellationToken))
    {
      // CancellationToken is given by the framework. Default value makes your test easier.
      // You can pass this CancellationToken to the Mediator method.
      // In this case, if the caller cancel the request, you can stop your process.
      return await _mediator.Send(new GetAllProduct(), cancelToken);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> Get(int id)
    {
      ProductDto product = await _mediator.Send(new GetProductById(id));

      if (product == null)
        return BadRequest(new { Message = $"Product({id}) is not found." });

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

        return NoContent();
      }
      catch (DeleteProductException ex)
      {
        ex.LogErrorIfSo($"Could not delete the product({id}).");

        throw; // Custom middleware will catch thit exception.
      }
    }
  }
}
