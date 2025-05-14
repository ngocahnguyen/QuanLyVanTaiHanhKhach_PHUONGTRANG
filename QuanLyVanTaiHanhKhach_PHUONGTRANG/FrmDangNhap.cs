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
    public partial class FrmDangNhap : Form
    {
        private Ketnoi ketnoi = new Ketnoi();
        
        public static string MaTK { get; set; }
        public FrmDangNhap()
        {
            InitializeComponent();
            txtmk.UseSystemPasswordChar = true;
        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU formTrangChu= new FrmBANDAU();
            formTrangChu.Show();
            this.Hide();
        }

        private void đăngNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDangNhap formDangNhap= new FrmDangNhap();
            formDangNhap.Show();
            this.Hide();
        }

       

        private void btnDangNhap_Click(object sender, EventArgs e)
        {

            string soDienThoai = txtsdt.Text;
            string matKhau = txtmk.Text;

            if (string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(matKhau))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (radioButton_KH.Checked)
            {
                // Đăng nhập với tư cách là khách hàng
                if (LoginKhachHang(soDienThoai, matKhau))
                {
                    FormChonChuyenDi formKhachHang = new FormChonChuyenDi("SomeStringValue", 100.50m);
                    formKhachHang.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Số điện thoại hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (radioButton_NV.Checked)
            {
                // Đăng nhập với tư cách là nhân viên
                if (LoginNhanVien(soDienThoai, matKhau))
                {
                    FrmNhanVien formNhanVien = new FrmNhanVien();
                    formNhanVien.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Số điện thoại hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại tài khoản để đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool LoginKhachHang(string soDienThoai, string matKhau)
        {
            using (SqlConnection conn = ketnoi.getConnect())
            {
                string query = "SELECT MaTK FROM TaiKhoan WHERE SoDienThoai = @SoDienThoai AND MatKhau = @MatKhau AND Vaitro = 'Khách hàng'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        MaTK = reader["MaTK"].ToString();  // Lấy MaTK
                        
                        return true;
                    }
                    return false;
                }
            }
        }

        private bool LoginNhanVien(string soDienThoai, string matKhau)
        {
            using (SqlConnection conn = ketnoi.getConnect())
            {
                string query = "SELECT MaTK FROM TaiKhoan WHERE SoDienThoai = @SoDienThoai AND MatKhau = @MatKhau AND Vaitro = 'Nhân viên'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        MaTK = reader["MaTK"].ToString();  // Lấy MaTK
             
                        return true;
                    }
                    return false;
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            FrmQuenMK formQuenMatKhau= new FrmQuenMK();
            formQuenMatKhau.Show();
            this.Hide();
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

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {

        }
    }
    
}
