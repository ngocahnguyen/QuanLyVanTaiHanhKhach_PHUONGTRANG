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
    public partial class FrmSLPH : Form
    {
        Ketnoi ketnoi = new Ketnoi();
        string maTK = FrmDangNhap.MaTK;
        public FrmSLPH()
        {
            InitializeComponent();
        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNhanVien nv = new FrmNhanVien();
            nv.Show();
            this.Hide();
        }

        private void lịchTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void thôngTinChuyếnĐiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLLichTrinh lt = new FrmQLLichTrinh();
            lt.Show();
            this.Hide();
        }

        private void thôngTinGhếToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQLVe qLVe = new FrmQLVe();
            qLVe.Show();
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
            FrmQLKH kh = new FrmQLKH();
            kh.Show();
            this.Hide();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTTNV tt = new FrmTTNV(maTK);
            tt.Show();
            this.Hide();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBANDAU bd= new FrmBANDAU();
            bd.Show();
            this.Hide();
        }

        private void btnBackup_Chon_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Backup Files|*.bak";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtBackup.Text = saveFileDialog.FileName;
            }
        }

        private void btnBackup_Saoluu_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = ketnoi.getConnect())
            {
                try
                {
                    string duongdanBackup = txtBackup.Text;
                    if (duongdanBackup == "")
                    {
                        MessageBox.Show("Ban can chon duong dan Backup");
                        return;
                    }

                    string query = $"BACKUP DATABASE [QLBH] TO DISK = '{duongdanBackup}'";

                    SqlCommand command = new SqlCommand(query, ketnoi.getConnect());

                    command.ExecuteNonQuery();

                    MessageBox.Show("Sao luu du lieu thanh cong!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sao luu khong thanh cong! Loi sao luu: " + ex.Message);
                }
            }
        }

        private void btnRestore_Chon_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Backup Files|*.bak";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtRestore.Text = openFileDialog.FileName;
            }
        }

        private void btnRestore_Phuchoi_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = ketnoi.getConnect())
            {
                try
                {
                    string duongdanRestore = txtRestore.Text;
                    if (duongdanRestore == "")
                    {
                        MessageBox.Show("Ban can chon duong dan Restore");
                        return;
                    }

                    string query = $"RESTORE DATABASE [QLBH] FROM DISK = '{duongdanRestore}' WITH REPLACE";

                    SqlCommand command = new SqlCommand(query, ketnoi.getConnect());
                    command.ExecuteNonQuery();

                    MessageBox.Show("Phuc hoi du lieu thanh cong!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Phuc hoi khong thanh cong! Loi phuc hoi: " + ex.Message);
                }
            }
        }
    }
}
