﻿using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.IO;
using System.CodeDom;
using Trooper.Interface.DynamicServiceHost;
using Trooper.DynamicServiceHost.Exceptions;

namespace Trooper.DynamicServiceHost
{
    public class HostBuilder
    {
        public static ServiceHost BuildHost(IHostInfo hostInfo)
        {
            return BuildHost(hostInfo, null);
        }

        public static ServiceHost BuildHost(IHostInfo hostInfo, Func<object> supporter)
        {
            ValidateHostInfo(hostInfo);
                        
            var code = new StringBuilder();
            var codeNamespace = string.Format("{0}.DynamicHost.{1}", typeof(HostBuilder).Namespace, hostInfo.CodeNamespace);

            AppendCode(code, 0, "namespace {0} {{", codeNamespace);

            code.Append(GenerateMappingsCode(hostInfo));

            code.Append(GenerateClassCode(hostInfo));

            code.AppendLine();

            code.Append(GenerateInerfaceCode(hostInfo));

            code.AppendLine();            
            
            AppendCode(code, 0, "}}");

            var c = code.ToString();

            var assembly = CompileSource(code.ToString(), codeNamespace + ".dll");
            var serviceType = assembly.GetType(string.Format("{0}.{1}", codeNamespace, hostInfo.ServiceName));
            var interfaceType = assembly.GetType(string.Format("{0}.{1}", codeNamespace, hostInfo.InterfaceName));

            var host = new HostFactoryBuilder.DynamicServiceHost(hostInfo, supporter, serviceType, hostInfo.Address);

            ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
            host.Description.Behaviors.Add(mBehave);
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            BasicHttpBinding httpb = new BasicHttpBinding();

            var endPoint = host.AddServiceEndpoint(interfaceType, httpb, hostInfo.Address);
            endPoint.Binding.Namespace = hostInfo.ServiceNampespace.ToString();

            var debug = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (debug == null)
            {
                host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }

            return host;
        }

        private static StringBuilder GenerateClassCode(IHostInfo hostInfo)
        {
            var code = new StringBuilder();

            AppendCode(code, 1, "using {0};", typeof(HostBuilder).Namespace);
            AppendCode(code, 1, "using System;");
            AppendCode(code, 1, "using System.Linq;");
            AppendCode(code, 1, "using System.ServiceModel;");
            AppendCode(code, 1, "using System.Diagnostics;");
            AppendCode(code, 1, "using System.Runtime.Serialization;");
            code.AppendLine();

            AppendCode(code, 1, "[ServiceBehavior(Namespace = \"{0}\")]", hostInfo.ServiceNampespace);
            AppendCode(code, 1, "public class {0} : {1} {{", hostInfo.ServiceName, hostInfo.InterfaceName);

            AppendCode(code, 2, "private HostInfo hostInfo;");
            AppendCode(code, 2, "private object supporter;");
            

            AppendCode(code, 2, "public {0}(HostInfo hostInfo, object supporter) {{ this.hostInfo = hostInfo; this.supporter = supporter; }}", hostInfo.ServiceName);
            code.AppendLine();

            foreach (var method in hostInfo.Methods)
            {               
                var hasReturn = method.Returns != null && !method.Returns.IsEquivalentTo(typeof(void));

                AppendCode(
                    code,
                    2,
                    "public {0} {1} ({2}) {{",
                    ResolveTypeAlias(method.Returns, hostInfo),
                    method.Name,
                    string.Join(",", method.Parameters.Select(p => string.Format("{0} {1}", ResolveTypeAlias(p.Type, hostInfo), p.Name))));

                if (method.DebugMethod)
                {
                    AppendCode(code, 3, "Debugger.Break();");
                }

                AppendCode(
                    code,
                    3,
                    @"var method = this.hostInfo.Methods.FirstOrDefault(m => m.Name == ""{0}"");",
                    method.Name);                

                if (hasReturn)
                {
                    AppendCode(
                        code,
                        3,
                        "var returnValue = method.Body(new MethodInput {{ Supporter = this.supporter, Inputs = new object[] {{{0}}} }});",
                        string.Join(",", method.Parameters.Select(p => p.Name)));

                    //AppendCode(code, 3, "{0}.{1}.ReturnTypeCheck(method, returnValue);", typeof(HostBuilder).Namespace, typeof(HostBuilder).Name);

                    if (method.Returns.IsPrimitive)
                    {
                        AppendCode(code, 3, "return ({0})returnValue;", GetTypeName(method.Returns));
                    }
                    else
                    {
                        AppendCode(code, 3, "return returnValue as {0};", ResolveTypeAlias(method.Returns, hostInfo));//GetTypeName(method.Returns));
                    }                    
                }
                else
                {
                    AppendCode(
                        code,
                        3,
                        "method.Body(new MethodInput {{ Supporter = this.supporter, Inputs = new object[] {{{0}}} }});",
                        string.Join(",", method.Parameters.Select(p => p.Name)));
                }

                AppendCode(code, 2, "}}");
            }

            AppendCode(code, 1, "}}");

            return code;
        }

        private static StringBuilder GenerateInerfaceCode(IHostInfo hostInfo)
        {
            var code = new StringBuilder();

            AppendCode(code, 1, "[ServiceContract(Namespace = \"{0}\")]", hostInfo.ServiceNampespace);
            AppendCode(code, 1, "public interface {0} {{", hostInfo.InterfaceName);

            foreach (var method in hostInfo.Methods)
            {
                AppendCode(code, 2, "[OperationContract]");
                AppendCode(code, 2, "{0} {1} ({2});",
                    ResolveTypeAlias(method.Returns, hostInfo),
                    method.Name,
                    string.Join(",", method.Parameters.Select(p => string.Format("{0} {1}", ResolveTypeAlias(p.Type, hostInfo), p.Name))));

                code.AppendLine();
            }

            AppendCode(code, 1, "}}");

            return code;
        }

        private static string TryGetLocation(Assembly assembly)
        {
            try
            {
                return assembly.Location;
            }
            catch
            {
                return null;
            }
        }

        private static StringBuilder GenerateMappingsCode(IHostInfo hostInfo) 
        {
            var code = new StringBuilder();

            //if (string.IsNullOrEmpty(dataClass.InterfaceName))
            //{
            //    AppendCode(code, 1, "public class {0} {{", dataClass.Name);
            //}
            //else
            //{
            //    AppendCode(code, 1, "public class {0} : {1} {{", dataClass.Name, dataClass.InterfaceName);
            //}

            //if (dataClass.Properties != null)
            //{
            //    foreach (var p in dataClass.Properties)
            //    {
            //        AppendCode(code, 2, "public {0} {1} {{ get; set; }}", p.TypeName, p.Name);
            //    }
            //}

            //AppendCode(code, 1, "}}");

            foreach (var m in hostInfo.Mappings)
            {
                //AppendCode(code, 1, "[DataContract]");
                //AppendCode(code, 1, "[ServiceContract(Namespace = \"{0}\")]", hostInfo.ServiceNampespace);

                //if (m.Source.IsEquivalentTo(m.ResolveTo))
                //{
                //    AppendCode(code, 1, "public class {0} : {1} {{ }}", m.Alias, HostBuilder.GetTypeName(m.Source));
                //}
                //else
                //{
                //    AppendCode(code, 1, "public class {0} : {1} {{ }}", m.Alias, HostBuilder.GetTypeName(m.ResolveTo));
                //}

                AppendCode(code, 1, "using {0} = {1};", m.Alias, HostBuilder.GetTypeName(m.ResolveTo));                
            }

            return code;
        }

        private static string ResolveTypeAlias(Type sourceType, IHostInfo hostInfo)
        {
            if (sourceType.IsPrimitive)
            {
                return GetTypeName(sourceType);
            }
            
            var mapping = hostInfo.Mappings.FirstOrDefault(m => m.Source.IsEquivalentTo(sourceType));

            if (mapping == null)
            {
                return GetTypeName(sourceType);
            }

            return mapping.Alias;            
        }

        private static Assembly CompileSource(string sourceCode, string outputLocation)
        {
            var options = new CompilerParameters();

            options.GenerateExecutable = false;
            options.OutputAssembly = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), outputLocation);
            options.TempFiles = new TempFileCollection(Environment.GetEnvironmentVariable("TEMP"), true);
            options.IncludeDebugInformation = true;

            //options.GenerateInMemory = true;

            var ignore = new string[] {
                "Microsoft.VisualStudio.HostingProcess.Utilities.dll", 
                "Microsoft.VisualStudio.HostingProcess.Utilities.Sync.dll", 
                "Microsoft.VisualStudio.Debugger.Runtime.dll",
                "Microsoft.VisualStudio.Diagnostics.ServiceModelSink.dll"
            };

            var assemblies = (from a in AppDomain.CurrentDomain.GetAssemblies()
                              let file = TryGetLocation(a)
                              where ignore.All(i => i != file)
                              select file).ToList();

            options.ReferencedAssemblies.AddRange(assemblies.ToArray());

            var providerOptions = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };
            var provider = new Microsoft.CSharp.CSharpCodeProvider(providerOptions);            
            var result = provider.CompileAssemblyFromSource(options, sourceCode);

            var sb = new StringBuilder();

            if (result.Errors.HasErrors)
            {
                var errors = new List<string>();

                foreach (CompilerError ce in result.Errors)
                {
                    if (ce.IsWarning) 
                    {
                        continue;
                    }

                    errors.Add(string.Format("{0}(line: {1}, column: {2}: error {3}: {4}", ce.FileName, ce.Line, ce.Column, ce.ErrorNumber, ce.ErrorText));
                }

                var exp = new HostBuilderDynamicCodeException("The comilation of a class and interface for a dynamic host has failed.", sourceCode, errors);

                throw exp;
            }

            return result.CompiledAssembly;
        }

        private static void ValidateHostInfo(IHostInfo hostInfo)
        {
            if (hostInfo == null)
            {
                throw new HostBuilderValidateHostInfoException("HostInfo cannot be null.");
            }

            if (hostInfo.Methods == null || !hostInfo.Methods.Any())
            {
                throw new HostBuilderValidateHostInfoException(
                    string.Format("No methods have been provided for the host '{0}'.", hostInfo.ServiceNampespace));
            }

            foreach (var m in hostInfo.Methods)
            {
                var count = hostInfo.Methods.Count(mc => mc.Name == m.Name);

                if (count > 1)
                {
                    throw new HostBuilderValidateHostInfoException(
                        string.Format("The method name '{0}' in '{1}' cannot be used more than once.", m.Name, hostInfo.ServiceNampespace));
                }
            }

            foreach (var m  in hostInfo.Methods) 
            {
                if (!CodeGenerator.IsValidLanguageIndependentIdentifier(m.Name))
                {
                    throw new HostBuilderValidateHostInfoException(
                        string.Format("No method name '{0}' in the host '{1}' is invalid.", m.Name, hostInfo.ServiceNampespace));
                }

                foreach (var p in m.Parameters)
                {
                    if (!CodeGenerator.IsValidLanguageIndependentIdentifier(p.Name))
                    {
                        throw new HostBuilderValidateHostInfoException(
                            string.Format("No parameter name '{0}' in method '{1}' in the host '{2}' is invalid.", p.Name, m.Name, hostInfo.ServiceNampespace));
                    }
                }
            }

            try
            {
                CodeGenerator.ValidateIdentifiers(new CodeNamespace(hostInfo.CodeNamespace));
            }
            catch
            {
                throw new HostBuilderValidateHostInfoException(
                    string.Format("'{0}' is an invalid code namespace for the service.", hostInfo.CodeNamespace));
            }

            var baseNs = typeof(HostBuilder).Namespace.Split('.').First();

            if (hostInfo.CodeNamespace.StartsWith(baseNs + "."))
            {
                throw new HostBuilderValidateHostInfoException(
                    string.Format("The code namespace '{0}' is not allowed to start with '{1}.'", hostInfo.CodeNamespace, baseNs));
            }

            if (!CodeGenerator.IsValidLanguageIndependentIdentifier(hostInfo.InterfaceName))
            {
                throw new HostBuilderValidateHostInfoException(
                    string.Format("'{0}' is an invalid interface name for the service.", hostInfo.CodeNamespace));
            }

            if (!CodeGenerator.IsValidLanguageIndependentIdentifier(hostInfo.ServiceName))
            {
                throw new HostBuilderValidateHostInfoException(
                    string.Format("'{0}' is an invalid class name for the service.", hostInfo.CodeNamespace));
            }

        }

        private static void AppendCode(StringBuilder sb, int tabs, string format, params object[] args)
        {
            for (var t = 0; t < tabs; t++)
            {
                sb.Append("   ");
            }

            sb.AppendFormat(format, args);
            sb.Append(Environment.NewLine);            
        }

        public static string GetTypeName(Type type)
        {
            var codeDomProvider = CodeDomProvider.CreateProvider("C#");
            var typeReferenceExpression = new CodeTypeReferenceExpression(new CodeTypeReference(type));
            using (var writer = new StringWriter())
            {
                codeDomProvider.GenerateCodeFromExpression(typeReferenceExpression, writer, new CodeGeneratorOptions());
                return writer.GetStringBuilder().ToString();
            }
        }

        //public static void ReturnTypeCheck(Method method, object returnValue)
        //{
        //    if (returnValue.GetType().IsEquivalentTo(method.Returns))
        //    {
        //        return;
        //    }

        //    var message = string.Format(
        //        "The service method return type '{0}' does not equal the method body return type '{1}'.",
        //        method.Returns.FullName,
        //        returnValue.GetType().FullName);

        //    throw new Exception(message);

        //}        
    }
}
