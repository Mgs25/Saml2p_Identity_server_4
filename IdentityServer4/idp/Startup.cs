using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using IdentityServer4.Quickstart.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rsk.Saml.Configuration;

namespace idp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddTestUsers(TestUsers.Users)
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddSigningCredential(new X509Certificate2("idsrv3test.pfx", "idsrv3test"));

            // Configure SAML Identity Provider and authorized Service Providers
            builder.AddSamlPlugin(options =>
                {
                    options.Licensee = "DEMO";
                    options.LicenseKey = "eyJTb2xkRm9yIjowLjAsIktleVByZXNldCI6NiwiU2F2ZUtleSI6ZmFsc2UsIkxlZ2FjeUtleSI6ZmFsc2UsIlJlbmV3YWxTZW50VGltZSI6IjAwMDEtMDEtMDFUMDA6MDA6MDAiLCJhdXRoIjoiREVNTyIsImV4cCI6IjIwMjItMTAtMDZUMDE6MjE6MjYuNjQzNTUxMiswMDowMCIsImlhdCI6IjIwMjItMDktMDZUMDE6MjE6MjYiLCJvcmciOiJERU1PIiwiYXVkIjoyfQ==.pWTdA5V5dkexQYRhs89Vjfj7UK2sDMc/STCPfx4hV1H5jLXyDOJtlW9PoxlhpdPjeIno/SVUYXmYePYTlQD48oaXGdl/+mLgX1UnP8tpPHO1Cpk+fLJz4b/6YWmj1efIAhok5OvETKdODvqHwZkSS+c0OVOBJqWJZ7hWImZ31evGOIb7bgQkWm8uRRSHQHt8RE2hCQ5o2zRnvI7Eu8vbsPHI4nRg3sykwsZe3XdxUW4L419c52lsLl/8hm73bTtg1zlfixWd/zWVhCTfVuDAzzjcOFQSqVc7tMOvRhRob4+/6+EjOtTi+K+rZHoFp6EjGPhtP3KiBJdUD0g730+8l1v23lUVuvmilWQ5qCqD6kNDvtE+WDB+tdevKEHsry0kLkiTlVlOV36l8Bh+j6UPhrLDxhRt6r4csDz/xTLd8bwyEaWGmaD3zKUJOGYEqWd8ffGPjZOTiwryhhesB8Z2IzmmEBkykMlFhbMr+rximkbkEKClalVKSNjNapopLX3yvMuYFuQTWDWAg4eEpGKHaohURPEf3UALC0mtHnWOg4JW2MhlRav8rg2kLQXRy2ys+7Fqj+ovrmE+f9+8wWP5q3lXPODjwAb7iFFzASUrsH0cH1owYis2Ff7Plam9ZDNHGYLryOP6gLOAh3h2qDcINl90VJVV/CEwF8+2inp+OOc=";

                    options.WantAuthenticationRequestsSigned = false;
                })
                .AddInMemoryServiceProviders(Config.GetServiceProviders());

            // use different cookie name that sp...
            builder.Services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme,
                cookie => { cookie.Cookie.Name = "idsrv.idp"; });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer()
               .UseIdentityServerSamlPlugin(); // enables SAML endpoints (e.g. ACS and SLO)

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}