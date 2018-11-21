using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;

namespace CHEER.PresentationLayer
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //using (var client = new WebClient())
                //{
                //    //client.Headers.Add("Vary", "Accept");
                //    client.Headers.Set("Accept", "application/json");
                //    client.Headers.Set("Content-Type", "application/json");
                //    client.Headers.Set("X-Gizwits-User-token", "ef7480695bb941be99b2fda22e9f7f75");
                //    client.Headers.Set("X-Gizwits-Application-Id", "852af2adf0f44bf595ab1085875dc259");
                //    string url = $@"https://api.gizwits.com/app/devices/D4fPnytwFHRJNEgz6CcBMU/raw_data?type=online&start_time=1527782400&end_time=1529319300&skip=0&limit=1000&sort=desc";
                //    client.Encoding = System.Text.Encoding.UTF8;
                //    var response = client.DownloadString(url);
                //    Response.Write(response);
                //}

                //try
                //{
                //    string url = $@"https://api.gizwits.com/app/devices/D4fPnytwFHRJNEgz6CcBMU/raw_data?type=online&start_time=1527782400&end_time=1529319300&skip=0&limit=1000&sort=desc";
                //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

                //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //    request.ProtocolVersion = HttpVersion.Version10;
                //    request.Accept = "*/*";
                //    request.Method = "GET";
                //    request.Headers.Set("X-Gizwits-Application-Id", "852af2adf0f44bf595ab1085875dc259");
                //    request.Headers.Set("X-Gizwits-User-token", "ef7480695bb941be99b2fda22e9f7f75");

                //    String test = String.Empty;
                //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //    {
                //        Stream dataStream = response.GetResponseStream();
                //        StreamReader reader = new StreamReader(dataStream);
                //        test = reader.ReadToEnd();
                //        reader.Close();
                //        dataStream.Close();
                //    }
                //    Response.Write(test);
                //}
                //catch (System.Net.WebException ex)
                //{
                //    var strResponse = (System.Net.HttpWebResponse)ex.Response;//这样获取web服务器返回数据  
                //    Response.Write(strResponse);
                //}

                string HostURI = $@"https://api.gizwits.com/app/devices/D4fPnytwFHRJNEgz6CcBMU/raw_data?type=online&start_time=1527782400&end_time=1529319300&skip=0&limit=1000&sort=desc";
                //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(HostURI);
                //request.Method = "GET";
                //request.Headers.Add("X-Gizwits-Application-Id", "852af2adf0f44bf595ab1085875dc259");
                //request.Headers.Add("X-Gizwits-User-token", "ef7480695bb941be99b2fda22e9f7f75");
                //request.ClientCertificates.Add(new X509Certificate());
                //String test = String.Empty;
                //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                //{
                //    Stream dataStream = response.GetResponseStream();
                //    StreamReader reader = new StreamReader(dataStream);
                //    test = reader.ReadToEnd();
                //    reader.Close();
                //    dataStream.Close();
                //}
                //Response.Write(test);

                //using (var httpClient = new HttpClient())
                //{
                //    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.gizwits.com/app/devices/D4fPnytwFHRJNEgz6CcBMU/raw_data?type=online&start_time=1527782400&end_time=1529319300&skip=0&limit=20&sort=desc"))
                //    {
                //        request.Headers.TryAddWithoutValidation("Accept", "application/json");
                //        request.Headers.TryAddWithoutValidation("X-Gizwits-Application-Id", "852af2adf0f44bf595ab1085875dc259");
                //        request.Headers.TryAddWithoutValidation("X-Gizwits-User-token", "ef7480695bb941be99b2fda22e9f7f75");

                //        var response = httpClient.SendAsync(request).ConfigureAwait(false);

                //    }
                //}

                GetJsonAsync(Response);

            }
        }

        public static async void GetJsonAsync(HttpResponse res)
        {
            //using (var client = new HttpClient())
            //{
            //    var jsonString = await client.GetStringAsync(uri);
            //    return JObject.Parse(jsonString);
            //}

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.gizwits.com/app/devices/D4fPnytwFHRJNEgz6CcBMU/raw_data?type=online&start_time=1529251200&end_time=1529337600&skip=0&limit=20&sort=desc"))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                    request.Headers.TryAddWithoutValidation("X-Gizwits-Application-Id", "852af2adf0f44bf595ab1085875dc259");
                    request.Headers.TryAddWithoutValidation("X-Gizwits-User-token", "ef7480695bb941be99b2fda22e9f7f75");

                    var response = await httpClient.SendAsync(request);
                    res.Write(await response.Content.ReadAsStringAsync());
                }
            }
        }

        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (error == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            Console.WriteLine("X509Certificate [{0}] Policy Error: '{1}'",
                cert.Subject,
                error.ToString());

            return false;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}