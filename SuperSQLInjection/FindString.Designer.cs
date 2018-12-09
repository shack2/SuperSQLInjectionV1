namespace SuperSQLInjection
{
    partial class FindString
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
            this.btn_find = new System.Windows.Forms.Button();
            this.txt_key = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_find
            // 
            this.btn_find.Location = new System.Drawing.Point(279, 17);
            this.btn_find.Name = "btn_find";
            this.btn_find.Size = new System.Drawing.Size(75, 23);
            this.btn_find.TabIndex = 0;
            this.btn_find.Text = "查找";
            this.btn_find.UseVisualStyleBackColor = true;
            this.btn_find.Click += new System.EventHandler(this.btn_find_Click);
            // 
            // txt_key
            // 
            this.txt_key.Location = new System.Drawing.Point(83, 19);
            this.txt_key.Name = "txt_key";
            this.txt_key.Size = new System.Drawing.Size(112, 21);
            this.txt_key.TabIndex = 1;
            this.txt_key.TextChanged += new System.EventHandler(this.txt_key_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "查找字符：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "匹配：0";
            // 
            // FindString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 61);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_key);
            this.Controls.Add(this.btn_find);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindString";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查找字符";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_find;
        private System.Windows.Forms.TextBox txt_key;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}