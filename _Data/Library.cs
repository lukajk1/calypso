
using System.Diagnostics;

namespace Calypso
{
    public class Library
    {
        public string Name { get; set; }
        public string Dirpath { get; set; }
        public List<TagNode> TagNodeList { get; set; } = new();
        public List<ImageData> ImageDataList { get; set; } = new();

        public Library(string name, string dirpath) 
        {
            Name = name;
            Dirpath = dirpath;
        }

        private void UpdateTagStructure()
        {
            DB.GenDictAndSaveLibrary();
            //TagTreePanel.i.Populate(DB.ActiveTagTree);
        }

        public bool AddTag(TagNode newTag)
        {
            string name = newTag.Name;
            if (name == "all" || name == "untagged")
            {
                Util.ShowErrorDialog($"Invalid name for a tag!");
                return false;
            }

            foreach (TagNode node in TagNodeList)
            {
                if (node.Name == newTag.Name)
                {
                    Util.ShowErrorDialog($"The tag {newTag.Name} already exists!");
                    return false;
                }
            }

            TagNodeList.Add(newTag);
            Debug.WriteLine("tagtree length: " + TagNodeList.Count);
            
            UpdateTagStructure();
            return true;
        }

        public bool RemoveTag(string tag)
        {
            List<TagNode> toRemove = new();

            foreach (TagNode node in TagNodeList)
            {
                if (node.Name == tag || node.Parent == tag)
                    toRemove.Add(node);
            }

            foreach (var node in toRemove)
                TagNodeList.Remove(node);

            UpdateTagStructure();
            return toRemove.Count > 0;
        }

    }
}
