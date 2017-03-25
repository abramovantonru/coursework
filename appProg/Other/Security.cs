using appProg.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static appProg.Other.Crypt;

namespace appProg.Other
{
	public class Security
	{
		readonly public static string dir = AppDomain.CurrentDomain.BaseDirectory + "security";
		readonly public static string pinFile = dir + "\\.md5_pin";

		public static bool isAdmin() 
		{
			MessageBox.Show(pinFile);
			md5 hash = new md5();
			
			if(Util.checkFile(pinFile)) {
				pinForm _pinForm = new pinForm(false);
				if (_pinForm.ShowDialog() == DialogResult.OK)
					return true;
				else {
					MessageBox.Show("Введен неверный пин-код.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
			}
			else {
				MessageBox.Show("Не создан пин-код. Укажите его в следующем окне.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				pinForm _pinForm = new pinForm(true);
				if (_pinForm.ShowDialog() == DialogResult.OK)
					MessageBox.Show("Зашифрованный пин-код успешно создан!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
				
			}
			return false;
		}

	}
}
