namespace Trooper.Testing.CustomShopApi.Business.Support
{
    using System.Collections.Generic;
    using Thorny.Business.Security;
    using Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Security;
    using Interface.Business.Support;
    using ShopModel.Model;

    public class ShopAuthorization : Authorization<Shop>, IShopAuthorization
    {
	    public override IList<IAssignment> Assignments
	    {
			get
			{
				var adminGroup = new List<string> { TestBase.ValidUsername, string.Format("{0}_1", TestBase.ValidUsername) };

				var adminRole = new Role { new Behaviour { Action = Action.AllActions, Allow = true } };
				var deniedRole = new Role { new Behaviour { Action = Action.AllActions, Allow = false } };
				var readerRole = new Role
				{
					new Behaviour {Action = Action.AllActions, Allow = false},
					new Behaviour {Action = Action.GetByKeyAction, Allow = true},
					new Behaviour {Action = Action.GetAllAction, Allow = true}
				};
				var noAddingRole = new Role
				{
					new Behaviour {Action = Action.AllActions, Allow = true},
					new Behaviour {Action = Action.AllAddActions, Allow = false}
				};

				var adminAssignment = new Assignment {Role = adminRole, UserGroups = adminGroup};
				var deniedAssignment = new Assignment {Role = deniedRole, Users = new[] {TestBase.InvalidUsername}};
				var noAdderAssignment = new Assignment {Role = noAddingRole, Users = new[] {"NoAdderUser"}};
				var readerAssignment = new Assignment {Role = readerRole, Users = new[] {"ReaderUser"}};

				return new List<IAssignment> { adminAssignment, noAdderAssignment, deniedAssignment, readerAssignment };
			}
	    }
    }
}
