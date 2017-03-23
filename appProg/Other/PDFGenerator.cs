using appProg.MySQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace appProg
{
	public class PDFGenerator
	{
		readonly static string dir = AppDomain.CurrentDomain.BaseDirectory + "reviews";

		public PDFGenerator(string _html = null)
		{
			/*this.createDir(dir);
			string filePath = dir + @"\" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm") + ".html";
			this.fillHTML(filePath, html);
			this.open(filePath);*/
		}

		private void createDir(string _dir) {
			try
			{
				if (!Directory.Exists(_dir))
				{
					Directory.CreateDirectory(_dir);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private bool open(string _path)
		{
			try
			{
				System.Diagnostics.Process.Start(_path);
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}
		}

		private void fillHTML(string _path, string _html) {
			try {
				File.WriteAllText(_path, _html);
			}catch(Exception e) {
				Console.WriteLine(e);
			}
		}

		public void orderByID(int id)
		{
			printOrder order = DB.getPrintOrderByID(id);
			if (order == null)
				MessageBox.Show("Не удалось получить информацию о заказе!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else {
				var _html = @"
				<html lang=ru>
					<meta charset='cp-1251'>
					<meta name='viewport'
						  content='width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0'>
					<meta http-equiv='X-UA-Compatible' content='ie=edge'>
					<title id='title'></title>
				<body>
				<style>
					h1{
						text-align: center;
						font-size: 28px;
						font-weight: bold;
					}
					table{
						width: 80%;
						margin: 0 auto;
						border: 0;
						overflow-x: auto;
					}
					table thead{font-weight: bold;}
					table thead tr td{
						border: 0;
						text-align: center;
					}
					table tbody tr td{border-bottom: 1px solid black;}
					table tbody tr td{text-align: center;}
					table tbody tr td:first-child{text-align: left;}
					table tbody tr td:last-child{text-align: right;}
					.hidden{display: none;}
				</style>
				<div id='print' class=''>
					<script>
					window.title.innerText = 'Заказ № " + order.id + @"';
					window.print();
					</script>
					<h1>Заказ № " + order.id + @"</h1>
					<table>
						<thead>
						<tr>
							<td>Название</td>
							<td>Грамм</td>
							<td>кДЖ</td>
							<td>Цена</td>
							<td>Кол-во</td>
							<td>ИТОГО</td>
						</tr>
						<tr><td colspan='6'>&nbsp;</td></tr>
						<tr><td colspan='6'>&nbsp;</td></tr>
						</thead>
						<tbody id='items'>";

						foreach(var dish in order.dishes)
							_html += "<tr><td>" + dish.name + @"</td><td>" + dish.weight + "</td><td>" + dish.energy + "</td><td>" + dish.cost + "</td><td>" + dish.count + "</td><td>" + (dish.count * dish.cost) + "</td></tr>";
						
						_html += 
						@"<tfoot>
						<tr>
							<td colspan='6' style='text-align: right'><span data-name='total'>" + order.total + @"</span></td>
						</tr>
						<tr><td colspan='6'>&nbsp;</td></tr>
						<tr><td colspan='6'>&nbsp;</td></tr>
						<tr>
							<td colspan='4' style='text-align: right'><b>Общая энергетическая ценность, кДж</b></td>
							<td colspan='2'>" + order.energy + @"</td>
						</tr>
						<tr>
							<td colspan='4' style='text-align: right'><b>Дата и время</b></td>
							<td colspan='2'>" + order.date + @"</td>
						</tr>
						<tr><td colspan='6'>&nbsp;</td></tr>
						<tr><td colspan='6'>&nbsp;</td></tr>
						</tfoot>
					</table>
				</div>
				</body>
				</html>";

				this.createDir(dir);
				string filePath = dir + @"\" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm") + ".html";
				this.fillHTML(filePath, _html);
				this.open(filePath);
			}
		}

		public void ordersByDates(string from, string to)
		{
			List<printOrder> orders = DB.getPrintOrdersByDates(from, to);
			int counts = 0;
			float total = 0.0f;
			var _html = @"
				<html lang=ru>
					<meta charset='cp-1251'>
					<meta name='viewport'
						  content='width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0'>
					<meta http-equiv='X-UA-Compatible' content='ie=edge'>
					<title id='title'></title>
				<body>
				<style>
					h1{
						text-align: center;
						font-size: 28px;
						font-weight: bold;
					}
					table{
						width: 80%;
						margin: 0 auto;
						border: 0;
						overflow-x: auto;
					}
					table thead{font-weight: bold;}
					table thead tr td{
						border: 0;
						text-align: center;
					}
					table tbody tr td{border-bottom: 1px solid black;}
					table tbody tr td{text-align: center;}
					.hidden{display: none;}
				</style>
				<div id='print' class=''>
					<script>
					window.title.innerText = 'Отчет " + from + " - " + to + @"';
					window.print();
					</script>
					<h1>Отчет " + from + " - " + to + @"</h1>
					<table>
						<thead>
						<tr>
							<td>№</td>
							<td>Дата и время</td>
							<td>Кол-во блюд</td>
							<td>Стоимость</td>
						</tr>
						<tr><td colspan='5'>&nbsp;</td></tr>
						<tr><td colspan='5'>&nbsp;</td></tr>
						</thead>
						<tbody id='items'>";

			foreach (var order in orders) {
				int count = order.dishes.Count;
				counts += count;
				total += order.total;
				_html += "<tr><td>" + order.id + @"</td><td>" + order.date + "</td><td>" + count + "</td><td>" + order.total + "</td></tr>";
			}
				
			_html +=
						@"<tfoot>
						<tr>
							<td colspan='2' style='text-align: right'><b>ИТОГО</b></td>
							<td style='text-align: center'>" + counts + @"</td>
							<td style='text-align: center'>" + total + @"</td>
						</tr>
						</tfoot>
					</table>
				</div>
				</body>
				</html>";

			this.createDir(dir);
			string filePath = dir + @"\" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss") + ".html";
			this.fillHTML(filePath, _html);
			this.open(filePath);
		}
	}
}
