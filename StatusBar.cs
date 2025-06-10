using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalypsoExperiment1
{
    internal class StatusBar
    {
        private static ToolStripStatusLabel? statusStripImageCount;
        public static void Init(ToolStripStatusLabel statusStripImageCount)
        {
            StatusBar.statusStripImageCount = statusStripImageCount;
        }


        private static int _imagesLoaded;
        public static int ImagesLoaded
        {
            set
            {
                _imagesLoaded = value;
                statusStripImageCount.Text = $"Image Count: {_imagesLoaded}";
            }
        }


    }
}
