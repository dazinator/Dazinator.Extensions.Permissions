namespace Dazinator.Extensions.Permissions.Tests
{
    [AppPermissions("System")]
    public enum SystemPermissions
    {
        [PermissionTypes(PermissionTypes.Create, PermissionTypes.Edit, PermissionTypes.Delete, PermissionTypes.View)]
        Users = 1,
        [PermissionTypes(PermissionTypes.Execute)]
        UserEnable = 2,
        [PermissionTypes(PermissionTypes.Execute)]
        UserDisable = 3,
        [PermissionTypes(PermissionTypes.Create, PermissionTypes.Edit, PermissionTypes.Delete, PermissionTypes.View)]
        Roles = 4,
    }

}
