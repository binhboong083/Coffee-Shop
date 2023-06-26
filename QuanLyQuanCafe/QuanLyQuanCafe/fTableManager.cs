using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyQuanCafe.fAccountProfile;
using static QuanLyQuanCafe.fAdmin;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;
        private SortedList<string, string> listMergeTable = new SortedList<string, string>();
        private SortedList<int, int> listIDTable = new SortedList<int, int>();
        public Account LoginAccount
        {
            get => loginAccount;
            set
            {
                loginAccount = value;
                ChangeAccount(loginAccount.Type);
            }
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(fTableManager_KeyDown);
            LoadTable();
            LoadCategory();
            LoadComboBoxTable(cbSwitchTable);
        }
        #region Method
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
            if (cbFood.Items.Count == 0)
            {
                cbFood.Text = "";
            }
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            //
            SortedList<int, int> listIDTableTemp = new SortedList<int, int>();
            int i = -1;
            foreach (Table item in tableList)
            {
                listMergeTable.Add($"{item.ID}", "");
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                if (listMergeTable[$"{item.ID}"] != "merge")
                {
                    btn.Text = item.Name + listMergeTable[$"{item.ID}"] + Environment.NewLine + item.Status;
                }
                else
                {
                    btn.Text = item.Name + Environment.NewLine + "(Tạm ngưng)";
                }

                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }
                if (listMergeTable[$"{item.ID}"] == "merge")
                {
                    btn.BackColor = Color.BurlyWood;
                }
                i++;
                listIDTableTemp.Add(item.ID, i);
                flpTable.Controls.Add(btn);
            }
            listIDTable = listIDTableTemp;
        }
        void LoadTable(int id)
        {
            //Control buttonToRemove = flpTable.Controls[id];
            //flpTable.Controls.Remove(buttonToRemove);

            //Button buttonToModify = flpTable.Controls[id - 1] as Button;
            Button buttonToModify = flpTable.Controls[listIDTable[id]] as Button;

            List<Table> tableList = TableDAO.Instance.LoadTableList();
            Table temp = tableList.Find(x => x.ID == id);
            if (listMergeTable[$"{temp.ID}"] != "merge")
            {
                buttonToModify.Text = temp.Name + listMergeTable[$"{temp.ID}"] + Environment.NewLine + temp.Status;
            }
            else
            {
                buttonToModify.Text = temp.Name + Environment.NewLine + "(Tạm ngưng)";
            }
            buttonToModify.Click += btn_Click;
            buttonToModify.Tag = temp;

            switch (temp.Status)
            {
                case "Trống":
                    buttonToModify.BackColor = Color.Aqua;
                    break;
                default:
                    buttonToModify.BackColor = Color.LightPink;
                    break;
            }
            if (listMergeTable[$"{temp.ID}"] == "merge")
            {
                buttonToModify.BackColor = Color.BurlyWood;
            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");//en-US
            //Thread.CurrentThread.CurrentCulture = culture;
            txtTotalPrice.Text = totalPrice.ToString("c", culture);
        }
        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        #endregion

        #region Events
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }


        private void fTableManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Xử lý khi nhấn phím Escape
                this.Close();
                //this.Hide();
            }
        }
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            // Các thao tác với món ăn
            f.InsertFood += f_InsertFood;
            f.UpdateFood += f_UpdateFood;
            f.DeleteFood += f_DeleteFood;
            // Các thao tác với danh mục món ăn
            f.InsertCategory += f_InsertCategory;
            f.UpdateCategory += f_UpdateCategory;
            f.DeleteCategory += f_DeleteCategory;
            // Các thao tác với bàn
            f.InsertTable += f_InsertTable;
            f.UpdateTable += f_UpdateTable;
            f.DeleteTable += f_DeleteTable;
            f.ShowDialog();
        }

        private void f_DeleteTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void f_UpdateTable(object sender, TableEvent e)
        {
            LoadTable(e.Table.ID);
        }

        private void f_InsertTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void f_DeleteCategory(object sender, EventArgs e)
        {
            LoadCategory();
            int idCategory = (cbCategory.SelectedItem as Category).ID;
            LoadFoodListByCategoryID(idCategory);

            if (lsvBill.Tag != null)
            {
                int idTable = (lsvBill.Tag as Table).ID;
                ShowBill(idTable);
            }
        }

        private void f_UpdateCategory(object sender, EventArgs e)
        {
            LoadCategory();
            int idCategory = (cbCategory.SelectedItem as Category).ID;
            LoadFoodListByCategoryID(idCategory);

            if (lsvBill.Tag != null)
            {
                int idTable = (lsvBill.Tag as Table).ID;
                ShowBill(idTable);
            }
        }

        private void f_InsertCategory(object sender, EventArgs e)
        {
            LoadCategory();
            int idCategory = (cbCategory.SelectedItem as Category).ID;
            LoadFoodListByCategoryID(idCategory);

            if (lsvBill.Tag != null)
            {
                int idTable = (lsvBill.Tag as Table).ID;
                ShowBill(idTable);
            }
        }

        private void f_InsertFood(object sender, EventArgs e)
        {
            int idCategory = (cbCategory.SelectedItem as Category).ID;
            LoadFoodListByCategoryID(idCategory);

            if (lsvBill.Tag != null)
            {
                int idTable = (lsvBill.Tag as Table).ID;
                ShowBill(idTable);
            }
        }
        private void f_UpdateFood(object sender, EventArgs e)
        {
            int idCategory = (cbCategory.SelectedItem as Category).ID;
            LoadFoodListByCategoryID(idCategory);

            if (lsvBill.Tag != null)
            {
                int idTable = (lsvBill.Tag as Table).ID;
                ShowBill(idTable);
            }
        }
        private void f_DeleteFood(object sender, EventArgs e)
        {
            int idCategory = (cbCategory.SelectedItem as Category).ID;
            LoadFoodListByCategoryID(idCategory);

            if (lsvBill.Tag != null)
            {
                int idTable = (lsvBill.Tag as Table).ID;
                ShowBill(idTable);
            }
            LoadTable();
        }


        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null) return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if (cbFood.Items.Count == 0)
            {
                MessageBox.Show("Hãy chọn món ăn đúng với danh mục món");
            }
            else
            {
                // Lấy ra table hiện tại
                Table table = lsvBill.Tag as Table;
                // Lấy ra idBill hiện tại
                if (table == null)
                {
                    MessageBox.Show("Hãy chọn bàn");
                    return;
                }
                if (listMergeTable[$"{table.ID}"] != "merge")
                {
                    int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                    int foodID = (cbFood.SelectedItem as Food).ID;
                    int count = (int)nmFoodCount.Value;

                    if (idBill == -1)
                    {
                        BillDAO.Instance.InsertBill(table.ID);
                        BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
                    }
                    else // Bill đã tồn tại
                    {
                        BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
                    }
                    ShowBill(table.ID);
                    //LoadTable(); 
                    LoadTable(table.ID);
                }
                else
                {
                    MessageBox.Show("Bàn đang tạm ngưng\n->Hãy thêm món ăn vào bàn khác");
                }
            }
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            // Lấy ra table hiện tại
            Table table = lsvBill.Tag as Table;
            // Lấy ra idBill hiện tại
            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }
            if (listMergeTable[$"{table.ID}"] != "merge")
            {
                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
                int discount = (int)nmDiscount.Value;

                double totalPrice = Convert.ToDouble(txtTotalPrice.Text.Split(',')[0].Replace(".", ""));
                double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

                if (idBill != -1)
                {
                    int msg = (int)MessageBox.Show(string.Format("Bạn có chắc chắn thanh toán hóa đơn cho bàn {0}\nTổng tiền - (Tổng tiền x Giảm giá%)\n= {1} - ({1} x {2}%) = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel);
                    if (msg == (int)DialogResult.OK)
                    {
                        BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                        ShowBill(table.ID);
                        //LoadTable();
                        LoadTable(table.ID);
                    }
                }
            }
            else
            {
                MessageBox.Show("Bàn đang tạm ngưng\n->Thanh toán lỗi");
            }
        }
        private void btnMoveTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID; 
            string name1 = (lsvBill.Tag as Table).Name;
            if (listMergeTable[$"{id1}"] == "")
            {
                int id2 = (cbSwitchTable.SelectedItem as Table).ID;
                string name2 = (cbSwitchTable.SelectedItem as Table).Name;
                if (listMergeTable[$"{id2}"] == "")
                {
                    if (id1 != id2)
                    {
                        int msg = (int)MessageBox.Show(string.Format("Bạn có thực sự muốn chuyển bàn {0} qua bàn {1}", name1, name2), "Thông báo", MessageBoxButtons.OKCancel);
                        if (msg == (int)DialogResult.OK)
                        {
                            TableDAO.Instance.SwitchTable(id1, id2, "move");
                            LoadTable(id1);
                            LoadTable(id2);
                            if (lsvBill.Tag != null)
                            {
                                int idTable = (lsvBill.Tag as Table).ID;
                                ShowBill(idTable);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hai bàn này có tên giống nhau mà!");
                    }
                }
                else
                {
                    MessageBox.Show($"{name2} đang được gộp\n->Không thao tác được");
                }
            }
            else
            {
                MessageBox.Show("Bàn đang được gộp\n->Không thao tác được");
            }
        }
        private void btnMergeTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;
            string name1 = (lsvBill.Tag as Table).Name;
            if (listMergeTable[$"{id1}"] != "merge")
            {
                int id2 = (cbSwitchTable.SelectedItem as Table).ID;
                string name2 = (cbSwitchTable.SelectedItem as Table).Name;
                if (listMergeTable[$"{id2}"] == "")
                {
                    if (id1 != id2)
                    {
                        int msg = (int)MessageBox.Show(string.Format("Bạn có thực sự muốn gộp bàn {1} vào bàn {0}", name1, name2), "Thông báo", MessageBoxButtons.OKCancel);
                        if (msg == (int)DialogResult.OK)
                        {
                            listMergeTable[$"{id1}"] += $" + {name2}";
                            listMergeTable[$"{id2}"] = "merge";
                            TableDAO.Instance.SwitchTable(id1, id2, "merge");
                            LoadTable(id1);
                            LoadTable(id2);
                            if (lsvBill.Tag != null)
                            {
                                int idTable = (lsvBill.Tag as Table).ID;
                                ShowBill(idTable);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Hai bàn này giống nhau mà");
                    }
                }
                else
                {
                    MessageBox.Show($"{name2} đang được gộp\n->Không thao tác được");
                }
            }
            else
            {
                MessageBox.Show("Bàn đang được gộp trước đó\n->Không thao tác được");
            }
        }
        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckOut_Click(this, new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());
        }
        #endregion
    }
}
