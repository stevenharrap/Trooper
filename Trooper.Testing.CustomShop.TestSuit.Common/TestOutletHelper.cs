namespace Trooper.Testing.CustomShop.TestSuit.Common
{
    using NUnit.Framework;
    using Interface.Thorny.Business.Operation.Single;
    using Interface.Thorny.Business.Security;
    using ShopPoco;
    using Thorny.Business.Security;
    using Thorny.Business.TestSuit;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class TestOutletHelper : TestSuitHelper<Outlet>
    {        
        public TestOutletHelper(IBusinessCreate<Outlet> boCreater, IBusinessRead<Outlet> boReader, IBusinessDelete<Outlet> boDeleter) 
            : base(boCreater, boReader, boDeleter) { }

        public override int DefaultRequiredInvalidItems
        {
            get
            {
                return 3;
            }
        }        

        public override void ChangeNonIdentifiers(Outlet item, IEnumerable<Outlet> otherItems)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (item == null) throw new ArgumentNullException(nameof(otherItems));

            item.Address = this.ChangeProperty(item.Address, otherItems.Select(i => i.Address), "{0} Thingo St");
            item.Name = this.ChangeProperty(item.Name, otherItems.Select(i => i.Name), "Stuff{0}");
        }

        public override void CopyIdentifiers(Outlet source, Outlet destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            destination.OutletId = source.OutletId;
        }

        public override void CopyNonIdentifiers(Outlet source, Outlet destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            destination.Address = source.Address;
            destination.Name = source.Name;
        }

        public override IIdentity GetAdminIdentity()
        {
            return new Identity
            {
                Username = "ValidTestUser",
                Password = "1234"
            };
        }

        public override bool IdentifiersAreEqual(Outlet itemA, Outlet itemB)
        {
            if (itemA == null) throw new ArgumentNullException(nameof(itemA));
            if (itemB == null) throw new ArgumentNullException(nameof(itemB));

            return itemA.OutletId == itemB.OutletId;
        }

        public override IEnumerable<IIdentity> MakeAllowedIdentities()
        {
            yield return TestIdentities.AllowedIdentity1();
        }

        public override IEnumerable<IIdentity> MakeDeniedIdentities()
        {
            yield return TestIdentities.DeniedIdentity1();
        }

        public override void MakeInvalidItems(List<Outlet> items, IEnumerable<Outlet> otherItems)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (otherItems == null) throw new ArgumentNullException(nameof(otherItems));

            const string badAddressFormat = "{0} Verylongstreetnamewhichisfartoolongtofitinanyreasonablestreetaddress St";
            const string badNameFormat = "Topcop-Topcop-Topcop-Topcop-Topcop-Topcop-Topcop-Topcop-{0}";

            for (var c = 0; c < items.Count(); c++)
            {
                var item = items[c];

                switch (c)
                {
                    case 0:
                        item.Address = this.ChangeProperty(item.Address, otherItems.Select(i => i.Address), badAddressFormat);
                        break;
                    case 1:
                        item.Name = this.ChangeProperty(item.Name, otherItems.Select(i => i.Name), badNameFormat);
                        break;
                    case 2:
                        item.Name = this.ChangeProperty(item.Name, otherItems.Select(i => i.Name), badNameFormat);
                        item.Address = this.ChangeProperty(item.Address, otherItems.Select(i => i.Address), badAddressFormat);
                        break;
                }
            }
        }

        public override IEnumerable<Outlet> MakeValidItems(int? required, Keys keyBehavious, IEnumerable<Outlet> otherItems)
        {
            var newOutletIds = new List<int?>();

            for (var i = 0; i < (required == null ? 3 : (int)required); i++)
            {
                var outletId = this.MakeKeyValue(keyBehavious, true, otherItems.Select(o => o?.OutletId).Concat(newOutletIds));
                newOutletIds.Add(outletId);

                switch (i)
                {
                    case 0:
                    default:
                        yield return new Outlet
                        {
                            OutletId = outletId,
                            Address = this.ChangeProperty(otherItems.Select(o => o?.Address), "{0} Trooper St"),
                            Name = this.ChangeProperty(otherItems.Select(o => o?.Name), "TopCop-{0}")
                        };
                        break;
                    case 1:
                        yield return new Outlet
                        {
                            OutletId = outletId,
                            Address = this.ChangeProperty(otherItems.Select(o => o?.Address), "{0} Trooper St"),
                            Name = null
                        };
                        break;
                    case 2:
                        yield return new Outlet
                        {
                            OutletId = outletId,
                            Address = null,
                            Name = this.ChangeProperty(otherItems.Select(o => o?.Name), "TopCop-{0}")
                        };
                        break;
                }
            }
        }

        public override bool NonIdentifiersAreEqual(Outlet itemA, Outlet itemB)
        {
            if (itemA == null) throw new ArgumentNullException(nameof(itemA));
            if (itemB == null) throw new ArgumentNullException(nameof(itemB));

            return itemA.Name == itemB.Name && itemA.Address == itemB.Address;
        }        
    }
}
