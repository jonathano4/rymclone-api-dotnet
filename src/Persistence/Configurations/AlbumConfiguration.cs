using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RymCloneApi.src.Domain.Entities;

namespace RymCloneApi.src.Persistence.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
  public void Configure(EntityTypeBuilder<Album> builder)
  {
    builder.ToTable("albums");
    builder.Property(e => e.Id).HasColumnName("id");
    builder
      .Property(e => e.Title)
      .HasColumnName("title")
      .HasColumnType("varchar")
      .HasMaxLength(250)
      .IsRequired();
    builder
      .Property(e => e.ReleaseDate)
      .HasColumnName("release_date")
      .HasColumnType("date")
      .IsRequired();
    builder.Property(e => e.ArtistId).HasColumnName("artist_id");
    builder.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
    builder.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
    builder.HasOne(e => e.Artist).WithMany(e => e.Albums).HasForeignKey(e => e.ArtistId).OnDelete(DeleteBehavior.Restrict);
  }
}
