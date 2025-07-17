using System;
using System.Windows.Forms;

namespace Calypso
{
    public partial class TextPrompt : Form
    {
        public string ResultText { get; private set; } = string.Empty;

        public TextPrompt(MainWindow mainW, string message)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowIcon = false;
            label1.Text = message;

            buttonOK.Click += buttonOK_Click;
            buttonCancel.Click += buttonCancel_Click;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ResultText = newTagTextBox.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
