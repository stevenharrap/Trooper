using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IGettingAll : IHelping
    {
        /// <summary>
        ///     Response.Items = all items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void ReturnsAllItemsWhenIdentityIsAllowed();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Access denied]
        /// </summary>
        void ReturnsNoItemsWhenIdentityIsDenied();

        /// <summary>
        ///     Response.Items = empty
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void ReturnsNoItemsWhenIdentityIsNull();        
    }
}
