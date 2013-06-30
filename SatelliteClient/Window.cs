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

namespace SatelliteClient
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
            _updateTimer = new System.Timers.Timer(50);
            _updateTimer.Elapsed += _updateTimer_Elapsed;

            // initialize the client
            NetTcpBinding binding = new NetTcpBinding();
            binding.MaxReceivedMessageSize = 20000000;
            binding.MaxBufferPoolSize = 20000000;
            binding.MaxBufferSize = 20000000;
            _scf = new ChannelFactory<SatelliteServer.ISatService>(
                        binding,
                        "net.tcp://localhost:8000");
           
            _satService = _scf.CreateChannel();

            //while (true)
            //{
            //    Console.Write("CLIENT - Name: ");
            //    string name = Console.ReadLine();
            //    if (name == "") break;

            //    string response = _satService.Ping(name);
            //    Console.WriteLine("CLIENT - Response from service: " + response);
            //}
            //(_satService as ICommunicationObject).Close();
        }



        void _updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.BeginInvoke(new Action(() =>
            {
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
            }));
        }

        private void captureBn_Click(object sender, EventArgs e)
        {
            captureBn.Enabled = false;
            pictureBox.Image = _satService.Capture();
            captureBn.Enabled = true;
        }
      
        System.Timers.Timer _updateTimer;

        private void Window_Load(object sender, EventArgs e)
        {
            _updateTimer.Start();
        }

        ChannelFactory<SatelliteServer.ISatService> _scf;
        SatelliteServer.ISatService _satService;
    }
}
