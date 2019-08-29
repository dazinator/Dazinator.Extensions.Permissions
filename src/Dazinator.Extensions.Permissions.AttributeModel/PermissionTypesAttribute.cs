using System;
using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions.AttributeModel
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class PermissionTypesAttribute : Attribute
    {
        public PermissionTypesAttribute(params PermissionTypes[] types)
        {
            var list = new List<Tuple<int, string>>();
            foreach (var item in types)
            {
                //if (item == Permissions.PermissionTypes.Edit)
                //{
                //    if (EditDependsOnView)
                //    {

                //    }
                //}
                var name = Enum.GetName(typeof(PermissionTypes), item);
                list.Add(new Tuple<int, string>((int)item, name));
            }
            PermissionTypes = list.ToArray();
        }

        public PermissionTypesAttribute(params object[] permissionTypeEnums)
        {
            var list = new List<Tuple<int, string>>();

            foreach (var item in permissionTypeEnums)
            {
                var name = Enum.GetName(item.GetType(), item);
                var id = (int)item;
                list.Add(new Tuple<int, string>(id, name));
            }

            PermissionTypes = list.ToArray();
        }

        public PermissionTypesAttribute(int permissionTypeId, string permissionTypeName)
        {
            PermissionTypes = new Tuple<int, string>[] { new Tuple<int, string>(permissionTypeId, permissionTypeName) };
        }

        public Tuple<int, string>[] PermissionTypes { get; }

        ///// <summary>
        ///// When true (the default), Edit permission will have a dependency on view permission. This implies that
        ///// the user cannot edit if they do not have view permission. When granting edit permission, you are also imlicitly granting]
        ///// view permission.
        ///// </summary>
        //public bool EditDependsOnView { get; set; } = true;

        public object DependsOn { get; set; }
        public int? DependsOnPermissionTypeId { get; set; }

        //public object DependsOnPermissionType { get; set; }

        public object DependsOnPermissionType
        {
            get
            {
                return  (object)DependsOnPermissionTypeId;
            }
            set
            {
                DependsOnPermissionTypeId = (int)value;
            }            
        }

    }

}
