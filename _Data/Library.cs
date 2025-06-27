using Calypso._Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    public class Library
    {
        public string Name { get; set; }
        public string Dirpath { get; set; }
        public List<TagNode> TagTree { get; set; }
    }
}
