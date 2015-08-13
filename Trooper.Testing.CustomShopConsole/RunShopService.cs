using System;
using Trooper.Testing.CustomShopApi;
using Trooper.Thorny.Configuration;

namespace Trooper.Testing.CustomShopSrvCon
{
    public class RunShopService
    {
        public static void Main(string[] args)
        {
            var container = BusinessModule.Start<ShopAppModule>();

            Console.WriteLine("ShopApp-started");
            Console.ReadLine();
            Console.WriteLine("ShopApp-stopped");
        }
    }
}
