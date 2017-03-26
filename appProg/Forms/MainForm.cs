

using cafeMenu.Forms;
using cafeMenu.Other;
/**
* Abramov Anton (c) 2017
* abramovanton@ya.ru
* 
* Application for menu of cafe
* Features:
* - Show menu
* - Show detail dish
* - Create new order
* - Load exist order
* - Save order
* - Remove order
* */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace cafeMenu
{
	public partial class MainForm : Form
	{
		private List<string> menuSections; // tabs of menu
		private DataGridView[] menuTables; // tables of menu
		private static AboutForm about; // about programm form

		public static List<int> selectedDishes = new List<int>();
		public static orderForm orderForm;

		public static Form detailImageWindow;

		/**
		 * Constructor of main window
		 * */
		public MainForm()
		{
			InitializeComponent();

			/**
			 * Get data from DB
			 * */
			menuSections = DB.getMenuSections(); // get all exist sections
			createTabs(menuSections); // create tabs by found sections
			loadTabDishes(0); // fill data to first tab
		}

		/**
		 * Func(wrapper) for load data from DB and fill to menu
		 * */
		private void loadTabDishes(int index)
		{
			prewiewDish dishes = DB.getTabDishesByName(menuSections[index]);
			menuTables[index].Rows.Clear();
			foreach (prewiewDish dish in dishes)
				menuTables[index].Rows.Add(new object[] { dish.id, dish.name, dish.weight, dish.cost });
		}
		/**
		 * Func for create tab on menu
		 * */
		private void createTabs(List<string> sections)
		{
			int menuHeight = this.menu.Height;
			int menuWidth = this.menu.Width;
			int countSections = sections.Count();
			menuTables = new DataGridView[sections.Count];

			menuColumn[] columns = {
				new menuColumn { name = "id", header = "ИД", width = menuWidth / 100 * 5 },
				new menuColumn { name = "name", header = "Название", width = menuWidth / 100 * 65},
				new menuColumn { name = "weight", header = "Вес, грамм", width = menuWidth / 100 * 15 },
				new menuColumn { name = "cost", header = "Стоимость, руб.", width = menuWidth / 100 * 15 },
			};
			int countColumns = columns.Count();

			for (int i = 0; i < countSections; i++){
				TabPage myTabPage = new TabPage(sections[i]);
				menuTables[i] = new DataGridView();
				menuTables[i].Left = menuTables[i].Top = 20;
				menuTables[i].Height = menuHeight - 60;
				menuTables[i].Width = menuWidth - 40;
				menuTables[i].RowHeadersVisible = false;
				menuTables[i].ScrollBars = ScrollBars.Vertical;
				menuTables[i].ReadOnly = true;
				menuTables[i].AllowUserToAddRows = false;
				menuTables[i].CellClick += new DataGridViewCellEventHandler(this.menuTableCell_Click);
				menuTables[i].SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				menuTables[i].RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
				menuTables[i].ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

				for (int j = 0; j < countColumns; j++){
					DataGridViewColumn column = new DataGridViewTextBoxColumn();
					column.Width = columns[j].width;
					column.Name = columns[j].name;
					column.HeaderText = columns[j].header;
					menuTables[i].Columns.Add(column);
				}

				myTabPage.Controls.Add(menuTables[i]);
				this.menu.TabPages.Add(myTabPage);
			}
		}
		/**
		 * Button for close application
		 * */
		private void btnAppClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		/**
		 * Select dish in menu(dish list)
		 * */
		private void menuTableCell_Click(object sender, DataGridViewCellEventArgs e)
		{
			int row = e.RowIndex;
			if (row > -1) {
				var tab = menu.SelectedIndex;
				int id = (int)menuTables[tab].Rows[row].Cells[0].Value;
				if (id > 0)
					if (!selectedDishes.Any() || selectedDishes.IndexOf(id) == -1)
						showDetailDish(id);
			}
		}
		/**
		 * Load data about dish by ID and fill to detail dish block
		 * */
		public static void showDetailDish(int id) {
			Form dishDetailForm = new dishDetailForm(id);
			selectedDishes.Add(id);
			dishDetailForm.FormClosed += (s, ev) => {
				int count = selectedDishes.Count;
				for (int i = 0; i < count; i++)
					if (selectedDishes[i] == id)
						selectedDishes.RemoveAt(i);
				
			};
			dishDetailForm.Show();
		}
		/**
		 * Change tab index in menu
		 * */
		private void menu_SelectedIndexChanged(object sender, EventArgs e)
		{
			int tab = this.menu.SelectedIndex;
			loadTabDishes(tab);
		}

		private void menuStrip_closeApp_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void menuStrip_order_Click(object sender, EventArgs e)
		{
			if(orderForm == null) {
				orderForm = new orderForm();
				orderForm.FormClosed += (s, ev) => { orderForm = null; };
				orderForm.Show();
			}else {
				orderForm.Focus();
			}
		}

		private void stripMenu_exportByDates_Click(object sender, EventArgs e)
		{
			if (Security.isAdmin())
			{
				Form openOrderDialog = new Form();
				DateTimePicker fromPicker = new DateTimePicker();
				DateTimePicker toPicker = new DateTimePicker();
				Button OK = new Button();
				Button Cancel = new Button();

				int wh = 200;
				openOrderDialog.Width = openOrderDialog.Height = wh;
				openOrderDialog.Text = "Сгенерировать отчет по датам";
				openOrderDialog.StartPosition = FormStartPosition.CenterScreen;
				openOrderDialog.ControlBox = false;
				openOrderDialog.FormBorderStyle = FormBorderStyle.FixedSingle;
				openOrderDialog.Icon = Properties.Resources.coffee_cup;

				OK.Width = 100;
				Cancel.Width = 60;
				OK.Location = new Point(
					wh / 2 - OK.Width / 2,
					wh / 2
				);
				//event for ok button
				OK.Click += (s, ev) => {
					string from = fromPicker.Value.ToShortDateString();
					string to = toPicker.Value.ToShortDateString();

					PDFGenerator pdf = new PDFGenerator();
					pdf.ordersByDates(from, to);
				};
				OK.Text = "Сгенерировать";

				Cancel.DialogResult = DialogResult.Cancel;
				Cancel.Location = new Point(
					wh / 2 - Cancel.Width / 2,
					wh / 2 + OK.PreferredSize.Height + 10
				);
				//event for cancel button
				Cancel.Click += (s, ev) => {
					openOrderDialog.Close();
				};
				Cancel.Text = "Отмена";

				fromPicker.Width = toPicker.Width = wh / 2;
				fromPicker.Format = toPicker.Format = DateTimePickerFormat.Short;
				fromPicker.Location = new Point(
					wh / 2 - fromPicker.Width / 2,
					wh / 2 - fromPicker.Height * 4
				);
				toPicker.Location = new Point(
					wh / 2 - toPicker.Width / 2,
					wh / 2 - toPicker.Height * 2
				);

				openOrderDialog.Controls.AddRange(new Control[] { OK, Cancel, fromPicker, toPicker });
				openOrderDialog.Show();
			}
		}

		private void menuStrip_exportByID_Click(object sender, EventArgs e)
		{
			if(Security.isAdmin()) {
				Form openOrderDialog = new Form();
				TextBox input = new TextBox();
				Button OK = new Button();
				Button Cancel = new Button();
				Label info = new Label();

				int wh = 200;
				openOrderDialog.Width = openOrderDialog.Height = wh;
				openOrderDialog.Text = "Сгенерировать отчет по номеру";
				openOrderDialog.StartPosition = FormStartPosition.CenterScreen;
				openOrderDialog.ControlBox = false;
				openOrderDialog.FormBorderStyle = FormBorderStyle.FixedSingle;

				info.Text = "Введите номер заказа";
				info.Width = wh;
				info.Location = new Point(
					wh / 2 - info.PreferredWidth / 2,
					info.Height
				);

				OK.Width = 100;
				Cancel.Width = 60;
				OK.Location = new Point(
					wh / 2 - OK.Width / 2,
					wh / 2
				);
				//event for ok button
				OK.Click += (s, ev) => {
					int id = Convert.ToInt32(input.Text);
					if (DB.existOrderByID(id))
					{
						PDFGenerator pdf = new PDFGenerator();
						pdf.orderByID(id);
					}
					else
					{
						MessageBox.Show("Заказ с данным номером (" + id + ") не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				};
				OK.Text = "Сгенерировать";

				Cancel.DialogResult = DialogResult.Cancel;
				Cancel.Location = new Point(
					wh / 2 - Cancel.Width / 2,
					wh / 2 + OK.PreferredSize.Height + 10
				);
				//event for cancel button
				Cancel.Click += (s, ev) => {
					openOrderDialog.Close();
				};
				Cancel.Text = "Отмена";

				input.Location = new Point(
					wh / 2 - input.Width / 2,
					wh / 2 - input.Height * 2
				);
				input.KeyPress += Util.keyPress_onlyInt;

				openOrderDialog.Controls.AddRange(new Control[] { OK, Cancel, input, info });
				openOrderDialog.Show();
			}
		}

		private void about_app_Click(object sender, EventArgs e)
		{
			if (about == null)
			{
				about = new AboutForm();
				about.FormClosed += (s, ev) => { about = null; };
				about.Show();
			}
			else
				about.Focus();
		}
	}

	/**
	 * Class-wrapper for tabs control
	 * */
	public class menuColumn
	{
		public string name;
		public string header;
		public int width;
	}
}

