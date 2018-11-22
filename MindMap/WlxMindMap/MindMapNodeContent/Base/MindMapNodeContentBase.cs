using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WlxMindMap.MindMapNode;

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

        /// <summary> 刷新节点内容的尺寸
        /// 
        /// </summary>
        public abstract void RefreshContentSize();
    }
}
