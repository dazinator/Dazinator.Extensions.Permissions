using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dazinator.Extensions.Permissions.EFCore.EntityConfiguration
{
    public class AppPermissionConfiguration<TAppPermission, TAppPermissionType> : IEntityTypeConfiguration<TAppPermission>
       where TAppPermissionType : class, IAppPermissionType, new()
       where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
    {
        public virtual void Configure(EntityTypeBuilder<TAppPermission> builder)
        {
            builder.HasIndex(o => new { o.AppId, o.AppPermissionSubjectId, o.AppPermissionTypeId }).IsUnique();

            builder.HasMany(e => e.Children)
                   .WithOne(e => e.Parent) //Each comment from Replies points back to its parent
                   .HasForeignKey(e => e.ParentId);

        }
    }
}
