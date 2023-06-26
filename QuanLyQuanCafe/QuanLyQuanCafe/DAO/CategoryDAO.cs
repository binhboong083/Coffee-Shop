using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Design patern Singleton
namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;//Ctrl + R + E
        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null) instance = new CategoryDAO();
                return CategoryDAO.instance;
            }
            private set => instance = value;
        }
        private CategoryDAO()
        {
        }
        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();
            string query = "SELECT * FROM FoodCategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }
            return list;
        }
        public Category GetCategoryByID(int id)
        {
            Category category = null;

            string query = "SELECT * FROM FoodCategory where id = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }
            return category;
        }
        public bool InsertCategory(string name)
        {
            string query = string.Format("insert into FoodCategory(name) VALUES (N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateCategory(int id, string name)
        {
            string query = string.Format("update FoodCategory set name = N'{0}' where id = {1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteCategory(int idCategory)
        {
            string query = string.Format("exec USP_DeleteCategory @idCategory = {0}", idCategory);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            int result = 0;
            int.TryParse(data.Rows[0]["result"].ToString(), out result);
            if (result == -1)
            {
                MessageBox.Show("Danh mục món ăn không đúng\n-> Xóa danh mục món ăn thất bại");
                return false;
            }
            else if (result == 0)
            {
                MessageBox.Show("Danh mục món ăn đang được sử dụng\n-> Xóa danh mục món ăn thất bại");
                return false;
            }
            else if (result == 1)
            {
                query = "SELECT * FROM Food where idCategory = " + idCategory;

                DataTable dataFood = DataProvider.Instance.ExecuteQuery(query);
                int idFood;
                foreach (DataRow item in dataFood.Rows)
                {
                    int.TryParse(item["id"].ToString(), out idFood);
                    FoodDAO.Instance.DeleteFood(idFood);
                }

                query = string.Format("Delete FoodCategory where id = {0}", idCategory);
                DataProvider.Instance.ExecuteNonQuery(query);
                MessageBox.Show("Xóa danh mục món ăn thành công");

                return true;
            }
            return false;
        }
    }
}
