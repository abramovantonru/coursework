using System.Collections.Generic;

namespace cafeMenu
{
	/**
	 * Dish class for view dish in list of order
	 * */
	public class orderDish
	{
		public int id;
		public string name;
		public string date;
		public float cost;
		public float weight;
		public List<dishInOrder> dishes = new List<dishInOrder>();
	}
}
