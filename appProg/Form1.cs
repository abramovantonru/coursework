using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

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


            this.menu.Width = width / 2 - 60;
            this.menu.Height = height - 40 - 20;
            this.menu.Location = new Point(
                20,
                40
            );

            /* dataGridView1.Width = width / 2 - 60;
             dataGridView1.Height = height - 40 - 20;
             dataGridView1.Location = new Point(
                 20,
                 40
             );*/


            this.btnAppClose.Location = new Point(
                width - btnAppClose.Width - 20,
                20
            );

            menuSections = DB.getMenuSections();
            createTabs(menuSections);
            fillTab(0, getTabDataByName(menuSections[0]));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /**
             * Убираем границы окна и делаем его на весь экран
             * */
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Left = Top = 0;
            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height;
        }

        private prewiewDish getTabDataByName(string name)
        {
            prewiewDish dishes = DB.getTabDishesByName(name);
            //dishes.Add(new prewiewDish { id = 1, name = "123", weight = 12, cost = 15.30f });
            //dishes.Add(new prewiewDish { id = 2, name = "456", weight = 16, cost = 75.90f });


            return dishes;
        }

        private void fillTab(int index, prewiewDish dishes)
        {
            foreach (prewiewDish dish in dishes)
                menuTables[index].Rows.Add(new object[] { dish.id, dish.name, dish.weight, dish.cost });
        }

        private void createTabs(List<string> sections)
        {
            int menuHeight = this.menu.Height;
            int menuWidth = this.menu.Width;
            int countSections = sections.Count();
            menuTables = new DataGridView[sections.Count];
            
            menuColumn[] columns = {
                new menuColumn { name = "id", header = "№", width = menuWidth / 100 * 5 },
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

        private void btnAppClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }

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

                    var id = Convert.ToInt32(row[0]);
                    var name = row[1].ToString();
                    var weight = (float)row[2];
                    var cost = (float)row[3];

                    dishes.Add(new prewiewDish { id = id, name = name, weight = weight, cost = cost });
                }
               
            }
               
            return dishes;
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


    public class detailDish
    {
        public int id;
        public string name;
        public float proteins; // белки
        public float fats; // жиры
        public float carbohydrates; // углеводы
        public float weight;
        public float cost;
    };

    public class prewiewDish : List<Object>
    {
        public int id;
        public string name;
        public float weight;
        public float cost;
    }

    public class menuColumn
    {
        public string name;
        public string header;
        public int width;
    }
   
}
