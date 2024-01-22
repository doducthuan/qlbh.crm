using Nancy.Json;
using QLBH_Dion.Repository;
using System.Net;
using FirebaseAdmin.Messaging;

namespace QLBH_Dion.Util
{
    public class FCMExtension
    {
        private IAccountRepository accountRepository;
        private static string serverKey = "AAAAQtjVmis:APA91bESc4uOKy1xUBg8CfL63b_rWzWgXddrWgmFo1ePklxzUKInqWj-YAGuEJAtql0CyjZWiRcFM1GPSABhEZgfBJcFXCsRBxc-bXG-0l_96ojhtL98-PwGe2AmAFXiFdcDFxDJl94T";
        private static string senderId = "287105718827";
        private static string webAddr = "https://fcm.googleapis.com/fcm/send";
        public FCMExtension(IAccountRepository _accountRepository)
        {
            accountRepository = _accountRepository;
        }
        public static string SendNotification(string deviceId, string title, string msg)
        {

            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";

            var payload = new
            {
                to = deviceId,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = msg,
                    title = title,
                }
            };
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public static string SendNotificationToMobile(string deviceId, string title, string msg, string key, int id)
        {
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
            httpWebRequest.Method = "POST";

            var payload = new
            {
                to = deviceId,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = msg,
                    title = title,
                },
                data = new
                {
                    id = id,
                    key = key,
                }
            };
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}
