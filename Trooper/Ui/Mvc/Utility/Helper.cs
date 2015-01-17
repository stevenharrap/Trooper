//--------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Utility
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using Microsoft.Practices.EnterpriseLibrary.Validation;

    /// <summary>
    /// General helper methods for working in MVC controllers. These may become obsolete over time
    /// as I understand more about MVC. (Steven)
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Adds the validation errors to the model state errors.
        /// </summary>
        /// <param name="modelState">
        /// The model state.
        /// </param>
        /// <param name="results">
        /// The results.
        /// </param>
        public static void AddModelErrors(ModelStateDictionary modelState, ValidationResults results)
        {
            foreach (var error in results)
            {
                modelState.AddModelError(error.Key, error.Message);
            }
        }

        /// <summary>
        /// Instantiates a form in the MVC view using the provided class name 
        /// </summary>
        /// <example>
        /// @using (Html.BeginFormWithClassName("form-class-name")) { form markup here }
        /// </example>
        /// <param name="helper">
        /// The helper.
        /// </param>
        /// <param name="cssClassName">
        /// The CSS class name.
        /// </param>
        /// <returns>
        /// The MVC Form
        /// </returns>
        public static MvcForm BeginFormWithClassName(HtmlHelper helper, string cssClassName)
        {
            var controllerName = (string)helper.ViewContext.RouteData.Values["controller"];
            var actionName = (string)helper.ViewContext.RouteData.Values["action"];
            return helper.BeginForm(actionName, controllerName, FormMethod.Post, new { @class = cssClassName });
        }

        /// <summary>
        /// Retrieves the action that called the page
        /// </summary>
        /// <returns>
        /// The action name
        /// </returns>
        public static string GetAction()
        {
            var action = HttpContext.Current.Request.RequestContext.RouteData.Values["action"];

            return action == null ? string.Empty : action.ToString();
        }

        /// <summary>
        /// Determines if the action is the supplied action
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// True if the action of the page is the supplied action
        /// </returns>
        public static bool IsAction(string action)
        {
            return GetAction() == action;
        }

        /// <summary>
        /// Retrieves the controller that called the page
        /// </summary>
        /// <returns>
        /// The action name
        /// </returns>
        public static string GetController()
        {
            var controller = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"];

            return controller == null ? string.Empty : controller.ToString();
        }

        /// <summary>
        /// Determines if the controller is the supplied controller
        /// </summary>
        /// <param name="controller">
        /// The controller.
        /// </param>
        /// <returns>
        /// True if the action of the page is the supplied action
        /// </returns>
        public static bool IsController(string controller)
        {
            return GetController() == controller;
        }
    }
}