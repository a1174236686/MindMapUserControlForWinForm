using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WlxMindMap.UseControl
{
    public partial class CollapseNodeButton : UserControl
    {
        public CollapseNodeButton()
        {
            InitializeComponent();
            Button_label.BackColor = BackColorObj.Normaly.Value;                      
        }
        public Font ButtonFont
        {
            get { return Button_label.Font; }
            set
            {
                Button_label.Parent = this;
                Button_label.Font = value;
                this.tableLayoutPanel1.ColumnStyles[1].Width = Button_label.Width;
                this.tableLayoutPanel1.RowStyles[1].Height = Button_label.Height;
                
                this.Height = Button_label.Height + 2;
                this.Width = Button_label.Width + 2;
                this.tableLayoutPanel1.Controls.Add(Button_label, 1, 1);
            }
        }

        /// <summary>获取或设置当前按钮的展开状态        
        /// [true:展开；false：折叠]
        /// </summary>
        public bool IsExpand
        {
            get
            {
                return _IsExpand;
            }

            set
            {
                _IsExpand = value;
                if (_IsExpand)
                {
                    Button_label.Text = "-";
                }
                else
                {
                    Button_label.Text = "+";
                }
            }
        }
        private bool _IsExpand = true;

        private MindMapNodeContent.Text_MindMapNodeContent.MindMapNodeBackColor BackColorObj = new MindMapNodeContent.Text_MindMapNodeContent.MindMapNodeBackColor(Color.FromArgb(200, 200, 200));

        [Description("折叠按钮被按下")]
        public event Action CollapseButtonDown = null;

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

     

        private void Button_label_MouseEnter(object sender, EventArgs e)
        {
            Button_label.BackColor = BackColorObj.Enter.Value;
        }

        private void Button_label_MouseLeave(object sender, EventArgs e)
        {
            Button_label.BackColor = BackColorObj.Normaly.Value;
        }
        private void Button_label_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
            Button_label.BackColor = BackColorObj.Down.Value;
        
        }
        private void Button_label_MouseUp(object sender, MouseEventArgs e)
        {
            Button_label.BackColor = BackColorObj.Enter.Value;
            this.OnMouseUp(e);
            if (CollapseButtonDown != null) CollapseButtonDown();
        }



        private void tableLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            //Button_label_MouseUp(null, null);
            this.OnMouseUp(e);
        }

        private void tableLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        private void tableLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private void tableLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            this.OnMouseClick(e);
        }

        
    }
}
