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
        private string PositionStr="";
        public string SnapShot { get; set; }

        public frmAddNote()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var ctx = YUDBContext.GetInstance())
            {
                ctx.Videos.Add(new Video { Path = this.Path, Position = this.Position,PositionStr = this.PositionStr, Key = txtKey.Text,SnapShot = this.SnapShot });
                ctx.SaveChanges();
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static frmAddNote GetForm(string path,double position,string positionStr)
        {
            if (_frm==null)
                _frm =  new frmAddNote();
            _frm.Path = path;
            _frm.Position = position;
            _frm.PositionStr = positionStr;
            return _frm;
        }

        private void frmAddNote_Load(object sender, EventArgs e)
        {
            labPath.Text = this.Path;
            labPosition.Text = this.PositionStr;
        }

        private void frmAddNote_Activated(object sender, EventArgs e)
        {
            txtKey.Focus();
            txtKey.SelectAll();
        }
    }
}
