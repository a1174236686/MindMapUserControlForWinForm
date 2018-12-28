using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WlxMindMap.NodeContent
{
    /// <summary> 节点内容的基类
    /// 如果你要自定义节点内容的布局等你就必须实现这些抽象方法
    /// ------------------------------------------------------------
    /// 需要注意的是：
    /// 当你继承该类后，你的窗体设计器是无法使用的。
    /// 这时候如果你想使用窗体设计器你可以临时将父类改为UserControl使用完毕后再切换回本类即可
    /// </summary>
    
    public abstract class MindMapNodeContentBase : UserControl
    {
      
        #region 子类必须实现的抽象方法
        /// <summary> 指示DataItem的结构
        /// 
        /// </summary>
        public abstract MindMapNodeStructBase DataStruct { get; set; }

        /// <summary> 获取或设置节点是否选中
        /// 
        /// </summary>
        public abstract bool Selected { get; set; }
             
        
        /// <summary> 表示用于显示内容的数据源
        /// 
        /// </summary>
        public abstract object DataItem { get; set; }

        /// <summary> 获取或设置当前内容的缩放比例
        /// 
        /// </summary>
        public abstract float CurrentScaling { get; set; }

        /// <summary> 刷新节点内容的尺寸
        /// 
        /// </summary>
        public abstract void RefreshContentSize();

      
        #endregion 子类必须实现的抽象方法

        #region 基类提供的方法     
     
        /// <summary> 获取会触发节点内容事件的控件，
        /// 例如节点的拖动事件，鼠标按下，鼠标弹起，鼠标移入，鼠标移出等
        /// 如果你不希望某个控件会触发事件你可以使用override重载这个方法并在返回结果中排除那个控件
        /// </summary>
        /// <returns></returns>
        public List<Control> GetEventControl()
        {            
            return this.GetAllControl();
        }


        #endregion 基类提供的方法

        #region 属性

        /// <summary>ParentMindMapNode 属性的锁，防止死递归，确保Set属性只会被访问一次
        /// 
        /// </summary>
        private bool ParentMindMapNodeLock = false;
        private MindMapNodeContainer _NodeContainer;
        /// <summary> 获取或设置节点容器
        /// 
        /// </summary>
        public MindMapNodeContainer NodeContainer
        {
            get { return _NodeContainer; }
            set
            {
                #region 为什么要有锁？
                /*
                    * 锁的概念是为了能够在设置本属性的同时联动将原父容器的节点内容设置为空
                    * 类似于Control类的Parent属性,使得节点容器和节点内容只需要修改其中一个实例另一个实例就会自动变
                    * 但是父节点容器也有相同的特性也会联动将原节点内容设置为空，所以会造成死递归                 
                    */
                #endregion 为什么要有锁？
                if (ParentMindMapNodeLock) return;//被锁住就直接返回
                ParentMindMapNodeLock = true;//打开锁防止死递归
                if (_NodeContainer == value) return;//相同属性就直接返回
                if (_NodeContainer != null) _NodeContainer.SetNodeContent(DataStruct, null);//把原来的置为null
                _NodeContainer = value;
                if (_NodeContainer != null)
                {
                    _NodeContainer.SetNodeContent(DataStruct, this);
                }
                ParentMindMapNodeLock = false;//关闭锁
            }
        }

        /// <summary>获取DataItem下的指定属性的值[如果找不到返回空字符串]
        /// 
        /// </summary>
        /// <param name="Propertyname"></param>
        /// <returns></returns>
        protected object GetDataValue(string Propertyname)
        {
            object ResultObj = null;
            try
            {
                ResultObj = DataItem.GetType().GetProperty(Propertyname).GetValue(DataItem);
            }
            catch
            {
                ResultObj = "";//如果无法找到该属性就填空字符串
            }
           return ResultObj;
        }
        
        #endregion 属性


    }
}
