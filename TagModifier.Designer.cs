namespace Calypso
{
    partial class TagModifier
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
            checkedListBox1 = new CheckedListBox();
            statusStrip1 = new StatusStrip();
            SuspendLayout();
            // 
            // checkedListBox1
            // 
            checkedListBox1.BackColor = SystemColors.ControlLight;
            checkedListBox1.BorderStyle = BorderStyle.None;
            checkedListBox1.Dock = DockStyle.Fill;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[] { "banana", "strawberry" });
            checkedListBox1.Location = new Point(0, 0);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(220, 465);
            checkedListBox1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 443);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(220, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // TagModifier
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(220, 465);
            Controls.Add(statusStrip1);
            Controls.Add(checkedListBox1);
            MaximizeBox = false;
            Name = "TagModifier";
            Text = "TagModifier";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox checkedListBox1;
        private StatusStrip statusStrip1;
    }
}