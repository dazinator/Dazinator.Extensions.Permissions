using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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

        public virtual TAppPermissionType GetOrCreatePermissionType(PermissionTypes permissionType)
        {
            var permissionTypeId = (int)permissionType;
            var set = _dbContext.Set<TAppPermissionType>();
            var existing = set.Find(permissionTypeId);
            if (existing == null)
            {
                var name = Enum.GetName(typeof(PermissionTypes), permissionType);
                existing = CreatePermissionTypeEntity(permissionTypeId, name);
                set.Add(existing);
            }
            return existing;
        }

        protected virtual TAppPermissionType CreatePermissionTypeEntity(int permissionTypeId, string name)
        {
            return new TAppPermissionType() { Id = permissionTypeId, Name = name };
        }

        public virtual TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, PermissionTypes allowedPermissionType)
        {
            var permissionTypeId = (int)allowedPermissionType;
            var existing = appPermissionSubject.Permissions.FirstOrDefault((a) => a.AppPermissionTypeId == permissionTypeId);
            if (existing == null)
            {
                // ensure permission type
                var permissionType = GetOrCreatePermissionType(allowedPermissionType);
                existing = CreatePermissionEntity(appPermissionSubject, permissionType);
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

        public virtual TAppPermissionSubject GetOrCreateAppSubject(TApp app, string name, int enumValue)
        {
            var existing = app.Subjects.FirstOrDefault((a) => (a.Name == name) && (a.Id == enumValue));
            if (existing == null)
            {
                existing = CreateSubjectEntity(enumValue, name);
                app.Subjects.Add(existing);
            }
            return existing;
        }

        protected virtual TAppPermissionSubject CreateSubjectEntity(int id, string name)
        {
            return new TAppPermissionSubject() { Name = name, Id = id };
        }

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

        private TApp CreateAppEntity(string appCode)
        {
            return new TApp() { Code = appCode };
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}