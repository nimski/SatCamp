using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;

namespace SatelliteServer
{
    [ServiceContract]
    public interface ISatService
    {
        [OperationContract]
        void SetStabilization(bool active);

        [OperationContract]
        bool GetStablizationActive();

        [OperationContract]
        void SetServoPos(int channel, int val);

        [OperationContract]
        int GetServoPos(int channel);

        [OperationContract]
        double[] GetEulerAngles();

        [OperationContract]
        Bitmap Capture(); 
    }
}
