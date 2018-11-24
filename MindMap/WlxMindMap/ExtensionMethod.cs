using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WlxMindMap.MindMapNodeContent;

namespace WlxMindMap
{
    public static class ExtensionMethod
    {
        public static MindMapNodeContentBase GetNodeContent(this Control thisControl)
        {
            MindMapNodeContentBase result = null;
            if (thisControl is MindMapNodeContentBase)
            {
                result = (MindMapNodeContentBase)thisControl;
                return result;
            }
            else
            {
                if (thisControl.Parent == null) return result;
                return thisControl.Parent.GetNodeContent();
            }
        }
    }
}
