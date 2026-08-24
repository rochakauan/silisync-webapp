using domain.silisync.Abstractions;

namespace domain.silisync.Entities;

public sealed class Category : Entity
{
   public string Name { get; init; } = string.Empty;
   public string? ParentCategoryId { get; init; }
   public string? MeliCategoryId { get; init; }

}