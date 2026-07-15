using RymCloneApi.src.Domain.Entities.Core;
using System;
using System.ComponentModel.DataAnnotations;

namespace RymCloneApi.src.Domain.Entities
{
  public class Artist : Entity
  {
    public Artist() : base()
    {
      Albums = [];
    }

    [Key]
    public int? Id { get; set; }
    [Required]
    public string? Name { get; set; }
    public ICollection<Album> Albums { get; set; }
  }
}