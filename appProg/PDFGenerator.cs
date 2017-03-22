using System;
using System.IO;

namespace appProg
{
	public class PDFGenerator
	{
		readonly static string dir = AppDomain.CurrentDomain.BaseDirectory + "reviews";

		string html = @"<!DOCTYPE html>
					<html>
					<body>

					<h1>My First Heading</h1>

					<p>My first paragraph.</p>
					<script>window.print();</script>
					</body>
					</html>
					";

		public PDFGenerator(string _html = null)
		{
			this.createDir(dir);
			string filePath = dir + @"\" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm") + ".html";
			this.fillHTML(filePath, html);
			this.open(filePath);
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
	}
}
