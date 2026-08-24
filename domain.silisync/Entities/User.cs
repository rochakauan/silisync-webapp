using domain.silisync.Abstractions;

namespace domain.silisync.Entities;

public sealed class User : Entity
{
    public string Name { get; private set; }

    private User(Guid publicId, string name) : base(publicId)
        => Name = name;
    
    public static User Create(Guid publicId, string name)
        => new(publicId, name);
}