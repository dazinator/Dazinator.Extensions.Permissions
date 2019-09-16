using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Dazinator.Extensions.Permissions.EFCore.EntityConfiguration
{
    public class RolePermissionConfiguration<TRoleKey, TRolePermission, TAppPermission, TAppPermissionType> : IEntityTypeConfiguration<TRolePermission>
        where TAppPermissionType : class, IAppPermissionType, new()
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
        where TRoleKey : IEquatable<TRoleKey>
        where TRolePermission : class, IRolePermission<TRoleKey, TAppPermission, TAppPermissionType>

    {
        public virtual void Configure(EntityTypeBuilder<TRolePermission> builder)
        {
            builder.HasKey(a => new { a.RoleId, a.PermissionId });
            builder.HasOne(a => a.Permission).WithMany().HasForeignKey(a => a.PermissionId);
        }
    }
}
