using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(fAdmin_KeyDown);
            Load();
        }
        #region methods
        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;
            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            //LoadListBillByDate(dtpFromDate.Value, dtpkToDate.Value);
            txtPage.Text = $"1/{totalPageDanhThu()}";
            LoadListBillByDateAndPage(dtpFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadListCategory();
            LoadListTable();
            LoadAccount();
            LoadCategoryIntoComboBox(cbFoodCategory);
            AddFoodBinding();
            AddCategoryBinding();
            AddTableBinding();
            AddAccountBinding();
        }
        void AddAccountBinding()
        {
            txtUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmTypeAccount.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut);
        }
        void LoadListBillByDateAndPage(DateTime checkIn, DateTime checkOut)
        {
            int page = 1;
            string[] pattern = new string[3];
            pattern[0] = @"^[0-9]+/?[0-9]+$";
            pattern[1] = @"^[0-9]+$";
            pattern[2] = @"";
            if (Regex.IsMatch(txtPage.Text, pattern[0]))
            {
                int.TryParse(txtPage.Text.Split('/')[0], out page);
                dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpFromDate.Value, dtpkToDate.Value, page);
            }
            else if (Regex.IsMatch(txtPage.Text, pattern[1]))
            {
                int.TryParse(txtPage.Text, out page);
                dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpFromDate.Value, dtpkToDate.Value, page);
            }
            else if (Regex.IsMatch(txtPage.Text, pattern[2])) {; }
            else
            {
                MessageBox.Show("Nhập vào số trang đúng định dạng\n-> Ví dụ: 1/2 hoặc 1");
            }
        }
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        void AddFoodBinding()
        {
            txtFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "name", true, DataSourceUpdateMode.Never));
            txtFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "id", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "price", true, DataSourceUpdateMode.Never));
        }
        void AddCategoryBinding()
        {
            txtCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "name", true, DataSourceUpdateMode.Never));
            txtCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "id", true, DataSourceUpdateMode.Never));
        }
        void AddTableBinding()
        {
            txtTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "id", true, DataSourceUpdateMode.Never));
            txtTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "name", true, DataSourceUpdateMode.Never));
            txtStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));
        }
        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableList();
        }
        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void AddAccount(string UserName, string DisplayName, int Type)
        {
            if (AccountDAO.Instance.InsertAccount(UserName, DisplayName, Type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }
            LoadAccount();
        }
        void EditAccount(string UserName, string DisplayName, int Type)
        {
            if (AccountDAO.Instance.UpdateAccount(UserName, DisplayName, Type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }
            LoadAccount();
        }
        void DeleteAccount(string UserName)
        {
            if (loginAccount.UserName.Equals(UserName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(UserName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }
            LoadAccount();
        }
        void ResetPassword(string UserName)
        {
            if (AccountDAO.Instance.ResetPassword(UserName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        int totalPageDanhThu()
        {
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtpFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
            {
                lastPage++;
            }
            return lastPage;
        }
        #endregion

        #region events

        private void fAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Xử lý khi nhấn phím Escape
                this.Close();
                //this.Hide();
            }
        }
        // Tab Food
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["idCategory"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    //cbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int idCategory = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            if (FoodDAO.Instance.InsertFood(name, idCategory, price))
            {
                MessageBox.Show("Thêm món ăn thành công");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món ăn");
            }
        }
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int idCategory = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txtFoodID.Text);
            if (FoodDAO.Instance.UpdateFood(id, name, idCategory, price))
            {
                MessageBox.Show("Sửa món ăn thành công");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món ăn");
            }
        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món ăn thành công");
                LoadListFood();
                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món ăn");
            }
        }
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpFromDate.Value, dtpkToDate.Value);
            txtPage.Text = "";
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }
        // Tab Category
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục món ăn thành công");
                LoadListCategory();
                if (insertCategory != null)
                {
                    insertCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm danh mục món ăn");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtCategoryID.Text);
            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                LoadListCategory();
                if (deleteCategory != null)
                {
                    deleteCategory(this, new EventArgs());
                }
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;
            int id = int.Parse(txtCategoryID.Text);
            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa danh mục món ăn thành công");
                LoadListCategory();
                if (updateCategory != null)
                {
                    updateCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa danh mục món ăn");
            }
        }
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }
        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }
        // Tab Table
        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;
            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadListTable();
                if (insertTable != null)
                {
                    insertTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn");
            }
        }
        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string name = txtTableName.Text;
            int id = int.Parse(txtTableID.Text);
            if (TableDAO.Instance.UpdateTable(id, name))
            {
                MessageBox.Show("Sửa thông tin bàn thành công");
                LoadListTable();
                if (updateTable != null)
                {
                    updateTable(this, new TableEvent(TableDAO.Instance.GetTableByID(id)));
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thông tin bàn");
            }
        }
        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtTableID.Text);

            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công");
                LoadListTable();
                if (deleteTable != null)
                {
                    deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa bàn");
            }
        }
        public class TableEvent : EventArgs
        {
            private Table table;
            public Table Table { get => table; set => table = value; }

            public TableEvent(Table table)
            {
                Table = table;
            }
        }
        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler<TableEvent> updateTable;

        public event EventHandler<TableEvent> UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txtSearchFoodName.Text);
        }
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            string DisplayName = txtDisplayName.Text;
            int Type = (int)nmTypeAccount.Value;
            AddAccount(UserName, DisplayName, Type);
        }
        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            string DisplayName = txtDisplayName.Text;
            int Type = (int)nmTypeAccount.Value;
            EditAccount(UserName, DisplayName, Type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            DeleteAccount(UserName);
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string UserName = txtUserName.Text;
            ResetPassword(UserName);
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            txtPage.Text = $"1/{totalPageDanhThu()}";
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            txtPage.Text = $"{totalPageDanhThu()}/{totalPageDanhThu()}";
        }
        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = 1;
            int.TryParse(txtPage.Text.Split('/')[0], out page);
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtpFromDate.Value, dtpkToDate.Value);

            if (page < totalPageDanhThu())
            {
                page++;
                txtPage.Text = page.ToString() + "/" + totalPageDanhThu();
            }
            else
                MessageBox.Show("Đây là trang cuối cùng\n--> Không di chuyển được nữa");
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = 1;
            int.TryParse(txtPage.Text.Split('/')[0], out page);
            if (page > 1)
            {
                page--;
                txtPage.Text = page.ToString() + "/" + totalPageDanhThu();
            }
            else
                MessageBox.Show("Đây là trang đầu tiên\n--> Không di chuyển được nữa");

        }
        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            LoadListBillByDateAndPage(dtpFromDate.Value, dtpkToDate.Value);
        }

        #endregion
    }
}
