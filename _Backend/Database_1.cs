using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal static partial class Database
    {
        public static Dictionary<string, List<ImageData>> tagIndex = new();
        public static void Search(string searchTextRaw, bool randomize, int upperLimit)
        {
            List<ImageData> results = new();
            string[] tagsInclude = { };
            string[] tagsExclude = { };

            string stripped = new string(searchTextRaw.Where(c => !char.IsWhiteSpace(c)).ToArray());
            stripped = stripped.ToLower();

            if (stripped == "all")
            {
                results = Database.loadedImageDataList;
            }
            else if (stripped == "untagged")
            {
                results = Database.dbUntaggedImageData;
            }
            else if (stripped == "randtag" || stripped == "rtag" || stripped == "randomtag")
            {
                var random = new Random();

                if (!(Database.tagDict.Count > 0)) 
                {
                    Gallery.Populate(results); // return empty
                }
                else
                {
                    string randTag = Database.tagDict.ElementAt(random.Next(Database.tagDict.Count)).Key;
                    Searchbar.Search(randTag); 
                    return;
                }

            }
            else
            {
                ParseQuery(stripped, out tagsInclude, out tagsExclude);
                results = FilterByTags(tagsInclude, tagsExclude);
            }

            if (randomize)
            {
                var rng = new Random();
                results = results.OrderBy(_ => rng.Next()).ToList();
            }

            if (upperLimit > 0 && upperLimit < results.Count)
            {
                results = results.Take(upperLimit).ToList();
            }

            Gallery.Populate(results);
        }
        public static void ParseQuery(string input, out string[] tagInclude, out string[] tagExclude)
        {
            var includeList = new List<string>();
            var excludeList = new List<string>();

            foreach (var tag in input.Split(','))
            {
                if (string.IsNullOrWhiteSpace(tag)) continue;

                if (tag.StartsWith("-"))
                    excludeList.Add(tag[1..]);
                else
                    includeList.Add(tag);
            }

            tagInclude = includeList.ToArray();
            tagExclude = excludeList.ToArray();
        }
        public static List<ImageData> FilterByTags(string[] tagInclude, string[] tagExclude)
        {
            var result = tagInclude
                .Where(tag => tagIndex.ContainsKey(tag))
                .SelectMany(tag => tagIndex[tag])
                .Distinct()
                .ToList();

            if (tagExclude.Length > 0)
            {
                var excludeSet = new HashSet<string>(tagExclude);
                result = result
                    .Where(img => img.Tags.All(t => !excludeSet.Contains(t)))
                    .ToList();
            }

            return result;
        }
        public static void GenerateTagDict(List<ImageData> allImages)
        {
            tagIndex.Clear();
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

        public static void DeleteImageData(List<ImageData> imgDataList)
        {
            foreach (ImageData imgData in imgDataList)
            {
                loadedImageDataList.Remove(imgData);

                if (File.Exists(imgData.ThumbnailPath))
                {
                    File.Delete(imgData.ThumbnailPath);
                }

                if (File.Exists(imgData.FullResPath))
                {
                    File.Delete(imgData.FullResPath);
                }
            }

            GenDictsAndSaveLibrary();
        }

    }
}
