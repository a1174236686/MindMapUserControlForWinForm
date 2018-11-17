namespace MindMap.View
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
            this.mindMapNode = new MindMap.View.MindMapNode();
            this.SuspendLayout();
            // 
            // mindMapNode
            // 
            this.mindMapNode.BackColor = System.Drawing.Color.White;
            this.mindMapNode.Location = new System.Drawing.Point(0, 0);
            this.mindMapNode.Name = "mindMapNode";
            this.mindMapNode.ParentNode = null;
            this.mindMapNode.Selected = false;
            this.mindMapNode.Size = new System.Drawing.Size(92, 23);
            this.mindMapNode.TabIndex = 0;
            this.mindMapNode.TextFont = new System.Drawing.Font("微软雅黑", 12F);
            this.mindMapNode.MindMapNodeMouseEnter += new System.EventHandler(this.mindMapNode_MindMapNodeMouseEnter);
            this.mindMapNode.MindMapNodeMouseLeave += new System.EventHandler(this.mindMapNode_MindMapNodeMouseLeave);
            this.mindMapNode.MindMapNodeMouseDown += new System.Windows.Forms.MouseEventHandler(this.mindMapNode_MindMapNodeMouseDown);
            this.mindMapNode.MindMapNodeMouseUp += new System.Windows.Forms.MouseEventHandler(this.mindMapNode_MindMapNodeMouseUp);
            // 
            // MindMap_Panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mindMapNode);
            this.Name = "MindMap_Panel";
            this.Size = new System.Drawing.Size(467, 389);
            this.ResumeLayout(false);

        }

        #endregion

        private MindMapNode mindMapNode;
    }
}
