using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Collections;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Runtime.InteropServices;

namespace WebApplication1
{
    public partial class AudioTestingForm : System.Web.UI.Page
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void RecordButton(System.Object sender, System.EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            record("open new Type waveaudio Alias recsound", "", 0, 0);
            record("record recsound", "", 0, 0);
        }

        public void StopButton(System.Object sender, System.EventArgs e)
        {
            timer1.Stop();
            timer1.Enabled = false;
            record("save recsound d:\\mic.wav", "", 0, 0);
            record("close recsound", "", 0, 0);
        }

        public void PlayButton(System.Object sender, System.EventArgs e)
        {
            ms = 0;
            h = 0;
            s = 0;
            m = 0;
            timer1.Enabled = false;
            lblhur.Text = "00";
            lblmin.Text = "00";
            lblsecond.Text = "00";
            (new Microsoft.VisualBasic.Devices.Audio()).Play("d:\\mic.wav");
        }
    }
}