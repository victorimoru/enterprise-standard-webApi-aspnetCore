using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Utility.Common
{
    public static class Extensions
    {
        public static bool ValidateExternalDataSourcePath(this string path)
        {
            Regex regex = new Regex(@"([a-zA-Z]:(\\w+)*\\[a-zA-Z0_9]+)?.xml");
            return regex.IsMatch(path);

        }
    }
}
