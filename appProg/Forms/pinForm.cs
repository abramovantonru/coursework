using appProg.Other;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static appProg.Other.Crypt;

namespace appProg.Forms
{
	public partial class pinForm : Form
	{
		private bool type = false; 

		public pinForm(bool _type = false)
		{
			InitializeComponent();

			type = _type;
		}

		private void pinForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && pinInput.Text.Length > 0)
				if (this.checkPinFromInput(pinInput.Text))
					this.DialogResult = DialogResult.OK;
				else
					this.DialogResult = DialogResult.Cancel;
			else if (e.KeyCode == Keys.Escape)
					this.DialogResult = DialogResult.Cancel;
		}

		public string getPasswordFromFile(string path)
		{
			string password = null;

			if (File.Exists(path))
				password = File.ReadLines(path).First();

			return password;
		}

		private bool checkPinFromInput(string pin) 
		{
			if (type)
			{
				md5 hash = new md5();

				Util.createDir(Security.dir);
				File.WriteAllText(Security.pinFile, hash.getHash(pin));
				return true;
			}
			else
			{
				if (Util.checkFile(Security.pinFile)) {
					md5 hash = new md5();
					string pin_hash = getPasswordFromFile(Security.pinFile);
					if (pin_hash != null)
						return hash.checkHash(pin, pin_hash);
					else
					{
						MessageBox.Show("Не удалось получить зашифрованный пин-код!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return false;
					}
				}
				else {
					MessageBox.Show("Не удалось найти зашифрованный пин-код!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return false;
				}
			}
		}

		private void pinInput_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && pinInput.Text.Length > 0)
				if (this.checkPinFromInput(pinInput.Text))
					this.DialogResult = DialogResult.OK;
				else
					this.DialogResult = DialogResult.Cancel;
			else if (e.KeyCode == Keys.Escape)
				this.DialogResult = DialogResult.Cancel;
		}
	}
}
