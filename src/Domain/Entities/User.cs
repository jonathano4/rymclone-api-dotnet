using RymCloneApi.src.Domain.Entities.Core;

namespace RymCloneApi.src.Domain.Entities
{
  public class User : Entity
  {
    public int? Id { get; set; }
    public String Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? Birthday { get; set; }
    public String? Bio { get; set; }
    public ICollection<Review> Reviews { get; set; }

    public User() : base()
    {
      Reviews = [];
    }
  }
}
