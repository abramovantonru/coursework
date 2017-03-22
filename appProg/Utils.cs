using appProg.Properties;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace appProg
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


	}
}
