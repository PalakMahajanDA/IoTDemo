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
    public class IotHubGateway
    {
        private readonly IContainer Container;
        private readonly IIoTController _ioTController;
        public IotHubGateway()
        {
            Container = AutofacContainerFactory.Create();
            _ioTController = Container.Resolve<IIoTController>();


        }

        [FunctionName("IotHubListener")]
        public async Task Run([IoTHubTrigger("messages/events", Connection = "IotEventHubConnectionString")] EventData message, ILogger log)
        {
        
            var messageData = (message.Body != null && message.Body.Array.Length > 0) ? Encoding.UTF8.GetString(message.Body.Array) : string.Empty;
            var iotNamespace = typeof(IotControllersModule).Namespace;
       
            var deviceId = message.SystemProperties["iothub-connection-device-id"];
            try
            {
                _ioTController.SendEmail(deviceId.ToString(),messageData);
            }
            catch (Exception e)
            {
                log.LogError(e, $"IoT Hub trigger function processed a message: {messageData}");
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