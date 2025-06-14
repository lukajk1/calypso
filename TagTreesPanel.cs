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
        static MainWindow? main;

        static int previousSplitterDistance;
        static bool isCollapsed;
        public static void Init(MainWindow main)
        {
            TagTreesPanel.main = main;
            TagTreesPanel.tree = main.tagTree;

            tree.Nodes.Clear();

            TreeNode nodeNone = tree.Nodes.Insert(0, "Untagged (16)");
            TreeNode nodeAll = tree.Nodes.Insert(0, "All Images (110)");
            TreeNode nodeResult = tree.Nodes.Insert(0, "Search Result");
            TreeNode nodeVG = tree.Nodes.Add("#videogame-graphics (15)");
            TreeNode picturesNode = tree.Nodes.Add("#photo (94)");

            nodeVG.Nodes.Add("#dishonored-2 (3)");
            nodeVG.Nodes.Add("#hl-2 (5)");

            picturesNode.Nodes.Add("#neoclassical (10)");
            picturesNode.Nodes.Add("#bananas (10)");

            //tree.Sort();
            tree.ExpandAll();
        }

    }
}
