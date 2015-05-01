using System.Runtime.Serialization;
using System.ServiceModel;
using Trooper.BusinessOperation2;

namespace Trooper.Interface.BusinessOperation2.Business.Response
{
	[ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface ISaveSomeItem<T>
    {
        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        [DataMember]
        T Item { get; set; }

        /// <summary>
        /// Gets or sets the the change type which indicates if it was an Add or Update.
        /// Null indicates failure.
        /// </summary>
        [DataMember]
        SaveChangeType? Change { get; set; }
    }
}
