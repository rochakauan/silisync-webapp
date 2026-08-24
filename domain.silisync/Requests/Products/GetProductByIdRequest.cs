using domain.silisync.Abstractions;

namespace domain.silisync.Requests.Products;

public class GetProductByIdRequest : Request
{
    public Guid Id { get; init; }
}