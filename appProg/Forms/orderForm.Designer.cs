namespace appProg
{
	partial class orderForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(orderForm));
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.btnOpenOrder = new System.Windows.Forms.ToolStripMenuItem();
			this.btnCreateOrder = new System.Windows.Forms.ToolStripMenuItem();
			this.btnSaveOrder = new System.Windows.Forms.ToolStripMenuItem();
			this.btnRemoveOrder = new System.Windows.Forms.ToolStripMenuItem();
			this.btnCloseOrder = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenOrder,
            this.btnCreateOrder,
            this.btnSaveOrder,
            this.btnRemoveOrder,
            this.btnCloseOrder});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(620, 24);
			this.menuStrip.TabIndex = 23;
			this.menuStrip.Text = "menuStrip1";
			// 
			// btnOpenOrder
			// 
			this.btnOpenOrder.Name = "btnOpenOrder";
			this.btnOpenOrder.Size = new System.Drawing.Size(66, 20);
			this.btnOpenOrder.Text = "Открыть";
			this.btnOpenOrder.Click += new System.EventHandler(this.btnOpenOrder_Click);
			// 
			// btnCreateOrder
			// 
			this.btnCreateOrder.Name = "btnCreateOrder";
			this.btnCreateOrder.Size = new System.Drawing.Size(62, 20);
			this.btnCreateOrder.Text = "Создать";
			this.btnCreateOrder.Click += new System.EventHandler(this.btnCreateOrder_Click);
			// 
			// btnSaveOrder
			// 
			this.btnSaveOrder.Name = "btnSaveOrder";
			this.btnSaveOrder.Size = new System.Drawing.Size(77, 20);
			this.btnSaveOrder.Text = "Сохранить";
			this.btnSaveOrder.Click += new System.EventHandler(this.btnSaveOrder_Click);
			// 
			// btnRemoveOrder
			// 
			this.btnRemoveOrder.Name = "btnRemoveOrder";
			this.btnRemoveOrder.Size = new System.Drawing.Size(63, 20);
			this.btnRemoveOrder.Text = "Удалить";
			this.btnRemoveOrder.Click += new System.EventHandler(this.btnRemoveOrder_Click);
			// 
			// btnCloseOrder
			// 
			this.btnCloseOrder.Name = "btnCloseOrder";
			this.btnCloseOrder.Size = new System.Drawing.Size(65, 20);
			this.btnCloseOrder.Text = "Закрыть";
			this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
			// 
			// orderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(620, 583);
			this.Controls.Add(this.menuStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip;
			this.Name = "orderForm";
			this.Text = "Заказы";
			this.Load += new System.EventHandler(this.orderForm_Load);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem btnOpenOrder;
		private System.Windows.Forms.ToolStripMenuItem btnCreateOrder;
		private System.Windows.Forms.ToolStripMenuItem btnSaveOrder;
		private System.Windows.Forms.ToolStripMenuItem btnRemoveOrder;
		private System.Windows.Forms.ToolStripMenuItem btnCloseOrder;
	}
}