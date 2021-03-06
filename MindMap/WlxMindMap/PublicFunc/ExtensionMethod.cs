﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WlxMindMap.NodeContent;

namespace WlxMindMap
{
    public static class ExtensionMethod
    {
        /// <summary> 通过节点内容里的某个控件，递归向上获取节点内容控件
        /// 
        /// </summary>
        /// <param name="thisControl"></param>
        /// <returns></returns>
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
                if(thisControl is MindMap_Panel) return result;//如果找到思维导图容器了还没有找到那这个控件就不是节点内容里的控件
                if (thisControl.Parent == null) return result;
                return thisControl.Parent.GetNodeContent();
            }
        }
        
        /// <summary>
        /// 清除事件绑定的函数
        /// </summary>
        /// <param name="objectHasEvents">拥有事件的实例</param>
        /// <param name="eventName">事件名称</param>
        public static void ClearAllEvents(this Control objectHasEvents, string eventName)
        {

            if (objectHasEvents == null)
            {
                return;
            }
            try
            {
                EventInfo[] events = objectHasEvents.GetType().GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (events == null || events.Length < 1)
                {
                    return;
                }
                for (int i = 0; i < events.Length; i++) 
                {
                    EventInfo ei = events[i];
                    if (ei.Name == eventName)
                    {
                        FieldInfo fi = ei.DeclaringType.GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (fi != null)
                        {
                            fi.SetValue(objectHasEvents, null);
                        }
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        public static void Center(this Control ThisControl)
        {
            if (ThisControl == null) return;
            if (ThisControl.Parent == null) return;
            Control ParentControl = ThisControl.Parent;
        }      
        
        

        /// <summary> 获取某控件下的所有子控件
        /// 
        /// </summary>
        /// <param name="ControlParame"></param>
        /// <returns></returns>
        public static List<Control> GetAllControl(this Control ControlParame)
        {
            List<Control> ResultList = new List<Control>();

            foreach (Control Item in ControlParame.Controls)
            {
                ResultList.Add(Item);
                ResultList.AddRange(Item.GetAllControl());//递归取子控件
            }
            return ResultList;
        }


        #region 缩放相关

        /// <summary> 按比例进行缩放，返回缩放后的实例
        /// 
        /// </summary>
        /// <param name="ThisFont"></param>
        /// <param name="Scaling">缩放比例[100%=1]</param>
        /// <returns>返回缩放后的实例</returns>
        public static Font ByScaling(this Font ThisFont, float Scaling)
        {
            Font Result = null;
            float FontSize = 1;
            if (Scaling <= 0)
            {
                FontSize = 1;
            }
            else
            {
                FontSize = ThisFont.SizeInPoints;
                FontSize = FontSize * Scaling;
            }

            Result = new Font(ThisFont.FontFamily, FontSize, ThisFont.Style, ThisFont.Unit, ThisFont.GdiCharSet, ThisFont.GdiVerticalFont);
            return Result;
        }

        /// <summary> 按比例进行缩放，返回缩放后的实例
        /// 
        /// </summary>
        /// <param name="ThisPadding"></param>
        /// <param name="Scaling">缩放比例[100%=1]</param>
        /// <returns>返回缩放后的实例</returns>
        public static Padding ByScaling(this Padding ThisPadding, float Scaling)
        {
            if (Scaling <= 0)
            {
                return new Padding(0, 0, 0, 0);
            }
            else
            {
                int _Left = (int)(ThisPadding.Left * Scaling);
                int _Right = (int)(ThisPadding.Right * Scaling);
                int _Top = (int)(ThisPadding.Top * Scaling);
                int _Bottom = (int)(ThisPadding.Bottom * Scaling);
                return new Padding(_Left, _Top, _Right, _Bottom);
            }
        }

        /// <summary> 按比例进行缩放，返回缩放后的实例
        /// 
        /// </summary>
        /// <param name="ThisSize"></param>
        /// <param name="Scaling">缩放比例[100%=1]</param>
        /// <returns>返回缩放后的实例</returns>
        public static Size ByScaling(this Size ThisSize, float Scaling)
        {
            if (Scaling <= 0)
            {
                return new Size(0, 0);
            }
            else
            {
                int _Width = (int)(ThisSize.Width * Scaling);
                int _Height = (int)(ThisSize.Height* Scaling);

                return new Size(_Width, _Height);
            }
        }
        #endregion 缩放相关
    }
}
