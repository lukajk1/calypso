
namespace Calypso
{
    public class Library
    {
        public string Name { get; set; }
        public string Dirpath { get; set; }
        public List<TagNode> TagTree { get; set; } = new();

        public bool AddTag(string tag)
        {
            foreach (TagNode node in TagTree)
            {
                if (node.Tag == tag) return false;
            }

            TagTree.Add(new TagNode() { Tag = tag });
            return true;
        }

        public bool AddChildTag(TagNode parent, string tag)
        {
            foreach (TagNode child in parent.Children)
            {
                if (child.Tag == tag) return false;
            }

            parent.Children.Add(new TagNode() { Tag = tag });
            return true;
        }
    }
}
