using domain.silisync.Abstractions;

namespace domain.silisync.Entities;

public sealed class ProductAttribute(
    string name,
    string value,
    string? meliAttributeId = null,
    string? meliValueId = null)
    : Entity
{
    public string Name { get; private set; } = name;
    public string Value { get; private set; } = value;

    public string? MeliAttributeId { get; private set; } = meliAttributeId;
    public string? MeliValueId { get; private set; } = meliValueId;

    public int ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
}