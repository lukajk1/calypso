﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calypso
{
    public partial class TagEditor : Form
    {
        private MainWindow? mainW;
        List<TileTag> selection = new();
        private ImageData currentImageData;
        private List<string> commonTags = new();

        public TagEditor(MainWindow mainW)
        {
            this.mainW = mainW;
            InitializeComponent();

            this.Deactivate += OnLossOfFocus;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = true; 
            this.KeyPreview = true;
            this.KeyDown += TagEditor_KeyDown; 
            this.ShowIcon = false;

            // treeview config
            tagEditorTree.CheckBoxes = true;
            tagEditorTree.BeforeSelect += (s, e) =>
            {
                if (e.Node.Tag is string tag && tag == "unselectable")
                    e.Cancel = true;
            };
            tagEditorTree.BeforeCollapse += (s, e) => e.Cancel = true;
            tagEditorTree.NodeMouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    e.Node.Checked = !e.Node.Checked;
                }
            };



            this.FormClosing += (s, e) =>
            {
                e.Cancel = true;
                CloseForm();
            };
        }

        public void Populate(List<TileTag> selection)
        {
            this.selection = selection;

            // get intersection of all tags on all tiletag objects. These are the ones that will be checked when displaying
            commonTags = selection
                .Select(t => t._ImageData.Tags)
                .Aggregate((prev, next) => prev.Intersect(next).ToList());

            GenerateTagTree(DB.appdata.ActiveLibrary.tagTree, DB.appdata.ActiveLibrary.tagDict);

        }
        public void GenerateTagTree(TagTreeRefactor tagTreeRefactor, Dictionary<string, List<ImageData>> tagDict)
        {
            tagEditorTree.BeginUpdate();
            tagEditorTree.Nodes.Clear();

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

            tagEditorTree.ExpandAll();
            tagEditorTree.EndUpdate();
        }

        private void AddToTree(TagNode node, int contentCount)
        {
            string displayText = $"#{node.Name}";
            bool isChecked = commonTags.Contains(node.Name);

            TreeNode newTreeNode = new TreeNode(displayText)
            {
                Tag = node,
                Checked = isChecked
            };

            if (node.Parent != string.Empty)
            {
                foreach (TreeNode parent in GetNodes(tagEditorTree))
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
                tagEditorTree.Nodes.Add(newTreeNode);
                //Debug.WriteLine($"added {node.Name} to tree");
            }
        }
        private IEnumerable<TreeNode> GetNodes(TreeView treeView, bool onlyChecked = false)
        {
            foreach (TreeNode node in treeView.Nodes)
                foreach (var child in GetNodes(node, onlyChecked))
                    yield return child;
        }

        private IEnumerable<TreeNode> GetNodes(TreeNode parent, bool onlyChecked = false)
        {
            if (!onlyChecked || parent.Checked)
                yield return parent;
            foreach (TreeNode child in parent.Nodes)
                foreach (var descendant in GetNodes(child, onlyChecked))
                    yield return descendant;
        }

        private void OnLossOfFocus(object sender, EventArgs e)
        {
            List<string> tagsToAdd = new();
            foreach (TreeNode node in GetNodes(tagEditorTree, true))
            {
                if (node.Tag is TagNode tagNode)
                {
                    tagsToAdd.Add(tagNode.Name);
                }
            }

            // remove all tags from all images that are not in tagsToAdd
            List<string> toRemove = new();
            foreach (TileTag tTag in selection)
            {
                foreach (string tag in tTag._ImageData.Tags)
                {
                    if (!tagsToAdd.Contains(tag))
                    {
                        //Debug.WriteLine(tag + "is not contained in tags to add.. removing..");
                        toRemove.Add(tag);
                    }
                }

                // remove tags from imagedata
                foreach (string tag in toRemove)
                {
                    DB.appdata.ActiveLibrary.UntagImage(tag, tTag._ImageData);
                }
            }


            //Debug.WriteLine("should only be preserving" + tagsToAdd.Count + "tags");

            // grab imagelist from selection
            List<ImageData> images = new();
            foreach (var tTag in selection)
            {
                images.Add(tTag._ImageData);
            }

            // tag images with each tag from checkedtags
            foreach (string tag in tagsToAdd)
            {
                DB.appdata.ActiveLibrary.TagImages(tag, images);
            }

            DB.GenTagDictAndSaveLibrary();
            ImageInfoPanel.Refresh(); // nonessential dependency, just QOL to refresh and show the new tags
            this.CloseForm();
        }
        private void TagEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseForm();
                e.Handled = true;
            }
        }
        private void CloseForm()
        {
            this.Hide();
        }
    }
}
