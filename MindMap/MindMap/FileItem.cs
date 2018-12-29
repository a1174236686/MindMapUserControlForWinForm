using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MindMap
{
    public partial class FileItem : UserControl
    {
        public FileItem()
        {
            InitializeComponent();
            
        }

        public Image FileIco
        {
            get { return pictureBox1.Image; }
            set { pictureBox1.Image = value; }
        }
        public string FileName {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public string FilePath { set; get; }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.FromName("control");
        }

        private void label1_Click(object sender, EventArgs e)
        {            
            Process.Start(FilePath);
        }
    }
}
