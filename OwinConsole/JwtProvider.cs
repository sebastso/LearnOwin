using System;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.OAuth;

namespace OwinConsole
{
    internal class JwtProvider : IOAuthBearerAuthenticationProvider
    {
        public Task ApplyChallenge(OAuthChallengeContext context)
        {
            //context.Validated();
            return Task.FromResult(0);
        }
        public bool Verify([FromBody] string accessToken)
        {
            string[] parts = accessToken.Split('.');

            //From the Cognito JWK set
            //{"alg":"RS256","e":"myE","kid":"myKid","kty":"RSA","n":"myN","use":"sig"}]}
            var n = Base64UrlDecode("pzio1YTX-ll4iM5Cz7EyfEBFLI2cKuR6cwczBDKftykHF6K9PzDWNqR11nAyEmpIniwv5zzaE5D11gqu38y11Ywn2B9Gs28NWZbGYJoXJ-2D2tWGyVtdIxNssdxjluXKP1qg94OSk3jKESmSgmjSwnHTUnCifcthMnc8fEVhwOVQlumbFYaY472yXX1vOcqlUyHL68cRJCdLbTIyZ8lDreLJPUj2itykjqQT4zaoaRtD62T8RTkqAY-5M9m7Bcvl9AeG8KB9AUqulNvQtFwMCPuU6Jj9fPGFAFp_EeQifLyjwCeQLyI56JsmbIHI1tPD705CeZQrHrbS2aXRWVQ3Nw");
            var e = Base64UrlDecode("AQAB");

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();

            provider.ImportParameters(new RSAParameters
            {
                Exponent = new BigInteger(e).ToByteArray(),
                Modulus = new BigInteger(n).ToByteArray()
            });

            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(parts[0] + "." + parts[1]));

            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(provider);
            rsaDeformatter.SetHashAlgorithm("SHA256");

            if (!rsaDeformatter.VerifySignature(hash, Base64UrlDecode(parts[2])))
                throw new ApplicationException(string.Format("Invalid signature"));

            return true;
        }

        // from JWT spec
        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 1: output += "==="; break; // Three pad chars
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
        private static void ValidateJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidAudience = "3ib0kpel42kkm4nhfvvriq1vtf",
                ValidIssuer = "https://cognito-idp.us-east-2.amazonaws.com/us-east-2_1kXC0Pa2p"
            };

            try
            {
                SecurityToken validatedToken;
                var principal = handler.ValidateToken(jwt, validationParameters, out validatedToken);
            }
            catch (Exception e)
            {

                Console.WriteLine("{0}\n {1}", e.Message, e.StackTrace);
            }

            Console.WriteLine();
        }
        public Task RequestToken(OAuthRequestTokenContext context)
        {
            Verify(context.Token);
            //ValidateJwt(context.Token);
            //context.Validated();
            return Task.FromResult(0);
        }

        public Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            context.Validated();
            return Task.FromResult(0);
        }
    }
}