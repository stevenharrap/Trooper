using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.BusinessOperation2.Business.Operation.Core;
using Trooper.BusinessOperation2.Interface;
using Trooper.Testing.DefaultShopApi.Facade;
using Trooper.Testing.DefaultShopApi.Interface.Business.Support;
using Trooper.Testing.ShopModel;
using Trooper.Testing.ShopModel.Interface;

namespace Trooper.Testing.DefaultShopApi.Business.Support
{
    //public class ShopBusinessCore : BusinessCore<Shop, IShop, UnitOfWork<ShopAppDbContext>, ShopFacade>, IShopBusinessCore
    public class ShopBusinessCore : BusinessCore<Shop, IShop>, IShopBusinessCore
    {
    }
}
