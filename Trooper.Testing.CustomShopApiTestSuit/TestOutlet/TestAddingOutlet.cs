using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.TestSuit;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Thorny.Business.TestSuit;

namespace Trooper.Testing.CustomShopApiTestSuit.TestOutlet
{
    [TestFixture]
    public class TestAddingOutlet : Adding<Outlet>
    {
        public override ITestSuitHelper<Outlet> Helper { get; set; }

        public override IBusinessCreate<Outlet> Creater { get; set; }

        public override IBusinessRead<Outlet> Reader { get; set; }

        [SetUp]
        public void Setup()
        {
            this.Helper = new TestAddingOutletHelper();



            //this happens before each test.
            //init an object for gethelper (maybe), getcreater and gereader
        }
        
    }
}
