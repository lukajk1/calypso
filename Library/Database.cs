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
        public static Database i { get; private set; }
        MainWindow mainW;
        public Database(MainWindow mainW)
        {
            i = this;
            this.mainW = mainW;

            Gallery.Init(mainW);
            StatusBar.Init(mainW);
            TagTreesPanel.Init(mainW);
            ImageInfoPanel.Init(mainW);
            Searchbar.Init(mainW);

            new LayoutManager(mainW);
            LayoutManager.i.SetLayout(LayoutManager.DefaultLayout);

            Start();
        }

        // needs to store library list.. that's it?  
        public void Start()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
"CalypsoManager");
            librariesFilePath = Path.Combine(appDataPath, "libraries.json");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            else // in theory this would prompt you to designate a directory but 
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
        public void LoadLibrary()
        {
            LoadLibrary(loadedLibraries[currentLibrary]);
        }
        private void LoadLibrary(LibraryDataStruct library)
        {
            // check for thumbnails
            //GalleryManager.Populate(library.DirectoryPath);
            string pathThumbnailDir = Path.Combine(library.DirectoryPath, "thumbnails");

            string pathDatabaseDirectory = Path.Combine(library.DirectoryPath, "database");
            string pathDatabaseJson = Path.Combine(pathDatabaseDirectory, "database.json");

            // deserialize the database, which is a fat List<ImageData>.
            // then check if every image in the library has an entry in the list, by matching the filename to the filename property of each ImageData.
            // ones that don't meet this requirement are added to a collection, where they will be iterated through and ImageData created for them. They will not have any tags associated with them. A thumbnail needs to be generated for each of them. 

            ImageData[] unregisteredImages;
            List<string> unregisteredImagesList = new List<string>();
            loadedImageDataDatabase.Clear();

            // create databse directory if dne. in either case, gather a list of unregistered images.
            if (Directory.Exists(pathDatabaseDirectory))
            {
                // read from the file. If the database directory exists then database.json is presumed to exist.
                loadedImageDataDatabase = Util.DeserializeFromFile<List<ImageData>>(pathDatabaseJson);
                Dictionary<string, ImageData> dict = DBUtilities.GenerateFilenameDict(loadedImageDataDatabase);

                string[] allImageFiles = Util.GetAllImageFilepaths(library.DirectoryPath);

                foreach (string filepath in allImageFiles)
                {
                    if (!dict.ContainsKey(Path.GetFileName(filepath)))
                    {
                        unregisteredImagesList.Add(filepath); // add all filenames that aren't registered in the database to a list
                    }
                }

            }
            else
            {
                Directory.CreateDirectory(pathDatabaseDirectory);
                string[] allImageFiles = Util.GetAllImageFilepaths(library.DirectoryPath);
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
                loadedImageDataDatabase.Add(new ImageData()
                {
                    FullResPath = filepath,
                    ThumbnailPath = thumbSavePath,
                    Filename = filename,
                    Tags = new()
                });
            }

            DBUtilities.GenerateTagDict(loadedImageDataDatabase);

            Util.SerializeToFile<List<ImageData>>(loadedImageDataDatabase, pathDatabaseJson); // save potential new images to file

            foreach (ImageData entry in  loadedImageDataDatabase)
            {
                if (entry.Tags.Count > 0)
                {
                    foreach (string item in entry.Tags)
                        Debug.WriteLine($"{entry.Filename} : {item}");
                }
                else
                    Debug.WriteLine("no tags found");
            }

            CollateTagCount(loadedImageDataDatabase);
            Searchbar.Search("all");
        }
        private void CollateTagCount(List<ImageData> db)
        {
            dbUntaggedImageData.Clear();

            Dictionary<string, int> tagCounts = new();
            int totalEntriesCount = db.Count;

            foreach (ImageData entry in db)
            {
                if (entry.Tags.Count == 0)
                {
                    dbUntaggedImageData.Add(entry);
                    continue;
                }

                foreach (string tag in entry.Tags)
                {
                    if (tagCounts.ContainsKey(tag))
                        tagCounts[tag]++;
                    else
                        tagCounts[tag] = 1;
                }
            }

            TagTreesPanel.Populate(tagCounts, dbUntaggedImageData.Count, totalEntriesCount);
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

    }
}
