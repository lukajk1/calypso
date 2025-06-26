using Calypso.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace Calypso
{
    internal static partial class DBUtility
    {
        public static List<LibraryData> Libraries = new();
        public static LibraryData? ActiveLibrary;

        private static string jsonLibraryList; // keep this, no need to move this field out of here.
        private static string appdataDirPath;
        private static string jsonActiveLibraryDB;

        // specific to active library? keep?
        public static List<ImageData> loadedImageDataList = new();
        public static List<ImageData> dbUntaggedImageData = new();
        public static Dictionary<string, int> tagDict = new();

        // events
        public static event Action<LibraryData> OnNewLibraryLoaded;
        public static void Init(out bool jsonExists)
        {
            appdataDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CalypsoManager");
            jsonLibraryList = Path.Combine(appdataDirPath, "libraries.json");

            bool appdataExists = Directory.Exists(appdataDirPath);
            if (!appdataExists) Directory.CreateDirectory(appdataDirPath);

            jsonExists = File.Exists(jsonLibraryList);
        }
        
        public static bool ReadDBJson()
        {
            if (Util.TryDeserializeFromFile<List<LibraryData>>(jsonLibraryList, out Libraries))
            {
                ActiveLibrary = Libraries[0];

                foreach (LibraryData lib in Libraries)
                {
                    if (lib.LastActive)
                    {
                        ActiveLibrary = lib;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }

        }
        public static bool CreateDBJson()
        {
            string libraryPath;

            if (PromptUserForLibrary("No library reference was found. Specify a directory now?", out libraryPath))
            {
                ActiveLibrary = new LibraryData()
                {
                    Name = Path.GetFileName(libraryPath.TrimEnd(Path.DirectorySeparatorChar)),
                    Dirpath = libraryPath,
                    LastActive = true
                };

                Libraries.Add(ActiveLibrary); // assume Libraries is null
                Util.SerializeToFile<List<LibraryData>>(Libraries, jsonLibraryList);

                return true;
            }
            else
            {
                return false;
            }

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
                        libraryPath = dialog.SelectedPath;
                        return true;
                    }
                }
            }
            // if either 'if' fails
            libraryPath = "";
            return false;
        }

        public static void LoadActiveLibrary()
        {
            LoadLibrary(ActiveLibrary);
        }
        public static void LoadLibrary(LibraryData lib)
        {
            ActiveLibrary = lib;

            string thumbPath = Path.Combine(lib.Dirpath, "thumbnails");
            jsonActiveLibraryDB = Path.Combine(lib.Dirpath, "database.json");

            List<string> unregisteredImages = new();
            string[] allImageFiles = Util.GetAllImageFilepaths(lib.Dirpath);
            loadedImageDataList.Clear();

            if (!Directory.Exists(thumbPath)) Directory.CreateDirectory(thumbPath);

            if (File.Exists(jsonActiveLibraryDB) && Util.TryDeserializeFromFile<List<ImageData>>(jsonActiveLibraryDB, out loadedImageDataList))
            {
                Dictionary<string, ImageData> fileNameDict = DBUtility.GenerateFilenameDict(loadedImageDataList);

                foreach (string filepath in allImageFiles)
                {
                    if (!fileNameDict.ContainsKey(Path.GetFileName(filepath)))
                    {
                        unregisteredImages.Add(filepath); // add all filenames that aren't registered in the database to a list
                    }
                }
            }
            else if (!File.Exists(jsonActiveLibraryDB) || 
                Util.TryDeserializeFromFile<List<ImageData>>(jsonActiveLibraryDB, out loadedImageDataList) == false)
            {
                foreach (string file in allImageFiles)
                {
                    unregisteredImages.Add(file);
                }
            }

            foreach (string filepath in unregisteredImages)
            { 
                string thumbSavePath = CreateThumbnail(filepath); 
                ImageData newImageData = new ImageData(filepath, thumbSavePath, Path.GetFileName(filepath));
                loadedImageDataList.Add(newImageData);
            }

            GenDictsAndSaveLibrary();
            OnNewLibraryLoaded?.Invoke(lib);
        }

        private static string CreateThumbnail(string filepath)
        {
            string originalFilename = Path.GetFileName(filepath);
            string thumbDir = Path.Combine(ActiveLibrary.Dirpath, "thumbnails");
            string thumbSavePath = Path.Combine(thumbDir, "thumb_" + originalFilename);

            if (File.Exists(thumbSavePath))
            {
                return thumbSavePath;
            }
            else
            {
                using Image thumb = Util.CreateThumbnail(filepath, GlobalValues.ThumbnailHeight);
                ImageFormat format = Util.GetImageFormatFromExtension(thumbSavePath);
                thumb.Save(thumbSavePath, format);

                return thumbSavePath;
            }
        }

        public static void AddNewLibrary()
        {
            string libPath;
            if (PromptUserForLibrary("Add new library?", out libPath))
            {
                LibraryData newLib = new LibraryData()
                {
                    Name = Path.GetFileName(libPath.TrimEnd(Path.DirectorySeparatorChar)),
                    Dirpath = libPath,
                    LastActive = true,
                };

                foreach (LibraryData libs in Libraries) 
                {
                    libs.LastActive = false;
                }

                Libraries.Add(newLib);
                Util.SerializeToFile<List<LibraryData>>(Libraries, jsonLibraryList);

                LoadLibrary(newLib);
            }
        }

        public static void GenDictsAndSaveLibrary()
        {
            BuildTagDict();
            DBUtility.GenerateTagDict(loadedImageDataList);
            Util.SerializeToFile<List<ImageData>>(loadedImageDataList, jsonActiveLibraryDB);
        }

        public static List<ImageData> AddFilesToLibrary(string[] filepaths)
        {
            string targetDir = ActiveLibrary.Dirpath;
            string thumbSavePath = string.Empty;
            string destPath = string.Empty;
            List<ImageData> newImages = new();

            foreach (string fp in filepaths)
            {
                // copy to main folder
                string filename = string.Empty;
                string ext = Path.GetExtension(fp).ToLower();
                if (ext is ".jpg" or ".jpeg" or ".png" or ".bmp" or ".gif")
                {
                    filename = Path.GetFileName(fp);
                    destPath = Path.Combine(targetDir, filename);

                    if (!File.Exists(destPath))
                    {
                        File.Copy(fp, destPath, overwrite: false);
                        thumbSavePath = CreateThumbnail(destPath);
                    }
                    else
                    {
                        Util.ShowErrorDialog($"A file named {filename} already exists in {targetDir}!");
                        return null;
                    }
                }

                if (thumbSavePath != string.Empty && filename != string.Empty)
                {
                    ImageData newImageData = new ImageData(destPath, thumbSavePath, filename);
                    newImages.Add(newImageData);
                    loadedImageDataList.Add(newImageData);
                }
            }

            GenDictsAndSaveLibrary();
            return newImages;
        }

        public static void OpenCurrentLibrarySourceFolder()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = ActiveLibrary.Dirpath,
                UseShellExecute = true
            });
        }
        public static void RemoveTag(string tag)
        {
            if (!DBUtility.tagIndex.ContainsKey(tag)) return;

            foreach (ImageData imgData in DBUtility.tagIndex[tag])
            {
                imgData.Tags.Remove(tag);
            }
            BuildTagDict();
        }
        private static void BuildTagDict()
        {
            dbUntaggedImageData.Clear();
            tagDict.Clear();

            int totalEntriesCount = loadedImageDataList.Count;

            foreach (ImageData entry in loadedImageDataList)
            {
                if (entry.Tags.Count == 0)
                {
                    dbUntaggedImageData.Add(entry);
                    continue;
                }

                foreach (string tag in entry.Tags)
                {
                    if (tagDict.ContainsKey(tag))
                        tagDict[tag]++;
                    else
                        tagDict[tag] = 1;
                }
            }

            //alphabetize
            tagDict = tagDict.OrderBy(kvp => kvp.Key)
                         .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            TreesPanel.Populate(tagDict, dbUntaggedImageData.Count, totalEntriesCount);
        }

        public static void CopyImageFilesToLibraryDir(string[] filepaths)
        {
            foreach (string filepath in filepaths)
            {
                if (File.Exists(filepath))
                {
                    string filename = Path.GetFileName(filepath);
                    string destFilepath = Path.Combine(ActiveLibrary.Dirpath, filename);
                    File.Copy(filepath, destFilepath, overwrite: false);
                }
            }
        }
    }
}
