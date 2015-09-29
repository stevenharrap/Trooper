using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Trooper.Thorny.Interface.DataManager;

    public class RequestArg<TPoco> : IRequestArg<TPoco> 
        where TPoco : class
    {
        public RequestArg() { }

        public RequestArg(TPoco item)
        {
            if (item == null) return;

            this.Items = new List<TPoco> { item };
        }

        public RequestArg(IEnumerable<TPoco> items)
        {
            if (Items == null) return;

            this.Items = items;
        }

        public string Action { get; set; }

        public ISearch Search { get; set; }
                
        public IEnumerable<TPoco> Items { get; set; }
    }
}
