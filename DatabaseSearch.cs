using MetadataExtractor;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal static class DatabaseSearch
    {

        static Dictionary<string, List<ImageData>> tagIndex = new();
        public static Dictionary<string, List<ImageData>> GenerateTagDict(List<ImageData> allImages) 
        {

            foreach (var image in allImages)
            {
                foreach (var tag in image.Tags)
                {
                    if (!tagIndex.TryGetValue(tag, out var list))
                    {
                        list = new List<ImageData>();
                        tagIndex[tag] = list;
                    }
                    list.Add(image);
                }
            }

            return tagIndex;

        }

        public static Dictionary<string, ImageData> GenerateFilenameDict(List<ImageData> allImages)
        {
            var filenameIndex = new Dictionary<string, ImageData>(StringComparer.OrdinalIgnoreCase);

            foreach (var image in allImages)
            {
                if (!filenameIndex.ContainsKey(image.Filename))
                    filenameIndex[image.Filename] = image;
            }

            return filenameIndex;
        }


        public static List<ImageData> Fetch(
            string[] tagInclude, 
            string[] tagExclude, 
            bool returnRandom, 
            int resultsUpperLimit = 0
            )
        {
            var result = tagInclude
                .Where(tag => tagIndex.ContainsKey(tag))
                .SelectMany(tag => tagIndex[tag])
                .Distinct()
                .ToList();

            if (tagExclude != null && tagExclude.Length > 0)
            {
                var excludeSet = new HashSet<string>(tagExclude);
                result = result
                    .Where(img => img.Tags.All(t => !excludeSet.Contains(t)))
                    .ToList();
            }

            if (returnRandom)
            {
                var rng = new Random();
                result = result.OrderBy(_ => rng.Next()).ToList();
            }

            if (resultsUpperLimit > 0 && resultsUpperLimit < result.Count)
            {
                result = result.Take(resultsUpperLimit).ToList();
            }

            return result;
        }


    }
}
