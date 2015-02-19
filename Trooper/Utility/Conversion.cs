//--------------------------------------------------------------------------------------
// <copyright file="Conversion.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.SqlTypes;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Utility static type conversion methods.
    /// </summary>
    public static class Conversion
    {
        /// <summary>
        /// Convert the given object to a <see cref="Guid"/> or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to <see cref="Guid"/>.</returns>
        public static Guid ConvertToGuid(object input, Guid defaultValue)
        {
            var result = defaultValue;

            if (input is string)
            {
                Guid.TryParse(input as string, out result);
            }
            else
            {
                try
                {
                    return (Guid)input;
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the given object to a <see cref="Guid"/> or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to <see cref="Guid"/> or null</returns>
        public static Guid? ConvertToGuid(object input)
        {
            if (input is string)
            {
                Guid result;

                if (!Guid.TryParse(input as string, out result))
                {
                    return null;
                }

                return result;
            }

            try
            {
                return (Guid)input;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the given object to a boolean or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to boolean.</returns>
        public static bool ConvertToBoolean(object input, bool defaultValue)
        {
            var result = defaultValue;

            if (input is string)
            {
                bool.TryParse(input as string, out result);
            }
            else
            {
                try
                {
                    return Convert.ToBoolean(input);
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the given object to a boolean or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to boolean or null</returns>
        public static bool? ConvertToBoolean(object input)
        {
            if (input is string)
            {
                bool result;

                if (!bool.TryParse(input as string, out result))
                {
                    return null;
                }

                return result;
            }

            try
            {
                return Convert.ToBoolean(input);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the given object to a double or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to double</returns>
        public static double ConvertToDouble(object input, double defaultValue)
        {
            var result = defaultValue;

            if (input is string)
            {
                double.TryParse(input as string, out result);
            }
            else
            {
                try
                {
                    return Convert.ToDouble(input);
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the given object to a double or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to double or null</returns>
        public static double? ConvertToDouble(object input)
        {
            if (input is string)
            {
                double result;

                if (!double.TryParse(input as string, out result))
                {
                    return null;
                }

                return result;
            }

            try
            {
                return Convert.ToDouble(input);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the given object to a float or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to float</returns>
        public static double ConvertToFloat(object input, float defaultValue)
        {
            var result = defaultValue;

            if (input is string)
            {
                float.TryParse(input as string, out result);
            }
            else
            {
                try
                {
                    return (float)input;
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the given object to a float or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to double or null</returns>
        public static float? ConvertToFloat(object input)
        {
            if (input is string)
            {
                float result;

                if (!float.TryParse(input as string, out result))
                {
                    return null;
                }

                return result;
            }

            try
            {
                return (float)input;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the given object to a 32-bit integer or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to 32-bit integer.</returns>
        public static int ConvertToInt32(object input, int defaultValue)
        {
            var result = defaultValue;

            if (input is string)
            {
                int.TryParse(input as string, out result);
            }
            else
            {
                try
                {
                    return Convert.ToInt32(input);
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the given object to a integer or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to 32-bit integer or null</returns>
        public static int? ConvertToInt32(object input)
        {
            if (input is string)
            {
                int result;

                if (!int.TryParse(input as string, out result))
                {
                    return null;
                }

                return result;
            }

            try
            {
                return Convert.ToInt32(input);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the given object to a 32-bit integer or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to a 32-bit integer.</returns>
        public static int ConvertToInt(object input, int defaultValue)
        {
            return ConvertToInt32(input, defaultValue);
        }

        /// <summary>
        /// Convert the given object to a 32-bit integer or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to a 32-bit integer or null.</returns>
        public static int? ConvertToInt(object input)
        {
            return ConvertToInt32(input);
        }

        /// <summary>
        /// Convert the given object to a 16-bit integer or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to 16-bit integer (short).</returns>
        public static short ConvertToInt16(object input, short defaultValue)
        {
            var result = defaultValue;

            if (input is string)
            {
                short.TryParse(input as string, out result);
            }
            else
            {
                try
                {
                    return Convert.ToInt16(input);
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the given object to a short or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to 16-bit integer or null</returns>
        public static short? ConvertToInt16(object input)
        {
            if (input is string)
            {
                short result;

                if (!short.TryParse(input as string, out result))
                {
                    return null;
                }

                return result;
            }

            try
            {
                return Convert.ToInt16(input);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the given object to a byte or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to byte.</returns>
        public static short ConvertToByte(object input, byte defaultValue)
        {
            var result = defaultValue;

            if (input is string)
            {
                byte.TryParse(input as string, out result);
            }
            else
            {
                try
                {
                    return Convert.ToByte(input);
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert the given object to a byte or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to byte or null</returns>
        public static byte? ConvertToByte(object input)
        {
            if (input is string)
            {
                byte result;

                if (!byte.TryParse(input as string, out result))
                {
                    return null;
                }

                return result;
            }

            try
            {
                return Convert.ToByte(input);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the given object to a 16-bit integer or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The value to return if the input cannot be converted</param>
        /// <returns>Object converted to 16-bit integer (short).</returns>
        public static short ConvertToShort(object input, short defaultValue)
        {
            return ConvertToInt16(input, defaultValue);
        }

        /// <summary>
        /// Convert the given object to a 16-bit integer or (if it fails) will return null.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Object converted to 16-bit integer (short) or null.</returns>
        public static short? ConvertToShort(object input)
        {
            return ConvertToInt16(input);
        }

        /// <summary>
        /// Convert the given object to a decimal or (if it fails) will return the default value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Object converted to decimal</returns>
        public static decimal ConvertToDecimal(object input, decimal defaultValue)
        {
            var result = defaultValue;

            if (input is string)
            {
                decimal.TryParse(input as string, out result);
            }
            else
            {
                try
                {
                    return Convert.ToDecimal(input);
                }
                catch
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the input to decimal.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Decimal equivalent or null.</returns>
        public static decimal? ConvertToDecimal(object input)
        {
            if (input is string)
            {
                decimal result;

                if (!decimal.TryParse(input as string, out result))
                {
                    return null;
                }

                return result;
            }

            try
            {
                return Convert.ToDecimal(input);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert the given object to a DateTime of (if it fails) will return the default value.
        /// The object can be a string or DateTime.
        /// The returned DateTime must also conform to ValidationHelper.IsDateMin or the default
        /// value will be returned.
        /// </summary>
        /// <param name="input">The input - it can be a string or DateTime.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Object converted to DateTime.</returns>
        public static DateTime? ConvertToDateTime(object input, DateTime? defaultValue)
        {
            var s = input as string;

            if (s != null)
            {
                try
                {
                    DateTime result;

                    if (!DateTime.TryParse(s, out result))
                    {
                        return defaultValue;
                    }

                    return IsDateMin(result) ? defaultValue : result;
                }
                catch
                {
                    return defaultValue;
                }
            }

            if (input is DateTime && IsDateMin((DateTime)input))
            {
                return defaultValue;
            }

            if (input is DateTime)
            {
                return (DateTime)input;
            }

            return defaultValue;
        }

        /// <summary>
        /// Converts the input to DateTime.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>DateTime equivalent or null.</returns>
        public static DateTime? ConvertToDateTime(object input)
        {
            return ConvertToDateTime(input, null);
        }

        /// <summary>
        /// Attempts to convert the given string representation of the enumeration to its actual enumeration.
        /// </summary>
        /// <typeparam name="T">The enumerated type to convert to.</typeparam>
        /// <param name="value">The string representation.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <returns>The enumerated type.</returns>
        /// <remarks>If conversion fails then the default value is returned.</remarks>
        public static T ConvertToEnum<T>(string value, T defaultValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Attempts to convert the given string representation of the enumeration to its actual enumeration.
        /// </summary>
        /// <typeparam name="T">The enumerated type to convert to.</typeparam>
        /// <param name="value">The string representation.</param>
        /// <returns>The enumerated type.</returns>
        /// <remarks>If conversion fails then null is returned.</remarks>
        public static T? ConvertToNullableEnum<T>(string value) where T : struct
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts the value to a string or returns the defaultValue if conversion
        /// cannot occur or value is a null in the first place.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="defaultValue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The converted value or defaultValue
        /// </returns>
        public static string ConvertToString(object value, string defaultValue = null)
        {
            if (value == null)
            {
                return defaultValue;
            }

            try
            {
                return value.ToString();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Converts the integer? to its string value using the <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="cultureInfo">The culture info. By default this is <see cref="CultureInfo.InvariantCulture"/></param>
        /// <param name="defaultValue">The default value. By default this <see cref="string.Empty"/></param>
        /// <returns>
        /// Returns the string representation or <paramref name="defaultValue"/> if conversion is not possible</returns>
        public static string NullableToString(int? value, CultureInfo cultureInfo = null, string defaultValue = null)
        {
            return value == null ? defaultValue ?? string.Empty : ((int)value).ToString(cultureInfo ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the short? to its string value using the <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="cultureInfo">The culture info. By default this is <see cref="CultureInfo.InvariantCulture"/></param>
        /// <param name="defaultValue">The default value. By default this <see cref="string.Empty"/></param>
        /// <returns>
        /// Returns the string representation or <paramref name="defaultValue"/> if conversion is not possible</returns>
        public static string NullableToString(short? value, CultureInfo cultureInfo = null, string defaultValue = null)
        {
            return value == null ? defaultValue ?? string.Empty : ((short)value).ToString(cultureInfo ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the float? to its string value using the <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="cultureInfo">The culture info. By default this is <see cref="CultureInfo.InvariantCulture"/></param>
        /// <param name="defaultValue">The default value. By default this <see cref="string.Empty"/></param>
        /// <returns>
        /// Returns the string representation or <paramref name="defaultValue"/> if conversion is not possible</returns>
        public static string NullableToString(float? value, CultureInfo cultureInfo = null, string defaultValue = null)
        {
            return value == null ? defaultValue ?? string.Empty : ((float)value).ToString(cultureInfo ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the double? to its string value using the <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="cultureInfo">The culture info. By default this is <see cref="CultureInfo.InvariantCulture"/></param>
        /// <param name="defaultValue">The default value. By default this <see cref="string.Empty"/></param>
        /// <returns>
        /// Returns the string representation or <paramref name="defaultValue"/> if conversion is not possible</returns>
        public static string NullableToString(double? value, CultureInfo cultureInfo = null, string defaultValue = null)
        {
            return value == null ? defaultValue ?? string.Empty : ((double)value).ToString(cultureInfo ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the long? to its string value using the <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="cultureInfo">The culture info. By default this is <see cref="CultureInfo.InvariantCulture"/></param>
        /// <param name="defaultValue">The default value. By default this <see cref="string.Empty"/></param>
        /// <returns>
        /// Returns the string representation or <paramref name="defaultValue"/> if conversion is not possible</returns>
        public static string NullableToString(long? value, CultureInfo cultureInfo = null, string defaultValue = null)
        {
            return value == null ? defaultValue ?? string.Empty : ((long)value).ToString(cultureInfo ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the boolean? to its string value using the <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="cultureInfo">The culture info. By default this is <see cref="CultureInfo.InvariantCulture"/></param>
        /// <param name="defaultValue">The default value. By default this <see cref="string.Empty"/></param>
        /// <returns>
        /// Returns the string representation or <paramref name="defaultValue"/> if conversion is not possible</returns>
        public static string NullableToString(bool? value, CultureInfo cultureInfo = null, string defaultValue = null)
        {
            return value == null ? defaultValue ?? string.Empty : ((bool)value).ToString(cultureInfo ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the DateTime? to its string value. Uses <code>"d/MM/yyyy h:mm:ss tt"</code> or 
        /// <see cref="CultureInfo.InvariantCulture"/> if the <paramref name="cultureInfo"/> is supplied.
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="cultureInfo">The culture info. Uses <code>"d/MM/yyyy h:mm:ss tt"</code> if not supplied.</param>
        /// <param name="defaultValue">The default value. By default this <see cref="string.Empty"/></param>
        /// <returns>
        /// Returns the string representation or <paramref name="defaultValue"/> if conversion is not possible</returns>
        public static string NullableToString(DateTime? value, CultureInfo cultureInfo = null, string defaultValue = null)
        {
            if (cultureInfo == null)
            {
                return value == null ? defaultValue ?? string.Empty : ((DateTime)value).ToString("d/MM/yyyy h:mm:ss tt");
            }

            return value == null ? defaultValue ?? string.Empty : ((DateTime)value).ToString(cultureInfo);
        }

        /// <summary>
        /// Attempts to convert the given parameter which should be an enumeration to a list of string. Each
        /// item in the list is the string equivalent of the item in the enumeration. Useful for quickly
        /// binding an enumeration to a dropdown control for example
        /// </summary>
        /// <param name="includeEmptyItem">
        /// The include an Empty Item. By default this does not occur
        /// </param>
        /// <typeparam name="T">
        /// Enumerated type
        /// </typeparam>
        /// <returns>
        /// List of string
        /// </returns>
        public static List<string> ConvertEnumToList<T>(bool includeEmptyItem = false)
        {
            try
            {
                var result = (from int val in Enum.GetValues(typeof(T)) select Enum.GetName(typeof(T), val)).ToList();

                if (includeEmptyItem)
                {
                    result.Insert(0, string.Empty);
                }

                return result;
            }
            catch
            {
                return new List<string>();
            }
        }

		/// <summary>
		/// Converts the Enum type to a list of its values.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>List of T values</returns>
		public static List<T> ConvertEnumToList<T>()
		{
			try
			{
				return Enum.GetValues(typeof (T)).Cast<T>().ToList();
			}
			catch
			{
				return new List<T>();
			}
		}

        /// <summary>
        /// Converts to date.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <returns>DateTime converted from String</returns>
        public static DateTime? ConvertToDate(string dateString)
        {
            try
            {
                var supportedFormats = new[] { "dd/MM/yyyy", "dd/MM/yy", "ddMMMyyyy", "dMMMyyyy", "d/M/yyyy" };

                return DateTime.ParseExact(dateString, supportedFormats, CultureInfo.CurrentCulture, DateTimeStyles.None);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts to date.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="supportedFormats">The supported formats.</param>
        /// <returns>DateTime converted from String</returns>
        /// <remarks>Useful to lock down the exact supported formats required.</remarks>
        public static DateTime? ConvertToDate(string dateString, string[] supportedFormats)
        {
            try
            {
                return DateTime.ParseExact(dateString, supportedFormats, CultureInfo.CurrentCulture, DateTimeStyles.None);
            }
            catch (Exception)
            {
                return null;
            }
        }               

        /// <summary>
        /// Determines whether [is date min] [the specified date time].
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        ///     <c>true</c> if [is date min] [the specified date time]; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     <para>DateTime can represent any time between 12:00:00 AM 1/1/0001 and 11:59:59 PM 12/31/9999,
        /// to the accuracy of 100 nanoseconds.</para>
        ///     <para>SqlDateTime represents the date and time data ranging in value from January 1, 1753 to December
        /// 31, 9999 to an accuracy of 3.33 milliseconds to be stored in or retrieved from a database.
        /// The System.Data.SqlTypes.SqlDateTime structure has a different underlying data structure from its
        /// corresponding .NET Framework type, System.Data.SqlTypes.SqlDateTime actually stores the relative
        /// difference to 00:00:00 AM 1/1/1900. Therefore, a conversion from "00:00:00 AM 1/1/1900" to an integer
        /// will return 0.</para>
        ///     <para>Sending an empty ASP.NET web form control value to database defaults to "1/1/1900 12:00:00 AM". 
        /// See the conversion notes above.</para>
        ///     <para>Maybe. It's complicated. See above.</para>
        /// </remarks>
        private static bool IsDateMin(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue || dateTime == SqlDateTime.MinValue)
            {
                return true;
            }

            return false;
        }
    }
}
