using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;
        public fAccountProfile(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
        }

        public Account LoginAccount
        {
            get => loginAccount;
            set
            {
                loginAccount = value;
                ChangeAccount(loginAccount);
            }
        }

        void ChangeAccount(Account acc)
        {
            txtUserName.Text = loginAccount.UserName;
            txtDisplayName.Text = loginAccount.DisplayName;
        }
        void UpdateAccountInfo()
        {
            string userName = txtUserName.Text;
            string displayName = txtDisplayName.Text;
            string password = txtPassword.Text;
            string newPass = txtNewPass.Text;
            string reEnterPass = txtReEnterPass.Text;

            // Mã hóa mật khẩu cũ
            byte[] tempPassword = ASCIIEncoding.ASCII.GetBytes(password);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(tempPassword);

            string hasPass = "";
            foreach (byte item in hasData)
            {
                hasPass += item;
            }

            string enCodePass = new string(hasPass.Reverse().ToArray());
            // Mã hóa mật khẩu mới
            byte[] tempNewPass = ASCIIEncoding.ASCII.GetBytes(newPass);
            byte[] hasNewData = new MD5CryptoServiceProvider().ComputeHash(tempNewPass);

            string hasNewPass = "";
            foreach (byte item in hasNewData)
            {
                hasNewPass += item;
            }

            string enCodeNewPass = new string(hasNewPass.Reverse().ToArray());

            if (!newPass.Equals(reEnterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, enCodePass, enCodeNewPass))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }
        private event EventHandler<AccountEvent> updateAccount;

        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
        public class AccountEvent : EventArgs
        {
            private Account acc;
            public Account Acc { get => acc; set => acc = value; }

            public AccountEvent(Account acc)
            {
                this.Acc = acc;
            }
        }
    }
}
