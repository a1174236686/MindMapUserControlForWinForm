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
using WlxMindMap.UseControl;
using System.Reflection;

namespace WlxMindMap
{
    /// <summary> 节点容器，
    /// 负责处理当前节点和子节点的关系，每一个节点都有一个单独的节点容器，但本身是不具备节点内容的（使用SetNoeContent方法来设置节点内容）
    /// 负责：
    /// 1、绘制当前节点与子节点之间的连接线
    /// 2、添加子节点或删除子节点
    /// 3、负责折叠或展开子节点
    /// 4、计算节点尺寸
    /// </summary>
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
        /// <summary>将当前尺寸记录为缩放比例为100%时的尺寸，缩放时将会基类该值进行调整
        /// 
        /// </summary>
        private void RecordScaling()
        {
            Scaling_LineSize = DrawingLine_panel.Size;
            Scaling_Margin = this.Margin;
            Scaling_CollapseButtonFont = collapseNodeButton1.ButtonFont;
        }
        private Size Scaling_LineSize = new Size();//缩放比例100%时画线的宽度
        private Padding Scaling_Margin = new Padding();//缩放比例100%时子节点之间的间隙
        private Font Scaling_CollapseButtonFont = new Font("", 1f);//缩放比例100%时折叠按钮尺寸        
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

                List<MindMapNodeContainer> ContainerList = GetChidrenNode();//获取子节点
                ContainerList.ForEach(Item => Item.CurrentScaling = _CurrentScaling);//将子节点的缩放比例也修改
                if (this._NodeContent != null) this._NodeContent.CurrentScaling = _CurrentScaling;

                this.DrawingLine_panel.Width = Scaling_LineSize.ByScaling(_CurrentScaling).Width;//更新连接线尺寸
                this.Margin = Scaling_Margin.ByScaling(_CurrentScaling); //子节点之间的间隙
                collapseNodeButton1.ButtonFont = Scaling_CollapseButtonFont.ByScaling(_CurrentScaling);

                ReSetSize();
            }
        }
        private float _CurrentScaling = 1;

        /// <summary> 设置节点内容的结构
        /// 
        /// </summary>
        public MindMapNodeStructBase DataStruct
        {
            set
            {
                if (_NodeContent == null) return;
                _NodeContent.DataStruct = value;
            }
            get
            {
                if (_NodeContent == null) return null;
                return _NodeContent.DataStruct;
            }
        }

        /// <summary> 获取或设置节点的数据源[数据源来自于NodeContent中]
        /// 
        /// </summary>
        public object DataItem
        {
            get
            {
                if (_NodeContent == null) return null;
                return _NodeContent.DataItem;
            }
            set
            {
                if (_NodeContent == null) throw new Exception("NodeContent：你需要先为节点容器设置节点内容");
                _NodeContent.DataItem = value;
            }

        }

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

        /// <summary> 获取该节点所在的思维导图容器
        /// 
        /// </summary>
        public MindMap_Panel MindMap_Panel
        {
            get
            {
                MindMapNodeContainer BaseNode = this;
                #region 向上找到根节点
                while (true)
                {
                    if (BaseNode.ParentNode != null)
                    {
                        BaseNode = BaseNode.ParentNode;
                    }
                    else
                    {
                        break;
                    }
                }
                #endregion 向上找到根节点


                #region 向上找到思维导图控件
                Control ControlTemp = BaseNode;
                while (true)
                {
                    if (ControlTemp.Parent != null)
                    {
                        if (ControlTemp is MindMap_Panel) return (MindMap_Panel)ControlTemp;

                        ControlTemp = ControlTemp.Parent;

                    }
                    else
                    {
                        return null;
                    }
                }
                #endregion 向上找到思维导图控件
            }
        }


        #endregion 属性               

        #region 方法
        /// <summary>SetNodeContent方法的锁。防止死递归,确保该方法只会被允许调用一次
        /// 
        /// </summary>
        private bool SetContentLock = false;
        /// <summary> 为节点设置内容布局样式
        /// 
        /// </summary>
        /// <param name="Struct">指示DataItem的数据结构</param>
        /// <param name="NodeContentParame">设置节点内容布局</param>
        public void SetNodeContent(MindMapNodeStructBase Struct, MindMapNodeContentBase NodeContentParame)
        {
            #region 为什么要有锁？
            /*
                * 锁的概念是为了调用本方法设置节点内容时，要将以前的节点内容的ParentMindMapNode属性设置为空
                * 类似于Control类的Parent属性,使得节点容器和节点内容只需要修改其中一个实例另一个实例就会自动变
                * 但是以前的节点内容的ParentMindMapNode属性也有相同的特性会返回来调用本方法，所以会造成死递归                 
                */
            #endregion 为什么要有锁？
            if (SetContentLock) return;//锁打开了就直接返回
            SetContentLock = true;//打开锁

            if (this._NodeContent == NodeContentParame) return;
            if (this._NodeContent != null) this._NodeContent.NodeContainer = null;//把以前的置为空
            this._NodeContent = NodeContentParame;
            this.Content_Panel.Controls.Clear();
            if (this._NodeContent != null)
            {
                this.NodeContent.DataStruct = Struct;
                this.Content_Panel.Controls.Add(this.NodeContent);
                this._NodeContent.NodeContainer = this;
                this._NodeContent.CurrentScaling = this.CurrentScaling;
            }
            ReSetSize();
            SetContentLock = false;//关闭锁
            if (AddNodeContent != null) AddNodeContent(this, null);

        }

        /// <summary> 为节点设置内容布局样式
        /// 
        /// </summary>
        /// <typeparam name="NodeContentClass">指定该节点内容采用哪种布局</typeparam>
        /// <param name="Struct">指示DataItem的数据结构</param>
        public void SetNodeContent<NodeContentClass>(MindMapNodeStructBase Struct) where NodeContentClass : MindMapNodeContentBase, new()
        {
            SetNodeContent(Struct, new NodeContentClass());
        }


        /// <summary> 在面板上画出当前节点的连接线
        /// 
        /// </summary>
        public void DrawingConnectLine()
        {
            int CurrentNodeHeightCenter = Content_Panel.Height / 2;
            Point StartPoint = new Point(0, CurrentNodeHeightCenter);
            Graphics LineGraphics = DrawingLine_panel.CreateGraphics();
            LineGraphics.Clear(DrawingLine_panel.BackColor);//清除之前的
            LineGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//开启抗锯齿

            Pen PenTemp = new Pen(Color.Black, 2);
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
            if (NodeSizeChanged != null) NodeSizeChanged();//隐藏思维导图用以跳过绘制过程
            Size ContentSize = new Size(0, 0);
            if (_NodeContent != null)
            {
                _NodeContent.RefreshContentSize();
                ContentSize = _NodeContent.Size;
            }
            Content_Panel.Width = ContentSize.Width;



            int MaxChidrenWidth = 0;//子节点最宽的宽度
            int HeightCount = 0;//子节点高度的总和
            List<MindMapNodeContainer> ChidrenNodeList = this.GetChidrenNode();//获取所有子节点
            if (ChidrenNodeList.Count > 0)//是否有子节点
            {
                //有子节点
                collapseNodeButton1.Visible = true;
                if (!collapseNodeButton1.IsExpand)//是否已经展开
                {
                    //已展开                   
                    DrawingLine_panel.Visible = true;
                    Chidren_Panel.Visible = true;
                    MaxChidrenWidth = ChidrenNodeList.Select(T1 => T1.Width).Max();//获取子节点最宽的宽度
                    HeightCount = ChidrenNodeList.Select(T1 => T1.Height + T1.Margin.Top + T1.Margin.Bottom).Sum();

                    //设置本节点容器的整体宽度（节点内容宽度 + 折叠按钮宽度 + 连接线宽度 + 最宽子节点的宽度）            
                    MaxChidrenWidth = MaxChidrenWidth + collapseNodeButton1.Width + DrawingLine_panel.Width + Content_Panel.Width;
                }
                else
                {
                    //未展开             
                    DrawingLine_panel.Visible = false;
                    Chidren_Panel.Visible = false;
                    MaxChidrenWidth = MaxChidrenWidth + collapseNodeButton1.Width + Content_Panel.Width;
                }
            }
            else
            {
                //没有子节点
                collapseNodeButton1.Visible = false;
                DrawingLine_panel.Visible = false;
                Chidren_Panel.Visible = false;
                MaxChidrenWidth = Content_Panel.Width;//没有子节点本节点的宽度就等于当前内容的宽度
            }
            this.Width = MaxChidrenWidth;

            //设置本节点容器的整体高度（所有子节点高度的总和，或节点内容的高度，两者较大的那一个）
            if (HeightCount < ContentSize.Height) HeightCount = ContentSize.Height;
            this.Height = HeightCount;
            DrawingConnectLine();
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

            if (NodeSizeChanged != null) NodeSizeChanged();//隐藏思维导图用以跳过绘制过程
            MindMapNodeContainer NewNode = MindMapNodeParame;
            Chidren_Panel.Controls.Add(NewNode);
            MindMapNodeParame.ParentNode = this;
            MindMapNodeParame.CurrentScaling = this.CurrentScaling;
            ResetNodeSize();

            //添加节点后禁用Tab键。否则Tab键无法触发到MindMap_Panel控件中
            this.GetAllControl().ForEach(T1 => T1.TabStop = false);
            this.TabStop = false;

            if (AddChidrenNode != null) AddChidrenNode(this, NewNode);
        }


        /// <summary> 移除一个节点
        /// 
        /// </summary>
        public void Remove(MindMapNodeContainer MindMapNodeParame)
        {
            if (MindMapNodeParame == null) return;
            if (NodeSizeChanged != null) NodeSizeChanged(); //隐藏思维导图用以跳过绘制过程
            MindMapNodeContainer MindMapNodeTemp = null;
            foreach (Control ControlItem in Chidren_Panel.Controls)
            {
                MindMapNodeTemp = (MindMapNodeContainer)ControlItem;
                if (MindMapNodeParame == MindMapNodeTemp)
                {
                    MindMapNodeTemp.Parent = null;
                    MindMapNodeTemp.ParentNode = null;
                    if (RemoveChidrenNode != null) RemoveChidrenNode(this, MindMapNodeTemp);
                    break;
                }
            }
            this.ResetNodeSize();
        }

        /// <summary> 展开/折叠当前节点
        /// 
        /// </summary>
        /// <param name="IsExpandParame">[true:显示为展开+；false：显示为折叠-]</param>
        public void ExpandOrCollapse(bool IsExpandParame)
        {
            collapseNodeButton1.IsExpand = IsExpandParame;
            ResetNodeSize();
        }

        /// <summary> 展开/折叠所有子节点
        /// 
        /// </summary>
        /// <param name="IsExpandParame">[true:显示为展开+；false：显示为折叠-]</param>
        public void ExpandOrCollapseAll(bool IsExpandParame)
        {
            collapseNodeButton1.IsExpand = IsExpandParame;
            this.GetChidrenNode().ForEach(T1 => T1.PrivateExpandOrCollapseAll(IsExpandParame));
            ResetNodeSize();
        }

        /// <summary> 将该节点滚动致可视范围
        /// 
        /// </summary>
        public void ScrollToView()
        {

            if (NodeContent == null) return;
            MindMap_Panel MindMap_PanelTemp = MindMap_Panel;
            Rectangle PanelRec = new Rectangle(MindMap_PanelTemp.PointToScreen(new Point(0, 0)), MindMap_PanelTemp.Size);
            Rectangle NodeRec = new Rectangle(NodeContent.PointToScreen(new Point(0, 0)), NodeContent.Size);

            if (PanelRec.Contains(NodeRec)) return;
            Point ResultPoint = new Point();
            #region 获取X轴偏移量
            if (PanelRec.Left > NodeRec.Left)
            {
                ResultPoint.X = NodeRec.Left - PanelRec.Left;
            }
            if (PanelRec.Left + PanelRec.Width < NodeRec.Left + NodeRec.Width)
            {
                ResultPoint.X = (NodeRec.Left + NodeRec.Width) - (PanelRec.Left + PanelRec.Width)+20;
            }
            #endregion 获取X轴偏移量
            #region 获取Y轴偏移量
            if (PanelRec.Top > NodeRec.Top)
            {
                ResultPoint.Y = NodeRec.Top - PanelRec.Top;
            }
            if (PanelRec.Top + PanelRec.Height < NodeRec.Top + NodeRec.Height)
            {
                ResultPoint.Y = (NodeRec.Top + NodeRec.Height) - (PanelRec.Top + PanelRec.Height)+20;
            }
            #endregion 获取Y轴偏移量

            MindMap_PanelTemp.ScrollMindMap(ResultPoint);
        }

        /// <summary> 将该节点居中
        /// 
        /// </summary>
        public void ScrollToCenter()
        {
            if (NodeContent == null) return;
            MindMap_Panel MindMap_PanelTemp = MindMap_Panel;
            Rectangle PanelRec = new Rectangle(MindMap_PanelTemp.PointToScreen(new Point(0, 0)), MindMap_PanelTemp.Size);
            Rectangle NodeRec = new Rectangle(NodeContent.PointToScreen(new Point(0, 0)), NodeContent.Size);

            Point ResultPoint = new Point();
            ResultPoint.X = PanelRec.Width - NodeContent.Width;
            ResultPoint.Y = PanelRec.Height - NodeContent.Height;
            ResultPoint.X = ResultPoint.X / 2;
            ResultPoint.Y = ResultPoint.Y / 2;
            ResultPoint.X = ResultPoint.X + PanelRec.X;
            ResultPoint.Y = ResultPoint.Y + PanelRec.Y;
            ResultPoint.X = NodeRec.Left - ResultPoint.X;
            ResultPoint.Y = NodeRec.Top - ResultPoint.Y;

            MindMap_PanelTemp.ScrollMindMap(ResultPoint);


        }

        #endregion 方法

        #region 私有方法
        /// <summary>递归向下展开/折叠所有子节点只供本类自己调用
        /// 
        /// </summary>
        /// <param name="IsExpandParame">[true:显示为展开+；false：显示为折叠-]</param>
        private void PrivateExpandOrCollapseAll(bool IsExpandParame)
        {
            collapseNodeButton1.IsExpand = IsExpandParame;
            this.GetChidrenNode().ForEach(T1 => T1.PrivateExpandOrCollapseAll(IsExpandParame));
            ReSetSize();
        }

        /// <summary> 折叠按钮被点击
        /// 
        /// </summary>
        private void collapseNodeButton1_CollapseButtonDown()
        {
            ExpandOrCollapse(!collapseNodeButton1.IsExpand);
            this.NodeContent.Selected = true;
        }
        #endregion 私有放

        #region 事件
        /// <summary> 重画时重新画出连接线
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMapNode_Paint(object sender, PaintEventArgs e)
        {
            //return;
            DrawingConnectLine();
            return;

        }

        private void EmptyRange_MouseClick(object sender, MouseEventArgs e)
        {
            if (EmptyRangeMouseClick != null) EmptyRangeMouseClick(this, e);
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
        public event MouseEventHandler EmptyRangeMouseClick;

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

        /// <summary> 添加节点
        /// 
        /// </summary>
        [Browsable(true), Description("添加子节点")]
        public event MindMapEventHandler AddChidrenNode;

        /// <summary> 删除节点
        /// 
        /// </summary>
        [Browsable(true), Description("删除节点")]
        public event MindMapEventHandler RemoveChidrenNode;

        /// <summary> 为节点添加节点内容时发生
        /// 
        /// </summary>
        [Browsable(true), Description("为节点添加节点内容时发生")]
        public event MindMapEventHandler AddNodeContent;

        /// <summary> 当节点自身尺寸即将发生改变时
        /// 隐藏思维导图用以跳过绘制过程
        /// </summary>
        [Browsable(true), Description("当节点自身尺寸即将发生改变时")]
        public event Action NodeSizeChanged;

        /// <summary> 删除或添加节点的委托类型
        /// 
        /// </summary>
        /// <param name="Sender">发生事件的节点</param>
        /// <param name="Chidren">被添加/删除的节点</param>
        public delegate void MindMapEventHandler(MindMapNodeContainer Sender, MindMapNodeContainer MindMapNodeContainer);

        #endregion 事件

    }
}

