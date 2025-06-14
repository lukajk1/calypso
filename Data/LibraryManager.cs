using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calypso
{
    internal class LibraryManager
    {
        int currentLibrary = 0;
        LibraryData[] loadedLibraries;
        string librariesFilePath;

        public static LibraryManager i { get; private set; }
        MainWindow mainW;
        public LibraryManager(MainWindow mainW)
        {
            i = this;
            this.mainW = mainW;

            GalleryManager.Init(mainW);
            StatusBar.Init(mainW);
            TagTreesPanel.Init(mainW);
            ImageInfoPanel.Init(mainW);
            new LayoutManager(mainW);

            Start();
        }

        // needs to store library list.. that's it?  
        private void Start()
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
"CalypsoManager");
            librariesFilePath = Path.Combine(appDataPath, "libraries.json");

            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            else
            {
                if (!File.Exists(librariesFilePath))
                {

                    LibraryData[] defaultLibraries = new LibraryData[]
                    {
                        new LibraryData() {
                            DirectoryPath = "C:\\Users\\lukaj\\My Drive\\art\\art ref\\calypso_artref_library"
                        }
                    };

                    Util.SerializeToFile<LibraryData[]>(defaultLibraries, librariesFilePath);
                }
                LibraryData[] loadedLibraries = Util.DeserializeFromFile<LibraryData[]>(librariesFilePath);

                currentLibrary = 0;
                LoadLibrary(loadedLibraries[currentLibrary]);
                LayoutManager.i.LoadLayout(LayoutManager.DefaultLayout);

            }
        }
        public void LoadLibrary(LibraryData library)
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
            List<ImageData> existingImageDataEntries = new();

            // create databse directory if dne. in either case, gather a list of unregistered images.
            if (Directory.Exists(pathDatabaseDirectory))
            {
                // read from the file. If the database directory exists then database.json is presumed to exist.
                existingImageDataEntries = Util.DeserializeFromFile<List<ImageData>>(pathDatabaseJson);
                Dictionary<string, ImageData> dict = DatabaseSearch.GenerateFilenameDict(existingImageDataEntries);

                string[] allImageFiles = Util.GetAllImageFilepaths(library.DirectoryPath);

                foreach (string file in allImageFiles)
                {
                    if (!dict.ContainsKey(file))
                    {
                        unregisteredImagesList.Add(file); // add all filenames that aren't registered in the database to a list
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
                // gen a thumbnail for each, save to thumb
                Image thumb = Util.CreateThumbnail(filepath, GlobalValues.ThumbnailHeight);

                string filename = Path.GetFileName(filepath);
                string thumbSavePath = Path.Combine(pathThumbnailDir, "thumb_" + filename);

                ImageFormat format = Util.GetImageFormatFromExtension(thumbSavePath);
                thumb.Save(thumbSavePath, format);

                // register each to the database via creating an ImageData for it

                existingImageDataEntries.Add(new ImageData()
                {
                    FullResPath = filepath,
                    ThumbnailPath = thumbSavePath,
                    Filename = filename,
                    Tags = new()
                });
            }

            Util.SerializeToFile<List<ImageData>>(existingImageDataEntries, pathDatabaseJson);

        }

        public void OpenSourceFolder()
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = loadedLibraries[currentLibrary].DirectoryPath,
                UseShellExecute = true
            });

        }
    }
}
