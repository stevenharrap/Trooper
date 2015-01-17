namespace Trooper.BusinessOperation2.Business.Security
{
    using System.Collections.Generic;
    using Trooper.BusinessOperation2.Interface.Business.Security;

    public class UserRole : IUserRole
    {
        public string Action { get; set; }

        public IList<string> UserGroups { get; set; }

        public IList<string> Users { get; set; }

        public bool Allow { get; set; }
    }
}
