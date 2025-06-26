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
namespace Calypso
{
    internal class Gallery
    {
        static FlowLayoutPanel? flowLayoutGallery; // FlowLayoutPanel is non-nullable by default, idk if it matters or not
        static ContextMenuStrip? imageContextMenuStrip; 
        static MainWindow? mainW;
        static PictureBox? pictureBoxPreview;
        static ToolStripStatusLabel? selectedCountLabel;
        static ToolStripStatusLabel? resultsCountLabel;

        static List<ImageData> lastSearch = new();
        static List<TileTag> selectedTiles = new();
        static List<TileTag> allTiles = new();

        private static float _loadProgress = 0f;
        private static int pbPerRow = 0;
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
            Gallery.selectedCountLabel = mainW.selectedCountLabel;
            Gallery.resultsCountLabel = mainW.statusLabelResultsCount;

            flowLayoutGallery.AllowDrop = true;
            flowLayoutGallery.DragEnter += flowLayoutGallery_DragEnter;
            flowLayoutGallery.DragDrop += flowLayoutGallery_DragDrop; 
            flowLayoutGallery.SizeChanged += FlowLayoutGallery_SizeChanged;
        }

        public static void Populate(List<ImageData> results)
        {
            lastSearch = results;
            ClearExistingControls();
            GenerateGallery(results);

            resultsCountLabel.Text = $"Results: {results.Count}";
        }
        private static void GenerateGallery(List<ImageData> results)
        {
            float processedCount = 0f;
            foreach (ImageData imageData in results)
            {
                AddCard(imageData);

                processedCount++;
                LoadProgress = processedCount / results.Count;
            }

            LoadProgress = 0f;
            CountPictureBoxesPerRow();
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

        private static void ClearSelection()
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
            selectedCountLabel.Text = $"Selected: ({selectedTiles.Count})";

            ImageInfoPanel.Display(tTag._ImageData);
        }

        public static void SelectAll()
        {
            foreach (Control control in flowLayoutGallery.Controls)
            {
                if (control is Panel panel &&
                    panel.Controls.OfType<PictureBox>().FirstOrDefault() is PictureBox pb &&
                    pb.Tag is TileTag tTag)
                {
                    AddToSelection(tTag);
                }
            }
        }

        public static void AddCard(ImageData imgData)
        {
            Label label = new Label
            {
                Text = imgData.Filename,
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
                Margin = new Padding(5)
            };
            using var stream = new FileStream(imgData.ThumbnailPath, FileMode.Open, FileAccess.Read);
            pb.Image = Image.FromStream(stream);

            TileTag tileTag = new TileTag
            {
                _ImageData = imgData,
                _Container = container,
                _PictureBox = pb
            };

            pb.Tag = tileTag;
            allTiles.Add(tileTag);

            pb.DoubleClick += PictureBox_DoubleClick;
            pb.MouseClick += PictureBox_MouseClick;
            AddDraggableHandlers(pb);

            pb.Dock = DockStyle.Top;
            container.Controls.Add(pb);
            container.Controls.Add(label);

            flowLayoutGallery.Controls.Add(container);
            //pb.Image.Dispose();
        }

        public static void DeleteSelected()
        {
            foreach (TileTag t in selectedTiles)
            {
                if (t._Container != null && flowLayoutGallery.Controls.Contains(t._Container))
                {
                    flowLayoutGallery.Controls.Remove(t._Container);
                    t._Container.Dispose();
                }

                if (t._PictureBox.Image != null)
                {
                    t._PictureBox.Image.Dispose();
                }
            }

            Database.DeleteImageData(selectedTiles.Select(t => t._ImageData).ToList());

            selectedTiles.Clear();
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
            List<ImageData> myList = Database.AddFilesToLibrary(files);
            
            foreach (ImageData img in myList)
            {
                AddCard(img);
                Debug.WriteLine("this was called");
            }
        }

        private static void FlowLayoutGallery_SizeChanged(object sender, EventArgs e)
        {
            CountPictureBoxesPerRow();
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

            bool ctrlHeld = (Control.ModifierKeys & Keys.Control) == Keys.Control;

            if (!ctrlHeld) ClearSelection();
            AddToSelection(tTag);

            //if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            //{
                
            //}

            if (e.Button == MouseButtons.Right && File.Exists(tTag._ImageData.FullResPath))
            {
                TagEditManager.Open(selectedTiles);
            }
        }

        public static void ArrowSelect(Keys keyData)
        {
            if (selectedTiles.Count != 1) return;

            int index = allTiles.IndexOf(selectedTiles[0]);
            int newIndex = 0;

            switch(keyData)
            {
                case Keys.Left:
                    newIndex = index - 1;
                    break;
                case Keys.Right:
                    newIndex = index + 1;
                    break;
                case Keys.Up:
                    newIndex = index - pbPerRow;
                    break;
                case Keys.Down:
                    newIndex = index + pbPerRow;
                    break;
            }

            if (newIndex >= 0 && newIndex < allTiles.Count)
            {
                ClearSelection();
                AddToSelection(allTiles[newIndex]);
            }
            else
            {
                return;
            }
        }

        public static int CountPictureBoxesPerRow()
        {
            int? currentRowY = null;
            int count = 0;

            foreach (Control ctrl in flowLayoutGallery.Controls)
            {
                if (ctrl is Panel panel)
                {
                    if (currentRowY == null)
                    {
                        currentRowY = panel.Top;
                    }

                    if (panel.Top == currentRowY)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            pbPerRow = count;
            return count;
        }
        public static void OpenTagEditorByCommand()
        {
            if (selectedTiles.Count > 0)
            {
                TagEditManager.Open(selectedTiles);
            }
        }

    }
}
