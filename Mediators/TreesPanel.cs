using Calypso.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal class TreesPanel
    {
        static TreeView? tagTree;
        static TreeView? searchTree;
        static MainWindow? mainW;

        static int previousSplitterDistance;
        static bool isCollapsed;
        static TreeNode selectedNode;
        static bool initialized;
        public static void Init(MainWindow mainW)
        {
            TreesPanel.mainW = mainW;
            TreesPanel.tagTree = mainW.tagTree;
            TreesPanel.searchTree = mainW.savedSearchesTreeView;

            TreeNode nodeRand = searchTree.Nodes.Insert(0, "Random Tag");
            nodeRand.Tag = "rtag";

            initialized = true;
        }

        public static void Populate(TagTreeData tagTreeData)
        {
            if (!initialized) return;

            tagTree.Nodes.Clear();

            tagTree.NodeMouseClick -= OnTagNodeClick; // Remove if already attached
            tagTree.NodeMouseClick += OnTagNodeClick; // Attach handler
            searchTree.NodeMouseClick -= OnTagNodeClick;
            searchTree.NodeMouseClick += OnTagNodeClick;

            GenerateTagTree(tagTreeData.TagDict);

            TreeNode separator = new TreeNode("──────────");
            separator.ForeColor = Color.Gray;
            separator.NodeFont = new Font("Consolas", 8, FontStyle.Regular);
            tagTree.Nodes.Insert(0, separator);

            TreeNode nodeNone = tagTree.Nodes.Insert(0, $"Untagged ({tagTreeData.UntaggedCount.ToString()})");
            nodeNone.Tag = "untagged";

            TreeNode nodeAll = tagTree.Nodes.Insert(0, $"All Images ({tagTreeData.TotalEntries.ToString()})");
            nodeAll.Tag = "all";

            tagTree.ExpandAll(); 
            

            tagTree.BeforeCollapse += (s, e) =>
            {
                e.Cancel = true; // Prevent collapsing
            };
        }
        public static void GenerateTagTree(Dictionary<TagNode, List<ImageData>> tagDictionary)
        {
            tagTree.BeginUpdate();
            tagTree.Nodes.Clear();

            foreach (var kvp in tagDictionary)
            {
                TreeNode treeNode = CreateTreeNodeRecursive(kvp.Key, tagDictionary);
                tagTree.Nodes.Add(treeNode);
            }

            tagTree.ExpandAll();
            tagTree.EndUpdate();
        }

        private static TreeNode CreateTreeNodeRecursive(TagNode node, Dictionary<TagNode, List<ImageData>> tagDictionary)
        {
            int contentCount = tagDictionary.TryGetValue(node, out var images) ? images.Count : 0;
            string displayText = $"#{node.Name} ({contentCount})";

            TreeNode treeNode = new TreeNode(displayText)
            {
                Tag = node.Name
            };

            //foreach (var child in node.Children)
            //{
            //    treeNode.Nodes.Add(CreateTreeNodeRecursive(child, tagDictionary));
            //}

            return treeNode;
        }



        private static string GetLastSegment(string dottedKey)
        {
            if (string.IsNullOrEmpty(dottedKey))
                return dottedKey;

            int lastDotIndex = dottedKey.LastIndexOf('.');
            return lastDotIndex >= 0 ? dottedKey.Substring(lastDotIndex + 1) : dottedKey;
        }


        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag as string == "unselectable")
            {
                e.Cancel = true;
            }
        }
        private static void OnTagNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node;

            if (e.Button == MouseButtons.Left)
            {
                string tagName = selectedNode.Tag as string;

                // Only handle clicks on tag nodes (nodes with Tag data)
                if (tagName != null)
                {
                    //MessageBox.Show($"Clicked on tag: {tagName}");
                    Searchbar.Search(tagName);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                mainW.contextMenuTagTree.Show(tagTree, e.Location);
            }
        }

        public static void RenameTag(object sender)
        {
            OpenPrompt();
            //new TextPrompt(mainW, $"Renaming {selectedNode.Tag}").Show();

        }
        public static void DeleteTag(object sender)
        {
            Util.ShowConfirmDialog("This tag has x children. Proceed to delete?");

        }
        public static void AddChildNode(object sender)
        {
            Debug.WriteLine("childnode called");

            string tagName = selectedNode.Tag as string;
            string newTag = $"{tagName}.{OpenPrompt()}";
            Debug.WriteLine(newTag);
        }

        public static string OpenPrompt()
        {
            using (var prompt = new TextPrompt(mainW, "Enter new tag name:"))
            {
                if (prompt.ShowDialog() == DialogResult.OK)
                {
                    return prompt.ResultText;
                    //Debug.WriteLine("User entered: " + userInput);
                }
                else
                {
                    return string.Empty;
                    //.WriteLine("User cancelled input.");
                }
            }
        }

        public static void PromptAddTag()
        {
        }
    }
}
