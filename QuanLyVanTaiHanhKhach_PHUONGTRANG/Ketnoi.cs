using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyVanTaiHanhKhach_PHUONGTRANG
{
    internal class Ketnoi
    {
        private string connectionString = "Data Source=DESKTOP-GLO31VG;Initial Catalog=QuanLyVanTaiHanhKhach_PhuongTrang;Integrated Security=True";

        public SqlConnection getConnect()
        {
            SqlConnection Conn = new SqlConnection(connectionString);
            Conn.Open();
            return Conn;
        }
        //public int ExecuteNonQuery(string query)
        //{
        //    int data = 0;
        //    using (SqlConnection kenoi = new SqlConnection(connectionString))
        //    {
        //        kenoi.Open();
        //        SqlCommand cmd = new SqlCommand(query, kenoi);
        //        data = cmd.ExecuteNonQuery();
        //        kenoi.Close();
        //    }
        //    return data;
        //}


        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection ketnoi = new SqlConnection(connectionString))
            {
                ketnoi.Open();
                SqlCommand thucthi = new SqlCommand(query, ketnoi);

                // Nếu có tham số, thêm chúng vào câu lệnh SQL
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        thucthi.Parameters.Add(parameter);
                    }
                }

                // Kiểm tra danh sách tham số đã được thêm vào đúng chưa
                foreach (SqlParameter parameter in thucthi.Parameters)
                {
                    Console.WriteLine($"Parameter: {parameter.ParameterName}, Value: {parameter.Value}");
                }

                // Sử dụng SqlDataAdapter để điền dữ liệu vào DataTable
                SqlDataAdapter da = new SqlDataAdapter(thucthi);
                da.Fill(dt);
            }
            return dt;
        }

        
        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            int data = 0;
            using (SqlConnection ketnoi = new SqlConnection(connectionString))
            {
                ketnoi.Open();
                SqlCommand cmd = new SqlCommand(query, ketnoi);

                // Nếu có tham số, thêm chúng vào câu lệnh SQL
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                // Thực hiện câu lệnh và trả về số dòng bị ảnh hưởng
                data = cmd.ExecuteNonQuery();
            }
            return data;
        }
        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            object result = null;
            using (SqlConnection ketnoi = new SqlConnection(connectionString))
            {
                ketnoi.Open();
                SqlCommand cmd = new SqlCommand(query, ketnoi);

                // If there are parameters, add them to the command
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                // Execute the query and get the single value
                result = cmd.ExecuteScalar();
            }
            return result;
        }

       
    }
}
