namespace cafeMenu.Forms
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.license = new System.Windows.Forms.Label();
			this.licenseLink = new System.Windows.Forms.LinkLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.developer = new System.Windows.Forms.Label();
			this.programName = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// license
			// 
			this.license.AutoSize = true;
			this.license.Font = new System.Drawing.Font("Calibri", 14F);
			this.license.Location = new System.Drawing.Point(45, 230);
			this.license.Name = "license";
			this.license.Size = new System.Drawing.Size(170, 23);
			this.license.TabIndex = 0;
			this.license.Text = "Лицензия: GPL 3.0 - ";
			// 
			// licenseLink
			// 
			this.licenseLink.AutoSize = true;
			this.licenseLink.Font = new System.Drawing.Font("Calibri", 14F);
			this.licenseLink.Location = new System.Drawing.Point(208, 230);
			this.licenseLink.Name = "licenseLink";
			this.licenseLink.Size = new System.Drawing.Size(327, 23);
			this.licenseLink.TabIndex = 1;
			this.licenseLink.TabStop = true;
			this.licenseLink.Text = "https://opensource.org/licenses/GPL-3.0";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::cafeMenu.Properties.Resources.png_coffe_cup;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(222, 194);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// developer
			// 
			this.developer.AutoSize = true;
			this.developer.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.developer.Location = new System.Drawing.Point(254, 121);
			this.developer.Name = "developer";
			this.developer.Size = new System.Drawing.Size(244, 23);
			this.developer.TabIndex = 3;
			this.developer.Text = "Разработчик: Абрамов Антон";
			// 
			// programName
			// 
			this.programName.AutoSize = true;
			this.programName.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.programName.Location = new System.Drawing.Point(254, 82);
			this.programName.Name = "programName";
			this.programName.Size = new System.Drawing.Size(207, 23);
			this.programName.TabIndex = 4;
			this.programName.Text = "Название: \"Меню кафе\" ";
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(584, 262);
			this.Controls.Add(this.programName);
			this.Controls.Add(this.developer);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.licenseLink);
			this.Controls.Add(this.license);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.Text = "О программе";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label license;
		private System.Windows.Forms.LinkLabel licenseLink;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label developer;
		private System.Windows.Forms.Label programName;
	}
}