using Calypso.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    public enum Pane
    {
        TagSideTab, 
        Gallery, 
        Searchbar
    }
    internal static class Mediator
    {
        private static MainWindow? mainW;
        public static string MainWindowTitle = "Calypso";
        public static Pane FocusedPane;
        public static void Init(MainWindow mainW)
        {
            Mediator.mainW = mainW;
            mainW.Activate();
            mainW.Focus();

            // The selected index needs to be set to 0 before the first search is run
            mainW.comboBoxResultsNum.SelectedIndex = 0;

            // initialize all mediators
            Gallery.Init(mainW);
            StatusBar.Init(mainW);
            TreesPanel.Init(mainW);
            ImageInfoPanel.Init(mainW);
            Searchbar.Init(mainW);
            TagEditManager.Init(mainW);
            DBManager.Init(mainW);
            LayoutManager.Init(mainW);
        }
    }
}
