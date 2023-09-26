using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Attributes
{
    public enum ActionMethod
    {
        Allowed,
        Read,
        Add,
        Edit,
        Delete
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ActionRoleAttribute : Attribute
    {
        public ActionMethod ActionMethod { get; }

        public ActionRoleAttribute(ActionMethod method)
        {
            ActionMethod = method;
        }

    }
}
