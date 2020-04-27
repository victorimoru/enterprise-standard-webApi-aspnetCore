using System;

namespace Shared.Utility
{
    public class GuidGenerator
    {
        public static string Generate()
        {
            Guid guid = Guid.NewGuid();
            var generatedString = guid.ToString("N").Substring(0, 16).Replace("-", string.Empty).ToUpperInvariant();
            return generatedString;
        }
    }
}
