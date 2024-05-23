using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using xNet;

namespace HttpRequestXNet
{
    public class RequestXNet
    {
        public HttpRequest request;

        public RequestXNet(string cookie, string userAgent, string proxy, int typeProxy)
        {
            if (userAgent == "")
            {
                userAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.1 Mobile/15E148 Safari/604.1";
            }
            request = new HttpRequest
            {
                KeepAlive = true,
                AllowAutoRedirect = true,
                Cookies = new CookieDictionary(),
                UserAgent = userAgent
            };
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            request.AddHeader("Accept-Language", "en-US,en;q=0.9");
            if (cookie != "")
            {
                AddCookie(cookie);
            }
            if (!(proxy != ""))
            {
                return;
            }
            switch (proxy.Split(':').Count())
            {
                case 1:
                    if (typeProxy == 0)
                    {
                        request.Proxy = HttpProxyClient.Parse("127.0.0.1:" + proxy);
                    }
                    else
                    {
                        request.Proxy = Socks5ProxyClient.Parse("127.0.0.1:" + proxy);
                    }
                    break;
                case 2:
                    if (typeProxy == 0)
                    {
                        request.Proxy = HttpProxyClient.Parse(proxy);
                    }
                    else
                    {
                        request.Proxy = Socks5ProxyClient.Parse(proxy);
                    }
                    break;
                case 4:
                    if (typeProxy == 0)
                    {
                        request.Proxy = new HttpProxyClient(proxy.Split(':')[0], Convert.ToInt32(proxy.Split(':')[1]), proxy.Split(':')[2], proxy.Split(':')[3]);
                    }
                    else
                    {
                        request.Proxy = new Socks5ProxyClient(proxy.Split(':')[0], Convert.ToInt32(proxy.Split(':')[1]), proxy.Split(':')[2], proxy.Split(':')[3]);
                    }
                    break;
                case 3:
                    break;
            }
        }

        public string Get(string url)
        {
            try
            {
                return request.Get(url).ToString();
            }
            catch (ProxyException val)
            {
                ProxyException val2 = val;
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] GetBytes(string url)
        {
            return request.Get(url).ToBytes();
        }

        public string Post(string url, string data = "", string contentType = "application/x-www-form-urlencoded")
        {
            string result;
            try
            {
                result = ((!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(contentType)) ? ((object)request.Post(url, data, contentType)).ToString() : ((object)request.Post(url)).ToString());
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }

        public void AddCookie(string cookie)
        {
            string[] array = cookie.Split(';');
            string[] array2 = array;
            string[] array3 = array2;
            foreach (string text in array3)
            {
                string[] array4 = text.Split('=');
                if (array4.Count() > 1)
                {
                    try
                    {
                        request.Cookies.Add(array4[0], array4[1]);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public string GetCookie()
        {
            return request.Cookies.ToString();
        }

        public string GetUrl()
        {
            return request.Address.AbsoluteUri;
        }

        public void AddFile(string name, string path, string format)
        {
            request.AddFile(name, path);
        }

        public string Upload(string url, MultipartContent data = null)
        {
            try
            {
                return (request.Post(url, (HttpContent)(object)data)).ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AddParam(string name, string value)
        {
            request.AddParam(name, value);
        }

        public void AddParam(Dictionary<string, string> dictionary)
        {
            foreach (var item in dictionary)
            {
                request.AddParam(item.Key, item.Value);
            }
        }

        public void AddHeader(string name, string value)
        {
            request.AddHeader(name, value);
        }

        public void AddHeader(Dictionary<string, string> dictionary)
        {
            foreach (var item in dictionary)
            {
                request.AddParam(item.Key, item.Value);
            }
        }

        public void Authorization(string Authorization)
        {
            request.Authorization = Authorization;
        }

        public string Address()
        {
            return request.Address.ToString();
        }

        public void TimeOut()
        {
            request.ConnectTimeout = 10000;
        }

        public void userAgent(string useragent)
        {
            request.UserAgent = useragent;
        }

        public void ClearCookie()
        {
            request.Cookies.Clear();
        }

        public void ClearHeader()
        {
            request.ClearAllHeaders();
        }

        public void Dispose()
        {
            request.Dispose();
        }
    }

}
