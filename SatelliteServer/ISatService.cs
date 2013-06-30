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
        string Ping(string name);

        [OperationContract]
        Bitmap Capture(); 
    }
}
