using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appProg
{
	public partial class dishDetailForm : Form
	{
		private static Form detailImageWindow; // window for view detail image of dish
		private static detailDish selectedDish;

		public dishDetailForm(int id)
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterScreen;
			this.dishDetail.TabPages.Clear();

			detailDish dish = DB.getDishByID(id);

			int width = dishDetail.Width;
			int height = dishDetail.Height;

			List<string> tabs = new List<string> { "Основная информация", "Картинки" };
			menuColumn[] columns = {
				new menuColumn { name = "key", header = "Параметр"},
				new menuColumn { name = "unit", header = "Единица"},
				new menuColumn { name = "value", header = "Значение"},
			};
			int countColumns = columns.Count();

			TabPage mainTab = new TabPage("Основная информация");
			TabPage imageTab = null;
			DataGridView detailDishTable = new DataGridView();

			detailDishTable.Top = 0;
			detailDishTable.Left = width / 2;
			detailDishTable.ReadOnly = true;
			detailDishTable.AllowUserToAddRows = false;
			detailDishTable.Height = height / 2;
			detailDishTable.Width = width;
			detailDishTable.RowHeadersVisible = false;
			detailDishTable.ScrollBars = ScrollBars.Vertical;
			detailDishTable.BackgroundColor = Color.White;
			detailDishTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			detailDishTable.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

			for (int j = 0; j < countColumns; j++)
			{
				DataGridViewColumn column = new DataGridViewTextBoxColumn();
				if(j == countColumns - 1)
					column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				else
					column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			
				column.Name = columns[j].name;
				column.HeaderText = columns[j].header;
				detailDishTable.Columns.Add(column);
			}

			Dictionary<string, string> rows = new Dictionary<string, string>();
			rows.Add("Тип, -", dish.type);
			rows.Add("Вес, грамм", dish.weight.ToString());
			rows.Add("Белки, грамм", dish.proteins.ToString());
			rows.Add("Жиры, грамм", dish.fats.ToString());
			rows.Add("Углеводы, грамм", dish.carbohydrates.ToString());
			rows.Add("Энергетическая ценность, кКал", dish.energy.ToString());
			rows.Add("Стоимость, руб. за 100г.", dish.cost.ToString());

			int countRows = rows.Count();
			int rowHeight = height / 2 / (countRows + 1);  // get dish table headers + params height
			for (int i = 0; i < countRows; i++)
			{
				var _row = rows.ElementAt(i);
				var _key = _row.Key.Split(',');
				var key = _key[0];
				var unit = _key[1];

				int rowIndex = detailDishTable.Rows.Add();
				var row = detailDishTable.Rows[rowIndex];
				row.Height = rowHeight;
				row.SetValues(new object[] { key, unit, _row.Value });
			}
			detailDishTable.ColumnHeadersHeight = rowHeight; // fix dish table headers  height
															 // fix dish table params height
			detailDishTable.Height = detailDishTable.Rows.GetRowsHeight(DataGridViewElementStates.Displayed) + rowHeight;

			// crate label for dish name
			Label name = new Label();
			name.Text = dish.name;
			name.Location = new Point(
				10,
				height / 2 + 10
			);
			name.Font = new Font("Calibri", 24, FontStyle.Bold);
			name.Height = name.Font.Height;

			// images exist
			if (dish.images.Any())
			{
				PictureBox image = new PictureBox();
				image.Image = dish.images[0];
				image.Height = height / 2;
				image.Width = width / 2;
				image.Left = image.Top = 0;
				image.Click += (s, e) => {
					showDetailImage(dish.images[0], dish.name);
				};
				image.SizeMode = PictureBoxSizeMode.StretchImage;

				mainTab.Controls.Add(image);
				int countDishImages = dish.images.Count();
				if (countDishImages > 1)
				{
					Gallery gallery = new Gallery(dish.images, width, height);

					imageTab = new TabPage("Картинки");
					imageTab.Controls.Add(gallery);
					imageTab.BackColor = Color.White;
				}
				// no images
			}
			else
			{
				// set default image from app resources
				PictureBox image = new PictureBox();

				image.Image = Util.defaultImage();
				image.Height = height / 2;
				image.Width = width / 2;
				image.Left = image.Top = 0;
				image.SizeMode = PictureBoxSizeMode.CenterImage;

				mainTab.Controls.Add(image);
			}

			mainTab.BackColor = Color.White;
			mainTab.Controls.Add(name);
			mainTab.Controls.Add(detailDishTable);
			this.dishDetail.TabPages.Add(mainTab);
			if (imageTab != null)
				this.dishDetail.TabPages.Add(imageTab);

			selectedDish = dish;
			this.Text = dish.name;
			setCountOfSelectedDish(1);
		}

		private void setCountOfSelectedDish(int count)
		{
			countOfDetailDish.Text = count.ToString();
		}

		private int getCountOfSelectedDish()
		{
			return Convert.ToInt32(countOfDetailDish.Text);
		}

		private void addToOrder_Click(object sender, EventArgs e)
		{
			int count = getCountOfSelectedDish();
			if(MainForm.orderForm == null) {
				MainForm.orderForm = new orderForm();
				MainForm.orderForm.Show();
			}

			orderForm.addDishToOrder(selectedDish.id, selectedDish.name, selectedDish.cost, count);
		}

		private void dishDetailForm_Load(object sender, EventArgs e)
		{

		}

		private void btnDecCountDish_Click(object sender, EventArgs e)
		{
			int count = getCountOfSelectedDish();
			if (count > 1)
				count--;
			countOfDetailDish.Text = count.ToString();
		}

		/**
		 * Show full size images of dishes
		 * */
		public static void showDetailImage(Image img, string dishName)
		{
			if (img != null)
			{
				MainForm.detailImageWindow.Hide();
				PictureBox image = new PictureBox();

				// settings full size image of dish
				image.Image = img;
				image.Height = img.Height;
				image.Width = img.Width;
				image.Left = image.Top = 0;

				// settings window
				MainForm.detailImageWindow.Height = image.Height;
				MainForm.detailImageWindow.Width = image.Width;
				MainForm.detailImageWindow.StartPosition = FormStartPosition.CenterScreen;
				MainForm.detailImageWindow.Text = dishName;
				MainForm.detailImageWindow.FormBorderStyle = FormBorderStyle.FixedToolWindow;

				// join image and window
				MainForm.detailImageWindow.Controls.Add(image);
				MainForm.detailImageWindow.Show();
			}
		}

		private void btnIncCountDish_Click(object sender, EventArgs e)
		{
			int count = getCountOfSelectedDish();
			if (count < 100)
				count++;
			setCountOfSelectedDish(getCountOfSelectedDish() + 1);
		}
	}
}
