namespace Trooper.Testing.CustomShop.Api.TestFromWs
{
    using NUnit.Framework;
    using System;
    using System.Diagnostics;
    using System.IO;
    using Thorny.Business.TestSuit;
    using Thorny.Business.TestSuit.Adding;
    using TestSuit.Common;
    using ShopPoco;
    using OutletBoServiceReference;

    [TestFixture]
    public class TestAddingOutlet : TestAdding<Outlet>
    {
        private Process srvCon;
        private OutletBoClient client;
        private WebServiceReaderMapper<Outlet> reader;
        private WebServiceCreaterMapper<Outlet> creater;
        private WebServiceDeleterMapper<Outlet> deleter;
        private TestOutletHelper helper;
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
            var path = Path.Combine(directory.FullName, "Trooper.Testing.CustomShop.Api.SrvCon", "bin", "Debug", "Trooper.Testing.CustomShop.Api.SrvCon.exe");

            Assert.That(File.Exists(path), Is.True, $"The path: '{path}' does not exist.");

            startInfo.FileName = path;
            this.srvCon = Process.Start(startInfo);

            var output = string.Empty;
            var count = 0;

            while (output != "ShopApp-started" && count < 100)
            {
                output = this.srvCon.StandardOutput.ReadLine();
                count++;
            }

            Assert.That(count, Is.LessThan(100), "The ShopApp Console did not start.");

            this.client = new OutletBoClient();
            this.client.Open();
            this.reader = new WebServiceReaderMapper<Outlet>(this.client);
            this.creater = new WebServiceCreaterMapper<Outlet>(this.client);
            this.deleter = new WebServiceDeleterMapper<Outlet>(this.client);
            this.helper = new TestOutletHelper(creater, reader, deleter);
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
