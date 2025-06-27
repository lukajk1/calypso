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
    internal partial class Gallery
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

        private static int pbPerRow = 0;
        private static float _loadProgress = 0f;
        public static float LoadProgress
        {
            get => _loadProgress;
            set
            {
                mainW.toolStripProgressBar1.Value = (int)(value * 100f);

                if (value == 0) mainW.toolStripProgressBar1.Visible = false;
                else mainW.toolStripProgressBar1.Visible = true;
            }
        }
        private static float _zoom = 1f;
        public static float Zoom
        {
            get => _zoom;
            set
            {
                if (value > 0)
                {
                    _zoom = value;
                    SetZoom(value);
                }
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
            flowLayoutGallery.MouseWheel += FlowLayoutGallery_MouseWheel;
        }

        public static void Populate(List<ImageData> results)
        {
            lastSearch = results;
            ClearExistingControls();
            GenerateGallery(results);

            resultsCountLabel.Text = $"Results: {results.Count}";
            mainW.toolStripLabelThumbnailSize.Text = $"Thumbnail Height: {GlobalValues.DefaultThumbnailSize}px";
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
        public static void ZoomFromWheel(MouseEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.Delta > 0)
                    Gallery.Zoom += 0.2f;
                else if (e.Delta < 0)
                    Gallery.Zoom -= 0.2f;
            }
        }
        private static void SetZoom(float magnifyingFactor)
        {
            int thumbSize = (int)(GlobalValues.DefaultThumbnailSize * magnifyingFactor);

            foreach (TileTag tile in allTiles)
            {
                tile._PictureBox.Size = new Size(thumbSize, thumbSize);

                Label? label = tile._Container.Controls.OfType<Label>().FirstOrDefault();
                int labelHeight = label?.Height ?? 20;

                tile._Container.Width = thumbSize + 10;
                tile._Container.Height = thumbSize + labelHeight + 10;
            }

            mainW.toolStripLabelThumbnailSize.Text = $"Thumbnail Height: {thumbSize}px";
            flowLayoutGallery.PerformLayout();
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
            ClearSelection(); // Optional, depending on behavior

            foreach (TileTag tTag in allTiles)
            {
                tTag._Container.BorderStyle = BorderStyle.FixedSingle;
            }

            selectedTiles = new List<TileTag>(allTiles);
            selectedCountLabel.Text = $"Selected: ({selectedTiles.Count})";

            if (selectedTiles.Count > 0)
                ImageInfoPanel.Display(selectedTiles[0]._ImageData);
        }


        public static void AddCard(ImageData imgData)
        {
            Label label = new Label
            {
                Text = imgData.Filename,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                AutoSize = false,
                Width = GlobalValues.DefaultThumbnailSize,
                Height = 20
            };

            Panel container = new Panel
            {
                Width = GlobalValues.DefaultThumbnailSize + 10,
                Height = GlobalValues.DefaultThumbnailSize + 30,
                Margin = new Padding(5)
            };


            PictureBox pb = new PictureBox
            {
                Cursor = Cursors.Hand,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(GlobalValues.DefaultThumbnailSize, GlobalValues.DefaultThumbnailSize),
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
