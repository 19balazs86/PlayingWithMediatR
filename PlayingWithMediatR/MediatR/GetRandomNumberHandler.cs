using MediatR;
using System.Runtime.CompilerServices;

namespace PlayingWithMediatR.MediatR;

// https://youtu.be/2TT3suofNlo?t=511
public sealed class GetRandomNumberHandler : IStreamRequestHandler<GetRandomNumberRequest, int>
{
    public async IAsyncEnumerable<int> Handle(GetRandomNumberRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        int counter = 0;

        while (!cancellationToken.IsCancellationRequested && counter++ < request.AmountOfRandomNumbers)
        {
            await Task.Delay(1_000, cancellationToken);

            yield return Random.Shared.Next();
        }
    }
}

public sealed class GetRandomNumberRequest : IStreamRequest<int>
{
    public int AmountOfRandomNumbers { get; set; }
}
