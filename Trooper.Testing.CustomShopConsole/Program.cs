using System;
using Trooper.Testing.CustomShopApi;
using Trooper.Thorny.Configuration;

namespace Trooper.Testing.CustomShopConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BusinessModule.Start<ShopAppModule>();
                
                //BusinessModuleBuilder.StartBusinessApp<ShopAppModule>();

            Console.WriteLine("ShopApp started");
            Console.ReadLine();
        }
    }
}
