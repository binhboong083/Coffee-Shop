using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Design patern Singleton
namespace QuanLyQuanCafe.DAO
{
    class DataProvider
    {
        private static DataProvider instance;//Ctrl + R + E
        public static DataProvider Instance {
            get
            {
                if (instance == null) instance = new DataProvider();
                return DataProvider.instance;
            }
          private set => instance = value; 
        }
        private DataProvider()
        {
        }
        private string connectionStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                //
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                // Làm trung gian truy vấn lấy dữ liệu ra
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                //
                connection.Close();
            }
            return data;
        }
        // ExecuteNonQuery chỉ dùng cho trường hợp Update, Insert, Delete
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                //
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                // Trả về số dòng thành công
                data = command.ExecuteNonQuery();
                //
                connection.Close();
            }
            return data;
        }
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                //
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                // phù hợp với select count(*)
                data = command.ExecuteScalar();
                //
                connection.Close();
            }
            return data;
        }

    }
}
