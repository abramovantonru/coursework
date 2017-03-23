using Newtonsoft.Json;
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
	public partial class orderForm : Form
	{
		public static TabControl orderList;
		private static List<orderDish> orders = new List<orderDish>(); // opened orders
		private static DataGridView[] orderTables;
		private static Form openOrderDialog;
		private static Form removeOrderDialog;

		public orderForm()
		{
			InitializeComponent();
			this.StartPosition = FormStartPosition.CenterScreen;
		}

		private void orderForm_Load(object sender, EventArgs e)
		{
			int h = menuStrip.Height;
			orderList = new TabControl();
			orderList.Dock = DockStyle.Bottom;
			orderList.Name = "order";
			orderList.SelectedIndex = 0;
			orderList.Size = new Size(this.Width, this.Height - h * 3);
			orderList.TabIndex = 17;
		
			this.Controls.Add(orderList);
			loadOrdersTabs();
		}

		public static void loadOrderTabDishes(int idx)
		{
			if (idx != -1 && orders.Any())
			{
				orderTables[idx].Rows.Clear();
				foreach (var dish in orders[idx].dishes)
				{
					orderTables[idx].Rows.Add(new object[] { dish.id, dish.name, dish.cost, dish.count });
				}
			}
		}

		public static void loadOrdersTabs()
		{
			orderList.TabPages.Clear();
			int orderHeight = orderList.Height;
			int orderWidth = orderList.Width;
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
				orderTables[i].AllowUserToResizeRows = false;
				orderTables[i].AllowUserToResizeColumns = false;
				orderTables[i].SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				orderTables[i].RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
				orderTables[i].ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
				orderTables[i].ScrollBars = ScrollBars.Vertical;
				orderTables[i].ReadOnly = true;
				orderTables[i].AllowUserToAddRows = false;
				orderTables[i].CellClick += (s, ev) => {
					int row = ev.RowIndex;
					if (row > -1)
					{
						var tab = orderList.SelectedIndex;
						int id = (int)orderTables[tab].Rows[row].Cells[0].Value;
						if (id > 0)
							if (!MainForm.selectedDishes.Any() || MainForm.selectedDishes.IndexOf(id) == -1)
								MainForm.showDetailDish(id);
					}
				};

				for (int j = 0; j < countColumns; j++)
				{
					DataGridViewColumn column = new DataGridViewTextBoxColumn();
					column.Width = columns[j].width;
					column.Name = columns[j].name;
					column.HeaderText = columns[j].header;
					orderTables[i].Columns.Add(column);
				}

				tabPage.Controls.Add(orderTables[i]);
				orderList.TabPages.Add(tabPage);
			}

			if (orders.Any())
			{
				for (int i = 0; i < countOrders; i++)
				{
					loadOrderTabDishes(i);
				}
			}
		}

		public static void addDishToOrder(int id, string name, float cost, int count)
		{
			int orderIdx = orderList.SelectedIndex;
			if (orderIdx == -1)
			{
				orderIdx = createOrder();
				orders[orderIdx].dishes.Add(new dishInOrder { id = id, name = name, cost = cost, count = count });
				loadOrdersTabs();
			}
			else
			{
				int dishIdx = findOrderDishIdxByDishID(orderIdx, id);
				if (dishIdx == -1)
					orders[orderIdx].dishes.Add(new dishInOrder { id = id, name = name, cost = cost, count = count });
				else
					orders[orderIdx].dishes[dishIdx].count += count;
				int selectedIdx = orderList.SelectedIndex;
				loadOrderTabDishes(selectedIdx);
			}
		}

		//create(append) order in global MainWindow var "order" [optional by exist orderDish object]
		private static int createOrder(orderDish order = null)
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

		//save order from global MainWindow var "order" by index to data base
		private int saveOrder(int index)
		{
			int id = -1;
			int orderID = orderIDbyIdx(index);

			if (orderID != -1)
			{
				string json = getSerializeOrderItems(index);
				if (json != "[]")
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

		private int openOrder(int id)
		{
			orderDish order = new orderDish();
			order = DB.getOrderByID(id);
			if (order != null)
			{
				createOrder(order);
				return findOrderIdxByOrderID(id);
			}
			else
			{
				MessageBox.Show("Неудалось получить информацию о заказе!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return -1;
		}

		private void showRemoveOrderDialog(int id)
		{
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
				h / 2 - info.Height * 2
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
					if (DB.removeOrderByID(id))
					{
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

		/**
		 * Utils
		 * */
		private string getSerializeOrderItems(int orderIdx)
		{
			List<string> _dish = new List<string>();
			string json = "";

			foreach (var dish in orders[orderIdx].dishes)
				_dish.Add(JsonConvert.SerializeObject(new Dictionary<int, int>() { { dish.id, dish.count } }));

			if (_dish.Any())
				json += String.Join(", ", _dish);

			return "[" + json + "]";
		}

		private static int findOrderDishIdxByDishID(int orderIdx, int id)
		{
			int idx = -1;

			if (orderIdx != -1)
			{
				int count = orders[orderIdx].dishes.Count;
				for (int i = 0; i < count; i++)
				{
					if (orders[orderIdx].dishes[i].id == id)
					{
						idx = i;
						break;
					}
				}
			}

			return idx;
		}

		private int findOrderIdxByOrderID(int id)
		{
			int idx = -1;
			int count = orders.Count;

			for (int i = 0; i < count; i++)
			{
				if (orders[i].id == id)
				{
					idx = i;
					break;
				}
			}

			return idx;
		}

		private int findOrderIdxByDishID(int id)
		{
			int idx = -1;
			int countOrder = orders.Count;

			for (int i = 0; i < countOrder; i++)
			{
				int countDishes = orders[i].dishes.Count;
				for (int j = 0; j < countDishes; j++)
				{
					if (orders[i].dishes[j].id == id)
					{
						idx = i;
						break;
					}
				}
			}

			return idx;
		}

		private int orderIDbyIdx(int idx)
		{
			int id = -1;

			if (idx < orders.Count && orders[idx] != null)
				id = orders[idx].id;

			return id;
		}

		/**
		 * Events
		 * */

		private void btnOpenOrder_Click(object sender, EventArgs e)
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
				if (DB.existOrderByID(id))
				{
					if (findOrderIdxByOrderID(id) == -1)
					{
						int selectedIdx = openOrder(id);
						openOrderDialog.Close();
						loadOrdersTabs();
						if (selectedIdx != -1)
							orderList.SelectedIndex = selectedIdx;
					}
					else
					{
						MessageBox.Show("Заказ с данным номером (" + id + ") уже открыт.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
				{
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

		private void btnCreateOrder_Click(object sender, EventArgs e)
		{
			int idx = createOrder();
			loadOrdersTabs();
			orderList.SelectedIndex = idx;
		}

		private void btnSaveOrder_Click(object sender, EventArgs e)
		{
			int index = orderList.SelectedIndex;
			if (index > -1)
			{
				int id = saveOrder(index);
				if (id == 0)
					MessageBox.Show("Заказ успешно сохранен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
				else if (id == -1)
					MessageBox.Show("Не удалось сохранить заказ!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else if (id > -1)
				{
					MessageBox.Show("Заказ успешно сохранен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
					orders[index].name = "Заказ №" + id;
					loadOrdersTabs();
				}
			}
		}

		private void btnRemoveOrder_Click(object sender, EventArgs e)
		{
			int index = orderList.SelectedIndex;
			if (index > -1 && orders[index] != null)
			{
				if (orders[index].name == "Новый заказ")
					orders.RemoveAt(index);
				else
				{
					int id = orders[index].id;
					if (DB.existOrderByID(id))
						showRemoveOrderDialog(id);
					else
						MessageBox.Show("Заказ с данным номером (" + id + ") не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				orderList.TabPages.Clear();
				loadOrdersTabs();
			}
		}

		private void btnCloseOrder_Click(object sender, EventArgs e)
		{
			int index = orderList.SelectedIndex;
			if (index != -1)
			{
				orders.RemoveAt(index);
				loadOrdersTabs();

				int newIndex = index - 1;
				if (newIndex < 0)
					newIndex = 0;
				orderList.SelectedIndex = newIndex;
			}
		}
	}
}
