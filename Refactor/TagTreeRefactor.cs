using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    public class TagTreeRefactor
    {
        public List<TagNode> tagNodes { get; set; } = new();

        public TagTreeRefactor()
        {

        }

        public TagNode? Lookup(string tagName)
        {
            foreach (TagNode tag in tagNodes)
            {
                if (tag.Name == tagName)
                {
                    return tag;
                }
            }

            return null;
        }

        public void OrderByDepthAndAlphabetical()
        {
            tagNodes = tagNodes.OrderBy(t => t.Depth).ThenBy(t => t.Name).ToList();
        }

        public void Rename(string oldName, string newName)
        {

        }
    }

    //public class TagNodeRefactor
    //{
    //    public string Name { get; set; }
    //    public TagNodeRefactor? Parent { get; set; }
    //    public List<TagNodeRefactor> Children { get; } = new();
    //    public TagNodeRefactor(string name, string parent = "")
    //    {
    //        this.Name = name;
    //        this.Parent = parent;
    //    }
    //}
}
