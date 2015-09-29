using System.Runtime.Serialization;
using System.ServiceModel;
using Trooper.Thorny;

namespace Trooper.Interface.Thorny.Business.Response
{
	[ServiceContract(Namespace = Constants.ServiceContractNameSpace)]
    public interface ISaveSomeItem<T>
    {
        /// <summary>
        /// Gets or sets the item being returned.
        /// </summary>
        T Item { get; set; }

        /// <summary>
        /// Gets or sets the the change type which indicates if it was an Add or Update.
        /// Null indicates failure.
        /// </summary>
        SaveChangeType Change { get; set; }
    }
}
