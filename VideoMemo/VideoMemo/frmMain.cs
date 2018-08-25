using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

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
            if (e.KeyCode == Keys.F3)
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

            if (e.KeyCode == Keys.Space)
            {
                if (this.mp.playState == WMPPlayState.wmppsPlaying)
                {
                    this.mp.Ctlcontrols.pause();
                    return;
                }
                if (this.mp.playState == WMPPlayState.wmppsPaused)
                {
                    this.mp.Ctlcontrols.play();
                    return;
                }
            }

            if (e.KeyCode == Keys.F12 && this.mp.playState == WMPPlayState.wmppsPaused)
            {
                btnStep_Click(this, null);
            }

            if (e.KeyCode == Keys.F11 && this.mp.playState == WMPPlayState.wmppsPaused)
            {
                btnStepPrev_Click(this, null);
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
            if (this.mp.playState == WMPLib.WMPPlayState.wmppsPlaying || this.mp.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                this.mp.Ctlcontrols.pause();

                TakePhoto();
                frmAddNote.GetForm(this.mp.URL, mp.Ctlcontrols.currentPosition, mp.Ctlcontrols.currentPositionString).ShowDialog();
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
                .OrderByDescending(x => x.Id)
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
            if (e.KeyCode == Keys.Enter)
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

        private void btnStep_Click(object sender, EventArgs e)
        {
            if (this.mp.playState == WMPPlayState.wmppsPaused)
            {
                IWMPControls2 ctl2 = (IWMPControls2)mp.Ctlcontrols;
                ctl2.step(1);
            }
        }

        private void btnStepPrev_Click(object sender, EventArgs e)
        {
            if (this.mp.playState == WMPPlayState.wmppsPaused)
            {
                IWMPControls2 ctl2 = (IWMPControls2)mp.Ctlcontrols;
                ctl2.step(-1);
            }
        }

        private void TakePhoto()
        {
            if (this.mp.playState == WMPPlayState.wmppsPlaying)
                this.mp.Ctlcontrols.pause();

            if (this.mp.playState == WMPPlayState.wmppsPaused)
            {
                System.Drawing.Image ret = null;
                try
                {
                    // take picture BEFORE saveFileDialog pops up!!
                    Bitmap bitmap = new Bitmap(this.mp.Width, this.mp.Height);
                    {
                        Graphics g = Graphics.FromImage(bitmap);
                        {
                            Graphics gg = this.mp.CreateGraphics();
                            {
                                //timerTakePicFromVideo.Start();
                                this.BringToFront();
                                g.CopyFromScreen(
                                    this.mp.PointToScreen(
                                        new System.Drawing.Point()).X,
                                    this.mp.PointToScreen(
                                        new System.Drawing.Point()).Y,
                                    0, 0,
                                    new System.Drawing.Size(
                                        this.mp.Width,
                                        this.mp.Height)
                                    );
                            }
                        }
                        // afterwards save bitmap file if user wants to
                        string fn = GetSnapShotFn();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            ret = System.Drawing.Image.FromStream(ms);
                            ret.Save(fn);
                        }

                    }
                }
                catch (Exception ex)
                {
                    SetErrorInfo(ex.Message);
                }
            }
        }

        private string GetSnapShotFn()
        {
            string path = Properties.Settings.Default.SnapshotPath;
            if (string.IsNullOrEmpty(path))
            {
                path = Application.StartupPath;
            }

            path = Path.Combine(path, Path.GetFileName(this.mp.URL) + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
            return path;
        }
    }
}
