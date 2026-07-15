using RymCloneApi.src.Domain.Entities.Core;
using System.ComponentModel.DataAnnotations;

namespace RymCloneApi.src.Domain.Entities;

public class Genre : Entity
{
  public Genre() : base()
  {
    Albums = [];
  }

  public int? Id { get; set; }
  public string? Name { get; set; }
  public ICollection<Album> Albums { get; set; }
}
