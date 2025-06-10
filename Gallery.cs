using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalypsoExperiment1
{
    internal class Gallery
    {
        public static int thumbnailHeight = 100;

        public static void Populate(string directoryPath, FlowLayoutPanel flowLayoutPanel)
        {
            if (!Directory.Exists(directoryPath))
                return;

            string[] imageFiles = Directory.GetFiles(directoryPath, "*.*")
                                           .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                           .ToArray();



            foreach (string file in imageFiles)
            {
                Image thumbnail = CreateThumbnail(file);

                PictureBox pb = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(100, 100),
                    Image = thumbnail,
                    Margin = new Padding(5),
                    Tag = file
                };

                //pb.DoubleClick += Form1.PictureBox_DoubleClick;
                pb.DoubleClick += PictureBox_DoubleClick;

                flowLayoutPanel.Controls.Add(pb);
            }
        }

        private static void PictureBox_DoubleClick(object? sender, EventArgs e)
        {
            if (sender is PictureBox pb && pb.Tag is string imagePath && File.Exists(imagePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = imagePath,
                    UseShellExecute = true
                });
            }
        }

        public static Image CreateThumbnail(string imagePath)
        {
            using Image fullImage = Image.FromFile(imagePath);
            int originalWidth = fullImage.Width;
            int originalHeight = fullImage.Height;

            int newHeight = thumbnailHeight;
            int newWidth = (int)(originalWidth * (newHeight / (float)originalHeight));

            return fullImage.GetThumbnailImage(newWidth, newHeight, () => false, IntPtr.Zero);
        }




    }
}
