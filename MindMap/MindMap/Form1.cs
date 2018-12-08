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
                string PathTemp = Program.ParamePath;
                string[] PathArrayTemp = PathTemp.Split('\\');
                PathTemp = PathArrayTemp.LastOrDefault();
                TestEntity TestEntityTemp = new TestEntity();
                TestEntityTemp.ID = GetID();
                TestEntityTemp.Text = PathTemp;
                TestEntityTemp.ParentID = "Base";
                TestEntityTemp.Path = Program.ParamePath;       
                DataSourceList.Add(TestEntityTemp);
                DataSourceList.AddRange(GetDirectoriesList(Program.ParamePath, TestEntityTemp.ID,20));
            }


            Text_MindMapNodeContent.Text_ContentStruct NodeStruct =new Text_MindMapNodeContent.Text_ContentStruct ();
            NodeStruct.MindMapID="ID";
            NodeStruct.MindMapParentID = "ParentID";
            NodeStruct.Text= "Text";

            mindMap_Panel1.DataStruct = NodeStruct;
            mindMap_Panel1.SetDataSource<WlxMindMap.MindMapNodeContent.Text_MindMapNodeContent, TestEntity>(DataSourceList);

            //MindMapNodeContainer ContainerTemp = new MindMapNodeContainer();
            //ContainerTemp.SetNodeContent<Text_MindMapNodeContent>(NodeStruct);
            //ContainerTemp.DataItem = new TestEntity() { ID = "100", ParentID = "123", Text = "手动添加" };
            //mindMap_Panel1.BaseNode.AddNode(ContainerTemp);
        }
        
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
            MindMapNodeContentBase contentTemp= ((Control)sender).GetNodeContent();
            if (contentTemp == null) return;
            TestEntity TestEntityTemp=(TestEntity)(contentTemp.DataItem);
            Process.Start(TestEntityTemp.Path);
        }

        #region 私有方法
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
                string[] PathArrayTemp = PathItem.Split('\\');
                string PathTemp = PathArrayTemp.LastOrDefault();
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
    }


    public class TestEntity
    {
        public string ID { get; set; }

        public string ParentID { set; get; }

        public string Text { get; set; }

        public string Path { get; set; }
    }

}
