using FCM.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace ConvoyServer.Controllers
{
    public class PositionController : ApiController
    {
        static string lastMessage;

        public string Get()
        {
            return lastMessage;
        }

        // POST: api/Position
        public void Post([FromBody]string value)
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json";
            var objNotification = new
            {
                registration_ids = IdController.registeredIds.ToArray(),
                priority = "high",
                data = new JavaScriptSerializer().Deserialize<object>(value),
                time_to_live = 0
            };

            string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);

            Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
            tRequest.Headers.Add(string.Format("Authorization: key={0}", Constants.FCM_KEY));           
            tRequest.ContentLength = byteArray.Length;
            tRequest.ContentType = "application/json";
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            lastMessage = tReader.ReadToEnd();
                        }
                    }

                }
            }
        }
    }
}
