using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal struct Session
    {
        public int WindowWidth;
        public int WindowHeight;
        public bool RandomiseChecked;
        public enum ResultsNum
        {
            All,
            TwentyFive,
            Fifty
        }
        public ResultsNum _ResultsNum;
    }
}
