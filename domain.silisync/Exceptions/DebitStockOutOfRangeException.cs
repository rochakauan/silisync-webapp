namespace domain.silisync.Exceptions;

public sealed class DebitStockOutOfRangeException(string message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public DebitStockOutOfRangeException(): this("The product's stock level is outside the permitted limit."){}
}