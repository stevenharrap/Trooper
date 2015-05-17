//--------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Thorny
{
    /// <summary>
    /// The constants of this project.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// This message is returned in exceptions when the program is attempting to use IsNewEntity in a 
        /// situation where that would be misleading.
        /// </summary>
        public const string NotAutoKeyMsg =
            "Load cannot be used in this context becuase the entity key is not automatically generated. "
            + "An existing primary value is missleading and does not imply that record exists in the DB.";

        /// <summary>
        /// The service contract name space.
        /// </summary>
        public const string ServiceContractNameSpace = "Trooper.ServiceOperations";

        public const string BusinessCoreErrorCodeRoot = "Trooper.Thorny.BusinessCore";

        public const string ValidationErrorCodeRoot = "Trooper.Thorny.Validation";

        public const string AuthorizationErrorCodeRoot = "Trooper.Thorny.Authorization";
    }
}