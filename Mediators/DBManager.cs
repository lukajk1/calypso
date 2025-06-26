using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal static class DBManager
    {
        private static MainWindow mainW;
        private static SessionData Session;
        public static void Init(MainWindow mainW)
        {
            DBManager.mainW = mainW;
            Database.OnNewLibraryLoaded += OnNewLibraryLoaded;

            Start();
        }

        private static void Start()
        {
            bool jsonExists;
            Database.Init(out jsonExists);

            if (jsonExists && Database.ReadDBJson())
            {
                Database.LoadActiveLibrary();
            }
            else if (Database.CreateDBJson())
            {
                Database.LoadActiveLibrary();
            }
            else return;

            PopulateLibraryUI();
            SessionData sessionData = default;

            if (Database.RetrieveSession(out sessionData))
            {
                mainW.LoadSession(sessionData);
            }

        }
        public static void LoadLibrary(int num)
        {
            if (Database.Libraries.ElementAtOrDefault(num-1) != null)
            {
                Database.LoadLibrary(Database.Libraries[num-1]);
            }
        }
        static void OnNewLibraryLoaded(LibraryData lib)
        {
            Searchbar.Search("all");
            mainW.Text = Mediator.MainWindowTitle + " - " + lib.Name;
        }
        static void PopulateLibraryUI()
        {
            var toRemove = new List<ToolStripItem>();

            foreach (ToolStripItem item in mainW.openExistingLibraryToolStripMenuItem.DropDownItems)
            {
                if (item is ToolStripMenuItem menuItem && item.Tag?.ToString() != "no-delete")
                {
                    toRemove.Add(item);
                }
            }

            foreach (var item in toRemove)
            {
                mainW.openExistingLibraryToolStripMenuItem.DropDownItems.Remove(item);
            }

            foreach (LibraryData lib in Database.Libraries)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem(lib.Name);
                //if (lib == DBUtility.ActiveLibrary) newItem.Enabled = false;

                newItem.Click += (sender, e) =>
                {
                    Database.LoadLibrary(lib);
                };

                mainW.openExistingLibraryToolStripMenuItem.DropDownItems.Insert(0, newItem);
            }
        }
    }
}
