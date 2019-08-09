using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dazinator.Extensions.Permissions.EFCore.EntityConfiguration
{
    public abstract class DefaultAppConfiguration<TApp, TAppPermissionType, TAppPermission, TAppPermissionSubject> : IEntityTypeConfiguration<TApp>
        where TAppPermissionType : class, IAppPermissionType, new()
        where TAppPermission : class, IAppPermission<TAppPermissionType>, new()
        where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
        where TApp : class, IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()
    {
        public virtual void Configure(EntityTypeBuilder<TApp> builder)
        {
            builder.HasIndex(u => u.Code).IsUnique();
            builder.HasMany<TAppPermissionSubject>(a => a.Subjects).WithOne().HasForeignKey((a) => a.AppId).IsRequired();
            builder.HasMany<TAppPermission>(a => a.Permissions).WithOne().HasForeignKey(ur => ur.AppId).IsRequired().OnDelete(DeleteBehavior.Restrict); //noaction
        }
    }
}
