using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appProg.MySQL
{
	public class printOrder
	{
		public int id;
		public string date;
		public int energy;
		public float total;
		public List<dishInOrder> dishes = new List<dishInOrder>();
	};
}
