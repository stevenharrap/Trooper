using System.Linq;
using Trooper.Testing.CoreShop.Business.Support;

namespace Trooper.Testing.CoreShop.Facade
{
    using Trooper.BusinessOperation2.Interface;
    using Trooper.BusinessOperation2.Interface.DataManager;
    using Trooper.Testing.CoreShop.Interface.Business.Support;
    using Trooper.Testing.CoreShop.Interface.Model;
    using Trooper.Testing.CoreShop.Model;


    public class ShopFacade : Facade<Shop, IShop>, IShopFacade
    {
		public override System.Collections.Generic.IEnumerable<Shop> GetSome(ISearch search)
		{
			if (search is ShopNameSearch)
			{
				var sns = search as ShopNameSearch;

				var data = from s in this.GetAll()
					where s.Name == sns.Name
					select s;

				return this.Limit(data, sns);
			}

			if (search is ShopAddressSearch)
			{
				var sns = search as ShopAddressSearch;

				var data = from s in this.GetAll()
						   where s.Address == sns.Address
						   select s;

				return this.Limit(data, sns);
			}

			return base.GetSome(search);
		}
    }
}
