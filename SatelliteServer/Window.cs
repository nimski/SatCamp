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
using System.ServiceModel;
using System.Net;
using System.Net.Sockets;

namespace SatelliteServer
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
            //_um6Driver = new Um6Driver("COM1", 115200);
            //_um6Driver.Init();
            //_servoDriver = new ServoDriver();
            _updateTimer = new System.Timers.Timer(50);
            _updateTimer.Elapsed += _updateTimer_Elapsed;           
            _cameraDriver = new CameraDriver(this.Handle.ToInt64());
            _cameraDriver.Init(pictureBox.Handle.ToInt64());
            _cameraDriver.CameraCapture += _cameraDriver_CameraCapture;

            // initialize the service
            //_host = new ServiceHost(typeof(SatService));
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxReceivedMessageSize = 20000000;
            binding.MaxBufferPoolSize = 20000000;
            binding.MaxBufferSize = 20000000;
            binding.Security.Mode = SecurityMode.None;
            _host = new ServiceHost(new SatService(_cameraDriver));
            _host.AddServiceEndpoint(typeof(ISatService),
                                   binding,
                                   "net.tcp://localhost:8000");
            _host.Open();
        }



        void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
                if (_um6Driver != null)
                {
                    tbRoll.Text = _um6Driver.Angles[0].ToString();
                    tbPitch.Text = _um6Driver.Angles[1].ToString();
                    tbYaw.Text = _um6Driver.Angles[2].ToString();
                }

                if (_servoDriver != null)
                {
                    if (pitchTrackBar.Value != _lastPitchVal)
                    {
                        _servoDriver.SetServo((Byte)0, (ushort)pitchTrackBar.Value);
                        _lastPitchVal = (ushort)pitchTrackBar.Value;
                    }

                    if (yawTrackBar.Value != _lastYawVal)
                    {
                        _servoDriver.SetServo((Byte)1, (ushort)yawTrackBar.Value);
                        _lastYawVal = (ushort)yawTrackBar.Value;
                    }
                }
            }));
        }

        private void captureBn_Click(object sender, EventArgs e)
        {
            _cameraDriver.StartVideo();
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
        ServoDriver _servoDriver;
        ServiceHost _host;
        ushort _lastPitchVal;
        ushort _lastYawVal;
        System.Timers.Timer _updateTimer;

        private void Window_Load(object sender, EventArgs e)
        {
            _updateTimer.Start();
            GetIpAddress();
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            _updateTimer.Stop();
            _cameraDriver.StopVideo();
        }

        public void GetIpAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            ipLabel.Text = "IP Address: " + localIP;
        }
        
    }
}
