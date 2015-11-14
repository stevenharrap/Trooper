namespace Trooper.Testing.CustomShop.TestSuit.Common
{
    using NUnit.Framework;
    using Interface.Thorny.Business.Operation.Single;
    using Interface.Thorny.Business.Security;
    using ShopPoco;
    using Thorny.Business.Security;
    using Thorny.Business.TestSuit;
    using System;
    using System.Collections.Generic;

    public class TestAddingOutletHelper : TestSuitHelper<Outlet>
    {
        public TestAddingOutletHelper(IBusinessCreate<Outlet> boCreater, IBusinessRead<Outlet> boReader, IBusinessDelete<Outlet> boDeleter) 
            : base(boCreater, boReader, boDeleter) { }

        public override IEnumerable<Outlet> MakeValidItems()
        {
            return new List<Outlet>
            {
                new Outlet
                {
                    Address = "42 Trooper St",
                    Name = "TopCop"
                },
                new Outlet
                {
                    Address = "38 Bouncer Rd",
                    Name = "DogHouse"
                }
            };
        }

        public override IEnumerable<Outlet> MakeInvalidItems()
        {
            return new List<Outlet>
            {
                new Outlet
                {
                    Address = "42 Verylongstreetnamewhichisfartoolongtofitinanyreasonablestreetaddress St",
                    Name = "TopCop"
                },
                null
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

        public override IEnumerable<IIdentity> MakeAllowedIdentities()
        {
            return new List<IIdentity>
            {
                new Identity
                {
                    Username = "ValidTestUser",
                    Password = "1234"
                }
            };  
        }

        public override IEnumerable<IIdentity> MakeDeniedIdentities()
        {
            return new List<IIdentity>
            {
                new Identity
                {
                    Username = "InvalidTestUser",
                    Password = "6543"
                }
            };
        }

        public override IIdentity GetAdminIdentity()
        {
            return new Identity
            {
                Username = "ValidTestUser",
                Password = "1234"
            };
        }
    }
}
