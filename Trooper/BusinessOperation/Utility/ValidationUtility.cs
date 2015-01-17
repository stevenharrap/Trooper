//--------------------------------------------------------------------------------------
// <copyright file="ValidationUtility.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation.Utility
{
    using System;
    using System.Data.SqlTypes;
    using System.Net.Mail;

    using Microsoft.Practices.EnterpriseLibrary.Validation;

    /// <summary>
    /// Provides constants and utility methods to assist with validation of entities.
    /// </summary>
    public class ValidationUtility
    {
        /// <summary>
        /// Provides a MessageTemplate string that results in something like 'FirstName has to be between 1 and 20 characters.'
        /// Suitable for System.ComponentModel.DataAnnotations.StringLengthAttribute
        /// </summary>
        public const string StringOutsideMsg = @"{0} has to be between {2} and {1} characters";

        /// <summary>
        /// Provides a message template string that results in something like 'Age has to be between 1 and 99.'
        /// </summary>
        public const string NumberOutsideMsg = @"{1} has to be between {3} and {5}.";

        /// <summary>
        /// Provides a MessageTemplate suitable for the regular expression validator. 
        /// [RegexValidator(PropertyDecoration.NoHtmlRegexp, System.Text.RegularExpressions.RegexOptions.IgnoreCase, Negated = true, MessageTemplate = PropertyDecoration.NoHtmlMsg)]
        /// <see cref="NoHtmlRegexp" />
        /// </summary>
        public const string NoHtmlMsg = @"{1} cannot contain any html markup.";

        /// <summary>
        /// Provides a regular expression value suitable for use with a regular expression validator that catches html markup
        /// Should match &lt;b&gt;&lt;/b&gt; or &lt;b&gt; or &amp;lt;
        /// <see cref="NoHtmlMsg" />
        /// </summary>
        public const string NoHtmlRegexp = @"<[^>]+>|<.*?>|&[^\s;]+;";

        /// <summary>
        /// Provides a MessageTemplate suitable for the regular expression validator.
        /// </summary>
        public const string BadCharactersMsg = @"{1} contains invalid characters.";

        /// <summary>
        /// Provides a regular expression value suitable for use with a regular expression validator that catches characters that are not A to Z and 0 to 9
        /// <see cref="NoHtmlMsg" />
        /// </summary>
        public const string UppercaseAlphanumericRegexp = @"^([A-Z0-9]+)$";

        /// <summary>
        /// Provides a regular expression value suitable for use with a regular expression validator that catches characters that are not a to z,  A to Z and 0 to 9
        /// <see cref="NoHtmlMsg" />
        /// </summary>
        public const string AlphanumericRegexp = @"^([a-zA-Z0-9]+)$";

        /// <summary>
        /// Provides a regular expression value suitable for use with a regular expression validator that catches character that are not 0 to 9
        /// <see cref="NoHtmlMsg" />
        /// </summary>
        public const string NumericRegexp = @"^([0-9]+)$";

        /// <summary>
        /// The is within the range of dates which is acceptable for SQL. If not then add a validation error.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <param name="target">
        /// The target to which the property belongs
        /// </param>
        /// <param name="validationResults">
        /// The validation results to populate
        /// </param>
        /// <param name="propertyName">
        /// The name of date property
        /// </param>
        /// <returns>
        /// True if the date within range.
        /// </returns>
        public static bool IsDateOk(
            DateTime? dateTime, 
            object target, 
            ValidationResults validationResults, 
            string propertyName = null)
        {
            if (!IsDateOk(dateTime))
            {
                validationResults.AddResult(
                    new ValidationResult(
                        string.Format("The {0} is required.", propertyName ?? "value"),
                        target,
                        propertyName,
                        null,
                        null));

                return false;
            }

            return true;
        }

        /// <summary>
        /// The is within the range of dates which is acceptable for SQL
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// True if the date within range.
        /// </returns>
        public static bool IsDateOk(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == SqlDateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The is within the range of dates which is acceptable for SQL
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// True if the date within range.
        /// </returns>
        public static bool IsDateOk(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return true;
            }

            if (dateTime == DateTime.MinValue || (DateTime)dateTime == SqlDateTime.MinValue)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether [is valid email address] [the specified email address].
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns><c>true</c> if [is valid email address] [the specified email address]; otherwise, <c>false</c>.</returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            bool isValid;

            try
            {
                // ReSharper disable UnusedVariable
                var mailAddress = new MailAddress(emailAddress);
                // ReSharper restore UnusedVariable
                isValid = true;
            }
            catch (FormatException)
            {
                isValid = false;
            }

            return isValid;
        }
    }
}