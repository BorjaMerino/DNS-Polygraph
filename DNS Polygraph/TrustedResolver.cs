using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DnsPolygraph
{
    internal class TrustedResolver
    {
        private readonly string _url;

        //Generate the URL API required by the service
        public TrustedResolver(string domainReq, string apiUrl, string type)
        {
            var domainArg = string.Join("=", "name", domainReq);
            var random = new Random();
            var length = random.Next(10, 50);
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";
            var randomPayload = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            var padArg = string.Join("=", "random_padding", randomPayload);
            var tmpArgs = type == "A" ? string.Join("&", padArg, "type=1", "edns_client_subnet=0.0.0.0", domainArg) : 
                string.Join("&", padArg, domainArg, "type=PTR");

            _url = string.Join("?", apiUrl, tmpArgs); ;
        }

        //DNS Request over HTTPS
        public string DnsOverHttps()
        {
            var lstRecords = new List<string>();
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Accept = "application/dns-json";
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        var html = reader.ReadToEnd();
                        var objHtml = JsonConvert.DeserializeObject<ResponseHtml>(html);
                        switch (objHtml.Status)
                        {   
                            case 0: //No error
                            {
                                if (objHtml.Answer != null)
                                    foreach (var ans in objHtml.Answer)
                                    {
                                        if (ans.Type == 1) // A (Host Address)
                                            lstRecords.Add(ans.Data);

                                        if (ans.Type == 12) // PTR (Domain Name Pointer)
                                        {
                                            //Right now, consider only the first answer, watch out!
                                            lstRecords.Add(ans.Data);
                                            break;
                                        }
                                    }

                                return string.Join(",", lstRecords.ToArray()).Replace(" ", string.Empty);
                            }
                            case 3:
                            {
                                return "NXDOMAIN";
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        Debug.WriteLine("[-] Exception parsing JSON: " + ex.Message);
                        return "";
                  
                    }
                }
                return "";
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                Debug.WriteLine("[-] Timeout error: " + e.Message);
                return "";
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.TrustFailure)
            {
                Debug.WriteLine("[-] TrustFailure error: " + e.Message);
                MessageBox.Show(@"Watch out! A server certificate could not be validated.", @"Certificate Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[-] Unknown error getting data over HTTPS: " + ex.Message);
                return "";
            }

        }
    }

    public class ResponseHtml
    {
        public int Status { get; set; }
        public bool RD { get; set; }
        public bool RA { get; set; }
        public List<Answers> Answer { get; set; }
    }

    public class Questions
    {
        public string Name { get; set; }
    }

    public class Answers
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public string Data { get; set; }
    }

}
