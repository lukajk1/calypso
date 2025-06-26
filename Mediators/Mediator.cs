using Calypso.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal static class Mediator
    {
        private static MainWindow? mainW;
        public static string MainWindowTitle = "Calypso";
        public static void Init(MainWindow mainW)
        {
            Mediator.mainW = mainW;
            mainW.Activate();
            mainW.Focus();
            // this order is kinda specific lol.
            // The selected index needs to be set to 0 before the first search is run.
            mainW.comboBoxResultsNum.SelectedIndex = 0;

            // mediator-level initializers
            Gallery.Init(mainW);
            StatusBar.Init(mainW);
            TreesPanel.Init(mainW);
            ImageInfoPanel.Init(mainW);
            Searchbar.Init(mainW);
            TagEditManager.Init(mainW);
            DBManager.Init(mainW);

            // remove singleton?
            new LayoutManager(mainW);
            LayoutManager.i.SetLayout(LayoutManager.DefaultLayout);
        }
    }
}
