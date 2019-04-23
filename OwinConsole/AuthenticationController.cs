using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinConsole
{
    //Remove: AWSSDK.CognitoIdentityProvider from nuget if you do not want this class
    public class AuthenticationController : ApiController
    {

        private const string _clientId = "3ib0kpel42kkm4nhfvvriq1vtf";
        private const string _poolID = "us-east-2_1kXC0Pa2p";
        private readonly RegionEndpoint _region = RegionEndpoint.USEast2;
        [HttpPost]
        [Route("signin")]
        public async Task<IHttpActionResult> SignInAsync()
        {
            var cognito = new AmazonCognitoIdentityProviderClient(_region);

            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = _poolID,
                ClientId = _clientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add("USERNAME", "testuser2");
            request.AuthParameters.Add("PASSWORD", "*Ibm12345");

            var response = await cognito.AdminInitiateAuthAsync(request);

            return Ok(response.AuthenticationResult.IdToken);
        }
    }
}
