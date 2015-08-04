using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Trooper.Thorny.Interface.DataManager;

    public class RequestArg<T> : IRequestArg<T> 
        where T : class
    {
        public string Action { get; set; }

        public ISearch Search { get; set; }

        public T Item
        {
            get
            {
                return this.Items == null ? null : this.Items.FirstOrDefault();
            }
            set
            {
                if (this.Items == null)
                {
                    this.Items = new List<T> { value };
                }
                else if (this.Items.Any())
                {
                    this.Items[0] = value;
                }
                else
                {
                    this.Items.Add(value);
                }
            }
        }

        public IList<T> Items { get; set; }
    }
}
