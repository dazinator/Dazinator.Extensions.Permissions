using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dazinator.Extensions.Permissions.EFCore.EntityConfiguration
{
    public abstract class DefaultAppPermissionConfiguration<TAppPermission,  TAppPermissionType> : IEntityTypeConfiguration<TAppPermission>
       where TAppPermissionType : class, IAppPermissionType, new()
       where TAppPermission : class, IAppPermission<TAppPermissionType>, new()
    {
        public virtual void Configure(EntityTypeBuilder<TAppPermission> builder)
        {           
            builder.HasIndex(o => new { o.AppPermissionSubjectId, o.AppPermissionTypeId }).IsUnique();
        }
    }
}
