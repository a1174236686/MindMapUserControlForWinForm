using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MindMap
{
    public partial class frmRightMenu : Form
    {
        public frmRightMenu()
        {
            InitializeComponent();
        }

        public DirectoryInfo Dir{ get; set; }
        private void frmRightMenu_Leave(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRightMenu_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRightMenu_Load(object sender, EventArgs e)
        {
            if (!(Dir.Exists)) this.Close();

            FileInfo[] FileArray = Dir.GetFiles();
            foreach (FileInfo FileInfoItem in FileArray)
            {
                FileItem FileControl = new FileItem();
                FileControl.FileName = FileInfoItem.Name;
                FileControl.FilePath = FileInfoItem.FullName;
                FileControl.FileIco = GetImageByFileName(FileInfoItem.FullName);
                FileControl.Parent = Files_Panel;
            }



        }
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
