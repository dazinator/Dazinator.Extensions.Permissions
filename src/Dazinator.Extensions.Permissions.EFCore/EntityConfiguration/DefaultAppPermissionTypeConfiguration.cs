using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dazinator.Extensions.Permissions.EFCore.EntityConfiguration
{
    public class DefaultAppPermissionTypeConfiguration<TAppPermissionType> : IEntityTypeConfiguration<TAppPermissionType>
        where TAppPermissionType : class, IAppPermissionType, new()

    {
        public virtual void Configure(EntityTypeBuilder<TAppPermissionType> builder)
        {

            builder.HasKey(rc => rc.Id);
            // b.HasMany<LicencedAppPermission>(a => a.).WithOne().HasForeignKey(ur => ur.AppPermissionTypeId); //.IsRequired();
        }
    }
}
