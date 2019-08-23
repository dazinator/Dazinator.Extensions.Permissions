using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dazinator.Extensions.Permissions.EFCore.EntityConfiguration
{
    public class AppPermissionSubjectConfiguration<TAppPermissionSubject, TAppPermissionType, TAppPermission> : IEntityTypeConfiguration<TAppPermissionSubject>
        where TAppPermissionType : class, IAppPermissionType, new()
        where TAppPermission : class, IAppPermission<TAppPermissionType>, new()
        where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()

    {
        public virtual void Configure(EntityTypeBuilder<TAppPermissionSubject> builder)
        {
            builder.HasKey(o => new { o.Id, o.AppId });
            builder.HasMany<TAppPermission>(a => a.Permissions).WithOne().HasForeignKey(ur => new { ur.AppPermissionSubjectId, ur.AppId }).IsRequired(); //multiple cascade paths - permissions are cascade deleted by deleting the app already (fk on app id)
        }
    }
}
