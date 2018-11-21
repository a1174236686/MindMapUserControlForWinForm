using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WlxMindMap.MindMapNodeContent
{
    public partial class Text_MindMapNodeContent : UserControl,IMindMapNodeContent
    {
        public Text_MindMapNodeContent()
        {
            InitializeComponent();
        }

        public object DataItem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ReSetContentSize()
        {
            throw new NotImplementedException();
        }


    }
}
