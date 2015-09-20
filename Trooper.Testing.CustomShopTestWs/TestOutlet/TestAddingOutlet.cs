using NUnit.Framework;
using System;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Testing.CustomShopTestWs.OutletBoServiceReference;
using Trooper.Thorny.Business.TestSuit.Adding;

namespace Trooper.Testing.CustomShopTestWs.TestOutlet
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
                    var client = new OutletBoClient();

                    var reader = client as IBusinessRead<Outlet>;//  IOutletBo;
                    var creater = client as IBusinessCreate<Outlet>;
                    var deleter = client as IBusinessDelete<Outlet>;
                    var helper = new TestAddingOutletHelper(creater, reader, deleter);

                    var addingRequirement = new AddingRequirment<Outlet>(helper, creater);
                    addingRequirement.OnDisposing += (f) => { client.Close(); };

                    return addingRequirement;                    
                };
            }
        }
    }
}
