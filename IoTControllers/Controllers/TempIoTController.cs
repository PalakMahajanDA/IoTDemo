using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace IoTControllers.Controllers
{
    public class TempIoTController : IIoTController
    {


       
        public TempIoTController()
        {

        }

        public void SendEmail(string deviceId, string messageData)
        {
            MailMessage mailMessage = new MailMessage("azurefundemo@gmail.com", "azurefundemo@gmail.com");
            mailMessage.Body = messageData;
            mailMessage.Subject = "Data received from "+ deviceId;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "azurefundemo@gmail.com",
                Password = "azurefunctiondemo1!"
            };
            smtpClient.EnableSsl = true;
            try
            {
                smtpClient.Send(mailMessage);

            }
            catch (Exception ex)
            {

            }
        }

    }

    public class TemperatureModel
    {
        public decimal Wind { get; set; }
        public decimal Humidity { get; set; }
        public decimal  Precipitation { get; set; }
    }
}
