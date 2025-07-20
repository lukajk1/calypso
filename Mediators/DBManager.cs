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
        private static Session Session;
        public static void Init(MainWindow mainW)
        {
            DBManager.mainW = mainW;
            DB.OnNewLibraryLoaded += OnNewLibraryLoaded;
        }
        public static void LoadLibrary(int num)
        {
            //if (DB.Libraries.ElementAtOrDefault(num-1) != null)
            //{
            //    DB.LoadLibrary(DB.Libraries[num-1]);
            //}
        }
        static void OnNewLibraryLoaded(Library lib)
        {
            Searchbar.Search("all");
            mainW.Text = MainWindow.MainWindowTitle + " - " + lib.Name;
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

            foreach (Library lib in DB.appdata.Libraries)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem(lib.Name);
                //if (lib == DBUtility.ActiveLibrary) newItem.Enabled = false;

                newItem.Click += (sender, e) =>
                {
                    DB.LoadLibrary(lib);
                };

                mainW.openExistingLibraryToolStripMenuItem.DropDownItems.Insert(0, newItem);
            }
        }
    }
}
