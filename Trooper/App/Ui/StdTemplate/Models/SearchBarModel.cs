//--------------------------------------------------------------------------------------
// <copyright file="SearchBarModel.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Ui.StdTemplate.Models
{
    using System;
    using System.ComponentModel;
    using System.Web.WebPages;

    /// <summary>
    /// The SearchBar Model is passed to the SearchBarView
    /// and allows the settings of more obscure options.
    /// </summary>
    public class SearchBarModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBarModel"/> class. 
        /// </summary>
        public SearchBarModel()
        {
            this.SearchCommand = Commands.Refresh;

            this.ClearCommand = Commands.Clear;
        }

        /// <summary>
        /// Gets or sets the StdLayoutModel
        /// </summary>
        public StdLayoutModel StdLayoutModel { get; set; }

        /// <summary>
        /// Gets or sets Html which is what appears in the inputs area
        /// </summary>
        [ReadOnly(true)]
        public Func<object, HelperResult> Html { get; set; }

        /// <summary>
        /// Gets or sets the command which the search button will use.
        /// </summary>
        [ReadOnly(true)]
        public string SearchCommand { get; set; }

        /// <summary>
        /// Gets or sets the command which the clear button will use.
        /// </summary>
        [ReadOnly(true)]
        public string ClearCommand { get; set; }

        /// <summary>
        /// Gets or sets the command to be passed to the controller
        /// </summary>
        public string Command { get; set; }
    }
}
