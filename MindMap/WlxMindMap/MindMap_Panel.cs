using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using WlxMindMap.MindMapNode;
using WlxMindMap.MindMapNodeContent;

namespace WlxMindMap
{
    public partial class MindMap_Panel : UserControl
    {
        

        public MindMap_Panel()
        {
            InitializeComponent();         
            this.MouseWheel += new MouseEventHandler(OnMouseWhell);
        }      
        private TreeNode g_BaseNode = null;

        private Font _TextFont = new Font(new FontFamily("微软雅黑"), 12);
        public Font TextFont
        {
            get
            {
                return _TextFont;
            }
            set
            {
                if (value == null) return;
                _TextFont = value;
                mindMapNode.SetTextFont(_TextFont);
            }
        }

        #region 公开方法


        /// <summary> 为控件设置数据源
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="DataSource"></param>
        /// <param name="NodeStruct"></param>
        public void SetDataSource<T>(List<T> DataSource, TreeViewNodeStruct NodeStruct)
        {
            //制作TreeNode
            g_BaseNode = null;
            SetTreeNode<T>(DataSource, NodeStruct);
            mindMapNode.TextFont = _TextFont;
            mindMapNode.TreeNode = g_BaseNode;


        }

        /// <summary> 用递归的方式将List数据按树状图添加到指定节点下
        /// </summary>
        /// <typeparam name="T">要添加的数据的模型类</typeparam>
        /// <param name="DataSource">要添加的List数据</param>
        /// <param name="NodeStruct">List的结构</param>
        /// <param name="ParenNode">父节点[为空则添加到根节点]</param>
        private void SetTreeNode<T>(List<T> DataSource, TreeViewNodeStruct NodeStruct, TreeNode ParenNode = null)
        {
            PropertyInfo KeyProperty = typeof(T).GetProperty(NodeStruct.KeyName);
            PropertyInfo ValueProperty = typeof(T).GetProperty(NodeStruct.ValueName);
            PropertyInfo ParentProperty = typeof(T).GetProperty(NodeStruct.ParentName);
            List<T> CurrentAddList = null;
            if (ParenNode == null) CurrentAddList = DataSource.Where(T1 => string.IsNullOrEmpty(ParentProperty.GetValue(T1).ToString())).ToList();//没有父节点就取父节点为空的记录
            else CurrentAddList = DataSource.Where(T1 => ParentProperty.GetValue(T1).ToString() == ParenNode.Name).ToList();//有父节点就取ParentID为父节点的记录
            foreach (T item in CurrentAddList)//遍历取出的记录
            {
                string StrKey = KeyProperty.GetValue(item).ToString();
                string StrValue = ValueProperty.GetValue(item).ToString();
                string StrParentValue = ParentProperty.GetValue(item).ToString();
                TreeNode NodeTemp = new TreeNode() { Name = StrKey, Text = StrValue, ImageKey = StrKey, SelectedImageKey = StrKey };
                SetTreeNode<T>(DataSource, NodeStruct, NodeTemp);
                (ParenNode == null ? GetBaseNodes() : ParenNode.Nodes).Add(NodeTemp);//将取出的记录添加到父节点下，没有父节点就添加到控件的Nodes下
            }
        }

        /// <summary> 获取根节点
        /// 
        /// </summary>
        /// <returns></returns>
        private TreeNodeCollection GetBaseNodes()
        {
            if (g_BaseNode == null)
            {
                g_BaseNode = new TreeNode();
                g_BaseNode.Text = "根节点";
                g_BaseNode.Name = "BaseNode";
            }
            return g_BaseNode.Nodes;
        }

        /// <summary> 获取所有被选中的节点
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MindMapNode.MindMapNode> GetSelectedNode()
        {
            List<MindMapNode.MindMapNode> ResultList = new List<MindMapNode.MindMapNode>();
            ResultList = mindMapNode.GetChidrenNode(true);
            ResultList.Add(mindMapNode);
            ResultList = ResultList.Where(T1 => T1.Selected == true).ToList();
            return ResultList;
        }

        #endregion 公开方法


        #region 公开事件委托
        /// <summary>节点被按下时
        /// 
        /// </summary>
        private void mindMapNode_MindMapNodeMouseDown(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseDown(this, e);
            if (MindMapNodeMouseDown != null) MindMapNodeMouseDown(this, e);
        }
        /// <summary>节点在鼠标弹起时
        /// 
        /// </summary>
        private void mindMapNode_MindMapNodeMouseUp(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseUp(this, e);
            if (MindMapNodeMouseUp != null) MindMapNodeMouseUp(this, e);
        }
        /// <summary>鼠标在节点移动时
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_MindMapNodeMouseMove(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseMove(this, e);
            if (MindMapNodeMouseMove != null) MindMapNodeMouseMove(sender, e);
        }
        /// <summary>鼠标进入节点范围时
        /// 
        /// </summary>
        private void mindMapNode_MindMapNodeMouseEnter(object sender, EventArgs e)
        {
            if (MindMapNodeMouseEnter != null) MindMapNodeMouseEnter(this, e);
        }
        /// <summary>鼠标移出节点范围事件
        /// 
        /// </summary>
        private void mindMapNode_MindMapNodeMouseLeave(object sender, EventArgs e)
        {
            if (MindMapNodeMouseLeave != null) MindMapNodeMouseLeave(this, e);
        }
        /// <summary> 鼠标单击某节点
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_MindMapNodeMouseClick(object sender, MouseEventArgs e)
        {
            MindMapNode.MindMapNode SenderObject = ((MindMapNode.MindMapNode)sender);

            if (Control.ModifierKeys != Keys.Control)//不按住ctrl就单选
            {
                List<MindMapNode.MindMapNode> MindMapNodeList = mindMapNode.GetChidrenNode(true);
                MindMapNodeList.Add(mindMapNode);
                MindMapNodeList.ForEach(T1 => T1.Selected = false);
                SenderObject.Selected = true;
            }
            else//按住ctrl可单选
            {
                SenderObject.Selected = !SenderObject.Selected;
            }



            if (MindMapNodeMouseClick != null) MindMapNodeMouseClick(this, e);
        }





        private void mindMapNode_EmptyRangeMouseDown(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseDown(sender, e);
            if (EmptyRangeMouseDown != null) EmptyRangeMouseDown(sender, e);
        }

        private void mindMapNode_EmptyRangeMouseMove(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseMove(sender, e);
            if (EmptyRangeMouseMove != null) EmptyRangeMouseMove(sender, e);
        }

        private void mindMapNode_EmptyRangeMouseUp(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseUp(sender, e);
            if (EmptyRangeMouseUp != null) EmptyRangeMouseUp(sender, e);
        }


        /// <summary>鼠标进入节点范围事件
        /// 
        /// </summary>
        [Description("鼠标进入节点范围事件")]
        public event EventHandler MindMapNodeMouseEnter;

        /// <summary>鼠标离开节点范围事件
        /// 
        /// </summary>
        [Description("鼠标离开节点范围事件")]
        public event EventHandler MindMapNodeMouseLeave;

        /// <summary> 节点被鼠标按下事件
        /// 
        /// </summary>
        [Description("节点被鼠标按下事件")]
        public event MouseEventHandler MindMapNodeMouseDown;

        /// <summary> 节点被鼠标弹起事件
        /// 
        /// </summary>
        [Description("节点被鼠标弹起事件")]
        public event MouseEventHandler MindMapNodeMouseUp;

        /// <summary> 节点被单击时
        /// 
        /// </summary>
        [Browsable(true), Description("节点被单击时")]
        public event MouseEventHandler MindMapNodeMouseClick;

        [Browsable(true), Description("节点被单击时")]
        public event MouseEventHandler MindMapNodeMouseDoubleClick;

        /// <summary> 空白处鼠标按下
        /// 
        /// </summary>
        [Browsable(true), Description("空白处鼠标按下")]
        public event MouseEventHandler EmptyRangeMouseDown;

        /// <summary> 空白处鼠标弹起
        /// 
        /// </summary>
        [Browsable(true), Description("空白处鼠标弹起")]
        public event MouseEventHandler EmptyRangeMouseUp;

        /// <summary> 空白处鼠标移动
        /// 
        /// </summary>
        [Browsable(true), Description("空白处鼠标移动")]
        public event MouseEventHandler EmptyRangeMouseMove;

        /// <summary> 点击空白处
        /// 
        /// </summary>
        [Browsable(true), Description("点击空白处")]
        public event EventHandler EmptyRangeClick;

        /// <summary> 鼠标在节点上移动时
        /// 
        /// </summary>
        [Description("鼠标在节点上移动时")]
        public event MouseEventHandler MindMapNodeMouseMove;

        #endregion 公开事件委托

        /// <summary> 空白处被单击取消所有选中        
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_EmptyRangeClick(object sender, EventArgs e)
        {


            if (NodeEdit_textBox.Visible)//如果正在编辑某节点则完成编辑
            {
                NodeEdit_textBox.Visible = false;
                GetSelectedNode().ForEach(Item => Item.MindMapNodeText = NodeEdit_textBox.Text);
                return;
            }

            List<MindMapNode.MindMapNode> MindMapNodeList = mindMapNode.GetChidrenNode(true);
            MindMapNodeList.Add(mindMapNode);
            MindMapNodeList.ForEach(T1 => T1.Selected = false);

            if (EmptyRangeClick != null) EmptyRangeClick(sender, e);



        }

        /// <summary> 节点编辑完成
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeEdit_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) mindMapNode_EmptyRangeClick(null, null);//如果按下回车则编辑完成
            if (e.KeyData == Keys.Escape) NodeEdit_textBox.Visible = false;//按下esc则取消编辑



        }

        /// <summary> 双击某节点后编辑某节点
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender == null) return;
            MindMapNode.MindMapNode SenderObject = ((MindMapNode.MindMapNode)sender);

            NodeEdit_textBox.Visible = true;
            NodeEdit_textBox.BringToFront();
            NodeEdit_textBox.Size = SenderObject.NodeContentSize;
            NodeEdit_textBox.Text = SenderObject.MindMapNodeText;
            NodeEdit_textBox.Font = SenderObject.TextFont;
            NodeEdit_textBox.Location = this.PointToClient(SenderObject.PointToScreen(SenderObject.NodeContentLocation));
            NodeEdit_textBox.Focus();
            if (MindMapNodeMouseDoubleClick != null) MindMapNodeMouseDoubleClick(this, e);

        }

        private void mindMapNode_Resize(object sender, EventArgs e)
        {

            ResetMindMapPanelSize();
            #region 注释，以前居中的方法


            //Scroll_panel.Height = mindMapNode.Height * 2;//容器高度
            //Scroll_panel.Width = mindMapNode.Width * 2;//容器宽度
            //if (Scroll_panel.Height < this.Height) Scroll_panel.Height = this.Height;
            //if (Scroll_panel.Width < this.Width) Scroll_panel.Width = this.Width;


            //#region 思维导图相对于容器居中
            //int IntTemp = Scroll_panel.Height - mindMapNode.Height;
            //IntTemp = IntTemp / 2;
            //mindMapNode.Top = IntTemp;
            //IntTemp = Scroll_panel.Width - mindMapNode.Width;
            //IntTemp = IntTemp / 2;
            //mindMapNode.Left = IntTemp;
            //#endregion 思维导图相对于容器居中
            //#region 将容器滚动至居中位置

            //int IntX = this.Scroll_panel.Width - this.Width;
            //int IntY = this.Scroll_panel.Height - this.Height;
            //Point PointTemp = new Point(IntX / 2, IntY / 2);
            //this.AutoScrollPosition = PointTemp;
            //#endregion 将容器滚动至居中位置 
            #endregion 注释，以前居中的方法
        }

        #region 鼠标中键拖动滚动条

        private bool IsMouseMove = false;
        private Point MoveValue = new Point();
        /// <summary> 按下中键时可拖动滚动条
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMap_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right)
            {
                MoveValue = Control.MousePosition;
                IsMouseMove = true;
            }
        }

        /// <summary> 弹起中键结束拖动滚动条
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMap_Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right)
                IsMouseMove = false;
        }

        /// <summary>按住鼠标中间可拖动滚动条
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMap_Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMouseMove)
            {

                MoveValue.X = MoveValue.X - Control.MousePosition.X;
                MoveValue.Y = MoveValue.Y - Control.MousePosition.Y;

                Point ResultPoint = new Point(this.HorizontalScroll.Value + MoveValue.X, this.VerticalScroll.Value + MoveValue.Y);

                this.AutoScrollPosition = ResultPoint;

                MoveValue = Control.MousePosition;


            }
        }



        #endregion 鼠标中键拖动滚动条

        private void MindMap_Panel_Resize(object sender, EventArgs e)
        {

            new Thread(() =>
            {
                Thread.Sleep(200);
                this.Invoke(new Action(() =>
                {
                    ResetMindMapPanelSize();
                }
                    ));
            }).Start();
        }

        /// <summary> 重新设置导图和容器在控件中的尺寸
        /// 
        /// </summary>
        private void ResetMindMapPanelSize()
        {
            Scroll_panel.Location = new Point(-this.HorizontalScroll.Value, -this.VerticalScroll.Value);

            int MaxHeight = this.Height * 2;//容器最大高度，父容器的2倍
            int MaxWidth = this.Width * 2;//容器最大宽度，父容器的2倍
            int MinHeight = mindMapNode.Height * 2;//容器最小高度，自身高度的两倍
            int MinWidth = mindMapNode.Width * 2;//容器最小宽度，自身宽度的两倍
            Scroll_panel.Height = MaxHeight > MinHeight ? MaxHeight : MinHeight;//优先最大高度
            Scroll_panel.Width = MaxWidth > MinWidth ? MaxWidth : MinWidth;//优先最大宽度

            #region 将容器滚动至居中位置

            int IntX = this.Scroll_panel.Width - this.Width;
            int IntY = this.Scroll_panel.Height - this.Height;
            Point PointTemp = new Point(IntX / 2, IntY / 2);
            this.AutoScrollPosition = PointTemp;
            #endregion 将容器滚动至居中位置

            #region 思维导图相对于容器居中
            int IntTemp = Scroll_panel.Height - mindMapNode.Height;
            IntTemp = IntTemp / 2;
            mindMapNode.Top = IntTemp;
            IntTemp = Scroll_panel.Width - mindMapNode.Width;
            IntTemp = IntTemp / 2;
            mindMapNode.Left = IntTemp;
            #endregion 思维导图相对于容器居中

            this.HorizontalScroll.Minimum = Scroll_panel.Width;
            this.VerticalScroll.Minimum = Scroll_panel.Height;
        }

        private int FontSize = 13;
        /// <summary> 滚轮放大缩小
        /// 
        /// </summary>
        /// <param name="Send"></param>
        /// <param name="e"></param>
        private void OnMouseWhell(object Send, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                int ChangeValue = 1;//每次放大或缩小的数值
                if (e.Delta < 0) FontSize = FontSize - ChangeValue <= ChangeValue ? ChangeValue : FontSize - ChangeValue;
                else FontSize = FontSize + ChangeValue;

                Font TextFontTemp = new Font(new FontFamily("微软雅黑"), FontSize);
                this.Visible = false;
                this.TextFont = TextFontTemp;
                this.Visible = true;
            }
        }


        #region 配套使用的内部类
        /// <summary> 用于指明SetDataSource的泛型类的结构
        /// [指明传入的哪个属性是ID，哪个属性是父ID，哪个属性是展示在前台的文本]
        /// </summary>
        public class TreeViewNodeStruct
        {
            /// <summary> 用于添加到TreeNode.Name中的属性名称
            /// 一般用于存ID值
            /// </summary>
            public string KeyName { get; set; }
            /// <summary> 用于展示到前台的属性名称
            /// [添加到TreeNode.Text中的值]
            /// </summary>
            public string ValueName { get; set; }
            /// <summary> 父ID的属性名称
            /// </summary>
            public string ParentName { get; set; }

        }
        #endregion 配套使用的内部类
    }
}
