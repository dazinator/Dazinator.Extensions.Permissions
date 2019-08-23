namespace Dazinator.Extensions.Permissions.Tests
{
    [AppPermissions("System")]
    public enum SystemPermissions
    {
        [ApplicablePermissionTypes(PermissionTypes.Create, PermissionTypes.Edit, PermissionTypes.Delete, PermissionTypes.View)]
        Users = 1,
        [ApplicablePermissionTypes(PermissionTypes.Execute)]
        UserEnable = 2,
        [ApplicablePermissionTypes(PermissionTypes.Execute)]
        UserDisable = 3,
        [ApplicablePermissionTypes(PermissionTypes.Create, PermissionTypes.Edit, PermissionTypes.Delete, PermissionTypes.View)]
        Roles = 4,
    }

}
