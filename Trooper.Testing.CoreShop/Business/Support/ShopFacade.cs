using System.Linq;
using Trooper.Testing.CustomShopApi.Business.Support;

namespace Trooper.Testing.CustomShopApi.Facade
{
    using Trooper.BusinessOperation2.Interface;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;

    public class ShopFacade : Facade<Shop, IShop>, IShopFacade
    {
		public override System.Collections.Generic.IEnumerable<Shop> GetSome(ISearch search)
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
