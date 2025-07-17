using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    public class TagNode
    {
        public string Tag { get; set; }
        public int ContentCount { get; set; } = 0;
        public List<TagNode> Children { get; set; } = new();
    }
}
