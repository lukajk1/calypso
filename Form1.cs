using System.Windows.Forms;

namespace Calypso
{
    public partial class MainWindow : Form
    {

        public MainWindow()
        {
            InitializeComponent();

            new LibraryManager(this);

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // single key shortcuts --------------------------------------------------------------------------------
            if (keyData == Keys.R)
            {
                LayoutManager.i.TogglePanel(tagTreeGallerySplitContainer, 1);
                return true; // suppress further handling
            }
            else if (keyData == Keys.N)
            {
                LayoutManager.i.TogglePanel(masterSplitContainer, 2);
                return true;
            }
            else if (keyData == Keys.I)
            {
                //MessageBox.Show("recieved");
                LayoutManager.i.TogglePanel(imageInfoHorizontalSplitContainer, 2);
                return true;
            }

            // ctrl shortcuts --------------------------------------------------------------------------------
            if (keyData == (Keys.Control | Keys.Q))
            {
                Close();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.L))
            {
                Control focused = Form.ActiveForm?.ActiveControl;

                if (focused == searchBox)
                {

                    // Move focus to next focusable control
                    SelectNextControl(ActiveControl, true, true, true, true);
                }
                else
                    searchBox.Focus();


                return true;
            }
            else if (keyData == (Keys.Control | Keys.D1))
            {
                LayoutManager.i.LoadLayout(LayoutManager.DefaultLayout);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D2))
            {
                //MessageBox.Show("received");
                LayoutManager.i.LoadLayout(LayoutManager.LargeWindow);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            {
                MessageBox.Show($"{masterSplitContainer.SplitterDistance}");


                return true;
            }

            // ctrl shift shortcuts --------------------------------------------------------------------------------
            //else if (keyData == (Keys.Control | Keys.Shift | Keys.D1))
            //{
            //    // Ctrl + Shift + 1
            //    return true;
            //}
            //else if (keyData == (Keys.Control | Keys.Shift | Keys.D2))
            //{
            //    // Ctrl + Shift + 2
            //    return true;
            //}



            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OpenTagModifier()
        {
            TagModifier tagM = new TagModifier(this);
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
            string title = "About";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information );
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the "ding" sound often associated with Enter key in textboxes
                e.SuppressKeyPress = true;
                e.Handled = true;

                GalleryManager.Populate(searchBox.Text);
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

            LayoutManager.i.LoadLayout(LayoutManager.DefaultLayout);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            LayoutManager.i.LoadLayout(LayoutManager.LargeWindow);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LibraryManager.i.OpenSourceFolder();
        }



    }
}
