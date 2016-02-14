namespace Trooper.Testing.CustomShop.Api.TestSuit.TestSystem
{
    using Autofac;
    using NUnit.Framework;
    using CustomShopApi;
    using Thorny.Configuration;
    using Interface.Business.Operation;
    using Thorny.Utility;
    using ShopModel.Model;
    using ShopPoco;
    using CustomShop.TestSuit.Common;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class Scenario001
    {
        private IContainer container;

        [Test]
        public void TestScenario()
        {
            this.ClearSystem();
            
            var outletBo = container.Resolve<IOutletBo>();
            var inventoryBo = container.Resolve<IInventoryBo>();
            var productBo = container.Resolve<IProductBo>();

            var outletResponse = outletBo.Add(
                new Outlet { Name = "Dougs Paints", Address = "55 Smith St" },
                TestIdentities.AllowedIdentity1());
            Assert.That(outletResponse.Ok, Is.True);            

            var productResponse = productBo.AddSome(
                new List<Product>
                {
                    new Product { Name = "Glossy Sky", Colour = "Blue" },
                    new Product { Name = "Occur Road", Colour = "Brown" },
                    new Product { Name = "Coffee Feel", Colour = "Black" },
                    new Product { Name = "Honney Bee", Colour = "Orrange" },
                }, 
                TestIdentities.AllowedIdentity1());
            Assert.That(productResponse.Ok, Is.True);

            var inventoryResponse = inventoryBo.AddSome(
                new List<Inventory> {
                    new Inventory
                    {
                        Bin = "B001",
                        OutletIdId = outletResponse.Item.OutletId,
                        ProductId = productResponse.Items.First(i => i.Name == "Glossy Sky").ProductId
                    },
                    new Inventory{
                        Bin = "B002",
                        OutletIdId = outletResponse.Item.OutletId,
                        ProductId = productResponse.Items.First(i => i.Name == "Occur Road").ProductId
                    },
                    new Inventory{
                        Bin = "B003",
                        OutletIdId = outletResponse.Item.OutletId,
                        ProductId = productResponse.Items.First(i => i.Name == "Coffee Feel").ProductId
                    },
                    new Inventory{
                        Bin = "B004",
                        OutletIdId = outletResponse.Item.OutletId,
                        ProductId = productResponse.Items.First(i => i.Name == "Honney Bee").ProductId
                    }
                },
                TestIdentities.AllowedIdentity1());
            Assert.That(inventoryResponse.Ok, Is.True);

        }        

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var parameters = new BusinessModuleStartParameters { AutoStartServices = false };
            this.container = BusinessModule.Start<ShopAppModule>(parameters);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BusinessModule.Stop(this.container);
        }

        private void ClearSystem()
        {
            using (var uow = BusinessUtility.GetUow(this.container))
            using (var ibp = BusinessUtility<InventoryEnt, Inventory>.GetBusinessPack(this.container, uow))
            using (var pbp = BusinessUtility<ProductEnt, Product>.GetBusinessPack(this.container, uow))
            using (var obp = BusinessUtility<OutletEnt, Outlet>.GetBusinessPack(this.container, uow))
            {
                ibp.Facade.DeleteSome(ibp.Facade.GetAll());
                pbp.Facade.DeleteSome(pbp.Facade.GetAll());
                obp.Facade.DeleteSome(obp.Facade.GetAll());

                uow.Save();
            }            
        }
    }    
}