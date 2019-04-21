using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Numerics;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace OwinConsole
{
    public class MyCustomAuthAttribute : AuthorizeAttribute
    {
        public bool VerifyCognitoJwt(string accessToken)
        {
            try
            {
                string[] parts = accessToken.Split('.');

                string header = parts[0];
                string payload = parts[1];

                string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
                JObject headerData = JObject.Parse(headerJson);

                string payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
                JObject payloadData = JObject.Parse(payloadJson);

                var kid = headerData["kid"];
                var iss = payloadData["iss"];

                var issUrl = iss + "/.well-known/jwks.json";
                var keysJson = string.Empty;

                using (WebClient wc = new WebClient())
                {
                    //We can optimize to download these only once when app starts and use  it just for verfification
                    //Not required to download each time
                    keysJson = wc.DownloadString(issUrl);
                }

                var keyData = GetKeyData(keysJson, kid.ToString());

                if (keyData == null)
                    throw new ApplicationException(string.Format("Invalid signature"));

                var modulus = Base64UrlDecode(keyData.Modulus);
                var exponent = Base64UrlDecode(keyData.Exponent);

                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();

                var rsaParameters = new RSAParameters();
                rsaParameters.Modulus = new BigInteger(modulus).ToByteArray();
                rsaParameters.Exponent = new BigInteger(exponent).ToByteArray();

                provider.ImportParameters(rsaParameters);

                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(parts[0] + "." + parts[1]));

                RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(provider);
                rsaDeformatter.SetHashAlgorithm(sha256.GetType().FullName);

                if (!rsaDeformatter.VerifySignature(hash, Base64UrlDecode(parts[2])))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public class KeyData
        {
            public string Modulus { get; set; }
            public string Exponent { get; set; }
        }

        private static KeyData GetKeyData(string keys, string kid)
        {
            var keyData = new KeyData();

            dynamic obj = JObject.Parse(keys);
            var results = obj.keys;
            bool found = false;

            foreach (var key in results)
            {
                if (found)
                    break;

                if (key.kid == kid)
                {
                    keyData.Modulus = key.n;
                    keyData.Exponent = key.e;
                    found = true;
                }
            }

            return keyData;
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
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new Exception("actionContextis null");
            }
            var headerToken = actionContext.Request.Headers;
            var token = headerToken.Authorization;
            if (token != null)
            {
                if (this.VerifyCognitoJwt(token.Parameter))
                    return true;
            }
            else
            {
                return false;
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                actionContext.Response =
               actionContext.ControllerContext.Request.CreateErrorResponse(
                                     HttpStatusCode.Unauthorized,
                                     "My Un-Authorized Message");
            }
        }
    }
}
