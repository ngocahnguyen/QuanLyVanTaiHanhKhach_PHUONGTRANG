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
    public partial class FrmNhanVien : Form
    {
        private Ketnoi ketnoi = new Ketnoi();
        string maTK = FrmDangNhap.MaTK;
        public FrmNhanVien()
        {
            InitializeComponent();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTTNV ttnv = new FrmTTNV(maTK);
            ttnv.Show();
            this.Hide();

        }

      

        private void FrmNhanVien_Load(object sender, EventArgs e)
        {

        }

        private void quảnLýThôngTinKháchHàngToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmQLKH qlkh = new FrmQLKH();
            qlkh.Show();
            this.Hide();
        }

        private void thôngTinChuyếnĐiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU bd = new FrmBANDAU();
            bd.Show();
            this.Hide();
        }

        private void thôngTinGhếToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU bd = new FrmBANDAU();
            bd.Show();
            this.Hide();
        }

        private void thôngTinChuyếnĐiToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmQLLichTrinh lt = new FrmQLLichTrinh();
            lt.Show();
            this.Hide();
        }

        private void thôngTinGhếToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmQLVe qlVe = new FrmQLVe();
            qlVe.Show();
            this.Hide();
        }

        private void quảnLýĐặtVéToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSLPH slph = new FrmSLPH();
            slph.Show();
            this.Hide();
        }

        private void quảnLýThôngTinKháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLKH qlkh = new FrmQLKH();
            qlkh.Show();
            this.Hide();
        }
    }
}
