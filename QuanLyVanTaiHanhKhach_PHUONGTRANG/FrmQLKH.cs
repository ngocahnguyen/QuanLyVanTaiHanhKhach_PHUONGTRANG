using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVanTaiHanhKhach_PHUONGTRANG
{
    public partial class FrmQLKH : Form
    {
        Ketnoi ketnoi = new Ketnoi();
        BindingSource bindingSource = new BindingSource();
        public FrmQLKH()
        {
            InitializeComponent();
            comboBox1.Items.Add("Nam");
            comboBox1.Items.Add("Nữ");
            comboBox1.Items.Add("Khác");
        }
        private void FrmQLKH_Load(object sender, EventArgs e)
        {
            LoadData();
            SetupBindingNavigator();
            BindingTextBox();
            
        }
        private void LoadData()
        {
            try
            {
                string query = @"
            SELECT
                KhachHang.MaKH,
                KhachHang.HoTenKH,
                KhachHang.DiaChi,
                KhachHang.Gioitinh,
                KhachHang.Email,
                KhachHang.Ngaysinh
            FROM
                KhachHang
            INNER JOIN
                TaiKhoan ON KhachHang.MaTK = TaiKhoan.MaTK";

                DataTable dt = ketnoi.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    bindingSource.DataSource = dt;
                    dataGridView1.DataSource = bindingSource;

                    dataGridView1.Columns["MaKH"].HeaderText = "Mã Khách Hàng";
                    dataGridView1.Columns["HoTenKH"].HeaderText = "Họ Tên";
                    dataGridView1.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                    dataGridView1.Columns["Gioitinh"].HeaderText = "Giới Tính";
                    dataGridView1.Columns["Email"].HeaderText = "Email";
                    dataGridView1.Columns["Ngaysinh"].HeaderText = "Ngày Sinh";
                }
                else
                {
                    bindingSource.Clear();  // Clear BindingSource if no data
                    dataGridView1.DataSource = null; // Set DataGridView to null
                    MessageBox.Show("No data available to display.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }
        
        
        private void SetupBindingNavigator()
        {
            bindingNavigator1.BindingSource = bindingSource; // Gắn BindingSource vào BindingNavigator
            //bindingNavigator1.AddNewItem.Click += AddNewItem_Click;
            //bindingNavigator1.DeleteItem.Click += DeleteItem_Click;
            //toolStripLabel1.Click += toolStripLabel1_Click;
            //toolStripLabel2.Click += toolStripLabel2_Click; // Sửa dữ liệu
        }
        private void BindingTextBox()
        {
            textBox6.DataBindings.Clear();
            textBox5.DataBindings.Clear();
            textBox3.DataBindings.Clear();
            textBox4.DataBindings.Clear();

            dateTimePicker1.DataBindings.Clear();
            comboBox1.DataBindings.Clear();

            // Now add new bindings
            textBox6.DataBindings.Add("Text", bindingSource, "MaKH");
            textBox5.DataBindings.Add("Text", bindingSource, "HoTenKH");
            textBox3.DataBindings.Add("Text", bindingSource, "Email");
            textBox4.DataBindings.Add("Text", bindingSource, "DiaChi");
            dateTimePicker1.DataBindings.Add("Value", bindingSource, "Ngaysinh");
            comboBox1.DataBindings.Add("Text", bindingSource, "Gioitinh");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                BindingTextBox(); // Tự động hiển thị thông tin khi dòng được chọn
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(keyword))
            {
                // Apply filter
                (bindingSource.DataSource as DataTable).DefaultView.RowFilter = $"HoTenKH LIKE '%{keyword}%'";

                // Check if there are any rows after filtering
                if ((bindingSource.DataSource as DataTable).DefaultView.Count == 0)
                {
                    MessageBox.Show("No matching data found.");
                }
            }
            else
            {
                // Clear filter if no keyword entered
                (bindingSource.DataSource as DataTable).DefaultView.RowFilter = "";
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
           
        }

     

        private void AddNewItem_Click(object sender, EventArgs e)
        {
            string query = @"
        INSERT INTO KhachHang (MaKH, HoTenKH, DiaChi, Email, Gioitinh, Ngaysinh) 
        VALUES (@MaKH, @HoTenKH, @DiaChi, @Email, @Gioitinh, @Ngaysinh)";

            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@MaKH", SqlDbType.NVarChar) { Value = textBox6.Text },
        new SqlParameter("@HoTenKH", SqlDbType.NVarChar) { Value = textBox5.Text },
        new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = textBox4.Text },
        new SqlParameter("@Email", SqlDbType.NVarChar) { Value = textBox3.Text },
        new SqlParameter("@Gioitinh", SqlDbType.NVarChar) { Value = comboBox1.Text },
        new SqlParameter("@Ngaysinh", SqlDbType.Date) { Value = dateTimePicker1.Value }
            };

            try
            {
                ketnoi.ExecuteNonQuery(query, parameters);
                LoadData(); // Reload data after inserting
                MessageBox.Show("Thêm khách hàng thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm khách hàng: {ex.Message}");
            }
        }

        private void SaveData()
        {
            try
            {
                // Cập nhật dữ liệu vào cơ sở dữ liệu
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@HoTenKH", SqlDbType.NVarChar) { Value = textBox5.Text },
            new SqlParameter("@Email", SqlDbType.NVarChar) { Value = textBox3.Text },
            new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = textBox4.Text },
            new SqlParameter("@Gioitinh", SqlDbType.NVarChar) { Value = comboBox1.Text },
            new SqlParameter("@Ngaysinh", SqlDbType.Date) { Value = dateTimePicker1.Value },
            new SqlParameter("@MaKH", SqlDbType.NVarChar) { Value = textBox6.Text }
                };

                // Gọi phương thức ExecuteNonQuery với mảng SqlParameter[]
                ketnoi.ExecuteNonQuery(
                    "UPDATE KhachHang SET HoTenKH = @HoTenKH, Email = @Email, DiaChi = @DiaChi, Gioitinh = @Gioitinh, Ngaysinh = @Ngaysinh WHERE MaKH = @MaKH",
                    parameters
                );

                MessageBox.Show("Dữ liệu đã được lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteItem_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM KhachHang WHERE MaKH = @MaKH";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaKH", SqlDbType.NVarChar) { Value = textBox6.Text }
            };

            try
            {
                ketnoi.ExecuteNonQuery(query, parameters);
                LoadData(); // Reload data after deleting
                MessageBox.Show("Xóa khách hàng thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}");
            }
        }

      
        



        private void Save_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void thôngTinChuyếnĐiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void quảnLýThôngTinKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU bd = new FrmBANDAU();
            bd.Show();
            this.Hide();
        }
        private string maTK;
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTTNV ttnv = new FrmTTNV(maTK);
            ttnv.Show();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDangNhap dn = new FrmDangNhap();
            dn.Show();
            this.Hide();
        }

        private void thôngTinChuyếnĐiToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmQLLichTrinh lt = new FrmQLLichTrinh();
            lt.Show();
            this.Hide();
        }

        private void thôngTinGhếToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLVe qLVe = new FrmQLVe();
            qLVe.Show();
            this.Hide();
        }

        private void quảnLýĐặtVéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSLPH slph = new FrmSLPH();
            slph.Show();
            this.Hide();
        }

        private void quảnLýThôngTinKháchHàngToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmQLKH kh = new FrmQLKH();
            kh.Show();
            this.Hide();
        }
    }
}
