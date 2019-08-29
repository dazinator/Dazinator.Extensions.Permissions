using Dazinator.Extensions.Permissions.AttributeModel;

namespace Dazinator.Extensions.Permissions.Tests
{
    [AppPermissions("AddOn")]
    public enum AddOnPermissions
    {
        [PermissionTypes(PermissionTypes.Create,
                         PermissionTypes.Edit,
                         PermissionTypes.Delete,
                         PermissionTypes.View,
                         DependsOn = SystemPermissions.Users,
                         DependsOnPermissionType = PermissionTypes.View)]
        UserReports = 1,

        [PermissionTypes(PermissionTypes.View)]
        [PermissionTypes(PermissionTypes.Edit, DependsOnPermissionType = PermissionTypes.View)]
        OtherThing = 2
    }

}
