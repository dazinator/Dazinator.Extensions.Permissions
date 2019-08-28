using System;
using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions
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

        public PermissionTypesAttribute(params Tuple<int, string>[] types)
        {
            PermissionTypes = types;
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

        public PermissionTypes DependsOnPermissionType
        {
            get
            {
                return  (PermissionTypes)DependsOnPermissionTypeId;
            }
            set
            {
                DependsOnPermissionTypeId = (int)value;
            }            
        }

    }

}
