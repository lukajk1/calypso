using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Calypso
{
    internal class ImageInfoPanel
    {

        static PictureBox? pictureBox;
        static PictureBox? senderPictureBox;
        static TableLayoutPanel? tableLayoutImageInfo;
        static MainWindow? main;
        static SplitContainer? splitContainer;
        static SplitContainer? splitHContainer;

        static int previousSplitterDistance;
        static bool mainPanelIsCollapsed;

        static int previousSplitterDistanceHorizontal;
        static bool isCollapsedHorizontal;
        public static void Init(MainWindow mainW)
        {
            ImageInfoPanel.pictureBox = mainW.pictureBoxImagePreview;
            ImageInfoPanel.tableLayoutImageInfo = mainW.tableLayoutImageInfo;
            ImageInfoPanel.main = mainW;
            ImageInfoPanel.splitContainer = mainW.masterSplitContainer;
            ImageInfoPanel.splitHContainer = main.imageInfoHorizontalSplitContainer;

            pictureBox.SizeChanged += (s, e) => Load(senderPictureBox);

            InfoTableSetup();
        }
        private static void InfoTableSetup()
        {
            tableLayoutImageInfo.RowCount = 0;
            tableLayoutImageInfo.ColumnCount = 2;
            tableLayoutImageInfo.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
            tableLayoutImageInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //tableLayoutImageInfo.ColumnStyles.Clear();
            //tableLayoutImageInfo.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));     // Column 0: fixed to content
            tableLayoutImageInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // Column 1: expands to fill


            tableLayoutImageInfo.Controls.Add(new Label { Text = "File Name" }, 0, 0);
            tableLayoutImageInfo.Controls.Add(new Label { Text = "--" }, 1, 0);

            tableLayoutImageInfo.Controls.Add(new Label { Text = "Dimensions" }, 0, 1);
            tableLayoutImageInfo.Controls.Add(new Label { Text = "--" }, 1, 1);

            tableLayoutImageInfo.Controls.Add(new Label { Text = "Size" }, 0, 2);
            tableLayoutImageInfo.Controls.Add(new Label { Text = "--" }, 1, 2);

            tableLayoutImageInfo.Controls.Add(new Label { Text = "Date Created" }, 0, 3);
            tableLayoutImageInfo.Controls.Add(new Label { Text = "--" }, 1, 3);

            tableLayoutImageInfo.Controls.Add(new Label { Text = "Date Modified" }, 0, 4);
            tableLayoutImageInfo.Controls.Add(new Label { Text = "--" }, 1, 4);

            tableLayoutImageInfo.Controls.Add(new Label { Text = "Tags" }, 0, 5);
            tableLayoutImageInfo.Controls.Add(new Label { Text = "banana, fruit cup, strawberry, ice cream" }, 1, 5);
        }
        public static void Load(PictureBox senderPictureBox)
        {
            if (senderPictureBox == null) return;
            ImageInfoPanel.senderPictureBox = senderPictureBox;

            string filePath = (string)senderPictureBox.Tag;

            tableLayoutImageInfo.GetControlFromPosition(1, 0).Text = Path.GetFileName(filePath);

            // calculate sizing and positioning
            using (var original = Image.FromFile(filePath))
            {
                float boxRatio = (float)pictureBox.Width / pictureBox.Height;
                float imageRatio = (float)original.Width / original.Height;

                int targetWidth, targetHeight;

                if (imageRatio > boxRatio)
                {
                    targetWidth = pictureBox.Width;
                    targetHeight = (int)(pictureBox.Width / imageRatio);
                }
                else
                {
                    targetHeight = pictureBox.Height;
                    targetWidth = (int)(pictureBox.Height * imageRatio);
                }

                var resized = new Bitmap(pictureBox.Width, pictureBox.Height);
                using (Graphics g = Graphics.FromImage(resized))
                {
                    g.Clear(Color.Transparent);
                    int x = (pictureBox.Width - targetWidth) / 2;
                    int y = (pictureBox.Height - targetHeight) / 2;
                    g.DrawImage(original, x, y, targetWidth, targetHeight);
                }

                pictureBox.Image = resized;
            }
        }

    }
}
