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
using MathNet.Numerics.LinearAlgebra.Double;

namespace SatelliteServer
{
    public partial class Window : Form
    {
        DenseMatrix Twp;
        Um6Driver _um6Driver;
        CameraDriver _cameraDriver;
        ServoDriver _servoDriver;
        SatService _service;
        ServiceHost _host;
        ushort _lastPitchVal;
        ushort _lastYawVal;
        System.Timers.Timer _updateTimer;
        int _stabPitchServo, _stabYawServo;

        const double PitchAngleCoefficient = 11.11;

       DenseMatrix Cart2R(
                            double r,
                            double p,
                            double q
                            )
        {
            DenseMatrix R = new DenseMatrix(3,3);
            // psi = roll, th = pitch, phi = yaw
            double cq, cp, cr, sq, sp, sr;
            cr = Math.Cos( r );
            cp = Math.Cos( p );
            cq = Math.Cos( q );

            sr = Math.Sin( r );
            sp = Math.Sin( p );
            sq = Math.Sin( q );

            R[0,0] = cp*cq;
            R[0,1] = -cr*sq+sr*sp*cq;
            R[0,2] = sr*sq+cr*sp*cq;

            R[1,0] = cp*sq;
            R[1,1] = cr*cq+sr*sp*sq;
            R[1,2] = -sr*cq+cr*sp*sq;

            R[2,0] = -sp;
            R[2,1] = sr*cp;
            R[2,2] = cr*cp;
            return R;
        }

        
        DenseVector GLR2Cart(
            DenseMatrix R
            )
        {
            DenseVector rpq = new DenseVector(3);
            // roll
            rpq[0] = Math.Atan2( R[2,1], R[2,2] );
 
            // pitch
            double det = -R[2,0] * R[2,0] + 1.0;
            if (det <= 0) {
                if (R[2,0] > 0){
                    rpq[1] = -Math.PI / 2.0;
                }
                else{
                    rpq[1] = Math.PI / 2.0;
                }
            }
            else{
                rpq[1] = -Math.Asin(R[2,0]);
            }
 
            // yaw
            rpq[2] = Math.Atan2(R[1,0], R[0,0]);
 
            return rpq;
        }

        public Window()
        {
            InitializeComponent();
            _um6Driver = new Um6Driver("COM1", 115200);
            _um6Driver.Init();
            _servoDriver = new ServoDriver();
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
            _service = new SatService(_cameraDriver);
            _host = new ServiceHost(_service);
            _host.AddServiceEndpoint(typeof(ISatService),
                                   binding,
                                   "net.tcp://localhost:8000");
            _host.Open();

            _stabPitchServo = 6000;
            _stabYawServo = 6000; 
        }



        void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _updateTimer.Enabled = false;
            this.Invoke(new Action(() =>
            {
                if (_um6Driver != null)
                {
                    tbRoll.Text = _um6Driver.Angles[0].ToString();
                    tbPitch.Text = _um6Driver.Angles[1].ToString();
                    tbYaw.Text = _um6Driver.Angles[2].ToString();
                    _service._eulerAngles = new double[3] { _um6Driver.Angles[0], _um6Driver.Angles[1], _um6Driver.Angles[2] };
                }

                if (_service._servoChanged[0] == true)
                {
                    pitchTrackBar.Value = _service._servoPos[0];
                    _service._servoChanged[0] = false;
                }

                if (_service._servoChanged[1] == true)
                {
                    yawTrackBar.Value = _service._servoPos[1];
                    _service._servoChanged[1] = false;
                }

                if (_service._bStabilizationChanged)
                {
                    stabilizeCb.Checked = _service._bStabilizationActive;
                    _service._bStabilizationChanged = false;
                }
                _service._bStabilizationActive = stabilizeCb.Checked;

                if (_servoDriver != null)
                {
                    if (pitchTrackBar.Value != _lastPitchVal)
                    {
                        _servoDriver.SetServo((Byte)1, (ushort)pitchTrackBar.Value);
                        _lastPitchVal = (ushort)pitchTrackBar.Value;
                        _service._servoPos[0] = pitchTrackBar.Value;
                    }

                    if (yawTrackBar.Value != _lastYawVal)
                    {
                        _servoDriver.SetServo((Byte)0, (ushort)yawTrackBar.Value);
                        _lastYawVal = (ushort)yawTrackBar.Value;
                        _service._servoPos[1] = yawTrackBar.Value;
                    }
                }

                // do stabilization if necessary
                if (stabilizeCb.Checked)
                {
                    DenseMatrix Twp2 = Cart2R(_um6Driver.Angles[0] * Math.PI / 180.0,
                                              _um6Driver.Angles[1] * Math.PI / 180.0,
                                              _um6Driver.Angles[2] * Math.PI / 180.0);
                    DenseMatrix Tp1p2 = (DenseMatrix)Twp.TransposeThisAndMultiply(Twp2);
                    DenseVector rpq = GLR2Cart(Tp1p2) * 180.0 / Math.PI;

                    pitchTrackBar.Value = _stabPitchServo - (int)(rpq[0] * PitchAngleCoefficient);
                    yawTrackBar.Value = _stabYawServo - (int)(rpq[2] * PitchAngleCoefficient);
                    // calculate angle differences
                    //double dPitch = _um6Driver.Angles[1] - _stabPitch;
                    //pitchTrackBar.Value = _stabPitchServo - (int)(dPitch * PitchAngleCoefficient);

                    //double dYaw = _um6Driver.Angles[2] - _stabYaw;
                    //yawTrackBar.Value = _stabYawServo - (int)(dYaw * PitchAngleCoefficient);
                }
            }));
            _updateTimer.Enabled = true;
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

        

        private void Window_Load(object sender, EventArgs e)
        {
            if (_updateTimer != null)
            {
                _updateTimer.Start();
            }
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

        private void stabilizeCb_CheckedChanged(object sender, EventArgs e)
        {
            if (stabilizeCb.Checked == true)
            {
                Twp = Cart2R(_um6Driver.Angles[0] * Math.PI / 180.0,
                             _um6Driver.Angles[1] * Math.PI / 180.0,
                             _um6Driver.Angles[2] * Math.PI / 180.0);
                _stabPitchServo = pitchTrackBar.Value;
                _stabYawServo = yawTrackBar.Value;
            }
        }
        
    }
}
