using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WlxMindMap.MindMapNodeContent
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

        /// <summary> 获取或设置节点是否处于编辑状态
        /// 
        /// </summary>
        public abstract bool Edited { get; set; }
        
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
        /// <summary>获取该节点内容的所有控件（含自己）
        /// 
        /// </summary>
        /// <param name="ControlParame"></param>
        /// <returns></returns>
        public List<Control> GetNodeControl(Control ControlParame = null)
        {
            List<Control> ResultList = new List<Control>();
            if (ControlParame == null)
            {
                ControlParame = this;
                ResultList.Add(this);
            }

            foreach (Control Item in ControlParame.Controls)
            {
                ResultList.Add(Item);
                ResultList.AddRange(GetNodeControl(Item));//递归取子控件
            }
            return ResultList;
        }
        #endregion 基类提供的方法

        #region 属性
        private MindMapNodeContainer _ParentMindMapNode;
        /// <summary> 获取或设置节点容器
        /// 
        /// </summary>
        public MindMapNodeContainer ParentMindMapNode
        {
            get { return _ParentMindMapNode; }
            set
            {
                if (_ParentMindMapNode == value) return;
                _ParentMindMapNode = value;
                if (_ParentMindMapNode != null)
                {
                    _ParentMindMapNode.SetNodeContent(DataStruct, this);
                }
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
