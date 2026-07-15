using RymCloneApi.src.Domain.Entities.Core;

namespace RymCloneApi.src.Domain.Entities
{
  public class Review : Entity
  {
    public int? Id { get; set; }
    public int Score { get; set; }
    public String? ReviewText { get; set; }
    public int AlbumId { get; set; }
    public Album Album { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }

    public Review() : base()
    {
    }
  }
}
