using Microsoft.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinConsole
{
    public class Startup
    {


        //public IServiceCollection ConfigureServices( IServiceCollection  services)
        //{
        //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //         .AddJwtBearer(options =>
        //         {
        //             options.TokenValidationParameters = new TokenValidationParameters
        //             {
        //                 IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
        //                 {
        //                    // get JsonWebKeySet from AWS
        //                    var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
        //                    // serialize the result
        //                    var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
        //                    // cast the result to be the type expected by IssuerSigningKeyResolver
        //                    return (IEnumerable<SecurityKey>)keys;
        //                 },

        //                 ValidIssuer = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p",
        //                 ValidateIssuerSigningKey = true,
        //                 ValidateIssuer = true,
        //                 ValidateLifetime = true,
        //                 ValidAudience = "3ib0kpel42kkm4nhfvvriq1vtf",
        //                 ValidateAudience = true
        //             };
        //         });
        //    return services;
        //}

            /*
        public void ConfigureOAuth(IAppBuilder app)
        {
            var issuer = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p";
            var audience = "3ib0kpel42kkm4nhfvvriq1vtf";
            var secret = TextEncodings.Base64Url.Decode("ABCD");
            JwtSettings settings = new JwtSettings();
            settings.Issuer = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p";
            settings.Key = "pzio1YTX-ll4iM5Cz7EyfEBFLI2cKuR6cwczBDKftykHF6K9PzDWNqR11nAyEmpIniwv5zzaE5D11gqu38y11Ywn2B9Gs28NWZbGYJoXJ-2D2tWGyVtdIxNssdxjluXKP1qg94OSk3jKESmSgmjSwnHTUnCifcthMnc8fEVhwOVQlumbFYaY472yXX1vOcqlUyHL68cRJCdLbTIyZ8lDreLJPUj2itykjqQT4zaoaRtD62T8RTkqAY-5M9m7Bcvl9AeG8KB9AUqulNvQtFwMCPuU6Jj9fPGFAFp_EeQifLyjwCeQLyI56JsmbIHI1tPD705CeZQrHrbS2aXRWVQ3Nw";
            settings.Expo = "AQAB";
            // Api controllers with an [Authorize] attribute will be validated with JWT
            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    TokenValidationParameters = settings.TokenValidationParameters,
                    Provider = new JwtProvider(),
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience },
                    IssuerSecurityKeyProviders = new IIssuerSecurityKeyProvider[]
                    {
                        new SymmetricKeyIssuerSecurityKeyProvider(issuer,secret)
                    }

                });

        }*/
        public void Configuration(IAppBuilder app)
        {
            //var serviceProvider = this.ConfigureServices(coll).BuildServiceProvider();

            // Configure Web API for self-host. 
            var config = new HttpConfiguration();
            //var serviceProvider = IocStartup.BuildServiceProvider();
            //IocStartup.
            //config.DependencyResolver = new DefaultDependencyResolver(serviceProvider);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //ConfigureOAuth(app);
            //app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            //{
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString("/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            //    Provider = new CustomAuthorizationServerProvider()
            //});
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            //config.Services.Add(serviceProvider);

            //services.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = tokenValidationParameters;
            //});

            //var jwtO = new JwtBearerOptions();
            //jwtO.Audience = "3ib0kpel42kkm4nhfvvriq1vtf";
            //jwtO.Authority = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p";

            //app.UseJwtBearerAuthentication(jwtO);
            //app.UseJwtBearerAuthentication(
            //new JwtBearerAuthenticationOptions
            //{
            //    AllowedAudiences = "3ib0kpel42kkm4nhfvvriq1vtf",

            //    ValidIssuer = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p",
            //    SaveToken = true

            //});


            // app.UseOAuthBearerAuthentication()

            app.UseWebApi(config);

        }
    }
}
