using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
using System.Threading;
using System.IO;

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

        public Bitmap Capture()
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

            _captureStream.Seek(0, SeekOrigin.Begin);
            _camImage.Save(_captureStream, System.Drawing.Imaging.ImageFormat.Jpeg);

            //if (_captureBmp == null)
            //{
            //    //_captureBmp = new Bitmap(m_uc480.GetDisplayWidth(), m_uc480.GetDisplayHeight());
            //    _captureBmp = (Bitmap)Bitmap.FromStream(_captureStream);
            //}
            Bitmap b = (Bitmap)Bitmap.FromStream(_captureStream);

            //Bitmap b = new Bitmap(_camImage.Width,_camImage.Height);
            //Graphics g = Graphics.FromImage(b);
            //g.DrawImage(_camImage, new Point(0, 0));
            //g.Dispose();

            return b;
            //}else{
            //    return new Bitmap(1,1);
            //}
            
        }

        MemoryStream _captureStream;
        Bitmap _camImage;
        AutoResetEvent _camEvent;
        CameraDriver _camDriver;        
    }
}
