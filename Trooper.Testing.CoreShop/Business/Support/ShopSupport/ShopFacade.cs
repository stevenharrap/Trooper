using System.Linq;
using Trooper.Testing.CustomShopApi.Business.Support;

namespace Trooper.Testing.CustomShopApi.Facade.ShopSupport
{
    using Trooper.Thorny.Interface;
    using Trooper.Thorny.Interface.DataManager;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;
    using Trooper.Testing.CustomShopApi.Business.Support.ShopSupport;

    public class ShopFacade : Facade<ShopEnt, Shop>, IShopFacade
    {
		public override System.Collections.Generic.IEnumerable<ShopEnt> GetSome(ISearch search)
		{
			if (search is ShopNameSearch)
			{
				var sns = search as ShopNameSearch;

				return from s in this.GetAll()
					where s.Name == sns.Name
					select s;
			}

			if (search is ShopAddressSearch)
			{
				var sns = search as ShopAddressSearch;

				return from s in this.GetAll()
						   where s.Address == sns.Address
						   select s;
			}

			return base.GetSome(search);
		}
    }
}
