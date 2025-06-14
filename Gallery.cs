using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MetadataExtractor;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Xmp;
using System.Diagnostics;
namespace Calypso
{
    internal class Gallery
    {
        static int thumbnailHeight = 150;
        static FlowLayoutPanel? flowLayoutGallery; // FlowLayoutPanel is non-nullable by default, idk if it matters or not
        static ContextMenuStrip? imageContextMenuStrip; 
        static MainWindow? mainW;
        static PictureBox? pictureBoxPreview;

        private static float _loadProgress = 0f;
        public static float LoadProgress
        {
            get => _loadProgress;
            set
            {
                mainW.toolStripProgressBar1.Value = (int)(value * 100f);
            }
        }

        public static void Init(MainWindow mainW)
        {
            Gallery.mainW = mainW;
            Gallery.flowLayoutGallery = mainW.flowLayoutGallery;
            Gallery.imageContextMenuStrip = mainW.imageContextMenuStrip;  
            Gallery.pictureBoxPreview = mainW.pictureBoxImagePreview;



            //Debug.WriteLine("ddd"+Gallery.flowLayoutGallery);
            //Debug.WriteLine("ddd"+Gallery.imageContextMenuStrip);
            //Debug.WriteLine("ddd" + Gallery.pictureBoxPreview);
        }

        public static void Populate(string directoryPath)
        {
            List<Control> controlsToRemove = flowLayoutGallery.Controls.Cast<Control>().ToList();

            foreach (Control control in controlsToRemove)
            {
                if (control is TextBox) continue;
                flowLayoutGallery.Controls.Remove(control); // Remove from the panel
                control.Dispose(); // Dispose of the control to release its resources
            }

            if (!System.IO.Directory.Exists(directoryPath))
            {
                MessageBox.Show("invalid directory!");
                return;
            }

            string[] imageFiles = System.IO.Directory.GetFiles(directoryPath, "*.*")
                                           .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                           .ToArray();

            StatusBar.ImagesLoaded = imageFiles.Length;
            Gallery.mainW.toolStripLabelThumbnailSize.Text = $"Thumbnail Height: {thumbnailHeight}px";

            GenerateContent(imageFiles);

            Debug.WriteLine("testing one two three");

            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(imageFiles[0]);
            foreach (var directory in directories)
                foreach (var tag in directory.Tags)
                    Debug.WriteLine($"{directory.Name} - {tag.Name} = {tag.Description}");
        }

        //private static void PlaceKeywords(string[] keywords)
        //{

        //}

        //private static string[] GetKeywords(string filepath)
        //{
            
        //}

        private static void GenerateContent(string[] imageFiles)
        {
            float processedCount = 0f;
            foreach (string file in imageFiles)
            {
                Image thumbnail = CreateThumbnail(file);

                PictureBox pb = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(thumbnailHeight, thumbnailHeight),
                    Image = thumbnail,
                    Margin = new Padding(5),
                    Tag = file
                };

                pb.DoubleClick += PictureBox_DoubleClick;
                pb.Click += PictureBox_Click;

                Label label = new Label
                {
                    Text = Path.GetFileName(file),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Bottom,
                    AutoSize = false,
                    Width = thumbnailHeight,
                    Height = 20
                };

                Panel container = new Panel
                {
                    Width = thumbnailHeight + 10,
                    Height = thumbnailHeight + 30,
                    Margin = new Padding(5), 
                    BackColor = Color.DarkGray
                };

                AddDraggableHandlers(pb);

                pb.Dock = DockStyle.Top;
                container.Controls.Add(pb);
                container.Controls.Add(label);

                flowLayoutGallery.Controls.Add(container);
                
                processedCount++;
                LoadProgress = processedCount / imageFiles.Length;
            }

            LoadProgress = 0f;
        }

        private static void AddDraggableHandlers(PictureBox pb)
        {
            Point dragStartPoint = Point.Empty;

            pb.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    dragStartPoint = e.Location;
            };

            pb.MouseMove += (s, e) =>
            {
                if (e.Button == MouseButtons.Left &&
                    dragStartPoint != Point.Empty &&
                    (Math.Abs(e.X - dragStartPoint.X) > SystemInformation.DragSize.Width / 2 ||
                     Math.Abs(e.Y - dragStartPoint.Y) > SystemInformation.DragSize.Height / 2))
                {
                    if (pb.Tag is string imagePath && File.Exists(imagePath))
                    {
                        pb.DoDragDrop(new DataObject(DataFormats.FileDrop, new string[] { imagePath }), DragDropEffects.Copy);
                        dragStartPoint = Point.Empty; // reset
                    }
                }
            };
        }

        public static void RightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                imageContextMenuStrip.Show(e.Location);
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
        private static void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender is PictureBox clickedPictureBox)
            {
                //pictureBoxPreview.Image = clickedPictureBox.Image;
                ImageInfoPanel.Load(clickedPictureBox);
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
