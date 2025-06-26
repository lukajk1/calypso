using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso._Backend
{
    internal class TagNode
    {
        public string Tag { get; set; }
        public List<TagNode> Children { get; set; } = new();
    }
}
