namespace Trooper.Testing.CustomShopApi.Business.Support.OutletSupport
{
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.Testing.CustomShopApi.Business.Support.InventorySupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.OutletSupport;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Thorny.Business.Operation.Core;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Utility;
    using System.Linq;
    using Trooper.Testing.CustomShopApi.Business.Model;

    public class OutletBusinessCore : BusinessCore<OutletEnt, Outlet>, IOutletBusinessCore
    {
        public ISaveResponse<ProductInOutlet> SaveProduct(ProductInOutlet productInShop, IIdentity identity)
        {
            using (var shopPack = this.GetBusinessPack())
            {
                var response = new SaveResponse<ProductInOutlet>();
                var productPack = shopPack.ResolveBusinessPack<IProductBusinessCore, ProductEnt, Product>();
                var inventoryPack = shopPack.ResolveBusinessPack<IInventoryBusinessCore, InventoryEnt, Inventory>();

                var getShop = this.GetByKey(shopPack, new OutletEnt { OutletId = productInShop.ShopId }, identity, response);

                if (!getShop.Ok)
                {                    
                    return MessageUtility.Add(getShop, response);
                }

                var savedProduct = productPack.BusinessCore.Save(productPack, productInShop.AsProduct(), identity, getShop);

                if (!savedProduct.Ok)
                {
                    return MessageUtility.Add(savedProduct, response);
                }

                var saveInventory = inventoryPack.BusinessCore.Save(inventoryPack, productInShop.AsInventory(), identity, savedProduct);

                if (saveInventory.Ok)
                {
                    shopPack.Uow.Save(saveInventory);
                }

                return MessageUtility.Add(savedProduct, response);            
            }
        }

        public IManyResponse<ProductInOutlet> GetProducts(Outlet shop, IIdentity identity)
        {
            using (var shopPack = this.GetBusinessPack())
            {
                var inventoryPack = shopPack.ResolveBusinessPack<IInventoryBusinessCore, InventoryEnt, Inventory>();
                var response = new ManyResponse<ProductInOutlet>();

                var getShop = this.GetByKey(shopPack, shop, identity);

                if (!getShop.Ok)
                {
                    return MessageUtility.Add(getShop, response);
                }

                var search = new InventorySearch { ShopId = shop.OutletId };
                var getSome = inventoryPack.BusinessCore.GetSome(inventoryPack, search, identity, false);

                if (!getShop.Ok)
                {
                    return MessageUtility.Add(getSome, response);
                }

                var items = from i in getSome.Items
                            let inventory = i as InventoryEnt
                            select new ProductInOutlet
                            {
                                Bin = i.Bin,
                                Colour = inventory.Product.Colour,
                                Name = inventory.Product.Name,
                                ProductId = i.ProductId,
                                Quantity = i.Quantity,
                                ShopId = i.ShopId
                            };

                response.Items = items.ToList();

                return response;
            }
        }

        public IAddResponse<Outlet> SimpleLittleThing(IIdentity identity)
        {
            return new AddResponse<Outlet> { Item = new Outlet { Name = "Coles", Address = "25 Brown St" } };
        }       
    }
}
