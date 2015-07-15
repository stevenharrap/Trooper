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
        public static void BuildHostInfo(IHostInfo hostInfo, IComponentContext container)
        {
            var serviceTypeName = hostInfo.SupportType.Name;
            var typeInterfaces = hostInfo.SupportType.GetInterfaces();
            var interfaceTypeName = typeInterfaces.Count() == 1 ? typeInterfaces.First().Name : null;
            var methods = GetMethods(hostInfo.SupportType, container).ToList();

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

        public static IHostInfo BuildHostInfo(Type serviceType, IComponentContext container)
        {            
            var serviceTypeName = serviceType.Name;
            var typeInterfaces = serviceType.GetInterfaces();
            var interfaceTypeName = typeInterfaces.Count() == 1 ? typeInterfaces.First().Name : null;

            var hostInfo = new HostInfo
            {
                InterfaceName = interfaceTypeName,
                ServiceName = serviceTypeName,
                SupportType = serviceType,
                Methods = new List<Method>()
            };

            hostInfo.Methods = GetMethods(serviceType, container).ToList();

            return hostInfo;
        }

        private static IEnumerable<Method> GetMethods(Type serviceType, IComponentContext container)
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
                    Parameters = typeMethod.GetParameters().Select(p => new Paramater { Name = p.Name, Type = ResolveType(p.ParameterType, container) }).ToList(),
                    Returns = ResolveType(typeMethod.ReturnType, container),
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

        private static Type ResolveType(Type sourceType, IComponentContext container)
        {
            if (sourceType.IsPrimitive)
            {
                return sourceType;
            }
            else
            {
                object returnObject = null;
                container.TryResolve(sourceType, out returnObject);

                if (returnObject == null && sourceType.IsInterface)
                {
                    throw new Exception(string.Format("A concrete type cannot be built from the type {0}.", sourceType.Name));
                }

                return returnObject == null ? sourceType : returnObject.GetType();
            }                
        }
    }
}
