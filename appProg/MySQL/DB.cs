using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appProg
{
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
				{ "dishes",     "dish" },
				{ "images",     "images"},
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
				for (int i = 0; i < rows; i++)
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
				if (row[3] != DBNull.Value)
				{
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

				dish = new detailDish
				{
					id = id,
					name = name,
					images = images,
					proteins = proteins,
					fats = fats,
					carbohydrates = carbohydrates,
					type = type,
					weight = weight,
					energy = energy,
					cost = cost
				};
			}
			return dish;
		}
		/**
		 * Get id of last order in data base
		 * */
		public static int getLastOrderID()
		{
			DB db = new DB();
			DBResult result = db.exec("SELECT id FROM " + tables["order"] + " ORDER BY id DESC LIMIT 1;");
			int id = 0;

			if (result.success)
			{
				id = (int)result.data[0][0];
			}

			return id;
		}

		public static bool existOrderByID(int id)
		{
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

		public static orderDish getOrderByID(int id)
		{
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
					if (json != "[]")
					{
						try
						{
							var _dishes = JsonConvert.DeserializeObject<List<Dictionary<int, int>>>(row[2].ToString());
							foreach (var _dish in _dishes)
							{
								dishInOrder dish = new dishInOrder();
								dish.id = _dish.Keys.ElementAt(0);
								dish.count = _dish.Values.ElementAt(0);
								detailDish __dish = DB.getDishByID(dish.id);
								if (__dish != null)
								{
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
					else
					{
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
			try
			{
				connection.Close();
			}
			catch (MySqlException e)
			{
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

		private int insert(string SQL)
		{
			int insertID = -1;

			try
			{
				this.connect();
				MySqlCommand query = new MySqlCommand(SQL + " SELECT last_insert_id();", connection);
				this.open();
				insertID = Convert.ToInt32(query.ExecuteScalar());
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			finally
			{
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
}
