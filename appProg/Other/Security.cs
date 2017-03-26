using cafeMenu.Forms;
using System;
using System.Windows.Forms;
using static cafeMenu.Other.Crypt;

namespace cafeMenu.Other
{
	public class Security
	{
		readonly public static string dir = AppDomain.CurrentDomain.BaseDirectory + "security";
		readonly public static string pinFile = dir + "\\.md5_pin";

		public static bool isAdmin() 
		{
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
