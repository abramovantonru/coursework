using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cafeMenu
{
	/**
 * Gallery images 
 * */
	public partial class Gallery : UserControl
	{
		/**
		 * Initial gallery settings
		 * */
		private static int maxCount = 4;
		private int onLine = 0;
		private int margin = 20;
		private int padding = 10;
		private int xPos = 0;
		private int yPos = 0;
		private int width = 0;
		private int imgWidth = 0;
		private string formName = "";

		/**
		 * Constructor 
		 * */
		public Gallery(List<Image> images, int _width, int _height, string _formName = "", int _count = 0)
		{
			// default max count on line
			if (_count != 0)
				maxCount = _count;
			width = _width - margin * 2 - padding * maxCount;
			imgWidth = width / maxCount;
			xPos = yPos = margin;
			formName = _formName;

			this.Top = this.Left = 0;
			this.Size = new Size(
				_width,
				_height
			);
			this.AutoScrollMinSize = new Size(_width, _height); // for show scroll bars if need

			this.CreateGallery(images);
		}

		private void DrawPictureBox(Image img)
		{
			PictureBox image = new PictureBox();

			// go to next line logic
			if (onLine == maxCount)
			{
				onLine = 0;
				xPos = margin;
				yPos += imgWidth + padding;
			}

			//image settings
			image.Image = img;
			image.Width = image.Height = imgWidth;
			image.SizeMode = PictureBoxSizeMode.StretchImage;
			image.Click += (s, e) => {
				dishDetailForm.showDetailImage(img, formName);
			};
			image.Top = yPos;
			image.Left = xPos;

			// setting next element on line
			xPos += imgWidth + padding;
			onLine++;

			this.Controls.Add(image);
		}

		private void CreateGallery(List<Image> images)
		{
			RemoveControls();
			int count = images.Count;

			for (int i = 0; i < count; i++)
				DrawPictureBox(images.ElementAt(i));
		}

		private void RemoveControls()
		{
			foreach (Control ctrl in this.Controls)
			{
				if ((ctrl) is PictureBox)
				{
					this.Controls.Remove(ctrl);
				}
			}
			if (this.Controls.Count > 0)
			{
				RemoveControls();
			}
		}
	}
}
