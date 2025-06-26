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

            Mediator.Init(this);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control focused = Form.ActiveForm?.ActiveControl;

            bool searchBoxFocused = focused == searchBox;
            Debug.WriteLine(focused.Name);
            // single key shortcuts --------------------------------------------------------------------------------
            if (keyData == Keys.R)
            {
                if (!searchBox.ContainsFocus)
                {
                    LayoutManager.i.TogglePanel(tagTreeGallerySplitContainer, 1);
                    return true; // suppress further handling
                }

            }
            else if (keyData == Keys.N)
            {
                if (!searchBox.ContainsFocus)
                {
                    LayoutManager.i.TogglePanel(masterSplitContainer, 2);
                    return true;
                }
            }
            else if (keyData == Keys.I)
            {
                if (!searchBox.ContainsFocus)
                {
                    LayoutManager.i.TogglePanel(imageInfoHorizontalSplitContainer, 2);
                    return true;
                }
            }
            else if (keyData == Keys.T)
            {
                if (!searchBox.ContainsFocus)
                {
                    Gallery.OpenTagEditorByCommand();
                    return true;
                }
            }

            else if (keyData == Keys.Delete)
            {
                Gallery.DeleteSelected();
            }

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
                DBUtility.OpenCurrentLibrarySourceFolder();
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
                LayoutManager.i.SetLayout(LayoutManager.DefaultLayout);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D2))
            {
                //MessageBox.Show("received");
                LayoutManager.i.SetLayout(LayoutManager.LargeWindow);
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

        private void OpenTagModifier()
        {
            TagEditor tagM = new TagEditor(this);
            tagM.Show();

        }
        public static void PictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (sender is PictureBox pb && pb.Tag is string imagePath)
            {
                // Example: open full image in default viewer
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = imagePath,
                    UseShellExecute = true
                });
            }
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

            LayoutManager.i.SetLayout(LayoutManager.DefaultLayout);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            LayoutManager.i.SetLayout(LayoutManager.LargeWindow);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Commands.AddFilesViaDialog();
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            DBUtility.OpenCurrentLibrarySourceFolder();
        }

        private void checkBoxRandomize_CheckedChanged(object sender, EventArgs e)
        {
            Searchbar.RepeatLastSearch();
        }

        private void toolStripMenuItemAddNewLibrary_Click(object sender, EventArgs e)
        {
            DBUtility.AddNewLibrary();
        }
    }
}
