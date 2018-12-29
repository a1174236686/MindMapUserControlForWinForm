using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WlxMindMap.NodeContent;
using WlxMindMap;
using System.IO;
using System.Runtime.InteropServices;

namespace MindMap
{
    //public partial class File_NodeContent : UserControl
    public partial class File_NodeContent : MindMapNodeContentBase
    {
        public File_NodeContent()
        {
            InitializeComponent();
            Title_Label.ForeColor = Color.FromName("white");
            Title_Label.BackColor = _FolderNameBackColor.Normaly.Value;
            File_Panel.BackColor = _FilePanelBackColor.Normaly.Value;

            RecordScaling();
        }



        /// <summary> 记录100%缩放比例时的尺寸
        /// 
        /// </summary>
        public void RecordScaling()
        {
            FolderTitle_Font = Title_Label.Font;
            FolderTitle_Padding = Title_Label.Padding;

        }
        private Font FolderTitle_Font = null;
        private Padding FolderTitle_Padding = new Padding ();



        /// <summary> 指示DataItem的结构
        /// 
        /// </summary>
        public override MindMapNodeStructBase DataStruct
        {
            get { return _DataStruct; }
            set
            {
                _DataStruct = value;
                if (!(_DataStruct is File_ContentStruct)) throw new Exception("指示内容结构的类必须为File_ContentStruct");
                g_DataStruct = (File_ContentStruct)_DataStruct;
            }
        }
        private MindMapNodeStructBase _DataStruct { get; set; }
        private File_ContentStruct g_DataStruct { get; set; }


        /// <summary> 获取或设置节点是否选中
        /// 
        /// </summary>
        public override bool Selected
        {
            get { return _Selected; }
            set
            {
                _Selected = value;
                if (_Selected)
                {
                    Title_Label.BackColor = _FolderNameBackColor.Down.Value;
                    File_Panel.BackColor = _FilePanelBackColor.Down.Value;
                }
                else
                {
                    Title_Label.BackColor = _FolderNameBackColor.Normaly.Value;
                    File_Panel.BackColor = _FilePanelBackColor.Normaly.Value;
                }
            }
        }
        private bool _Selected = false;

        /// <summary> 表示用于显示内容的数据源
        /// 
        /// </summary>
        public override object DataItem
        {
            get { return _DataItem; }
            set
            {
                _DataItem = value;
                Title_Label.Text = base.GetDataValue(g_DataStruct.FolderName).ToString();
                DirectoryInfo DirTemp = new DirectoryInfo(base.GetDataValue(g_DataStruct.Path).ToString());
                File_Panel.Controls.Clear();
                IsFiles = false;
                if (DirTemp.Exists)
                {
                    FileInfo[] FileArray = DirTemp.GetFiles();

                    if (FileArray.Length != 0) IsFiles = true;
                    int MaxFileCount = 3;//这里限制扫描出来的文件不超过三个，否则如果文件太多了节点会非常的长
                    foreach (var FileItem in FileArray)
                    {
                        PictureBox ControlTemp = new PictureBox();
                        //ControlTemp.BorderStyle = BorderStyle.FixedSingle;

                        ControlTemp.Margin = new Padding(1);
                        ControlTemp.Padding = new Padding();
                        ControlTemp.SizeMode = PictureBoxSizeMode.StretchImage;
                        ControlTemp.Image = GetImageByFileName(FileItem.FullName);

                        ControlTemp.Parent = File_Panel;
                        MaxFileCount--;
                        if (MaxFileCount <= 0) break;
                    }
                }
                if (NodeContainer != null) NodeContainer.ResetNodeSize();
            }
        }
        private object _DataItem = null;

        private bool IsFiles=false;//指示该节点下是否有文件存在

        /// <summary> 获取或设置当前内容的缩放比例
        /// 
        /// </summary>
        public override float CurrentScaling { get; set; }

        /// <summary> 刷新节点内容的尺寸
        /// 
        /// </summary>
        public override void RefreshContentSize()
        {
            Title_Label.Font = FolderTitle_Font.ByScaling(CurrentScaling);//按照比例缩放
            Title_Label.Padding = FolderTitle_Padding.ByScaling(CurrentScaling);//按照比例缩放
            Title_panel.Width = Title_Label.Width;
            this.Height = Title_Label.Height;    
            
            File_Panel.Visible = IsFiles;//该文件夹下是否有文件
            int widthCount = 0;//文件图标所占的宽度
            if (IsFiles)//如果有文件就计算文件图标所占的宽度
            {
                foreach (Control ImgItem in File_Panel.Controls)
                {
                    ImgItem.Size = new Size(Title_Label.Height, Title_Label.Height);
                    widthCount += ImgItem.Margin.Left;
                    widthCount += ImgItem.Margin.Right;
                    widthCount += ImgItem.Width;
                }                
              
            }
            this.Width = widthCount + Title_panel.Width;
        }


        /// <summary> 文件夹名称的背景色
        /// 
        /// </summary>
        private Text_NodeContent.MindMapNodeBackColor _FolderNameBackColor = new Text_NodeContent.MindMapNodeBackColor(Color.FromArgb(48, 120, 215));

        /// <summary> 文件图标容器背景颜色
        /// 
        /// </summary>
        private Text_NodeContent.MindMapNodeBackColor _FilePanelBackColor = new Text_NodeContent.MindMapNodeBackColor(Color.FromArgb(98, 170, 255));


        /// <summary> 用于指示File_NodeContent下的数据的结构
        /// 
        /// </summary>
        public class File_ContentStruct : MindMapNodeStructBase
        {
            /// <summary> 文件夹名称
            /// 
            /// </summary>
            public string FolderName { get; set; }

            /// <summary> 文件夹路径
            /// 
            /// </summary>
            public string Path { get; set; }
        }

        #region 鼠标移入动画
        private void Title_panel_MouseEnter(object sender, EventArgs e)
        {
            Color ResultFolderColor = _FolderNameBackColor.Enter.Value;
            Color ResultFileColor = _FilePanelBackColor.Enter.Value;

            if (_Selected)
            {
                ResultFolderColor = _FolderNameBackColor.Down.Value;
                ResultFileColor = _FilePanelBackColor.Down.Value;
            }
            Title_Label.BackColor = ResultFolderColor;
            File_Panel.BackColor = ResultFileColor;
        }

        private void Title_panel_MouseLeave(object sender, EventArgs e)
        {
            Color ResultFolderColor = _FolderNameBackColor.Normaly.Value;
            Color ResultFileColor = _FilePanelBackColor.Normaly.Value;

            if (_Selected)
            {
                ResultFolderColor = _FolderNameBackColor.Down.Value;
                ResultFileColor = _FilePanelBackColor.Down.Value;
            }
            Title_Label.BackColor = ResultFolderColor;
            File_Panel.BackColor = ResultFileColor;
        }

        private void Title_panel_MouseDown(object sender, MouseEventArgs e)
        {
            Title_Label.BackColor = _FolderNameBackColor.Down.Value;
            File_Panel.BackColor = _FilePanelBackColor.Down.Value;
        }

        private void Title_panel_MouseUp(object sender, MouseEventArgs e)
        {
            Color ResultFolderColor = _FolderNameBackColor.Normaly.Value;
            Color ResultFileColor = _FilePanelBackColor.Normaly.Value;

            if (_Selected)
            {
                ResultFolderColor = _FolderNameBackColor.Down.Value;
                ResultFileColor = _FilePanelBackColor.Down.Value;
            }
            Title_Label.BackColor = ResultFolderColor;
            File_Panel.BackColor = ResultFileColor;
        }
        #endregion 鼠标移入动画
        #region 获取文件图标

        /// <summary> 获取一个文件的图标
        /// </summary>
        /// <param name="Filepath">文件路径</param>
        /// <returns>获取到的图标</returns> 
        public Image GetImageByFileName(string Filepath)
        {

            //Bitmap BitmapTemp = Icon.ExtractAssociatedIcon(Filepath).ToBitmap();
            //IntPtr Handle = BitmapTemp.GetHbitmap();
            //Image ReturnImage = Image.FromHbitmap(Handle);
            //return ReturnImage;
            SHFILEINFO SHFILEINFOTemp = new SHFILEINFO();
            SHGetFileInfo(Filepath, 0, ref SHFILEINFOTemp, (uint)System.Runtime.InteropServices.Marshal.SizeOf(SHFILEINFOTemp), (uint)FileInfoFlags.SHGFI_ICON | (uint)FileInfoFlags.SHGFI_USEFILEATTRIBUTES | (uint)FileInfoFlags.SHGFI_LARGEICON);
            System.Drawing.Icon IconImage = System.Drawing.Icon.FromHandle(SHFILEINFOTemp.hIcon);
            return Image.FromHbitmap(IconImage.ToBitmap().GetHbitmap());
        }

        #region 结构
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
        #endregion 结构

        #region 枚举
        public enum FileInfoFlags : uint
        {
            SHGFI_ICON = 0x000000100,  //  get icon
            SHGFI_DISPLAYNAME = 0x000000200,  //  get display name
            SHGFI_TYPENAME = 0x000000400,  //  get type name
            SHGFI_ATTRIBUTES = 0x000000800,  //  get attributes
            SHGFI_ICONLOCATION = 0x000001000,  //  get icon location
            SHGFI_EXETYPE = 0x000002000,  //  return exe type
            SHGFI_SYSICONINDEX = 0x000004000,  //  get system icon index
            SHGFI_LINKOVERLAY = 0x000008000,  //  put a link overlay on icon
            SHGFI_SELECTED = 0x000010000,  //  show icon in selected state
            SHGFI_ATTR_SPECIFIED = 0x000020000,  //  get only specified attributes
            SHGFI_LARGEICON = 0x000000000,  //  get large icon
            SHGFI_SMALLICON = 0x000000001,  //  get small icon
            SHGFI_OPENICON = 0x000000002,  //  get open icon
            SHGFI_SHELLICONSIZE = 0x000000004,  //  get shell size icon
            SHGFI_PIDL = 0x000000008,  //  pszPath is a pidl
            SHGFI_USEFILEATTRIBUTES = 0x000000010,  //  use passed dwFileAttribute
            SHGFI_ADDOVERLAYS = 0x000000020,  //  apply the appropriate overlays
            SHGFI_OVERLAYINDEX = 0x000000040   //  Get the index of the overlay
        }
        public enum FileAttributeFlags : uint
        {
            FILE_ATTRIBUTE_READONLY = 0x00000001,
            FILE_ATTRIBUTE_HIDDEN = 0x00000002,
            FILE_ATTRIBUTE_SYSTEM = 0x00000004,
            FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
            FILE_ATTRIBUTE_ARCHIVE = 0x00000020,
            FILE_ATTRIBUTE_DEVICE = 0x00000040,
            FILE_ATTRIBUTE_NORMAL = 0x00000080,
            FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
            FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,
            FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,
            FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
            FILE_ATTRIBUTE_OFFLINE = 0x00001000,
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,
            FILE_ATTRIBUTE_ENCRYPTED = 0x00004000
        }
        #endregion 枚举

        /// <summary>获取图标
        /// 
        /// </summary>
        /// <param name="pszPath"></param>
        /// <param name="dwFileAttributes"></param>
        /// <param name="psfi"></param>
        /// <param name="cbFileInfo"></param>
        /// <param name="uFlags"></param>
        /// <returns></returns>
        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);


        #endregion 获取文件图标

    }
}
