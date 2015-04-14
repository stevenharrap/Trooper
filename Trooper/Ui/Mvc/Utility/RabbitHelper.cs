//--------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Ui.Mvc.Utility
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    using Microsoft.Practices.EnterpriseLibrary.Validation;
    using System.Collections.Generic;
    using Trooper.Ui.Mvc.Rabbit;
    using System.Linq.Expressions;
    using System;
    using Trooper.Utility;

    /// <summary>
    /// General helper methods for working in MVC controllers. These may become obsolete over time
    /// as I understand more about MVC. (Steven)
    /// </summary>
    public class RabbitHelper
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

        public static IDictionary<string, string> AddAttributes(
           IDictionary<string, string> attributes,
           IDictionary<string, string> newAttributes)
        {
            if (attributes == null)
            {
                attributes = new Dictionary<string, string>();
            }

            if (newAttributes != null)
            {
                foreach (var i in newAttributes.Where(i => i.Key != null && !attributes.ContainsKey(i.Key)))
                {
                    attributes.Add(i.Key, i.Value);
                }
            }

            return attributes;
        }

        public static IDictionary<string, string> AddAttribute(IDictionary<string, string> attributes, string name, string value)
        {
            if (attributes == null)
            {
                attributes = new Dictionary<string, string>();
            }

            if (attributes.ContainsKey(name))
            {
                attributes[name] = value;
            }

            attributes.Add(name, value);

            return attributes;
        }

        public static IDictionary<string, string> AddNotEmptyAttribute(IDictionary<string, string> attributes, string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return attributes ?? new Dictionary<string, string>();
            }

            return AddAttribute(attributes, name, value);
        }

        public static string MakeAttributesList(IDictionary<string, string> attributes)
        {
            if (attributes == null)
            {
                return string.Empty;
            }

            return string.Join(" ", attributes.Select(i => string.Format("{0}=\"{1}\"", i.Key, i.Value)));
        }

        /// <summary>
        /// The add class to the classes and return the result.
        /// If the supplied classes is null then a new classes is created.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <param name="className">
        /// The class name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static IList<string> AddClass(IList<string> classes, string className)
        {
            if (classes == null)
            {
                classes = new List<string>();
            }

            if (!string.IsNullOrEmpty(className) && !classes.Contains(className))
            {
                classes.Add(className);
            }

            return classes;
        }

        /// <summary>
        /// Add the new classes to the classes and return the result.
        /// If the supplied classes is null then a new classes is created.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <param name="newClasses">
        /// The class names to add.
        /// </param>
        /// <returns>
        /// The result list.
        /// </returns>
        public static IList<string> AddClasses(IList<string> classes, IList<string> newClasses)
        {
            if (classes == null)
            {
                classes = new List<string>();
            }

            if (newClasses == null || !newClasses.Any())
            {
                return classes;
            }

			foreach (var c in newClasses)
	        {
		        classes.Add(c);
	        }

            return classes.Distinct().ToList();
        }

        /// <summary>
        /// Generates a class attribute content with the given classes. Duplicates are removed.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <returns>
        /// The class <see cref="string"/>.
        /// </returns>
        public static string MakeClassAttributeContent(IList<string> classes)
        {
            if (classes == null || !classes.Any())
            {
                return string.Empty;
            }

            return string.Join(" ", classes.Distinct());
        }

        /// <summary>
        /// Generates a class attribute with the given classes. Duplicates are removed.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <returns>
        /// The class <see cref="string"/>.
        /// </returns>
        public static string MakeClassAttribute(IList<string> classes)
        {
            if (classes == null || !classes.Any())
            {
                return "class=\"\"";
            }

            return string.Format("class=\"{0}\"", MakeClassAttributeContent(classes));
        }

        public static string GetJsBool(bool? value)
        {
            return Conversion.ConvertToBoolean(value, false).ToString().ToLower();
        }

        /// <summary>
        /// Makes an icon using the supplied icon image from the BootStrap library. 
        /// <see cref="http://getbootstrap.com/components/"/>
        /// Only use the last part of the icon name. E.g.'volume-up' from 'glyphicon-volume-up'
        /// </summary>
        /// <param name="iconImage">
        /// The icon image name.
        /// </param>
        /// <returns>
        /// The html string.
        /// </returns>
        public static string MakeIcon(string iconImage)
        {
            return string.Format("<span class=\"input-group-addon\">\n<span class=\"glyphicon glyphicon-{0}\"></span>\n</span>\n", iconImage);
        }

        /// <summary>
        /// Gets the value of the expression from the property.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <typeparam name="TValue">
        /// The data type of the expression
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static TValue GetExpressionValue<TModel, TValue>(Expression<Func<TModel, TValue>> expression, HtmlHelper<TModel> htmlHelper)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            if (metaData == null || metaData.Model == null)
            {
                return default(TValue);
            }

            var value = (TValue)metaData.Model;

            return value;
        }

        /// <summary>
        /// Converts the buttonType to a string suitable for Bootstrap classes
        /// </summary>
        /// <param name="buttonType">
        /// The button type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ButtonTypeToString(ButtonTypes buttonType)
        {
            if (buttonType == ButtonTypes.None)
            {
                return string.Empty;
            }

            return "btn-" + buttonType.ToString().ToLower();
        }

        /// <summary>
        /// Converts the placement to a string suitable for Bootstrap classes
        /// </summary>
        /// <param name="placement">
        /// The placement.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string PopoverPlacementToString(PopoverPlacements placement)
        {
            return placement.ToString().ToLower();
        }
    }
}