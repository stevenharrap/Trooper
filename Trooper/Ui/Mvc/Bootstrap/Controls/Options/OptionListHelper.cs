namespace Trooper.Ui.Mvc.Bootstrap.Controls.Options
{
    using System;
    using System.Text.RegularExpressions;

    public class OptionListHelper
    {
        public static OptionList<T?, string> FromEnum<T>(EnumParams ep = null)
            where T : struct
        {
            const string Regex = @"/([^A-Za-z0-9\.\$])|([A-Z])(?=[A-Z][a-z])|([^\-\$\.0-9])(?=\$?[0-9]+(?:\.[0-9]+)?)|([0-9])(?=[^\.0-9])|([a-z])(?=[A-Z])/g";
            var regex = new Regex(Regex);
            var result = new OptionList<T?, string>();

            ep = ep ?? new EnumParams();

            if (ep.IncludeBlank)
            {
                result.Add(null, string.IsNullOrEmpty(ep.BlankValue) ? string.Empty : ep.BlankValue);
            }

            foreach (int val in Enum.GetValues(typeof(T)))
            {
                var value = Enum.GetName(typeof(T), val);
                var key = (T?)Enum.GetValues(typeof(T)).GetValue(val);

                if (!string.IsNullOrEmpty(value))
                {
                    if (ep.SpaceCamel)
                    {
                        value = regex.Replace(value, "$2$3$4$5 ");
                    }

                    if (ep.LowerCase)
                    {
                        value = value.ToUpper();
                    }
                }

                result.Add(key, value);
            }

            return result;
        }
    }
}
