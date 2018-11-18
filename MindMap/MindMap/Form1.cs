using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MindMap.View;

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
            TestEntityTemp.ID = "1";
            TestEntityTemp.ParentID = "";
            TestEntityTemp.Text = "面向过程";
            DataSourceList.Add(TestEntityTemp);

            TestEntityTemp = new TestEntity();
            TestEntityTemp.ID = "2";
            TestEntityTemp.ParentID = "";
            TestEntityTemp.Text = "面向对象";
            DataSourceList.Add(TestEntityTemp);

            TestEntityTemp = new TestEntity();
            TestEntityTemp.ID = "23";
            TestEntityTemp.ParentID = "";
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

            MindMap_Panel.TreeViewNodeStruct NodeStruct = new MindMap_Panel.TreeViewNodeStruct();
            NodeStruct.KeyName="ID";
            NodeStruct.ParentName = "ParentID";
            NodeStruct.ValueName = "Text";

            mindMap_Panel1.SetDataSource<TestEntity>(DataSourceList, NodeStruct);


        }
        private int FontSize = 13;
        /// <summary> 滚轮放大缩小
        /// 
        /// </summary>
        /// <param name="Send"></param>
        /// <param name="e"></param>
        private void OnMouseWhell(object Send, MouseEventArgs e)
        {
            int ChangeValue = 1;//每次放大或缩小的数值
            if (e.Delta < 0) FontSize = FontSize - ChangeValue <= ChangeValue ? ChangeValue : FontSize - ChangeValue;
            else FontSize = FontSize + ChangeValue;

            Font TextFontTemp = new Font(new FontFamily("微软雅黑"), FontSize);
            mindMap_Panel1.Visible = false;
            mindMap_Panel1.TextFont = TextFontTemp;
            mindMap_Panel1.Visible = true;
                                 
        }
                
    }

    public class TestEntity
    {
        public string ID { get; set; }

        public string ParentID { set; get; }

        public string Text { get; set; }
    }

}
