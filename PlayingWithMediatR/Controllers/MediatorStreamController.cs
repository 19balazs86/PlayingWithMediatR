using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlayingWithMediatR.MediatR;

namespace PlayingWithMediatR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediatorStreamController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MediatorStreamController(IMediator mediator) => _mediator = mediator;

        [HttpGet("RandomNumbers/{amount}")]
        public IAsyncEnumerable<int> GetRandomNumbers([FromRoute] int amount, CancellationToken cancellationToken)
        {
            var request = new GetRandomNumberRequest { AmountOfRandomNumbers = amount };

            return _mediator.CreateStream(request, cancellationToken);
        }
    }
}
