namespace CalypsoExperiment1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Gallery.Populate(@"C:\Users\lukaj\My Drive\art\art ref\may25", flowLayoutPanel1);

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            flowLayoutPanel1.Width = this.ClientSize.Width;
        }

        //private void PictureBox1_DragEnter(object sender, DragEventArgs e)
        //{
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //        e.Effect = DragDropEffects.Copy;
        //}

        //private void PictureBox1_DragDrop(object sender, DragEventArgs e)
        //{
        //    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
        //    if (files.Length > 0)
        //        pictureBox1.Image = Image.FromFile(files[0]);
        //}

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
    }
}
