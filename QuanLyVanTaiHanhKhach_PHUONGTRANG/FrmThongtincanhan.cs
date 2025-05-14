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
    public partial class FrmThongtincanhan : Form
    {
        Ketnoi ketnoi=new Ketnoi();
        private string maTK;
        public FrmThongtincanhan(string maTK)
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

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChonChuyenDi formtt=new FormChonChuyenDi("SomeStringValue", 100.50m);
            formtt.Show();
            this.Hide();
        }

        private void lịchTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLichTrinh formlt=new FrmLichTrinh();
            formlt.Show();
            this.Hide();
        }

        private void quảnLýĐặtVéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTracuuve formtracuu=new FrmTracuuve(FrmDangNhap.MaTK);
            formtracuu.Show();
            this.Hide();
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
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongtincanhan formttcn=new FrmThongtincanhan(maTK);
            formttcn.Show();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU formbd=new FrmBANDAU();
            formbd.Show();
            this.Hide();
        }

        private void FrmThongtincanhan_Load(object sender, EventArgs e)
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
            string query = @"SELECT KH.HoTenKH, TK.Sodienthoai, KH.DiaChi, KH.Email, KH.Gioitinh, KH.Ngaysinh
                     FROM KhachHang KH
                     JOIN TaiKhoan TK ON KH.MaTK = TK.MaTK
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
                    textBox1.Text = dt.Rows[0]["HoTenKH"].ToString();
                    textBox2.Text = dt.Rows[0]["Sodienthoai"].ToString();
                    textBox3.Text = dt.Rows[0]["DiaChi"].ToString();
                    textBox4.Text = dt.Rows[0]["Email"].ToString();
                    comboBox1.SelectedItem = dt.Rows[0]["Gioitinh"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[0]["Ngaysinh"]);
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

            string query = @"UPDATE KhachHang
                             SET DiaChi = @DiaChi, 
                                 Email = @Email, Gioitinh = @GioiTinh, Ngaysinh = @NgaySinh
                             WHERE MaTK = @MaTK";

            // Tạo các tham số
            SqlParameter[] parameters = new SqlParameter[]
            {
           
                new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = textBox3.Text },
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = textBox4.Text },
                new SqlParameter("@GioiTinh", SqlDbType.NVarChar) { Value = comboBox1.SelectedItem.ToString() },
                new SqlParameter("@NgaySinh", SqlDbType.DateTime) { Value = dateTimePicker1.Value },
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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void lịchTrìnhToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmLichTrinh lt =new FrmLichTrinh();
            lt.ShowDialog();
            this.Hide();

        }

        private void quảnLýĐặtVéToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmTracuuve tcv = new FrmTracuuve(maTK);
            tcv.ShowDialog();
            this.Hide();
        }

        private void traCứuVéXeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmXemVeXe xvx = new FrmXemVeXe(maTK);
            xvx.Show();
            this.Show();
        }

        private void hỗTrợToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Mở trang web FUTABUS
                System.Diagnostics.Process.Start("https://futabus.vn/ve-chung-toi");
            }
            catch (Exception ex)
            {
                // Nếu có lỗi khi mở trang web
                MessageBox.Show("Không thể mở trang web. Lỗi: " + ex.Message);
            }
        }
    }
}
