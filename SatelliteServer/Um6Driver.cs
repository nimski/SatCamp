using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace SatelliteServer
{
    /// <summary>
    /// Driver for the UM6 orientaion sensor
    /// </summary>
    class Um6Driver
    {
        /// <summary>
        /// The euler angles parameter
        /// </summary>
        public double[] Angles { get { lock (this) { return _dAngles; } } }

        public Um6Driver(string portName,int baudRate)
        {
            _port = new SerialPort(portName,baudRate,Parity.None,8,StopBits.One);
            _port.Handshake = Handshake.None;
            _port.ReadTimeout = 500;
            _port.WriteTimeout = 500;

            _dAngles = new double[3];
        }

        /// <summary>
        /// Open the port and initialize the sensor
        /// </summary>
        public void Init()
        {
            try
            {
                _port.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to open COM port " + _port.PortName + " when connecting to UM6 sensor.", e);
            }

            _port.DataReceived += _port_DataReceived;
        }

        /// <summary>
        /// Callback from the serial port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort port = (SerialPort)sender;
                byte[] buffer = new byte[1024];
                //now parse the data we're getting
                int nBytes = port.BytesToRead;
                if (nBytes > 10)
                {
                    nBytes = port.Read(buffer, 0, Math.Min(port.BytesToRead, buffer.Length));
                    bool bValid = false;
                    bool bBatch = false, bData = false;
                    int nDataLength = 0;
                    int nTotalLength = 0;
                    int nStartOffset = -1;
                    int nAddress = -1;

                    //find the start of the packet
                    for (int ii = 0; ii < nBytes-4; ii++)
                    {
                        if (buffer[ii + 0] == 's' && buffer[ii + 1] == 'n' && buffer[ii + 2] == 'p')
                        {
                            nStartOffset = ii;
                            bValid = true;
                        }
                    }
                    
                
                    if (bValid == true)
                    {
                        //augment the number of bytes read
                        nBytes -= nStartOffset;

                        Array.Copy(buffer, nStartOffset, buffer, 0, (buffer.Length - nStartOffset));
                        nStartOffset = 0;

                        

                        //now read the length byte
                        bData = (buffer[nStartOffset+3] & 0x80) != 0;
                        if (bData)
                        {
                            bBatch = (buffer[nStartOffset+3] & 0x40) != 0;
                            nDataLength = (((int)buffer[nStartOffset+3] >> 2) & 0x0F);
                            if (bBatch)
                            {
                                nDataLength *= 4;
                            }
                        }

                        //add the address and checksum bytes
                        nTotalLength = nDataLength + 4;

                        //now read out the rest
                        while (nBytes < nTotalLength)
                        {
                            int nRead = port.Read(buffer, nBytes + nStartOffset, nTotalLength - nBytes);
                            nBytes += nRead;
                        }

                        nAddress = buffer[4 + nStartOffset];

                        //now see if we have a complete packet
                        if (bData && nBytes >= nTotalLength)
                        {
                            //extract the data
                             //Console.WriteLine("Received " + nDataLength.ToString() + " data bytes at address " + nAddress.ToString() + ". Discarding " + (nBytes - nTotalLength).ToString() + " bytes.");

                            //now depending on the address, set some things
                            switch (nAddress)
                            {
                                case Address_Euler_Phi_Theta:
                                    //set the euler angles
                                    byte[] reg1 = buffer.Skip(nStartOffset + 5).Take(4).ToArray();
                                    byte[] reg2 = buffer.Skip(nStartOffset + 5+4).Take(4).ToArray();
                                    Array.Reverse(reg1);
                                    Array.Reverse(reg2);
                                    int nPitch = BitConverter.ToInt16(reg1, 0);
                                    int nRoll = BitConverter.ToInt16(reg1, 2);
                                    int nYaw = BitConverter.ToInt16(reg2, 2);
                                    lock (this)
                                    {
                                        _dAngles[0] = (double)nRoll * Angle_Coefficient;
                                        _dAngles[1] = (double)nPitch * Angle_Coefficient;
                                        _dAngles[2] = (double)nYaw * Angle_Coefficient;
                                    }

                                    //Console.WriteLine("Received euler angles: " + _dAngles[0] + " " + _dAngles[1] + " " + _dAngles[2]);
                                    break;

                                case Address_Euler_Temp:
                                    break;
                            }
                        }
                    }
                }
              
        }

        

        private Thread _readThread;
        private SerialPort _port;
        private double[] _dAngles;
        private const int Address_Euler_Phi_Theta = 0x62;
        private const int Address_Euler_Temp = 0x76;
        private const double Angle_Coefficient = 0.0109863;
    }
}
