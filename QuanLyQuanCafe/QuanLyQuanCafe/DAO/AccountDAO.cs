using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
//Design patern Singleton
namespace QuanLyQuanCafe.DAO
{
    class AccountDAO
    {
        private static AccountDAO instance;//Ctrl + R + E
        public static AccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new AccountDAO();
                return AccountDAO.instance;
            }
            private set => instance = value;
        }
        private AccountDAO()
        {

        }
        public bool Login(string userName, string password)
        {
            // Mã hóa mật khẩu
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";
            foreach (byte item in hasData)
            {
                hasPass += item;
            }

            string enCodePass = new string(hasPass.Reverse().ToArray());

            string query = "USP_Login @userName , @PassWord";
            //DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, hasPass /*enCodePass*/ });
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, enCodePass });

            return result.Rows.Count > 0;
        }
        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)
        {
            string query = "exec USP_UpdateAccount @UserName , @DisplayName , @password , @newPassword";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName, displayName, pass, newPass });
            return result > 0;
        }
        public Account GetAccountByUserName(string userName)
        {
            string query = "select * from Account where UserName = '" + userName + "'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }
        public DataTable GetListAccount()
        {
            string query = "select UserName, DisplayName, Type from Account";
            return DataProvider.Instance.ExecuteQuery(query);
        }
        public bool InsertAccount(string UserName, string DisplayName, int Type)
        {
            string query = string.Format("insert into Account(UserName, DisplayName, Type, PassWord) VALUES (N'{0}', N'{1}', {2}, N'5512317111114510840231031535810616566202691')", UserName, DisplayName, Type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccount(string UserName, string DisplayName, int Type)
        {
            string query = string.Format("update Account set DisplayName = N'{1}', Type = {2} where UserName = N'{0}'", UserName, DisplayName, Type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string UserName)
        {
            string query = string.Format("Delete Account where UserName = N'{0}'", UserName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPassword(string UserName)
        {
            string query = string.Format("update Account set PassWord = N'5512317111114510840231031535810616566202691' where UserName = N'{0}'", UserName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
    }
}
