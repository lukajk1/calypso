using Calypso.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calypso
{
    internal class Database
    {
        int currentLibrary = 0;
        LibraryDataStruct[] loadedLibraries;
        string librariesFilePath;
        public List<ImageData> loadedImageDataDatabase = new();
        public List<ImageData> dbUntaggedImageData = new();

        public Dictionary<string, int> tagDict = new();
        private string currentLoadedDBJson;
        public static Database i { get; private set; }
        MainWindow mainW;
        public Database(MainWindow mainW)
        {
            i = this;
            this.mainW = mainW;

            Start();
        }

        public void Start()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
"CalypsoManager");
            librariesFilePath = Path.Combine(appDataPath, "libraries.json");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            else // in theory this would prompt you to designate a directory but whatever
            {
                if (!File.Exists(librariesFilePath))
                {

                    LibraryDataStruct[] defaultLibraries = new LibraryDataStruct[]
                    {
                        new LibraryDataStruct() {
                            DirectoryPath = @"C:\Users\lukaj\My Drive\art\art ref\calypso_artref_library"
                        }
                    };

                    Util.SerializeToFile<LibraryDataStruct[]>(defaultLibraries, librariesFilePath);
                }
                loadedLibraries = Util.DeserializeFromFile<LibraryDataStruct[]>(librariesFilePath);

                currentLibrary = 0;
                LoadLibrary(loadedLibraries[currentLibrary]);

            }
        }
        public void RefreshLibrary()
        {
            LoadLibrary(loadedLibraries[currentLibrary]);
        }
        private void LoadLibrary(LibraryDataStruct library)
        {
            string pathThumbnailDir = Path.Combine(library.DirectoryPath, "thumbnails");
            string pathDatabaseDirectory = Path.Combine(library.DirectoryPath, "database");
            currentLoadedDBJson = Path.Combine(pathDatabaseDirectory, "database.json");

            ImageData[] unregisteredImages;
            List<string> unregisteredImagesList = new List<string>();
            string[] allImageFiles = Util.GetAllImageFilepaths(library.DirectoryPath);

            loadedImageDataDatabase.Clear();

            if (Directory.Exists(pathDatabaseDirectory))
            {
                loadedImageDataDatabase = Util.DeserializeFromFile<List<ImageData>>(currentLoadedDBJson);

                Dictionary<string, ImageData> fileNameDict = DBUtilities.GenerateFilenameDict(loadedImageDataDatabase);

                foreach (string filepath in allImageFiles)
                {
                    if (!fileNameDict.ContainsKey(Path.GetFileName(filepath)))
                    {
                        unregisteredImagesList.Add(filepath); // add all filenames that aren't registered in the database to a list
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(pathDatabaseDirectory);
                foreach (string file in allImageFiles)
                {
                    unregisteredImagesList.Add(file);
                }
            }
            
            if (!Directory.Exists(pathThumbnailDir)) 
                Directory.CreateDirectory(pathThumbnailDir);

            foreach (string filepath in unregisteredImagesList)
            {
                string filename = Path.GetFileName(filepath);
                string thumbSavePath = Path.Combine(pathThumbnailDir, "thumb_" + filename);

                if (File.Exists(thumbSavePath))
                    continue;

                using Image thumb = Util.CreateThumbnail(filepath, GlobalValues.ThumbnailHeight);
                ImageFormat format = Util.GetImageFormatFromExtension(thumbSavePath);
                thumb.Save(thumbSavePath, format);

                // register to db
                loadedImageDataDatabase.Add(new ImageData(filepath, thumbSavePath, filename));
            }

            RefreshAndSaveDatabase();
        }
        public void DeleteImageData(List<ImageData> imgDataList)
        {
            foreach (ImageData imgData in imgDataList)
            {
                loadedImageDataDatabase.Remove(imgData);

                if (File.Exists(imgData.ThumbnailPath))
                {
                    File.Delete(imgData.ThumbnailPath);
                }

                if (File.Exists(imgData.FullResPath))
                {
                    File.Delete(imgData.FullResPath);
                }
            }

            RefreshAndSaveDatabase();
        }
        public void RemoveTag(string tag)
        {
            if (!DBUtilities.tagIndex.ContainsKey(tag)) return;

            foreach (ImageData imgData in DBUtilities.tagIndex[tag])
            {
                imgData.Tags.Remove(tag);
            }
            BuildTagDict();
        }
        public void RefreshAndSaveDatabase()
        {
            BuildTagDict();
            DBUtilities.GenerateTagDict(loadedImageDataDatabase);
            Util.SerializeToFile<List<ImageData>>(loadedImageDataDatabase, currentLoadedDBJson);
        }
        private void BuildTagDict()
        {
            dbUntaggedImageData.Clear();
            tagDict.Clear();

            int totalEntriesCount = loadedImageDataDatabase.Count;

            foreach (ImageData entry in loadedImageDataDatabase)
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
        public void OpenSourceFolder()
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = loadedLibraries[currentLibrary].DirectoryPath,
                UseShellExecute = true
            });

        }

        public void SendImageFilesToLibrary(string[] filepaths)
        {
            foreach (string filepath in filepaths)
            {
                if (File.Exists(filepath))
                {
                    string filename = Path.GetFileName(filepath);
                    string destFilepath = Path.Combine(loadedLibraries[currentLibrary].DirectoryPath, filename);
                    File.Copy(filepath, destFilepath, overwrite: false);
                }
            }
        }

        public LibraryDataStruct GetCurrentLibrary()
        {
            return loadedLibraries[currentLibrary];
        }
    }
}
