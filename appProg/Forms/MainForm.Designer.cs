namespace appProg
{
    partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menu = new System.Windows.Forms.TabControl();
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.manuStrip_review = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip_exportByID = new System.Windows.Forms.ToolStripMenuItem();
			this.stripMenu_exportByDates = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip_order = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip_app = new System.Windows.Forms.ToolStripMenuItem();
			this.about_app = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip_closeApp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menu
			// 
			this.menu.Dock = System.Windows.Forms.DockStyle.Fill;
			this.menu.Location = new System.Drawing.Point(0, 24);
			this.menu.Name = "menu";
			this.menu.SelectedIndex = 0;
			this.menu.Size = new System.Drawing.Size(616, 632);
			this.menu.TabIndex = 2;
			this.menu.SelectedIndexChanged += new System.EventHandler(this.menu_SelectedIndexChanged);
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manuStrip_review,
            this.menuStrip_order,
            this.menuStrip_app});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(616, 24);
			this.menuStrip.TabIndex = 17;
			this.menuStrip.Text = "menuStrip1";
			// 
			// manuStrip_review
			// 
			this.manuStrip_review.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStrip_exportByID,
            this.stripMenu_exportByDates});
			this.manuStrip_review.Name = "manuStrip_review";
			this.manuStrip_review.Size = new System.Drawing.Size(51, 20);
			this.manuStrip_review.Text = "Отчет";
			// 
			// menuStrip_exportByID
			// 
			this.menuStrip_exportByID.Name = "menuStrip_exportByID";
			this.menuStrip_exportByID.Size = new System.Drawing.Size(139, 22);
			this.menuStrip_exportByID.Text = "По заказу";
			this.menuStrip_exportByID.Click += new System.EventHandler(this.menuStrip_exportByID_Click);
			// 
			// stripMenu_exportByDates
			// 
			this.stripMenu_exportByDates.Name = "stripMenu_exportByDates";
			this.stripMenu_exportByDates.Size = new System.Drawing.Size(139, 22);
			this.stripMenu_exportByDates.Text = "По периоду";
			this.stripMenu_exportByDates.Click += new System.EventHandler(this.stripMenu_exportByDates_Click);
			// 
			// menuStrip_order
			// 
			this.menuStrip_order.Name = "menuStrip_order";
			this.menuStrip_order.Size = new System.Drawing.Size(49, 20);
			this.menuStrip_order.Text = "Заказ";
			this.menuStrip_order.Click += new System.EventHandler(this.menuStrip_order_Click);
			// 
			// menuStrip_app
			// 
			this.menuStrip_app.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.about_app,
            this.menuStrip_closeApp});
			this.menuStrip_app.Name = "menuStrip_app";
			this.menuStrip_app.Size = new System.Drawing.Size(91, 20);
			this.menuStrip_app.Text = "Приложение";
			// 
			// about_app
			// 
			this.about_app.Name = "about_app";
			this.about_app.Size = new System.Drawing.Size(152, 22);
			this.about_app.Text = "О программе";
			this.about_app.Click += new System.EventHandler(this.about_app_Click);
			// 
			// menuStrip_closeApp
			// 
			this.menuStrip_closeApp.Name = "menuStrip_closeApp";
			this.menuStrip_closeApp.Size = new System.Drawing.Size(152, 22);
			this.menuStrip_closeApp.Text = "Закрыть";
			this.menuStrip_closeApp.Click += new System.EventHandler(this.menuStrip_closeApp_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(616, 656);
			this.Controls.Add(this.menu);
			this.Controls.Add(this.menuStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Кафе";
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl menu;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem manuStrip_review;
		private System.Windows.Forms.ToolStripMenuItem menuStrip_order;
		private System.Windows.Forms.ToolStripMenuItem menuStrip_app;
		private System.Windows.Forms.ToolStripMenuItem menuStrip_closeApp;
		private System.Windows.Forms.ToolStripMenuItem menuStrip_exportByID;
		private System.Windows.Forms.ToolStripMenuItem stripMenu_exportByDates;
		private System.Windows.Forms.ToolStripMenuItem about_app;
	}
}

