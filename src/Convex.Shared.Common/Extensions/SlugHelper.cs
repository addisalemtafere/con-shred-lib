using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Convex.Shared.Common.Extensions
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            input = input.ToLowerInvariant();

            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(input);
            input = System.Text.Encoding.ASCII.GetString(bytes);

            input = Regex.Replace(input, @"\s+", "-");
            input = Regex.Replace(input, @"[^a-z0-9\-]", "");
            input = input.Trim('-');

            return input;
        }

        public static string GenerateSlugWithGuid(string baseSlug)
        {
            var guid = Guid.NewGuid().ToString();
            return $"{baseSlug}-{guid}";
        }
    }
}
