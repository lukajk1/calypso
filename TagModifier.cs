using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calypso
{
    public partial class TagModifier : Form
    {
        private MainWindow? main;

        public TagModifier(MainWindow main)
        {
            this.main = main;
            InitializeComponent();

            this.Deactivate += OnFormDeactivate;

        }

        private void OnFormDeactivate(object? sender, EventArgs e)
        {
            main.Activate();
            main.Focus();
            this.Close();
        }
    }
}
