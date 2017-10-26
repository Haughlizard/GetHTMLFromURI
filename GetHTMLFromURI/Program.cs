using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GetHTMLFromURI
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string urlString = "https://www.baidu.com";
                Uri url = new Uri(urlString);
                var html = Program.GetHtmlFromUrlAsync(url);
                Program.WriteStringToFile("获取", html.Result);
                Console.WriteLine(html.Result);
            }
        }

        public static async Task<string> GetHtmlFromUrlAsync(Uri url)
        {
            string html = string.Empty;
            HttpWebRequest request = GenerateHttpWebRequest(url);
            request.Accept = "*/*";
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.UseNagleAlgorithm = false;
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                if (CategorizeResponse(response) == ResponseCategories.Success)
                {
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        html = reader.ReadToEnd();
                    }
                }
            }
            return html;
        }

        public static HttpWebRequest GenerateHttpWebRequest(Uri uri)
        {
            // ظॺ؛๔൩൱
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(uri);
            // 返回൩൱
            return httpRequest;
        }
        // POSTዘሜ
        public static HttpWebRequest GenerateHttpWebRequest(Uri uri, string postData, string contentType)
        {
            // ظॺ؛๔൩൱
            HttpWebRequest httpRequest = GenerateHttpWebRequest(uri);
            // इڥ൩൱的字বຕፇ，需要ᇨံ৊ႜ转ᅭ
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            // ยዃ要༵঍ຕ਍的ాඹ类႙
            httpRequest.ContentType = contentType;
            //"application/x-www-form-urlencoded"; ܔᇀ՗ڇ
            //"application/json" ܔᇀjsonຕ਍
            //"application/xml" ܔᇀxmlຕ਍
            // ยዃ要༵঍字符串的ాඹ܈׊
            httpRequest.ContentLength = postData.Length;
            // इൽ൩൱ୁ，ժႀ入݀ք的ຕ਍
            using (Stream requestStream = httpRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            // 返回请求
            return httpRequest;
        }

        public static ResponseCategories CategorizeResponse(HttpWebResponse httpResponse)
        {
            // 为କ૙ت࿄ઠ在HttpStatusCode中ۨᅭ的߸多成پࠀஓ，
            // ُ࿢்تॽॠֱ"成پ"ࠀஓྷ，ݔ而不是使用HttpStatusCode஼ਉ，
            // ᅺ为໲ዘሜକ某些值
            int statusCode = (int)httpResponse.StatusCode;
            if ((statusCode >= 100) && (statusCode <= 199))
            {
                return ResponseCategories.Informational;
            }
            else if ((statusCode >= 200) && (statusCode <= 299))
            {
                return ResponseCategories.Success;
            }
            else if ((statusCode >= 300) && (statusCode <= 399))
            {
                return ResponseCategories.Redirected;
            }
            else if ((statusCode >= 400) && (statusCode <= 499))
            {
                return ResponseCategories.ClientError;
            }
            else if ((statusCode >= 500) && (statusCode <= 599))
            {
                return ResponseCategories.ServerError;
            }
            return ResponseCategories.Unknown;
        }
        public enum ResponseCategories
        {
            Unknown, // 未知代码（<100或>599）
            Informational, // 消息代码（100<=199）
            Success, // 成功代码（200<=299）
            Redirected, // 重定向代码(300<=399)
            ClientError, // 客户端错误代码（400<=499）
            ServerError // ޜ服务器错误代码（500<=599）
        }

        public static void WriteStringToFile(string fileName, string contents)
        {
            StreamWriter sWriter = null;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                sWriter = new StreamWriter(fileStream);
                sWriter.Write(contents);
            }

        }
        public static void AppendStringToFile(string fileName, string contents)
        {
            StreamWriter sWriter = null;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write))
            {
                sWriter = new StreamWriter(fileStream);
                sWriter.Write(contents);
            }
        }
    }
}
