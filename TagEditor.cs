using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private ImageData currentImageData;
        Dictionary<string, int> tagDict;
        Dictionary<string, int> existingTagCountOnSelection = new();
        List<TileTag> currentSelection = new();
        List<string> tagsUnchecked = new();
        List<string> multipleSelectionTagsChecked = new();
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

            buttonAddTag.Click += AddTagEvent;
            newTagTextBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AddTagEvent(s, e);
                    e.SuppressKeyPress = true; // prevent beep
                }
            };

            checkedListBox1.CheckOnClick = true; 
            checkedListBox1.KeyDown += CheckedListBox1_KeyDown;

            this.FormClosing += (s, e) =>
            {
                e.Cancel = true;
                CloseForm();
            };
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
        public void Populate(Dictionary<string, int> dbTagDict, List<TileTag> selection)
        {
            currentSelection = selection;
            checkedListBox1.Items.Clear();
            this.tagDict = dbTagDict;

            this.existingTagCountOnSelection.Clear();

            foreach (TileTag tTag in selection)
            {
                foreach (string tag in tTag._ImageData.Tags)
                {
                    if (existingTagCountOnSelection.ContainsKey(tag))
                    {
                        existingTagCountOnSelection[tag]++;
                    }
                    else
                    {
                        existingTagCountOnSelection.Add(tag, 1);
                    }


                    // remove item from dict to prevent the same tag being added twice
                }
            }


            tagsUnchecked.Clear();
            multipleSelectionTagsChecked.Clear();

            foreach (var kvp in existingTagCountOnSelection)
            {
                bool isChecked = (kvp.Value == selection.Count);
                if (isChecked) multipleSelectionTagsChecked.Add(kvp.Key);

                checkedListBox1.Items.Add($"# {kvp.Key} ({kvp.Value})", isChecked);

            }


            foreach (var kvp in dbTagDict)
            {
                if (!existingTagCountOnSelection.ContainsKey(kvp.Key))
                    checkedListBox1.Items.Add("#" + kvp.Key, false);
            }
        }
        private void CheckedListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && checkedListBox1.SelectedItem != null)
            {
                string tag = checkedListBox1.SelectedItem.ToString();
                if (tag != null && tag.StartsWith("#"))
                    tag = tag.Substring(1);

                checkedListBox1.Items.Remove(checkedListBox1.SelectedItem);

                DBUtility.RemoveTag(tag);
                DBUtility.GenDictsAndSaveLibrary();
                e.Handled = true;
            }
        }

        private void AddTagEvent(object sender, EventArgs e)
        {
            string tag = newTagTextBox.Text;
            newTagTextBox.Clear();

            tag = tag.Trim().ToLower().Replace(" ", "-");

            if (tag != string.Empty)
            {
                if (tagDict.ContainsKey(tag))
                {
                    mainW.Activate();
                    mainW.Focus();

                    Util.ShowErrorDialog("Tag already exists!");
                }
                else
                {
                    tagDict.Add(tag, 1);
                    checkedListBox1.Items.Add("#" + tag, true);
                }
            }

        }


        private void OnLossOfFocus(object sender, EventArgs e)
        {
            List<string> checkedTags = checkedListBox1.CheckedItems
                .Cast<string>()
                .Select(s => s.StartsWith("#") ? s.Substring(1) : s)
                .Select(s => s.TrimEnd().Split(' ')[0])
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            List<string> removeTags = new();

            foreach (var tag in multipleSelectionTagsChecked)
            {
                if (!checkedTags.Contains(tag))
                {
                    removeTags.Add(tag);
                }
            }

            if (currentSelection.Count == 1)
            {
                currentSelection[0]._ImageData.SetTags(checkedTags);
            }
            else
            {
                foreach (TileTag tTag in currentSelection)
                {
                    tTag._ImageData.SetTagsOnlyAdd(checkedTags);
                    tTag._ImageData.RemoveTags(removeTags);
                }
                //foreach (var tag in existingTagCountOnSelection)
                //{
                //    if (!checkedTags.Contains(tag.Key) && tag.Value == currentSelection.Count)
                //    {
                //        foreach (var tTag in currentSelection)
                //        {
                //            tTag._ImageData.RemoveTags(tag.Key);
                //        }
                //    }
                //}

            }

            DBUtility.GenDictsAndSaveLibrary();

            this.CloseForm();
        }
    }
}
