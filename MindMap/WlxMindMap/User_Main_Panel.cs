using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WlxMindMap
{
    class User_Main_Panel:Panel
    {
        /// <summary> 禁止每当控件获得焦点后横向滚动条总会自动滚动到根节点
        /// 
        /// </summary>
        /// <param name="activeControl"></param>
        /// <returns></returns>
        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }
    }
}
