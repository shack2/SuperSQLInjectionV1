namespace SuperSQLInjection
{
    partial class AddNode
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_addNode = new System.Windows.Forms.Button();
            this.txt_node_text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_addNode
            // 
            this.btn_addNode.Location = new System.Drawing.Point(192, 21);
            this.btn_addNode.Name = "btn_addNode";
            this.btn_addNode.Size = new System.Drawing.Size(56, 23);
            this.btn_addNode.TabIndex = 0;
            this.btn_addNode.Text = "添 加";
            this.btn_addNode.UseVisualStyleBackColor = true;
            this.btn_addNode.Click += new System.EventHandler(this.btn_addNode_Click);
            // 
            // txt_node_text
            // 
            this.txt_node_text.Location = new System.Drawing.Point(21, 21);
            this.txt_node_text.Name = "txt_node_text";
            this.txt_node_text.Size = new System.Drawing.Size(152, 21);
            this.txt_node_text.TabIndex = 1;
            // 
            // AddNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 64);
            this.Controls.Add(this.txt_node_text);
            this.Controls.Add(this.btn_addNode);
            this.Name = "AddNode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddNode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_addNode;
        private System.Windows.Forms.TextBox txt_node_text;
    }
}