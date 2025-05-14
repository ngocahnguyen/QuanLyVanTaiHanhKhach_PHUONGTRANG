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
    public partial class FrmQuenMK : Form
    {
        public FrmQuenMK()
        {
            InitializeComponent();
        }
        private Ketnoi ketnoi = new Ketnoi();
        private void button1_Click(object sender, EventArgs e)
        {
            string soDienThoai = txtsdt.Text;
            string matKhauMoi = txtmk.Text;

            if (string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(matKhauMoi))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại và mật khẩu mới!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (KiemTraSoDienThoai(soDienThoai))
            {
                if (ThayDoiMatKhau(soDienThoai, matKhauMoi))
                {
                    MessageBox.Show("Mật khẩu đã được thay đổi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FrmDangNhap formDangNhap = new FrmDangNhap();
                    formDangNhap.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi thay đổi mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Số điện thoại không tồn tại trong hệ thống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool KiemTraSoDienThoai(string soDienThoai)
        {
            using (SqlConnection conn = ketnoi.getConnect())
            {
                string query = "SELECT COUNT(*) FROM TaiKhoan WHERE SoDienThoai = @SoDienThoai";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private bool ThayDoiMatKhau(string soDienThoai, string matKhauMoi)
        {
            using (SqlConnection conn = ketnoi.getConnect())
            {
                string query = "UPDATE TaiKhoan SET MatKhau = @MatKhauMoi WHERE SoDienThoai = @SoDienThoai";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                    cmd.Parameters.AddWithValue("@MatKhauMoi", matKhauMoi);
                    int result = cmd.ExecuteNonQuery();

                    return result > 0;  // Nếu cập nhật thành công thì trả về true
                }
            }
        }
    }
    
}
