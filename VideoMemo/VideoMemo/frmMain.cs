using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoMemo
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            gv.AutoGenerateColumns = false;
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
           

        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 )
            {
                OpenFile();
            }

            if (e.KeyCode == Keys.F5)
            {
                SetKeyWord();
            }

            if (e.KeyCode == Keys.F1)
            {
                txtSearch.Focus();
                txtSearch.SelectAll();
            }
        }

        private void OpenFile()
        {
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                mp.URL = openFileDlg.FileName;
                mp.Ctlcontrols.play();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetKeyWord();
        }

        private void SetKeyWord()
        {
            if (this.mp.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.mp.Ctlcontrols.pause();
                frmAddNote.GetForm(this.mp.URL, mp.Ctlcontrols.currentPosition).ShowDialog();
                this.mp.Ctlcontrols.play();
                RefreshDataDefault();
            }
            else
            {
                SetErrorInfo("不在播放中");
            }
        }

        private void RefreshDataDefault()
        {
            gv.DataSource = null;
            gv.DataSource = YUDBContext.GetInstance().Videos
                .OrderByDescending(x=>x.Id)
                .Take(20)
                .ToList();
        }

        private void SetErrorInfo(string v)
        {
            timer1.Start();
            labError.Text = v;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labError.Text = "";
            timer1.Stop();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            RefreshDataDefault();
            
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OpenItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter )
            {
                if (txtSearch.Text.Trim().Length > 0)
                {
                    gv.DataSource = null;
                    gv.DataSource = YUDBContext.GetInstance().Videos
                        .Where(x => x.Key.Contains(txtSearch.Text.Trim()))
                        .OrderByDescending(x => x.Id)
                        .ToList();
                }
                else
                {
                    RefreshDataDefault();
                }
                txtSearch.SelectAll();
            }
        }
    }
}
