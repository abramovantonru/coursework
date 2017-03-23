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
using appProg.Properties;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace appProg
{
	public partial class MainForm : Form
	{
		private List<string> menuSections; // tabs of menu
		private DataGridView[] menuTables; // tables of menu

		public static List<int> selectedDishes = new List<int>();
		public static orderForm orderForm;

		public static Form detailImageWindow = new Form();

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
			orderForm = new orderForm();
			orderForm.Show();
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

