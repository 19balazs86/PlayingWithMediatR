namespace PlayingWithMediatR.Exceptions;

public sealed class DeleteProductException : CustomExceptionBase
{
    public DeleteProductException() : base() { }

    public DeleteProductException(string message) : base(message) { }

    public DeleteProductException(string message, Exception exception) : base(message, exception) { }
}
