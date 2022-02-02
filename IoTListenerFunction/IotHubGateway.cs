using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventHubs;
using System.Text;
using Microsoft.Extensions.Logging;
using Autofac;
using IoTControllers;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.Devices.Common.Exceptions;
using IoTControllers.Controllers;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Net.Mail;

namespace IoTListenerFunction
{
    public  class IotHubGateway
    {
        private readonly IContainer Container;
        public IotHubGateway()
        {
            Container = AutofacContainerFactory.Create();
        }

        [FunctionName("IotHubListener")]
        public async Task Run([IoTHubTrigger("messages/events", Connection = "IotEventHubConnectionString")]EventData message, ILogger log)
        {
            var prop = message.Properties.FirstOrDefault();

            var messageData = (message.Body != null && message.Body.Array.Length > 0) ? Encoding.UTF8.GetString(message.Body.Array) : string.Empty;
            var iotNamespace = typeof(IotControllersModule).Namespace;
            var command = prop.Key;
            //var controllerType = Assembly
            //    .GetExecutingAssembly()
            //    .GetReferencedAssemblies()
            //    .Where(x => x.Name == iotNamespace)
            //    .Select(Assembly.Load)
            //    .SelectMany(x => x.ExportedTypes)
            //    .FirstOrDefault(x => x.Name == command + "IotController");
            //if (controllerType == null) throw new IotHubCommunicationException("Route not found");
            //var controllerInstance = Container.Resolve(controllerType) as IoTController;
            //var methodInfo = controllerType.GetMethod(prop.Value.ToString());

            //var methodParams = new List<object>();

            //if (!string.IsNullOrWhiteSpace(messageData) && messageData != "{}")
            //{
            //    if (messageData.StartsWith('[') && messageData.EndsWith(']'))
            //    {
            //        methodParams = JsonConvert.DeserializeObject<List<object>>(messageData);
            //    }
            //    else if (messageData.StartsWith('{') && messageData.EndsWith('}'))
            //    {
            //        var columns = JsonConvert.DeserializeObject<Dictionary<string, object>>(messageData);
            //        methodParams = columns.Select(col => col.Value).ToList();
            //    }
            //}
            //var deviceId = message.SystemProperties["demodevice"];
            //methodParams.Add(deviceId);
            try
            {
                SendEmail();
                }
            catch (Exception e)
            {
                log.LogError(e, $"IoT Hub trigger function processed a message: {messageData}");
            }

        }

        public static void SendEmail()
        {
            MailMessage mailMessage = new MailMessage("azurefundemo@gmail.com", "azurefundemo@gmail.com");
            mailMessage.Body = "data received";
            mailMessage.Subject = "trigger";
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
            catch(Exception ex)
            { 
            
            }
        }
    }
    public static class ExtensionMethods
    {
        public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            var task = (Task)@this.Invoke(obj, parameters);
            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }
    }
}