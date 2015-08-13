using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Trooper.Testing.CustomShopTestWs
{
    [TestFixture]
    [Category("BusinessOperation")]
    public class Class1
    {

        private Process srvCon;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var startInfo = new ProcessStartInfo();
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            directory = directory.Parent.Parent.Parent;
            var path = Path.Combine(directory.FullName, "Trooper.Testing.CustomShopConsole", "bin", "Debug", "Trooper.Testing.CustomShopSrvCon.exe");

            Assert.That(File.Exists(path), Is.True);

            startInfo.FileName = path;
            this.srvCon = Process.Start(startInfo);

            var output = this.srvCon.StandardOutput.ReadLine();
            Assert.IsTrue(output.Contains("ShopApp-started"));            
        }

        [Test]
        public void DoATest()
        {
            Thread.Sleep(5000);
            Assert.True(true);
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
