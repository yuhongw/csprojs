using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VideoMemo
{
    public partial class frmAddNote : Form
    {
        private static frmAddNote _frm;
        private string Path;
        private double Position;

        public frmAddNote()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var ctx = YUDBContext.GetInstance())
            {
                ctx.Videos.Add(new Video { Path = this.Path, Position = this.Position, Key = txtKey.Text });
                ctx.SaveChanges();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static frmAddNote GetForm(string path,double checkPoint)
        {
            if (_frm==null)
                _frm =  new frmAddNote();
            _frm.Path = path;
            _frm.Position = checkPoint;
            return _frm;
        }

        private void frmAddNote_Load(object sender, EventArgs e)
        {
            labPath.Text = this.Path;
            labPosition.Text = this.Position.ToString("F");
        }

        private void frmAddNote_Activated(object sender, EventArgs e)
        {
            txtKey.Focus();
            txtKey.SelectAll();
        }
    }
}
