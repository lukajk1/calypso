using Calypso.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal class TagTreesPanel
    {
        static TreeView? tree;
        static MainWindow? mainW;

        static int previousSplitterDistance;
        static bool isCollapsed;
        public static void Init(MainWindow mainW)
        {
            TagTreesPanel.mainW = mainW;
            TagTreesPanel.tree = mainW.tagTree;
        }

        public static void Populate(Dictionary<string, int> tagCounts, int untaggedEntriesCount, int totalEntriesCount)
        {
            tree.Nodes.Clear();

            tree.NodeMouseClick -= OnTagNodeClick; // Remove if already attached
            tree.NodeMouseClick += OnTagNodeClick; // Attach handler

            foreach (KeyValuePair<string, int> kvp in tagCounts)
            {
                TreeNode node = new TreeNode($"#{kvp.Key} ({kvp.Value.ToString()})");

                node.Tag = kvp.Key;
                tree.Nodes.Add(node);
            }

            TreeNode nodeNone = tree.Nodes.Insert(0, $"Untagged ({untaggedEntriesCount.ToString()})");
            nodeNone.Tag = "untagged";

            TreeNode nodeAll = tree.Nodes.Insert(0, $"All Images ({totalEntriesCount.ToString()})");
            nodeAll.Tag = "all";
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
