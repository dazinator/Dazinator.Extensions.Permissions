using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Dazinator.Extensions.Permissions.EFCore.EntityConfiguration
{
    public class UserPermissionConfiguration<TUserKey, TUserPermission, TAppPermission, TAppPermissionType> : IEntityTypeConfiguration<TUserPermission>
        where TAppPermissionType : class, IAppPermissionType, new()
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
        where TUserKey : IEquatable<TUserKey>
        where TUserPermission : class, IUserPermission<TUserKey, TAppPermission, TAppPermissionType>

    {
        public virtual void Configure(EntityTypeBuilder<TUserPermission> builder)
        {
            builder.HasKey(a => new { a.UserId, a.PermissionId });
            builder.HasOne(a => a.Permission).WithMany().HasForeignKey(a => a.PermissionId);
        }
    }
}
