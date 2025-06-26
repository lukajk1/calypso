using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal static class Searchbar
    {
        static MainWindow? mainW;
        static string lastSearch = "all";
        public static void Init(MainWindow? mainW)
        {
            Searchbar.mainW = mainW;
        }

        public static void Search(string text)
        {
            mainW.searchBox.Text = text;
            lastSearch = text;

            int index = mainW.comboBoxResultsNum.SelectedIndex;
            int[] map = { 0, 25, 50 }; // upperlimit settings
            int resultsCount = map[index];

            DBUtility.Search(text, mainW.checkBoxRandomize.Checked, resultsCount);
        }

        public static void RepeatLastSearch()
        {
            Search(lastSearch);
        }
    }
}
