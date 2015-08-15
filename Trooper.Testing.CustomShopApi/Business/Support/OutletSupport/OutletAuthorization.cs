namespace Trooper.Testing.CustomShopApi.Business.Support.OutletSupport
{
    using System.Collections.Generic;
    using Thorny.Business.Security;
    using Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Security;
    using Interface.Business.Support;
    using ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.OutletSupport;

    public class OutletAuthorization : Authorization<OutletEnt>, IOutletAuthorization
    {
        public static IList<IAssignment> GeneralAssigments
        {
            get
            {
                var adminGroup = new List<string> { TestBase.ValidUsername, string.Format("{0}_1", TestBase.ValidUsername) };

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
                var deniedAssignment = new Assignment { Role = deniedRole, Users = new[] { TestBase.InvalidUsername } };
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
