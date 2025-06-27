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
        private static void FlowLayoutGallery_MouseWheel(object sender, MouseEventArgs e)
        {
            ZoomFromWheel(e);
        }
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
            bool shiftHeld = (Control.ModifierKeys & Keys.Shift) == Keys.Shift;

            if (shiftHeld)
            {
                if (selectedTiles.Count == 1)
                {
                    int index1 = allTiles.IndexOf(selectedTiles[0]);
                    int index2 = allTiles.IndexOf(tTag);

                    if (index1 != -1 && index2 != -1)
                    {
                        int start = Math.Min(index1, index2);
                        int end = Math.Max(index1, index2);

                        for (int i = start; i <= end; i++)
                        {
                            AddToSelection(allTiles[i]);
                        }
                    }
                }
            }
            else if (!ctrlHeld)
            {
                ClearSelection();
            }
            AddToSelection(tTag);

            //if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {}

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

            newIndex = Math.Max(0, Math.Min(newIndex, allTiles.Count - 1));

            ClearSelection();
            AddToSelection(allTiles[newIndex]);
        }

    }
}
