namespace MindMap
{
    partial class File_NodeContent
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Title_panel = new System.Windows.Forms.Panel();
            this.Title_Label = new System.Windows.Forms.Label();
            this.File_Panel = new System.Windows.Forms.FlowLayoutPanel();
            this.Title_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Title_panel
            // 
            this.Title_panel.Controls.Add(this.Title_Label);
            this.Title_panel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Title_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.Title_panel.Location = new System.Drawing.Point(0, 0);
            this.Title_panel.Margin = new System.Windows.Forms.Padding(0);
            this.Title_panel.Name = "Title_panel";
            this.Title_panel.Size = new System.Drawing.Size(71, 30);
            this.Title_panel.TabIndex = 0;
            this.Title_panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Title_panel_MouseDown);
            this.Title_panel.MouseEnter += new System.EventHandler(this.Title_panel_MouseEnter);
            this.Title_panel.MouseLeave += new System.EventHandler(this.Title_panel_MouseLeave);
            this.Title_panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Title_panel_MouseUp);
            // 
            // Title_Label
            // 
            this.Title_Label.AutoSize = true;
            this.Title_Label.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Title_Label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Title_Label.Location = new System.Drawing.Point(0, 0);
            this.Title_Label.Margin = new System.Windows.Forms.Padding(0);
            this.Title_Label.Name = "Title_Label";
            this.Title_Label.Padding = new System.Windows.Forms.Padding(5);
            this.Title_Label.Size = new System.Drawing.Size(51, 22);
            this.Title_Label.TabIndex = 0;
            this.Title_Label.Text = "label1";
            this.Title_Label.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Title_panel_MouseDown);
            this.Title_Label.MouseEnter += new System.EventHandler(this.Title_panel_MouseEnter);
            this.Title_Label.MouseLeave += new System.EventHandler(this.Title_panel_MouseLeave);
            this.Title_Label.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Title_panel_MouseUp);
            // 
            // File_Panel
            // 
            this.File_Panel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.File_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.File_Panel.Location = new System.Drawing.Point(71, 0);
            this.File_Panel.Margin = new System.Windows.Forms.Padding(1);
            this.File_Panel.Name = "File_Panel";
            this.File_Panel.Size = new System.Drawing.Size(127, 30);
            this.File_Panel.TabIndex = 1;
            this.File_Panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Title_panel_MouseDown);
            this.File_Panel.MouseEnter += new System.EventHandler(this.Title_panel_MouseEnter);
            this.File_Panel.MouseLeave += new System.EventHandler(this.Title_panel_MouseLeave);
            this.File_Panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Title_panel_MouseUp);
            // 
            // File_NodeContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.File_Panel);
            this.Controls.Add(this.Title_panel);
            this.Name = "File_NodeContent";
            this.Size = new System.Drawing.Size(198, 30);
            this.Title_panel.ResumeLayout(false);
            this.Title_panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Title_panel;
        private System.Windows.Forms.Label Title_Label;
        private System.Windows.Forms.FlowLayoutPanel File_Panel;
    }
}
