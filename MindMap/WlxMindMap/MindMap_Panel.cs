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


namespace WlxMindMap
{
    /// <summary>思维导图的本体可以直接在窗体设计器里将该控件直接拖动到窗体上
    /// 负责了以下任务：
    /// 1、右键拖拽节点时移动思维导图的位置
    /// 2、左键拖动鼠标时绘制矩形框来选中节点
    /// 3、向用户提供一些事件和方法供用户使用代码操作如：绑定数据、获取选中节点、设置缩放比例等
    /// </summary>

    public partial class MindMap_Panel : UserControl, IMessageFilter
    {
        /// <summary> 根节点容器
        /// 
        /// </summary>
        private MindMapNodeContainer mindMapNode = null;

        public MindMap_Panel()
        {
            InitializeComponent();
            //if(false)
            //new Thread(() => {
            //    Thread.Sleep(20);
            //    this.Invoke(new Action (() => {
            //        Descraption_label.Visible = false;
            //    }));
            //    }).Start();

            Descraption_label.Visible = false;
            this.AutoScroll = false;
            #region 根节点容器
            InitBaseNode();//初始化根节点
            #endregion 根节点容器            
            Application.AddMessageFilter(this);//当按住Control后滚轮无法控制滚动条
        }
        private void InitBaseNode()
        {
            if (mindMapNode != null)
            {
                mindMapNode.GetAllControl().ForEach(ControlItem => ControlItem.Dispose());//把老数据的资源全部释放
                mindMapNode.Dispose();//释放资源
            }
            this.mindMapNode = new MindMapNodeContainer();
            this.mindMapNode.BackColor = System.Drawing.Color.White;
            this.mindMapNode.Location = new System.Drawing.Point(181, 166);
            this.mindMapNode.Name = "mindMapNode";
            this.mindMapNode.ParentNode = null;
            this.mindMapNode.Size = new System.Drawing.Size(86, 23);
            this.mindMapNode.TabIndex = 0;
            this.mindMapNode.EmptyRangeMouseClick += new MouseEventHandler(this.mindMapNode_EmptyRangeMouseClick);
            this.mindMapNode.EmptyRangeMouseDown += new System.Windows.Forms.MouseEventHandler(this.mindMapNode_EmptyRangeMouseDown);
            this.mindMapNode.EmptyRangeMouseUp += new System.Windows.Forms.MouseEventHandler(this.mindMapNode_EmptyRangeMouseUp);
            this.mindMapNode.EmptyRangeMouseMove += new System.Windows.Forms.MouseEventHandler(this.mindMapNode_EmptyRangeMouseMove);
            this.mindMapNode.RemoveChidrenNode += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeRemoveChidrenNode);
            this.mindMapNode.Resize += new System.EventHandler(this.mindMapNode_Resize);
            this.Scroll_panel.Controls.Add(this.mindMapNode);
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
                        
                _CurrentScaling = value;
                Scaling_button.Text = ((int)(this._CurrentScaling * 100)).ToString() + "%";//将当前比例显示到前台
                mindMapNode.CurrentScaling = value;
           
            }
        }

        /// <summary> 获取或设置根节点容器
        /// 
        /// </summary>
        public MindMapNodeContainer BaseNode { get { return mindMapNode; } set { mindMapNode = value; } }
        #endregion 属性

        #region 公开方法

        /// <summary> 根据DataStruct的结构取出根节点
        /// 
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="DataSource">数据源</param>
        /// <returns>根节点的实例</returns>
        private T GetBaseData<T>(List<T> DataSource)
        {
            if (DataStruct == null) throw new Exception("DataStruct为空：你需要先指定数据源的结构");
            PropertyInfo IDProperty = typeof(T).GetProperty(DataStruct.MindMapID);
            PropertyInfo ParentProperty = typeof(T).GetProperty(DataStruct.MindMapParentID);

            List<T> CurrentAddList;
            var Temp1 = DataSource.Select(T1 => new { ID = IDProperty.GetValue(T1).ToString(), ParentID = ParentProperty.GetValue(T1).ToString() });//将父子关系的属性反射出来
            List<string> IDList = Temp1.Select(T1 => T1.ID).ToList();//所有ID
            Temp1 = Temp1.Where(T1 => !IDList.Contains(T1.ParentID)).ToList();//筛选出父节点不在当前DataSource的记录

            if (Temp1.Count() > 1) throw new Exception("不允许有多个根节点");
            if (Temp1.Count() != 1) throw new Exception("未找到根节点");

            //筛选出来只有一个符合要求的那就把他作为根节点
            string BaseNodeID = Temp1.FirstOrDefault().ID;
            CurrentAddList = DataSource.Where(T1 => IDProperty.GetValue(T1).ToString() == BaseNodeID).ToList();//没有父节点就优先取父节点为空的记录
            return CurrentAddList.FirstOrDefault();
        }

        /// <summary> 为思维导图绑定数据
        /// 
        /// </summary>
        /// <typeparam name="NodeContent">采用哪种内容布局</typeparam>
        /// <typeparam name="DataEntity">数据的模型</typeparam>
        /// <param name="DataSource">数据源</param>
        /// <param name="ParentID">父ID，留空则表示智能获取根节点ID</param>
        /// <returns>返回添加后的节点容器</returns>
        public List<MindMapNodeContainer> SetDataSource<NodeContent, DataEntity>(List<DataEntity> DataSource, string ParentID = null) where NodeContent : MindMapNodeContentBase, new()
        {
            if (DataStruct == null) throw new Exception("DataStruct为空：你需要先指定数据源的结构，再绑定数据源");
            PropertyInfo IDProperty = typeof(DataEntity).GetProperty(DataStruct.MindMapID);
            PropertyInfo ParentProperty = typeof(DataEntity).GetProperty(DataStruct.MindMapParentID);
            List<DataEntity> CurrentAddList;
            bool IsBaseNode = string.IsNullOrEmpty(ParentID);
            if (IsBaseNode)
            {
                InitBaseNode();//如果是根节点就初始化根节点
                CurrentAddList = new List<DataEntity>();
                CurrentAddList.Add(GetBaseData<DataEntity>(DataSource));
                
                #region MyRegion
              
                #endregion
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
                if (IsBaseNode) NewNode = mindMapNode;//如果没有父节点就赋值根节点
                NewNode.SetNodeContent<NodeContent>(DataStruct);
                ContainerListTemp.ForEach(item => NewNode.AddNode(item));
                NewNode.NodeContent.DataItem = AddDataItem;
                ContainerList.Add(NewNode);
            }
            if (IsBaseNode)
            {
                SetEvent(mindMapNode);//所有节点都绑定完了就统一为这些节点绑定事件
                ScrollCenter();              
            }
            return ContainerList;
        }
  
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

        /// <summary> 获取被选中的根节点
        /// 例如节点A下有B、C两个节点，如果此时都被选中本方法只会返回A
        /// </summary>
        /// <returns></returns>
        public List<MindMapNodeContainer> GetSelectedBaseNode()
        {
            List<MindMapNodeContainer> ContainerList=  GetSelectedNode();
            ContainerList = ContainerList.Where(T1 => !ParentContaine(ContainerList, T1.ParentNode)).ToList();
            return ContainerList;
        }

        /// <summary> 递归判断是否有父节点存在于集合中
        /// 
        /// </summary>
        /// <param name="ContaineListPatam"></param>
        /// <param name="CurrentNode"></param>
        /// <returns></returns>
        private bool ParentContaine(List<MindMapNodeContainer> ContaineListPatam , MindMapNodeContainer CurrentNode)
        {
            if (CurrentNode == null) return false;

            if (ContaineListPatam.Contains(CurrentNode))
            {
                return true;
            }
            else
            {
                return ParentContaine(ContaineListPatam, CurrentNode.ParentNode);
            }
        }

        /// <summary> 刷新布局,当思维导图出现异常时可以尝试调用重新布局
        /// 
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.CurrentScaling = this.CurrentScaling;

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
            if (MindMapContainerParame == null) return;
            //节点容器添加事件
            List<MindMapNodeContainer> NodeContainsList = MindMapContainerParame.GetChidrenNode(true);//获取所有节点容器
            NodeContainsList.Add(MindMapContainerParame);//包括自己
            #region 为节点容器添加事件
            List<Control> NodeContentList = new List<Control>();//用List来收集所有节点内容的控件
            NodeContainsList.ForEach(NodeItem =>
            {
                if (AddEvent)
                {
                    NodeItem.EmptyRangeMouseClick += new MouseEventHandler(mindMapNode_EmptyRangeMouseClick);
                    NodeItem.EmptyRangeMouseDown += new MouseEventHandler(mindMapNode_EmptyRangeMouseDown);
                    NodeItem.EmptyRangeMouseMove += new MouseEventHandler(mindMapNode_EmptyRangeMouseMove);
                    NodeItem.EmptyRangeMouseUp += new MouseEventHandler(mindMapNode_EmptyRangeMouseUp);
                    NodeItem.AddChidrenNode += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeAddChidrenNode);
                    NodeItem.RemoveChidrenNode += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeRemoveChidrenNode);
                    NodeItem.AddNodeContent += new MindMapNodeContainer.MindMapEventHandler(MindMapNodeAddContent);
                    NodeItem.NodeSizeChanged += new Action (HideMindMap);
                }
                else
                {
                    //避免重复添加委托队列
                    NodeItem.EmptyRangeMouseClick -= new MouseEventHandler(mindMapNode_EmptyRangeMouseClick);
                    NodeItem.EmptyRangeMouseDown -= new MouseEventHandler(mindMapNode_EmptyRangeMouseDown);
                    NodeItem.EmptyRangeMouseMove -= new MouseEventHandler(mindMapNode_EmptyRangeMouseMove);
                    NodeItem.EmptyRangeMouseUp -= new MouseEventHandler(mindMapNode_EmptyRangeMouseUp);
                    NodeItem.AddChidrenNode -= new MindMapNodeContainer.MindMapEventHandler(MindMapNodeAddChidrenNode);
                    NodeItem.RemoveChidrenNode -= new MindMapNodeContainer.MindMapEventHandler(MindMapNodeRemoveChidrenNode);
                    NodeItem.AddNodeContent -= new MindMapNodeContainer.MindMapEventHandler(MindMapNodeAddContent);
                    NodeItem.NodeSizeChanged -= new Action(HideMindMap);
                }
                if (NodeItem.NodeContent != null) NodeContentList.AddRange(NodeItem.NodeContent.GetEventControl());//获取当前节点内容的所有控件
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
                    ControlItem.MouseDoubleClick += new MouseEventHandler(mindMapNode_MouseDoubleClick);
                    ControlItem.AllowDrop = true;//允许接收拖拽行为
                    ControlItem.DragOver += new DragEventHandler(MindMapNode_DragOver);
                    ControlItem.DragEnter += new DragEventHandler(MindMapNode_DragEnter);
                    ControlItem.DragDrop += new DragEventHandler(MindMapNode_DragDrop);

                }
                else
                {
                    //避免重复添加委托队列
                    ControlItem.MouseDown -= new MouseEventHandler(mindMapNode_MindMapNodeMouseDown);
                    ControlItem.MouseUp -= new MouseEventHandler(mindMapNode_MindMapNodeMouseUp);
                    ControlItem.MouseMove -= new MouseEventHandler(mindMapNode_MindMapNodeMouseMove);
                    ControlItem.MouseEnter -= new EventHandler(mindMapNode_MindMapNodeMouseEnter);
                    ControlItem.MouseLeave -= new EventHandler(mindMapNode_MindMapNodeMouseLeave);                    
                    ControlItem.MouseDoubleClick -= new MouseEventHandler(mindMapNode_MouseDoubleClick);

                    ControlItem.DragOver -= new DragEventHandler(MindMapNode_DragOver);
                    ControlItem.DragEnter -= new DragEventHandler(MindMapNode_DragEnter);
                    ControlItem.DragDrop -= new DragEventHandler(MindMapNode_DragDrop);

                }
            });
            #endregion 为节点内容添加事件
        }

        #region 节点内容相关事件方法
        #region 拖拽会用到的私有方法

        /// <summary> 是否从其他节点拖出过来的
        /// 
        /// </summary>
        /// <param name="IDataObjectParame"></param>
        /// <returns></returns>
        private bool IsElseNode(IDataObject IDataObjectParame)
        {
            List<object> objList = new List<object>();
            foreach (string FormatItem in IDataObjectParame.GetFormats())
            {
                object obj = IDataObjectParame.GetData(FormatItem);
                objList.Add(obj);
            }
            objList = objList.Where(T1 => T1 is MindMapNodeContentBase).ToList();
            if (objList.Count == 1)
                return true;
            return false;

        }

        /// <summary> 判断是否允许拖动到指定节点下[不允许拖动到被选中节点和它的子节点下]
        /// 
        /// </summary>
        /// <param name="DragedContainer"></param>
        /// <returns></returns>
        private bool AllowDrag(MindMapNodeContainer DragedContainer)
        {
            #region 获取所有选中节点和他的子节点
            List<MindMapNodeContainer> NodeContainerList = GetSelectedNode();//获取所有选中节点          
            List<MindMapNodeContainer> SelectedAndChidrenList = new List<MindMapNodeContainer>();
            SelectedAndChidrenList.AddRange(NodeContainerList);//获取选中节点
            NodeContainerList.ForEach(T1 => SelectedAndChidrenList.AddRange(T1.GetChidrenNode(true)));//获取选中节点的子节点
            SelectedAndChidrenList = SelectedAndChidrenList.Distinct().ToList();//去重，两个已选中节点可能是父子关系
            #endregion 获取所有选中节点和他的子节点

            if (SelectedAndChidrenList.Contains(DragedContainer))//不允许拖动到被选中节点和它的子节点下
                return false;
            return true;
        }

        #endregion 拖拽会用到的私有方法

        /// <summary> 传入发生事件节点的Sender参数返回它的节点容器
        /// 
        /// </summary>
        /// <param name="ControlObj"></param>
        /// <returns></returns>
        private Control GetNodeContainer(object ControlObj)
        {
            Control ContainerControl = null;
            #region 获取节点容器
            if (ControlObj == null) return null;
            if (!(ControlObj is Control)) return null;
            MindMapNodeContentBase ContainerTemp = ((Control)ControlObj).GetNodeContent();
            if (ContainerTemp == null) return null;
            ContainerControl = ContainerTemp.NodeContainer;
            #endregion 获取节点容器
            return ContainerControl;
        }

        /// <summary>开始拖拽节点
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MindMapNode_DragOver(object sender, DragEventArgs e)
        {
            if (MindMapNodeDragOver != null) MindMapNodeDragOver(GetNodeContainer(sender), e);
        }

        /// <summary> 拖拽到某节点
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MindMapNode_DragEnter(object sender, DragEventArgs e)
        {
            DragEventHandler RunEvent = MindMapNodeDragEnter;
            if (IsElseNode(e.Data))//拖拽操作是从其他节点发起的？
            {
                MindMapNodeContentBase Content = ((Control)sender).GetNodeContent();
                if (AllowDrag(Content.NodeContainer)) //当前节点不能拖动到选中节点和它的子节点下
                {
                    e.Effect = e.AllowedEffect;
                    RunEvent = MindeMapNodeToNodeDragEnter;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            if (RunEvent != null) RunEvent(GetNodeContainer(sender), e);

        }

        /// <summary> 在某节点完成拖拽
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MindMapNode_DragDrop(object sender, DragEventArgs e)
        {
            DragEventHandler RunEvent = MindMapNodeDragDrop;
            if (IsElseNode(e.Data))//拖拽操作是从其他节点发起的？
            {
                MindMapNodeContentBase Content = ((Control)sender).GetNodeContent();
                if (AllowDrag(Content.NodeContainer)) //当前节点不能拖动到选中节点和它的子节点下
                {
                    RunEvent = MindeMapNodeToNodeDragDrop;
                }
            }
            if (RunEvent != null) RunEvent(GetNodeContainer(sender), e);
        }

        /// <summary>节点被按下时
        /// 
        /// </summary>
        private void mindMapNode_MindMapNodeMouseDown(object sender, MouseEventArgs e)
        {
           
            
                MindMapNodeContentBase SenderObject = ((Control)sender).GetNodeContent();
                List<MindMapNodeContainer> MindMapNodeList = mindMapNode.GetChidrenNode(true);
                MindMapNodeList.Add(mindMapNode);
                if (Control.ModifierKeys != Keys.Control || e.Button == MouseButtons.Right)//不按住ctrl就单选
                {
                    if (!SenderObject.Selected)//如果单击已选中节点就不做任何事，因为可能需要按住进行拖拽
                    {
                        MindMapNodeList.ForEach(T1 => T1.NodeContent.Selected = false);
                        SenderObject.Selected = true;
                    }
                }
                else//按住ctrl可单选
                {
                    SenderObject.Selected = !SenderObject.Selected;
                }
            
            MindMap_Panel_MouseDown(sender, e);
            if (MindMapNodeMouseDown != null) MindMapNodeMouseDown(GetNodeContainer(sender), e);
        }

        /// <summary>节点在鼠标弹起时
        /// 
        /// </summary>
        private void mindMapNode_MindMapNodeMouseUp(object sender, MouseEventArgs e)
        {
            MindMap_Panel_MouseUp(sender, e);
            Control  ContainerControl=GetNodeContainer(sender); 
            if (MindMapNodeMouseUp != null) MindMapNodeMouseUp(ContainerControl, e);
            //鼠标在按下后和弹起前鼠标没有进行过拖拽操作则认为这是一次单击操作
            if (MindMapNodeMouseClick != null && !IsDrag) MindMapNodeMouseClick(ContainerControl, e);

        }

        /// <summary>鼠标在节点移动时
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_MindMapNodeMouseMove(object sender, MouseEventArgs e)
        {
          
                MindMap_Panel_MouseMove(sender, e);
            if (MindMapNodeMouseMove != null) MindMapNodeMouseMove(GetNodeContainer(sender), e);
        }

        /// <summary>鼠标进入节点范围时
        /// 
        /// </summary>
        private void mindMapNode_MindMapNodeMouseEnter(object sender, EventArgs e)
        {
            if (MindMapNodeMouseEnter != null) MindMapNodeMouseEnter(GetNodeContainer(sender), e);
        }

        /// <summary>鼠标移出节点范围事件
        /// 
        /// </summary>
        private void mindMapNode_MindMapNodeMouseLeave(object sender, EventArgs e)
        {
                if (MindMapNodeMouseLeave != null) MindMapNodeMouseLeave(GetNodeContainer(sender), e);
        }

        /// <summary> 双击某节点后编辑某节点
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (MindMapNodeMouseDoubleClick != null) MindMapNodeMouseDoubleClick(GetNodeContainer(sender), e);

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
        private void mindMapNode_EmptyRangeMouseClick(object sender, MouseEventArgs e)
        {

            if (EmptyRangeMouseClick != null) EmptyRangeMouseClick(sender, e);

        }

        /// <summary> 空白处鼠标按下
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMapNode_EmptyRangeMouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            //if (e.Button == MouseButtons.Left)
            {
                #region 取消所有编辑状态
                List<MindMapNodeContainer> MindMapNodeList = mindMapNode.GetChidrenNode(true);
                MindMapNodeList.Add(mindMapNode);
                //foreach (MindMapNodeContainer ContainerItem in MindMapNodeList)
                //{
                //    if (ContainerItem.NodeContent.Edited)
                //    {
                //        ContainerItem.NodeContent.Edited = false;
                //        break;
                //    }
                //}
                #endregion 取消所有编辑状态

                if (Control.ModifierKeys != Keys.Control)//不按住ctrl就直接取消所有选中
                {
                    MindMapNodeList.ForEach(T1 => T1.NodeContent.Selected = false);
                }


            }
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

        [Browsable(true), Description("节点开始拖拽")]
        public event DragEventHandler MindMapNodeDragOver;

        [Browsable(true), Description("拖拽到某节点边缘时")]
        public event DragEventHandler MindMapNodeDragEnter;

        [Browsable(true), Description("某节点完成拖拽操作")]
        public event DragEventHandler MindMapNodeDragDrop;

        [Browsable(true), Description("从某节点拖动到另一个节点边界时触发")]
        public event DragEventHandler MindeMapNodeToNodeDragEnter;

        [Browsable(true), Description("从某节点拖动到另一个节点完成时触发")]
        public event DragEventHandler MindeMapNodeToNodeDragDrop;


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
        public event MouseEventHandler EmptyRangeMouseClick;

        #endregion 节点容器相关事件委托[在非节点处发生的事件]


        /// <summary> 焦点在思维导图任何位置时，键盘按下事件
        /// 
        /// </summary>
        public event KeyEventHandler MindNodemapKeyDown;

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

        #endregion 公开事件委托    

        #region 鼠标中键拖动滚动条


        /// <summary> 左键拖动前就已经选中的节点
        /// 
        /// </summary>
       private List<MindMapNodeContainer> SelectedNodeList = new List<MindMapNodeContainer>();


        /// <summary> 鼠标在按下后和弹起前鼠标是否进行过拖拽操作
        /// 
        /// </summary>
        private bool IsDrag = false;

        /// <summary> 是否在节点中被按住
        /// 
        /// </summary>
        private bool IsMindMapNode = false;

        /// <summary> 鼠标拖动前的位置
        /// 
        /// </summary>
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
                MoveValue = e.Location;
            }
            else if (e.Button == MouseButtons.Left)
            {
                MindMapNodeContent.MindMapNodeContentBase ContentControl = ((Control)sender).GetNodeContent();
                if (ContentControl == null)
                {
                    IsMindMapNode = false;
                    #region 空白处按住左键，即将进行矩形选择
                    if (Control.ModifierKeys == Keys.Control)//按住ctrl就记录已选中节点
                    {
                        SelectedNodeList = GetSelectedNode();
                    }
                    else
                    {
                        SelectedNodeList.Clear();//虽然不按住Control在调用本方法之前所有节点就已经取消选中了，但为了性能还是不要调用GetSelectedNode方法了吧
                    }
                    ShowOrHideLine(true);//显示矩形的线条 
                    #endregion 空白处按住左键，即将进行矩形选择
                }
                else
                {
                    // 在节点中按住左键,即将进行拖拽操作
                    IsMindMapNode = true;          
                }
                MoveValue = Scroll_panel.PointToClient(Control.MousePosition);
            
            }
            IsDrag = false;
        }

        /// <summary> 弹起中键结束拖动滚动条
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMap_Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowOrHideLine(false);//隐藏矩形的线条
            }
            IsMindMapNode = false;

        }

        /// <summary>按住鼠标中间可拖动滚动条
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MindMap_Panel_MouseMove(object sender, MouseEventArgs e)
        {
        
                #region 计算矩形的尺寸
                Point PointTemp1 = Scroll_panel.PointToClient(Control.MousePosition);
                Point PanelLocation = new Point(0, 0);
                Size PanelSize = new Size();
                if (MoveValue.X < PointTemp1.X)
                {
                    PanelLocation.X = MoveValue.X;
                    PanelSize.Width = PointTemp1.X - MoveValue.X;
                }
                else
                {
                    PanelLocation.X = PointTemp1.X;
                    PanelSize.Width = MoveValue.X - PointTemp1.X;
                }

                if (MoveValue.Y < PointTemp1.Y)
                {
                    PanelLocation.Y = MoveValue.Y;
                    PanelSize.Height = PointTemp1.Y - MoveValue.Y;
                }
                else
                {
                    PanelLocation.Y = PointTemp1.Y;
                    PanelSize.Height = MoveValue.Y - PointTemp1.Y;
                }
                #endregion 计算矩形的尺寸
                Rectangle CurrentRectangle = new Rectangle(PanelLocation, PanelSize);//计算出当前在控件中左键画出的矩形
                if (CurrentRectangle.Width < 3 || CurrentRectangle.Height < 3) return;//必须拖动一定距离才触发，否则单击也会触发                

                IsDrag = true;

                if (Control.MouseButtons == MouseButtons.Right)
                {
                    #region 右键拖动思维导图
                    Point PointTemp = new Point();
                    PointTemp.X = MoveValue.X - e.Location.X;
                    PointTemp.Y = MoveValue.Y - e.Location.Y;
                    Point ResultPoint = new Point(Main_Panel.HorizontalScroll.Value + PointTemp.X, Main_Panel.VerticalScroll.Value + PointTemp.Y);
                    Main_Panel.AutoScrollPosition = ResultPoint;
                    RecordScrollPosition();
                    #endregion 右键拖动思维导图
                }
                else if (Control.MouseButtons == MouseButtons.Left)
                {

                    if (IsMindMapNode) //是否在节点中进行左键拖拽
                {
                    //是：拖拽节点
                    MindMapNodeContent.MindMapNodeContentBase ContentControl = ((Control)sender).GetNodeContent();
                    if (ContentControl == null) return;
                    ContentControl.DoDragDrop(ContentControl, DragDropEffects.Move);
                }
                else
                {
                    //不是：就拖拽出矩形框框
                    #region 画出矩形
                    Selected_Top_panel.Location = PanelLocation;
                    Selected_Top_panel.Width = PanelSize.Width;

                    Selected_Left_panel.Location = PanelLocation;
                    Selected_Left_panel.Height = PanelSize.Height;

                    Selected_Right_panel.Location = new Point(PanelLocation.X + PanelSize.Width, PanelLocation.Y);
                    Selected_Right_panel.Height = PanelSize.Height;

                    Selected_Bottom_panel.Location = new Point(PanelLocation.X, PanelLocation.Y + PanelSize.Height);
                    Selected_Bottom_panel.Width = PanelSize.Width;


                    #endregion 画出矩形

                    #region 选中框住的节点
                    //获取所有节点实例
                    List<MindMapNodeContainer> MindMapNodeContainerTemp = mindMapNode.GetChidrenNode(true);
                    MindMapNodeContainerTemp.Add(mindMapNode);
                    foreach (MindMapNodeContainer MindMapNodeContaineritem in MindMapNodeContainerTemp)//遍历所有节点实例
                    {
                        #region 获取当前节点的坐标和尺寸信息[矩形信息]
                        Point ContentPoint = MindMapNodeContaineritem.NodeContent.PointToScreen(new Point());//获取节点屏幕坐标
                        ContentPoint = Scroll_panel.PointToClient(ContentPoint);//获取节点在控件中的位置
                        Rectangle RectangleTemp = new Rectangle(ContentPoint, MindMapNodeContaineritem.NodeContent.Size);//获取节点的位置和尺寸信息 
                        #endregion 获取当前节点的坐标和尺寸信息[矩形信息]

                        #region 被框住就选中或反选节点
                        if (CurrentRectangle.IntersectsWith(RectangleTemp))//左键拖出的矩形是否与当前实例有交集
                        {
                            if (SelectedNodeList.Contains(MindMapNodeContaineritem))//如果已经选中了就取消选中
                            {
                                MindMapNodeContaineritem.NodeContent.Selected = false;
                            }
                            else
                            {
                                MindMapNodeContaineritem.NodeContent.Selected = true;
                            }
                        }
                        else
                        {
                            if (SelectedNodeList.Contains(MindMapNodeContaineritem))
                                continue;//在拖动之前就已经选中了就不管
                            MindMapNodeContaineritem.NodeContent.Selected = false;
                        }
                        #endregion 被框住就选中或反选节点
                    }
                    #endregion 选中框住的节点

                }
                }


            
        }

        /// <summary> 显示或隐藏左键拖动的矩形线条
        /// 
        /// </summary>
        /// <param name="VisibleParame">[true:显示；false:隐藏]</param>
        private void ShowOrHideLine(bool VisibleParame)
        {

            if (VisibleParame)
            {
                //显示
                Selected_Top_panel.Visible = true;
                Selected_Left_panel.Visible = true;
                Selected_Right_panel.Visible = true;
                Selected_Bottom_panel.Visible = true;
            }
            else
            {
                #region 隐藏矩形线条
                Selected_Top_panel.Visible = false;
                Selected_Left_panel.Visible = false;
                Selected_Right_panel.Visible = false;
                Selected_Bottom_panel.Visible = false;

                Selected_Top_panel.Location = new Point();
                Selected_Left_panel.Location = new Point();
                Selected_Right_panel.Location = new Point();
                Selected_Bottom_panel.Location = new Point();

                Selected_Top_panel.Width = 0;
                Selected_Left_panel.Height = 0;
                Selected_Right_panel.Height = 0;
                Selected_Bottom_panel.Width = 0;
                #endregion 隐藏矩形线条
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
            //所以当本控件尺寸发生改变时延迟50毫秒设置滚动条尺寸        
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

        PointF CurrentProportion = new PointF();//当前滚动条比例
        /// <summary> 记录当前滚动条比例 [配合SetScroll方法可以按比例滚动]
        /// 
        /// </summary>
        private void RecordScrollPosition()
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
            RecordScrollPosition();
        }

        /// <summary> 设置滚动条比例
        /// [ 调用前请使用RecordScalingPosition记录比例]
        /// </summary>
        private void SetScroll()
        {

            int LeftTemp = (int)(Scroll_panel.Width * CurrentProportion.X);
            int TopTemp = (int)(Scroll_panel.Height * CurrentProportion.Y);
            LeftTemp = LeftTemp - (Main_Panel.Width / 2);
            TopTemp = TopTemp - (Main_Panel.Height / 2);
            Main_Panel.AutoScrollPosition = new Point(LeftTemp, TopTemp);
        }

        /// <summary> 移动滚动条时记录滚动比例
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Panel_Scroll(object sender, ScrollEventArgs e)
        {
            RecordScrollPosition();
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
            PaintNum = 50;//倒计时300毫秒
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
                        this.CurrentScaling = this._CurrentScaling;                        
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

                        float ChangeValue = 0.2f;//默认每次缩放20%
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
            
            CurrentScaling = 1;//将比例设置为100%
          
        }

        #endregion 按住Ctrl+滚轮缩放
        
        #region 用于跳过绘制过程

        private int CountDown = 50;//倒计时50毫秒
        private Thread DelayThread = null;//用于倒计时的线程
        /// <summary> 临时隐藏思维导图节点并在50毫秒后显示出来
        /// 如果高平率反复调用本方法则会重置倒计时时间，以保证最后一次临时隐藏到显示间隔50毫秒
        /// 主要是由于winform的缺陷在进行大面积变动时（用户在循环中添加节点或删除节点）时。如果不隐藏思维导图就会展示添加/删除节点的过程（绘制过程）
        /// 每当有节点添加或删除时或尺寸改变时（改变数据源）都会调用本方法用于跳过绘制过程
        /// </summary>
        private void HideMindMap()
        {

            CountDown = 50;//重置倒计时时间
            mindMapNode.Visible = false;//隐藏思维导图节点
            if (DelayThread != null) return;//如果之前就已经开始倒计时了就直接返回。
            DelayThread = new Thread(() =>
            {
                while (true)
                {
                    CountDown = CountDown - 10;
                    if (CountDown <= 0) break;
                    Thread.Sleep(10);
                }
                DelayThread = null;
                this.Invoke(new Action(() =>
                {
                    mindMapNode.Visible = true;
                }));
            });
            DelayThread.Start();

        }

        #endregion 用于跳过绘制过程



    }
}