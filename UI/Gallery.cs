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
using Calypso.UI;
using MetadataExtractor;
namespace Calypso
{
    internal class Gallery
    {
        static FlowLayoutPanel? flowLayoutGallery; // FlowLayoutPanel is non-nullable by default, idk if it matters or not
        static ContextMenuStrip? imageContextMenuStrip; 
        static MainWindow? mainW;
        static PictureBox? pictureBoxPreview;

        static List<ImageData> lastResults = new();
        static List<TileTag> selectedTiles = new();

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
            lastResults = results;
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
            float processedCount = 0f;
            foreach (ImageData imageData in content)
            {



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
                    Margin = new Padding(5)
                };

                PictureBox pb = new PictureBox
                {
                    Cursor = Cursors.Hand,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(GlobalValues.ThumbnailHeight, GlobalValues.ThumbnailHeight),
                    Image = Image.FromFile(imageData.ThumbnailPath),
                    Margin = new Padding(5)
                };

                TileTag tileTag = new TileTag
                {
                    _ImageData = imageData,
                    _Container = container,
                    _PictureBox = pb
                };

                pb.Tag = tileTag;

                pb.DoubleClick += PictureBox_DoubleClick;
                pb.MouseClick += PictureBox_MouseClick;
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
            string targetDir = Database.i.GetCurrentLibrary().DirectoryPath;

            foreach (string file in files)
            {
                string ext = Path.GetExtension(file).ToLower();
                if (ext is ".jpg" or ".jpeg" or ".png" or ".bmp" or ".gif")
                {
                    string destPath = Path.Combine(targetDir, Path.GetFileName(file));

                    if (!File.Exists(destPath))
                        File.Copy(file, destPath, overwrite: false);
                    else
                        Util.ShowErrorDialog($"A file named {Path.GetFileName(file)} already exists in {targetDir}!");
                }
            }
        }

        const int DragThreshold = 17; // (px) less sensitive than system default to avoid false positives more 
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
                    (Math.Abs(e.X - dragStartPoint.X) > DragThreshold ||
                     Math.Abs(e.Y - dragStartPoint.Y) > DragThreshold))
                {
                    if (pb.Tag is TileTag tTag && File.Exists(tTag._ImageData.FullResPath))
                    {
                        pb.DoDragDrop(
                            new DataObject(DataFormats.FileDrop, new string[] { tTag._ImageData.FullResPath }),
                            DragDropEffects.Copy);
                        dragStartPoint = Point.Empty;
                    }
                }
            };
        }



        private static void PictureBox_DoubleClick(object? sender, EventArgs e)
        {
            if (sender is PictureBox pb && pb.Tag is TileTag tTag && File.Exists(tTag._ImageData.FullResPath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tTag._ImageData.FullResPath,
                    UseShellExecute = true
                });
            }
        }
        private static void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender is not PictureBox pb || pb.Tag is not TileTag tTag)
                return;

            bool shiftHeld = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            if (!shiftHeld) ClearSelectionList();
            AddToSelection(tTag);

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                ImageInfoPanel.Display(tTag._ImageData);
                tTag._Container.BorderStyle = BorderStyle.FixedSingle;
            }

            if (e.Button == MouseButtons.Right && File.Exists(tTag._ImageData.FullResPath))
            {
                TagEditManager.Open(selectedTiles);
            }
        }

        public static void OpenTagEditorByCommand()
        {
            if (selectedTiles.Count > 0)
            {
                TagEditManager.Open(selectedTiles);
            }
        }

        private static void ClearSelectionList()
        {
            foreach (TileTag tTag in selectedTiles)
            {
                tTag._Container.BorderStyle = BorderStyle.None;
            }

            selectedTiles.Clear();
        }

        private static void AddToSelection(TileTag tTag)
        {
            selectedTiles.Add(tTag);
            tTag._Container.BorderStyle = BorderStyle.FixedSingle;
        }

    }
}
