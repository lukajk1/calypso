using Calypso.UI;
using System;
using System.Collections.Generic;
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
        public static void Init(MainWindow mainW)
        {
            TreesPanel.mainW = mainW;
            TreesPanel.tagTree = mainW.tagTree;
            TreesPanel.searchTree = mainW.savedSearchesTreeView;

            TreeNode nodeRand = searchTree.Nodes.Insert(0, "Random Tag");
            nodeRand.Tag = "rtag";
        }

        public static void Populate(Dictionary<string, int> tagData, int untaggedEntriesCount, int totalEntriesCount)
        {
            tagTree.Nodes.Clear();

            tagTree.NodeMouseClick -= OnTagNodeClick; // Remove if already attached
            tagTree.NodeMouseClick += OnTagNodeClick; // Attach handler
            searchTree.NodeMouseClick -= OnTagNodeClick;
            searchTree.NodeMouseClick += OnTagNodeClick;

            foreach (KeyValuePair<string, int> kvp in tagData)
            {
                TreeNode node = new TreeNode($"#{kvp.Key} ({kvp.Value.ToString()})");

                node.Tag = kvp.Key;
                tagTree.Nodes.Add(node);
            }

            TreeNode separator = new TreeNode("──────────");
            separator.ForeColor = Color.Gray;
            separator.NodeFont = new Font("Consolas", 8, FontStyle.Regular);
            tagTree.Nodes.Insert(0, separator);

            TreeNode nodeNone = tagTree.Nodes.Insert(0, $"Untagged ({untaggedEntriesCount.ToString()})");
            nodeNone.Tag = "untagged";

            TreeNode nodeAll = tagTree.Nodes.Insert(0, $"All Images ({totalEntriesCount.ToString()})");
            nodeAll.Tag = "all";
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
            TreeNode clickedNode = e.Node;
            string tagName = clickedNode.Tag as string;

            // Only handle clicks on tag nodes (nodes with Tag data)
            if (tagName != null)
            {
                //MessageBox.Show($"Clicked on tag: {tagName}");
                Searchbar.Search(tagName);
            }
        }
    }
}
