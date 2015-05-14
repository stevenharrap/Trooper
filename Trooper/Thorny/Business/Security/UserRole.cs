using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
    using System.Collections.Generic;

	public class UserRole : IUserRole
    {
        public string Action { get; set; }

        public IList<string> UserGroups { get; set; }

        public IList<string> Users { get; set; }

        public bool Allow { get; set; }
    }
}
