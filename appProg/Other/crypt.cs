using System.Security.Cryptography;
using System.Text;

namespace cafeMenu.Other
{
	public class Crypt
	{
		public class md5 
		{
			public string getHash(string str)
			{
				byte[] hash = Encoding.ASCII.GetBytes(str);
				MD5 md5 = new MD5CryptoServiceProvider();
				byte[] hashenc = md5.ComputeHash(hash);

				string result = "";
				foreach (var b in hashenc)
					result += b.ToString("x2");
				
				return result;
			}

			public bool checkHash(string password, string hash) 
			{
				if (this.getHash(password) == hash)
					return true;
				else
					return false;
			}
		}
	}
}
