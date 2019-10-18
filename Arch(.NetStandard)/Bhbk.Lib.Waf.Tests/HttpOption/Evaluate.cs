using Bhbk.Lib.Waf.HttpOption;
using System;
using System.Reflection;

namespace Bhbk.Lib.Waf.Tests.HttpOption
{
    public class Evaluate
    {
        public static bool IsHttpsValid(HttpOptionAttribute attribute, Uri url)
        {
            return (bool)typeof(HttpOptionAttribute).GetMethod("IsHttpOptionAllowed", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(attribute, new object[] { url });
        }
    }
}
