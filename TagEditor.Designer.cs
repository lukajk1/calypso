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
            checkedListBoxTags = new CheckedListBox();
            statusStrip1 = new StatusStrip();
            panel1 = new Panel();
            label1 = new Label();
            newTagTextBox = new TextBox();
            buttonAddTag = new Button();
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // checkedListBox1
            // 
            checkedListBoxTags.BackColor = SystemColors.ControlLight;
            checkedListBoxTags.BorderStyle = BorderStyle.None;
            checkedListBoxTags.Dock = DockStyle.Fill;
            checkedListBoxTags.FormattingEnabled = true;
            checkedListBoxTags.Items.AddRange(new object[] { "item 1", "item 2" });
            checkedListBoxTags.Location = new Point(8, 8);
            checkedListBoxTags.Name = "checkedListBox1";
            checkedListBoxTags.Size = new Size(188, 313);
            checkedListBoxTags.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.BackColor = SystemColors.ControlLight;
            statusStrip1.Location = new Point(0, 409);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(204, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlLight;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(newTagTextBox);
            panel1.Controls.Add(buttonAddTag);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 329);
            panel1.Name = "panel1";
            panel1.Size = new Size(204, 80);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 44);
            label1.MaximumSize = new Size(180, 0);
            label1.Name = "label1";
            label1.Size = new Size(155, 30);
            label1.TabIndex = 2;
            label1.Text = "To delete a tag, select it and press [Del].";
            // 
            // newTagTextBox
            // 
            newTagTextBox.BackColor = SystemColors.Control;
            newTagTextBox.Location = new Point(12, 14);
            newTagTextBox.Name = "newTagTextBox";
            newTagTextBox.Size = new Size(104, 23);
            newTagTextBox.TabIndex = 1;
            // 
            // buttonAddTag
            // 
            buttonAddTag.Location = new Point(122, 14);
            buttonAddTag.Name = "buttonAddTag";
            buttonAddTag.Size = new Size(70, 25);
            buttonAddTag.TabIndex = 0;
            buttonAddTag.Text = "Add Tag";
            buttonAddTag.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ControlLight;
            panel2.Controls.Add(checkedListBoxTags);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(8);
            panel2.Size = new Size(204, 329);
            panel2.TabIndex = 3;
            // 
            // TagEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(204, 431);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            MaximizeBox = false;
            MaximumSize = new Size(220, 999);
            MinimumSize = new Size(220, 470);
            Name = "TagEditor";
            Text = "Tag Editor";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox checkedListBoxTags;
        private StatusStrip statusStrip1;
        private Panel panel1;
        public Button buttonAddTag;
        public TextBox newTagTextBox;
        private Label label1;
        private Panel panel2;
    }
}