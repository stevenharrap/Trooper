//--------------------------------------------------------------------------------------
// <copyright file="StdLayoutModel.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.App.Ui.StdTemplate.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;

    using Trooper.App.Ui.StdTemplate.Classes;
    using Trooper.BusinessOperation.Business;
    using Trooper.BusinessOperation.Interface;
    using Trooper.BusinessOperation.Response;
    using Trooper.BusinessOperation.Utility;
    using Trooper.Utility;

    /// <summary>
    /// The base model. All models extend this model and it attempts to provide the core 
    /// components for all pages. You should use this as the extension to your base model in 
    /// your application.
    /// </summary>
    public class StdLayoutModel
    {
        /// <summary>
        /// The showLogoRow value that determines if the logo row should show.
        /// </summary>
        private bool showLogoRow;

        /// <summary>
        /// hidden field determining if showRecordManagement should be always hidden or shown.
        /// </summary>
        private bool? showRecordManagement;

        /// <summary>
        /// Gets or sets the validation results which the page will display if there are errors.
        /// </summary>
        [ReadOnly(true)]
        public IOperationResponse OperationResponse { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show record management should be displayed.
        /// Setting this to true or false will always hide or show the errors.
        /// If not set then it will decided itself based on the state of the operation response.
        /// </summary>
        public bool ShowRecordManagement
        {
            get
            {
                if (this.showRecordManagement == null)
                {
                    if (this.OperationResponse != null && !this.OperationResponse.Ok && !this.OperationResponse.Warn)
                    {
                        return true;
                    }

                    return this.OperationResponse != null 
                        && !string.IsNullOrEmpty(this.Command)
                        && this.Command != Commands.Clear 
                        && this.Command != Commands.Refresh
                        && this.Command != Commands.Unknown 
                        && this.Command != Commands.View;
                }
                
                return (bool)this.showRecordManagement;
            }

            set
            {
                this.showRecordManagement = value;
            }
        }

        /// <summary>
        /// Gets or sets the top navigation items that make up your main menu.
        /// </summary>
        [ReadOnly(true)]
        public List<NavItem> TopNavItems { get; set; }
        
        /// <summary>
        /// Gets or sets the left navigation items that make up your main menu. If is null
        /// or empty then no menu will be generated. You'll get an empty area if this is empty.
        /// Use <see cref="StdTemplateHelper.RenderContextMenu"/> to render the menu.
        /// </summary>
        [ReadOnly(true)]
        public List<NavItem> LeftNavItems { get; set; }
        
        /// <summary>
        /// Gets or sets the title that appears if the command was ok. If this is not set
        /// then a default title will be produced in the view
        /// </summary>
        [ReadOnly(true)]
        public string CommandOkTitle { get; set; }

        /// <summary>
        /// Gets or sets the title that appears if the command was ok but there are warnings. If this is not set
        /// then a default title will be produced in the view
        /// </summary>
        [ReadOnly(true)]
        public string CommandWarningTitle { get; set; }

        /// <summary>
        /// Gets or sets the title that appears if the command failed. If this is not set
        /// then a default title will be produced in the view
        /// </summary>
        [ReadOnly(true)]
        public string CommandErrorTitle { get; set; }

        /// <summary>
        /// Gets the record management title. Based on the operation result and the values
        /// held in CommandOkTitle and CommandErrorTitle this will used in the view.
        /// </summary>
        public string RecordManagementTitle
        {
            get
            {
                if (this.OperationResponse.Ok && this.OperationResponse.Warn)
                {
                    if (!string.IsNullOrEmpty(this.CommandWarningTitle))
                    {
                        return this.CommandWarningTitle;
                    }

                    return "The operation was completed successfully but there are warnings.";
                }

                if (this.OperationResponse.Ok)
                {
                    if (!string.IsNullOrEmpty(this.CommandOkTitle))
                    {
                        return this.CommandOkTitle;
                    }

                    switch (this.Command)
                    {
                        case Commands.Duplicate:
                        case Commands.Update:
                        case Commands.Add:
                        case Commands.Save:
                            return "The record has been saved.";
                        case Commands.Delete:
                            return "The record has been deleted.";
                        case Commands.Validate:
                            return "The record is valid and can be saved.";
                        default:
                            return "The operation was completed successfully.";
                    }
                }

                if (!string.IsNullOrEmpty(this.CommandErrorTitle))
                {
                    return this.CommandErrorTitle;
                }

                switch (this.Command)
                {
                    case Commands.Duplicate:
                    case Commands.Update:
                    case Commands.Add:
                    case Commands.Save:
                        return "The record could not be saved.";
                    case Commands.Delete:
                        return "The record could not be deleted.";
                    case Commands.Validate:
                        return "The record is not valid.";
                    default:
                        return "The operation could not be completed.";
                }
            }
        }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Gets the application name from the web.config.
        /// </summary>
        public string AppName
        {
            get
            {
                return ConfigurationManager.AppSettings["AppName"];
            }
        }

        /// <summary>
        /// Gets or sets the form action URL.
        /// </summary>
        [ReadOnly(true)]
        public string FormAction { get; set; }

        /// <summary>
        /// Gets or sets the page name that appears below the application title.
        /// </summary>
        [ReadOnly(true)]
        public string PageName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the top area search are should be displayed.
        /// If you set this to true then ensure that you include "@section TopSearch" in your view.
        /// </summary>
        [ReadOnly(true)]
        public bool ShowTopSearch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contextual navigation should be displayed.
        /// If you set this to true then ensure that you include "@section LeftNavigation" in your view.
        /// </summary>
        [ReadOnly(true)]
        public bool ShowLeftNavigation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contextual navigation buttons should be displayed.
        /// These appear in the left on the bottom bar.
        /// If you set this to true then ensure that you include "@section LeftNavigationButtons" in your view.
        /// </summary>
        [ReadOnly(true)]
        public bool ShowLeftNavigationButtons { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show logo row.
        /// This means that the logo row will display for about 5 seconds
        /// and then disappear leaving more space on the page.
        /// </summary>
        [ReadOnly(true)]
        public bool ShowLogoRow
        {
            get
            {
                return this.showLogoRow;
            }

            set
            {
                if (!value)
                {
                    this.showLogoRow = false;
                    return;
                }

                var lastShow = Conversion.ConvertToDateTime(HttpContext.Current.Session["ShowLogoRow"]);

                this.showLogoRow = (lastShow == null) || ((DateTime.Now - (DateTime)lastShow).TotalMinutes >= 60);

                if (this.showLogoRow)
                {
                    HttpContext.Current.Session["ShowLogoRow"] = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// The message from the operation response (if not null).
        /// </summary>
        /// <returns>
        /// The list of messages from operation response.
        /// </returns>
        public List<Message> GetMessages()
        {
            return this.OperationResponse == null ? null : this.OperationResponse.Messages;
        }
    }
}