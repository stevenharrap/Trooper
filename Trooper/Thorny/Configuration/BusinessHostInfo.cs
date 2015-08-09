using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.DynamicServiceHost;
using Trooper.Interface.Thorny.Configuration;
using Trooper.Thorny.Interface.DataManager;

namespace Trooper.Thorny.Configuration
{
    public class BusinessHostInfo : HostInfo, IBusinessHostInfo
    {
        public BusinessHostInfo()
        {
            this.UseDefaultTypes = true;
        }

        public string BaseAddress { get; set; }

        public bool UseDefaultTypes { get; set; }

        public Action<IBusinessHostInfo> HostInfoBuilt { get; set; }

        //public IList<ClassMapping> SearchMappings { get; set; }

        //public static ClassMapping NewSearch<TISearch, TSearch>()
        //    where TISearch : class, ISearch
        //    where TSearch : class, TISearch, new()
        //{
        //    return ClassMapping.Make<TISearch, TSearch>();

        //}

        //public static ClassMapping NewSearch<TSearch>()
        //    where TSearch : class, ISearch, new()
        //{
        //    return ClassMapping.Make<TSearch>();

        //}
    }

    
}
