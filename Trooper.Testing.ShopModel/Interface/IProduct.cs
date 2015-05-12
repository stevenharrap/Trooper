using System;

namespace Trooper.Testing.ShopModel.Interface
{
    interface IProduct
    {
        int ProductId { get; set; }

        string Colour { get; set; }

        string Name { get; set; }

        //int? SpecDocId { get; set; }
    }
}
