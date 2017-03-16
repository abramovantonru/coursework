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
			this.order = new System.Windows.Forms.TabControl();
			this.btnCreateOrder = new System.Windows.Forms.Button();
			this.btnSaveOrder = new System.Windows.Forms.Button();
			this.btnRemoveOrder = new System.Windows.Forms.Button();
			this.btnCloseOrder = new System.Windows.Forms.Button();
			this.btnAddToOrder = new System.Windows.Forms.Button();
			this.countOfDetailDish = new System.Windows.Forms.TextBox();
			this.btnIncCountDish = new System.Windows.Forms.Button();
			this.btnDecCountDish = new System.Windows.Forms.Button();
			this.btnOpenOrder = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnAppClose
			// 
			this.btnAppClose.Location = new System.Drawing.Point(781, 12);
			this.btnAppClose.Name = "btnAppClose";
			this.btnAppClose.Size = new System.Drawing.Size(125, 23);
			this.btnAppClose.TabIndex = 1;
			this.btnAppClose.Text = "Закрыть приложение";
			this.btnAppClose.UseVisualStyleBackColor = true;
			this.btnAppClose.Click += new System.EventHandler(this.btnAppClose_Click);
			// 
			// menu
			// 
			this.menu.Location = new System.Drawing.Point(12, 41);
			this.menu.Name = "menu";
			this.menu.SelectedIndex = 0;
			this.menu.Size = new System.Drawing.Size(454, 603);
			this.menu.TabIndex = 2;
			this.menu.SelectedIndexChanged += new System.EventHandler(this.menu_SelectedIndexChanged);
			// 
			// dishDetail
			// 
			this.dishDetail.Location = new System.Drawing.Point(472, 41);
			this.dishDetail.Name = "dishDetail";
			this.dishDetail.SelectedIndex = 0;
			this.dishDetail.Size = new System.Drawing.Size(434, 287);
			this.dishDetail.TabIndex = 3;
			// 
			// order
			// 
			this.order.Location = new System.Drawing.Point(472, 363);
			this.order.Name = "order";
			this.order.SelectedIndex = 0;
			this.order.Size = new System.Drawing.Size(434, 281);
			this.order.TabIndex = 7;
			// 
			// btnCreateOrder
			// 
			this.btnCreateOrder.Location = new System.Drawing.Point(585, 334);
			this.btnCreateOrder.Name = "btnCreateOrder";
			this.btnCreateOrder.Size = new System.Drawing.Size(90, 23);
			this.btnCreateOrder.TabIndex = 8;
			this.btnCreateOrder.Text = "Новый заказ";
			this.btnCreateOrder.UseVisualStyleBackColor = true;
			this.btnCreateOrder.Click += new System.EventHandler(this.createOrder_Click);
			// 
			// btnSaveOrder
			// 
			this.btnSaveOrder.Location = new System.Drawing.Point(694, 334);
			this.btnSaveOrder.Name = "btnSaveOrder";
			this.btnSaveOrder.Size = new System.Drawing.Size(80, 23);
			this.btnSaveOrder.TabIndex = 9;
			this.btnSaveOrder.Text = "Сохранить";
			this.btnSaveOrder.UseVisualStyleBackColor = true;
			this.btnSaveOrder.Click += new System.EventHandler(this.saveOrder_Click);
			// 
			// btnRemoveOrder
			// 
			this.btnRemoveOrder.Location = new System.Drawing.Point(780, 335);
			this.btnRemoveOrder.Name = "btnRemoveOrder";
			this.btnRemoveOrder.Size = new System.Drawing.Size(60, 23);
			this.btnRemoveOrder.TabIndex = 10;
			this.btnRemoveOrder.Text = "Удалить";
			this.btnRemoveOrder.UseVisualStyleBackColor = true;
			this.btnRemoveOrder.Click += new System.EventHandler(this.removeOrder_Click);
			// 
			// btnCloseOrder
			// 
			this.btnCloseOrder.Location = new System.Drawing.Point(846, 334);
			this.btnCloseOrder.Name = "btnCloseOrder";
			this.btnCloseOrder.Size = new System.Drawing.Size(60, 23);
			this.btnCloseOrder.TabIndex = 11;
			this.btnCloseOrder.Text = "Закрыть";
			this.btnCloseOrder.UseVisualStyleBackColor = true;
			this.btnCloseOrder.Click += new System.EventHandler(this.closeOrder_Click);
			// 
			// btnAddToOrder
			// 
			this.btnAddToOrder.Enabled = false;
			this.btnAddToOrder.Location = new System.Drawing.Point(472, 12);
			this.btnAddToOrder.Name = "btnAddToOrder";
			this.btnAddToOrder.Size = new System.Drawing.Size(120, 23);
			this.btnAddToOrder.TabIndex = 12;
			this.btnAddToOrder.Text = "Добавить в заказ";
			this.btnAddToOrder.UseVisualStyleBackColor = true;
			this.btnAddToOrder.Click += new System.EventHandler(this.addToOrder_Click);
			// 
			// countOfDetailDish
			// 
			this.countOfDetailDish.Enabled = false;
			this.countOfDetailDish.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.countOfDetailDish.Location = new System.Drawing.Point(645, 14);
			this.countOfDetailDish.MaxLength = 2;
			this.countOfDetailDish.Name = "countOfDetailDish";
			this.countOfDetailDish.Size = new System.Drawing.Size(30, 21);
			this.countOfDetailDish.TabIndex = 13;
			// 
			// btnIncCountDish
			// 
			this.btnIncCountDish.Enabled = false;
			this.btnIncCountDish.Location = new System.Drawing.Point(615, 11);
			this.btnIncCountDish.Name = "btnIncCountDish";
			this.btnIncCountDish.Size = new System.Drawing.Size(23, 23);
			this.btnIncCountDish.TabIndex = 14;
			this.btnIncCountDish.Text = "+";
			this.btnIncCountDish.UseVisualStyleBackColor = true;
			this.btnIncCountDish.Click += new System.EventHandler(this.IncCountDish_Click);
			// 
			// btnDecCountDish
			// 
			this.btnDecCountDish.Enabled = false;
			this.btnDecCountDish.Location = new System.Drawing.Point(680, 12);
			this.btnDecCountDish.Name = "btnDecCountDish";
			this.btnDecCountDish.Size = new System.Drawing.Size(23, 23);
			this.btnDecCountDish.TabIndex = 15;
			this.btnDecCountDish.Text = "-";
			this.btnDecCountDish.UseVisualStyleBackColor = true;
			this.btnDecCountDish.Click += new System.EventHandler(this.DecCountDish_Click);
			// 
			// btnOpenOrder
			// 
			this.btnOpenOrder.Location = new System.Drawing.Point(472, 334);
			this.btnOpenOrder.Name = "btnOpenOrder";
			this.btnOpenOrder.Size = new System.Drawing.Size(100, 23);
			this.btnOpenOrder.TabIndex = 16;
			this.btnOpenOrder.Text = "Открыть заказ";
			this.btnOpenOrder.UseVisualStyleBackColor = true;
			this.btnOpenOrder.Click += new System.EventHandler(this.openOrder_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(918, 656);
			this.Controls.Add(this.btnOpenOrder);
			this.Controls.Add(this.btnDecCountDish);
			this.Controls.Add(this.btnIncCountDish);
			this.Controls.Add(this.countOfDetailDish);
			this.Controls.Add(this.btnAddToOrder);
			this.Controls.Add(this.btnCloseOrder);
			this.Controls.Add(this.btnRemoveOrder);
			this.Controls.Add(this.btnSaveOrder);
			this.Controls.Add(this.btnCreateOrder);
			this.Controls.Add(this.order);
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
		private System.Windows.Forms.TabControl order;
		private System.Windows.Forms.Button btnCreateOrder;
		private System.Windows.Forms.Button btnSaveOrder;
		private System.Windows.Forms.Button btnRemoveOrder;
		private System.Windows.Forms.Button btnCloseOrder;
		private System.Windows.Forms.Button btnAddToOrder;
		private System.Windows.Forms.TextBox countOfDetailDish;
		private System.Windows.Forms.Button btnIncCountDish;
		private System.Windows.Forms.Button btnDecCountDish;
		private System.Windows.Forms.Button btnOpenOrder;
	}
}

