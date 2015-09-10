using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Thorny.Business.Security;
using Trooper.Thorny.Business.TestSuit;

namespace Trooper.Testing.CustomShopApiTestSuit.TestOutlet
{
    public class TestAddingOutletHelper : TestSuitHelper<Outlet>
    {
        public override Outlet MakeValidItem()
        {
            return new Outlet
            {
                Address = "42 Trooper St",
                Name = "TopCop"
            };
        }

        public override Outlet MakeInvalidItem()
        {
            return new Outlet
            {
                Address = "42 Verylongstreetnamewhichisfartoolongtofitinanyreasonablestreetaddress St",
                Name = "TopCop"
            };
        }

        public override IIdentity MakeValidIdentity()
        {
            return new Identity 
            {
                Username = "ValidTestUser"
            };
        }

        public override IIdentity MakeInvalidIdentity()
        {
            return new Identity
            {
                Username = "InvalidTestUser"
            };
        }

        public override bool IdentifierAreEqual(Outlet itemA, Outlet itemB)
        {
            Assert.IsNotNull(itemA);
            Assert.IsNotNull(itemB);

            return itemA.OutletId == itemB.OutletId;
        }

        public override bool NonIdentifersAreEqual(Outlet itemA, Outlet itemB)
        {
            Assert.IsNotNull(itemA);
            Assert.IsNotNull(itemB);

            return itemA.Name == itemB.Name && itemA.Address == itemB.Address;
        }

        public override void ChangeNonIdentifiers(Outlet item)
        {
            Assert.IsNotNull(item);

            item.Address = item.Address + "1";
            item.Name = item.Name + "1";
        }

        public override Outlet CopyItem(Outlet item)
        {
            Assert.IsNotNull(item);

            return new Outlet
            {
                Address = item.Address,
                Name = item.Name,
                OutletId = item.OutletId
            };
        }
    }
}
