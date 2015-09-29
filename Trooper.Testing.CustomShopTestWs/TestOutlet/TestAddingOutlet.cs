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
        private OutletBoClient client;
        private WebServiceReaderMapper<Outlet> reader;
        private WebServiceCreaterMapper<Outlet> creater;
        private WebServiceDeleterMapper<Outlet> deleter;
        private TestAddingOutletHelper helper;
        private AddingRequirment<Outlet> addingRequirement;

        public override Func<AddingRequirment<Outlet>> Requirement
        {
            get
            {
                return () =>
                {
                    //var x = this.reader.GetAll(this.helper.MakeValidIdentity());

                    return this.addingRequirement;
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

            this.client = new OutletBoClient();
            this.client.Open();
            this.reader = new WebServiceReaderMapper<Outlet>(this.client);
            this.creater = new WebServiceCreaterMapper<Outlet>(this.client);
            this.deleter = new WebServiceDeleterMapper<Outlet>(this.client);
            this.helper = new TestAddingOutletHelper(creater, reader, deleter);
            this.addingRequirement = new AddingRequirment<Outlet>(helper, creater);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            this.client.Close();

            this.srvCon.StandardInput.WriteLine();
            var output = this.srvCon.StandardOutput.ReadLine();
            Assert.IsTrue(output.Contains("ShopApp-stopped"));
            this.srvCon.WaitForExit();
        }
    }
}
