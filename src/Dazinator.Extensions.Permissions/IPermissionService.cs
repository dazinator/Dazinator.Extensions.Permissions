using Dazinator.Extensions.Permissions.Entities;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{
    public interface IPermissionService<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>
          where TAppPermissionType : IAppPermissionType
          where TAppPermission : IAppPermission<TAppPermissionType>
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
    {
        TApp GetOrCreateApp(string appCode);
        TAppPermissionSubject GetOrCreateAppSubject(TApp app, string name, int subjectId);
        TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, PermissionTypes allowedPermissionType);
        TAppPermissionType GetOrCreatePermissionType(PermissionTypes permissionType);

        Task SaveChangesAsync();
    }
}