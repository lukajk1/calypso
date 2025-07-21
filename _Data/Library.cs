
using System.Diagnostics;

namespace Calypso
{
    public class Library
    {
        public string Name { get; set; }
        public string Dirpath { get; set; }
        public TagTreeRefactor tagTree { get; set; } = new();
        public Dictionary<string, List<ImageData>> tagDict { get; set; } = new();
        public Dictionary<string, ImageData> filenameDict { get; set; } = new();

        public Library(string name, string dirpath) 
        {
            Name = name;
            Dirpath = dirpath;
        }

        private void UpdateTagStructure()
        {
            DB.GenTagDictAndSaveLibrary();
            TagTreePanel.i.Populate(tagTree, tagDict);
        }

        public bool AddTag(TagNode newTag)
        {
            string name = newTag.Name;
            if (name == "all" || name == "untagged")
            {
                Util.ShowErrorDialog($"Invalid name for a tag!");
                return false;
            }

            foreach (TagNode node in tagTree.tagNodes)
            {
                if (node.Name == newTag.Name)
                {
                    Util.ShowErrorDialog($"The tag {newTag.Name} already exists!");
                    return false;
                }
            }

            tagTree.tagNodes.Add(newTag);
            tagDict[name] = new List<ImageData>();
            //Debug.WriteLine("tagtree length: " + TagNodeList.Count);

            UpdateTagStructure();
            return true;
        }
        public bool RemoveTag(string tag)
        {
            if (!tagTree.tagNodes.Any(n => n.Name == tag))
                return false;

            List<TagNode> toRemove = new();
            Queue<string> queue = new();
            queue.Enqueue(tag);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                TagNode node = tagTree.tagNodes.First(n => n.Name == current);
                toRemove.Add(node);

                foreach (string child in node.Children)
                    queue.Enqueue(child);
            }

            foreach (var node in toRemove)
            {
                tagTree.tagNodes.Remove(node);
                tagDict.Remove(node.Name);
            }

            UpdateTagStructure();
            return true;
        }





    }
}
