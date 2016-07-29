using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DMPSystem.Core.WebModule.Http
{
    public static class HttpClientProvider
    {
        static HttpClientProvider()
        {
            //对象所允许的最大并发连接数//可在配置文件中设置
            System.Net.ServicePointManager.DefaultConnectionLimit = 1024;
            //是否使用 Nagle 不使用 提高效率 
            System.Net.ServicePointManager.UseNagleAlgorithm = false;
            //对象的最大空闲时间.(默认为100秒的)
            System.Net.ServicePointManager.MaxServicePointIdleTime = 3600 * 1000;
        }

     

        public static T GetResponse<T>(string url)
            where T : class, new()
        { 
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.GetAsync(url).Result;
            var result = default(T);

            if (!response.IsSuccessStatusCode) return result;
            var t = response.Content.ReadAsStringAsync();
            var s = t.Result;
            result = JsonConvert.DeserializeObject<T>(s);
            response.Dispose();
            httpClient.Dispose();
            return result;
        }

        public static async Task<string> GetAsyncResponse(string url)
        {
           
               var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
               httpWebRequest.ContentType = "application/json";
               httpWebRequest.Method = "GET";
               httpWebRequest.Timeout = 20000;
            //   httpWebRequest.Proxy = null;
               httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
               httpWebRequest.KeepAlive = true;
               //数据是否缓冲 false 提高效率  
               httpWebRequest.AllowWriteStreamBuffering = false;
               //byte[] btBodys = Encoding.UTF8.GetBytes(body);
               //httpWebRequest.ContentLength = btBodys.Length;
               //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

               var response = await httpWebRequest.GetResponseAsync().Timeout(TimeSpan.FromMilliseconds(20000),"");
              
               using (var stm = response.GetResponseStream())
               {
                   if (stm == null) return null;
                   using (var reader = new StreamReader(stm))
                   {
                       //var content = reader.ReadToEnd();
                       var content = await reader.ReadToEndAsync();
                       return content;
                   }
                   
               }
        }

        public static async Task Timeout(this Task task, TimeSpan timeout, string msg = "")
        {
            var delay = Task.Delay(timeout);
            if (await Task.WhenAny(task, delay) == delay)
            {
                throw new TimeoutException(msg);
            }
        }

        public static async Task<T> Timeout<T>(this Task<T> task, TimeSpan timeout, string msg = "")
        {
            await ((Task)task).Timeout(timeout, msg);
            return await task;
        }

        public static string GetResponse(string url)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;
            httpWebRequest.Proxy = null;
            httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            httpWebRequest.KeepAlive = true;
            //数据是否缓冲 false 提高效率  
            httpWebRequest.AllowWriteStreamBuffering = false;
            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }


        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static T PostResponse<T>(string url, string postData)
            where T : class, new()
        {
            var httpClient = new HttpClient();
            var result = default(T);
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = httpClient.PostAsync(url, httpContent).Result;
            if (!response.IsSuccessStatusCode) return result;
            var t = response.Content.ReadAsStringAsync();
            var s = t.Result;
            result = JsonConvert.DeserializeObject<T>(s);
            return result;
        }

        /// <summary>
        /// V3接口全部为Xml形式，故有此方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T PostXmlResponse<T>(string url, string xmlString)
            where T : class, new()
        { 
            var httpClient = new HttpClient();
            var result = default(T);
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(xmlString);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           
            var response = httpClient.PostAsync(url, httpContent).Result;
            if (!response.IsSuccessStatusCode) return result;
            var t = response.Content.ReadAsStringAsync();
            var s = t.Result;
            result = XmlDeserialize<T>(s);
            response.Dispose();
            httpClient.Dispose();
            return result;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlString)
            where T : class, new()
        {
            try
            {
                var ser = new XmlSerializer(typeof(T));
                using (var reader = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlDeserialize发生异常：xmlString:" + xmlString + "异常信息：" + ex.Message);
            }

        }
    }
}