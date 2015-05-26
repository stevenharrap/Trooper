using System;

namespace Trooper.Testing.CustomShopApi.Business.Support
{
    using System.Collections.Generic;
    using Trooper.Thorny.Business.Security;
    using Trooper.Thorny.Interface.OperationResponse;
    using Trooper.Thorny.OperationResponse;
    using Trooper.Thorny.UnitTestBase;
    using Trooper.Thorny.Utility;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Model;

    public class ShopAuthorization : Authorization<Shop>, IShopAuthorization
    {
		private readonly IList<IAssignment> roles = new IAssignment[]
		{
			new Assignment {Action = Action.AllActions, Allow = true, Users = new[] {TestBase.ValidUsername}},
			new Assignment {Action = Action.AllActions, Allow = false, Users = new[] {TestBase.InvalidUsername}},
			new Assignment {Action = Action.GetByKeyAction, Allow = true, Users = new[] {"ReaderUser"}}
		};


		public override IList<IAssignment> Roles
		{
			get { return roles; }
			set { throw new NotImplementedException(); }
		}

		//public override bool IsAllowed(IRequestArg<Shop> arg, ICredential credential, IResponse response)
		//{
		//	if (credential.Username == TestBase.InvalidUsername)
		//	{
		//		MessageUtility.Errors.Add(string.Format("'{0}' is not allowed", TestBase.InvalidUsername), UserDeniedCode, response);

		//		return false;
		//	}

		//	return base.IsAllowed(arg, credential, response);
		//}
    }
}
