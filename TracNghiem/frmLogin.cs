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

namespace TracNghiem
{
    public partial class frmLogin : Form
    {

        private SqlConnection conn_publisher = new SqlConnection();

        private void LayDSPM(String cmd)
        {
            DataTable dt = new DataTable();
            if (conn_publisher.State == ConnectionState.Closed) conn_publisher.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn_publisher);
            da.Fill(dt);
            conn_publisher.Close();
            Program.bds_dspm.DataSource = dt;
            cmbChinhanh.DataSource = Program.bds_dspm;
            cmbChinhanh.DisplayMember = "TENCN"; cmbChinhanh.ValueMember = "TENSERVER";
        }

        public frmLogin()
        {
            InitializeComponent();
        }

        private int KetNoi_CSDLGOC()
        {
            if (conn_publisher != null && conn_publisher.State == ConnectionState.Open)
                conn_publisher.Close();
            try
            {
                conn_publisher.ConnectionString = Program.connstr_publisher;
                conn_publisher.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("1Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại user name và password.\n " + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (KetNoi_CSDLGOC() == 0) return;
            LayDSPM("SELECT * FROM Get_Subscribes");
            cmbChinhanh.SelectedIndex = 1; cmbChinhanh.SelectedIndex = 0;
        }

        private void cmbChinhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Program.servername = cmbChinhanh.SelectedValue.ToString();
            }
            catch (Exception)
            {

            }
        }

        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            Close(); Program.frmChinh.Close();
        }

        private void btn_DangNhap_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text.Trim() == "" || txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Login name và mật khuẩ không được trống", "", MessageBoxButtons.OK);
                return;
            }

            Program.mlogin = txtLogin.Text; Program.password = txtPass.Text;
            if (Program.KetNoi() == 0)
            {
                return;
            }

            Program.mChinhanh = cmbChinhanh.SelectedIndex;
            Program.mloginDN = Program.mlogin;
            Program.passwordDN = Program.password;
            string strLenh = "EXEC SP_Lay_thong_tin_NV_Tu_Login  '" + Program.mlogin + "'";

            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null)
            {
                return;
            }
            Program.myReader.Read();

            Program.username = Program.myReader.GetString(0);
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Login bạn nhập không có quyền truy cập dữ liệu \n Bạn xem lại user name, password", "", MessageBoxButtons.OK);
                return;
            }

            Program.mHoten = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);
            Program.myReader.Close();
            Program.frmChinh.MANV.Text = "Mã NV = " + Program.username;
            Program.frmChinh.HOTEN.Text = "Họ tên = " + Program.mHoten;
            Program.frmChinh.NHOM.Text = "Nhóm = " + Program.mGroup;

        }
    }
}
