
using System.Diagnostics;

namespace Calypso
{
    public class Library
    {
        public string Name { get; set; }
        public string Dirpath { get; set; }
        public List<TagNode> TagTree { get; set; } = new();
        public List<ImageData> ImageDataList { get; set; } = new();

        public Library(string name, string dirpath) 
        {
            Name = name;
            Dirpath = dirpath;
        }

        public bool AddTag(TagNode newTag)
        {
            string name = newTag.Name;
            if (name == "all" || name == "untagged")
            {
                Util.ShowErrorDialog($"Invalid name for a tag!");
                return false;
            }

            foreach (TagNode node in TagTree)
            {
                if (node.Name == newTag.Name)
                {
                    Util.ShowErrorDialog($"The tag {newTag.Name} already exists!");
                    return false;
                }
            }

            TagTree.Add(newTag);
            Debug.WriteLine("tagtree length: " + TagTree.Count);

            TreesPanel.Populate(DB.GenCurrentTagTree());
            return true;
        }
    }
}
