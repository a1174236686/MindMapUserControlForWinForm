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
            this.panel1 = new System.Windows.Forms.Panel();
            this.RunPath_TextBox = new System.Windows.Forms.TextBox();
            this.MinWindow_Label = new System.Windows.Forms.Label();
            this.MaxWIndow_Label = new System.Windows.Forms.Label();
            this.CloseWindow_Label = new System.Windows.Forms.Label();
            this.MoveWindow_Label = new System.Windows.Forms.Label();
            this.RightKey_Menu.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.mindMap_Panel1.Location = new System.Drawing.Point(0, 22);
            this.mindMap_Panel1.Name = "mindMap_Panel1";
            this.mindMap_Panel1.Size = new System.Drawing.Size(1569, 810);
            this.mindMap_Panel1.TabIndex = 0;
            this.mindMap_Panel1.MindeMapNodeToNodeDragDrop += new System.Windows.Forms.DragEventHandler(this.mindMap_Panel1_MindeMapNodeToNodeDragDrop);
            this.mindMap_Panel1.MindMapNodeMouseClick += new System.Windows.Forms.MouseEventHandler(this.mindMap_Panel1_MindMapNodeMouseClick);
            this.mindMap_Panel1.MindMapNodeMouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mindMap_Panel1_MindMapNodeMouseDoubleClick);
            this.mindMap_Panel1.MindNodemapKeyDown += new System.Windows.Forms.KeyEventHandler(this.mindMap_Panel1_MindNodemapKeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RunPath_TextBox);
            this.panel1.Controls.Add(this.MinWindow_Label);
            this.panel1.Controls.Add(this.MaxWIndow_Label);
            this.panel1.Controls.Add(this.CloseWindow_Label);
            this.panel1.Controls.Add(this.MoveWindow_Label);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1569, 22);
            this.panel1.TabIndex = 2;
            // 
            // RunPath_TextBox
            // 
            this.RunPath_TextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunPath_TextBox.Location = new System.Drawing.Point(51, 0);
            this.RunPath_TextBox.Name = "RunPath_TextBox";
            this.RunPath_TextBox.ReadOnly = true;
            this.RunPath_TextBox.Size = new System.Drawing.Size(1401, 21);
            this.RunPath_TextBox.TabIndex = 1;
            // 
            // MinWindow_Label
            // 
            this.MinWindow_Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MinWindow_Label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MinWindow_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.MinWindow_Label.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MinWindow_Label.ForeColor = System.Drawing.Color.White;
            this.MinWindow_Label.Location = new System.Drawing.Point(1452, 0);
            this.MinWindow_Label.Name = "MinWindow_Label";
            this.MinWindow_Label.Size = new System.Drawing.Size(39, 22);
            this.MinWindow_Label.TabIndex = 4;
            this.MinWindow_Label.Text = "_";
            this.MinWindow_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MinWindow_Label.Click += new System.EventHandler(this.CloseWindow_Label_Click);
            this.MinWindow_Label.MouseEnter += new System.EventHandler(this.CloseWindow_Label_MouseEnter);
            this.MinWindow_Label.MouseLeave += new System.EventHandler(this.CloseWindow_Label_MouseLeave);
            // 
            // MaxWIndow_Label
            // 
            this.MaxWIndow_Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MaxWIndow_Label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MaxWIndow_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.MaxWIndow_Label.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaxWIndow_Label.ForeColor = System.Drawing.Color.White;
            this.MaxWIndow_Label.Location = new System.Drawing.Point(1491, 0);
            this.MaxWIndow_Label.Name = "MaxWIndow_Label";
            this.MaxWIndow_Label.Size = new System.Drawing.Size(39, 22);
            this.MaxWIndow_Label.TabIndex = 3;
            this.MaxWIndow_Label.Text = "口";
            this.MaxWIndow_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MaxWIndow_Label.Click += new System.EventHandler(this.CloseWindow_Label_Click);
            this.MaxWIndow_Label.MouseEnter += new System.EventHandler(this.CloseWindow_Label_MouseEnter);
            this.MaxWIndow_Label.MouseLeave += new System.EventHandler(this.CloseWindow_Label_MouseLeave);
            // 
            // CloseWindow_Label
            // 
            this.CloseWindow_Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CloseWindow_Label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseWindow_Label.Dock = System.Windows.Forms.DockStyle.Right;
            this.CloseWindow_Label.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CloseWindow_Label.ForeColor = System.Drawing.Color.White;
            this.CloseWindow_Label.Location = new System.Drawing.Point(1530, 0);
            this.CloseWindow_Label.Name = "CloseWindow_Label";
            this.CloseWindow_Label.Size = new System.Drawing.Size(39, 22);
            this.CloseWindow_Label.TabIndex = 2;
            this.CloseWindow_Label.Text = "×";
            this.CloseWindow_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CloseWindow_Label.Click += new System.EventHandler(this.CloseWindow_Label_Click);
            this.CloseWindow_Label.MouseEnter += new System.EventHandler(this.CloseWindow_Label_MouseEnter);
            this.CloseWindow_Label.MouseLeave += new System.EventHandler(this.CloseWindow_Label_MouseLeave);
            // 
            // MoveWindow_Label
            // 
            this.MoveWindow_Label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MoveWindow_Label.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.MoveWindow_Label.Dock = System.Windows.Forms.DockStyle.Left;
            this.MoveWindow_Label.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MoveWindow_Label.ForeColor = System.Drawing.Color.White;
            this.MoveWindow_Label.Location = new System.Drawing.Point(0, 0);
            this.MoveWindow_Label.Name = "MoveWindow_Label";
            this.MoveWindow_Label.Size = new System.Drawing.Size(51, 22);
            this.MoveWindow_Label.TabIndex = 0;
            this.MoveWindow_Label.Text = "Move";
            this.MoveWindow_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MoveWindow_Label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveWindow_Label_MouseDown);
            this.MoveWindow_Label.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveWindow_Label_MouseMove);
            this.MoveWindow_Label.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveWindow_Label_MouseUp);
            // 
            // frmMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1569, 832);
            this.Controls.Add(this.Edit_textBox);
            this.Controls.Add(this.mindMap_Panel1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMainForm";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.frmMainForm_Activated);
            this.Load += new System.EventHandler(this.frmMainForm_Load);
            this.RightKey_Menu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox RunPath_TextBox;
        private System.Windows.Forms.Label MinWindow_Label;
        private System.Windows.Forms.Label MaxWIndow_Label;
        private System.Windows.Forms.Label CloseWindow_Label;
        private System.Windows.Forms.Label MoveWindow_Label;
    }
}

