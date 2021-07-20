using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TracNghiem
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {


        public frmMain()
        {
            InitializeComponent();
        }

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }


        private void btnDangNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            Form frm = this.CheckExists(typeof(frmLogin));
            if (frm != null)
            {
                frm.Activate();
            }
            else
            {
                frmLogin f = new frmLogin();
                f.MdiParent = this;
                f.Show();
            }
        
        }
    }
}
