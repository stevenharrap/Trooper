using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Trooper.Interface.Thorny.Configuration;

namespace Trooper.Testing.CustomShopWeb.Models
{
    public class HomeModel
    {

        public IEnumerable<IBusinessOperationService> AllServices { get; set; }
    }
}