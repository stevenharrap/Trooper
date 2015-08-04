
using Trooper.Interface.Thorny.Business.Response;

namespace Trooper.Thorny.Business.Response
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel;

    [DataContract(Name = "SaveSomeItemOf{0}")]
    [ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public class SaveSomeItem<T> : ISaveSomeItem<T>
    {
        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        public T Item { get; set; }

        /// <summary>
        /// Gets or sets the the change type which indicates if it was an Add or Update.
        /// Null indicates failure.
        /// </summary>
        [DataMember]
        public SaveChangeType? Change { get; set; }
    }
}
