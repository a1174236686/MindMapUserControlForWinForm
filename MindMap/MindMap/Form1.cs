using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WlxMindMap;
using WlxMindMap.MindMapNodeContent;
namespace MindMap
{
    public partial class frmMainForm : Form
    {
        public frmMainForm()
        {
            InitializeComponent();


        }
        private void frmMainForm_Activated(object sender, EventArgs e)
        {
            this.TopMost = false;
        }

        private void frmMainForm_Load(object sender, EventArgs e)
        {
            List<TestEntity> DataSourceList = new List<TestEntity>();
            if (String.IsNullOrEmpty(Program.ParamePath))
            {
                //以前用于测试的数据
                #region List数据源

                TestEntity TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "0";
                TestEntityTemp.ParentID = "-1";
                TestEntityTemp.Text = "编程语言";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "1";
                TestEntityTemp.ParentID = "0";
                TestEntityTemp.Text = "面向过程";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "2";
                TestEntityTemp.ParentID = "0";
                TestEntityTemp.Text = "面向对象";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "23";
                TestEntityTemp.ParentID = "0";
                TestEntityTemp.Text = "标记语言";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "3";
                TestEntityTemp.ParentID = "2";
                TestEntityTemp.Text = "JAVA";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "4";
                TestEntityTemp.ParentID = "2";
                TestEntityTemp.Text = "C++";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "5";
                TestEntityTemp.ParentID = "2";
                TestEntityTemp.Text = "C#";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "6";
                TestEntityTemp.ParentID = "1";
                TestEntityTemp.Text = "C";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "7";
                TestEntityTemp.ParentID = "1";
                TestEntityTemp.Text = "汇编语言";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "8";
                TestEntityTemp.ParentID = "6";
                TestEntityTemp.Text = "指针";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "9";
                TestEntityTemp.ParentID = "6";
                TestEntityTemp.Text = "函数";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "10";
                TestEntityTemp.ParentID = "6";
                TestEntityTemp.Text = "头文件";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "11";
                TestEntityTemp.ParentID = "7";
                TestEntityTemp.Text = "内存地址";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "12";
                TestEntityTemp.ParentID = "7";
                TestEntityTemp.Text = "寄存器";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "13";
                TestEntityTemp.ParentID = "7";
                TestEntityTemp.Text = "结构偏移";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "14";
                TestEntityTemp.ParentID = "3";
                TestEntityTemp.Text = "Tomcat";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "15";
                TestEntityTemp.ParentID = "3";
                TestEntityTemp.Text = "Spring";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "16";
                TestEntityTemp.ParentID = "3";
                TestEntityTemp.Text = "Eclipse";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "17";
                TestEntityTemp.ParentID = "4";
                TestEntityTemp.Text = "类型不安全";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "18";
                TestEntityTemp.ParentID = "4";
                TestEntityTemp.Text = "VC6.0";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "19";
                TestEntityTemp.ParentID = "4";
                TestEntityTemp.Text = "运算符重载";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "20";
                TestEntityTemp.ParentID = "5";
                TestEntityTemp.Text = "委托";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "21";
                TestEntityTemp.ParentID = "5";
                TestEntityTemp.Text = "匿名对象";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "22";
                TestEntityTemp.ParentID = "5";
                TestEntityTemp.Text = "Linq";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "24";
                TestEntityTemp.ParentID = "23";
                TestEntityTemp.Text = "Html+Css";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "25";
                TestEntityTemp.ParentID = "23";
                TestEntityTemp.Text = "XAML";
                DataSourceList.Add(TestEntityTemp);

                TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = "26";
                TestEntityTemp.ParentID = "23";
                TestEntityTemp.Text = "Json";
                DataSourceList.Add(TestEntityTemp);
                #endregion List数据源

            }
            else
            {
                //string PathTemp = Program.ParamePath;
                //string[] PathArrayTemp = PathTemp.Split('\\');
                //PathTemp = PathArrayTemp.LastOrDefault();

                string PathTemp = new DirectoryInfo(Program.ParamePath).Name;
                TestEntity TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = GetID();
                TestEntityTemp.Text = PathTemp;
                TestEntityTemp.ParentID = "Base";
                TestEntityTemp.Path = Program.ParamePath;
                DataSourceList.Add(TestEntityTemp);
                DataSourceList.AddRange(GetDirectoriesList(Program.ParamePath, TestEntityTemp.ID, 20));
            }


            Text_MindMapNodeContent.Text_ContentStruct NodeStruct = new Text_MindMapNodeContent.Text_ContentStruct();
            NodeStruct.MindMapID = "ID";
            NodeStruct.MindMapParentID = "ParentID";
            NodeStruct.Text = "Text";

            mindMap_Panel1.DataStruct = NodeStruct;
            mindMap_Panel1.SetDataSource<WlxMindMap.MindMapNodeContent.Text_MindMapNodeContent, TestEntity>(DataSourceList);

          
            //MindMapNodeContainer ContainerTemp = new MindMapNodeContainer();
            //ContainerTemp.SetNodeContent<Text_MindMapNodeContent>(NodeStruct);
            //ContainerTemp.DataItem = new TestEntity() { ID = "100", ParentID = "123", Text = "手动添加" };
            //mindMap_Panel1.BaseNode.AddNode(ContainerTemp);
        }

        #region 私有方法
        /// <summary> 是否选中一个节点
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsSelectedOne()
        {
            List<MindMapNodeContainer> MindMapNodeContainerList = mindMap_Panel1.GetSelectedNode();
            if (MindMapNodeContainerList.Count == 1) return true;
            return false;


        }


        /// <summary> 获取当前选中的节点内容
        /// 
        /// </summary>
        /// <returns></returns>
        private MindMapNodeContentBase GetCurrentContent()
        {
            if (!IsSelectedOne()) return null;//多个节点被选中
            List<MindMapNodeContainer> MindMapNodeContainerList = mindMap_Panel1.GetSelectedNode();
            MindMapNodeContainer CurrentNode = MindMapNodeContainerList.FirstOrDefault();
            if (CurrentNode == null) return null;//必须有被选中节点
            return CurrentNode.NodeContent;
        }


        /// <summary> 递归获取文件夹
        /// 
        /// </summary>
        /// <param name="ParentPath">要获取的路径</param>
        /// <param name="ParentID">父节点ID</param>
        /// <param name="i_Parame">获取深度</param>
        /// <returns></returns>
        private List<TestEntity> GetDirectoriesList(string ParentPath, string ParentID, int i_Parame)
        {
            List<TestEntity> ResultList = new List<TestEntity>();
            if (i_Parame == 0) return ResultList;
            int CurrentI = i_Parame - 1;

            string[] PathArray = Directory.GetDirectories(ParentPath);

            foreach (string PathItem in PathArray)
            {
                string PathTemp = new DirectoryInfo(PathItem).Name;
                Regex regexTemp = new Regex(@"[\u4e00-\u9fa5]");
                if (!regexTemp.IsMatch(PathTemp)) continue;
                TestEntity TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = GetID();
                TestEntityTemp.Text = PathTemp;
                TestEntityTemp.ParentID = ParentID;
                TestEntityTemp.Path = PathItem;
                ResultList.Add(TestEntityTemp);
                ResultList.AddRange(GetDirectoriesList(PathItem, TestEntityTemp.ID, CurrentI));
            }
            return ResultList;
        }

        private static Random DBRandom = new Random();
        /// <summary> 发号[暂时随机发号，以后数据量多的话再制定发号规则]
        /// </summary>
        /// <returns></returns>
        private string GetID()
        {
            string IncludeChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random RandomTemp = DBRandom;
            StringBuilder StringBuilderTemp = new StringBuilder();
            for (int i = 0; i < 20; i++)
            {
                int CharIndex = RandomTemp.Next(IncludeChar.Length);
                StringBuilderTemp.Append(IncludeChar[CharIndex]);
            }
            return StringBuilderTemp.ToString();



        }
        #endregion 私有方法

        #region 右键菜单

        private void 重命名QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MindMapNodeContentBase CurrentContent = GetCurrentContent();

            if (CurrentContent == null) return;
            if (!(CurrentContent is Text_MindMapNodeContent)) return;
            Edit_textBox.Text = ((Text_MindMapNodeContent)CurrentContent).ContentText;
            Edit_textBox.Visible = true;
            #region 居中编辑框

            Edit_textBox.Left = (this.Size.Width - Edit_textBox.Size.Width) / 2;
            Edit_textBox.Top = (this.Size.Height - Edit_textBox.Size.Height) / 2;

            #endregion 居中编辑框
            Edit_textBox.Focus();

        }

        private void 添加同级文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MindMapNodeContentBase MindMapNodeContentBase = GetCurrentContent();
            if (MindMapNodeContentBase == null) return;
            MindMapNodeContainer MindMapNodeContainer = MindMapNodeContentBase.ParentMindMapNode.ParentNode;
            if (MindMapNodeContainer == null) return;//没有父节点直接返回
            AddFolder(MindMapNodeContainer);//添加一个文件夹


        }

        private void 添加子文件夹ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MindMapNodeContentBase MindMapNodeContentBase = GetCurrentContent();
            if (MindMapNodeContentBase == null) return;
            AddFolder(MindMapNodeContentBase.ParentMindMapNode);//添加一个文件夹

        }

        private void 删除文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<MindMapNodeContainer> MindMapNodeContainerList = mindMap_Panel1.GetBaseSelectedNode();


            foreach (var item in MindMapNodeContainerList)
            {
                TestEntity TestEntityTemp = (TestEntity)(item.DataItem);
                new DirectoryInfo(TestEntityTemp.Path).Delete(true);
                item.ParentNode = null;
            }

        }
        /// <summary> 按照默认文件名添加一个文件夹
        /// 
        /// </summary>
        /// <param name="ContainerParame">需要添加文件夹的节点</param>
        private void AddFolder(MindMapNodeContainer ContainerParame)
        {
            TestEntity TestEntityTemp = (TestEntity)(ContainerParame.DataItem);
            string FileName = GetMinDefaultsFolderName(TestEntityTemp.Path);
            DirectoryInfo CreatedFolder = new DirectoryInfo(TestEntityTemp.Path).CreateSubdirectory(FileName);

            TestEntity NewTestEntity = new TestEntity();
            NewTestEntity.ParentID = TestEntityTemp.ID;
            NewTestEntity.ID = GetID();
            NewTestEntity.Path = CreatedFolder.FullName;
            NewTestEntity.Text = CreatedFolder.Name;

            MindMapNodeContainer MindMapNodeContainerTemp = new MindMapNodeContainer();
            MindMapNodeContainerTemp.SetNodeContent<Text_MindMapNodeContent>(ContainerParame.DataStruct);
            MindMapNodeContainerTemp.NodeContent.DataItem = NewTestEntity;
            ContainerParame.AddNode(MindMapNodeContainerTemp);

            mindMap_Panel1.GetSelectedNode().ForEach(T1 => T1.NodeContent.Selected = false);//取消所有选中
            MindMapNodeContainerTemp.NodeContent.Selected = true;//选中刚添加的


        }

        /// <summary> 获取默认的文件名称
        /// 
        /// </summary>
        /// <param name="StrPathParame">文件夹路径</param>
        /// <returns></returns>
        private string GetMinDefaultsFolderName(string StrPathParame)
        {
            DirectoryInfo[] DirectoryInfoArray = new DirectoryInfo(StrPathParame).GetDirectories();
            Regex RegexTemp = new Regex(@"(?<=文件夹)\d+");
            List<string> FolderNameList = DirectoryInfoArray.Where(T1 => RegexTemp.IsMatch(T1.Name)).Select(T1 => T1.Name).ToList();//获取已有默认名称的文件夹名称
            FolderNameList = FolderNameList.Select(T1 => RegexTemp.Match(T1).Value).ToList();//将文件夹名称的序号部分截取出来
            List<int> FolderNumList = FolderNameList.Select(T1 => Int32.Parse(T1)).ToList();//将String转换为int;

            List<int> NumberInt = new List<int>();//声明0-99的集合
            for (int i = 1; i < 99; i++) NumberInt.Add(i);

            NumberInt = NumberInt.Where(T1 => !FolderNumList.Contains(T1)).ToList();//剔除掉已有的文件序号
            int FolderNum = NumberInt.FirstOrDefault();//获取最小的序号
            return "文件夹" + FolderNum.ToString();

        }

        #endregion 右键菜单

        #region 事件

        /// <summary> 用于编辑的TextBox按下回车完成编辑，按下esc取消编辑        
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    List<MindMapNodeContainer> MindMapNodeContainerList = mindMap_Panel1.GetSelectedNode();
                    MindMapNodeContainer CurrentNode = MindMapNodeContainerList.FirstOrDefault();
                    if (CurrentNode != null)//必须有被选中节点
                    {
                        if (CurrentNode.DataItem is TestEntity)//被选中节点数据类型必须为TestEntity
                        {
                            TestEntity CurrentData = (TestEntity)CurrentNode.DataItem;
                            string StrPath = Path.GetDirectoryName(CurrentData.Path);
                            StrPath = StrPath + @"\" + Edit_textBox.Text;
                            if (!Directory.Exists(StrPath))
                            {
                                DirectoryInfo DirectoryInfoTemp = new DirectoryInfo(CurrentData.Path);
                                DirectoryInfoTemp.MoveTo(StrPath);

                                CurrentData.Text = Edit_textBox.Text;
                                CurrentData.Path = StrPath;
                                CurrentNode.NodeContent.DataItem = CurrentData;
                            }
                        }
                    }
                    Edit_textBox.Visible = false;
                    break;

                case Keys.Escape:
                    Edit_textBox.Visible = false;
                    break;
            }
            //e.Handled = true;
        }

        /// <summary> 焦点离开编辑节点的编辑框就取消编辑
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Edit_textBox_Leave(object sender, EventArgs e)
        {
            ((Control)sender).Visible = false;
        }

        /// <summary> 右键节点显示菜单
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMap_Panel1_MindMapNodeMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                RightKey_Menu.Show(Control.MousePosition);
            }
        }

        /// <summary> 键盘按下的快捷键
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMap_Panel1_MindNodemapKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Tab:
                    添加子文件夹ToolStripMenuItem1_Click(null, null);
                    break;
                case Keys.Enter:
                    添加同级文件夹ToolStripMenuItem_Click(null, null);
                    break;
                case Keys.Space:
                    重命名QToolStripMenuItem_Click(null, null);
                    break;
                case Keys.Delete:
                    删除文件夹ToolStripMenuItem_Click(null, null);
                    break;
            }
        }

        /// <summary> 拖动选中节点
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMap_Panel1_MindeMapNodeToNodeDragDrop(object sender, DragEventArgs e)
        {
            MindMapNodeContainer DragTarget = ((Control)sender).GetNodeContent().ParentMindMapNode;
            mindMap_Panel1.Visible = false;

            mindMap_Panel1.GetSelectedNode().ForEach(T1 => T1.ParentNode = DragTarget);
            mindMap_Panel1.Visible = true;
        }

        /// <summary> 双击节点打开文件夹
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mindMap_Panel1_MindMapNodeMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            MindMapNodeContentBase contentTemp = ((Control)sender).GetNodeContent();
            if (contentTemp == null) return;
            TestEntity TestEntityTemp = (TestEntity)(contentTemp.DataItem);
            Process.Start(TestEntityTemp.Path);
        }
        #endregion 事件
        
        #region 这个可以用于禁用窗体的tab键

        //protected override bool ProcessDialogKey(Keys keydata)
        //{
        //    if (keydata == Keys.Tab)
        //    {
        //        //添加子文件夹ToolStripMenuItem1_Click(null, null);
        //        //return false;
        //    }
        //    return base.ProcessDialogKey(keydata);
        //} 
        #endregion 这个可以用于禁用窗体的tab键
        
    }


    public class TestEntity
    {
        public string ID { get; set; }

        public string ParentID { set; get; }

        public string Text { get; set; }

        public string Path { get; set; }
    }

}
