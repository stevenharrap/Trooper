//--------------------------------------------------------------------------------------
// <copyright file="FrameModel.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Bootstrap.Models
{
    using System;

    /// <summary>
    /// The model that form inside the IFrame uses.
    /// </summary>
    public class FrameModel
    {
        /// <summary>
        /// Gets or sets the input id of the upload control.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the file. When the IFrame submits the controller will return
        /// the form with the display name of the file. The exterior page picks the up with JQuery and 
        /// updates display.
        /// </summary>
        public string DisplayFilename { get; set; }

        /// <summary>
        /// Gets or sets the GUID which identifies the temporary uploaded file with the control.
        /// </summary>
        public Guid PersistedId { get; set; }
    }
}
