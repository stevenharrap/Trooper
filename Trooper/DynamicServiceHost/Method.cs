using System;
using System.Collections.Generic;

namespace Trooper.DynamicServiceHost
{
    public class Method
    {
        public string Name { get; set; }

        public Type Returns { get; set; }

        public List<Paramater> Parameters { get; set; }

        public Func<MethodInput, object> Body { get; set; }

        public bool DebugMethod { get; set; }
    }
}
