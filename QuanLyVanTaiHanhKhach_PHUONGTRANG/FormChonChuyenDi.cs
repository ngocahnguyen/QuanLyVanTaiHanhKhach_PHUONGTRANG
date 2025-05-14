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
    public partial class FormChonChuyenDi : Form
    {
        private Ketnoi ketnoi = new Ketnoi();
        string maTK = FrmDangNhap.MaTK;
        private DataTable chuyendiData = new DataTable();
        private DataTable chuyenveData = new DataTable();
        public static string MaCD { get; set; }
        public static decimal SoVe { get; set; }
        public FormChonChuyenDi(string maCD,decimal  SoVe)
        {
            InitializeComponent();
            InitializeVScrollBar();
            InitializeDataGridView();
        }

        private int scrollPosition = 0;
        private void InitializeVScrollBar()
        {
            // Thiết lập các thuộc tính cho VScrollBar
            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = 100; // Số lượng lớn hơn để phù hợp với nội dung
            vScrollBar1.LargeChange = 10; // Kích thước phần lớn
            vScrollBar1.SmallChange = 1; // Kích thước phần nhỏ

            // Đăng ký sự kiện cuộn cho VScrollBar
            vScrollBar1.Scroll += new ScrollEventHandler(vScrollBar1_Scroll);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            scrollPosition = e.NewValue;
            foreach (Control control in this.Controls)
            {
                if (control != vScrollBar1) // Bỏ qua VScrollBar
                {
                    control.Location = new Point(control.Location.X, control.Location.Y - (scrollPosition - e.OldValue));
                }
            }

        }
        private string[] tinh = new string[]
        {
                "An Giang", "Bà Rịa - Vũng Tàu", "Bắc Giang", "Bắc Ninh", "Bến Tre",
                "Bình Định", "Bình Dương", "Bình Phước", "Cà Mau", "Cần Thơ",
                "Đà Nẵng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Hà Giang","Hậu Giang",
                "Hà Nam", "Hà Tĩnh", "Hải Dương", "Hải Phòng", "Hòa Bình",
                "Hưng Yên", "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu",
                "Lâm Đồng", "Lang Son", "Nam Định", "Ninh Bình", "Ninh Thuận",
                "Phú Thọ", "Phú Yên", "Quảng Bình", "Quảng Nam", "Quảng Ngãi",
                "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh",
                "Thái Bình", "Thái Nguyên", "Thanh Hóa", "Thừa Thiên - Huế", "Tiền Giang",
                "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái",
                "Hà Nội", "Thành phố Hồ Chí Minh"
        };

        private void FormChonChuyenDi_Load(object sender, EventArgs e)
        {
            
             // Khởi tạo các cột cho DataGridView
            listBox1.Items.AddRange(tinh);
            listBox2.Items.AddRange(tinh);
            


        }
    
        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChonChuyenDi formTrangchu = new FormChonChuyenDi("SomeStringValue", 100.50m);
            formTrangchu.Show();
            this.Hide();
        }
      
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmThongtincanhan formttcn = new FrmThongtincanhan(maTK);
            formttcn.Show();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU formbd = new FrmBANDAU();
            formbd.Show();
            this.Hide();
        }

        private void lịchTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLichTrinh formLt = new FrmLichTrinh();
            formLt.Show();
            this.Hide();
        }
      
        private void quảnLýĐặtVéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTracuuve formtracuu = new FrmTracuuve(FrmDangNhap.MaTK);
            formtracuu.Show();
            this.Hide();
        }
        private void UpdateListBox(string text, ListBox listBox)
        {
            listBox.Items.Clear();
            listBox.Visible = true;
            foreach (string item in tinh)
            {
                if (item.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                {
                    listBox.Items.Add(item);
                }
            }
            if (listBox.Items.Count == 0) // Nếu không có gợi ý nào phù hợp, ẩn ListBox
            {
                listBox.Visible = false;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox1.Text.ToLower();
            UpdateListBox(textBox1.Text, listBox1);
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textBox2.Text.ToLower();
            UpdateListBox(textBox2.Text, listBox2);
        }
        private void radioButton_Motchieu_CheckedChanged(object sender, EventArgs e)
        {
            label9.Visible = false;
            dateTimePicker2.Visible = false;
        }

        private void radioButton_Khuhoi_CheckedChanged(object sender, EventArgs e)
        {

            label9.Visible = true;
            dateTimePicker2.Visible = true;
        }
        private void textbox1_Leave(object sender, EventArgs e)
        {
            listBox1.Visible = false; // Ẩn ListBox khi rời khỏi TextBox
        }

        private void textbox2_Leave(object sender, EventArgs e)
        {
            listBox2.Visible = false;
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                // Chọn mục trong ListBox và cập nhật vào TextBox
                textBox1.Text = listBox1.Items[index].ToString();
                listBox1.Visible = false; // Ẩn ListBox sau khi chọn
            }
        }

        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listBox2.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                // Chọn mục trong ListBox và cập nhật vào TextBox
                textBox2.Text = listBox2.Items[index].ToString();
                listBox2.Visible = false; // Ẩn ListBox sau khi chọn
            }
        }
        
        private void btn_TimChuyen_Click(object sender, EventArgs e)
        {
            string diemdi = textBox1.Text;
            string diemden = textBox2.Text;
            DateTime ngayDi = dateTimePicker1.Value.Date;
            DateTime? ngayVe = radioButton_Khuhoi.Checked ? dateTimePicker2.Value.Date : (DateTime?)null;
            int soVe = (int)numericUpDown1.Value;

            // Truy vấn chuyến đi
            string queryDi = @"
        SELECT MaChuyendi, Diemdi, Diemden, 
               Ngaykhoihanh, Thoigiankhoihanh, Ngaydukienden,
               Thoigiandukienden, Thoigiandichuyen,
               Soghecontrong, Loaixe, Giatien, Donvitinh
        FROM Chuyendi
        WHERE Diemdi = @Diemdi
          AND Diemden = @Diemden
          AND Ngaykhoihanh = @NgayKhoiHanh";

            // Truy vấn chuyến về (nếu có)
            string queryVe = @"
        SELECT MaChuyendi, Diemdi, Diemden, 
               Ngaykhoihanh, Thoigiankhoihanh, Ngaydukienden,
               Thoigiandukienden, Thoigiandichuyen,
               Soghecontrong, Loaixe, Giatien, Donvitinh
        FROM Chuyendi
        WHERE Diemdi = @Diemdi
          AND Diemden = @Diemden
          AND CONVERT(date, Ngaykhoihanh) = @NgayVe";

            SqlParameter[] parametersDi = {
        new SqlParameter("@Diemdi", diemdi),
        new SqlParameter("@Diemden", diemden),
        new SqlParameter("@NgayKhoiHanh", ngayDi)
    };

            // Lấy kết quả chuyến đi
            chuyendiData = ketnoi.ExecuteQuery(queryDi, parametersDi);
            if (chuyendiData.Rows.Count > 0)
            {
                int sogheconTrong = Convert.ToInt32(chuyendiData.Rows[0]["Soghecontrong"]);

                // Kiểm tra số lượng vé chọn có hợp lệ
                if (soVe > sogheconTrong)
                {
                    MessageBox.Show("Không tìm thấy chuyến đi phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hiển thị kết quả tìm kiếm vào DataGridView
                DisplaySearchResults();
            }
            else
            {
                MessageBox.Show("Không tìm thấy chuyến đi phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Lấy kết quả chuyến về (nếu có)
            if (ngayVe.HasValue)
            {
                SqlParameter[] parametersVe = {
            new SqlParameter("@Diemdi", diemden),
            new SqlParameter("@Diemden", diemdi),
            new SqlParameter("@NgayVe", ngayVe.Value)
        };
                chuyenveData = ketnoi.ExecuteQuery(queryVe, parametersVe);
                if (chuyenveData.Rows.Count > 0)
                {
                    // Kiểm tra số lượng vé chuyến về có hợp lệ
                    if (soVe > Convert.ToInt32(chuyenveData.Rows[0]["Soghecontrong"]))
                    {
                        MessageBox.Show("Không tìm thấy chuyến về phù hợp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Hiển thị chuyến về vào DataGridView
                    DisplaySearchResults();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy chuyến về phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Hiển thị kết quả tìm kiếm vào DataGridView
            








        }

        public void DisplaySearchResults()
        {
            // Xóa dữ liệu cũ trong DataGridView
            dataGridView1.Rows.Clear();

            // Thêm chuyến đi vào DataGridView
            foreach (DataRow row in chuyendiData.Rows)
            {
                string thoiGianKhoiHanh = Convert.ToDateTime(row["Ngaykhoihanh"]).ToString("dd/MM/yyyy") + " - " + row["Thoigiankhoihanh"].ToString();
                string thoiGianDuKienDen = Convert.ToDateTime(row["Ngaydukienden"]).ToString("dd/MM/yyyy") + " - " + row["Thoigiandukienden"].ToString();

                dataGridView1.Rows.Add(
                    row["MaChuyendi"],
                    row["Diemdi"] + " -> " + row["Diemden"],
                    thoiGianKhoiHanh,
                    thoiGianDuKienDen,
                    row["Thoigiandichuyen"].ToString(),
                    row["Loaixe"].ToString(),
                    row["Soghecontrong"].ToString(),
                    row["Giatien"].ToString(),
                    row["Donvitinh"].ToString(),
                    "Đặt vé"
                );
            }

            // Thêm chuyến về vào DataGridView nếu có
            if ( chuyenveData.Rows.Count > 0)
            {
                foreach (DataRow row in chuyenveData.Rows)
                {
                    string thoiGianKhoiHanh = Convert.ToDateTime(row["Ngaykhoihanh"]).ToString("dd/MM/yyyy") + " - " + row["Thoigiankhoihanh"].ToString();
                    string thoiGianDuKienDen = Convert.ToDateTime(row["Ngaydukienden"]).ToString("dd/MM/yyyy") + " - " + row["Thoigiandukienden"].ToString();

                    dataGridView1.Rows.Add(
                        row["MaChuyendi"],
                        row["Diemdi"] + " -> " + row["Diemden"],
                        thoiGianKhoiHanh,
                        thoiGianDuKienDen,
                        row["Thoigiandichuyen"].ToString(),
                        row["Loaixe"].ToString(),
                        row["Soghecontrong"].ToString(),
                        row["Giatien"].ToString(),
                        row["Donvitinh"].ToString(),
                        "Đặt vé"
                    );
                }
            }
        }
       

        // Hàm khởi tạo DataGridView với các cột cần thiết
        private void InitializeDataGridView()
        {

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


            // Khởi tạo các cột cần thiết


            dataGridView1.Columns.Add("MaChuyendi", "Mã chuyến đi");
            dataGridView1.Columns.Add("DiemDiDen", "Điểm đi -> Điểm đến");
            dataGridView1.Columns.Add("ThoiGianKhoiHanh", "Thời gian khởi hành");
            dataGridView1.Columns.Add("ThoiGianDuKienDen", "Thời gian kiến đến");
            dataGridView1.Columns.Add("Thoigiandichuyen", "Thời gian di chuyển");
            dataGridView1.Columns.Add("LoaiXe", "Loại xe");
            dataGridView1.Columns.Add("SoGheConTrong", "Số ghế còn trống");
            dataGridView1.Columns.Add("GiaTien", "Giá tiền");
            dataGridView1.Columns.Add("Donvitinh", "Đơn vị tính");


            // Thêm cột button cho đặt vé
            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.Name = "DatVe"; // Give it a name for easier reference
            btnColumn.HeaderText = "Đặt vé";
            btnColumn.Text = "Đặt vé";
            btnColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btnColumn);

            // Cập nhật DataGridView để hiển thị
            dataGridView1.Refresh();
            // Cấu hình để DataGridView tự động điều chỉnh kích thước cột và dòng theo nội dung
       

            // Thiết lập màu chữ và màu nền cho các ô dữ liệu
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Blue;

            // Thiết lập màu chữ và màu nền cho tiêu đề
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);

            // Ngăn người dùng thêm hoặc xóa hàng
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;

            // Cập nhật DataGridView để hiển thị
            dataGridView1.Refresh();



        }
    
      
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


            if (e.ColumnIndex == dataGridView1.Columns["DatVe"].Index)
            {
                var maChuyenDi = dataGridView1.Rows[e.RowIndex].Cells["MaChuyendi"].Value.ToString();
                var formDatVe = new FrmDatVe(maChuyenDi, FrmDangNhap.MaTK, numericUpDown1.Value);  // Truyền dữ liệu chuyến đi, tài khoản, số vé
                formDatVe.Show();
                this.Show();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void traCứuVéXeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmXemVeXe xvx = new FrmXemVeXe(maTK);
            xvx.Show();
            this.Hide();
        }

        private void quảnLýĐặtVéToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmTracuuve tcv = new FrmTracuuve(maTK);
            tcv.Show();
            this.Hide();
        }

        private void lịchTrìnhToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmLichTrinh lt = new FrmLichTrinh();
            lt.Show();
            this.Hide();
        }
    }
}

