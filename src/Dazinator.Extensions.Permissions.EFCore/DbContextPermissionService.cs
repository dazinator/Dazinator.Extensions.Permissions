using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{

    public class DbContextPermissionService<TDbContext, TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> : IPermissionService<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> where TDbContext : DbContext
           where TAppPermissionType : class, IAppPermissionType, new()
          where TAppPermission : class, IAppPermission<TAppPermissionType>, new()
          where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
          where TApp : class, IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()

    {
        private readonly TDbContext _dbContext;

        public DbContextPermissionService(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region PermissionType

        public virtual TAppPermissionType GetOrCreatePermissionType(PermissionTypes permissionType)
        {
            var name = Enum.GetName(typeof(PermissionTypes), permissionType);
            var permissionTypeId = (int)permissionType;
            return GetOrCreatePermissionType(permissionTypeId, name);
        }

        public TAppPermissionType GetOrCreatePermissionType(int id, string name)
        {
            var set = _dbContext.Set<TAppPermissionType>();
            var existing = set.Find(id);
            if (existing == null)
            {
                existing = CreatePermissionTypeEntity(id, name);
                set.Add(existing);
            }
            return existing;
        }

        protected virtual TAppPermissionType CreatePermissionTypeEntity(int permissionTypeId, string name)
        {
            return new TAppPermissionType() { Id = permissionTypeId, Name = name };
        }

        public TAppPermissionType GetPermissionType(PermissionTypes permissionType)
        {
            var id = (int)permissionType;
            return GetPermissionType(id);
        }

        public TAppPermissionType GetPermissionType(int id)
        {
            var set = _dbContext.Set<TAppPermissionType>();
            return set.Find(id);
        }

        #endregion

        #region Permission
        public virtual TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, PermissionTypes allowedPermissionType)
        {
            var permissionType = GetOrCreatePermissionType(allowedPermissionType);
            return GetOrCreatePermission(app, appPermissionSubject, permissionType);
        }

        public TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, TAppPermissionType allowedPermissionType)
        {
            // var permissionTypeId = (int)allowedPermissionType;
            var existing = GetPermission(app, appPermissionSubject, allowedPermissionType);
            if (existing == null)
            {
                // ensure permission type               
                existing = CreatePermissionEntity(appPermissionSubject, allowedPermissionType);
                appPermissionSubject.Permissions.Add(existing);
                // OnPermissionCreated(existing);
            }
            return existing;
        }

        protected virtual TAppPermission CreatePermissionEntity(TAppPermissionSubject subject, TAppPermissionType permissionType)
        {
            return new TAppPermission()
            {
                // AppPermissionSubject = subject,
                AppPermissionType = permissionType
            };
        }

        public TAppPermission GetPermission(TApp app, TAppPermissionSubject appPermissionSubject, TAppPermissionType allowedPermissionType)
        {
            var existing = appPermissionSubject.Permissions.FirstOrDefault((a) => a.AppPermissionType == allowedPermissionType);
            return existing;
        }

        public TAppPermission FindPermission(string appCode, string subjectName, PermissionTypes permissionType)
        {
            return FindPermission(appCode, subjectName, (int)permissionType);
        }

        public TAppPermission FindPermission(string appCode, string subjectName, int permissionTypeId)
        {
            var app = GetApp(appCode);
            if (app == null)
            {
                return default(TAppPermission);
            }

            var subject = GetAppSubject(app, subjectName);
            if (subject == null)
            {
                return default(TAppPermission);
            }

            var permissionType = GetPermissionType(permissionTypeId);
            if (permissionType == null)
            {
                return default(TAppPermission);
            }

            return GetPermission(app, subject, permissionType);
        }

        public IEnumerable<TAppPermission> FindPermissions(string appCode, string subjectName = null, int? permissionTypeId = null)
        {
            var app = GetApp(appCode);
            if (app == null)
            {
                return Enumerable.Empty<TAppPermission>();
            }

            var results = app.Subjects.Where((s) => subjectName == null || s.Name == subjectName)
                .SelectMany(a => a.Permissions.Where(p => permissionTypeId == null || p.AppPermissionTypeId == permissionTypeId));

            return results;            
        }

        #endregion

        #region Subject

        public virtual TAppPermissionSubject GetOrCreateAppSubject(TApp app, string name, int subjectId)
        {
            var existing = app.Subjects.FirstOrDefault((a) => (a.Name == name) && (a.Id == subjectId));
            if (existing == null)
            {
                existing = CreateSubjectEntity(subjectId, name);
                app.Subjects.Add(existing);
            }
            return existing;
        }

        public TAppPermissionSubject GetAppSubject(TApp app, string name)
        {
            var existing = app.Subjects.FirstOrDefault((a) => (a.Name == name));
            return existing;
        }

        protected virtual TAppPermissionSubject CreateSubjectEntity(int id, string name)
        {
            return new TAppPermissionSubject() { Name = name, Id = id };
        }

        #endregion

        #region App
        public virtual TApp GetOrCreateApp(string appCode)
        {
            var appSet = _dbContext.Set<TApp>();
            var existing = appSet
                    //.Include(a => a.Subjects)
                    //  .ThenInclude((a) => a.Permissions)
                    .FirstOrDefault((a) => a.Code == appCode);

            if (existing == null)
            {
                existing = CreateAppEntity(appCode);
                _dbContext.Add(existing);
            }
            return existing;
        }

        public TApp GetApp(string appCode)
        {
            var appSet = _dbContext.Set<TApp>();
            var existing = appSet
                    //.Include(a => a.Subjects)
                    //  .ThenInclude((a) => a.Permissions)
                    .FirstOrDefault((a) => a.Code == appCode);

            return existing;
        }

        private TApp CreateAppEntity(string appCode)
        {
            return new TApp() { Code = appCode };
        }

        #endregion

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

    }
}