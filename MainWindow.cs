using Calypso.UI;
using System.Diagnostics;
using System.Windows.Forms;

namespace Calypso
{
    public partial class MainWindow : Form
    {

        public MainWindow()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.FormClosed += MainWindow_FormClosed;
            this.MouseWheel += MainWindow_MouseWheel;

            Mediator.Init(this);

            var myList = new List<TagNode> 
            { 
                new() { Tag = "banana", ContentCount = 6, Children = 
                    new() { 
                        new() { Tag = "genz", ContentCount = 3, Children =
                            new() {
                                new() { Tag = "d", ContentCount = 3 }
                            }  } 
                    } 
                }, 
                new() { Tag = "pineapple", ContentCount = 6 }
            
            };


            TreesPanel.Populate(myList, 15, 16);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control focused = Form.ActiveForm?.ActiveControl;

            bool searchBoxFocused = (Mediator.FocusedPane != Pane.Searchbar);

            #region single key shortcuts
            if (keyData == Keys.R)
            {
                if (!searchBoxFocused)
                {
                    LayoutManager.TogglePanel(tagTreeGallerySplitContainer, 1);
                    return true; // suppress further handling
                }

            }
            else if (keyData == Keys.N)
            {
                if (!searchBoxFocused)
                {
                    LayoutManager.TogglePanel(masterSplitContainer, 2);
                    return true;
                }
            }
            else if (keyData == Keys.I)
            {
                if (!searchBoxFocused)
                {
                    LayoutManager.TogglePanel(imageInfoHorizontalSplitContainer, 2);
                    return true;
                }
            }
            else if (keyData == Keys.T)
            {
                if (!searchBoxFocused)
                {
                    Gallery.OpenTagEditorByCommand();
                    return true;
                }
            }

            else if (keyData == Keys.Delete)
            {
                if (Mediator.FocusedPane == Pane.Gallery) Gallery.DeleteSelected();
            }
            #endregion

            // ctrl shortcuts --------------------------------------------------------------------------------
            if (keyData == (Keys.Control | Keys.Q))
            {
                Close();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.L))
            {
                if (focused == searchBox)
                {
                    // focus the tagtree I guess. just has to move focus off the searchbar
                    tagTree.Focus();
                }
                else
                    searchBox.Focus();


                return true;
            }
            else if (keyData == (Keys.Control | Keys.Enter))
            {
                DB.OpenCurrentLibrarySourceFolder();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.A))
            {
                if (!(focused == searchBox))
                {
                    Gallery.SelectAll();
                }
                return true;
            }

            else if (keyData == (Keys.Control | Keys.D1))
            {
                LayoutManager.SetLayout(LayoutManager.DefaultLayout);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D2))
            {
                //MessageBox.Show("received");
                LayoutManager.SetLayout(LayoutManager.LargeWindow);
                return true;
            }

            else if (keyData == (Keys.Control | Keys.S))
            {
                MessageBox.Show($"{masterSplitContainer.SplitterDistance}");


                return true;
            }
            else if (keyData == (Keys.Control | Keys.I))
            {
                Commands.AddFilesViaDialog();
                return true;
            }
            // arrow keys
            else if (keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down)
            {
                Gallery.ArrowSelect(keyData);
            }


            else if (keyData == Keys.Enter)
            {
                if (Mediator.FocusedPane == Pane.Gallery)
                {
                    Gallery.OpenSelected();
                }
                return true;
            }

            // shift
            else if ((keyData & (Keys.Control | Keys.Shift)) == (Keys.Control | Keys.Shift))
            {
                for (int i = 1; i <= 9; i++)
                {
                    if ((keyData & Keys.KeyCode) == (Keys)((int)Keys.D0 + i))
                    {
                        DBManager.LoadLibrary(i);
                        break;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void MainWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            Gallery.ZoomFromWheel(e);
        }

        public void LoadSession(Session session)
        {
            this.Height = session.WindowHeight;
            this.Width = session.WindowWidth;
            this.checkBoxRandomize.Checked = session.RandomiseChecked;
            this.WindowState = session.WindowState;
            DB.ActiveLibrary = session.LastActiveLibrary;
            Gallery.Zoom = session.ZoomFactor;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Session session = new Session()
            {
                WindowHeight = this.Height,
                WindowWidth = this.Width,
                RandomiseChecked = this.checkBoxRandomize.Checked,
                WindowState = this.WindowState,
                LastActiveLibrary = DB.ActiveLibrary,
                ZoomFactor = Gallery.Zoom
            };
            DB.SaveSession(session);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Calypso Image Manager v1.0.0\nSupported file types: .jpg, .jpeg, .png, .bmp, .gif\nCreated by lukajk";
            string title = "About Calypso";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // suppress potential sfx
                e.SuppressKeyPress = true;
                e.Handled = true;


                if (File.Exists(searchBox.Text))
                {
                    // handle file explorer capabilities at some point? 
                }
                else
                {
                    Searchbar.Search(searchBox.Text);
                }

            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

            LayoutManager.SetLayout(LayoutManager.DefaultLayout);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            LayoutManager.SetLayout(LayoutManager.LargeWindow);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Commands.AddFilesViaDialog();
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            DB.OpenCurrentLibrarySourceFolder();
        }

        private void checkBoxRandomize_CheckedChanged(object sender, EventArgs e)
        {
            Searchbar.RepeatLastSearch();
        }

        private void toolStripMenuItemAddNewLibrary_Click(object sender, EventArgs e)
        {
            DB.AddNewLibrary();
        }

        private void newGalleryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB.AddNewLibrary();
        }

        private void removeTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreesPanel.RenameTag(sender);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreesPanel.DeleteTag(sender);
        }

        private void addChildTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreesPanel.AddChildNode(sender);
        }

        private void addNewTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreesPanel.PromptAddTag();
        }
    }
}
