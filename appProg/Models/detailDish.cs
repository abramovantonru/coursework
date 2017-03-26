using System.Collections.Generic;
using System.Drawing;

namespace appProg
{
	/**
	* Dish class for detail view
	* */
	public class detailDish
	{
		public int id;
		public string name;
		public string description;
		public string type;
		public List<Image> images;
		public float proteins;
		public float fats;
		public float carbohydrates;
		public int energy;
		public float weight;
		public float cost;
	};
}
