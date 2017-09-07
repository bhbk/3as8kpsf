using Elmah.Contrib.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

[assembly: OwinStartup(typeof(Bhbk.WebApi.Sample.WebApi.Startup))]
namespace Bhbk.WebApi.Sample.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ConfigureWebApi(httpConfig);
            ConfigureOAuthTokenConsumption(app);

            app.UseWebApi(httpConfig);
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            var issuer = "BHBK";

            try
            {
                List<string> namespaces = new List<string>{
                    "Bhbk.WebApi.Sample"
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.AppSettings["IdentityBaseUrl"]);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.PostAsJsonAsync("api/core/identity/venue/v1/namespaces", namespaces).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var apps = response.Content.ReadAsAsync<Dictionary<Guid, string>>().Result;

                        app.UseJwtBearerAuthentication(
                            new JwtBearerAuthenticationOptions
                            {
                                AuthenticationMode = AuthenticationMode.Active,
                                AllowedAudiences = apps.Select(a => a.Key.ToString().ToUpper()),
                                IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                            {
                                new SymmetricKeyIssuerSecurityTokenProvider(issuer, apps.Select(a => TextEncodings.Base64Url.Decode(a.Value)))
                            }
                            });
                    }
                    else
                        throw new Exception("Token Service unreachable");
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}