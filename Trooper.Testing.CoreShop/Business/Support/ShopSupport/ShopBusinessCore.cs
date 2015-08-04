namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
{
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.Testing.CustomShopApi.Business.Support.InventorySupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Thorny.Business.Operation.Core;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Utility;
    using System.Linq;
    using Trooper.Testing.CustomShopApi.Business.Model;

    public class ShopBusinessCore : BusinessCore<ShopEnt, Shop>, IShopBusinessCore
    {
        public ISaveResponse<ProductInShop> SaveProduct(ProductInShop productInShop, IIdentity identity)
        {
            using (var shopPack = this.GetBusinessPack())
            {
                var response = new SaveResponse<ProductInShop>();
                var productPack = shopPack.ResolveBusinessPack<IProductBusinessCore, ProductEnt, Product>();
                var inventoryPack = shopPack.ResolveBusinessPack<IInventoryBusinessCore, InventoryEnt, Inventory>();

                var getShop = this.GetByKey(shopPack, new ShopEnt { ShopId = productInShop.ShopId }, identity, response);

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

        public IManyResponse<ProductInShop> GetProducts(Shop shop, IIdentity identity)
        {
            using (var shopPack = this.GetBusinessPack())
            {
                var inventoryPack = shopPack.ResolveBusinessPack<IInventoryBusinessCore, InventoryEnt, Inventory>();
                var response = new ManyResponse<ProductInShop>();

                var getShop = this.GetByKey(shopPack, shop, identity);

                if (!getShop.Ok)
                {
                    return MessageUtility.Add(getShop, response);
                }

                var search = new InventorySearch { ShopId = shop.ShopId };
                var getSome = inventoryPack.BusinessCore.GetSome(inventoryPack, search, identity, false);

                if (!getShop.Ok)
                {
                    return MessageUtility.Add(getSome, response);
                }

                var items = from i in getSome.Items
                            let inventory = i as InventoryEnt
                            select new ProductInShop
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

        public IAddResponse<Shop> SimpleLittleThing(IIdentity identity)
        {
            return new AddResponse<Shop> { Item = new Shop { Name = "Coles", Address = "25 Brown St" } };
        }       
    }
}
