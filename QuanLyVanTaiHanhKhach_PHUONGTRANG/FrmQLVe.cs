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
    public partial class FrmQLVe : Form
    {
        Ketnoi ketnoi = new Ketnoi();
        string maTK = FrmDangNhap.MaTK;
        BindingSource bindingSource = new BindingSource();
        public FrmQLVe()
        {
            InitializeComponent();
        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNhanVien nv = new FrmNhanVien();
            nv.ShowDialog();
            this.Hide();
        }

        private void lịchTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thôngTinChuyếnĐiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLLichTrinh lt = new FrmQLLichTrinh();
            lt.ShowDialog();
            this.Hide();
        }

        private void thôngTinGhếToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLVe qLVe = new FrmQLVe();
            qLVe.ShowDialog();
            this.Hide();
        }

        private void quảnLýĐặtVéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSLPH slph = new FrmSLPH();
            slph.ShowDialog();
            this.Hide();
        }

        private void quảnLýThôngTinKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLKH kh = new FrmQLKH();
            kh.ShowDialog();
            this.Hide();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTTNV nv = new FrmTTNV(maTK);
            nv.ShowDialog();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU bd = new FrmBANDAU();
            bd.ShowDialog();
            this.Hide();
        }

        private void FrmQLVe_Load(object sender, EventArgs e)
        {
            LoadData();

        }

        private void LoadData()
        {
            string query = $"SELECT * FROM VeXe";
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
            }
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
    }
}
