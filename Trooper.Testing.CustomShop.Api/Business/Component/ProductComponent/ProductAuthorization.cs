namespace Trooper.Testing.CustomShop.Api.Business.Support.ProductComponent
{
    using System.Collections.Generic;
    using Thorny.Business.Security;
    using Trooper.Interface.Thorny.Business.Security;
    using ShopPoco;
    using Interface.Business.Support.ProductComponent;
    using OutletComponent;

    public class ProductAuthorization : Authorization<Product>, IProductAuthorization
    {
	    public override IList<IAssignment> Assignments
	    {
			get
			{
                return OutletAuthorization.GeneralAssigments;
			}
	    }
    }
}
