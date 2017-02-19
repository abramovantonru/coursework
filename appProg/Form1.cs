using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace appProg
{
	public partial class Form1 : Form
	{
		private List<string> menuSections;
		private DataGridView[] menuTables;

		int height = Screen.PrimaryScreen.Bounds.Height;
		int width = Screen.PrimaryScreen.Bounds.Width;

		public Form1()
		{
			InitializeComponent();

			/**
			 * Form settings
			 * */

			// left part window = cafe's menu
			this.menu.Width = width / 2 - 60;
			this.menu.Height = height - 40 - 20;
			this.menu.Location = new Point(
				20,
				40
			);

			//exit btn
			this.btnAppClose.Location = new Point(
				width - btnAppClose.Width - 20,
				15
			);

			//detail dish wrapper
			this.dishDetail.Width = width / 2 - 30;
			this.dishDetail.Height = height / 2;
			this.dishDetail.Location = new Point(
				width / 2,
				40
			);

			//TODO set properties for search
			/**
			 * Form settings
			 * */

			/**
			 * Get data from DB
			 * */

			menuSections = DB.getMenuSections(); // get all exist sections
			createTabs(menuSections); // create tabs by found sections
			
			loadTabDishes(0); // fill data to first tab

			/**
			 * Get data from DB
			 * */
		}

		/**
		 * On load main window
		 * */
		private void onLoad(object sender, EventArgs e)
		{
			/**
			 * Hide windows's borders and resize to fullscreen
			 * */
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			Left = Top = 0;
			Width = Screen.PrimaryScreen.Bounds.Width;
			Height = Screen.PrimaryScreen.Bounds.Height;
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
				new menuColumn { name = "name", header = "Название", width = menuWidth / 100 * 65  - 40},
				new menuColumn { name = "weight", header = "Вес, грамм", width = menuWidth / 100 * 15 },
				new menuColumn { name = "cost", header = "Стоимость, руб.", width = menuWidth / 100 * 15 },
			};
			int countColumns = columns.Count();
			
			for (int i = 0; i < countSections; i++)
			{

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

				for (int j = 0; j < countColumns; j++)
				{
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
				showDetailDish(id);
			}
		}

		/**
		 * Load data about dish by ID and fill to detail dish block
		 * */
		private void showDetailDish(int id) {
			detailDish dish = DB.getDishByID(id);

			MessageBox.Show("DISH NAME = " + dish.name);
		}

		/**
		 * Change tab index in menu
		 * */
		private void menu_SelectedIndexChanged(object sender, EventArgs e)
		{
			int tab = this.menu.SelectedIndex;
			loadTabDishes(tab);
		}
	}
	/**
	 * Class for work with Mysql Data Base;
	 * 
	 * class DBResult - get response data from DB
	 * 
	 * */
	public class DB
	{
		private static string database = "cafe";
		private static string host = "127.0.0.1";
		private static string user = "mysql";
		private static string password = "mysql";
		private MySqlConnection connection;

		public class DBResult
		{
			public bool success = false;
			public string error = "Неизвестная ошибка.";
			public List<List<Object>> data = new List<List<Object>>();
		}

		public static List<string> getMenuSections()
		{
			DB db = new DB();
			List<string> sections = new List<string>();
			DBResult result = db.exec("SELECT type FROM dish ORDER BY CAST(type AS CHAR)");

			if (result.success)
			{
				int rows = result.data.Count();
				for(int i = 0; i < rows; i++)
					sections.Add(result.data[i][0].ToString());
			}

			return sections;
		}

		public static prewiewDish getTabDishesByName(string sectionName)
		{
			DB db = new DB();
			prewiewDish dishes = new prewiewDish();
			DBResult result = db.exec("SELECT id, name, weight, cost FROM dish WHERE type = '" + sectionName + "';");

			if (result.success)
			{
				int rows = result.data.Count();
				for (int i = 0; i < rows; i++)
				{
					var row = result.data[i];

					var id = (int)row[0];
					var name = row[1].ToString();
					var weight = (float)row[2];
					var cost = (float)row[3];

					dishes.Add(new prewiewDish { id = id, name = name, weight = weight, cost = cost });
				}
			}
			return dishes;
		}

		public static detailDish getDishByID(int id)
		{
			DB db = new DB();
			DBResult result = db.exec("SELECT * FROM dish WHERE id = '" + id + "' LIMIT 1;");
			detailDish dish = null;

			if (result.success)
			{
				var row = result.data[0];
				var name = row[1].ToString();

				dish = new detailDish { id = id, name = name };
			}
			return dish;
		}


		public void connect()
		{
			try
			{
				connection = this.create();
			}
			catch (MySqlException e)
			{
				Console.WriteLine(e);
				MessageBox.Show("Не удалось создать подключение к базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}
		}

		private void open()
		{
			try
			{
				connection.Open();
			}
			catch (MySqlException e)
			{
				Console.WriteLine(e);
				MessageBox.Show("Не удалось открыть подключение к базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}
		}

		public void disconnect()
		{
			connection.Close();
		}

		private MySqlConnection create()
		{
			string connectionData =
				"database=" + database +
				";server=" + host +
				";uid=" + user +
				";pwd=" + password + ";";
			return new MySqlConnection(connectionData);
		}

		private DBResult exec(string SQL)
		{
			DBResult result = new DBResult();
			try
			{
				this.connect();

				MySqlCommand query = new MySqlCommand(SQL, connection);
				this.open();

				MySqlDataReader reader = query.ExecuteReader();
				if (reader.HasRows)
				{
					result.success = true;
					int row = 0;
					while (reader.Read())
					{
						result.data.Add(new List<Object>());
						Object[] values = new Object[reader.FieldCount];
						int fieldCount = reader.GetValues(values);
						for (int i = 0; i < fieldCount; i++)
							result.data[row].Add(values[i]);
						row++;
						
					}
					reader.Close();
				}
				else
				{
					result.error = "По запросу получено 0 записей.";
				}
				this.disconnect();
			}
			catch (MySqlException e)
			{
				Console.WriteLine(e);
				MessageBox.Show("Не удалось выполнить запрос к базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}
			catch (InvalidOperationException e)
			{
				Console.WriteLine(e);
				MessageBox.Show("Не известная ошибка при исполнении запроса.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}

			return result;
		}
	}

	/**
	 * Dish class for detail view
	 * */
	public class detailDish
	{
		public int id;
		public string name;
		public float proteins;
		public float fats;
		public float carbohydrates;
		public float weight;
		public float cost;
	};

	/**
	 * Dish class for preview
	 * */
	public class prewiewDish : List<Object>
	{
		public int id;
		public string name;
		public float weight;
		public float cost;
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
