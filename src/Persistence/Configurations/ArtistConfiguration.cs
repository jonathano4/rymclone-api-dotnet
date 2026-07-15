using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RymCloneApi.src.Domain.Entities;

namespace RymCloneApi.src.Persistence.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
  public void Configure(EntityTypeBuilder<Artist> builder)
  {
    builder.ToTable("artists");
    builder.Property(e => e.Id).HasColumnName("id");
    builder
      .Property(s => s.Name)
      .HasColumnName("name")
      .HasMaxLength(250)
      .HasColumnType("varchar")
      .IsRequired();
    builder.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
    builder.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
    //builder.HasMany<Album>(e => e.Albums).WithOne().HasForeignKey(al => al.ArtistId).OnDelete(DeleteBehavior.Restrict);
  }
}
