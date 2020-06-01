using Dazinator.Extensions.Permissions.Entities;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{
    public interface IAppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>
          where TAppPermissionType : IAppPermissionType
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
    {
        Task Seed(IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> permissionService);
    }
}