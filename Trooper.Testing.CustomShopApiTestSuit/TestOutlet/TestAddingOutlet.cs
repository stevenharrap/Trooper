using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.TestSuit;
using Trooper.Testing.CustomShopApi;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Thorny.Business.TestSuit;
using Trooper.Thorny.Configuration;
using Autofac;
using Trooper.Interface.Thorny.Business.Operation.Composite;
using Trooper.Testing.CustomShopApi.Interface.Business.Operation;
using Trooper.Thorny.Business.TestSuit.Adding;

namespace Trooper.Testing.CustomShopApiTestSuit.TestOutlet
{
    [TestFixture]
    public class TestAddingOutlet : Adding<Outlet>
    {
        public override Func<AddingRequirment<Outlet>> Requirement
        {
            get
            {
                return () =>
                {
                    var parameters = new BusinessModuleStartParameters { AutoStartServices = false };
                    var container = BusinessModule.Start<ShopAppModule>(parameters);

                    var reader = container.Resolve<IOutletBo>();
                    var creater = container.Resolve<IOutletBo>();
                    var deleter = container.Resolve<IOutletBo>();
                    var helper = new TestAddingOutletHelper(creater, creater, deleter);

                    var addingRequirement = new AddingRequirment<Outlet>(helper, creater);
                    addingRequirement.OnDisposing += (f) => { BusinessModule.Stop(container); };

                    return addingRequirement;
                };
            }
        }
    }
}
