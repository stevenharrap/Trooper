namespace Trooper.Testing.CustomShop.TestSuit.Common
{
    using NUnit.Framework;
    using Interface.Thorny.Business.Operation.Single;
    using Interface.Thorny.Business.Security;
    using ShopPoco;
    using Thorny.Business.Security;
    using Thorny.Business.TestSuit;
    using System;

    public class TestAddingOutletHelper : TestSuitHelper<Outlet>
    {
        public TestAddingOutletHelper(IBusinessCreate<Outlet> boCreater, IBusinessRead<Outlet> boReader, IBusinessDelete<Outlet> boDeleter) 
            : base(boCreater, boReader, boDeleter) { }

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
               

        public override IIdentity MakeInvalidIdentity()
        {
            return new Identity
            {
            };
        }

        public override bool IdentifiersAreEqual(Outlet itemA, Outlet itemB)
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

        public override void CopyIdentifiers(Outlet source, Outlet destination)
        {
            Assert.IsNotNull(source);
            Assert.IsNotNull(destination);

            destination.OutletId = source.OutletId;
        }

        public override void CopyNonIdentifiers(Outlet source, Outlet destination)
        {
            Assert.IsNotNull(source);
            Assert.IsNotNull(destination);

            destination.Address = source.Address;
            destination.Name = source.Name;
        }

        public override IIdentity MakeAllowedIdentity()
        {
            return new Identity
            {
                Username = "ValidTestUser",
                Password = "1234"
            };
        }

        public override IIdentity MakeDeniedIdentity()
        {
            return new Identity
            {
                Username = "InvalidTestUser",
                Password = "6543"
            };
        }
    }
}
