using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal static partial class DB
    {
        public static Dictionary<string, List<ImageData>> tagIndex = new();
        #region searching
        public static void Search(string searchTextRaw, bool randomize, int upperLimit)
        {
        //    if (ActiveTagTree == null) return;

        //    List<ImageData> results = new();
        //    string[] tagsInclude = { };
        //    string[] tagsExclude = { };

        //    string stripped = new string(searchTextRaw.Where(c => !char.IsWhiteSpace(c)).ToArray());
        //    stripped = stripped.ToLower();

        //    if (stripped == "randtag" || stripped == "rtag" || stripped == "randomtag")
        //    {
        //        var random = new Random();

        //        if (!(ActiveTagTree.TagDict.Count < 3)) // all tagdicts have 2 elements by default, all and untagged
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            // implement eventually maybe
        //            return;
        //        }

        //    }
        //    else
        //    {
        //        foreach (var kvp in ActiveTagTree.TagDict)
        //        {
        //            if (kvp.Key.Name == stripped)
        //            {
        //                results = kvp.Value;
        //                break;
        //            }
        //        }
        //    }

        //    if (randomize)
        //    {
        //        var rng = new Random();
        //        results = results.OrderBy(_ => rng.Next()).ToList();
        //    }

        //    if (upperLimit > 0 && upperLimit < results.Count)
        //    {
        //        results = results.Take(upperLimit).ToList();
        //    }

        //    Gallery.Populate(results);
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

            //if (tagExclude.Length > 0)
            //{
            //    var excludeSet = new HashSet<string>(tagExclude);
            //    result = result
            //        .Where(img => img.Tags.All(t => !excludeSet.Contains(t)))
            //        .ToList();
            //}

            return result;
        }
        public static void GenerateTagDict(List<ImageData> allImages)
        {
            //tagIndex.Clear();
            //foreach (var image in allImages)
            //{
            //    foreach (var tag in image.Tags)
            //    {
            //        if (!tagIndex.TryGetValue(tag, out var list))
            //        {
            //            list = new List<ImageData>();
            //            tagIndex[tag] = list;
            //        }
            //        list.Add(image);
            //    }
            //}

        }
        #endregion

        private static void Save()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Formatting = Formatting.Indented
                };

                string json = JsonConvert.SerializeObject(appdata, settings);
                File.WriteAllText(appdataFilePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to save appdata: {ex.Message}");
            }
        }


        private static bool Load()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };

                string json = File.ReadAllText(appdataFilePath);
                appdata = JsonConvert.DeserializeObject<Appdata>(json, settings);

                return appdata != null;
            }
            catch (Exception ex)
            {
                Util.ShowErrorDialog($"Failed to load appdata: {ex.Message}");
                return false;
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

        public static void OnClose(Session session)
        {
            appdata.LastSession = session;
            Save();
        }

        #region miscellaneous helpers
        public static void DeleteImageData(List<ImageData> imgDataList)
        {
            //foreach (ImageData imgData in imgDataList)
            //{
            //    appdata.ActiveLibrary.ImageDataList.Remove(imgData);

            //    if (File.Exists(imgData.ThumbnailPath))
            //    {
            //        File.Delete(imgData.ThumbnailPath);
            //    }

            //    if (File.Exists(imgData.Filepath))
            //    {
            //        File.Delete(imgData.Filepath);
            //    }
            //}

            //GenTagDictAndSaveLibrary();
        }
        public static TagNode? StringToTagNode(string tag)
        {
            foreach (TagNode node in appdata.ActiveLibrary.tagTree.tagNodes)
            {
                if (node.Name == tag)
                {
                    return node;
                }
            }
            // otherwise
            return null;
        }
        public static void RemoveLibrary(Library library) { }
        public static void AddNewLibrary()
        {
            string libPath;
            if (PromptUserForLibrary("Add new library?", out libPath))
            {
                // if there is a library already registered with that directory, simply open that existing library
                foreach (Library lib in appdata.Libraries)
                {
                    if (lib.Dirpath == libPath)
                    {
                        LoadLibrary(lib);
                        return;
                    }
                }

                Library newLib = new Library(
                    name: Path.GetFileName(libPath.TrimEnd(Path.DirectorySeparatorChar)),
                    dirpath: libPath
                );

                appdata.Libraries.Add(newLib);
                LoadLibrary(newLib);
            }
        }
        public static void OpenCurrentLibrarySourceFolder()
        {
            if (appdata.ActiveLibrary == null) return;

            Process.Start(new ProcessStartInfo
            {
                FileName = appdata.ActiveLibrary.Dirpath,
                UseShellExecute = true
            });
        }

        public static List<ImageData> AddFilesToLibrary(string[] filepaths)
        {
            //string targetDir = appdata.ActiveLibrary.Dirpath;
            //string thumbSavePath = string.Empty;
            //string destPath = string.Empty;
            //List<ImageData> newImages = new();

            //foreach (string fp in filepaths)
            //{
            //    // copy to main folder
            //    string filename = string.Empty;
            //    string ext = Path.GetExtension(fp).ToLower();
            //    if (ext is ".jpg" or ".jpeg" or ".png" or ".bmp" or ".gif")
            //    {
            //        filename = Path.GetFileName(fp);
            //        destPath = Path.Combine(targetDir, filename);

            //        if (!File.Exists(destPath))
            //        {
            //            File.Copy(fp, destPath, overwrite: false);
            //            thumbSavePath = Util.CreateThumbnail(appdata.ActiveLibrary, destPath);
            //        }
            //        else
            //        {
            //            Util.ShowErrorDialog($"A file named {filename} already exists in {targetDir}!");
            //            return null;
            //        }
            //    }

            //    if (thumbSavePath != string.Empty && filename != string.Empty)
            //    {
            //        ImageData newImageData = new(destPath, thumbSavePath);
            //        newImages.Add(newImageData);
            //        appdata.ActiveLibrary.ImageDataList.Add(newImageData);
            //    }
            //}

            GenTagDictAndSaveLibrary();
            return new List<ImageData>();
        }
        #endregion
    }
}
