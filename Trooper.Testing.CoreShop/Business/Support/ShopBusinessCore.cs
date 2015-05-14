using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Thorny.Business.Operation.Core;
using Trooper.Thorny.Interface;
using Trooper.Testing.CustomShopApi.Facade;
using Trooper.Testing.CustomShopApi.Interface.Business.Support;
using Trooper.Testing.ShopModel;
using Trooper.Testing.ShopModel.Interface;
using Trooper.Testing.ShopModel.Model;

namespace Trooper.Testing.CustomShopApi.Business.Support
{
    public class ShopBusinessCore : BusinessCore<Shop, IShop>, IShopBusinessCore
    {
    }
}
