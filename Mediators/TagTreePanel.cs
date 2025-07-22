﻿using Calypso.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calypso
{
    internal class TagTreePanel
    {
        // main refs
        private TreeView tagTree;
        private MainWindow mainW;
        public static TagTreePanel i;

        // internal use
        private TreeNode? selectedNode = null;

        public TagTreePanel(MainWindow mainW)
        {
            i = this;
            this.mainW = mainW;
            tagTree = mainW.tagTree;
            tagTree.BeforeCollapse += (s, e) =>
            {
                e.Cancel = true; // Prevent collapsing 
            };

            Populate(DB.appdata.ActiveLibrary.tagTree, DB.appdata.ActiveLibrary.tagDict);
        }

        public void Populate(TagTreeRefactor tagTreeRefactor, Dictionary<string, List<ImageData>> tagDict)
        {
            tagTree.Nodes.Clear();

            tagTree.NodeMouseClick -= OnTagNodeClick; // Remove if already attached
            tagTree.NodeMouseClick += OnTagNodeClick; // Attach handler

            GenerateTagTree(tagTreeRefactor, tagDict);

            TreeNode separator = new TreeNode("──────────");
            separator.ForeColor = Color.Gray;
            separator.NodeFont = new Font("Consolas", 8, FontStyle.Regular);
            tagTree.Nodes.Insert(0, separator);

            TreeNode nodeNone = tagTree.Nodes.Insert(0, $"Untagged ({tagDict["untagged"].Count})");
            nodeNone.Tag = "untagged";

            TreeNode nodeAll = tagTree.Nodes.Insert(0, $"All Images ({tagDict["all"].Count})");
            nodeAll.Tag = "all";

            tagTree.ExpandAll(); 
            
        }
        public void GenerateTagTree(TagTreeRefactor tagTreeRefactor, Dictionary<string, List<ImageData>> tagDict)
        {
            tagTree.BeginUpdate();
            tagTree.Nodes.Clear();

            foreach (TagNode node in tagTreeRefactor.tagNodes)
            {
                int num;
                if (tagDict.ContainsKey(node.Name))
                {
                    num = tagDict[node.Name].Count;
                }
                else
                {
                    continue;
                }

                AddToTree(node, num);
            }

            tagTree.ExpandAll();
            tagTree.EndUpdate();
        }

        private void AddToTree(TagNode node, int contentCount)
        {
            string displayText = $"#{node.Name} ({contentCount})";

            TreeNode newTreeNode = new TreeNode(displayText)
            {
                Tag = node
            };

            if (node.Parent != string.Empty)
            {
                foreach (TreeNode parent in GetAllNodes(tagTree))
                {
                    if (parent.Tag is TagNode parentTagNode && parentTagNode.Name == node.Parent)
                    {
                        parent.Nodes.Add(newTreeNode);
                        //Debug.WriteLine($"added {node.Name} to tree");
                    }
                }
            }
            else
            {
                tagTree.Nodes.Add(newTreeNode);
                //Debug.WriteLine($"added {node.Name} to tree");
            }
        }

        private IEnumerable<TreeNode> GetAllNodes(TreeView treeView)
        {
            foreach (TreeNode node in treeView.Nodes)
                foreach (var child in GetAllNodes(node))
                    yield return child;
        }

        private  IEnumerable<TreeNode> GetAllNodes(TreeNode parent)
        {
            yield return parent;
            foreach (TreeNode child in parent.Nodes)
                foreach (var descendant in GetAllNodes(child))
                    yield return descendant;
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag as string == "unselectable")
            {
                e.Cancel = true;
            }
        }
        private void OnTagNodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedNode = e.Node;

            if (e.Button == MouseButtons.Left)
            {
                if (selectedNode.Tag is TagNode tagNode)
                {
                    Searchbar.Search(tagNode.Name);
                }
                else if (selectedNode.Tag is string value)
                {
                    Searchbar.Search(value);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (sender is TreeNode treeNode) selectedNode = treeNode;
                mainW.contextMenuTagTree.Show(tagTree, e.Location);
            }
        }

        public  void RenameTag(object sender)
        {
            if (Util.TextPrompt("Set new tag name: ", out string newName))
            {
                if (selectedNode.Tag is TagNode tagNode)
                {
                    DB.appdata.ActiveLibrary.RenameTag(tagNode.Name, newName);
                }
            }

        }
        public void DeleteTag(object sender)
        {
            int children = selectedNode.Nodes.Count;
            if (children > 0)
            {
                Util.ShowConfirmDialog($"This tag has children which will be deleted as well. Proceed?");
            }
            if (selectedNode.Tag is TagNode tn)
            {
                //Util.ShowInfoDialog("delete" + tn.Name);
                DB.appdata.ActiveLibrary.DeleteTagFromTree(tn.Name);

            }
        }

        public  void AddChildTag(object sender)
        {
            if (selectedNode.Tag is TagNode parentNode)
            {
                if (Util.TextPrompt("Set tag name: ", out string name))
                {
                    //Util.ShowInfoDialog($"parentnode depth : {parentNode.Depth}");
                    var newTag = new TagNode(name, parentNode.Name, parentNode.Depth + 1);
                    //Util.ShowInfoDialog($"newtag depth : {newTag.Depth}");

                    DB.appdata.ActiveLibrary.AddTagToTree(newTag);
                }
            }
        }
    }
}
