using Calypso.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace Calypso
{
    public class Appdata
    {
        public List<Library> Libraries { get; set; }
        public Library ActiveLibrary { get; set; }
        public Session LastSession { get; set; }

        public Appdata(List<Library> libraries, Library activeLibrary, Session lastSession) 
        { 
            Libraries = libraries;
            ActiveLibrary = activeLibrary;
            LastSession = lastSession;
        }
    }
    
    public class TagTreeData
    {
        public Dictionary<TagNode, List<ImageData>> TagDict = new();
        public int UntaggedCount { get; set; }
        public int TotalEntries { get; set; }

        public TagTreeData(Dictionary<TagNode, List<ImageData>> tagDict, int totalEntries, int untaggedCount)
        {
            TagDict = tagDict;
            TotalEntries = totalEntries;
            UntaggedCount = untaggedCount;
        }
    }

    internal static partial class DB
    {
        public static Appdata appdata;
        private static string appdataFilePath = string.Empty;
        public static TagTreeData ActiveTagTree;

        // events
        public static event Action<Library>? OnNewLibraryLoaded;

        public static bool Init(MainWindow mainW)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string myAppFolder = Path.Combine(appDataPath, "Calypso");
            Directory.CreateDirectory(myAppFolder);
            appdataFilePath = Path.Combine(myAppFolder, "database.save");

            // exclamation is null-forgiving operator
            if (Util.TryLoad<Appdata>(appdataFilePath, out appdata) || (appdata = NewAppdata()!) != null)
            {
                mainW.LoadSession(appdata.LastSession);
                LoadLibrary(appdata.LastSession.LastActiveLibrary);
                return true;
            }

            return false;
        }
        private static Appdata? NewAppdata()
        {
            string libraryPath;

            if (PromptUserForLibrary("No existing library was found. Specify a folder now?", out libraryPath))
            {
                Library newLib = new(
                    name: Path.GetFileName(libraryPath.TrimEnd(Path.DirectorySeparatorChar)),
                    dirpath: libraryPath
                );

                var newAppdata = new Appdata(
                    libraries: new List<Library>() { newLib },
                    activeLibrary: newLib,
                    lastSession: MainWindow.i.CaptureCurrentSession(newLib)
                );

                Util.Save<Appdata>(newAppdata, appdataFilePath);
                return newAppdata;
            }

            return null; // No valid appdata could be created
        }

        private static bool PromptUserForLibrary(string message, out string libraryPath)
        {
            DialogResult result = Util.ShowInfoDialog(message);

            if (result == DialogResult.OK)
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    DialogResult resultPath = dialog.ShowDialog();
                    if (resultPath == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                    {
                        Debug.WriteLine(dialog.SelectedPath);
                        libraryPath = dialog.SelectedPath;
                        return true;
                    }
                }
            }
            // if either 'if' fails
            libraryPath = "";
            return false;
        }

        public static void LoadLibrary(Library lib)
        {
            Debug.WriteLine(lib.Dirpath);
            appdata.ActiveLibrary = lib;

            if (!Directory.Exists(lib.Dirpath))
            {
                Util.ShowInfoDialog($"The directory for library \"{lib.Name}\" at {lib.Dirpath} could not be found. This reference will now be removed. (If this library was manually moved you can re-add it via \"File > Add New Library\" at any time.");
                return;
            }

            string thumbPath = Path.Combine(lib.Dirpath, "data");
            if (!Directory.Exists(thumbPath)) Directory.CreateDirectory(thumbPath);

            string[] allImageFiles = Util.GetAllImageFilepaths(lib.Dirpath);
            List<string> unregisteredImages = allImageFiles.ToList();

            // build the unregistered image list by removing the ones that have an imagedata object already
            foreach (ImageData img in lib.ImageDataList)
            {
                if (allImageFiles.Contains(img.Filepath))
                {
                    unregisteredImages.Remove(img.Filepath);
                }
            }

            // create an imagedata wrapper for each new file
            foreach (string file in unregisteredImages)
            {
                ImageData newImage = new(file, Util.CreateThumbnail(lib, file));
                lib.ImageDataList.Add(newImage);
            }

            // if any other imagedata do not have a valid thumbnail, generate.
            foreach (ImageData img in lib.ImageDataList)
            {
                if (!File.Exists(img.ThumbnailPath))
                {
                    Util.CreateThumbnail(lib, img.Filepath);
                }
            }

            GenDictAndSaveLibrary();
            OnNewLibraryLoaded?.Invoke(lib);
        }

        public static void GenDictAndSaveLibrary()
        {
            ActiveTagTree = GenTagDictionary();
            Util.Save<Appdata>(appdata, appdataFilePath);
        }

        public static TagTreeData GenTagDictionary()
        {
            Dictionary<TagNode, List<ImageData>> tagDict = new();
            List<ImageData> untagged = new();
            List<ImageData> imgList = appdata.ActiveLibrary.ImageDataList;

            foreach (TagNode tag in appdata.ActiveLibrary.TagNodeList)
            {
                if (!tagDict.TryGetValue(tag, out var list)) // if not an entry in tagDict already
                {
                    list = new List<ImageData>();
                    tagDict[tag] = list;
                }
            }

            tagDict.TryAdd(new TagNode("all"), imgList);
            tagDict.TryAdd(new TagNode("untagged"), untagged);

            foreach (ImageData img in imgList)
            {
                if (img.Tags.Count == 0)
                {
                    untagged.Add(img);
                    continue;
                }

                foreach (TagNode node in img.Tags)
                {
                    if (!tagDict.TryGetValue(node, out var list))
                    {
                        list = new List<ImageData>();
                        tagDict[node] = list;
                    }
                    list.Add(img);
                }
            }

            return new TagTreeData(tagDict, imgList.Count, untagged.Count);
        }


    }
}
