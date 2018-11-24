namespace WlxMindMap
{
    partial class MindMap_Panel
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
            this.Scroll_panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Scroll_panel
            // 
            this.Scroll_panel.BackColor = System.Drawing.Color.White;
            this.Scroll_panel.Location = new System.Drawing.Point(0, 0);
            this.Scroll_panel.Name = "Scroll_panel";
            this.Scroll_panel.Size = new System.Drawing.Size(467, 389);
            this.Scroll_panel.TabIndex = 2;
            this.Scroll_panel.Click += new System.EventHandler(this.mindMapNode_EmptyRangeClick);
            this.Scroll_panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MindMap_Panel_MouseDown);
            this.Scroll_panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MindMap_Panel_MouseMove);
            this.Scroll_panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MindMap_Panel_MouseUp);
            // 
            // MindMap_Panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.Scroll_panel);
            this.Name = "MindMap_Panel";
            this.Size = new System.Drawing.Size(467, 389);
            this.Click += new System.EventHandler(this.mindMapNode_EmptyRangeClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MindMap_Panel_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MindMap_Panel_MouseUp);
            this.Resize += new System.EventHandler(this.MindMap_Panel_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Scroll_panel;
        
    }
}
