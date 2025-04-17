using System;
using System.Text.RegularExpressions;

namespace Bhbk.Lib.Common.Validators
{
    public static class RegEx
    {
        /*
         * Simplest way to test for an invalid regular expression pattern is to try & use it then
         * catch exception if one is thrown.
         */
        public static bool IsPatternValid(string expr)
        {
            if (!string.IsNullOrEmpty(expr))
            {
                try
                {
                    Regex.Match(string.Empty, expr);
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }
}
