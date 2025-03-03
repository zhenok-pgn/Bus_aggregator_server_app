using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string HashedPassword { get; set; }
        public required string Role { get; set; }
        public bool IsBan { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.UserName)
                .IsRequired();
            builder.Property(p => p.HashedPassword)
                .IsRequired();
        }
    }
}
