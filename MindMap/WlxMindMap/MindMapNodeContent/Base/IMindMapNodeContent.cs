using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WlxMindMap.MindMapNode;

namespace WlxMindMap.MindMapNodeContent
{
    public interface IMindMapNodeContent
    {
        /// <summary> 指示DataItem的结构
        /// 
        /// </summary>
        MindMapNodeStructBase DataStruct { get; set; }

        /// <summary> 表示用于显示内容的数据源
        /// 
        /// </summary>
        object DataItem { get; set; }
        
        /// <summary> 设置节点内容的尺寸
        /// 
        /// </summary>
        void ReSetContentSize();

    }
}
