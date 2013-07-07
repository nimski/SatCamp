﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;

namespace SatelliteServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class SatService : ISatService
    {
        public SatService(CameraDriver camDriver)
        {
            _camEvent = new AutoResetEvent(false);
            _camDriver = camDriver;
            _camDriver.CameraCapture += _camDriver_CameraCapture;

            _bStabilizationChanged = false;

            _servoPos = new int[10];
            _servoChanged = new bool[10];
            for (int ii = 0; ii < 10; ii++)
            {
                _servoPos[ii] = 6000;
                _servoChanged[ii] = false;
            }
            _eulerAngles = new double[3];
        }

        public void SetStabilization(bool active)
        {
            _bStabilizationActive = active;
            _bStabilizationChanged = true;
        }

        public bool GetStablizationActive()
        {
            return _bStabilizationActive;
        }

        public double[] GetEulerAngles()
        {
            return _eulerAngles;
        }

        public void SetServoPos(int channel, int val)
        {
            _servoPos[channel] = val;
            _servoChanged[channel] = true;
        }

        public int GetServoPos(int channel)
        {
            return _servoPos[channel];
        }

        void _camDriver_CameraCapture(object sender, Bitmap b)
        {
            if (_camImage == null)
            {
                _camImage = new Bitmap(b.Width,b.Height);
            }
            Graphics g = Graphics.FromImage(_camImage);
            g.DrawImage(b, new Point(0, 0));
            g.Dispose();
            _camEvent.Set();
        }

        public string Ping(string name)
        {
            Console.WriteLine("SERVER - Processing Ping('{0}')", name);
            return "Hello, " + name;
        }

        public byte[] Capture()
        {
            if (_camDriver.IsVideoStarted() == false)
            {
                _camDriver.StartVideo();
            }
            // wait for the camera to capture something
            //if (_camDriver.Capture())
            //{
            _camEvent.WaitOne();

            if (_captureStream == null)
            {
                _captureStream = new MemoryStream();
            }

            ImageCodecInfo jpgEncoder = ImageCodecInfo.GetImageEncoders().Single(x => x.FormatDescription == "JPEG");
            System.Drawing.Imaging.Encoder encoder2 = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters parameters = new System.Drawing.Imaging.EncoderParameters(1);
            EncoderParameter parameter = new EncoderParameter(encoder2, 50L);
            parameters.Param[0] = parameter;

            
            _captureStream.Seek(0, SeekOrigin.Begin);
            _camImage.Save(_captureStream, jpgEncoder, parameters);
            //_camImage.Save(_captureStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] buffer = new byte[_captureStream.Length];
            Console.WriteLine("Sending image with size " + buffer.Length);
            //_captureStream.Read(buffer,0,(int)_captureStream.Length);
            buffer = _captureStream.ToArray();

            return buffer;
            
            //return b;            
        }

        public double[] _eulerAngles;
        public int[] _servoPos;
        public bool[] _servoChanged;
        public bool _bStabilizationChanged;
        public  bool _bStabilizationActive;
        MemoryStream _captureStream;
        Bitmap _camImage;
        AutoResetEvent _camEvent;
        CameraDriver _camDriver;        
    }
}
