﻿using System;
using System.Collections.Generic;
using System.IO;
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
        static MainWindow? mainW;

        static Image senderImageData;

        static Label labelTags;
        static Label labelDimensions;
        static Label labelFilename;
        static Label labelFilesize;


        public static void Init(MainWindow mainW)
        {
            ImageInfoPanel.mainW = mainW;
            ImageInfoPanel.pictureBox = mainW.pictureBoxImagePreview;
            ImageInfoPanel.tableLayoutImageInfo = mainW.tableLayoutImageInfo;

            pictureBox.SizeChanged += (s, e) => DrawImage(senderImageData);

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

            // references, so each must be set to invidually
            labelFilename = new Label { Text = "--" };
            labelDimensions = new Label { Text = "--" }; 
            labelTags = new Label { Text = "--" }; 
            labelFilesize  = new Label { Text = "--" };

            tableLayoutImageInfo.Controls.Add(new Label { Text = "File Name" }, 0, 0);
            tableLayoutImageInfo.Controls.Add(labelFilename, 1, 0);

            tableLayoutImageInfo.Controls.Add(new Label { Text = "Dimensions" }, 0, 1);
            tableLayoutImageInfo.Controls.Add(labelDimensions, 1, 1);

            tableLayoutImageInfo.Controls.Add(new Label { Text = "Size" }, 0, 2);
            tableLayoutImageInfo.Controls.Add(labelFilesize, 1, 2);

            //tableLayoutImageInfo.Controls.Add(new Label { Text = "Date Created" }, 0, 3);
            //tableLayoutImageInfo.Controls.Add(new Label { Text = "--" }, 1, 3);

            //tableLayoutImageInfo.Controls.Add(new Label { Text = "Date Modified" }, 0, 4);
            //tableLayoutImageInfo.Controls.Add(new Label { Text = "--" }, 1, 4);

            tableLayoutImageInfo.Controls.Add(new Label { Text = "Tags" }, 0, 5);
            tableLayoutImageInfo.Controls.Add(labelTags, 1, 5);
        }
        public static void Display(ImageData imgData)
        {
            if (!Path.Exists(imgData.FullResPath))
            {
                Util.ShowErrorDialog("Error loading image.");
                return;
            }

            using (Image img = Image.FromFile(imgData.FullResPath))
            {
                Image clone = new Bitmap(img);
                senderImageData = clone; // memory leak proofing

                DrawImage(clone);
                SetTableInfo(imgData, clone);
            }


        }

        private static void SetTableInfo(ImageData imgData, Image img)
        {
            if (imgData.Tags.Count > 0) labelTags.Text = string.Join(", ", imgData.Tags);
            else labelTags.Text = "none";

            labelDimensions.Text = $"{img.Width} x {img.Height}";
            labelFilename.Text = imgData.Filename;

            long byteSize = new FileInfo(imgData.FullResPath).Length;
            string sizeStr = byteSize >= 1024 * 1024
                ? $"{byteSize / (1024.0 * 1024.0):F1} MB"
                : $"{byteSize / 1024.0:F1} KB";
            labelFilesize.Text = sizeStr;   
        }

        private static void DrawImage(Image img)
        {
            if (img == null) return;

            float boxRatio = (float)pictureBox.Width / pictureBox.Height;
            float imageRatio = (float)img.Width / img.Height;

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
                g.DrawImage(img, x, y, targetWidth, targetHeight);
            }

            pictureBox.Image = resized;
        }

    }
}
