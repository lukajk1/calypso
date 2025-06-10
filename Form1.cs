namespace CalypsoExperiment1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Gallery.Init(this, flowLayoutPanel1, imageContextMenuStrip);
            StatusBar.Init(toolStripStatusLabelImageCount);
            TagTree.Init(tagTree);

            Gallery.Populate(@"C:\Users\lukaj\My Drive\art\art ref\may25");

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
            this.Close();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by lukajk");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the "ding" sound often associated with Enter key in textboxes
                e.SuppressKeyPress = true;
                e.Handled = true;

                Gallery.Populate(textBox1.Text);
            }
        }
    }
}
