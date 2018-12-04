namespace WlxMindMap
{
    public partial class MindMapNodeContainer
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
            this.Content_Panel = new System.Windows.Forms.Panel();
            this.Chidren_Panel = new System.Windows.Forms.FlowLayoutPanel();
            this.DrawingLine_panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Content_Panel
            // 
            this.Content_Panel.BackColor = System.Drawing.Color.Transparent;
            this.Content_Panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.Content_Panel.Location = new System.Drawing.Point(0, 0);
            this.Content_Panel.Margin = new System.Windows.Forms.Padding(0);
            this.Content_Panel.Name = "Content_Panel";
            this.Content_Panel.Size = new System.Drawing.Size(61, 35);
            this.Content_Panel.TabIndex = 0;
            this.Content_Panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseClick);
            this.Content_Panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseDown);
            this.Content_Panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseMove);
            this.Content_Panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseUp);
            // 
            // Chidren_Panel
            // 
            this.Chidren_Panel.BackColor = System.Drawing.Color.Transparent;
            this.Chidren_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Chidren_Panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Chidren_Panel.Location = new System.Drawing.Point(95, 0);
            this.Chidren_Panel.Margin = new System.Windows.Forms.Padding(0);
            this.Chidren_Panel.Name = "Chidren_Panel";
            this.Chidren_Panel.Size = new System.Drawing.Size(840, 35);
            this.Chidren_Panel.TabIndex = 1;
            this.Chidren_Panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseClick);
            this.Chidren_Panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseDown);
            this.Chidren_Panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseMove);
            this.Chidren_Panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseUp);
            // 
            // DrawingLine_panel
            // 
            this.DrawingLine_panel.BackColor = System.Drawing.Color.White;
            this.DrawingLine_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.DrawingLine_panel.Location = new System.Drawing.Point(61, 0);
            this.DrawingLine_panel.Margin = new System.Windows.Forms.Padding(0);
            this.DrawingLine_panel.Name = "DrawingLine_panel";
            this.DrawingLine_panel.Size = new System.Drawing.Size(34, 35);
            this.DrawingLine_panel.TabIndex = 2;
            this.DrawingLine_panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseClick);
            this.DrawingLine_panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseDown);
            this.DrawingLine_panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseMove);
            this.DrawingLine_panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EmptyRange_MouseUp);
            // 
            // MindMapNodeContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Chidren_Panel);
            this.Controls.Add(this.DrawingLine_panel);
            this.Controls.Add(this.Content_Panel);
            this.Name = "MindMapNodeContainer";
            this.Size = new System.Drawing.Size(935, 35);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MindMapNode_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Content_Panel;
        private System.Windows.Forms.FlowLayoutPanel Chidren_Panel;
        private System.Windows.Forms.Panel DrawingLine_panel;
    }
}
