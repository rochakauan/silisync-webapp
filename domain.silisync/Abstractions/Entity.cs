namespace domain.silisync.Abstractions;

public abstract class Entity
{
    public int PrivateId { get; private set; }
    public Guid PublicId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public bool IsDeleted { get; set; }

    protected Entity()
    {
        PublicId = Guid.CreateVersion7();
        CreatedAt = DateTimeOffset.UtcNow;
    }

    protected Entity(Guid publicId)
    {
        PublicId = publicId;
        CreatedAt = DateTimeOffset.UtcNow;
    }
    
    public void MarkAsDeleted() => IsDeleted = true;
}