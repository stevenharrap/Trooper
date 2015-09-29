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
        //public ISaveResponse<ProductInOutlet> SaveProduct(ProductInOutlet productInShop, IIdentity identity)
        //{
        //    using (var outletPack = this.GetBusinessPack())
        //    {
        //        var response = new SaveResponse<ProductInOutlet>();
        //        var productPack = outletPack.ResolveBusinessPack<IProductBusinessCore, ProductEnt, Product>();
        //        var inventoryPack = outletPack.ResolveBusinessPack<IInventoryBusinessCore, InventoryEnt, Inventory>();

        //        var getShop = this.GetByKey(outletPack, new OutletEnt { OutletId = productInShop.ShopId }, identity, response);

        //        if (!getShop.Ok)
        //        {                    
        //            return MessageUtility.Add(getShop, response);
        //        }

        //        var productEnt = productPack.Facade.ToEnt(productInShop.AsProduct());
        //        var savedProduct = productPack.BusinessCore.Save(productPack, productEnt, identity, getShop);

        //        if (!savedProduct.Ok)
        //        {
        //            return MessageUtility.Add(savedProduct, response);
        //        }

        //        var inventoryEnt = inventoryPack.Facade.ToEnt(productInShop.AsInventory());
        //        var saveInventory = inventoryPack.BusinessCore.Save(inventoryPack, inventoryEnt, identity, savedProduct);

        //        if (saveInventory.Ok)
        //        {
        //            outletPack.Uow.Save(saveInventory);
        //        }

        //        return MessageUtility.Add(savedProduct, response);            
        //    }
        //}

        //public IManyResponse<ProductInOutlet> GetProducts(Outlet outlet, IIdentity identity)
        //{
        //    using (var outletPack = this.GetBusinessPack())
        //    {
        //        var inventoryPack = outletPack.ResolveBusinessPack<IInventoryBusinessCore, InventoryEnt, Inventory>();
        //        var response = new ManyResponse<ProductInOutlet>();

        //        var outletEnt = outletPack.Facade.ToEnt(outlet);
        //        var getOutlet = this.GetByKey(outletPack, outletEnt, identity);

        //        if (!getOutlet.Ok)
        //        {
        //            return MessageUtility.Add(getOutlet, response);
        //        }

        //        var search = new InventorySearch { ShopId = outlet.OutletId };
        //        var getSome = inventoryPack.BusinessCore.GetSome(inventoryPack, search, identity, false);

        //        if (!getOutlet.Ok)
        //        {
        //            return MessageUtility.Add(getSome, response);
        //        }

        //        var items = from i in getSome.Items
        //                    let inventory = i as InventoryEnt
        //                    select new ProductInOutlet
        //                    {
        //                        Bin = i.Bin,
        //                        Colour = inventory.Product.Colour,
        //                        Name = inventory.Product.Name,
        //                        ProductId = i.ProductId,
        //                        Quantity = i.Quantity,
        //                        ShopId = i.ShopId
        //                    };

        //        response.Items = items.ToList();

        //        return response;
        //    }
        //}

        //public IAddResponse<Outlet> SimpleLittleThing(IIdentity identity)
        //{
        //    return new AddResponse<Outlet> { Item = new Outlet { Name = "Coles", Address = "25 Brown St" } };
        //}       
    }
}
