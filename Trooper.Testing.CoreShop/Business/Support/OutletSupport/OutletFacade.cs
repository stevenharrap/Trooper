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
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.OutletSupport;
    using Trooper.Testing.CustomShopApi.Business.Support.OutletSupport;
    using System.Collections.Generic;

    public class OutletFacade : Facade<OutletEnt, Outlet>, IOutletFacade
    {
        public OutletFacade() : base()
        {
            this.ClearSearches();
            this.AddSearch<OutletAddressSearch>();
            this.AddSearch<OutletNameSearch>();
        }

		public override IEnumerable<OutletEnt> GetSome(ISearch search)
		{
			if (search is OutletNameSearch)
			{
				var sns = search as OutletNameSearch;

				return from s in this.GetAll()
					where s.Name == sns.Name
					select s;
			}

			if (search is OutletAddressSearch)
			{
				var sns = search as OutletAddressSearch;

				return from s in this.GetAll()
						   where s.Address == sns.Address
						   select s;
			}

			return base.GetSome(search);
		}
    }
}
