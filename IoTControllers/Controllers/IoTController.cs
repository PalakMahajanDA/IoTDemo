using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IoTControllers.Controllers
{
    public interface IIoTController
    {
        void  SendEmail(string deviceId, string messageData);
    }
}
