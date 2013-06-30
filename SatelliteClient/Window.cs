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
using SatelliteClient.Properties;
using System.IO;

namespace SatelliteClient
{
    public partial class Window : Form
    {
        bool _bConnected;
        System.Timers.Timer _captureTimer;
        System.Timers.Timer _updateTimer;
        ChannelFactory<SatelliteServer.ISatService> _scf;
        SatelliteServer.ISatService _satService;

        public Window()
        {
            InitializeComponent();
            _captureTimer = new System.Timers.Timer(250);
            _updateTimer = new System.Timers.Timer(50);
            _updateTimer.Elapsed += _updateTimer_Elapsed;
            _captureTimer.Elapsed += _captureTimer_Elapsed;

            

            //while (true)
            //{
            //    Console.Write("CLIENT - Name: ");
            //    string name = Console.ReadLine();
            //    if (name == "") break;

            //    string response = _satService.Ping(name);
            //    Console.WriteLine("CLIENT - Response from service: " + response);
            //}
            //(_satService as ICommunicationObject).Close();
            _bConnected = false;
        }

        void _captureTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _captureTimer.Enabled = false;
            if (_bConnected)
            {
                this.Invoke(new Action(() =>
                {
                    captureBn.Enabled = false;
                    byte[] buffer = _satService.Capture();
                    Console.Write("Received image with " + buffer.Length + " bytes.");
                    MemoryStream stream = new MemoryStream(buffer);
                    pictureBox.Image = new Bitmap(stream);
                    captureBn.Enabled = true;
                }));
            }
            _captureTimer.Enabled = true;
        }



        void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _updateTimer.Enabled = false;
            this.Invoke(new Action(() =>
            {
                if (_bConnected)
                {
                    double[] euler = _satService.GetEulerAngles();
                    tbRoll.Text = euler[0].ToString();
                    tbPitch.Text = euler[1].ToString();
                    tbYaw.Text = euler[2].ToString();

                    pitchTrackBar.Value = _satService.GetServoPos(0);
                    yawTrackBar.Value = _satService.GetServoPos(1);
                    //if (_um6Driver != null)
                    //{
                    //    tbRoll.Text = _um6Driver.Angles[0].ToString();
                    //    tbPitch.Text = _um6Driver.Angles[1].ToString();
                    //    tbYaw.Text = _um6Driver.Angles[2].ToString();
                    //}

                    //if (_servoDriver != null)
                    //{
                    //    _servoDriver.SetServo((Byte)0, (ushort)pitchTrackBar.Value);
                    //    _servoDriver.SetServo((Byte)1, (ushort)yawTrackBar.Value);
                    //}
                }
            }));
            _updateTimer.Enabled = true;
        }

        private void captureBn_Click(object sender, EventArgs e)
        {
            _captureTimer.Enabled = false;
            if (_bConnected)
            {
                captureBn.Enabled = false;
                pictureBox.Image = _satService.Capture();
                captureBn.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please connect to the server first.");
            }
        }
      
        

        private void Window_Load(object sender, EventArgs e)
        {
            _updateTimer.Start();
            ipTb.Text = Settings.Default["IP"].ToString();
        }

        

        private void connectBn_Click(object sender, EventArgs e)
        {
            Settings.Default["IP"] = ipTb.Text;
            try
            {
                // initialize the client
                NetTcpBinding binding = new NetTcpBinding();
                binding.MaxReceivedMessageSize = 20000000;
                binding.MaxBufferPoolSize = 20000000;
                binding.MaxBufferSize = 20000000;
                binding.Security.Mode = SecurityMode.None;
                _scf = new ChannelFactory<SatelliteServer.ISatService>(
                            binding,
                            "net.tcp://" + ipTb.Text + ":8000");
                //"net.tcp://192.168.1.137:8000");

                _satService = _scf.CreateChannel();
            }
            catch (Exception ex)
            {
                connectBn.Enabled = true;
                MessageBox.Show("Failed to connect to server: " + ex.Message);
                _bConnected = false;
                return;
            }

            _bConnected = true;
            connectBn.Enabled = false;
        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            _updateTimer.Stop();
            Settings.Default.Save();
        }

        private void stabilizeCb_CheckedChanged(object sender, EventArgs e)
        {
            if (_bConnected)
            {
                this.BeginInvoke(new Action(() =>
                {
                    _satService.SetStabilization(stabilizeCb.Checked);
                }));
            }
        }

        private void pitchTrackBar_Scroll(object sender, EventArgs e)
        {
            if (_bConnected)
            {
                this.BeginInvoke(new Action(() =>
                {
                    _satService.SetServoPos(0, pitchTrackBar.Value);
                }));
            }
        }

        private void yawTrackBar_Scroll(object sender, EventArgs e)
        {
            if (_bConnected)
            {
                this.BeginInvoke(new Action(() =>
                {
                    _satService.SetServoPos(1, yawTrackBar.Value);
                }));
            }
        }

        private void videoBn_Click(object sender, EventArgs e)
        {
            _captureTimer.Enabled = true;
        }
    }
}
