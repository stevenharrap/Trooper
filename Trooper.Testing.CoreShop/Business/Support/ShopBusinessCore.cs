using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.BusinessOperation2.Business.Operation.Core;
using Trooper.BusinessOperation2.Interface;
using Trooper.Testing.CoreShop.Facade;
using Trooper.Testing.CoreShop.Interface.Business.Support;
using Trooper.Testing.CoreShop.Interface.Model;
using Trooper.Testing.CoreShop.Model;

namespace Trooper.Testing.CoreShop.Business.Support
{
    //public class ShopBusinessCore : BusinessCore<Shop, IShop, UnitOfWork<ShopAppDbContext>, ShopFacade>, IShopBusinessCore
    public class ShopBusinessCore : BusinessCore<Shop, IShop>, IShopBusinessCore
    {
    }
}
