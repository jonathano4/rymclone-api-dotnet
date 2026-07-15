using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RymCloneApi.src.Domain.Entities;

namespace RymCloneApi.src.Persistence.Configurations
{
  public class UserConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> builder)
    {
      builder.ToTable("user");
      builder.Property(u => u.Id).HasColumnName("id");
      builder.Property(u => u.Username).HasColumnName("username").IsRequired().HasMaxLength(50);
      builder.Property(u => u.FirstName).HasColumnName("first_name").HasColumnType("varchar");
      builder.Property(u => u.LastName).HasColumnName("last_name").HasColumnType("varchar");
      builder.Property(u => u.Birthday).HasColumnName("birthday");
      builder.Property(u => u.Bio).HasColumnName("bio").HasColumnType("text");
      builder.Property(e => e.CreatedAt).IsRequired().HasColumnName("created_at");
      builder.Property(e => e.UpdatedAt).IsRequired().HasColumnName("updated_at");
      builder.HasIndex(u => u.Username).IsUnique();
    }
  }
}
