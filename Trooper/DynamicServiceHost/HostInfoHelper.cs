using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Trooper.Interface.DynamicServiceHost;

namespace Trooper.DynamicServiceHost
{
    public class HostInfoHelper
    {
        public static void BuildHostInfo(Func<object> supporter, IHostInfo hostInfo)
        {
            var supportType = supporter().GetType();

            BuildHostInfo(supportType, hostInfo);
        }

        public static void BuildHostInfo(Type supportType, IHostInfo hostInfo)
        {
            var serviceTypeName = supportType.Name;
            var typeInterfaces = supportType.GetInterfaces();
            var interfaceTypeName = typeInterfaces.Count() == 1 ? typeInterfaces.First().Name : null;
            var methods = GetMethods(supportType).ToList();

            hostInfo.InterfaceName = hostInfo.InterfaceName ?? interfaceTypeName;
            hostInfo.ServiceName = hostInfo.ServiceName ?? serviceTypeName;

            if (hostInfo.Methods == null)
            {
                hostInfo.Methods = methods;
            }
            else
            {
                hostInfo.Methods.AddRange(methods);
            }
        }

        public static IHostInfo BuildHostInfo(Type serviceType)
        {            
            var serviceTypeName = serviceType.Name;
            var typeInterfaces = serviceType.GetInterfaces();
            var interfaceTypeName = typeInterfaces.Count() == 1 ? typeInterfaces.First().Name : null;

            var hostInfo = new HostInfo
            {
                InterfaceName = interfaceTypeName,
                ServiceName = serviceTypeName,
                Methods = new List<Method>()
            };

            hostInfo.Methods = GetMethods(serviceType).ToList();

            return hostInfo;
        }

        private static IEnumerable<Method> GetMethods(Type serviceType)
        {
            var typeInterfaces = serviceType.GetInterfaces();

            foreach (var typeMethod in serviceType.GetMethods())
            {
                var typeMethodParameters = typeMethod.GetParameters().Select(p => p.ParameterType).ToArray();
                var interfaceMethod = (from ti in typeInterfaces
                                       let m = ti.GetMethod(typeMethod.Name, typeMethodParameters)
                                       where m != null
                                       select m).FirstOrDefault();

                var hasOperationContract = interfaceMethod != null && interfaceMethod.GetCustomAttributes(typeof(OperationContractAttribute), true).Any();

                if (!hasOperationContract)
                {
                    continue;
                }

                var method = new Method
                {
                    Name = typeMethod.Name,
                    Parameters = typeMethod.GetParameters().Select(p => new Paramater(p.ParameterType, p.Name)).ToList(),
                    Returns = typeMethod.ReturnType,
                    Body = (MethodInput input) =>
                    {
                        var targetMethod = input.Supporter.GetType().GetMethod(typeMethod.Name);
                        var result = targetMethod.Invoke(input.Supporter, input.Inputs);

                        return typeMethod.ReturnType.IsEquivalentTo(typeof(void)) ? null : result;
                    }
                };

                yield return method;
            }
        }
        
        public static string MakeClassName(Type classType, bool incInterfacePrefix = false)
        {
            var name = classType.Name;
            
            if (classType.GenericTypeArguments.Any())
            {
                name += string.Format("Of{0}", classType.GenericTypeArguments.First().Name);
            }

            if (classType.GenericTypeArguments.Count() > 1)
            {
                name += string.Join("And", classType.GenericTypeArguments.Select(a => a.Name));
            }

            if (incInterfacePrefix)
            {
                name = "I" + name;
            }

            return name;
        }
    }
}
