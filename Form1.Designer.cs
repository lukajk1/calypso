namespace Calypso
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newGalleryToolStripMenuItem = new ToolStripMenuItem();
            openExistingLibraryToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripMenuItem();
            toolStripMenuItem7 = new ToolStripMenuItem();
            toolStripMenuItem9 = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            toolStripMenuItem4 = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripMenuItem6 = new ToolStripMenuItem();
            testToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            flowLayoutGallery = new FlowLayoutPanel();
            searchBox = new TextBox();
            statusStrip1 = new StatusStrip();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripStatusLabelImageCount = new ToolStripStatusLabel();
            toolStripLabelThumbnailSize = new ToolStripStatusLabel();
            BottomToolStripPanel = new ToolStripPanel();
            TopToolStripPanel = new ToolStripPanel();
            RightToolStripPanel = new ToolStripPanel();
            LeftToolStripPanel = new ToolStripPanel();
            ContentPanel = new ToolStripContentPanel();
            imageContextMenuStrip = new ContextMenuStrip(components);
            item1ToolStripMenuItem = new ToolStripMenuItem();
            item2ToolStripMenuItem = new ToolStripMenuItem();
            tagTree = new TreeView();
            searchPanel = new Panel();
            comboBox1 = new ComboBox();
            checkBoxRandomize = new CheckBox();
            tableLayoutImageInfo = new TableLayoutPanel();
            button1 = new Button();
            pictureBoxImagePreview = new PictureBox();
            middleFillPanelContainer = new Panel();
            tagTreeGallerySplitContainer = new SplitContainer();
            horizontalLeftSplitContainer = new SplitContainer();
            savedSearchesTreeView = new TreeView();
            imageInfoHorizontalSplitContainer = new SplitContainer();
            imageMetadataPanel = new Panel();
            panel1 = new Panel();
            button2 = new Button();
            masterSplitContainer = new SplitContainer();
            masterPanelWrapper = new Panel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            imageContextMenuStrip.SuspendLayout();
            searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImagePreview).BeginInit();
            middleFillPanelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tagTreeGallerySplitContainer).BeginInit();
            tagTreeGallerySplitContainer.Panel1.SuspendLayout();
            tagTreeGallerySplitContainer.Panel2.SuspendLayout();
            tagTreeGallerySplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)horizontalLeftSplitContainer).BeginInit();
            horizontalLeftSplitContainer.Panel1.SuspendLayout();
            horizontalLeftSplitContainer.Panel2.SuspendLayout();
            horizontalLeftSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imageInfoHorizontalSplitContainer).BeginInit();
            imageInfoHorizontalSplitContainer.Panel1.SuspendLayout();
            imageInfoHorizontalSplitContainer.Panel2.SuspendLayout();
            imageInfoHorizontalSplitContainer.SuspendLayout();
            imageMetadataPanel.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)masterSplitContainer).BeginInit();
            masterSplitContainer.Panel1.SuspendLayout();
            masterSplitContainer.Panel2.SuspendLayout();
            masterSplitContainer.SuspendLayout();
            masterPanelWrapper.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem, testToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1384, 24);
            menuStrip1.TabIndex = 6;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newGalleryToolStripMenuItem, openExistingLibraryToolStripMenuItem, toolStripMenuItem9, toolStripMenuItem1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newGalleryToolStripMenuItem
            // 
            newGalleryToolStripMenuItem.Name = "newGalleryToolStripMenuItem";
            newGalleryToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + N";
            newGalleryToolStripMenuItem.Size = new Size(245, 22);
            newGalleryToolStripMenuItem.Text = "&New Library...";
            // 
            // openExistingLibraryToolStripMenuItem
            // 
            openExistingLibraryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem8, toolStripMenuItem7 });
            openExistingLibraryToolStripMenuItem.Name = "openExistingLibraryToolStripMenuItem";
            openExistingLibraryToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + O";
            openExistingLibraryToolStripMenuItem.Size = new Size(245, 22);
            openExistingLibraryToolStripMenuItem.Text = "&Open Library";
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Enabled = false;
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new Size(170, 22);
            toolStripMenuItem8.Text = "Library 1 (Current)";
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new Size(170, 22);
            toolStripMenuItem7.Text = "Library 2";
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.ShortcutKeyDisplayString = "Ctrl + I";
            toolStripMenuItem9.Size = new Size(245, 22);
            toolStripMenuItem9.Text = "&Add Files to Library...";
            toolStripMenuItem9.Click += toolStripMenuItem9_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl + Enter";
            toolStripMenuItem1.Size = new Size(245, 22);
            toolStripMenuItem1.Text = "Open Library &Folder";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + Q";
            exitToolStripMenuItem.Size = new Size(245, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem2 });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "&View";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem5, toolStripMenuItem3, toolStripSeparator1, toolStripMenuItem4, toolStripSeparator2, toolStripMenuItem6 });
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(110, 22);
            toolStripMenuItem2.Text = "Layout";
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.ShortcutKeyDisplayString = "Ctrl + 1";
            toolStripMenuItem5.Size = new Size(240, 22);
            toolStripMenuItem5.Text = "Default";
            toolStripMenuItem5.Click += toolStripMenuItem5_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.ShortcutKeyDisplayString = "Ctrl + 2";
            toolStripMenuItem3.Size = new Size(240, 22);
            toolStripMenuItem3.Text = "Large Preview Window";
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(237, 6);
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.ShortcutKeyDisplayString = "";
            toolStripMenuItem4.Size = new Size(240, 22);
            toolStripMenuItem4.Text = "Save Current Layout...";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(237, 6);
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.ShortcutKeyDisplayString = "Ctrl + 3";
            toolStripMenuItem6.Size = new Size(240, 22);
            toolStripMenuItem6.Text = "Custom Layout 1";
            // 
            // testToolStripMenuItem
            // 
            testToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            testToolStripMenuItem.Name = "testToolStripMenuItem";
            testToolStripMenuItem.Size = new Size(44, 20);
            testToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "&About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // flowLayoutGallery
            // 
            flowLayoutGallery.AutoScroll = true;
            flowLayoutGallery.AutoSize = true;
            flowLayoutGallery.BackColor = SystemColors.ControlLight;
            flowLayoutGallery.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutGallery.Dock = DockStyle.Fill;
            flowLayoutGallery.Location = new Point(0, 53);
            flowLayoutGallery.Name = "flowLayoutGallery";
            flowLayoutGallery.Padding = new Padding(12, 12, 12, 45);
            flowLayoutGallery.Size = new Size(705, 762);
            flowLayoutGallery.TabIndex = 7;
            // 
            // searchBox
            // 
            searchBox.BackColor = SystemColors.ControlLight;
            searchBox.BorderStyle = BorderStyle.FixedSingle;
            searchBox.Location = new Point(16, 14);
            searchBox.Name = "searchBox";
            searchBox.Size = new Size(389, 23);
            searchBox.TabIndex = 11;
            searchBox.KeyDown += textBox1_KeyDown;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.Control;
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripProgressBar1, toolStripStatusLabelImageCount, toolStripLabelThumbnailSize });
            statusStrip1.Location = new Point(0, 839);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1384, 22);
            statusStrip1.TabIndex = 9;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(100, 16);
            // 
            // toolStripStatusLabelImageCount
            // 
            toolStripStatusLabelImageCount.Name = "toolStripStatusLabelImageCount";
            toolStripStatusLabelImageCount.Padding = new Padding(15, 0, 0, 0);
            toolStripStatusLabelImageCount.Size = new Size(107, 17);
            toolStripStatusLabelImageCount.Text = "Image Count: --";
            toolStripStatusLabelImageCount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripLabelThumbnailSize
            // 
            toolStripLabelThumbnailSize.Name = "toolStripLabelThumbnailSize";
            toolStripLabelThumbnailSize.Size = new Size(132, 17);
            toolStripLabelThumbnailSize.Text = "Thumbnail Height: --px";
            // 
            // BottomToolStripPanel
            // 
            BottomToolStripPanel.Location = new Point(0, 0);
            BottomToolStripPanel.Name = "BottomToolStripPanel";
            BottomToolStripPanel.Orientation = Orientation.Horizontal;
            BottomToolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            BottomToolStripPanel.Size = new Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            TopToolStripPanel.Location = new Point(0, 0);
            TopToolStripPanel.Name = "TopToolStripPanel";
            TopToolStripPanel.Orientation = Orientation.Horizontal;
            TopToolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            TopToolStripPanel.Size = new Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            RightToolStripPanel.Location = new Point(0, 0);
            RightToolStripPanel.Name = "RightToolStripPanel";
            RightToolStripPanel.Orientation = Orientation.Horizontal;
            RightToolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            RightToolStripPanel.Size = new Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            LeftToolStripPanel.Location = new Point(0, 0);
            LeftToolStripPanel.Name = "LeftToolStripPanel";
            LeftToolStripPanel.Orientation = Orientation.Horizontal;
            LeftToolStripPanel.RowMargin = new Padding(3, 0, 0, 0);
            LeftToolStripPanel.Size = new Size(0, 0);
            // 
            // ContentPanel
            // 
            ContentPanel.Size = new Size(312, 292);
            // 
            // imageContextMenuStrip
            // 
            imageContextMenuStrip.Items.AddRange(new ToolStripItem[] { item1ToolStripMenuItem, item2ToolStripMenuItem });
            imageContextMenuStrip.Name = "contextMenuStrip1";
            imageContextMenuStrip.Size = new Size(105, 48);
            // 
            // item1ToolStripMenuItem
            // 
            item1ToolStripMenuItem.Name = "item1ToolStripMenuItem";
            item1ToolStripMenuItem.Size = new Size(104, 22);
            item1ToolStripMenuItem.Text = "item1";
            // 
            // item2ToolStripMenuItem
            // 
            item2ToolStripMenuItem.Name = "item2ToolStripMenuItem";
            item2ToolStripMenuItem.Size = new Size(104, 22);
            item2ToolStripMenuItem.Text = "item2";
            // 
            // tagTree
            // 
            tagTree.BackColor = SystemColors.ControlLight;
            tagTree.Dock = DockStyle.Fill;
            tagTree.Location = new Point(0, 0);
            tagTree.Name = "tagTree";
            tagTree.Size = new Size(293, 389);
            tagTree.TabIndex = 10;
            // 
            // searchPanel
            // 
            searchPanel.BackColor = SystemColors.ControlLight;
            searchPanel.BorderStyle = BorderStyle.FixedSingle;
            searchPanel.Controls.Add(comboBox1);
            searchPanel.Controls.Add(checkBoxRandomize);
            searchPanel.Controls.Add(searchBox);
            searchPanel.Dock = DockStyle.Top;
            searchPanel.Location = new Point(0, 0);
            searchPanel.Name = "searchPanel";
            searchPanel.Size = new Size(705, 53);
            searchPanel.TabIndex = 12;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "All", "25", "50" });
            comboBox1.Location = new Point(440, 29);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(66, 23);
            comboBox1.TabIndex = 13;
            // 
            // checkBoxRandomize
            // 
            checkBoxRandomize.AutoSize = true;
            checkBoxRandomize.Location = new Point(440, 15);
            checkBoxRandomize.Name = "checkBoxRandomize";
            checkBoxRandomize.Size = new Size(85, 19);
            checkBoxRandomize.TabIndex = 12;
            checkBoxRandomize.Text = "Randomize";
            checkBoxRandomize.UseVisualStyleBackColor = true;
            // 
            // tableLayoutImageInfo
            // 
            tableLayoutImageInfo.AutoSize = true;
            tableLayoutImageInfo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutImageInfo.BackColor = SystemColors.ControlLight;
            tableLayoutImageInfo.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutImageInfo.ColumnCount = 1;
            tableLayoutImageInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutImageInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutImageInfo.Dock = DockStyle.Top;
            tableLayoutImageInfo.Location = new Point(0, 0);
            tableLayoutImageInfo.Name = "tableLayoutImageInfo";
            tableLayoutImageInfo.Padding = new Padding(3, 13, 3, 0);
            tableLayoutImageInfo.RowCount = 2;
            tableLayoutImageInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutImageInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutImageInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutImageInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutImageInfo.Size = new Size(354, 56);
            tableLayoutImageInfo.TabIndex = 3;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            button1.Location = new Point(9, 20);
            button1.Name = "button1";
            button1.Size = new Size(180, 30);
            button1.TabIndex = 0;
            button1.Text = "[T] Open Tag Editor";
            button1.UseVisualStyleBackColor = true;
            // 
            // pictureBoxImagePreview
            // 
            pictureBoxImagePreview.BackColor = SystemColors.ControlDarkDark;
            pictureBoxImagePreview.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxImagePreview.Dock = DockStyle.Fill;
            pictureBoxImagePreview.Location = new Point(0, 0);
            pictureBoxImagePreview.Name = "pictureBoxImagePreview";
            pictureBoxImagePreview.Size = new Size(356, 548);
            pictureBoxImagePreview.TabIndex = 0;
            pictureBoxImagePreview.TabStop = false;
            // 
            // middleFillPanelContainer
            // 
            middleFillPanelContainer.Controls.Add(flowLayoutGallery);
            middleFillPanelContainer.Controls.Add(searchPanel);
            middleFillPanelContainer.Dock = DockStyle.Fill;
            middleFillPanelContainer.Location = new Point(0, 0);
            middleFillPanelContainer.Name = "middleFillPanelContainer";
            middleFillPanelContainer.Size = new Size(705, 815);
            middleFillPanelContainer.TabIndex = 14;
            // 
            // tagTreeGallerySplitContainer
            // 
            tagTreeGallerySplitContainer.Dock = DockStyle.Fill;
            tagTreeGallerySplitContainer.Location = new Point(0, 0);
            tagTreeGallerySplitContainer.Name = "tagTreeGallerySplitContainer";
            // 
            // tagTreeGallerySplitContainer.Panel1
            // 
            tagTreeGallerySplitContainer.Panel1.Controls.Add(horizontalLeftSplitContainer);
            // 
            // tagTreeGallerySplitContainer.Panel2
            // 
            tagTreeGallerySplitContainer.Panel2.Controls.Add(middleFillPanelContainer);
            tagTreeGallerySplitContainer.Size = new Size(1005, 815);
            tagTreeGallerySplitContainer.SplitterDistance = 293;
            tagTreeGallerySplitContainer.SplitterWidth = 7;
            tagTreeGallerySplitContainer.TabIndex = 17;
            // 
            // horizontalLeftSplitContainer
            // 
            horizontalLeftSplitContainer.Dock = DockStyle.Fill;
            horizontalLeftSplitContainer.Location = new Point(0, 0);
            horizontalLeftSplitContainer.Name = "horizontalLeftSplitContainer";
            horizontalLeftSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // horizontalLeftSplitContainer.Panel1
            // 
            horizontalLeftSplitContainer.Panel1.Controls.Add(tagTree);
            // 
            // horizontalLeftSplitContainer.Panel2
            // 
            horizontalLeftSplitContainer.Panel2.Controls.Add(savedSearchesTreeView);
            horizontalLeftSplitContainer.Size = new Size(293, 815);
            horizontalLeftSplitContainer.SplitterDistance = 389;
            horizontalLeftSplitContainer.TabIndex = 11;
            // 
            // savedSearchesTreeView
            // 
            savedSearchesTreeView.BackColor = SystemColors.ControlLight;
            savedSearchesTreeView.Dock = DockStyle.Fill;
            savedSearchesTreeView.Location = new Point(0, 0);
            savedSearchesTreeView.Name = "savedSearchesTreeView";
            savedSearchesTreeView.Size = new Size(293, 422);
            savedSearchesTreeView.TabIndex = 11;
            // 
            // imageInfoHorizontalSplitContainer
            // 
            imageInfoHorizontalSplitContainer.BackColor = SystemColors.ControlLight;
            imageInfoHorizontalSplitContainer.Dock = DockStyle.Fill;
            imageInfoHorizontalSplitContainer.Location = new Point(0, 0);
            imageInfoHorizontalSplitContainer.Name = "imageInfoHorizontalSplitContainer";
            imageInfoHorizontalSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // imageInfoHorizontalSplitContainer.Panel1
            // 
            imageInfoHorizontalSplitContainer.Panel1.Controls.Add(pictureBoxImagePreview);
            // 
            // imageInfoHorizontalSplitContainer.Panel2
            // 
            imageInfoHorizontalSplitContainer.Panel2.Controls.Add(imageMetadataPanel);
            imageInfoHorizontalSplitContainer.Size = new Size(356, 815);
            imageInfoHorizontalSplitContainer.SplitterDistance = 548;
            imageInfoHorizontalSplitContainer.SplitterWidth = 5;
            imageInfoHorizontalSplitContainer.TabIndex = 0;
            // 
            // imageMetadataPanel
            // 
            imageMetadataPanel.BorderStyle = BorderStyle.FixedSingle;
            imageMetadataPanel.Controls.Add(panel1);
            imageMetadataPanel.Controls.Add(tableLayoutImageInfo);
            imageMetadataPanel.Dock = DockStyle.Fill;
            imageMetadataPanel.Location = new Point(0, 0);
            imageMetadataPanel.Name = "imageMetadataPanel";
            imageMetadataPanel.Size = new Size(356, 262);
            imageMetadataPanel.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 201);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(6);
            panel1.Size = new Size(354, 59);
            panel1.TabIndex = 1;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            button2.Location = new Point(113, 20);
            button2.Name = "button2";
            button2.Size = new Size(180, 30);
            button2.TabIndex = 1;
            button2.Text = "Random Image";
            button2.UseVisualStyleBackColor = true;
            // 
            // masterSplitContainer
            // 
            masterSplitContainer.Dock = DockStyle.Fill;
            masterSplitContainer.Location = new Point(8, 0);
            masterSplitContainer.Name = "masterSplitContainer";
            // 
            // masterSplitContainer.Panel1
            // 
            masterSplitContainer.Panel1.Controls.Add(tagTreeGallerySplitContainer);
            // 
            // masterSplitContainer.Panel2
            // 
            masterSplitContainer.Panel2.Controls.Add(imageInfoHorizontalSplitContainer);
            masterSplitContainer.Size = new Size(1368, 815);
            masterSplitContainer.SplitterDistance = 1005;
            masterSplitContainer.SplitterWidth = 7;
            masterSplitContainer.TabIndex = 0;
            // 
            // masterPanelWrapper
            // 
            masterPanelWrapper.Controls.Add(masterSplitContainer);
            masterPanelWrapper.Dock = DockStyle.Fill;
            masterPanelWrapper.Location = new Point(0, 24);
            masterPanelWrapper.Name = "masterPanelWrapper";
            masterPanelWrapper.Padding = new Padding(8, 0, 8, 0);
            masterPanelWrapper.Size = new Size(1384, 815);
            masterPanelWrapper.TabIndex = 0;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1384, 861);
            Controls.Add(masterPanelWrapper);
            Controls.Add(menuStrip1);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(450, 450);
            Name = "MainWindow";
            Text = "Calypso";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            imageContextMenuStrip.ResumeLayout(false);
            searchPanel.ResumeLayout(false);
            searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImagePreview).EndInit();
            middleFillPanelContainer.ResumeLayout(false);
            middleFillPanelContainer.PerformLayout();
            tagTreeGallerySplitContainer.Panel1.ResumeLayout(false);
            tagTreeGallerySplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tagTreeGallerySplitContainer).EndInit();
            tagTreeGallerySplitContainer.ResumeLayout(false);
            horizontalLeftSplitContainer.Panel1.ResumeLayout(false);
            horizontalLeftSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)horizontalLeftSplitContainer).EndInit();
            horizontalLeftSplitContainer.ResumeLayout(false);
            imageInfoHorizontalSplitContainer.Panel1.ResumeLayout(false);
            imageInfoHorizontalSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)imageInfoHorizontalSplitContainer).EndInit();
            imageInfoHorizontalSplitContainer.ResumeLayout(false);
            imageMetadataPanel.ResumeLayout(false);
            imageMetadataPanel.PerformLayout();
            panel1.ResumeLayout(false);
            masterSplitContainer.Panel1.ResumeLayout(false);
            masterSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)masterSplitContainer).EndInit();
            masterSplitContainer.ResumeLayout(false);
            masterPanelWrapper.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        public MenuStrip menuStrip1;
        public ToolStripMenuItem testToolStripMenuItem;
        public ToolStripMenuItem aboutToolStripMenuItem;
        public ToolStripMenuItem fileToolStripMenuItem;
        public ToolStripMenuItem exitToolStripMenuItem;
        public FlowLayoutPanel flowLayoutGallery;
        public StatusStrip statusStrip1;
        public ToolStripPanel BottomToolStripPanel;
        public ToolStripPanel TopToolStripPanel;
        public ToolStripPanel RightToolStripPanel;
        public ToolStripPanel LeftToolStripPanel;
        public ToolStripContentPanel ContentPanel;
        public ToolStripStatusLabel toolStripStatusLabelImageCount;
        public ContextMenuStrip imageContextMenuStrip;
        public ToolStripMenuItem item1ToolStripMenuItem;
        public ToolStripMenuItem item2ToolStripMenuItem;
        public TreeView tagTree;
        public ToolStripProgressBar toolStripProgressBar1;
        public TextBox searchBox;
        public Panel searchPanel;
        public ToolStripMenuItem viewToolStripMenuItem;
        public ToolStripStatusLabel toolStripLabelThumbnailSize;
        public Panel middleFillPanelContainer;
        public Splitter splitterRight;
        public PictureBox pictureBoxImagePreview;
        public TableLayoutPanel tableLayoutImageInfo;
        public ToolStripMenuItem newGalleryToolStripMenuItem;
        public ToolStripMenuItem openExistingLibraryToolStripMenuItem;
        public ToolStripMenuItem toolStripMenuItem2;
        public ToolStripMenuItem toolStripMenuItem3;
        public ToolStripMenuItem toolStripMenuItem4;
        public ToolStripMenuItem toolStripMenuItem5;
        public ToolStripSeparator toolStripSeparator1;
        public ToolStripSeparator toolStripSeparator2;
        public ToolStripMenuItem toolStripMenuItem6;
        public CheckBox checkBoxRandomize;
        public SplitContainer tagTreeGallerySplitContainer;
        public SplitContainer imageInfoHorizontalSplitContainer;
        public SplitContainer masterSplitContainer;
        public Button button1;
        public SplitContainer horizontalLeftSplitContainer;
        public TreeView savedSearchesTreeView;
        public Panel masterPanelWrapper;
        public Panel panel1;
        public Panel imageMetadataPanel;
        public Button button2;
        private ToolStripMenuItem toolStripMenuItem7;
        private ComboBox comboBox1;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private ToolStripMenuItem toolStripMenuItem1;
    }
}
