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
	public partial class Form1 : Form
	{
		private List<string> menuSections; // tabs of menu
		private DataGridView[] menuTables; // tables of menu

		private detailDish selectedDish;

		private List<orderDish> orders = new List<orderDish>(); // opened orders
		DataGridView[] orderTables;

		public static Form detailImageWindow; // window for view detail image of dish
		public static Form openOrderDialog;
		public static Form removeOrderDialog;

		int height = Screen.PrimaryScreen.Bounds.Height;
		int width = Screen.PrimaryScreen.Bounds.Width;

		/**
		 * Constructor of main window
		 * */
		public Form1()
		{
			InitializeComponent();
			/**
			 * Form settings
			 * */
			// left part window = cafe's menu
			this.menu.Width = width / 2 - 60;
			this.menu.Height = height - 60;
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
			this.dishDetail.Height = height / 2 - 40;
			this.dishDetail.Location = new Point(
				width / 2,
				40
			);

			//dish controls
			this.btnAddToOrder.Location = new Point(
				width / 2,
				15
			);
			this.countOfDetailDish.Height = 50;
			this.countOfDetailDish.Location = new Point(
				this.btnAddToOrder.Bounds.Right + 10,
				15
			);
			this.btnIncCountDish.Location = new Point(
				this.countOfDetailDish.Bounds.Right + 10,
				15
			);
			this.btnDecCountDish.Location = new Point(
				this.btnIncCountDish.Bounds.Right + 2,
				15
			);


			//order wrapper
			this.order.Width = width / 2 - 30;
			this.order.Height = height / 2 - 60;
			this.order.Location = new Point(
				width / 2,
				height / 2 + 40
			);

			//order buttons
			this.btnOpenOrder.Location = new Point(
				width / 2,
				height / 2 + 10
			);
			this.btnCreateOrder.Location = new Point(
				btnOpenOrder.Bounds.Right + 10,
				height / 2 + 10
			);
			this.btnSaveOrder.Location = new Point(
				btnCreateOrder.Bounds.Right + 10,
				height / 2 + 10
			);
			this.btnRemoveOrder.Location = new Point(
				btnSaveOrder.Bounds.Right + 10,
				height / 2 + 10
			);
			this.btnCloseOrder.Location = new Point(
				btnRemoveOrder.Bounds.Right + 10,
				height / 2 + 10
			);

			//TODO set properties for search

			/**
			 * Get data from DB
			 * */
			menuSections = DB.getMenuSections(); // get all exist sections
			createTabs(menuSections); // create tabs by found sections
			
			loadTabDishes(0); // fill data to first tab

		}
		/**
		 * On load main window
		 * */
		private void onLoad(object sender, EventArgs e)
		{
			/**
			 * Hide windows's borders and resize to fullscreen
			 * */
			FormBorderStyle = FormBorderStyle.None;
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
				if (id > 0)
					if(selectedDish == null || selectedDish.id != id)
						showDetailDish(id);
			}
		}
		/**
		 * Load data about dish by ID and fill to detail dish block
		 * */
		private void showDetailDish(int id) {
			this.dishDetail.TabPages.Clear();

			detailDish dish = DB.getDishByID(id);

			int width = dishDetail.Width;
			int height = dishDetail.Height;

			List<string> tabs = new List<string>{ "Основная информация", "Картинки" };
			menuColumn[] columns = {
				new menuColumn { name = "key", header = "Параметр", width = width / 200 * 60 },
				new menuColumn { name = "unit", header = "Единица", width = width / 200 * 30},
				new menuColumn { name = "value", header = "Значение", width = width / 200 * 30},
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

			for (int j = 0; j < countColumns; j++)
			{
				DataGridViewColumn column = new DataGridViewTextBoxColumn();
				column.Width = columns[j].width;
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
			for (int i = 0; i < countRows; i++) {
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
			if(dish.images.Any()) {
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
				if (countDishImages > 1) {				
					Gallery gallery = new Gallery(dish.images, width, height);
				
					imageTab = new TabPage("Картинки");
					imageTab.Controls.Add(gallery);
					imageTab.BackColor = Color.White;
				}
			// no images
			}else {
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
			if(imageTab != null)
				this.dishDetail.TabPages.Add(imageTab);
			if (selectedDish == null)
			{
				btnAddToOrder.Enabled = true;
				countOfDetailDish.Enabled = true;
				btnIncCountDish.Enabled = true;
				btnDecCountDish.Enabled = true;
			}
			selectedDish = dish;
			setCountOfSelectedDish(1);
		}
		/**
		 * Change tab index in menu
		 * */
		private void menu_SelectedIndexChanged(object sender, EventArgs e)
		{
			int tab = this.menu.SelectedIndex;
			loadTabDishes(tab);
		}
		/**
		 * Show full size images of dishes
		 * */
		public static void showDetailImage(Image img, string dishName)
		{
			if(img != null) {
				detailImageWindow = new Form();
				PictureBox image = new PictureBox();

				// settings full size image of dish
				image.Image = img;
				image.Height = img.Height;
				image.Width = img.Width;
				image.Left = image.Top = 0;

				// settings window
				detailImageWindow.Height = image.Height;
				detailImageWindow.Width = image.Width;
				detailImageWindow.StartPosition = FormStartPosition.CenterScreen;
				detailImageWindow.Text = dishName;

				// join image and window
				detailImageWindow.Controls.Add(image);
				detailImageWindow.Show();
			}
		}
		/**
		 * Order logic
		 * */
		 //event handler for create order
		private void createOrder_Click(object sender, EventArgs e)
		{
			int idx = createOrder();
			loadOrdersTabs();
			order.SelectedIndex = idx;
		}
		//create(append) order in global MainWindow var "order" [optional by exist orderDish object]
		private int createOrder(orderDish order = null)
		{
			int id = orders.Count;
			if (order == null)
			{
				order = new orderDish();
				order.name = "Новый заказ";
				order.id = DB.getLastOrderID() + 1;
			}

			orders.Add(order);
			return id;
		}
		//event handler for save order
		private void saveOrder_Click(object sender, EventArgs e)
		{
			int index = order.SelectedIndex;
			if (index > -1) {
				int id = saveOrder(index);
				if (id == 0) 
					MessageBox.Show("Заказ успешно сохранен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else if(id == -1)
					MessageBox.Show("Не удалось сохранить заказ!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else if(id > -1) {
					MessageBox.Show("Заказ успешно сохранен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
					orders[index].name = "Заказ №" + id;
					loadOrdersTabs();
				}
			}
		}
		//save order from global MainWindow var "order" by index to data base
		private int saveOrder(int index) {
			int id = -1;
			int orderID = orderIDbyIdx(index);

			if(orderID != -1) {
				string json = getSerializeOrderItems(index);
				if(json != "[]")
					if (!DB.existOrderByID(orderID))
						id = DB.insertOrder(json);
					else
						if (DB.updateOrder(orderID, json))
							id = 0;
						else
							id = -1;
				else MessageBox.Show("Невозможно сохранить пустой заказ!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return id;
		}

		private void removeOrder_Click(object sender, EventArgs e)
		{
			int index = order.SelectedIndex;
			if(index > -1 && orders[index] != null) {
				if (orders[index].name == "Новый заказ")
					orders.RemoveAt(index);
				else {
					int id = orders[index].id;
					if (DB.existOrderByID(id))
						showRemoveOrderDialog(id);
					else
						MessageBox.Show("Заказ с данным номером (" + id + ") не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				order.TabPages.Clear();
				loadOrdersTabs();
			}
		}

		private void closeOrder_Click(object sender, EventArgs e)
		{
			int index = order.SelectedIndex;
			if(index != -1) {
				orders.RemoveAt(index);
				loadOrdersTabs();

				int newIndex = index - 1;
				if (newIndex < 0)
					newIndex = 0;
				order.SelectedIndex = newIndex;
			}
		}

		private int findOrderDishIdxByDishID(int orderIdx, int id) {
			int idx = -1;
			
			if(orderIdx != -1) {
				int count = orders[orderIdx].dishes.Count;
				for (int i = 0; i < count; i++){
					if (orders[orderIdx].dishes[i].id == id){
						idx = i;
						break;
					}
				}
			}

			return idx;
		}

		private int findOrderIdxByOrderID(int id) {
			int idx = -1;
			int count = orders.Count;
			
			for (int i = 0; i < count; i++){
				if (orders[i].id == id){
					idx = i;
					break;
				}
			}
		
			return idx;
		}

		private int findOrderIdxByDishID(int id){
			int idx = -1;
			int countOrder = orders.Count;
		
			for (int i = 0; i < countOrder; i++){
				int countDishes = orders[i].dishes.Count;
				for (int j = 0; j < countDishes; j++){
					if (orders[i].dishes[j].id == id){
						idx = i;
						break;
					}
				}
			}

			return idx;
		}

		private int orderIDbyIdx(int idx) {
			int id = -1;

			if (idx < orders.Count && orders[idx] != null)
				id = orders[idx].id;

			return id;
		}

		private void addDishToOrder(int id, string name, float cost, int count, int orderIdx = -1) {
			if(orderIdx == -1) {
				orderIdx = createOrder();
				orders[orderIdx].dishes.Add(new dishInOrder { id = id, name = name, cost = cost, count = count });
				loadOrdersTabs();
			}else {
				int dishIdx = findOrderDishIdxByDishID(orderIdx, id);
				if (dishIdx == -1)
					orders[orderIdx].dishes.Add(new dishInOrder { id = id, name = name, cost = cost, count = count });
				else
					orders[orderIdx].dishes[dishIdx].count += count;
				int selectedIdx = order.SelectedIndex;
				loadOrderTabDishes(selectedIdx);
			}
		}

		private void loadOrderTabDishes(int idx){
			if (idx != -1 && orders.Any())
			{
				orderTables[idx].Rows.Clear();
				foreach (var dish in orders[idx].dishes)
				{
					orderTables[idx].Rows.Add(new object[] { dish.id, dish.name, dish.cost, dish.count });
				}
			}
		}

		private void loadOrdersTabs() {
			order.TabPages.Clear();
			int orderHeight = this.order.Height;
			int orderWidth = this.order.Width;
			int countOrders = orders.Count;
			orderTables = new DataGridView[countOrders];

			menuColumn[] columns = {
				new menuColumn { name = "id", header = "ИД", width = orderWidth / 100 * 5 },
				new menuColumn { name = "name", header = "Название", width = orderWidth / 100 * 65},
				new menuColumn { name = "cost", header = "Стоимость, руб.", width = orderWidth / 100 * 15 },
				new menuColumn { name = "count", header = "Количество, шт.", width = orderWidth / 100 * 20 },
			};
			int countColumns = columns.Count();

			for (int i = 0; i < countOrders; i++)
			{
				TabPage tabPage = new TabPage(orders[i].name);
				orderTables[i] = new DataGridView();
				orderTables[i].Left = orderTables[i].Top = 0;
				orderTables[i].Height = orderHeight;
				orderTables[i].Width = orderWidth;
				orderTables[i].RowHeadersVisible = false;
				orderTables[i].ScrollBars = ScrollBars.Vertical;
				orderTables[i].ReadOnly = true;
				orderTables[i].AllowUserToAddRows = false;
				orderTables[i].CellClick += new DataGridViewCellEventHandler(this.orderTableCell_Click);

				for (int j = 0; j < countColumns; j++)
				{
					DataGridViewColumn column = new DataGridViewTextBoxColumn();
					column.Width = columns[j].width;
					column.Name = columns[j].name;
					column.HeaderText = columns[j].header;
					orderTables[i].Columns.Add(column);
				}

				tabPage.Controls.Add(orderTables[i]);
				this.order.TabPages.Add(tabPage);
			}

			if(orders.Any()) {
				for(int i = 0; i < countOrders; i++) {
					loadOrderTabDishes(i);
				}
			}			
		}

		private void orderTableCell_Click(object sender, DataGridViewCellEventArgs e)
		{
			int row = e.RowIndex;
			if (row > -1)
			{
				var tab = order.SelectedIndex;
				int id = (int)orderTables[tab].Rows[row].Cells[0].Value;
				if (id > 0)
					if (selectedDish == null || selectedDish.id != id)
						showDetailDish(id);
			}
		}
		
		private void addToOrder_Click(object sender, EventArgs e)
		{
			int count = Convert.ToInt32(countOfDetailDish.Text);
			addDishToOrder(selectedDish.id, selectedDish.name, selectedDish.cost, count, order.SelectedIndex);
		}

		private void DecCountDish_Click(object sender, EventArgs e)
		{
			int count = getCountOfSelectedDish();
			if (count > 1)
				count--;
			countOfDetailDish.Text = count.ToString();
		}

		private void IncCountDish_Click(object sender, EventArgs e)
		{
			int count = getCountOfSelectedDish();
			if(count < 100)
				count++;
			setCountOfSelectedDish(getCountOfSelectedDish() + 1);
		}

		private void setCountOfSelectedDish(int count) {
			countOfDetailDish.Text = count.ToString();
		}

		private int getCountOfSelectedDish() {
			return Convert.ToInt32(countOfDetailDish.Text);
		}

		/**
		 * Ooops..
		 * */
		private string getSerializeOrderItems(int orderIdx) {
			List<string> _dish = new List<string>();
			string json = "";

			foreach (var dish in orders[orderIdx].dishes)
				_dish.Add(JsonConvert.SerializeObject(new Dictionary<int, int>() { { dish.id, dish.count } }));

			if(_dish.Any())
				json += String.Join(", ", _dish);
			/*
				List<Foo> arr = new List<Foo>(dict.Values);
				Foo[] arr = (new List<Foo>(dict.Values)).ToArray();
			 */
			return "[" + json + "]";
		}

		private void openOrder_Click(object sender, EventArgs e)
		{
			openOrderDialog = new Form();
			TextBox input = new TextBox();
			Button OK = new Button();
			Button Cancel = new Button();
			Label info = new Label();

			int wh = 200;
			openOrderDialog.Width = openOrderDialog.Height = wh;
			openOrderDialog.Text = "Открыть заказ по номеру";
			openOrderDialog.StartPosition = FormStartPosition.CenterScreen;
			openOrderDialog.ControlBox = false;
			openOrderDialog.FormBorderStyle = FormBorderStyle.FixedSingle;

			info.Text = "Введите номер заказа";
			info.Width = wh;
			info.Location = new Point(
				wh / 2 - info.PreferredWidth / 2,
				info.Height
			);

			OK.Width = Cancel.Width = 60;
			OK.Location = new Point(
				wh / 2 - OK.Width - 10,
				wh / 2
			);
			//event for ok button
			OK.Click += (s, ev) => {
				int id = Convert.ToInt32(input.Text);
				if (DB.existOrderByID(id)) {
					if(findOrderIdxByOrderID(id) == -1) {
						int selectedIdx = openOrder(id);
						openOrderDialog.Close();
						loadOrdersTabs();
						if (selectedIdx != -1)
							order.SelectedIndex = selectedIdx;
					}else {
						MessageBox.Show("Заказ с данным номером (" + id + ") уже открыт.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else {
					MessageBox.Show("Заказ с данным номером (" + id + ") не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			};
			OK.Text = "Открыть";

			Cancel.DialogResult = DialogResult.Cancel;
			Cancel.Location = new Point(
				wh / 2 + 10,
				wh / 2
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
		private int openOrder(int id)
		{
			orderDish order = new orderDish();
			order = DB.getOrderByID(id);
			if (order != null) {
				createOrder(order);
				return findOrderIdxByOrderID(id);
			}
			else {
				MessageBox.Show("Неудалось получить информацию о заказе!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return -1;
		}

		private void showRemoveOrderDialog(int id) {
			removeOrderDialog = new Form();
			TextBox input = new TextBox();
			Button OK = new Button();
			Button Cancel = new Button();
			Label info = new Label();

			int w = 300;
			int h = 200;
			removeOrderDialog.Width = w;
			removeOrderDialog.Height = h;
			removeOrderDialog.Text = "Удаление заказа №" + id;
			removeOrderDialog.StartPosition = FormStartPosition.CenterScreen;
			removeOrderDialog.ControlBox = false;
			removeOrderDialog.FormBorderStyle = FormBorderStyle.FixedSingle;

			info.Text = "Вы уверены, что хотите удалить заказ №" + id + " ?";
			info.Width = w;
			info.Location = new Point(
				w / 2 - info.PreferredWidth / 2,
				h /2 - info.Height * 2
			);

			OK.Width = Cancel.Width = 60;
			OK.Location = new Point(
				w / 2 - OK.Width - 10,
				h / 2
			);
			//event for ok button
			OK.Click += (s, ev) => {
				int idx = findOrderIdxByOrderID(id);
				if (idx != -1)
				{
					if (DB.removeOrderByID(id)) {
						orders.RemoveAt(idx);
						MessageBox.Show("Заказ с данным номером (" + id + ") успешно удален.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
						loadOrdersTabs();
					}	
					else MessageBox.Show("Не удалось удалить заказ №" + id + "!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else MessageBox.Show("Заказ с данным номером (" + id + ") не найден.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
				removeOrderDialog.Close();
			};
			OK.Text = "Удалить";

			Cancel.DialogResult = DialogResult.Cancel;
			Cancel.Location = new Point(
				w / 2 + 10,
				h / 2
			);
			//event for cancel button
			Cancel.Click += (s, ev) => {
				removeOrderDialog.Close();
			};
			Cancel.Text = "Отмена";

			removeOrderDialog.Controls.AddRange(new Control[] { OK, Cancel, info });
			removeOrderDialog.Show();
		}
	}
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

		/**
		 * Constructor 
		 * */
		public Gallery(List<Image> images, int _width, int _height, int _count = 0)
		{
			// default max count on line
			if (_count != 0)
				maxCount = _count;
			width = _width - margin * 2 - padding * maxCount;
			imgWidth = width / maxCount;
			xPos = yPos = margin;

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
			if (onLine == maxCount) {
				onLine = 0;
				xPos = margin;
				yPos += imgWidth + padding; 
			}
			
			//image settings
			image.Image = img;
			image.Width = image.Height = imgWidth;
			image.SizeMode = PictureBoxSizeMode.StretchImage;
			image.Click += (s, e) => {
				Form1.showDetailImage(img, "");
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

			for(int i = 0; i < count; i++)
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
	/**
	 * Other functions for magic
	 * */
	class Util {
		/**
		 * Get default image from resources
		 * */
		public static Image defaultImage() {
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
	/**
	 * Easy way for processing exceptions
	 * */
	class ExceptionHunter {
		public static void error() {

		}
		public static void info() {

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
		/**
		 * DB Settings
		 * */
		private static string database = "cafe";
		private static string host = "127.0.0.1";
		private static string user = "mysql";
		private static string password = "mysql";
		private static Dictionary<string, string> tables = 
			new Dictionary<string, string>()
			{
				{ "dishes",		"dish" },
				{ "images",		"images"},
				{ "order",      "`order`"}
			};
		private MySqlConnection connection;
		/**
		 * Class for work with response from DBResult
		 * */
		public class DBResult
		{
			public bool success = false; // status of response
			public bool empty = false;
			public string error = "Неизвестная ошибка."; // default error
			public List<List<Object>> data = new List<List<Object>>(); // main wrapper for data: rows and cols
		}
		/**
		 * Get all exist tabs of menu from data base
		 * */
		public static List<string> getMenuSections()
		{
			DB db = new DB();
			List<string> sections = new List<string>();
			DBResult result = db.exec("SELECT type FROM " + tables["dishes"] + " ORDER BY CAST(type AS CHAR)");

			if (result.success)
			{
				int rows = result.data.Count();
				for(int i = 0; i < rows; i++)
					sections.Add(result.data[i][0].ToString());
			}

			return sections;
		}
		/**
		 * Get tab of menu from data base by name of section
		 * */
		public static prewiewDish getTabDishesByName(string sectionName)
		{
			DB db = new DB();
			prewiewDish dishes = new prewiewDish();
			DBResult result = db.exec("SELECT id, name, weight, cost FROM " + tables["dishes"] + " WHERE type = '" + sectionName + "';");

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
		/**
		 * Get image from data base by ID
		 * */
		public static Image getImageByID(int id)
		{
			DB db = new DB();
			DBResult result = new DBResult();
			Image img = null;
			try
			{
				db.connect();
				string SQL = "SELECT content FROM " + tables["images"] + " WHERE id = " + id + ";";
				MySqlCommand query = new MySqlCommand(SQL, db.connection);
				db.open();

				MySqlDataReader reader = query.ExecuteReader();
				if (reader.HasRows)
				{
					if (reader.Read())
					{
						Byte[] data = (Byte[])reader.GetValue(0);
						img = Util.bytesToImage(data);
					}
					reader.Close();
					return img;
				}
				else
				{
					result.error = "По запросу получено 0 записей.";
				}
				db.disconnect();
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
			return null;
		}
		/**
		 * Get data for detail dish view by ID
		 * */
		public static detailDish getDishByID(int id)
		{
			DB db = new DB();
			DBResult result = db.exec("SELECT * FROM " + tables["dishes"] + " WHERE id = " + id + " LIMIT 1;");
			detailDish dish = null;

			if (result.success)
			{
				List<int> _images = new List<int>();
				var row = result.data[0];
				
				var name = row[1].ToString();
				var type = row[2].ToString();
				var images = new List<Image>();
				Console.WriteLine(row[3]);
				if(row[3] != DBNull.Value) {
					try
					{
						_images = JsonConvert.DeserializeObject<List<int>>(row[3].ToString());
						int countImages = _images.Count();
						for (int i = 0; i < countImages; i++)
						{
							images.Add(DB.getImageByID(_images[i]));
						}
					}
					catch (JsonException e)
					{
						Console.WriteLine(e);
						MessageBox.Show("Не удалось получить данные о изображениях блюда", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
				
				var description = row[4].ToString();
				var weight = (float)row[5];
				var proteins = (float)row[6];
				var fats = (float)row[7];
				var carbohydrates = (float)row[8];
				var energy = (int)row[9];
				var cost = (float)row[10];

				dish = new detailDish { 
					id = id, name = name, images = images,
					proteins = proteins, fats = fats, carbohydrates = carbohydrates,
					type = type, weight = weight, energy = energy, cost = cost
				};
			}
			return dish;
		}
		/**
		 * Get id of last order in data base
		 * */
		public static int getLastOrderID() {
			DB db = new DB();
			DBResult result = db.exec("SELECT id FROM " + tables["order"] + " ORDER BY id DESC LIMIT 1;");
			int id = 0;

			if (result.success)
			{
				id = (int)result.data[0][0];
			}

			return id;
		}

		public static bool existOrderByID(int id) {
			DB db = new DB();
			DBResult result = db.exec("SELECT id FROM " + tables["order"] + " WHERE id = " + id + " LIMIT 1;");

			return (!result.empty);
		}

		public static bool removeOrderByID(int id)
		{
			DB db = new DB();
			bool result = db.delete("DELETE FROM " + tables["order"] + " WHERE id = " + id + " LIMIT 1;");

			return result;
		}

		public static int insertOrder(string json) 
		{
			int insertID = -1;

			DB db = new DB();
			insertID = db.insert("INSERT INTO " + tables["order"] + " VALUES(NULL, DEFAULT, '" + json + "');");

			return insertID;
		}
		public static bool updateOrder(int id, string json)
		{
			DB db = new DB();
			bool success = db.update("UPDATE " + tables["order"] + " set dishes = '" + json + "', date = DEFAULT WHERE id = " + id + ";");

			return success;
		}

		public static orderDish getOrderByID(int id) {
			DB db = new DB();
			DBResult result = db.exec("SELECT * FROM " + tables["order"] + " WHERE id = " + id + " LIMIT 1;");
			orderDish order = null;

			if (result.success)
			{
				var row = result.data[0];

				var name = "Заказ №" + id;
				var date = row[1].ToString();
				var dishes = new List<dishInOrder>();
				Console.WriteLine(row[2]);
				if (row[2] != DBNull.Value)
				{
					string json = row[2].ToString();
					if(json != "[]") {
						try
						{
							var _dishes = JsonConvert.DeserializeObject<List<Dictionary<int, int>>>(row[2].ToString());
							foreach (var _dish in _dishes)
							{
								dishInOrder dish = new dishInOrder();
								dish.id = _dish.Keys.ElementAt(0);
								dish.count = _dish.Values.ElementAt(0);
								detailDish __dish = DB.getDishByID(dish.id);
								if(__dish != null) {
									dish.name = __dish.name;
									dish.cost = __dish.cost;
									dishes.Add(dish);
								}
							}
						}
						catch (JsonException e)
						{
							Console.WriteLine(e);
							MessageBox.Show("Не удалось получить данные о изображениях блюда", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
					else {
						//TODO write logic
					}
				}

				order = new orderDish
				{
					id = id,
					name = name,
					date = date,
					dishes = dishes
				};
			}
			return order;
		}
		/**
		 * Create connection
		 * */
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
		/**
		 * Open connection
		 * */
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
		/**
		 * Delete/Close connection with data base
		 * */
		public void disconnect()
		{
			try {
				connection.Close();
			}catch(MySqlException e) {
				Console.WriteLine(e);
				MessageBox.Show("Не удалось закрыть подключение к базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(-1);
			}
		}
		/**
		 * Create mysql connection with data base by settings
		 * */
		private MySqlConnection create()
		{
			string connectionData =
				"database=" + database +
				";server=" + host +
				";uid=" + user +
				";pwd=" + password + ";";
			return new MySqlConnection(connectionData);
		}

		private int insert(string SQL) {
			int insertID = -1;
			
			try
			{
				this.connect();
				MySqlCommand query = new MySqlCommand(SQL + " SELECT last_insert_id();", connection);
				this.open();
				insertID = Convert.ToInt32(query.ExecuteScalar());
			}
			catch(Exception e) {
				Console.WriteLine(e);
			}
			finally {
				this.disconnect();
			}

			return insertID;
		}
		private bool update(string SQL)
		{
			bool success = false;
			try
			{
				this.connect();
				MySqlCommand query = new MySqlCommand(SQL, connection);
				this.open();
				int count = query.ExecuteNonQuery();
				if (count > 0)
					success = true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			finally
			{
				this.disconnect();
			}

			return success;
		}
		private bool delete(string SQL) 
		{
			bool success = false;

			try
			{
				this.connect();
				MySqlCommand query = new MySqlCommand(SQL, connection);
				this.open();
				int count = query.ExecuteNonQuery();
				if (count > 0)
					success = true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			finally
			{
				this.disconnect();
			}

			return success;
		}
		/**
		 * Main wrapper for work with data
		 * Get data to DBResult object from data base
		 * */
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
					result.empty = true;
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
		public string type;
		public List<Image> images;
		public float proteins;
		public float fats;
		public float carbohydrates;
		public int energy;
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
	 * Dish class for view dish in list of order
	 * */
	public class orderDish {
		public int id;
		public string name;
		public string date;
		public float cost;
		public float weight;
		public List<dishInOrder> dishes = new List<dishInOrder>();
	}

	public class dishInOrder{
		public int id;
		public string name;
		public float cost;
		public int count;
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
