using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WlxMindMap
{
    class WindowsAPI
    {
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);
    }
}
