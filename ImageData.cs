using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal class ImageData
    {
        public string FullResPath {  get; set; }
        public string ThumbnailPath {  get; set; }
        public string Filename {  get; set; }
        public string[] Tags {  get; set; }
    }
}
