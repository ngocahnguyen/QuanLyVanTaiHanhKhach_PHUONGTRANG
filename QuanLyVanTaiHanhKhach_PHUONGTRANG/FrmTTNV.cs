using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVanTaiHanhKhach_PHUONGTRANG
{
    public partial class FrmTTNV : Form
    {
        Ketnoi ketnoi = new Ketnoi();
        private string maTK;
        public FrmTTNV(string maTK)
        {
            InitializeComponent();
            this.maTK = maTK;
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;
            comboBox1.Items.Add("Nam");
            comboBox1.Items.Add("Nữ");
            comboBox1.Items.Add("Khác");

            // Đặt giá trị mặc định là "Nam" hoặc giá trị đã lưu từ cơ sở dữ liệu
            comboBox1.SelectedIndex = 0;
        }
        public string GetMaTKFromDatabaseByPhone(string phoneNumber)
        {
            string query = @"SELECT MaTK FROM TaiKhoan WHERE Sodienthoai = @Sodienthoai";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@Sodienthoai", SqlDbType.NVarChar) { Value = phoneNumber }
            };

            DataTable dt = ketnoi.ExecuteQuery(query, parameters);  // Giả sử ExecuteQuery là phương thức của bạn để thực thi truy vấn

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["MaTK"].ToString();
            }
            else
            {
                return null;  // Hoặc thông báo nếu không tìm thấy
            }
        }
        private void FrmTTNV_Load(object sender, EventArgs e)
        {
            LoadUserInfo(FrmDangNhap.MaTK);
        }
        public void LoadUserInfo(string maTK)
        {
            if (string.IsNullOrEmpty(maTK))
            {
                MessageBox.Show("Mã tài khoản không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Câu lệnh SQL để lấy thông tin người dùng
            string query = @"SELECT NV.HoTenNV, TK.Sodienthoai, NV.DiaChi, NV.Email, NV.Gioitinh, NV.Ngaysinh, NV.ChucVu
                     FROM NhanVien NV
                     JOIN TaiKhoan TK ON NV.MaTK = TK.MaTK
                     WHERE TK.MaTK = @MaTK";

            // Tạo tham số
            SqlParameter[] parameters = new SqlParameter[] {
        new SqlParameter("@MaTK", SqlDbType.NVarChar) { Value = maTK }
    };

            try
            {
                // Gọi phương thức ExecuteQuery để lấy dữ liệu
                DataTable dt = ketnoi.ExecuteQuery(query, parameters);

                // Kiểm tra và hiển thị dữ liệu nếu có
                if (dt.Rows.Count > 0)
                {
                    textBox1.Text = dt.Rows[0]["HoTenNV"].ToString();
                    textBox2.Text = dt.Rows[0]["Sodienthoai"].ToString();
                    textBox3.Text = dt.Rows[0]["DiaChi"].ToString();
                    textBox4.Text = dt.Rows[0]["Email"].ToString();
                    comboBox1.SelectedItem = dt.Rows[0]["Gioitinh"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[0]["Ngaysinh"]);
                    textBox5.Text = dt.Rows[0]["ChucVu"].ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin người dùng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // Hàm cập nhật thông tin người dùng
        public void UpdateUserInfo(string maTK)
        {

            if (string.IsNullOrEmpty(maTK))
            {
                MessageBox.Show("Mã tài khoản không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = @"UPDATE NhanVien
                             SET DiaChi = @DiaChi, 
                                 Email = @Email, Gioitinh = @GioiTinh, Ngaysinh = @NgaySinh, ChucVu=@ChucVu
                             WHERE MaTK = @MaTK";

            // Tạo các tham số
            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = textBox3.Text },
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = textBox4.Text },
                new SqlParameter("@GioiTinh", SqlDbType.NVarChar) { Value = comboBox1.SelectedItem.ToString() },
                new SqlParameter("@NgaySinh", SqlDbType.DateTime) { Value = dateTimePicker1.Value },
                new SqlParameter("@ChucVu", SqlDbType.NVarChar) { Value = textBox5.Text },
                new SqlParameter("@MaTK", SqlDbType.NVarChar) { Value = maTK }
            };

            // Gọi phương thức ExecuteNonQuery để thực hiện cập nhật
            int result = ketnoi.ExecuteNonQuery(query, parameters);
            if (result > 0)
            {
                MessageBox.Show("Cập nhật thông tin thành công!");
                LoadUserInfo(FrmDangNhap.MaTK);
            }
            else
            {
                MessageBox.Show("Cập nhật thông tin không thành công!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateUserInfo(FrmDangNhap.MaTK);
        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNhanVien nv = new FrmNhanVien();
            nv.Show();
            this.Hide();
        }

        private void thôngTinChuyếnĐiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLLichTrinh lt = new FrmQLLichTrinh();
            lt.Show();
            this.Hide();
        }

        private void quảnLýThôngTinKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLKH kh = new FrmQLKH();
            kh.Show();
            this.Hide();
        }
    
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTTNV ttnv = new FrmTTNV(maTK);
            ttnv.Show();
            this.Hide();

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU bd = new FrmBANDAU();
            bd.Show();
            this.Hide();
        }

        private void thôngTinGhếToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void lịchTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
