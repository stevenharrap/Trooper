namespace Trooper.Testing.CustomShopApi.Business.Support.ShopSupport
{
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.Testing.CustomShopApi.Business.Support.InventorySupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Model;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ShopSupport;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;
    using Trooper.Thorny.Business.Operation.Core;
    using Trooper.Thorny.Business.Response;
    using Trooper.Thorny.Utility;
    using System.Linq;
    using Trooper.Testing.CustomShopApi.Business.Model;

    public class ShopBusinessCore : BusinessCore<Shop, IShop>, IShopBusinessCore
    {
        public ISaveResponse<IProductInShop> SaveProduct(IProductInShop productInShop, IIdentity identity)
        {
            using (var shopPack = this.GetBusinessPack())
            {
                var response = new SaveResponse<IProductInShop>();
                var productPack = shopPack.ResolveBusinessPack<IProductBusinessCore, Product, IProduct>();
                var inventoryPack = shopPack.ResolveBusinessPack<IInventoryBusinessCore, Inventory, IInventory>();

                var getShop = this.GetByKey(shopPack, new Shop { ShopId = productInShop.ShopId }, identity, response);

                if (!getShop.Ok)
                {                    
                    return MessageUtility.Add(getShop, response);
                }

                var savedProduct = productPack.BusinessCore.Save(productPack, productInShop, identity, getShop);

                if (!savedProduct.Ok)
                {
                    return MessageUtility.Add(savedProduct, response);
                }

                var saveInventory = inventoryPack.BusinessCore.Save(inventoryPack, productInShop, identity, savedProduct);

                if (saveInventory.Ok)
                {
                    shopPack.Uow.Save(saveInventory);
                }

                return MessageUtility.Add(savedProduct, response);            
            }
        }

        public IManyResponse<IProductInShop> GetProducts(IShop shop, IIdentity identity)
        {
            using (var shopPack = this.GetBusinessPack())
            {
                var inventoryPack = shopPack.ResolveBusinessPack<IInventoryBusinessCore, Inventory, IInventory>();
                var response = new ManyResponse<IProductInShop>();

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
                            let inventory = i as Inventory
                            select new ProductInShop 
                            { 
                                Bin = i.Bin, 
                                Colour = inventory.Product.Colour, 
                                Name = inventory.Product.Name, 
                                ProductId = i.ProductId, 
                                Quantity = i.Quantity, 
                                ShopId = i.ShopId 
                            } as IProductInShop;

                response.Items = items.ToList();

                return response;
            }
        }
    }
}
