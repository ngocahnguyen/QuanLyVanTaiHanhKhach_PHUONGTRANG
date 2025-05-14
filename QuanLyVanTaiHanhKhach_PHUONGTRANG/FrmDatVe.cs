using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using WinFormsButton = System.Windows.Forms.Button;

namespace QuanLyVanTaiHanhKhach_PHUONGTRANG
{

    public partial class FrmDatVe : Form
    {
        Ketnoi ketnoi= new Ketnoi();    
        private string maChuyenDi;
        string maTK = FrmDangNhap.MaTK;
        private decimal soVeYeuCau;
        private int soVeDaChon = 0;
        private List<WinFormsButton> danhSachGheDaChon = new List<WinFormsButton>(); // Danh sách các ghế đã chọn
        private decimal giaVeDi;
        private decimal tongTienDi;
        private string maNV;

        private int scrollPosition = 0;
        public static string maDV { get; set; }
        public FrmDatVe(string maChuyenDi, string maTK, decimal soVeYeuCau)
        {
            InitializeComponent();
            InitializeVScrollBar();
            maNV = GetRandomMaNV();
            this.maChuyenDi = maChuyenDi;
            this.maTK = maTK;
            this.soVeYeuCau = soVeYeuCau;
            if (string.IsNullOrEmpty(this.maChuyenDi))
            {
                MessageBox.Show("Mã chuyến đi không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LoadSeatLayout();
            LoadPaymentMethods();
            LoadCustomerInfo();
            LoadTripInfo();
           
        }
        public string GetRandomMaNV()
        {
            string query = "SELECT TOP 1 MaNV FROM NhanVien ORDER BY NEWID()"; // Chọn ngẫu nhiên một mã nhân viên
            DataTable result = ketnoi.ExecuteQuery(query); // Thực hiện truy vấn qua lớp ketnoi

            if (result.Rows.Count > 0)
            {
                return result.Rows[0]["MaNV"].ToString(); // Lấy giá trị MaNV từ kết quả
            }
            return null; // Trả về null nếu không có kết quả
        }
        private void LoadCustomerInfo()
        {
            // Load thông tin khách hàng từ CSDL (ví dụ: tên, số điện thoại, email)
            DataTable customerData = ketnoi.ExecuteQuery(@"SELECT KH.HoTenKH, TK.Sodienthoai, KH.Email
                     FROM KhachHang KH
                     JOIN TaiKhoan TK ON KH.MaTK = TK.MaTK
                     WHERE TK.MaTK = @MaTK",
              new SqlParameter[] { new SqlParameter("@MaTK", maTK) });

            if (customerData.Rows.Count > 0)
            {
                textBox2.Text = customerData.Rows[0]["HoTenKH"].ToString();
                textBox3.Text = customerData.Rows[0]["Sodienthoai"].ToString();
                textBox4.Text = customerData.Rows[0]["Email"].ToString();
                
            }
        }
        private void LoadTripInfo()
        {
            // Load thông tin chuyến đi từ CSDL
            DataTable tripData = ketnoi.ExecuteQuery("SELECT * FROM ChuyenDi WHERE MaChuyendi = @MaChuyendi",
                                                     new SqlParameter[] { new SqlParameter("@MaChuyendi", maChuyenDi) });

            if (tripData.Rows.Count > 0)
            {
                // Lấy thông tin điểm đi và điểm đến
                textBox5.Text = $"{tripData.Rows[0]["DiemDi"]} -> {tripData.Rows[0]["DiemDen"]}";

                // Kết hợp ngày và giờ khởi hành
                string ngayKhoiHanh = tripData.Rows[0]["Ngaykhoihanh"].ToString();  // Chuỗi ngày
                string thoiGianKhoiHanh = tripData.Rows[0]["Thoigiankhoihanh"].ToString();  // Chuỗi giờ
                textBox6.Text = $"{ngayKhoiHanh} {thoiGianKhoiHanh}";

                // Thông tin số lượng vé yêu cầu
                textBox7.Text = soVeYeuCau.ToString();
                giaVeDi = decimal.Parse(tripData.Rows[0]["Giatien"].ToString()); // Giá vé đi
                // Assuming the return price is the same. Modify as needed.
                

                // Tính tổng tiền lượt đi và lượt về
                tongTienDi = soVeYeuCau * giaVeDi;

                textBox11.Text = tongTienDi.ToString();
            }

          
        }

        private void InitializeVScrollBar()
        {
            //Thiết lập các thuộc tính cho VScrollBar
            vScrollBar1.Minimum = 0;
            vScrollBar1.Maximum = 100; // Số lượng lớn hơn để phù hợp với nội dung
            vScrollBar1.LargeChange = 10; // Kích thước phần lớn
            vScrollBar1.SmallChange = 1; // Kích thước phần nhỏ

            // Đăng ký sự kiện cuộn cho VScrollBar
            vScrollBar1.Scroll += new ScrollEventHandler(vScrollBar1_Scroll);
        }
        private void LoadSeatLayout()
        {
            Panel seatPanel = new Panel
            {
                Width = 700,
                Height = 500,
                AutoScroll = true,
                Location = new Point(10, 10),
                BorderStyle= BorderStyle.Fixed3D

            };
            

            this.Controls.Add(seatPanel);

            // Tiêu đề của Panel
            Label panelTitle = new Label
            {
                Text = "CHỌN GHẾ",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(250, 10)
            };
            seatPanel.Controls.Add(panelTitle);

            // Tiêu đề cho tầng A và B
            Label floorALabel = new Label
            {
                Text = "Tầng A",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 50)
            };
            seatPanel.Controls.Add(floorALabel);

            Label floorBLabel = new Label
            {
                Text = "Tầng B",
                Font = new Font("Arial", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(350, 50)
            };
            seatPanel.Controls.Add(floorBLabel);

          
            DataTable seatData = ketnoi.ExecuteQuery("SELECT MaGhe, Trangthai, Tang FROM Ghe WHERE MaChuyendi = @MaChuyendi",
                                                     new SqlParameter[] { new SqlParameter("@MaChuyendi", maChuyenDi) });

            int seatWidth = 50, seatHeight = 50;
            int seatSpacingX = 70, seatSpacingY = 60;
            int startX_A = 20, startY = 80;
            int startX_B = startX_A + 3 * seatSpacingX + 100; // Offset cho Tầng B bên phải với khoảng cách 100 pixel

            // Divider line giữa Tầng A và B
            Label dividerLine = new Label
            {
                Size = new Size(5, 6 * seatSpacingY),
                Location = new Point(startX_A + 3 * seatSpacingX + 50, startY),
                BackColor = Color.Black
            };
            seatPanel.Controls.Add(dividerLine); // Thêm divider vào Panel

            Dictionary<string, Point> seatPositions = new Dictionary<string, Point>
    {
        // Vị trí cho tầng dưới (A)
        {"A01", new Point(startX_A, startY)}, {"A02", new Point(startX_A + 2 * seatSpacingX, startY)},
        {"A03", new Point(startX_A, startY + seatSpacingY)}, {"A04", new Point(startX_A + seatSpacingX, startY + seatSpacingY)}, {"A05", new Point(startX_A + 2 * seatSpacingX, startY + seatSpacingY)},
        {"A06", new Point(startX_A, startY + 2 * seatSpacingY)}, {"A07", new Point(startX_A + seatSpacingX, startY + 2 * seatSpacingY)}, {"A08", new Point(startX_A + 2 * seatSpacingX, startY + 2 * seatSpacingY)},
        {"A09", new Point(startX_A, startY + 3 * seatSpacingY)}, {"A10", new Point(startX_A + seatSpacingX, startY + 3 * seatSpacingY)}, {"A11", new Point(startX_A + 2 * seatSpacingX, startY + 3 * seatSpacingY)},
        {"A12", new Point(startX_A, startY + 4 * seatSpacingY)}, {"A13", new Point(startX_A + seatSpacingX, startY + 4 * seatSpacingY)}, {"A14", new Point(startX_A + 2 * seatSpacingX, startY + 4 * seatSpacingY)},
        {"A15", new Point(startX_A, startY + 5 * seatSpacingY)}, {"A16", new Point(startX_A + seatSpacingX, startY + 5 * seatSpacingY)}, {"A17", new Point(startX_A + 2 * seatSpacingX, startY + 5 * seatSpacingY)},

        // Vị trí cho tầng trên (B) với offset
        {"B01", new Point(startX_B, startY)}, {"B02", new Point(startX_B + 2 * seatSpacingX, startY)},
        {"B03", new Point(startX_B, startY + seatSpacingY)}, {"B04", new Point(startX_B + seatSpacingX, startY + seatSpacingY)}, {"B05", new Point(startX_B + 2 * seatSpacingX, startY + seatSpacingY)},
        {"B06", new Point(startX_B, startY + 2 * seatSpacingY)}, {"B07", new Point(startX_B + seatSpacingX, startY + 2 * seatSpacingY)}, {"B08", new Point(startX_B + 2 * seatSpacingX, startY + 2 * seatSpacingY)},
        {"B09", new Point(startX_B, startY + 3 * seatSpacingY)}, {"B10", new Point(startX_B + seatSpacingX, startY + 3 * seatSpacingY)}, {"B11", new Point(startX_B + 2 * seatSpacingX, startY + 3 * seatSpacingY)},
        {"B12", new Point(startX_B, startY + 4 * seatSpacingY)}, {"B13", new Point(startX_B + seatSpacingX, startY + 4 * seatSpacingY)}, {"B14", new Point(startX_B + 2 * seatSpacingX, startY + 4 * seatSpacingY)},
        {"B15", new Point(startX_B, startY + 5 * seatSpacingY)}, {"B16", new Point(startX_B + seatSpacingX, startY + 5 * seatSpacingY)}, {"B17", new Point(startX_B + 2 * seatSpacingX, startY + 5 * seatSpacingY)}
    };

            foreach (DataRow row in seatData.Rows)
            {
                string seatCode = row["MaGhe"].ToString();
                bool isBooked = (bool)row["Trangthai"];
                string floor = row["Tang"].ToString();

                if (!seatPositions.ContainsKey(seatCode))
                    continue;

                WinFormsButton seatButton = new WinFormsButton
                {
                    Text = seatCode,
                    Width = seatWidth,
                    Height = seatHeight,
                    Location = seatPositions[seatCode],
                    Enabled = !isBooked // Disable if booked
                };
                if (isBooked)
                {
                    seatButton.BackColor = Color.Gray; // Ghế đã đặt
                }
                else
                {
                    seatButton.BackColor = Color.Green; // Ghế còn trống
                }
                seatButton.Click += (sender, e) =>
                {
                    if (seatButton.BackColor == Color.Green && soVeDaChon < soVeYeuCau)
                    {
                        seatButton.BackColor = Color.Yellow; // Đổi màu ghế thành vàng khi được chọn
                        danhSachGheDaChon.Add(seatButton); // Thêm ghế vào danh sách các ghế đã chọn
                        soVeDaChon++;
                    }
                    else if (seatButton.BackColor == Color.Yellow)
                    {
                        seatButton.BackColor = Color.Green; // Đổi màu ghế lại thành xanh nếu bỏ chọn
                        danhSachGheDaChon.Remove(seatButton); // Xóa ghế khỏi danh sách các ghế đã chọn
                        soVeDaChon--;
                    }
                    else if (soVeDaChon >= soVeYeuCau)
                    {
                        MessageBox.Show("Số lượng ghế đã chọn đạt tối đa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                };

                seatPanel.Controls.Add(seatButton);
               
            }
        }

        private void LoadPaymentMethods()
        {
            comboBox1.Items.Add("Tiền mặt");
            comboBox1.Items.Add("Thẻ tín dụng");
            comboBox1.Items.Add("Chuyển khoản");
        }

       
        private void FrmDatVe_Load(object sender, EventArgs e)
        {

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

        private void button1_Click(object sender, EventArgs e)
        {
            string maKH = GetMaKHFromMaTK(maTK);
            //string maDatVe = GenerateMaDatVe(); // Tạo mã vé tự động

            //string maChuyenDi = this.maChuyenDi; // Mã chuyến đi
            int soLuongVe = soVeDaChon; // Số lượng vé đã chọn
            DateTime ngayDat = DateTime.Now; // Ngày đặt vé hiện tại
            decimal tongTien = tongTienDi; // Tổng tiền đã tính

            // Kiểm tra nếu số lượng vé đã chọn không bằng số lượng vé yêu cầu
            if (soVeDaChon != soVeYeuCau)
            {
                MessageBox.Show("Vui lòng chọn đủ số lượng ghế!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra số tiền khách hàng thanh toán có hợp lệ
            if (string.IsNullOrWhiteSpace(textBox16.Text))
            {
                MessageBox.Show("Vui lòng nhập số tiền thanh toán.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng nếu không nhập số tiền thanh toán
            }

            decimal tongTienThanhToan;
            bool isDecimal = decimal.TryParse(textBox16.Text, out tongTienThanhToan); // Kiểm tra xem giá trị nhập vào có phải là số thập phân hợp lệ không

            if (!isDecimal)
            {
                MessageBox.Show("Số tiền thanh toán không hợp lệ. Vui lòng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng nếu giá trị không phải là số hợp lệ
            }

            if (tongTienThanhToan != tongTienDi || tongTienThanhToan < tongTienDi || tongTienThanhToan > tongTienDi)
            {
                MessageBox.Show("Số tiền thanh toán không đúng. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng nếu số tiền không khớp
            }

            string maDatVe = GenerateMaDatVe(); // Tạo mã đặt vé mới

            // Cập nhật bảng Đặt Vé
            string queryDatVe = @"INSERT INTO Datve (MaDatve, MaKH, MaChuyendi, Soluongve, Ngaydat, Tongtien)
                          VALUES (@MaDatVe, @MaKH, @MaChuyendi, @Soluongve, @Ngaydat, @Tongtien)";
            SqlParameter[] paramsDatVe = {
        new SqlParameter("@MaDatVe", maDatVe),
        new SqlParameter("@MaKH", maKH),
        new SqlParameter("@MaChuyendi", maChuyenDi),
        new SqlParameter("@Soluongve", soVeDaChon),
        new SqlParameter("@Ngaydat", DateTime.Now),
        new SqlParameter("@Tongtien", tongTien)
    };
            ketnoi.ExecuteNonQuery(queryDatVe, paramsDatVe);

            // Cập nhật bảng Vé Xe và Ghế
            foreach (WinFormsButton button in danhSachGheDaChon)
            {
                string maGhe = button.Text;  // Assuming the Text of the button holds the seat number (MaGhe)

                if (!string.IsNullOrEmpty(maGhe))
                {
                    // Continue with your logic here
                    string queryVeXe = @"INSERT INTO VeXe (MaVeXe, MaDatve, MaGhe, Biensoxe, MaChuyenDi, NgayXuatVe, MaNV, TrangThai)
                             VALUES (@MaVeXe, @MaDatVe, @MaGhe, @Biensoxe, @MaChuyenDi, @NgayXuatVe, @MaNV, @TrangThai)";
                    string maVeXe = GenerateMaVeXe(); // Tạo mã vé xe

                    SqlParameter[] paramsVeXe = {
            new SqlParameter("@MaVeXe", maVeXe),
            new SqlParameter("@MaDatve", maDatVe),
            new SqlParameter("@MaGhe", maGhe),
            new SqlParameter("@Biensoxe", "Biển số xe"), // Giả định có thông tin biển số xe
            new SqlParameter("@MaChuyenDi", maChuyenDi),
            new SqlParameter("@NgayXuatVe", DateTime.Now),
            new SqlParameter("@MaNV", maNV),
            new SqlParameter("@TrangThai", "Sử dụng")
        };

                    ketnoi.ExecuteNonQuery(queryVeXe, paramsVeXe);  // Execute the insert for VeXe

                    // Cập nhật trạng thái ghế
                    string queryUpdateGhe = "UPDATE Ghe SET Trangthai = 1 WHERE MaGhe = @MaGhe AND MaChuyendi = @MaChuyendi";
                    SqlParameter[] paramsUpdateGhe = {
            new SqlParameter("@MaGhe", maGhe),
            new SqlParameter("@MaChuyendi", maChuyenDi)
        };

                    ketnoi.ExecuteNonQuery(queryUpdateGhe, paramsUpdateGhe);  // Execute the update for Ghe
                }
                else
                {
                    // Handle the case where the button does not have a seat number (Text)
                    MessageBox.Show("Ghế không có mã!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void UpdateAvailableSeats()
        {
            // Cập nhật số ghế còn lại trong bảng Chuyến Đi
            string queryUpdateChuyenDi = "UPDATE ChuyenDi SET SoLuongGheConLai = SoLuongGheConLai - @SoVeDaChon WHERE MaChuyendi = @MaChuyendi";
            SqlParameter[] paramsUpdateChuyenDi = {
        new SqlParameter("@SoVeDaChon", soVeDaChon),
        new SqlParameter("@MaChuyendi", maChuyenDi)
    };
            ketnoi.ExecuteNonQuery(queryUpdateChuyenDi, paramsUpdateChuyenDi);
        }

        private string GenerateMaDatVe()
        {
            return Guid.NewGuid().ToString().Substring(0, 6); // Mã đặt vé dạng ngẫu nhiên
        }

        private string GenerateMaVeXe()
        {
            return Guid.NewGuid().ToString().Substring(0, 6); // Mã vé xe dạng ngẫu nhiên
        }
        private void ThanhToanThanhCong()
        {
            // Cập nhật trạng thái ghế và số ghế còn lại trong chuyến đi
            foreach (var ghe in danhSachGheDaChon)
            {
                // Cập nhật trạng thái ghế (đặt)
                ketnoi.ExecuteNonQuery("UPDATE Ghe SET Trangthai = 1 WHERE MaGhe = @MaGhe AND MaChuyendi = @MaChuyendi",
                    new SqlParameter[] {
                new SqlParameter("@MaGhe", ghe.Name),
                new SqlParameter("@MaChuyendi", maChuyenDi)
                    });

                // Cập nhật số ghế còn lại trong chuyến đi
                ketnoi.ExecuteNonQuery("UPDATE Chuyendi SET Soghecontrong = Soghecontrong - 1 WHERE MaChuyendi = @MaChuyendi",
                    new SqlParameter[] {
                new SqlParameter("@MaChuyendi", maChuyenDi)
                    });
            }

            // Lưu thông tin đặt vé và vé xe
            LuuThongTinDatVe();
        }

        private async void LuuThongTinDatVe()
        {
            string maDatVe;

            do
            {
                maDatVe = TaoMaDatVe();
                var dataTable = ketnoi.ExecuteQuery(
                    "SELECT COUNT(*) AS SoLuong FROM Datve WHERE MaDatve = @MaDatve",
                    new SqlParameter[] { new SqlParameter("@MaDatve", maDatVe) });

                int soLuong = Convert.ToInt32(dataTable.Rows[0]["SoLuong"]);

                if (soLuong > 0)
                {
                    // Nếu mã đặt vé đã tồn tại, tạo mã mới
                    continue;
                }
                else
                {
                    break; // Mã đặt vé là duy nhất
                }
            } while (true);

            string maKH = GetMaKH(maTK); // Lấy MaKH từ MaTK
            ketnoi.ExecuteNonQuery(
                "INSERT INTO Datve (MaDatve, MaKH, MaChuyendi, Soluongve, Ngaydat, Tongtien) VALUES (@MaDatve, @MaKH, @MaChuyendi, @Soluongve, @Ngaydat, @Tongtien)",
                new SqlParameter[] {
            new SqlParameter("@MaDatve", maDatVe),
            new SqlParameter("@MaKH", maKH),
            new SqlParameter("@MaChuyendi", maChuyenDi),
            new SqlParameter("@Soluongve", soVeDaChon),
            new SqlParameter("@Ngaydat", DateTime.Now),
            new SqlParameter("@Tongtien", tongTienDi)
                });

            MessageBox.Show("Đặt vé thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string TaoMaDatVe()
        {
            // Lấy mã đặt vé lớn nhất hiện tại trong bảng Datve
            string query = "SELECT TOP 1 MaDatve FROM Datve ORDER BY MaDatve DESC";
            DataTable dt = ketnoi.ExecuteQuery(query);
            string maDatVeMoi = "DV0001"; // Mã mặc định nếu bảng trống

            if (dt.Rows.Count > 0)
            {
                maDatVeMoi = (int.Parse(dt.Rows[0]["MaDatve"].ToString().Substring(2)) + 1).ToString("D4");
            }

            return "DV" + maDatVeMoi;
        }

        private string TaoMaVeXe()
        {
            // Lấy mã vé xe lớn nhất hiện tại trong bảng VeXe
            string query = "SELECT TOP 1 MaVeXe FROM VeXe ORDER BY MaVeXe DESC";
            DataTable dt = ketnoi.ExecuteQuery(query);
            string maVeXeMoi = "VX0001"; // Mã mặc định nếu bảng trống

            if (dt.Rows.Count > 0)
            {
                maVeXeMoi = (int.Parse(dt.Rows[0]["MaVeXe"].ToString().Substring(2)) + 1).ToString("D4");
            }

            return "VX" + maVeXeMoi;
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

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
       
        private string GetMaKHFromMaTK(string maTK)
        {
            // Truy vấn lấy MaKH từ MaTK
            DataTable customerData = ketnoi.ExecuteQuery(@"SELECT KH.MaKH, KH.HoTenKH, TK.Sodienthoai, KH.Email
                     FROM KhachHang KH
                     JOIN TaiKhoan TK ON KH.MaTK = TK.MaTK
                     WHERE TK.MaTK = @MaTK",
              new SqlParameter[] { new SqlParameter("@MaTK", maTK) });

            if (customerData.Rows.Count > 0)
            {
                return customerData.Rows[0]["MaKH"].ToString();  // Lấy MaKH từ kết quả truy vấn
            }
            else
            {
                MessageBox.Show("Không tìm thấy mã khách hàng với MaTK này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }
  

       
    }
}
