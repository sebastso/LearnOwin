using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OwinConsole
{
    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(
            OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult(0);
        }

        public override Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers
                .Add(new KeyValuePair<string, string[]>("Access-Control-Allow-Origin", new[] { "*" }));

           // var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
           // var user = userManager.Find(context.UserName, context.Password);
            /*if (user == null)
            {
                context.SetError("invalid_grant", "Username and password do not match.");
                return Task.FromResult(0);
            }*/

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, "sony"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "sony100"));

            context.Validated(identity);
            return Task.FromResult(0);

        }
    }
}