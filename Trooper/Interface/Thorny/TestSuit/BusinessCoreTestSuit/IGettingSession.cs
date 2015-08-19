using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit
{
    public interface IGettingSession
    {
        /// <summary>
        ///     Response.Item = new Guid
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void AValidIdentityWillGetASession();

        /// <summary>
        ///     Response.Item = default Guid
        ///     Response.Ok = false
        ///     Response.Messages = [Identity no allowed]
        /// </summary>
        void AnIdentityWithNoAccessWillNotGetASession();

        /// <summary>
        ///     Response.Item = default Guid
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        void AnNullIdentityWillNotGetASession();

        /// <summary>
        ///     Response.Items = some items
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        void AValidSessionCanBeUsedWithGetAll();
        
        /// <summary>
        ///     Response.Items = null
        ///     Response.Ok = false
        ///     Response.Messages = empty
        /// </summary>
        void AnInvalidSessionCannotBeUsedWithGetAll();
    }
}
