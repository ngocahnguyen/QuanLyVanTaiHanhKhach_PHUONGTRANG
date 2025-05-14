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

    public partial class FrmQLLichTrinh : Form
    {
        Ketnoi ketnoi = new Ketnoi();
        BindingSource bindingSource = new BindingSource();
        public FrmQLLichTrinh()
        {
            InitializeComponent();
            InitializeVScrollBar();
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
        private void FrmQLLichTrinh_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            radioButton1.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            radioButton2.CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
            // Đảm bảo khi bắt đầu, RadioButton "Mã chuyến đi" được chọn mặc định
            radioButton1.Checked = true;
            RadioButton_CheckedChanged(null, null); // Gọi để cập nhật giao diện khi load

            LoadData();
            SetupBindingNavigator();
            BindingTextBox();

            dataGridView1.Columns["MaChuyenDi"].HeaderText = "Mã Chuyến Đi";
            dataGridView1.Columns["Diemdi"].HeaderText = "Điểm Đi";
            dataGridView1.Columns["Diemden"].HeaderText = "Điểm Đến";
            dataGridView1.Columns["Ngaykhoihanh"].HeaderText = "Ngày Khởi Hành";
            dataGridView1.Columns["Thoigiankhoihanh"].HeaderText = "Thời Gian Khởi Hành";
            dataGridView1.Columns["Ngaydukienden"].HeaderText = "Ngày Dự Kiến Đến";
            dataGridView1.Columns["Thoigiandukienden"].HeaderText = "Thời Gian Dự Kiến Đến";
            dataGridView1.Columns["Thoigiandichuyen"].HeaderText = "Thời Gian Di Chuyển";
            dataGridView1.Columns["Soghecontrong"].HeaderText = "Số Ghế Còn Trống";
            dataGridView1.Columns["Loaixe"].HeaderText = "Loại Xe";
            dataGridView1.Columns["Giatien"].HeaderText = "Giá Tiền";
            dataGridView1.Columns["Donvitinh"].HeaderText = "Đơn Vị Tính";
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                // Tìm kiếm theo Mã chuyến đi
                textBox1.Enabled = true;
                textBox2.Enabled = false;
                textBox7.Enabled = false;
            }
            else if (radioButton2.Checked)
            {
                // Tìm kiếm theo Điểm đi/Điểm đến
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                textBox7.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ngayKhoiHanh = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string ngayDuKienDen = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            string query = "";

            // Kiểm tra nếu radioButton1 (tìm theo mã chuyến đi) được chọn
            if (radioButton1.Checked)
            {
                string maChuyenDi = textBox1.Text.Trim();

                if (!string.IsNullOrEmpty(maChuyenDi))
                {
                    query = "SELECT * FROM Chuyendi WHERE MaChuyendi = @MaChuyendi";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@MaChuyendi", SqlDbType.NVarChar) { Value = maChuyenDi }
                    };

                    DataTable dt = ketnoi.ExecuteQuery(query, parameters);
                    if (dt.Rows.Count > 0)
                    {
                        // Cập nhật DataGridView
                        dataGridView1.DataSource = dt;

                        // Cập nhật các TextBox với thông tin chuyến đi
                        DataRow row = dt.Rows[0];

                        // Cập nhật thông tin vào TextBox
                        textBox6.Text = row["MaChuyenDi"].ToString();
                        textBox5.Text = row["Diemdi"].ToString();  // Điểm đi
                        textBox8.Text = row["Diemden"].ToString();  // Điểm đến

                        // Gộp ngày và giờ vào TextBox3 và TextBox10
                        dateTimePicker1.Value = DateTime.Parse(row["Ngaykhoihanh"].ToString());
                        dateTimePicker2.Value = DateTime.Parse(row["Ngaydukienden"].ToString());
                        textBox3.Text = row["Thoigiankhoihanh"].ToString();
                        textBox16.Text = row["Thoigiandukienden"].ToString();
                        textBox11.Text = row["Soghecontrong"].ToString();  // Số ghế còn trống
                        textBox9.Text = row["Loaixe"].ToString();  // Loại xe
                        textBox12.Text = row["Giatien"].ToString();  // Giá tiền
                        textBox13.Text = row["Donvitinh"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy chuyến đi!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập mã chuyến đi để tìm kiếm.");
                }
            }
            // Kiểm tra nếu radioButton2 (tìm theo điểm đi/điểm đến) được chọn
            else if (radioButton2.Checked)
            {
                string diemDi = textBox2.Text.Trim();
                string diemDen = textBox7.Text.Trim();

                if (!string.IsNullOrEmpty(diemDi) && !string.IsNullOrEmpty(diemDen))
                {
                    query = "SELECT * FROM Chuyendi WHERE Diemdi = @Diemdi AND Diemden = @Diemden";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@Diemdi", SqlDbType.NVarChar) { Value = diemDi },
                new SqlParameter("@Diemden", SqlDbType.NVarChar) { Value = diemDen }
                    };

                    DataTable dt = ketnoi.ExecuteQuery(query, parameters);
                    if (dt.Rows.Count > 0)
                    {
                        // Cập nhật DataGridView
                        dataGridView1.DataSource = dt;

                        // Cập nhật các TextBox với thông tin chuyến đi
                        DataRow row = dt.Rows[0];

                        // Cập nhật thông tin vào TextBox
                        textBox2.Text = row["Diemdi"].ToString();  // Điểm đi
                        textBox8.Text = row["Diemden"].ToString();  // Điểm đến

                        // Gộp ngày và giờ vào TextBox3 và TextBox10
                        dateTimePicker1.Value = DateTime.Parse(row["Ngaykhoihanh"].ToString());
                        dateTimePicker2.Value = DateTime.Parse(row["Ngaydukienden"].ToString());
                        textBox3.Text = row["Thoigiankhoihanh"].ToString();
                        textBox16.Text = row["Thoigiandukienden"].ToString();
                        textBox4.Text = row["Thoigiandichuyen"].ToString();
                        textBox11.Text = row["Soghecontrong"].ToString();  // Số ghế còn trống
                        textBox9.Text = row["Loaixe"].ToString();  // Loại xe
                        textBox12.Text = row["Giatien"].ToString();  // Giá tiền
                        textBox13.Text = row["Donvitinh"].ToString();
                    }

                    else
                    {
                        MessageBox.Show("Không tìm thấy chuyến đi!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ điểm đi và điểm đến.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phương thức tìm kiếm.");
            }

        }
        private void LoadData()
        {
            try
            {
                string query = "SELECT * FROM Chuyendi";  // Truy vấn lấy toàn bộ dữ liệu chuyến đi
                DataTable dt = ketnoi.ExecuteQuery(query);  // Giả sử bạn đã có lớp ketnoi cho kết nối DB
                if (dt.Rows.Count > 0)
                {
                    bindingSource.DataSource = dt;
                    dataGridView1.DataSource = bindingSource;
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
            bindingNavigator1.BindingSource = bindingSource;

        }

        private void BindingTextBox()
        {
            textBox6.DataBindings.Clear();
            textBox5.DataBindings.Clear();
            textBox8.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            textBox3.DataBindings.Clear();
            dateTimePicker1.DataBindings.Clear();
            dateTimePicker2.DataBindings.Clear();
            textBox16.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            textBox11.DataBindings.Clear();
            textBox9.DataBindings.Clear();
            textBox12.DataBindings.Clear();
            textBox13.DataBindings.Clear();



            // Now add new bindings
            textBox6.DataBindings.Add("Text", bindingSource, "MaChuyendi");
            textBox5.DataBindings.Add("Text", bindingSource, "Diemdi");
            textBox8.DataBindings.Add("Text", bindingSource, "Diemden");
            dateTimePicker1.DataBindings.Add("Value", bindingSource, "Ngaykhoihanh");     // Ngaykhoihanh
            dateTimePicker2.DataBindings.Add("Value", bindingSource, "Ngaydukienden");
            textBox4.DataBindings.Add("Text", bindingSource, "Thoigiandichuyen"); // Thoigiandichuyen
            textBox3.DataBindings.Add("Text", bindingSource, "Thoigiankhoihanh"); // Thoigiankhoihanh

            textBox16.DataBindings.Add("Text", bindingSource, "Thoigiandukienden");// Thoigiandukienden
            textBox11.DataBindings.Add("Text", bindingSource, "Soghecontrong");   // Soghecontrong
            textBox9.DataBindings.Add("Text", bindingSource, "Loaixe");           // Loaixe
            textBox12.DataBindings.Add("Text", bindingSource, "Giatien");         // Giatien
            textBox13.DataBindings.Add("Text", bindingSource, "Donvitinh");


        }
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            string query = @"
    INSERT INTO Chuyendi (MaChuyendi, Diemdi, Diemden, Ngaykhoihanh, Thoigiankhoihanh, Ngaydukienden, Thoigiandukienden,
    Thoigiandichuyen, Soghecontrong, Loaixe, Giatien, Donvitinh) 
    VALUES (@MaChuyendi, @Diemdi, @Diemden, @Ngaykhoihanh, @Thoigiankhoihanh, @Ngaydukienden, @Thoigiandukienden,
    @Thoigiandichuyen, @Soghecontrong, @Loaixe, @Giatien, @Donvitinh)";

            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@MaChuyendi", SqlDbType.NVarChar) { Value = textBox6.Text },
        new SqlParameter("@Diemdi", SqlDbType.NVarChar) { Value = textBox5.Text },
        new SqlParameter("@Diemden", SqlDbType.NVarChar) { Value = textBox8.Text },
        new SqlParameter("@Ngaykhoihanh", SqlDbType.Date) { Value = dateTimePicker1.Value },
        new SqlParameter("@Thoigiankhoihanh", SqlDbType.Time) { Value = textBox3.Text },
        new SqlParameter("@Ngaydukienden", SqlDbType.Date) { Value = dateTimePicker2.Value },
        new SqlParameter("@Thoigiandukienden", SqlDbType.Time) { Value = textBox16.Text },
        new SqlParameter("@Thoigiandichuyen", SqlDbType.Time) { Value = textBox4.Text },
        new SqlParameter("@Soghecontrong", SqlDbType.Int) { Value = int.Parse(textBox11.Text) },
        new SqlParameter("@Loaixe", SqlDbType.NVarChar) { Value = textBox9.Text },
        new SqlParameter("@Giatien", SqlDbType.Float) { Value = float.Parse(textBox12.Text) },
        new SqlParameter("@Donvitinh", SqlDbType.NVarChar) { Value = textBox13.Text }
            };

            try
            {
                ketnoi.ExecuteNonQuery(query, parameters);
                LoadData(); // Reload data after inserting
                MessageBox.Show("Thêm chuyến đi thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm chuyến đi: {ex.Message}");
            }
            
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Chuyendi WHERE MaChuyendi = @MaChuyendi";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaChuyendi", SqlDbType.NVarChar) { Value = textBox6.Text }
            };

            try
            {
                ketnoi.ExecuteNonQuery(query, parameters);
                LoadData(); // Reload data after deleting
                MessageBox.Show("Xóa chuyến đi thành công.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa chuyến đi: {ex.Message}");
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {

            try
            {
                // Thêm tham số @MaChuyendi vào mảng các tham số
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@MaChuyendi", SqlDbType.NVarChar) { Value = textBox6.Text }, // Thêm tham số @MaChuyendi
            new SqlParameter("@Diemdi", SqlDbType.NVarChar) { Value = textBox5.Text },
            new SqlParameter("@Diemden", SqlDbType.NVarChar) { Value = textBox8.Text },
            new SqlParameter("@Ngaykhoihanh", SqlDbType.Date) { Value = dateTimePicker1.Value.ToString("yyyy-MM-dd") },
            new SqlParameter("@Thoigiankhoihanh", SqlDbType.NVarChar) { Value = textBox3.Text },
            new SqlParameter("@Ngaydukienden", SqlDbType.Date) { Value = dateTimePicker2.Value.ToString("yyyy-MM-dd") },
            new SqlParameter("@Thoigiandukienden", SqlDbType.NVarChar) { Value = textBox16.Text },
            new SqlParameter("@Thoigiandichuyen", SqlDbType.NVarChar) { Value = textBox4.Text },
            new SqlParameter("@Soghecontrong", SqlDbType.Int) { Value = int.Parse(textBox11.Text) },
            new SqlParameter("@Loaixe", SqlDbType.NVarChar) { Value = textBox9.Text },
            new SqlParameter("@Giatien", SqlDbType.Decimal) { Value = decimal.Parse(textBox12.Text) },
            new SqlParameter("@Donvitinh", SqlDbType.NVarChar) { Value = textBox13.Text }
                };

                // Cập nhật dữ liệu vào cơ sở dữ liệu
                int rowsAffected = ketnoi.ExecuteNonQuery(
             "UPDATE Chuyendi SET Diemdi = @Diemdi, " +
             "Diemden = @Diemden, Ngaykhoihanh = @Ngaykhoihanh, Thoigiankhoihanh = @Thoigiankhoihanh," +
             "Ngaydukienden = @Ngaydukienden, Thoigiandukienden=@Thoigiandukienden," +
             "Thoigiandichuyen=@Thoigiandichuyen, Loaixe=@Loaixe, Giatien=@Giatien, Donvitinh=@Donvitinh " +
             "WHERE MaChuyendi = @MaChuyendi",
             parameters
         );

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Dữ liệu đã được lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Gọi lại phương thức tải dữ liệu mới sau khi lưu
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào được cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




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
        private string maTK;
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTTNV tt = new FrmTTNV(maTK);
            tt.Show();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU bd = new FrmBANDAU();
            bd.Show();
            this.Hide();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count > 0)
            //{
            //    DataGridViewRow row = dataGridView1.SelectedRows[0];
            //    textBox6.Text = row.Cells["MaChuyenDi"].Value.ToString();
            //    textBox5.Text = row.Cells["Diemdi"].Value.ToString();
            //    textBox8.Text = row.Cells["Diemden"].Value.ToString();
            //    textBox14.Text= row.Cells["Ngaykhoihanh"].Value.ToString();
            //    textBox3.Text = row.Cells["Thoigiankhoihanh"].Value.ToString();
            //    textBox15.Text = row.Cells["Ngaydukienden"].Value.ToString();
            //    textBox16.Text = row.Cells["Thoigiandukienden"].Value.ToString();
            //    textBox4.Text = row.Cells["Thoigiandichuyen"].Value.ToString();
            //    textBox11.Text = row.Cells["Soghecontrong"].Value.ToString();
            //    textBox12.Text = row.Cells["Giatien"].Value.ToString();
            //    textBox9.Text = row.Cells["Loaixe"].Value.ToString();
            //    textBox13.Text = row.Cells["Donvitinh"].Value.ToString();
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                BindingTextBox(); // Tự động hiển thị thông tin khi dòng được chọn
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void lịchTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thôngTinChuyếnĐiToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmQLLichTrinh lt = new FrmQLLichTrinh();
            lt.Show();
            this.Hide();
        }

        private void thôngTinGhếToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLVe qLVe= new FrmQLVe();
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

