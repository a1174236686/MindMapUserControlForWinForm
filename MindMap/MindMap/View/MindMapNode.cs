using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMap.View
{
    public partial class MindMapNode : UserControl
    {
        public MindMapNode()
        {
            InitializeComponent();
        }

        #region 属性


        private MindMapNode _ParentNode = null;
        /// <summary> 设置或获取父节点
        /// 
        /// </summary>
        public MindMapNode ParentNode
        {
            set { _ParentNode = value; }
            get { return _ParentNode; }
        }

        /// <summary> 设置当前节点的背景颜色
        /// 
        /// </summary>
        public Color _NodeBackColor
        {
            get { return Content_lable.BackColor; }
            set { Content_lable.BackColor = value; }
        }

        private Font g_TextFont = new Font(new FontFamily("微软雅黑"), 12);
        /// <summary> 当前节点的字体
        /// 
        /// </summary>
        public Font TextFont
        {
            get
            {
                return g_TextFont;
            }
            set
            {
                g_TextFont = value;
                Content_lable.Font = g_TextFont;//设置字体
                ResetNodeSize();//重新设置节点尺寸

            }
        }

        private TreeNode _TreeNode = null;
        /// <summary> 设置当前节点的内容
        /// 
        /// </summary>
        public TreeNode TreeNode
        {
            set
            {
                if (value == null) return;
                _TreeNode = value;
                Content_lable.Text = _TreeNode.Text;
                Content_lable.Font = g_TextFont;
                Content_lable.ForeColor = Color.FromArgb(255, 255, 255);
                Content_lable.BackColor = Color.FromArgb(48, 120, 215);

                Chidren_Panel.Controls.Clear();
                foreach (TreeNode TreeNodeItem in _TreeNode.Nodes)
                {
                    MindMapNode MindMapNodeTemp = new MindMapNode();
                    MindMapNodeTemp.TextFont = g_TextFont;
                    MindMapNodeTemp.TreeNode = TreeNodeItem;
                    MindMapNodeTemp.Margin = new Padding(0, 2, 0, 2);
                    Chidren_Panel.Controls.Add(MindMapNodeTemp);
                }
                ReSetSize();
                DrawingConnectLine();
            }
        }

        private bool _Selected = false;
        /// <summary>当前节点是否被选中
        /// 
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
            }
        }
        #endregion 属性               

        #region 方法

        /// <summary> 再面板上画出当前节点的连接线
        /// 
        /// </summary>
        private void DrawingConnectLine()
        {
            int CurrentNodeHeightCenter = Content_Panel.Height / 2;
            Point StartPoint = new Point(0, CurrentNodeHeightCenter);

            Graphics LineGraphics = DrawingLine_panel.CreateGraphics();
            LineGraphics.Clear(DrawingLine_panel.BackColor);//清除之前的
            LineGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//开启抗锯齿

            Pen PenTemp = new Pen(Color.Black, 1);
            foreach (Control ControlItem in this.Chidren_Panel.Controls)
            {
                int TopTemp = ControlItem.Top + (ControlItem.Height / 2);
                Point PointTemp = new Point(this.DrawingLine_panel.Width, TopTemp);


                //LineGraphics.DrawLine(PenTemp, StartPoint, PointTemp);

                List<Point> PointArray = new List<Point>();
                PointArray.Add(StartPoint);
                PointArray.Add(new Point(StartPoint.X + DrawingLine_panel.Width / 2, StartPoint.Y));
                PointArray.Add(new Point(PointTemp.X - DrawingLine_panel.Width / 2, PointTemp.Y));

                PointArray.Add(PointTemp);
                LineGraphics.DrawCurve(PenTemp, PointArray.ToArray(), 0);
            }
        }

        /// <summary> 刷新单个节点的宽度和高度
        /// 
        /// </summary>
        private void ReSetSize()
        {
            Graphics g = this.CreateGraphics();
            g.PageUnit = GraphicsUnit.Display;
            SizeF ContentSize = g.MeasureString(Content_lable.Text, g_TextFont);
            Content_Panel.Width = Convert.ToInt32(ContentSize.Width + (ContentSize.Width * 0.15));

            int MaxChidrenWidth = 0;
            int HeightCount = 0;
            foreach (Control ControlItem in this.Chidren_Panel.Controls)
            {
                if (MaxChidrenWidth < ControlItem.Width) MaxChidrenWidth = ControlItem.Width;
                HeightCount += ControlItem.Height + 4;
            }

            MaxChidrenWidth = MaxChidrenWidth + DrawingLine_panel.Width + Content_Panel.Width;
            this.Width = MaxChidrenWidth;
            if (HeightCount < ContentSize.Height) HeightCount = Convert.ToInt32(ContentSize.Height);
            this.Height = HeightCount;

            Content_lable.Width = Content_Panel.Width + 10;
            Content_lable.Height = Convert.ToInt32(ContentSize.Height);
            Content_lable.Left = 0;
            Content_lable.Top = (Content_Panel.Height - Content_lable.Height) / 2;

        }

        /// <summary> 获取该节点下的子节点
        /// 
        /// </summary>
        /// <param name="IsAll">是否包含孙节点在内的所有子节点</param>
        /// <returns></returns>
        public List<MindMapNode> GetChidrenNode(bool IsAll = false)
        {
            List<MindMapNode> ResultList = new List<MindMapNode>();
            foreach (MindMapNode MindMapNodeItem in Chidren_Panel.Controls)
            {
                ResultList.Add(MindMapNodeItem);
                if (IsAll)
                {
                    ResultList.AddRange(MindMapNodeItem.GetChidrenNode(IsAll));
                }
            }
            return ResultList;
        }

        /// <summary> 递归设置子节点
        /// 
        /// </summary>
        /// <param name="FontSource"></param>
        public void SetTextFont(Font FontSource)
        {
            if (FontSource == null) return;
            g_TextFont = FontSource;
            Content_lable.Font = g_TextFont;
            GetChidrenNode(false).ForEach(T1 => T1.SetTextFont(FontSource));//递归将子节点也设置字体
            ReSetSize();
        }

        /// <summary> 递归向上设置父节点的尺寸
        /// 用于如果某节点修改了文本或字体，需要重新计算该节点的大小，其父节点的子节点容器也需要调节大小
        /// </summary>
        public void ResetNodeSize()
        {
            ReSetSize();
            if (this._ParentNode != null)
            {
                _ParentNode.ResetNodeSize();
            }
        }

        /// <summary> 添加一个节点
        /// 
        /// </summary>
        public void AddNode(TreeNode TreeNodeParame)
        {
            _TreeNode.Nodes.Add(TreeNodeParame);
            MindMapNode NewNode = new MindMapNode();
            NewNode.TextFont = g_TextFont;
            NewNode.TreeNode = TreeNodeParame;
            NewNode.Margin = new Padding(0, 2, 0, 2);
            Chidren_Panel.Controls.Add(NewNode);
            ResetNodeSize();

        }
        #endregion 方法

        #region 事件
        /// <summary> 重画时重新画出连接线
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMapNode_Paint(object sender, PaintEventArgs e)
        {
            DrawingConnectLine();

        }

        #endregion 事件
    }
}
