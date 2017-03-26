using System.Collections.Generic;

namespace cafeMenu.MySQL
{
	public class printOrder
	{
		public int id;
		public string date;
		public int energy;
		public float total;
		public float weight;
		public List<dishInOrder> dishes = new List<dishInOrder>();
	};
}
