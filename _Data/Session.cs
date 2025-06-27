using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    public struct Session
    {
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public bool RandomiseChecked { get; set; }
        public FormWindowState WindowState { get; set; }
        public Library LastActiveLibrary { get; set; }
    }
}
