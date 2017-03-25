namespace appProg.Forms
{
	partial class pinForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(pinForm));
			this.pinInput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// pinInput
			// 
			this.pinInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F);
			this.pinInput.Location = new System.Drawing.Point(56, 64);
			this.pinInput.Name = "pinInput";
			this.pinInput.PasswordChar = '@';
			this.pinInput.Size = new System.Drawing.Size(179, 41);
			this.pinInput.TabIndex = 0;
			this.pinInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pinInput_KeyDown);
			// 
			// pinForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 162);
			this.Controls.Add(this.pinInput);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "pinForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Введите пин-код";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pinForm_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox pinInput;
	}
}