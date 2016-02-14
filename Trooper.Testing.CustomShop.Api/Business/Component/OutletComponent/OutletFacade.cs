namespace Trooper.Testing.CustomShop.Api.Facade.ShopSupport
{
    using System.Linq;
    using Thorny.Interface;
    using Thorny.Interface.DataManager;
    using ShopPoco;
    using ShopModel.Model;
    using System.Collections.Generic;
    using Interface.Business.Support.OutletComponent;
    using Business.Support.OutletComponent;

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
