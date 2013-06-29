using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace SatelliteServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //_um6Driver = new Um6Driver("COM7", 115200);
            //_um6Driver.Init();
            //_updateTimer = new System.Timers.Timer(50);
            //_updateTimer.Elapsed += _updateTimer_Elapsed;
            //_updateTimer.Start();
            _cameraDriver = new CameraDriver(this.Handle.ToInt64());
            _cameraDriver.Init(pictureBox.Handle.ToInt64());
            _cameraDriver.CameraCapture += _cameraDriver_CameraCapture;
        }

        

        void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                tbRoll.Text = _um6Driver.Angles[0].ToString();
                tbPitch.Text = _um6Driver.Angles[1].ToString();
                tbYaw.Text = _um6Driver.Angles[2].ToString();
            }));
        }

        private void captureBn_Click(object sender, EventArgs e)
        {
            _cameraDriver.Capture();
        }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages
            if (_cameraDriver != null)
            {
                switch (m.Msg)
                {
                    case uc480.IS_UC480_MESSAGE:
                        _cameraDriver.HandleMessage(m.Msg, m.LParam.ToInt64(), m.WParam.ToInt32());
                        break;
                }
                
            }
            base.WndProc(ref m);
        }

        void _cameraDriver_CameraCapture(object sender, Bitmap b)
        {
            pictureBox.Image = b;
        }

        Um6Driver _um6Driver;
        CameraDriver _cameraDriver;
        System.Timers.Timer _updateTimer;

        
    }
}
