namespace MindMap
{
    partial class frmMainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RightKey_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.重命名QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加同级文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加子文件夹ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除文件夹ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Edit_textBox = new System.Windows.Forms.TextBox();
            this.mindMap_Panel1 = new WlxMindMap.MindMap_Panel();
            this.RightKey_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // RightKey_Menu
            // 
            this.RightKey_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重命名QToolStripMenuItem,
            this.添加同级文件夹ToolStripMenuItem,
            this.添加子文件夹ToolStripMenuItem1,
            this.删除文件夹ToolStripMenuItem});
            this.RightKey_Menu.Name = "RightKey_Menu";
            this.RightKey_Menu.Size = new System.Drawing.Size(177, 92);
            // 
            // 重命名QToolStripMenuItem
            // 
            this.重命名QToolStripMenuItem.Name = "重命名QToolStripMenuItem";
            this.重命名QToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.重命名QToolStripMenuItem.Text = "重命名(&Q)";
            this.重命名QToolStripMenuItem.Click += new System.EventHandler(this.重命名QToolStripMenuItem_Click);
            // 
            // 添加同级文件夹ToolStripMenuItem
            // 
            this.添加同级文件夹ToolStripMenuItem.Name = "添加同级文件夹ToolStripMenuItem";
            this.添加同级文件夹ToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.添加同级文件夹ToolStripMenuItem.Text = "添加同级文件夹(&A)";
            this.添加同级文件夹ToolStripMenuItem.Click += new System.EventHandler(this.添加同级文件夹ToolStripMenuItem_Click);
            // 
            // 添加子文件夹ToolStripMenuItem1
            // 
            this.添加子文件夹ToolStripMenuItem1.Name = "添加子文件夹ToolStripMenuItem1";
            this.添加子文件夹ToolStripMenuItem1.Size = new System.Drawing.Size(176, 22);
            this.添加子文件夹ToolStripMenuItem1.Text = "添加子文件夹(&S)";
            this.添加子文件夹ToolStripMenuItem1.Click += new System.EventHandler(this.添加子文件夹ToolStripMenuItem1_Click);
            // 
            // 删除文件夹ToolStripMenuItem
            // 
            this.删除文件夹ToolStripMenuItem.Name = "删除文件夹ToolStripMenuItem";
            this.删除文件夹ToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.删除文件夹ToolStripMenuItem.Text = "删除文件夹(&D)";
            this.删除文件夹ToolStripMenuItem.Click += new System.EventHandler(this.删除文件夹ToolStripMenuItem_Click);
            // 
            // Edit_textBox
            // 
            this.Edit_textBox.Location = new System.Drawing.Point(572, 401);
            this.Edit_textBox.Name = "Edit_textBox";
            this.Edit_textBox.Size = new System.Drawing.Size(459, 21);
            this.Edit_textBox.TabIndex = 1;
            this.Edit_textBox.Visible = false;
            this.Edit_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Edit_textBox_KeyDown);
            this.Edit_textBox.Leave += new System.EventHandler(this.Edit_textBox_Leave);
            // 
            // mindMap_Panel1
            // 
            this.mindMap_Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.mindMap_Panel1.CurrentScaling = 1F;
            this.mindMap_Panel1.DataStruct = null;
            this.mindMap_Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mindMap_Panel1.Location = new System.Drawing.Point(0, 0);
            this.mindMap_Panel1.Name = "mindMap_Panel1";
            this.mindMap_Panel1.Size = new System.Drawing.Size(1663, 893);
            this.mindMap_Panel1.TabIndex = 0;
            this.mindMap_Panel1.MindeMapNodeToNodeDragDrop += new System.Windows.Forms.DragEventHandler(this.mindMap_Panel1_MindeMapNodeToNodeDragDrop);
            this.mindMap_Panel1.MindMapNodeMouseClick += new System.Windows.Forms.MouseEventHandler(this.mindMap_Panel1_MindMapNodeMouseClick);
            this.mindMap_Panel1.MindMapNodeMouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mindMap_Panel1_MindMapNodeMouseDoubleClick);
            this.mindMap_Panel1.MindNodemapKeyDown += new System.Windows.Forms.KeyEventHandler(this.mindMap_Panel1_MindNodemapKeyDown);
            // 
            // frmMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1663, 893);
            this.Controls.Add(this.Edit_textBox);
            this.Controls.Add(this.mindMap_Panel1);
            this.Name = "frmMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMainForm";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmMainForm_Activated);
            this.Load += new System.EventHandler(this.frmMainForm_Load);
            this.RightKey_Menu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private WlxMindMap.MindMap_Panel mindMap_Panel1;
        private System.Windows.Forms.ContextMenuStrip RightKey_Menu;
        private System.Windows.Forms.ToolStripMenuItem 重命名QToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加同级文件夹ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加子文件夹ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除文件夹ToolStripMenuItem;
        private System.Windows.Forms.TextBox Edit_textBox;
    }
}

