using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;

namespace Trooper.DynamicServiceHost
{
    public class Paramater
    {
        public string Name { get; set; }

        public Type Type { get; set; }

        public Paramater(Type type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }
}
