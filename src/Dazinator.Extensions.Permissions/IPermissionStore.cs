﻿using Dazinator.Extensions.Permissions.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{
    public interface IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>
          where TAppPermissionType : IAppPermissionType
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
    {
        TApp GetOrCreateApp(string appCode);

        TApp GetApp(string appCode);
        TAppPermissionSubject GetOrCreateAppSubject(TApp app, string name, int subjectId);
        TAppPermissionSubject GetAppSubject(TApp app, string name);
        TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, TAppPermissionType allowedPermissionType);
        TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, PermissionTypes allowedPermissionType);
        TAppPermission GetPermission(TApp app, TAppPermissionSubject appPermissionSubject, TAppPermissionType allowedPermissionType);

        TAppPermission FindPermission(string appCode, string subjectName, PermissionTypes permissionType);
        TAppPermission FindPermission(string appCode, string subjectName, int permissionTypeId);
        IEnumerable<TAppPermission> FindPermissions(string appCode, string subjectName = null, int? permissionTypeId = null);
        TAppPermissionType GetOrCreatePermissionType(PermissionTypes permissionType);
        TAppPermissionType GetOrCreatePermissionType(int id, string name);
        TAppPermissionType GetPermissionType(PermissionTypes permissionType);
        TAppPermissionType GetPermissionType(int permissionTypeId);      

        Task SaveChangesAsync();
    }
}