using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Food
    {
        private int iD;
        private string name;
        private int idCategory;
        private float price;

        public Food(int iD, string name, int idCategory, float price)
        {
            ID = iD;
            Name = name;
            IdCategory = idCategory;
            Price = price;
        }
        public Food(DataRow row)
        {
            ID = (int)row["id"];
            Name = row["name"].ToString();
            IdCategory = (int)row["idCategory"];
            Price = (float)Convert.ToDouble(row["price"].ToString());
        }
        public int ID { get => iD; set => iD = value; }
        public string Name { get => name; set => name = value; }
        public int IdCategory { get => idCategory; set => idCategory = value; }
        public float Price { get => price; set => price = value; }
    }
}
