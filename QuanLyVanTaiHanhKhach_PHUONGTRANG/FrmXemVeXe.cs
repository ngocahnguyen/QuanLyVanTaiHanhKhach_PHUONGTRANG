using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyVanTaiHanhKhach_PHUONGTRANG
{
    public partial class FrmXemVeXe : Form
    {
        Ketnoi ketnoi = new Ketnoi();
        string maDatve = FrmTracuuve.maDV;
        string maTK = FrmDangNhap.MaTK;
        public static string maVeXe { get; set; }
        public FrmXemVeXe(string maDatve)
        {
            InitializeComponent();
            
            this.maDatve = maDatve;
            LoadTicketDetails(this.maDatve);
        }

        private void FrmXemVeXe_Load(object sender, EventArgs e)
        {
            
        }
        private void LoadTicketDetails(string maVeXe)
        {
           

            string query = $"SELECT * FROM VeXe WHERE MaDatve = '{maDatve}'";
            DataTable dt = ketnoi.ExecuteQuery(query);

            if (dt.Rows.Count > 0)
            {
                // Hiển thị dữ liệu vào DataGridView
                dataGridView1.DataSource = dt;

                // Đặt tên hiển thị cho các cột
                dataGridView1.Columns["MaVeXe"].HeaderText = "Mã Vé Xe";
                dataGridView1.Columns["MaGhe"].HeaderText = "Mã Ghế";
                dataGridView1.Columns["Biensoxe"].HeaderText = "Biển Số Xe";
                dataGridView1.Columns["MaChuyenDi"].HeaderText = "Mã Chuyến Đi";
                dataGridView1.Columns["NgayXuatVe"].HeaderText = "Ngày Xuất Vé";
                dataGridView1.Columns["MaNV"].HeaderText = "Mã Nhân Viên";
                dataGridView1.Columns["TrangThai"].HeaderText = "Trạng Thái";
                dataGridView1.Columns["GhiChu"].HeaderText = "Ghi Chú";

                DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
                btnColumn.Name = "btnExport";
                btnColumn.Text = "Xuất Vé";
                btnColumn.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btnColumn);
            }
            else
            {
                MessageBox.Show("Không tìm thấy vé xe này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["btnExport"].Index)
            {
                string maVeXe = dataGridView1.Rows[e.RowIndex].Cells["MaVeXe"].Value.ToString();

                // Kiểm tra thông tin vé xe từ cơ sở dữ liệu
                string query = $"SELECT * FROM VeXe WHERE MaVeXe = '{maVeXe}'";
                DataTable dt = ketnoi.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    // Nếu tìm thấy thông tin vé xe, mở form xem chi tiết
                    FrmVeXe form = new FrmVeXe(maVeXe);
                    form.ShowDialog();
                    this.Hide();
                }
                else
                {
                    // Nếu không tìm thấy vé, thông báo lỗi
                    MessageBox.Show("Không tìm thấy vé xe này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Mở form báo cáo và truyền các thông tin
               
            }
        }

        private void lịchTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLichTrinh lt=new FrmLichTrinh();
            lt.ShowDialog();
            this.Hide();
        }

        private void quảnLýĐặtVéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTracuuve tcv = new FrmTracuuve(maTK);
            tcv.ShowDialog();
            this.Hide();
        }

        private void traCứuVéXeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmXemVeXe vx = new FrmXemVeXe(maVeXe);
            vx.ShowDialog();
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

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChonChuyenDi ccd = new FormChonChuyenDi("SomeStringValue", 100.50m);
            ccd.ShowDialog();
            this.Hide();

        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU bd = new FrmBANDAU();
            bd.ShowDialog();
            this.Hide();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongtincanhan ttcn = new FrmThongtincanhan(maTK);
            ttcn.ShowDialog();
            this.Hide();

        }
    }
}
