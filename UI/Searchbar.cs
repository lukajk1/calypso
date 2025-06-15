using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso.UI
{
    internal static class Searchbar
    {
        static MainWindow? mainW;

        public static void Init(MainWindow? mainW)
        {
            Searchbar.mainW = mainW;
        }

        public static void Search(string text)
        {
            mainW.searchBox.Text = text;
            DBUtilities.SearchByTags(text, mainW.checkBoxRandomize.Checked, 25);
        }
    }
}
