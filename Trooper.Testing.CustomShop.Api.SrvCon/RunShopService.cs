namespace Trooper.Testing.CustomShop.Api.SrvCon
{
    using System;
    using System.Linq;
    using CustomShopApi;
    using Thorny.Configuration;

    public class RunShopService
    {
        public static void Main(string[] args)
        {
            var container = BusinessModule.Start<ShopAppModule>();

            foreach (var service in BusinessModule.GetAllServices(container))
            {
                Console.WriteLine("Service: {0}", service.ServiceHost.BaseAddresses.First());
            }

            Console.WriteLine("ShopApp-started");
            Console.ReadLine();
            Console.WriteLine("ShopApp-stopped");
        }
    }
}
