using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Testing.CustomShopTestWs.OutletBoServiceReference;
using Trooper.Thorny.Business.TestSuit;
using Trooper.Thorny.Business.TestSuit.Adding;

namespace Trooper.Testing.CustomShopTestWs.TestOutlet
{
    [TestFixture]
    public class TestAddingOutlet : Adding<Outlet>
    {
        private Process srvCon;

        public override Func<AddingRequirment<Outlet>> Requirement
        {
            get
            {
                return () =>
                {
                    var client = new OutletBoClient();

                    var reader = new WebServiceReaderMapper<Outlet>(client);
                    var creater = new WebServiceCreaterMapper<Outlet>(client);
                    var deleter = new WebServiceDeleterMapper<Outlet>(client);                                        
                    var helper = new TestAddingOutletHelper(creater, reader, deleter);

                    var addingRequirement = new AddingRequirment<Outlet>(helper, creater);
                    addingRequirement.OnDisposing += (f) => { client.Close(); };

                    return addingRequirement;                    
                };
            }
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var startInfo = new ProcessStartInfo();
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            directory = directory.Parent.Parent.Parent;
            var path = Path.Combine(directory.FullName, "Trooper.Testing.CustomShopSrvCon", "bin", "Debug", "Trooper.Testing.CustomShopSrvCon.exe");

            Assert.That(File.Exists(path), Is.True);

            startInfo.FileName = path;
            this.srvCon = Process.Start(startInfo);

            var output = this.srvCon.StandardOutput.ReadLine();
            Assert.IsTrue(output.Contains("ShopApp-started"));
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            this.srvCon.StandardInput.WriteLine();
            var output = this.srvCon.StandardOutput.ReadLine();
            Assert.IsTrue(output.Contains("ShopApp-stopped"));
            this.srvCon.WaitForExit();
        }
    }
}
