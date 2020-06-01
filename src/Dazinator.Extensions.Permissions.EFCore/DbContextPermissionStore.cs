using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{

    public class DbContextPermissionStore<TDbContext, TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> : IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> where TDbContext : DbContext
           where TAppPermissionType : class, IAppPermissionType, new()
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
          where TApp : class, IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()

    {
        private readonly TDbContext _dbContext;

        public DbContextPermissionStore(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region PermissionType     

        /// <summary>
        /// Gets the permission type with the specified id if it exists. If it doesn't exist, creates it with the specified id and name.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name">name only used if permission type is created.</param>
        /// <returns></returns>
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
              
        public TAppPermissionType GetPermissionType(int id)
        {
            var set = _dbContext.Set<TAppPermissionType>();
            return set.Find(id);
        }

        #endregion

        #region Permission
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

        /// <summary>
        /// Gets the existing subject for the app, with the specified subject id. Creates the subject if not found.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="name">Only used if subject is created.</param>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public virtual TAppPermissionSubject GetOrCreateAppSubject(TApp app, string name, int subjectId)
        {
            var existing = app.Subjects.FirstOrDefault(a => a.Id == subjectId);
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
        public TApp GetApp(int id)
        {
            var appSet = _dbContext.Set<TApp>();
            return appSet.Single(a => a.Id == id);
        }

        public virtual TApp GetOrCreateApp(string appCode)
        {
            
            var appSet = _dbContext.Set<TApp>();

            var existingLocal = appSet.Local.FirstOrDefault((a) => a.Code == appCode);
            if(existingLocal != null)
            {
                return existingLocal;
            }
            var existing = appSet
                    .Include(a => a.Subjects)
                      .ThenInclude((a) => a.Permissions)
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

            var existingLocal = appSet.Local.FirstOrDefault((a) => a.Code == appCode);
            if (existingLocal != null)
            {
                return existingLocal;
            }

            var existing = appSet
                    .Include(a => a.Subjects)
                      .ThenInclude((a) => a.Permissions)
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