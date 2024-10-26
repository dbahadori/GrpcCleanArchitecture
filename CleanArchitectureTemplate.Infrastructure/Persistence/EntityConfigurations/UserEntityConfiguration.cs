using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CleanArchitectureTemplate.Infrastructure.Persistence.EntityConfiguration
{
    internal sealed class UserEntityConfiguration : object, IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            #region User
            builder.HasKey(x => x.Id);
            builder.HasIndex(u => u.NationalCode).IsUnique();
            #endregion

        }
    }
}
