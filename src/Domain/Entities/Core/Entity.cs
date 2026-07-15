namespace RymCloneApi.src.Domain.Entities.Core
{
  public class Entity
  {
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Entity ()
    {
      CreatedAt = DateTimeOffset.UtcNow;
      UpdatedAt = DateTimeOffset.UtcNow;
    }
  }
}
