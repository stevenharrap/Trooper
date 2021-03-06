﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Trooper.Testing.CustomShop.Api.TestFromWs.InventoryBoServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
        "BoNs", ConfigurationName="InventoryBoServiceReference.IInventoryBo")]
    public interface IInventoryBo {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetAll", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetAllResponse")]
        Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory> GetAll(Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetAll", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetAllResponse")]
        System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory>> GetAllAsync(Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetByKey", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetByKeyResponse")]
        Trooper.Thorny.Business.Response.SingleResponse<Trooper.Testing.ShopPoco.Inventory> GetByKey(Trooper.Testing.ShopPoco.Inventory item, Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetByKey", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetByKeyResponse")]
        System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.SingleResponse<Trooper.Testing.ShopPoco.Inventory>> GetByKeyAsync(Trooper.Testing.ShopPoco.Inventory item, Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/ExistsByKey", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/ExistsByKeyResponse")]
        Trooper.Thorny.Business.Response.SingleResponse<bool> ExistsByKey(Trooper.Testing.ShopPoco.Inventory item, Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/ExistsByKey", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/ExistsByKeyResponse")]
        System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.SingleResponse<bool>> ExistsByKeyAsync(Trooper.Testing.ShopPoco.Inventory item, Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/IsAllowed", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/IsAllowedResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.Business.Response.Response))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.Business.Response.Message[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.Business.Response.Message))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.Business.Response.SingleResponse<Trooper.Testing.ShopPoco.Inventory>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.Business.Response.SingleResponse<bool>))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.MessageAlertLevel))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.Business.Security.Identity))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Thorny.Business.Operation.Core.Search))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Testing.ShopPoco.Inventory[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(Trooper.Testing.ShopPoco.Inventory))]
        Trooper.Thorny.Business.Response.SingleResponse<bool> IsAllowed(Trooper.Thorny.Business.Security.RequestArg<Trooper.Testing.ShopPoco.Inventory> argument, Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/IsAllowed", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/IsAllowedResponse")]
        System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.SingleResponse<bool>> IsAllowedAsync(Trooper.Thorny.Business.Security.RequestArg<Trooper.Testing.ShopPoco.Inventory> argument, Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetSomeBySearch", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetSomeBySearchResponse")]
        Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory> GetSomeBySearch(Trooper.Thorny.Business.Operation.Core.Search search, Trooper.Thorny.Business.Security.Identity identity);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetSomeBySearch", ReplyAction="http://localhost:8000/Trooper.Testing.CustomShop.Api.Business.Operation.Inventory" +
            "BoNs/IInventoryBo/GetSomeBySearchResponse")]
        System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory>> GetSomeBySearchAsync(Trooper.Thorny.Business.Operation.Core.Search search, Trooper.Thorny.Business.Security.Identity identity);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IInventoryBoChannel : Trooper.Testing.CustomShop.Api.TestFromWs.InventoryBoServiceReference.IInventoryBo, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InventoryBoClient : System.ServiceModel.ClientBase<Trooper.Testing.CustomShop.Api.TestFromWs.InventoryBoServiceReference.IInventoryBo>, Trooper.Testing.CustomShop.Api.TestFromWs.InventoryBoServiceReference.IInventoryBo {
        
        public InventoryBoClient() {
        }
        
        public InventoryBoClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public InventoryBoClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InventoryBoClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public InventoryBoClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory> GetAll(Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.GetAll(identity);
        }
        
        public System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory>> GetAllAsync(Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.GetAllAsync(identity);
        }
        
        public Trooper.Thorny.Business.Response.SingleResponse<Trooper.Testing.ShopPoco.Inventory> GetByKey(Trooper.Testing.ShopPoco.Inventory item, Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.GetByKey(item, identity);
        }
        
        public System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.SingleResponse<Trooper.Testing.ShopPoco.Inventory>> GetByKeyAsync(Trooper.Testing.ShopPoco.Inventory item, Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.GetByKeyAsync(item, identity);
        }
        
        public Trooper.Thorny.Business.Response.SingleResponse<bool> ExistsByKey(Trooper.Testing.ShopPoco.Inventory item, Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.ExistsByKey(item, identity);
        }
        
        public System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.SingleResponse<bool>> ExistsByKeyAsync(Trooper.Testing.ShopPoco.Inventory item, Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.ExistsByKeyAsync(item, identity);
        }
        
        public Trooper.Thorny.Business.Response.SingleResponse<bool> IsAllowed(Trooper.Thorny.Business.Security.RequestArg<Trooper.Testing.ShopPoco.Inventory> argument, Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.IsAllowed(argument, identity);
        }
        
        public System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.SingleResponse<bool>> IsAllowedAsync(Trooper.Thorny.Business.Security.RequestArg<Trooper.Testing.ShopPoco.Inventory> argument, Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.IsAllowedAsync(argument, identity);
        }
        
        public Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory> GetSomeBySearch(Trooper.Thorny.Business.Operation.Core.Search search, Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.GetSomeBySearch(search, identity);
        }
        
        public System.Threading.Tasks.Task<Trooper.Thorny.Business.Response.ManyResponse<Trooper.Testing.ShopPoco.Inventory>> GetSomeBySearchAsync(Trooper.Thorny.Business.Operation.Core.Search search, Trooper.Thorny.Business.Security.Identity identity) {
            return base.Channel.GetSomeBySearchAsync(search, identity);
        }
    }
}
