using System;
using System.Collections.Generic;
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

            Gallery.Init(mainW);
            StatusBar.Init(mainW);
            TagTreesPanel.Init(mainW);
            ImageInfoPanel.Init(mainW);
            new LayoutManager(mainW);

            StartUp();
        }

        // needs to store library list.. that's it?  
        private void StartUp()
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
                    //create a default library

                    LibraryData[] defaultLibraries = new LibraryData[]
                    {
                        new LibraryData() {
                            LayoutName = "Default",
                            DirectoryPath = "C:\\Users\\lukaj\\My Drive\\art\\art ref\\may25"
                        }
                    };

                    string json = JsonSerializer.Serialize(defaultLibraries);
                    File.WriteAllText(librariesFilePath, json);
                }

                LibraryData[] loadedLibraries = DeserializeLibraries();

                currentLibrary = 0;
                LoadLibrary(loadedLibraries[currentLibrary]);
                LayoutManager.i.LoadLayout(LayoutManager.DefaultLayout);

            }
        }

        private LibraryData[]? DeserializeLibraries()
        {
            loadedLibraries = Array.Empty<LibraryData>();

            string jsonString = File.ReadAllText(librariesFilePath);

            return JsonSerializer.Deserialize<LibraryData[]>(jsonString);
        }

        public void LoadLibrary(LibraryData library)
        {
            Gallery.Populate(library.DirectoryPath);
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
