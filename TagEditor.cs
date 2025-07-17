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
        Dictionary<string, int> TagCountInSelection = new();
        List<TileTag> selection = new();
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

            checkedListBoxTags.CheckOnClick = true; 
            checkedListBoxTags.KeyDown += checkedListBoxTags_KeyDown;

            this.FormClosing += (s, e) =>
            {
                e.Cancel = true;
                CloseForm();
            };
        }

        public void Populate(List<TileTag> selection)
        {
            this.selection = selection;
            checkedListBoxTags.Items.Clear();

            this.TagCountInSelection.Clear();

            foreach (TileTag tTag in selection)
            {
                foreach (string tag in tTag._ImageData.Tags)
                {
                    if (TagCountInSelection.ContainsKey(tag))
                    {
                        TagCountInSelection[tag]++;
                    }
                    else
                    {
                        TagCountInSelection.Add(tag, 1);
                    }
                }
            }

            tagsUnchecked.Clear();
            multipleSelectionTagsChecked.Clear();

            foreach (var kvp in TagCountInSelection)
            {
                bool isChecked = (kvp.Value == selection.Count);

                if (isChecked)
                {
                    multipleSelectionTagsChecked.Add(kvp.Key);
                }

                checkedListBoxTags.Items.Add($"# {kvp.Key} ({kvp.Value})", isChecked);

            }

            // add tags not in selection
            foreach (var kvp in DB.TagDict)
            {
                if (!TagCountInSelection.ContainsKey(kvp.Key))
                    checkedListBoxTags.Items.Add("#" + kvp.Key, false);
            }
        }

        // handle tag deletion
        private void checkedListBoxTags_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && checkedListBoxTags.SelectedItem != null)
            {
                string tag = checkedListBoxTags.SelectedItem.ToString();
                if (tag != null && tag.StartsWith("#"))
                    tag = tag.Substring(1);

                checkedListBoxTags.Items.Remove(checkedListBoxTags.SelectedItem);

                DB.RemoveTag(tag);
                DB.GenDictsAndSaveLibrary();
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
                if (DB.TagDict.ContainsKey(tag))
                {
                    mainW.Activate();
                    mainW.Focus();

                    Util.ShowErrorDialog("Tag already exists!");
                }
                else
                {
                    checkedListBoxTags.Items.Add("#" + tag, true);
                }
            }

        }

        private void OnLossOfFocus(object sender, EventArgs e)
        {
            List<string> checkedTags = checkedListBoxTags.CheckedItems
                .Cast<string>()
                .Select(s => s.StartsWith("#") ? s.Substring(1) : s)
                .Select(s => s.TrimEnd().Split(' ')[0])
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            foreach (TileTag tTag in selection)
            {
                tTag._ImageData.SetTagList(checkedTags);
            }

            DB.GenDictsAndSaveLibrary();

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
