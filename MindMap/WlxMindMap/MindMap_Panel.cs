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
using WlxMindMap;
using WlxMindMap.MindMapNodeContent;
using WlxMindMap.MindMapNode;

namespace WlxMindMap
{
    public partial class MindMap_Panel : UserControl, IMessageFilter
    {
        private MindMapNodeContainer mindMapNode = new MindMapNodeContainer();

        public MindMap_Panel()
        {
            InitializeComponent();
            #region 根节点容器

            // 
            // mindMapNode
            // 
            this.mindMapNode.BackColor = System.Drawing.Color.White;
            this.mindMapNode.Location = new System.Drawing.Point(181, 166);
            this.mindMapNode.Name = "mindMapNode";
            this.mindMapNode.ParentNode = null;
            this.mindMapNode.Size = new System.Drawing.Size(86, 23);
            this.mindMapNode.TabIndex = 0;
            this.mindMapNode.EmptyRangeClick += new System.EventHandler(this.mindMapNode_EmptyRangeClick);
            this.mindMapNode.EmptyRangeMouseDown += new System.Windows.Forms.MouseEventHandler(this.mindMapNode_EmptyRangeMouseDown);
            this.mindMapNode.EmptyRangeMouseUp += new System.Windows.Forms.MouseEventHandler(this.mindMapNode_EmptyRangeMouseUp);
            this.mindMapNode.EmptyRangeMouseMove += new System.Windows.Forms.MouseEventHandler(this.mindMapNode_EmptyRangeMouseMove);
            //this.mindMapNode.AddChidrenNode += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeAddChidrenNode);
            this.mindMapNode.RemoveChidrenNode += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeRemoveChidrenNode);

            this.mindMapNode.Resize += new System.EventHandler(this.mindMapNode_Resize);
            this.Scroll_panel.Controls.Add(this.mindMapNode);
            #endregion 根节点容器            
            Application.AddMessageFilter(this);//当按住Control后滚轮无法控制滚动条
        }

        #region 属性   
        /// <summary>获取或设置数据源数据结构
        /// 
        /// </summary>
        public MindMapNodeStructBase DataStruct { get; set; }

        public float _CurrentScaling = 1;
        /// <summary> 获取或设置当前的缩放比例
        /// 
        /// </summary>
        public float CurrentScaling
        {
            get { return _CurrentScaling; }
            set
            {
              
                Scaling_button.Text = ((int)(this._CurrentScaling * 100)).ToString() + "%";//将当前比例显示到前台
                _CurrentScaling = value;
                mindMapNode.CurrentScaling = value;
            }
        }

        /// <summary> 获取或设置根节点容器
        /// 
        /// </summary>
        public MindMapNodeContainer BaseNode { get { return mindMapNode; } set { mindMapNode = value; } }
        #endregion 属性

        #region 公开方法

        /// <summary> 为思维导图绑定数据
        /// 
        /// </summary>
        /// <typeparam name="NodeContent">采用哪种内容布局</typeparam>
        /// <typeparam name="DataEntity">数据的模型</typeparam>
        /// <param name="DataSource">数据源</param>
        /// <param name="ParentID">父ID，留空为根节点</param>
        /// <returns>返回添加后的节点容器</returns>
        public List<MindMapNodeContainer> SetDataSource<NodeContent, DataEntity>(List<DataEntity> DataSource, string ParentID = null) where NodeContent : MindMapNodeContentBase, new()
        {
            if (DataStruct == null) throw new Exception("DataStruct为空：你需要先指定数据源的结构，再绑定数据源");
            PropertyInfo IDProperty = typeof(DataEntity).GetProperty(DataStruct.MindMapID);
            PropertyInfo ParentProperty = typeof(DataEntity).GetProperty(DataStruct.MindMapParentID);
            bool NullParent = string.IsNullOrEmpty(ParentID);
            List<DataEntity> CurrentAddList = new List<DataEntity>();

            if (NullParent)
            {
                CurrentAddList = DataSource.Where(T1 => string.IsNullOrEmpty(ParentProperty.GetValue(T1).ToString())).ToList();//没有父节点就取父节点为空的记录
                if (CurrentAddList.Count == 0) throw new Exception("未找到根节点");
                if (CurrentAddList.Count > 1) throw new Exception("不允许有多个根节点");
            }
            else
            {
                CurrentAddList = DataSource.Where(T1 => ParentProperty.GetValue(T1).ToString() == ParentID).ToList();
            }

            List<MindMapNodeContainer> ContainerList = new List<MindMapNodeContainer>();

            foreach (DataEntity AddDataItem in CurrentAddList)
            {
                string CurrentId = IDProperty.GetValue(AddDataItem).ToString();
                List<MindMapNodeContainer> ContainerListTemp = SetDataSource<NodeContent, DataEntity>(DataSource, CurrentId);
                MindMapNodeContainer NewNode = new MindMapNodeContainer();
                if (NullParent) NewNode = mindMapNode;//如果没有父节点就赋值根节点



                NewNode.SetNodeContent<NodeContent>(DataStruct);
                ContainerListTemp.ForEach(item => NewNode.AddNode(item));
                NewNode.NodeContent.DataItem = AddDataItem;
                ContainerList.Add(NewNode);
            }
            if (NullParent)
            {
                SetEvent(mindMapNode);//所有节点都绑定完了就统一为这些节点绑定事件
                ScrollCenter();
            }
            return ContainerList;
        }
        #region 绑定数据老代码
        /// <summary> 为思维导图载入数据
        /// 
        /// </summary>
        /// <typeparam name="NodeContent">采用哪种内容布局</typeparam>
        /// <typeparam name="DataEntity">数据的模型</typeparam>
        /// <param name="DataSource"></param>
        //public void SetDataSource<NodeContent, DataEntity>(List<DataEntity> DataSource) where NodeContent : MindMapNodeContentBase, new()
        //{
        //    if (DataStruct == null) throw new Exception("DataStruct为空：你需要先指定数据源的结构，再绑定数据源");
        //    PropertyInfo IDProperty = typeof(DataEntity).GetProperty(DataStruct.MindMapID);
        //    PropertyInfo ParentProperty = typeof(DataEntity).GetProperty(DataStruct.MindMapParentID);
        //    //没有父节点就取父节点为空的记录
        //    List<DataEntity> CurrentAddList = DataSource.Where(T1 => string.IsNullOrEmpty(ParentProperty.GetValue(T1).ToString())).ToList();


        //    if (CurrentAddList.Count == 0) throw new Exception ("未找到根节点");
        //    if (CurrentAddList.Count > 1) throw new Exception("不允许有多个根节点");

        //    string CurrentId = IDProperty.GetValue(CurrentAddList[0]).ToString();
        //    List<MindMapNodeContainer> ContainerList = SetDataSource<NodeContent, DataEntity>(DataSource, CurrentId);
        //    ContainerList.ForEach(item => mindMapNode.AddNode(item));
        //    mindMapNode.SetNodeContent<NodeContent>(DataStruct);
        //    mindMapNode.NodeContent.DataItem = CurrentAddList[0];

        //    SetEvent(mindMapNode);
        //}

        //private List<MindMapNodeContainer> SetDataSource<NodeContent, DataEntity>(List<DataEntity> DataSource, string ParentID) where NodeContent : MindMapNodeContentBase, new()
        //{
        //    PropertyInfo IDProperty = typeof(DataEntity).GetProperty(DataStruct.MindMapID);
        //    PropertyInfo ParentProperty = typeof(DataEntity).GetProperty(DataStruct.MindMapParentID);
        //    //有父节点就取ParentID为父节点的记录
        //    List<DataEntity> CurrentAddList = DataSource.Where(T1 => ParentProperty.GetValue(T1).ToString() == ParentID).ToList();
        //    List<MindMapNodeContainer> ContainerList = new List<MindMapNodeContainer>();

        //    foreach (DataEntity AddDataItem in CurrentAddList)
        //    {
        //        string CurrentId = IDProperty.GetValue(AddDataItem).ToString();
        //        List<MindMapNodeContainer> ContainerListTemp = SetDataSource<NodeContent, DataEntity>(DataSource, CurrentId);
        //        MindMapNodeContainer NewNode = new MindMapNodeContainer ();
        //        NewNode.SetNodeContent<NodeContent>(DataStruct);
        //        ContainerListTemp.ForEach(item => NewNode.AddNode(item));              
        //        NewNode.NodeContent.DataItem = AddDataItem;
        //        ContainerList.Add(NewNode);
        //    }
        //    return ContainerList;
        //} 
        #endregion 绑定数据老代码

        /// <summary> 获取所有被选中的节点
        /// 
        /// </summary>
        /// <returns></returns>
        public List<MindMapNodeContainer> GetSelectedNode()
        {
            List<MindMapNodeContainer> ResultList = new List<MindMapNodeContainer>();
            ResultList = mindMapNode.GetChidrenNode(true);
            ResultList.Add(mindMapNode);
            ResultList = ResultList.Where(T1 => T1.NodeContent.Selected == true).ToList();
            return ResultList;
        }

        #endregion 公开方法

        #region 公开事件委托
        private void SetEvent(MindMapNodeContainer MindMapContainerParame)
        {
            AddOrRemoveEvent(MindMapContainerParame, false);//先删除事件，避免重复添加
            AddOrRemoveEvent(MindMapContainerParame, true);//添加事件
        }

        /// <summary> 为节点添加或删除事件
        /// 
        /// </summary>
        /// <param name="MindMapContainerParame">要添加或删除的节点容器</param>
        /// <param name="AddEvent">[true：添加事件；false：删除事件]</param>
        private void AddOrRemoveEvent(MindMapNodeContainer MindMapContainerParame, bool AddEvent)
        {
            #region 为节点容器添加事件
            //节点容器添加事件
            List<MindMapNodeContainer> NodeContainsList = MindMapContainerParame.GetChidrenNode(true);//获取所有节点容器
            NodeContainsList.Add(MindMapContainerParame);//包括自己

            List<Control> NodeContentList = new List<Control>();//用List来收集所有节点内容的控件
            NodeContainsList.ForEach(NodeItem =>
            {
                if (AddEvent)
                {
                    NodeItem.EmptyRangeClick += new EventHandler(mindMapNode_EmptyRangeClick);
                    NodeItem.EmptyRangeMouseDown += new MouseEventHandler(mindMapNode_EmptyRangeMouseDown);
                    NodeItem.EmptyRangeMouseMove += new MouseEventHandler(mindMapNode_EmptyRangeMouseMove);
                    NodeItem.EmptyRangeMouseUp += new MouseEventHandler(mindMapNode_EmptyRangeMouseUp);
                    NodeItem.AddChidrenNode += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeAddChidrenNode);
                    NodeItem.RemoveChidrenNode += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeRemoveChidrenNode);
                    NodeItem.AddNodeContent += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeRemoveChidrenNode);                    
                }
                else
                {
                    //避免重复添加委托队列
                    NodeItem.EmptyRangeClick -= new EventHandler(mindMapNode_EmptyRangeClick);
                    NodeItem.EmptyRangeMouseDown -= new MouseEventHandler(mindMapNode_EmptyRangeMouseDown);
                    NodeItem.EmptyRangeMouseMove -= new MouseEventHandler(mindMapNode_EmptyRangeMouseMove);
                    NodeItem.EmptyRangeMouseUp -= new MouseEventHandler(mindMapNode_EmptyRangeMouseUp);
                    NodeItem.AddChidrenNode -= new MindMapNodeContainer.MindMapEventHandler(MindMapNodeAddChidrenNode);
                    NodeItem.RemoveChidrenNode -= new MindMapNodeContainer.MindMapEventHandler(MindMapNodeRemoveChidrenNode);
                    NodeItem.AddNodeContent -= new MindMapNodeContainer.MindMapEventHandler(MindMapNodeRemoveChidrenNode);
                }
                if(NodeItem.NodeContent!=null) NodeContentList.AddRange(NodeItem.NodeContent.GetNodeControl());//获取当前节点内容的所有控件
             
            });
            #endregion 为节点容器添加事件
            #region 为节点内容添加事件
            NodeContentList.ForEach(ControlItem =>
            {
                if (AddEvent)
                {
                    ControlItem.MouseDown += new MouseEventHandler(mindMapNode_MindMapNodeMouseDown);
                    ControlItem.MouseUp += new MouseEventHandler(mindMapNode_MindMapNodeMouseUp);
                    ControlItem.MouseMove += new MouseEventHandler(mindMapNode_MindMapNodeMouseMove);
                    ControlItem.MouseEnter += new EventHandler(mindMapNode_MindMapNodeMouseEnter);
                    ControlItem.MouseLeave += new EventHandler(mindMapNode_MindMapNodeMouseLeave);
                    ControlItem.MouseClick += new MouseEventHandler(mindMapNode_MindMapNodeMouseClick);
                    ControlItem.MouseDoubleClick += new MouseEventHandler(mindMapNode_MouseDoubleClick);
                }
                else
                {
                    //避免重复添加委托队列
                    ControlItem.MouseDown -= new MouseEventHandler(mindMapNode_MindMapNodeMouseDown);
                    ControlItem.MouseUp -= new MouseEventHandler(mindMapNode_MindMapNodeMouseUp);
                    ControlItem.MouseMove -= new MouseEventHandler(mindMapNode_MindMapNodeMouseMove);
                    ControlItem.MouseEnter -= new EventHandler(mindMapNode_MindMapNodeMouseEnter);
                    ControlItem.MouseLeave -= new EventHandler(mindMapNode_MindMapNodeMouseLeave);
                    ControlItem.MouseClick -= new MouseEventHandler(mindMapNode_MindMapNodeMouseClick);
                    ControlItem.MouseDoubleClick -= new MouseEventHandler(mindMapNode_MouseDoubleClick);
                }


              
            });
            #endregion 为节点内容添加事件
        }

        #region 节点内容相关事件方法

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

            MindMapNodeContentBase SenderObject = ((Control)sender).GetNodeContent();
            #region 取消所有节点的编辑状态
            List<MindMapNodeContainer> MindMapNodeList = mindMapNode.GetChidrenNode(true);
            MindMapNodeList.Add(mindMapNode);
            foreach (MindMapNodeContainer ContainerItem in MindMapNodeList)
            {
                if (ContainerItem.NodeContent.Edited)
                {
                    if (SenderObject.ParentMindMapNode == ContainerItem) return;
                    ContainerItem.NodeContent.Edited = false;
                    return;
                }
            }
            #endregion 取消所有节点的编辑状态
            if (Control.ModifierKeys != Keys.Control)//不按住ctrl就单选
            {
                MindMapNodeList.ForEach(T1 => T1.NodeContent.Selected = false);
                SenderObject.Selected = true;
            }
            else//按住ctrl可单选
            {
                SenderObject.Selected = !SenderObject.Selected;
            }
            if (MindMapNodeMouseClick != null) MindMapNodeMouseClick(this, e);
        }

        /// <summary> 双击某节点后编辑某节点
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender == null) return;
            MindMapNodeContentBase SenderObject = ((Control)sender).GetNodeContent();
            SenderObject.Edited = true;
            if (MindMapNodeMouseDoubleClick != null) MindMapNodeMouseDoubleClick(this, e);

        }

        /// <summary> 当节点添加时发生
        /// 
        /// </summary>
        /// <param name="Sender">发生事件的节点</param>
        /// <param name="Chidren">被添加的节点</param>
        private void MindMapNodeAddChidrenNode(MindMapNodeContainer Sender, MindMapNodeContainer Chidren)
        {
            AddOrRemoveEvent(Chidren, false);
            AddOrRemoveEvent(Chidren, true);
            if (MindMapAddNode != null) MindMapAddNode(Sender, Chidren);



        }
        /// <summary> 当节点被移除时发生
        /// 
        /// </summary>
        /// <param name="Sender">发生事件的节点</param>
        /// <param name="Chidren">被移除的节点</param>
        private void MindMapNodeRemoveChidrenNode(MindMapNodeContainer Sender, MindMapNodeContainer Chidren)
        {
            AddOrRemoveEvent(Chidren, false);
            if (MindMapRemoveNode != null) MindMapRemoveNode(Sender, Chidren);
        }

        /// <summary>添加节点内容时发生
        /// 
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void MindMapNodeAddContent(MindMapNodeContainer Sender, MindMapNodeContainer e)
        {
            SetEvent(Sender);
            if (MindMapAddContent != null) MindMapAddContent(Sender, null);
        }

        #endregion 节点内容相关事件方法

        #region 非节点内容的事件方法

        /// <summary> 空白处被单击取消所有选中        
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_EmptyRangeClick(object sender, EventArgs e)
        {
            List<MindMapNodeContainer> MindMapNodeList = mindMapNode.GetChidrenNode(true);
            MindMapNodeList.Add(mindMapNode);
            foreach (MindMapNodeContainer ContainerItem in MindMapNodeList)
            {
                if (ContainerItem.NodeContent.Edited)
                {
                    ContainerItem.NodeContent.Edited = false;
                    return;
                }
            }
            MindMapNodeList.ForEach(T1 => T1.NodeContent.Selected = false);

            if (EmptyRangeClick != null) EmptyRangeClick(sender, e);
        }

        /// <summary> 空白处鼠标按下
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_EmptyRangeMouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            MindMap_Panel_MouseDown(sender, e);
            if (EmptyRangeMouseDown != null) EmptyRangeMouseDown(sender, e);
        }

        /// <summary>空白处鼠标移动
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_EmptyRangeMouseMove(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseMove(sender, e);
            if (EmptyRangeMouseMove != null) EmptyRangeMouseMove(sender, e);
        }

        /// <summary> 空白处鼠标弹起
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_EmptyRangeMouseUp(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseUp(sender, e);
            if (EmptyRangeMouseUp != null) EmptyRangeMouseUp(sender, e);
        }

        #endregion 非节点内容的事件方法

        #region 对外开放的节点内容相关事件委托

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

        /// <summary> 节点被双击时
        /// 
        /// </summary>
        [Browsable(true), Description("节点被双击时")]
        public event MouseEventHandler MindMapNodeMouseDoubleClick;

        /// <summary> 鼠标在节点上移动时
        /// 
        /// </summary>
        [Description("鼠标在节点上移动时")]
        public event MouseEventHandler MindMapNodeMouseMove;

        /// <summary> 某节点添加子节点时发生
        /// 
        /// </summary>
        [Description("某节点添加子节点时发生")]
        public event MindMapNodeContainer.MindMapEventHandler MindMapAddNode;

        /// <summary> 某节点移除子节点时发生
        /// 
        /// </summary>
        [Description("某节点移除子节点时发生")]
        public event MindMapNodeContainer.MindMapEventHandler MindMapRemoveNode;

        /// <summary> 某节点设置节点内容布局时发生
        /// 
        /// </summary>
        [Description("某节点设置节点内容布局时发生")]
        public event MindMapNodeContainer.MindMapEventHandler MindMapAddContent;
        #endregion 对外开放的节点内容相关事件委托

        #region 节点容器相关事件委托[在非节点处发生的事件]

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

        #endregion 节点容器相关事件委托[在非节点处发生的事件]


        /// <summary> 焦点在思维导图任何位置时，键盘按下事件
        /// 
        /// </summary>
        public event KeyEventHandler MindNodemapKeyDown;

        #endregion 公开事件委托    

        #region 鼠标中键拖动滚动条

        private bool IsMouseMove = false;//是否可以开始拖动鼠标了
        private Point MoveValue = new Point();//鼠标拖动前的位置;

        /// <summary> 按下中键时可拖动滚动条
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMap_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right)
            {
                MoveValue = e.Location;
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
                Point PointTemp = new Point();
                PointTemp.X = MoveValue.X - e.Location.X;
                PointTemp.Y = MoveValue.Y - e.Location.Y;
                Point ResultPoint = new Point(Main_Panel.HorizontalScroll.Value + PointTemp.X, Main_Panel.VerticalScroll.Value + PointTemp.Y);
                Main_Panel.AutoScrollPosition = ResultPoint;
                RecordScalingPosition();
            }
        }



        #endregion 鼠标中键拖动滚动条

        #region 当控件尺寸改变时更改滚动条尺寸

        /// <summary> 思维导图尺寸改变时，滚动条尺寸也要跟着变
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_Resize(object sender, EventArgs e)
        {
            ResetMindMapPanelSize();            
        }

        /// <summary> 当本控件的尺寸改变时，滚动条尺寸也要改变
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMap_Panel_Resize(object sender, EventArgs e)
        {
            //不知道什么原因，如果本控件尺寸改变后如果立即设置滚动条的尺寸，会出现Bug
            //例如外部Winform添加本控件后，Dock设为Fill，在窗体最大化，或从最大或变成正常态时将会出现Bug
            //所以当本控件尺寸发生改变时延迟200毫秒设置滚动条尺寸        
            DelayShow();
        }

        /// <summary> 重新设置导图的容器在控件中的尺寸
        /// 
        /// </summary>
        private void ResetMindMapPanelSize()
        {
            #region 设置思维导图的容器的尺寸
            Scroll_panel.Location = new Point(-Main_Panel.HorizontalScroll.Value, -Main_Panel.VerticalScroll.Value);
            int MaxHeight = Main_Panel.Height * 2;//容器最大高度，父容器的2倍
            int MaxWidth = Main_Panel.Width * 2;//容器最大宽度，父容器的2倍
            int MinHeight = mindMapNode.Height * 2;//容器最小高度，自身高度的两倍
            int MinWidth = mindMapNode.Width * 2;//容器最小宽度，自身宽度的两倍
            Scroll_panel.Height = MaxHeight > MinHeight ? MaxHeight : MinHeight;//优先最大高度
            Scroll_panel.Width = MaxWidth > MinWidth ? MaxWidth : MinWidth;//优先最大宽度 
            #endregion 设置思维导图的容器的尺寸
            #region 思维导图相对于容器居中
            int IntTemp = Scroll_panel.Height - mindMapNode.Height;
            IntTemp = IntTemp / 2;
            mindMapNode.Top = IntTemp;
            IntTemp = Scroll_panel.Width - mindMapNode.Width;
            IntTemp = IntTemp / 2;
            mindMapNode.Left = IntTemp;
            #endregion 思维导图相对于容器居中
            SetScroll();

        }
        
        PointF CurrentProportion = new PointF ();//当前滚动条比例
        /// <summary> 记录当前滚动条比例 [配合SetScroll方法可以按比例滚动]
        /// 
        /// </summary>
        private void RecordScalingPosition()
        {
            float LeftProportionTemp = (Main_Panel.Width / 2) + Main_Panel.HorizontalScroll.Value;
            float TopProportionTemp = (Main_Panel.Height / 2) + Main_Panel.VerticalScroll.Value;
            
            CurrentProportion.X = LeftProportionTemp / ((float)Scroll_panel.Width);
            CurrentProportion.Y = TopProportionTemp / ((float)Scroll_panel.Height);
        }

        /// <summary> 将思维导图滚动至居中位置
        /// 
        /// </summary>
        public void ScrollCenter()
        {
            #region 将容器滚动至居中位置
            int IntX = this.Scroll_panel.Width - this.Width;
            int IntY = this.Scroll_panel.Height - this.Height;
            Point PointTemp = new Point(IntX / 2, IntY / 2);
            Main_Panel.AutoScrollPosition = PointTemp;
            #endregion 将容器滚动至居中位置
            RecordScalingPosition();
        }
        
        /// <summary> 设置滚动条比例
        /// [ 调用前请使用RecordScalingPosition记录比例]
        /// </summary>
        private void SetScroll()
        {
          
            int LeftTemp =(int)(Scroll_panel.Width* CurrentProportion.X);
            int TopTemp = (int)(Scroll_panel.Height * CurrentProportion.Y);
            LeftTemp = LeftTemp - (Main_Panel.Width / 2);
            TopTemp = TopTemp - (Main_Panel.Height / 2);
            Main_Panel.AutoScrollPosition = new Point(LeftTemp, TopTemp);
        }

        #endregion 当控件尺寸改变时更改滚动条尺寸

        #region 按住Ctrl+滚轮缩放
        private int PaintNum = 0;//倒计时的时间
        private Thread PaintTread = null;//用于倒计时的线程（不阻塞UI线程）
        /// <summary> 延时200毫秒刷新滚动条尺寸和位置
        /// 
        /// </summary>
        private void DelayShow()
        {
            PaintNum = 300;//倒计时300毫秒
            if (PaintTread == null)//为空表示线程没有开启线程
            {
                PaintTread = new Thread(() =>
                {
                    while (PaintNum > 0)//时间没到就一直循环
                    {
                        Thread.Sleep(10);
                        PaintNum = PaintNum - 10;
                    }
                    this.Invoke(new Action(() =>
                    {
                        mindMapNode.Visible = false;//当所有尺寸设置完成后再显示，可以有效加快控件的绘制过程
                        this.CurrentScaling = this._CurrentScaling;
                        mindMapNode.Visible = true;
                        ResetMindMapPanelSize();//将思维导图居中                        
                    }));
                    PaintTread = null;//将自己置空来表示自己已经运行结束了
                });
                PaintTread.Start();
            }
        }

        /// <summary> 滚轮放大缩小(当按住Control后滚轮无法控制滚动条)
        /// 
        /// </summary>       
        public bool PreFilterMessage(ref Message m)
        {

            switch (m.Msg)
            {
                case 522:
                    //case 0x0100:
                    List<IntPtr> HandleList = this.GetAllControl().Select(T1 => T1.Handle).ToList();//获取思维导图所有控件的窗口句柄
                    IntPtr MouseHandle = WindowsAPI.WindowFromPoint(Control.MousePosition);//获取鼠标所在窗口句柄
                    if (!HandleList.Contains(MouseHandle)) break;//鼠标不在思维导图控件上时就不缩放
                    #region 缩放相关代码
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        #region 获取本次缩放的值[1%-50%:每次缩放10%；50%-100%:每次缩放20%；100%以上:每次缩放50%]
                        float ChangeValue = 0.1F;//每次放大或缩小10%
                        if (0.5 <= this._CurrentScaling && this._CurrentScaling < 1.5)
                        {
                            ChangeValue = 0.2F;
                        }
                        else if (1.5 <= this._CurrentScaling)
                        {
                            ChangeValue = 0.5F;
                        }
                        #endregion 获取本次缩放的值[1%-50%:每次缩放10%；50%-100%:每次缩放20%；100%以上:每次缩放50%]

                        float ResultScaling = 1;//结果的比例
                        int WheelValue = m.WParam.ToInt32();
                        if (WheelValue < 0)
                        {
                            //缩小
                            ResultScaling = this._CurrentScaling - ChangeValue;
                            if (ResultScaling <= 0) ResultScaling = 0.1F;
                        }
                        else
                        {
                            //放大
                            ResultScaling = this._CurrentScaling + ChangeValue;
                        }
                        this._CurrentScaling = ResultScaling;
                        Scaling_button.Text = ((int)(this._CurrentScaling * 100)).ToString() + "%";
                        DelayShow(); //延时200毫秒显示,如果200毫秒之内没有再请求显示那就显示，减少重复绘制的次数
                        return true;
                    }
                    #endregion 缩放相关代码
                    break;

            }
            return false;
        }

        /// <summary> 当前比例被单击就返回100%比例
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scaling_button_Click(object sender, EventArgs e)
        {
            mindMapNode.Visible = false;
            CurrentScaling = 1;//将比例设置为100%
            mindMapNode.Visible = true;
        }

        /// <summary> 禁止每当控件获得焦点后横向滚动条总会自动滚动到根节点
        /// 
        /// </summary>
        /// <param name="activeControl"></param>
        /// <returns></returns>
        //protected override Point ScrollToControl(Control activeControl)
        //{
        //    return Main_Panel.AutoScrollPosition;
        //}
        #endregion 按住Ctrl+滚轮缩放

        /// <summary> 控件中键盘按下事件
        /// 
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (MindNodemapKeyDown != null) MindNodemapKeyDown(this, new KeyEventArgs(keyData));
            return base.ProcessDialogKey(keyData);
        }

        private void Main_Panel_Scroll(object sender, ScrollEventArgs e)
        {
            RecordScalingPosition();
        }
    }
}