using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WlxMindMap.MindMapNodeContent;
using WlxMindMap.MindMapNode;
using System.Reflection;

namespace WlxMindMap.MindMapNode
{
    public partial class MindMapNodeContainer : UserControl
    {
        public MindMapNodeContainer()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            RecordScaling();
            this.Margin = new Padding(0, 2, 0, 2);

        }

        #region 缩放相关
        /// <summary>记录当前尺寸为100%时的尺寸，缩放时将会基类该值进行调整
        /// 
        /// </summary>
        private void RecordScaling()
        {
            Scaling_LineSize = DrawingLine_panel.Size;
            Scaling_Margin = this.Margin;

        }
        private Size Scaling_LineSize=new Size ();
        private Padding Scaling_Margin = new Padding();


        #endregion 缩放相关

        #region 属性

        /// <summary> 获取节点的内容布局的实例
        /// 
        /// </summary>
        public MindMapNodeContentBase NodeContent { get { return _NodeContent; } }
        private MindMapNodeContentBase _NodeContent;


        /// <summary>获取或设置当前的缩放比例（百分比）
        /// 
        /// </summary>
        public float CurrentScaling
        {
            get { return _CurrentScaling; }
            set
            {
                _CurrentScaling = value;

                List<MindMapNodeContainer>ContainerList= GetChidrenNode();//获取子节点
                ContainerList.ForEach(Item => Item.CurrentScaling = _CurrentScaling);//将子节点的缩放比例也修改

                this._NodeContent.CurrentScaling = _CurrentScaling;
                DrawingLine_panel.Width = Scaling_LineSize.ByScaling(_CurrentScaling).Width;//更新连接线尺寸
                ReSetSize();
            }
        }
        private float _CurrentScaling = 1;

        /// <summary> 设置节点内容的结构
        /// 
        /// </summary>
        public MindMapNodeStructBase DataStruct { set; get; }
        private MindMapNodeStructBase _DataStruct = new MindMapNodeStructBase();

        /// <summary> 设置或获取父节点
        /// 
        /// </summary>
        public MindMapNodeContainer ParentNode
        {
            set
            {
                if (_ParentNode == value) return;
                if (_ParentNode != null) _ParentNode.Remove(this);//将本容器从原有父容器中移出
                _ParentNode = value;
                if (_ParentNode != null) _ParentNode.AddNode(this);//将本容器添加到新的父容器中

            }
            get { return _ParentNode; }
        }
        private MindMapNodeContainer _ParentNode = null;
    
        
        
        #endregion 属性               

        #region 方法
        /// <summary> 设置内容布局样式
        /// 
        /// </summary>
        /// <typeparam name="NodeContentClass"></typeparam>
        /// <param name="Struct"></param>
        public void SetNodeContent<NodeContentClass>(MindMapNodeStructBase Struct, MindMapNodeContentBase NodeContentParame = null) where NodeContentClass : MindMapNodeContentBase, new()
        {
            if (NodeContentParame == null) NodeContentParame = new NodeContentClass();
            if (this._NodeContent == NodeContentParame) return;
            else this._NodeContent = NodeContentParame;
            this.NodeContent.DataStruct = Struct;
            this.Content_Panel.Controls.Add(this.NodeContent);            
            this._NodeContent.ParentMindMapNode = this;
            this._NodeContent.CurrentScaling = this.CurrentScaling;
            ReSetSize();
        }

        /// <summary> 在面板上画出当前节点的连接线
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

                List<Point> PointArray = new List<Point>();
                PointArray.Add(StartPoint);
                PointArray.Add(new Point(StartPoint.X + DrawingLine_panel.Width / 2, StartPoint.Y));
                PointArray.Add(new Point(PointTemp.X - DrawingLine_panel.Width / 2, PointTemp.Y));

                PointArray.Add(PointTemp);
                LineGraphics.DrawCurve(PenTemp, PointArray.ToArray(), 0);
            }
        }

        /// <summary> 获取该节点下的子节点
        /// 
        /// </summary>
        /// <param name="IsAll">是否包含孙节点在内的所有子节点</param>
        /// <returns></returns>
        public List<MindMapNodeContainer> GetChidrenNode(bool IsAll = false)
        {
            List<MindMapNodeContainer> ResultList = new List<MindMapNodeContainer>();
            foreach (MindMapNodeContainer MindMapNodeItem in Chidren_Panel.Controls)
            {
                ResultList.Add(MindMapNodeItem);
                if (IsAll)
                {
                    ResultList.AddRange(MindMapNodeItem.GetChidrenNode(IsAll));
                }
            }
            return ResultList;
        }
                
        /// <summary> 刷新节点的宽度和高度
        /// 
        /// </summary>
        private void ReSetSize()
        {
            Size ContentSize = new Size(0, 0);
            if (_NodeContent != null)
            {
                _NodeContent.RefreshContentSize();
                ContentSize = _NodeContent.Size;
            }


            Content_Panel.Width = ContentSize.Width;
            int MaxChidrenWidth = 0;//子节点最宽的宽度
            int HeightCount = 0;//子节点高度的总和
            foreach (Control ControlItem in this.Chidren_Panel.Controls)//遍历所有子节点容器
            {
                if (MaxChidrenWidth < ControlItem.Width) MaxChidrenWidth = ControlItem.Width;//获取子节点最宽的宽度
                HeightCount += ControlItem.Height + 4;//获取子节点高度的总和
            }

            //设置本节点容器的整体宽度（节点内容宽度+连接线宽度+最宽子节点的宽度）            
            MaxChidrenWidth = MaxChidrenWidth + DrawingLine_panel.Width + Content_Panel.Width;
            this.Width = MaxChidrenWidth;

            //设置本节点容器的整体高度（所有子节点高度的总和，或节点内容的高度，两者较大的那一个）
            if (HeightCount < ContentSize.Height) HeightCount = ContentSize.Height;
            this.Height = HeightCount;
            if (_NodeContent != null)
            {
                _NodeContent.Left = 0;
                _NodeContent.Top = (Content_Panel.Height - ContentSize.Height) / 2;
            }
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
        public void AddNode(MindMapNodeContainer MindMapNodeParame)
        {
            if (MindMapNodeParame == null) return;
            List<MindMapNodeContainer> MindMapNodeList = GetChidrenNode();
            int FindCount = MindMapNodeList.Where(T1 => T1 == MindMapNodeParame).Count();
            if (FindCount != 0) return;//如果要添加的节点已经存在就直接返回

            MindMapNodeContainer NewNode = MindMapNodeParame;

            Chidren_Panel.Controls.Add(NewNode);
            MindMapNodeParame.ParentNode = this;
            NewNode.ResetNodeSize();
        }


        /// <summary> 移除一个节点
        /// 
        /// </summary>
        public void Remove(MindMapNodeContainer MindMapNodeParame)
        {
            if (MindMapNodeParame == null) return;
            MindMapNodeContainer MindMapNodeTemp = null;
            foreach (Control ControlItem in Chidren_Panel.Controls)
            {
                MindMapNodeTemp = (MindMapNodeContainer)ControlItem;
                if (MindMapNodeParame == MindMapNodeTemp)
                {
                    MindMapNodeTemp.Parent = null;
                    MindMapNodeTemp.ParentNode = null;
                    break;
                }
            }
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
            return;

        }

    
        private void EmptyRange_Click(object sender, EventArgs e)
        {
            if (EmptyRangeClick != null) EmptyRangeClick(this, e);
        }

        private void EmptyRange_MouseDown(object sender, MouseEventArgs e)
        {

            if (EmptyRangeMouseDown != null) EmptyRangeMouseDown(this, e);
        }

        private void EmptyRange_MouseUp(object sender, MouseEventArgs e)
        {
            if (EmptyRangeMouseUp != null) EmptyRangeMouseUp(this, e);
        }

        private void EmptyRange_MouseMove(object sender, MouseEventArgs e)
        {
            if (EmptyRangeMouseMove != null) EmptyRangeMouseMove(this, e);
        }

        
        /// <summary> 点击空白处
        /// 
        /// </summary>
        [Browsable(true), Description("点击空白处")]
        public event EventHandler EmptyRangeClick;
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

        #endregion 事件
    }

}

