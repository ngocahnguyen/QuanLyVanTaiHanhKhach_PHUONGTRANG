using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVanTaiHanhKhach_PHUONGTRANG
{
    public partial class FrmTracuuve : Form
    {
        Ketnoi ketnoi = new Ketnoi();
        BindingSource bindingSource = new BindingSource();
        string maTK = FrmDangNhap.MaTK;
        public static string maDV { get; set; }
        public FrmTracuuve(string MaTK)
        {
            InitializeComponent();
        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChonChuyenDi formtc = new FormChonChuyenDi("SomeStringValue", 100.50m);
            formtc.Show();
            this.Hide();
        }

        private void lịchTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLichTrinh formlt = new FrmLichTrinh();
            formlt.Show();
            this.Hide();
        }

        private void quảnLýĐặtVéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTracuuve formtracuu = new FrmTracuuve(maTK);
            formtracuu.Show();
            this.Hide();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongtincanhan formttcn = new FrmThongtincanhan(FrmDangNhap.MaTK);
            formttcn.Show();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU formbd = new FrmBANDAU();
            formbd.Show();
            this.Hide();
        }
        private string GetMaKH(string maTK)
        {
            string query = $"SELECT MaKH FROM KhachHang WHERE MaTK = '{maTK}'";
            DataTable dt = ketnoi.ExecuteQuery(query);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["MaKH"].ToString(); // Lấy Mã KH từ kết quả
            }
            return null;
        }
        private void LoadData()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            string maKH = GetMaKH(maTK); // Lấy MaKH từ MaTK
            if (maKH != null)
            {
                string query = $"SELECT * FROM Datve WHERE MaKH = '{maKH}'";
                DataTable dt = ketnoi.ExecuteQuery(query);
                dataGridView1.DataSource = dt;

                // Đặt tên hiển thị cho các cột
                dataGridView1.Columns["MaDatve"].HeaderText = "Mã Đặt Vé";
                dataGridView1.Columns["MaKH"].HeaderText = "Mã Khách Hàng";
                dataGridView1.Columns["MaChuyendi"].HeaderText = "Mã Chuyến Đi";
                dataGridView1.Columns["Soluongve"].HeaderText = "Số Lượng Vé";
                dataGridView1.Columns["Ngaydat"].HeaderText = "Ngày Đặt";
                dataGridView1.Columns["Tongtien"].HeaderText = "Tổng Tiền";


                DataGridViewButtonColumn btnViewDetails = new DataGridViewButtonColumn();
                btnViewDetails.Name = "btnViewDetails";
                btnViewDetails.HeaderText = "Xem Chi Tiết";
                btnViewDetails.Text = "Xem Chi Tiết";
                btnViewDetails.UseColumnTextForButtonValue = true; // Make the button text always "Xem Chi Tiết"
                dataGridView1.Columns.Add(btnViewDetails);

            }
            else
            {
                MessageBox.Show("Không tìm thấy mã khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FrmTracuuve_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string madatve = textBox1.Text.Trim(); // Lấy mã đặt vé từ textbox
            if (!string.IsNullOrEmpty(madatve))
            {
                string query = $"SELECT * FROM Datve WHERE MaDatve = '{madatve}'";
                DataTable dt = ketnoi.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt; // Hiển thị dữ liệu lên DataGridView
                }
                else
                {
                    MessageBox.Show("Không tìm thấy mã đặt vé này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Load lại tất cả dữ liệu nếu không tìm thấy
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã đặt vé cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["btnViewDetails"].Index)
            {
                // Lấy Mã Đặt Vé từ dòng được chọn
                string maDatve = dataGridView1.Rows[e.RowIndex].Cells["MaDatve"].Value.ToString();

                // Kiểm tra thông tin vé xe từ cơ sở dữ liệu
                string query = $"SELECT * FROM VeXe WHERE MaDatve = '{maDatve}'";
                DataTable dt = ketnoi.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    // Nếu tìm thấy thông tin vé xe, mở form xem chi tiết
                    FrmXemVeXe form = new FrmXemVeXe(maDatve);
                    form.ShowDialog();
                    this.Hide();
                }
                else
                {
                    // Nếu không tìm thấy vé, thông báo lỗi
                    MessageBox.Show("Không tìm thấy vé xe này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ViewTicketDetails(string maDatVe)
        {
            //// Query to get the ticket details
            //string query = $"SELECT * FROM VeXe WHERE MaDatve = '{maDatVe}'";
            //DataTable dt = ketnoi.ExecuteQuery(query);

            //if (dt.Rows.Count > 0)
            //{
            //    // Retrieve ticket details
            //    string maVeXe = dt.Rows[0]["MaVeXe"].ToString();
            //    string maGhe = dt.Rows[0]["MaGhe"].ToString();
            //    string bienSoXe = dt.Rows[0]["Biensoxe"].ToString();
            //    string maChuyenDi = dt.Rows[0]["MaChuyenDi"].ToString();
            //    DateTime ngayXuatVe = Convert.ToDateTime(dt.Rows[0]["NgayXuatVe"]);
            //    string maNV = dt.Rows[0]["MaNV"].ToString();
            //    string trangThai = dt.Rows[0]["TrangThai"].ToString();
            //    string ghiChu = dt.Rows[0]["GhiChu"].ToString();

            //    // Display ticket details in a new form or show in a message box
            //    FrmTicketDetails frmTicketDetails = new FrmTicketDetails(maVeXe, maGhe, bienSoXe, maChuyenDi, ngayXuatVe, maNV, trangThai, ghiChu);
            //    frmTicketDetails.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Không tìm thấy thông tin vé xe!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void traCứuVéXeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmXemVeXe xvx = new FrmXemVeXe(maTK);
            xvx.Show();
            this.Show();
        }

        private void lịchTrìnhToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmLichTrinh lt = new FrmLichTrinh();
            lt.ShowDialog();
            this.Hide();
        }

        private void quảnLýĐặtVéToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmTracuuve tcv = new FrmTracuuve(maTK);
            tcv.ShowDialog();
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
    }
}
