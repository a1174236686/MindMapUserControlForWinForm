using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WlxMindMap.MindMapNode
{
    /// <summary> 用于生成思维导图的结构的基类
    /// 只提供[指明传入的哪个属性是ID，哪个属性是父ID]
    /// </summary>
    public class MindMapNodeStructBase
    {
        /// <summary> DateSource中ID的属性名称
        /// 一般用于存ID值
        /// </summary>
        public string MindMapID { get; set; }
     
        /// <summary> 父ID的属性名称
        /// </summary>
        public string MindMapParentID { get; set; }
    }
}
