using Bhbk.Lib.Cryptography.Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Bhbk.Lib.Hosting.Options
{
    public static class KestrelOptions
    {
        public static void ConfigureEndpoints(this KestrelServerOptions options)
        {
            var config = options.ApplicationServices.GetRequiredService<IConfiguration>();

            var endpoints = config.GetSection("KestrelServer:Endpoints")
                .GetChildren()
                .ToDictionary(section => section.Key, section =>
                {
                    var endpoint = new EndPointConfig();
                    section.Bind(endpoint);

                    return endpoint;
                });

            var certificate = X509.CreateSelfSigned(RsaKeyLength.Bits2048, SignatureType.SHA256WithRSA);

            foreach (var endpoint in endpoints)
            {
                if (endpoint.Value.Enable)
                {
                    var list = new List<IPAddress>();

                    if (endpoint.Value.Host == "localhost")
                    {
                        list.Add(IPAddress.Loopback);
                        list.Add(IPAddress.IPv6Loopback);
                    }
                    else if (IPAddress.TryParse(endpoint.Value.Host, out var address))
                        list.Add(address);
                    else
                        list.Add(IPAddress.IPv6Any);

                    foreach (var ip in list)
                    {
                        options.Listen(ip, endpoint.Value.Port,
                            listenOptions =>
                            {
                                if (endpoint.Value.Scheme == "https")
                                {
                                    listenOptions.UseHttps(certificate);
                                }
                            });
                    }
                }
            }
        }
    }

    public class EndPointConfig
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Enable { get; set; }
    }
}
