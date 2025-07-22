namespace Calypso
{
    partial class TagEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            statusStrip1 = new StatusStrip();
            panel2 = new Panel();
            tagEditorTree = new TreeView();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.ControlLight;
            statusStrip1.Location = new Point(0, 409);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(254, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlLight;
            panel2.Controls.Add(tagEditorTree);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(8);
            panel2.Size = new Size(254, 409);
            panel2.TabIndex = 3;
            // 
            // tagEditorTree
            // 
            tagEditorTree.BackColor = SystemColors.ControlLight;
            tagEditorTree.Dock = DockStyle.Fill;
            tagEditorTree.Location = new Point(8, 8);
            tagEditorTree.Name = "tagEditorTree";
            tagEditorTree.Size = new Size(238, 393);
            tagEditorTree.TabIndex = 0;
            // 
            // TagEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(254, 431);
            Controls.Add(panel2);
            Controls.Add(statusStrip1);
            MaximizeBox = false;
            MaximumSize = new Size(600, 999);
            MinimumSize = new Size(270, 470);
            Name = "TagEditor";
            Text = "Tag Editor";
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private Panel panel2;
        private TreeView tagEditorTree;
    }
}