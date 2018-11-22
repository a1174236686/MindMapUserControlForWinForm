namespace WlxMindMap.MindMapNodeContent
{
    partial class Text_MindMapNodeContent
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
            this.Content_lable = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // Content_lable
            // 
            this.Content_lable.AutoSize = true;
            this.Content_lable.BackColor = System.Drawing.Color.DodgerBlue;
            this.Content_lable.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.Content_lable.LinkColor = System.Drawing.Color.White;
            this.Content_lable.Location = new System.Drawing.Point(0, 0);
            this.Content_lable.Name = "Content_lable";
            this.Content_lable.Padding = new System.Windows.Forms.Padding(5);
            this.Content_lable.Size = new System.Drawing.Size(63, 22);
            this.Content_lable.TabIndex = 0;
            this.Content_lable.TabStop = true;
            this.Content_lable.Text = "新的节点";
            this.Content_lable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Content_lable.VisitedLinkColor = System.Drawing.Color.White;
            // 
            // Text_MindMapNodeContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Content_lable);
            this.Name = "Text_MindMapNodeContent";
            this.Size = new System.Drawing.Size(149, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel Content_lable;
    }
}
