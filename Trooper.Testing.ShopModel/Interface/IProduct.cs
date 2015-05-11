using System;

namespace Trooper.Testing.ShopModel.Interface
{
    interface IProduct
    {
        string Colour { get; set; }

        string Name { get; set; }

        int ShopId { get; set; }

        int? SpecDocId { get; set; }
    }
}
