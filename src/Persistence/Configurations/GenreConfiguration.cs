using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RymCloneApi.src.Domain.Entities;

namespace RymCloneApi.src.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
  public void Configure(EntityTypeBuilder<Genre> builder)
  {
    builder.ToTable("genres");
    builder.Property(e => e.Id).HasColumnName("id");
    builder
      .Property(s => s.Name)
      .HasColumnName("name")
      .HasMaxLength(100)
      .HasColumnType("varchar")
      .IsRequired();
    builder.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
    builder.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
    builder.HasMany<Album>(e => e.Albums).WithMany(e => e.Genres).UsingEntity(
      "AlbumGenre",
      rightTable => rightTable.HasOne(typeof(Album)).WithMany().HasForeignKey("AlbumId"),
      leftTable => leftTable.HasOne(typeof(Genre)).WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.Restrict),
      joinTable =>
      {
        joinTable.ToTable("albums_genres");
        joinTable.Property<int>("AlbumId").HasColumnName("album_id");
        joinTable.Property<int>("GenreId").HasColumnName("genre_id");
      }
    );
  }
}
