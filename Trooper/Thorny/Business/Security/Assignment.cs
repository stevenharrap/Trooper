using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
    using System.Collections.Generic;

	public class Assignment : IAssignment
    {
        public IRole Role { get; set; }

        public IList<string> UserGroups { get; set; }

        public IList<string> Users { get; set; }
    }
}
