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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyVanTaiHanhKhach_PHUONGTRANG
{
    public partial class FrmLichTrinh : Form
    {
        Ketnoi ketnoi=new Ketnoi();
        string maTK = FrmDangNhap.MaTK;
        public FrmLichTrinh()
        {
            InitializeComponent();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
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
        private void FrmLichTrinh_Load(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(tinh);
            listBox2.Items.AddRange(tinh);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


            
            // Cập nhật dữ liệu vào DataGridView (thực hiện một truy vấn để hiển thị kết quả)
            LoadData();
            //DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            //btnColumn.HeaderText = "Tìm chuyến đi";
            //btnColumn.Text = "Tìm chuyến";
            //btnColumn.UseColumnTextForButtonValue = true;  // Đặt giá trị cho nút
            //dataGridView1.Columns.Add(btnColumn);
            dataGridView1.Columns[0].HeaderText = "Điểm đi";         // Column 0: Diemdi
            dataGridView1.Columns[1].HeaderText = "Điểm đến";         // Column 1: Diemden
            dataGridView1.Columns[2].HeaderText = "Loại xe";          // Column 2: Loaixe
            dataGridView1.Columns[3].HeaderText = "Thời gian di chuyển"; // Column 3: Thoigiandichuyen
            dataGridView1.Columns[4].HeaderText = "Giá tiền";
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
        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            FormChonChuyenDi formtc= new FormChonChuyenDi("SomeStringValue", 100.50m);
            formtc.Show();
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
            FrmTracuuve formtracuu= new FrmTracuuve(FrmDangNhap.MaTK);
            formtracuu.Show();
            this.Hide();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string diemdi = textBox1.Text;
            string diemden = textBox2.Text;

            if (string.IsNullOrWhiteSpace(diemdi) || string.IsNullOrWhiteSpace(diemden))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Điểm đi và Điểm đến!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng Ketnoi để kết nối và truy vấn CSDL
            string query = "SELECT Diemdi, Diemden, Loaixe, Thoigiandichuyen, Giatien FROM Chuyendi WHERE Diemdi = @Diemdi AND Diemden = @Diemden";

            // Tạo tham số SQL
            SqlParameter[] parameters = {
        new SqlParameter("@Diemdi", SqlDbType.NVarChar) { Value = diemdi },
        new SqlParameter("@Diemden", SqlDbType.NVarChar) { Value = diemden }
    };

            // Gọi phương thức ExecuteQuery để thực hiện truy vấn
            DataTable dt = ketnoi.ExecuteQuery(query, parameters);

            // Kiểm tra nếu không tìm thấy chuyến đi
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy chuyến đi từ " + diemdi + " đến " + diemden + ".", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Dừng lại, không hiển thị DataGridView nếu không có dữ liệu
            }

            // Hiển thị kết quả trong DataGridView nếu tìm thấy chuyến đi
            dataGridView1.DataSource = dt;
        }

        private void LoadData()
        {
            string query = "SELECT Diemdi, Diemden, Loaixe, Thoigiandichuyen, Giatien FROM Chuyendi";
            DataTable dt = ketnoi.ExecuteQuery(query);
            dataGridView1.DataSource = dt;
        }
      
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            tcv.Show();
            this.Hide();
        }

        private void traCứuVéXeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmXemVeXe xvx = new FrmXemVeXe(maTK);
            xvx.Show();
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
