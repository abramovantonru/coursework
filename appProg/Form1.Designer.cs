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
			this.dishDetail = new System.Windows.Forms.TabControl();
			this.searchBtn = new System.Windows.Forms.Button();
			this.searchInput = new System.Windows.Forms.TextBox();
			this.searchLabel = new System.Windows.Forms.Label();
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
			this.menu.Location = new System.Drawing.Point(12, 41);
			this.menu.Name = "menu";
			this.menu.SelectedIndex = 0;
			this.menu.Size = new System.Drawing.Size(454, 530);
			this.menu.TabIndex = 2;
			this.menu.SelectedIndexChanged += new System.EventHandler(this.menu_SelectedIndexChanged);
			// 
			// dishDetail
			// 
			this.dishDetail.Location = new System.Drawing.Point(472, 41);
			this.dishDetail.Name = "dishDetail";
			this.dishDetail.SelectedIndex = 0;
			this.dishDetail.Size = new System.Drawing.Size(434, 257);
			this.dishDetail.TabIndex = 3;
			// 
			// searchBtn
			// 
			this.searchBtn.Location = new System.Drawing.Point(338, 12);
			this.searchBtn.Name = "searchBtn";
			this.searchBtn.Size = new System.Drawing.Size(75, 23);
			this.searchBtn.TabIndex = 4;
			this.searchBtn.Text = "Поиск";
			this.searchBtn.UseVisualStyleBackColor = true;
			// 
			// searchInput
			// 
			this.searchInput.Location = new System.Drawing.Point(100, 14);
			this.searchInput.Name = "searchInput";
			this.searchInput.Size = new System.Drawing.Size(196, 20);
			this.searchInput.TabIndex = 5;
			// 
			// searchLabel
			// 
			this.searchLabel.AutoSize = true;
			this.searchLabel.Location = new System.Drawing.Point(22, 17);
			this.searchLabel.Name = "searchLabel";
			this.searchLabel.Size = new System.Drawing.Size(39, 13);
			this.searchLabel.TabIndex = 6;
			this.searchLabel.Text = "Поиск";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(918, 656);
			this.Controls.Add(this.searchLabel);
			this.Controls.Add(this.searchInput);
			this.Controls.Add(this.searchBtn);
			this.Controls.Add(this.dishDetail);
			this.Controls.Add(this.menu);
			this.Controls.Add(this.btnAppClose);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Кафе";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Load += new System.EventHandler(this.onLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAppClose;
        private System.Windows.Forms.TabControl menu;
		private System.Windows.Forms.TabControl dishDetail;
		private System.Windows.Forms.Button searchBtn;
		private System.Windows.Forms.TextBox searchInput;
		private System.Windows.Forms.Label searchLabel;
	}
}

