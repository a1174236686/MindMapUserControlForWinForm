using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            mindMap_Panel1.MouseWheel += new MouseEventHandler(OnMouseWhell);
            
        }

        private void frmMainForm_Load(object sender, EventArgs e)
        {
            #region List数据源
            List<TestEntity> DataSourceList = new List<TestEntity>();
            TestEntity TestEntityTemp = new TestEntity();
            TestEntityTemp.ID = "0";
            TestEntityTemp.ParentID = "";
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

            Text_MindMapNodeContent.Text_ContentStruct NodeStruct =new Text_MindMapNodeContent.Text_ContentStruct ();
            NodeStruct.MindMapID="ID";
            NodeStruct.MindMapParentID = "ParentID";
            NodeStruct.Text= "Text";

            mindMap_Panel1.DataStruct = NodeStruct;
            mindMap_Panel1.SetDataSource<WlxMindMap.MindMapNodeContent.Text_MindMapNodeContent, TestEntity>(DataSourceList);

            MindMapNodeContainer ContainerTemp = new MindMapNodeContainer();
            ContainerTemp.SetNodeContent<Text_MindMapNodeContent>(NodeStruct);
            ContainerTemp.DataItem = new TestEntity() { ID = "100", ParentID = "123", Text = "手动添加" };
            mindMap_Panel1.BaseNode.AddNode(ContainerTemp);

          


        }
        /// <summary> 滚轮放大缩小
        /// 
        /// </summary>
        /// <param name="Send"></param>
        /// <param name="e"></param>
        private void OnMouseWhell(object Send, MouseEventArgs e)
        {
            return;          
                                 
        }

        private void mindMap_Panel1_MouseDown(object sender, MouseEventArgs e)
        {

        }
    }

    public class TestEntity
    {
        public string ID { get; set; }

        public string ParentID { set; get; }

        public string Text { get; set; }
    }

}
