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
        public override ITestSuitHelper<Outlet> GetHelper()
        {
            throw new NotImplementedException();
        }

        public override IBusinessCreate<Outlet> GetCreater()
        {
            throw new NotImplementedException();
        }

        public override IBusinessRead<Outlet> GetReader()
        {
            throw new NotImplementedException();
        }

        [SetUp]
        public void Setup()
        {
            //this happens before each test.
            //init an object for gethelper (maybe), getcreater and gereader
        }
        
    }
}
