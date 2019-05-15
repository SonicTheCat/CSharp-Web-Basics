namespace SIS.HTTP.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException($"{nameof(str)} cannot be null!"); 
            }

            return str.ToLower().Replace(str[0].ToString(), str[0].ToString().ToUpper());
        }
    }
}