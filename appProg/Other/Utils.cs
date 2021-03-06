﻿using cafeMenu.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace cafeMenu
{
	/**
	 * Other functions for magic
	 * */
	class Util
	{
		/**
		 * Get default image from resources
		 * */
		public static Image defaultImage()
		{
			Image defaultImage = Resources.image_not_found;
			return defaultImage;
		}
		/**
		 * Convert from byte[] to Image datatype
		 * */
		public static Image bytesToImage(byte[] bytes)
		{
			using (var stream = new MemoryStream(bytes))
			{
				return Image.FromStream(stream);
			}
		}

		public static void keyPress_onlyFloat(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
				e.Handled = true;

			if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
				e.Handled = true;
		}

		public static void keyPress_onlyInt(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
				e.Handled = true;
		}

		public static bool checkFile(string path)
		{
			if (File.Exists(path))
				return true;
			else
				return false;
		}

		public static void createDir(string dir)
		{
			try
			{
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				MessageBox.Show("Не удалось создать директорию!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public static void openScreenKeyboard()
		{
			Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\system32\\osk.exe");
		}
	}
}
