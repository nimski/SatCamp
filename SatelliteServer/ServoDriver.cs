using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pololu.Usc;
using Pololu.UsbWrapper;

namespace SatelliteServer
{
    class ServoDriver
    {
        public ServoDriver()
        {
            _device = Connect();
        }
        /// <summary>
        /// sets the pwm value of a particular servo
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="value">Target, in units of quarter microseconds.  For typical servos, 6000 is neutral and the acceptable range is 4000-8000.</param>
        public void SetServo(Byte channel, UInt16 value)
        {
            _device.setTarget(channel, value);
        }

        public Usc Connect()
        {
            List<DeviceListItem> connectedDevices = Usc.getConnectedDevices();
            foreach (DeviceListItem dli in connectedDevices)
            {
                // If you have multiple devices connected and want to select a particular
                // device by serial number, you could simply add a line like this:
                //   if (dli.serialNumber != "00012345"){ continue; }

                Usc device = new Usc(dli); // Connect to the device.
                return device;             // Return the device.
            }
            throw new Exception("Could not find servo driver device.  Make sure it is plugged in to USB ");
        }

        private Usc _device;
    }
}
