//using Exp018DynamicTake6.DynamicServiceHost;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Trooper.DynamicServiceHost;

//namespace Trooper.DynamicServiceHost
//{
//    public class DataClass
//    {
//        public string Name { get; set; }

//        public string Extends { get; set; }

//        //public string InterfaceName { get; set; }

//        //public List<Property> Properties { get; set; }

//        //public DataClass(string name)
//        //{
//        //    this.Name = name;
//        //}

//        //public DataClass(string name, Type interfaceType)
//        //{
//        //    this.Name = name;
//        //    this.InterfaceName = HostBuilder.GetTypeName(interfaceType);
//        //}

//        //public static DataClass FromInterface(Type interfaceType)
//        //{
//        //    var name = interfaceType.Name;

//        //    if (name.StartsWith("I")) 
//        //    {
//        //        name = name.Substring(1);
//        //    }
            
//        //    return FromInterface(interfaceType, true, name);
//        //}

//        //public static DataClass FromInterface(Type interfaceType, bool inherit, string name)
//        //{
//        //    var result = inherit ? new DataClass(name, interfaceType) : new DataClass(name);

//        //    result.Properties = new List<Property>();

//        //    foreach (var p in interfaceType.GetProperties())
//        //    {
//        //        result.Properties.Add(new Property(p.PropertyType, p.Name));
//        //    }

//        //    return result;
//        //}
//    }
//}
