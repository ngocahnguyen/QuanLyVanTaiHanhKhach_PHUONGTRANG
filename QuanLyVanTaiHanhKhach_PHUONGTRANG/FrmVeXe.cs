using Microsoft.Reporting.WinForms;
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
    public partial class FrmVeXe : Form
    {
        Ketnoi ketnoi=new Ketnoi();
        string maVeXe = FrmXemVeXe.maVeXe;
        public FrmVeXe(string maVeXe)
        {
            InitializeComponent();
            this.maVeXe= maVeXe;
        }

        private void FrmVeXe_Load(object sender, EventArgs e)
        {
            ketnoi.getConnect();
            string query = @"
                SELECT 
                    v.MaVeXe, 
                    v.MaDatve, 
                    v.MaGhe, 
                    c.Diemdi, 
                    c.Diemden, 
                    c.Giatien, 
                    CONVERT(date, c.Ngaykhoihanh) AS NgayKhoiHanh,
                    c.Thoigiankhoihanh, 
                    v.Biensoxe, 
                   v.NgayXuatve
                    
                FROM 
                    VeXe v
                JOIN 
                    Datve d ON v.MaDatve = d.MaDatve
                JOIN 
                    Chuyendi c ON v.MaChuyenDi = c.MaChuyendi
                JOIN 
                    NhanVien nv ON v.MaNV = nv.MaNV
                WHERE 
                    v.TrangThai = N'Sử dụng';";
            SqlDataAdapter data = new SqlDataAdapter(query, ketnoi.getConnect());
            DULIEU danhsach = new DULIEU();
            data.Fill(danhsach.VEXE);
            ReportDataSource report = new ReportDataSource("DataSet1", (DataTable)danhsach.VEXE);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.ReportPath = @"Vexe.rdlc";
            this.reportViewer1.LocalReport.DataSources.Add(report);
            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
    
}
