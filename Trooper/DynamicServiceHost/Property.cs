using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.DynamicServiceHost;

namespace Exp018DynamicTake6.DynamicServiceHost
{
    public class Property
    {
        public string Name { get; set; }

        public string TypeName { get; set; }

        public Property(Type type, string name)
        {
            this.TypeName = HostBuilder.GetTypeName(type);
            this.Name = name;
        }

        public Property(string typeName, string name)
        {
            this.TypeName = typeName;
            this.Name = name;
        }      
    }
}
