namespace appProg
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAppClose = new System.Windows.Forms.Button();
            this.menu = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // btnAppClose
            // 
            this.btnAppClose.Location = new System.Drawing.Point(831, 12);
            this.btnAppClose.Name = "btnAppClose";
            this.btnAppClose.Size = new System.Drawing.Size(75, 23);
            this.btnAppClose.TabIndex = 1;
            this.btnAppClose.Text = "Закрыть";
            this.btnAppClose.UseVisualStyleBackColor = true;
            this.btnAppClose.Click += new System.EventHandler(this.btnAppClose_Click);
            // 
            // menu
            // 
            this.menu.Location = new System.Drawing.Point(12, 12);
            this.menu.Name = "menu";
            this.menu.SelectedIndex = 0;
            this.menu.Size = new System.Drawing.Size(454, 559);
            this.menu.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(918, 656);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.btnAppClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Кафе";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAppClose;
        private System.Windows.Forms.TabControl menu;
    }
}

