namespace Trooper.Testing.CustomShopApi.Business.Support
{
    using System.Collections.Generic;
    using Trooper.BusinessOperation2.Business.Security;
    using Trooper.BusinessOperation2.Interface.OperationResponse;
    using Trooper.BusinessOperation2.OperationResponse;
    using Trooper.BusinessOperation2.UnitTestBase;
    using Trooper.BusinessOperation2.Utility;
    using Trooper.Interface.BusinessOperation2.Business.Security;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Model;

    public class ShopAuthorization : Authorization<Shop>, IShopAuthorization
    {
        public override bool IsAllowed(IRequestArg<Shop> arg, ICredential credential, IResponse response)
        {
            if (credential.Username == TestBase.InvalidUsername)
            {
                MessageUtility.Notes.Add(string.Format("'{0}' is not allowed", TestBase.InvalidUsername), response);

                return false;
            }

            return base.IsAllowed(arg, credential, response);
        }
    }
}
