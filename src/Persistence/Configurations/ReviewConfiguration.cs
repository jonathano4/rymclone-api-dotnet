using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RymCloneApi.src.Domain.Entities;

namespace RymCloneApi.src.Persistence.Configurations
{
  public class ReviewConfiguration : IEntityTypeConfiguration<Review>
  {
    public void Configure(EntityTypeBuilder<Review> builder)
    {
      builder.ToTable("review");
      builder.Property(r => r.Id).HasColumnName("id");
      builder.Property(r => r.Score).HasColumnName("score").IsRequired();
      builder.Property(r => r.ReviewText).HasColumnName("review_text").HasColumnType("text");
      builder.Property(r => r.UserId).HasColumnName("user_id").IsRequired();
      builder.Property(r => r.AlbumId).HasColumnName("album_id").IsRequired();
      builder.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
      builder.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
      builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
      builder.HasOne(r => r.Album).WithMany().HasForeignKey(r => r.AlbumId).OnDelete(DeleteBehavior.Cascade);

      builder.HasIndex(r => new { r.UserId, r.AlbumId }).IsUnique();
    }
  }
}
