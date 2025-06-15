using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Data;
namespace Calypso
{
    internal class Gallery
    {
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

        public enum GalleryMode
        {
            Directory,
            Tag
        }

        public static void Init(MainWindow mainW)
        {
            Gallery.mainW = mainW;
            Gallery.flowLayoutGallery = mainW.flowLayoutGallery;
            Gallery.imageContextMenuStrip = mainW.imageContextMenuStrip;  
            Gallery.pictureBoxPreview = mainW.pictureBoxImagePreview;

            flowLayoutGallery.AllowDrop = true;
            flowLayoutGallery.DragEnter += flowLayoutGallery_DragEnter;
            flowLayoutGallery.DragDrop += flowLayoutGallery_DragDrop;
        }


        //public static void Populate(string directoryPath)
        //{
        //    ClearExistingControls();

        //    if (!System.IO.Directory.Exists(directoryPath))
        //    {
        //        MessageBox.Show("invalid directory!");
        //        return;
        //    }

        //    string[] imageFiles = Util.GetAllImageFilepaths(directoryPath);

        //    StatusBar.ImagesLoaded = imageFiles.Length;
        //    GalleryManager.mainW.toolStripLabelThumbnailSize.Text = $"Thumbnail Height: {GlobalValues.ThumbnailHeight}px";

        //    GenerateContent(imageFiles);
        //}

        public static void Populate(List<ImageData> results)
        {
            ClearExistingControls();
            GenerateContent(results);
        }

        //private static void GenerateContent(string[] imageFiles)
        //{
        //    float processedCount = 0f;
        //    foreach (string file in imageFiles)
        //    {
        //        Image thumbnail = Util.CreateThumbnail(file, GlobalValues.ThumbnailHeight);

        //        PictureBox pb = new PictureBox
        //        {
        //            SizeMode = PictureBoxSizeMode.Zoom,
        //            Size = new Size(GlobalValues.ThumbnailHeight, GlobalValues.ThumbnailHeight),
        //            Image = thumbnail,
        //            Margin = new Padding(5),
        //            Tag = file
        //        };

        //        pb.DoubleClick += PictureBox_DoubleClick;
        //        pb.Click += PictureBox_Click;

        //        Label label = new Label
        //        {
        //            Text = Path.GetFileName(file),
        //            TextAlign = ContentAlignment.MiddleCenter,
        //            Dock = DockStyle.Bottom,
        //            AutoSize = false,
        //            Width = GlobalValues.ThumbnailHeight,
        //            Height = 20
        //        };

        //        Panel container = new Panel
        //        {
        //            Width = GlobalValues.ThumbnailHeight + 10,
        //            Height = GlobalValues.ThumbnailHeight + 30,
        //            Margin = new Padding(5), 
        //            BackColor = Color.DarkGray
        //        };

        //        AddDraggableHandlers(pb);

        //        pb.Dock = DockStyle.Top;
        //        container.Controls.Add(pb);
        //        container.Controls.Add(label);

        //        flowLayoutGallery.Controls.Add(container);
                
        //        processedCount++;
        //        LoadProgress = processedCount / imageFiles.Length;
        //        thumbnail.Dispose();
        //    }

        //    LoadProgress = 0f;
        //}
        private static void GenerateContent(List<ImageData> content)
        {
            Debug.WriteLine("displaying " +content.Count +" results");
            float processedCount = 0f;
            foreach (ImageData imageData in content)
            {
                PictureBox pb = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(GlobalValues.ThumbnailHeight, GlobalValues.ThumbnailHeight),
                    Image = Image.FromFile(imageData.ThumbnailPath),
                    Margin = new Padding(5),
                    Tag = imageData
                };

                pb.DoubleClick += PictureBox_DoubleClick;
                pb.Click += PictureBox_Click;

                Label label = new Label
                {
                    Text = imageData.Filename,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Bottom,
                    AutoSize = false,
                    Width = GlobalValues.ThumbnailHeight,
                    Height = 20
                };

                Panel container = new Panel
                {
                    Width = GlobalValues.ThumbnailHeight + 10,
                    Height = GlobalValues.ThumbnailHeight + 30,
                    Margin = new Padding(5), 
                    BackColor = Color.DarkGray
                };

                AddDraggableHandlers(pb);

                pb.Dock = DockStyle.Top;
                container.Controls.Add(pb);
                container.Controls.Add(label);

                flowLayoutGallery.Controls.Add(container);
                
                processedCount++;
                LoadProgress = processedCount / content.Count;
                //pb.Image.Dispose();
            }

            LoadProgress = 0f;
        }

        private static void ClearExistingControls()
        {
            List<Control> controlsToRemove = flowLayoutGallery.Controls.Cast<Control>().ToList();

            foreach (Control control in controlsToRemove)
            {
                if (control is TextBox) continue;
                flowLayoutGallery.Controls.Remove(control); // Remove from the panel
                control.Dispose(); // Dispose of the control to release its resources
            }
        }

        // events ------------------------------------------------------------------------------------------
        private static void flowLayoutGallery_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private static void flowLayoutGallery_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string targetDir = @"C:\Users\lukaj\OneDrive\Desktop\New folder (2)";

            foreach (string file in files)
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext is ".jpg" or ".jpeg" or ".png" or ".bmp" or ".gif")
                {
                    string destPath = Path.Combine(targetDir, Path.GetFileName(file));
                    File.Copy(file, destPath, overwrite: false);
                }
            }
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





    }
}
