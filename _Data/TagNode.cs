using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    public struct TagNode
    {
        public string Parent { get; set; }
        public string Name { get; set; }

        public TagNode(string tag, string parent = "") // parent empty by default
        {
            Name = tag;
            Parent = parent;
        }    
    }
}
