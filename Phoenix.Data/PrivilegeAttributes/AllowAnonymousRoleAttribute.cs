using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAnonymousRoleAttribute : Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute
    {
    }
}
