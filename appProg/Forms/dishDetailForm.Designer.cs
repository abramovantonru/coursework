namespace cafeMenu
{
	partial class dishDetailForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dishDetailForm));
			this.dishDetail = new System.Windows.Forms.TabControl();
			this.btnDecCountDish = new System.Windows.Forms.Button();
			this.btnIncCountDish = new System.Windows.Forms.Button();
			this.countOfDetailDish = new System.Windows.Forms.TextBox();
			this.btnAddToOrder = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// dishDetail
			// 
			this.dishDetail.Location = new System.Drawing.Point(0, 44);
			this.dishDetail.Name = "dishDetail";
			this.dishDetail.SelectedIndex = 0;
			this.dishDetail.Size = new System.Drawing.Size(670, 448);
			this.dishDetail.TabIndex = 4;
			// 
			// btnDecCountDish
			// 
			this.btnDecCountDish.Location = new System.Drawing.Point(221, 11);
			this.btnDecCountDish.Name = "btnDecCountDish";
			this.btnDecCountDish.Size = new System.Drawing.Size(23, 23);
			this.btnDecCountDish.TabIndex = 19;
			this.btnDecCountDish.Text = "-";
			this.btnDecCountDish.UseVisualStyleBackColor = true;
			this.btnDecCountDish.Click += new System.EventHandler(this.btnDecCountDish_Click);
			// 
			// btnIncCountDish
			// 
			this.btnIncCountDish.Location = new System.Drawing.Point(150, 11);
			this.btnIncCountDish.Name = "btnIncCountDish";
			this.btnIncCountDish.Size = new System.Drawing.Size(23, 23);
			this.btnIncCountDish.TabIndex = 18;
			this.btnIncCountDish.Text = "+";
			this.btnIncCountDish.UseVisualStyleBackColor = true;
			this.btnIncCountDish.Click += new System.EventHandler(this.btnIncCountDish_Click);
			// 
			// countOfDetailDish
			// 
			this.countOfDetailDish.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.countOfDetailDish.Location = new System.Drawing.Point(182, 12);
			this.countOfDetailDish.MaxLength = 2;
			this.countOfDetailDish.Name = "countOfDetailDish";
			this.countOfDetailDish.Size = new System.Drawing.Size(30, 21);
			this.countOfDetailDish.TabIndex = 17;
			// 
			// btnAddToOrder
			// 
			this.btnAddToOrder.Location = new System.Drawing.Point(12, 8);
			this.btnAddToOrder.Name = "btnAddToOrder";
			this.btnAddToOrder.Size = new System.Drawing.Size(120, 30);
			this.btnAddToOrder.TabIndex = 16;
			this.btnAddToOrder.Text = "Добавить в заказ";
			this.btnAddToOrder.UseVisualStyleBackColor = true;
			this.btnAddToOrder.Click += new System.EventHandler(this.addToOrder_Click);
			// 
			// dishDetailForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(670, 492);
			this.Controls.Add(this.btnDecCountDish);
			this.Controls.Add(this.btnIncCountDish);
			this.Controls.Add(this.countOfDetailDish);
			this.Controls.Add(this.btnAddToOrder);
			this.Controls.Add(this.dishDetail);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "dishDetailForm";
			this.Load += new System.EventHandler(this.dishDetailForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl dishDetail;
		private System.Windows.Forms.Button btnDecCountDish;
		private System.Windows.Forms.Button btnIncCountDish;
		private System.Windows.Forms.TextBox countOfDetailDish;
		private System.Windows.Forms.Button btnAddToOrder;
	}
}