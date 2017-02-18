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

            List<string> sections = DB.menuSections();
            createTabs(sections);
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

        private void createTabs(List<string> sections)
        {
            int menuHeight = this.menu.Height;
            int menuWidth = this.menu.Width;
            int count = sections.Count;
            DataGridView[] dataGridViews = new DataGridView[count];
            int w = menuWidth - 40;
            int columnsCount = 3;
            menuColumn[] columns = {
                new menuColumn { name = "id", header = "№", width = w / 100 * 5 },
                new menuColumn { name = "name", header = "Название", width = w / 100 * 70 },
                new menuColumn { name = "cost", header = "Стоимость", width = w / 100 * 10 },
            };

            for (int i = 0; i < count; i++)
            {
                
                TabPage myTabPage = new TabPage(sections[i]);
                dataGridViews[i] = new DataGridView();
                dataGridViews[i].Left = dataGridViews[i].Top = 20;
                dataGridViews[i].Height = menuHeight - 60;
                dataGridViews[i].Width = w;

                for (int j = 0; j < columnsCount; j++)
                {
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.Width = columns[j].width;
                    column.Name = columns[j].name;
                    column.HeaderText = columns[j].header;
                    dataGridViews[i].Columns.Add(column);
                }

                myTabPage.Controls.Add(dataGridViews[i]);
                this.menu.TabPages.Add(myTabPage);
            }

            for(int i = 0; i < 200; i++)
            {
                dataGridViews[0].Rows.Add("123");
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
            public bool success;
            public string error;
            public List<Object> data = new List<Object>();
        }

        public static List<string> menuSections()
        {
            DB db = new DB();
            List<string> sections = new List<string>();
            DBResult result = db.exec("SELECT type FROM dish ORDER BY CAST(type AS CHAR)");

            if (result.success)
                foreach(var row in result.data)
                    sections.Add(row.ToString());
           
            return sections;
        }


        public void connect()
        {
            connection = this.create();
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
            this.connect();
            DBResult result = new DBResult();
            MySqlCommand query = new MySqlCommand(SQL, connection);
            this.open();

            MySqlDataReader reader = query.ExecuteReader();
            if (reader.HasRows)
            {
                result.success = true;
                while (reader.Read())
                {
                    Object[] values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    for (int i = 0; i < fieldCount; i++)
                        result.data.Add(values[i]);
                }
                reader.Close();
            }else
            {
                result.success = false;
            }

            this.disconnect();
            return result;
        }
    }


    /*public class detailDish
    {
        public string article;
        public string name;
        public float proteins; // белки
        public float fats; // жиры
        public float carbohydrates; // углеводы
        public float cost;
    };*/

    public class prewiewDish
    {
        public string article;
        public string name;
        public float proteins; // белки
        public float fats; // жиры
        public float carbohydrates; // углеводы
        public float cost;
    }

    public class menuColumn
    {
        public string name;
        public string header;
        public int width;
    }
}
