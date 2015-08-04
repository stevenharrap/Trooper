namespace Trooper.Thorny.Business.Operation.Core
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using Trooper.Thorny.Interface.DataManager;

    [DataContract]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class Search : ISearch
    {
        [DataMember]
        public int SkipItems { get; set; }

        [DataMember]
        public int TakeItems { get; set; }
    }
}
