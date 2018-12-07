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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.mindMap_Panel1 = new WlxMindMap.MindMap_Panel();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 872);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1663, 21);
            this.textBox1.TabIndex = 1;
            // 
            // mindMap_Panel1
            // 
            this.mindMap_Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.mindMap_Panel1.CurrentScaling = 1F;
            this.mindMap_Panel1.DataStruct = null;
            this.mindMap_Panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mindMap_Panel1.Location = new System.Drawing.Point(0, 0);
            this.mindMap_Panel1.Name = "mindMap_Panel1";
            this.mindMap_Panel1.Size = new System.Drawing.Size(1663, 872);
            this.mindMap_Panel1.TabIndex = 0;
            this.mindMap_Panel1.MindeMapNodeToNodeDragDrop += new System.Windows.Forms.DragEventHandler(this.mindMap_Panel1_MindeMapNodeToNodeDragDrop);
            // 
            // frmMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1663, 893);
            this.Controls.Add(this.mindMap_Panel1);
            this.Controls.Add(this.textBox1);
            this.Name = "frmMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMainForm";
            this.Load += new System.EventHandler(this.frmMainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private WlxMindMap.MindMap_Panel mindMap_Panel1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

