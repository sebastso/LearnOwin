using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinConsole
{
    public static class IocStartup
    {
        public static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();


            //services.AddMvc();

            /* services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                          {
                              // get JsonWebKeySet from AWS
                              var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                              // serialize the result
                              var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                              // cast the result to be the type expected by IssuerSigningKeyResolver
                              return (IEnumerable<SecurityKey>)keys;
                          },

                          ValidIssuer = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p",
                          ValidateIssuerSigningKey = true,
                          ValidateIssuer = true,
                          ValidateLifetime = true,
                          ValidAudience = "3ib0kpel42kkm4nhfvvriq1vtf",
                          ValidateAudience = true
                      };
                  });*/
            // Register all dependent services
            // 
            //IocSomeAssembly.Register(services);    
            // 
            // services.AddTransient<ISomething, Something>()

            // For WebApi controllers, you may want to have a bit of reflection
            var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
              .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
              .Where(t => typeof(ApiController).IsAssignableFrom(t));
            // || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Audience = "3ib0kpel42kkm4nhfvvriq1vtf";
                options.Authority = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p";
            });
            //services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(o =>
            //{
            //    o.Authority = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p";
            //    o.Audience = "3ib0kpel42kkm4nhfvvriq1vtf";
            //    o.RequireHttpsMetadata = false;
            //});

            //services.AddMvc();

            // It is only that you need to get service provider in the end
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
