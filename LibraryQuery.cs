using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal class LibraryQuery
    {
        public static void ParseQuery(string input, out string[] tagInclude, out string[] tagExclude)
        {
            var includeList = new List<string>();
            var excludeList = new List<string>();

            foreach (var rawTag in input.Split(','))
            {
                var tag = rawTag.Trim();
                if (string.IsNullOrWhiteSpace(tag)) continue;

                if (tag.StartsWith("-"))
                    excludeList.Add(tag[1..].Trim());
                else
                    includeList.Add(tag);
            }

            tagInclude = includeList.ToArray();
            tagExclude = excludeList.ToArray();
        }

    }
}
