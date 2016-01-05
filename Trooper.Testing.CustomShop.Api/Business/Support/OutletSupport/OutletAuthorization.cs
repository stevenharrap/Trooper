namespace Trooper.Testing.CustomShop.Api.Business.Support.OutletSupport
{
    using System.Collections.Generic;
    using Thorny.Business.Security;
    using Trooper.Interface.Thorny.Business.Security;
    using ShopPoco;
    using Interface.Business.Support.OutletSupport;

    public class OutletAuthorization : Authorization<Outlet>, IOutletAuthorization
    {
        public const string ValidUsername = "ValidTestUser";

        public const string InvalidUsername = "InvalidTestUser";

        public static IList<IAssignment> GeneralAssigments
        {
            get
            {
                var adminGroup = new List<string> { ValidUsername, string.Format("{0}_1", ValidUsername) };

                var adminRole = new Role { new Behaviour { Action = OperationAction.AllActions, Allow = true } };
                var deniedRole = new Role { new Behaviour { Action = OperationAction.AllActions, Allow = false } };
                var readerRole = new Role
				{
					new Behaviour {Action = OperationAction.AllActions, Allow = false},
					new Behaviour {Action = OperationAction.GetByKeyAction, Allow = true},
					new Behaviour {Action = OperationAction.GetAllAction, Allow = true}
				};
                var noAddingRole = new Role
				{
					new Behaviour {Action = OperationAction.AllActions, Allow = true},
					new Behaviour {Action = OperationAction.AllAddActions, Allow = false}
				};

                var adminAssignment = new Assignment { Role = adminRole, UserGroups = adminGroup };
                var deniedAssignment = new Assignment { Role = deniedRole, Users = new[] { InvalidUsername } };
                var noAdderAssignment = new Assignment { Role = noAddingRole, Users = new[] { "NoAdderUser" } };
                var readerAssignment = new Assignment { Role = readerRole, Users = new[] { "ReaderUser" } };

                return new List<IAssignment> { adminAssignment, noAdderAssignment, deniedAssignment, readerAssignment };
            }
        }
    

	    public override IList<IAssignment> Assignments
	    {
			get
			{
                return GeneralAssigments;
			}
	    }
    }
}
