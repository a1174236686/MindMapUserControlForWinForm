using System;
using System.Drawing;
using System.Windows.Forms;


namespace WlxMindMap.MindMapNodeContent
{
    public partial class Text_MindMapNodeContent :
    MindMapNodeContentBase
    //UserControl
    {
        public Text_MindMapNodeContent()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            Content_lable.ForeColor = Color.FromArgb(255, 255, 255);
            Content_lable.BackColor = NodeBackColor.Normaly.Value;            
            RecordScling();         
        }

        public string ContentText { get { return Content_lable.Text; } }

        #region 缩放相关

        /// <summary>记录当前尺寸为100%时的尺寸，缩放时将会基类该值进行调整
        /// 
        /// </summary>
        private void RecordScling()
        {
            Scaling_ContentFont = Content_lable.Font;
            Scaling_ContentPadding = Content_lable.Padding;
        }

        /// <summary> 缩放比例为100%时内容字体的大小
        /// 
        /// </summary>
        private Font Scaling_ContentFont;
        /// <summary> 缩放比例为100%时内容的边距
        /// 
        /// </summary>
        private Padding Scaling_ContentPadding;

        #endregion 缩放相关

        #region 实现基类的抽象方法
        private bool _Selected = false;
        /// <summary> 获取或设置节点是否选中
        /// 
        /// </summary>
        public override bool Selected
        {
            get => _Selected; set
            {
                _Selected = value;
                if (_Selected)
                {
                    Content_lable.BackColor = NodeBackColor.Down.Value;
                }
                else
                {
                    Content_lable.BackColor = NodeBackColor.Normaly.Value;
                }
            }
        }

        //private bool _Edited = false;
        ///// <summary>  获取或设置节点是否处于编辑状态
        ///// 
        ///// </summary>
        //public override bool Edited
        //{
        //    get => _Edited;
        //    set
        //    {
        //        if (value == _Edited) return;
        //        if (value)
        //        {

        //            Edit_TextBox.Visible = true;

        //            Edit_TextBox.Size = Content_lable.Size;
        //            Edit_TextBox.Location = Content_lable.Location;
        //            Edit_TextBox.Font = Content_lable.Font;
        //            Edit_TextBox.Text = Content_lable.Text;
        //            Edit_TextBox.BringToFront();
        //            Edit_TextBox.Focus();
        //            Content_lable.Visible = false;
        //        }
        //        else
        //        {
        //            Edit_TextBox.Visible = false;
        //            Content_lable.Visible = true;
        //            ParentMindMapNode.ResetNodeSize();
        //        }
        //        _Edited = value;
        //    }
        //}

        private object _DataItem;
        /// <summary> 获取或设置用于显示内容的数据源
        /// 
        /// </summary>
        public override object DataItem
        {
            get
            {
                return _DataItem;
            }
            set
            {
                
                _DataItem = value;

                Content_lable.Text = GetDataValue(g_DataStruct.Text).ToString();


                if (ParentMindMapNode != null) ParentMindMapNode.ResetNodeSize();
            }
        }

        private float _CurrentScaling = 1;
        /// <summary> 获取或设置当前内容的缩放比例
        /// 
        /// </summary>
        public override float CurrentScaling
        {
            get
            {
                return _CurrentScaling;
            }
            set
            {
                _CurrentScaling = value;
                //this.RefreshContentSize();
            }
        }

        private Text_ContentStruct g_DataStruct = new Text_ContentStruct();
        private MindMapNodeStructBase _DataStruct = new MindMapNodeStructBase();
        /// <summary> 获取或设置指示DataItem的结构
        /// 
        /// </summary>
        public override MindMapNodeStructBase DataStruct
        {
            get
            {
                return _DataStruct;
            }
            set
            {
                _DataStruct = value;
                if (!(_DataStruct is Text_ContentStruct)) throw new Exception("指示内容结构的类必须为Text_ContentStruct");
                g_DataStruct = (Text_ContentStruct)_DataStruct;

            }
        }
        
        /// <summary> 刷新节点内容的尺寸
        /// 
        /// </summary>
        public override void RefreshContentSize()
        {
            Content_lable.Font = Scaling_ContentFont.ByScaling(_CurrentScaling);
            Content_lable.Padding = Scaling_ContentPadding.ByScaling(_CurrentScaling);
            this.Width = Content_lable.Width;
            this.Height = Content_lable.Height;
        }
        #endregion 实现基类的抽象方法

        #region 鼠标移入移出的动画效果
        private void Content_lable_MouseEnter(object sender, EventArgs e)
        {
            Color ResultColor = _NodeBackColor.Enter.Value;
            if (_Selected)
            {
                ResultColor = _NodeBackColor.Down.Value;
            }
            Content_lable.BackColor = ResultColor;
        }

        private void Content_lable_MouseLeave(object sender, EventArgs e)
        {
            Color ResultColor = _NodeBackColor.Normaly.Value;
            if (_Selected)
            {
                ResultColor = _NodeBackColor.Down.Value;
            }
            Content_lable.BackColor = ResultColor;
        }

        private void Content_lable_MouseDown(object sender, MouseEventArgs e)
        {        
            Content_lable.BackColor = _NodeBackColor.Down.Value;           
        }

        private void Content_lable_MouseUp(object sender, MouseEventArgs e)
        {
            Color ResultColor = _NodeBackColor.Normaly.Value;
            if (_Selected)
            {
                ResultColor = _NodeBackColor.Down.Value;
            }
            Content_lable.BackColor = ResultColor;
        }
        #endregion 鼠标移入移出的动画效果

        /// <summary> 设置当前节点的背景颜色
        /// 
        /// </summary>
        public MindMapNodeBackColor NodeBackColor
        {
            get { return _NodeBackColor; }
            set
            {
                if (value == null) return;
                Content_lable.BackColor = _NodeBackColor.Normaly.Value;
                _NodeBackColor = value;
            }
        }
        private MindMapNodeBackColor _NodeBackColor = new MindMapNodeBackColor(Color.FromArgb(48, 120, 215));

        /// <summary> 用于编辑的TextBox按下回车完成编辑，按下esc取消编辑        
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyData)
            //{
            //    case Keys.Enter:
            //        Content_lable.Text = Edit_TextBox.Text;
            //        RefreshContentSize();
            //        Edited = false;
            //        break;

            //    case Keys.Escape:
            //        Edited = false;
            //        break;
            //}
            //e.Handled = true;
        }

        #region 配套使用的内部类
        /// <summary> Text_MindMapNodeContent中指示DataItem的结构
        /// 
        /// </summary>
        public class Text_ContentStruct : MindMapNodeStructBase
        {
            /// <summary> DataItem中用于显示文本的属性名
            /// 
            /// </summary>
            public string Text { get; set; }
        }

        #region 用于指明节点的背景色
        /// <summary> 用于指明节点的背景色
        /// 
        /// </summary>
        public class MindMapNodeBackColor
        {
            /// <summary> 必须设置节点在正常时的背景颜色，如果其他颜色为空则，其他色在取值时会基于正常色自动给出缺省颜色
            /// 
            /// </summary>
            /// <param name="ColorParame"></param>
            public MindMapNodeBackColor(Color NormalyColor)
            {
                _Normaly = NormalyColor;
            }

            /// <summary>节点在正常时的背景颜色
            /// 
            /// </summary>
            public Color? Normaly { get { return _Normaly; } set { _Normaly = value; } }
            private Color? _Normaly = null;

            /// <summary> 节点在鼠标进入时的背景颜色 [如果为空取值时将取到比正常色稍亮一些的颜色]
            /// 
            /// </summary>
            public Color? Enter
            {
                get
                {
                    if (_Enter == null)
                    {
                        int IntRed = _Normaly.Value.R + 30;
                        int IntGreen = _Normaly.Value.G + 30;
                        int IntBlue = _Normaly.Value.B + 30;
                        IntRed = GetColorValue(IntRed);
                        IntGreen = GetColorValue(IntGreen);
                        IntBlue = GetColorValue(IntBlue);
                        _Enter = Color.FromArgb(IntRed, IntGreen, IntBlue);
                    }
                    return _Enter;
                }
                set { _Enter = value; }
            }
            private Color? _Enter = null;

            /// <summary> 节点在鼠标按下时的背景颜色 [如果为空取值时将取到比正常色稍暗一些的颜色]
            /// 
            /// </summary>
            public Color? Down
            {
                get
                {
                    if (_Down == null)
                    {
                        int IntRed = _Normaly.Value.R - 50;
                        int IntGreen = _Normaly.Value.G - 50;
                        int IntBlue = _Normaly.Value.B - 50;
                        IntRed = GetColorValue(IntRed);
                        IntGreen = GetColorValue(IntGreen);
                        IntBlue = GetColorValue(IntBlue);
                        _Down = Color.FromArgb(IntRed, IntGreen, IntBlue);
                    }
                    return _Down;


                }
                set { _Down = value; }
            }
            private Color? _Down = null;

            /// <summary> 限制int不能超过0-255的范围，超过最小值则取最小值超过最大值则取最大值
            /// 
            /// </summary>
            /// <param name="ColorValue"></param>
            /// <returns></returns>
            private int GetColorValue(int ColorValue)
            {
                int IntResult = ColorValue;
                IntResult = IntResult > 255 ? 255 : IntResult;//不能大于255，如果大于就取255
                IntResult = IntResult < 0 ? 0 : IntResult;//不能小于0，如果小于0那就取0
                return IntResult;
            }
        }






        #endregion 用于指明节点的背景色

        #endregion 配套使用的内部类

     
    }
}
