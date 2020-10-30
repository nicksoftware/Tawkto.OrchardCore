using OrchardCore.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tawkto.OrchardCore
{
    public  class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageTawkto = new Permission("Tawk to", "Manage Tawk to");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageTawkto }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageTawkto }
                }
            };
        }
    }
}
