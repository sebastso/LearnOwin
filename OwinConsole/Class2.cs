﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OwinConsole
{
    public class JwtSettings
{
    public string Issuer { get; set; }
    public string Key { get; set; }
    public string Expo { get; set; }
 
    public RsaSecurityKey SigningKey
    {
        get
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(
                new RSAParameters()
                {
                    Modulus = Base64UrlEncoder.DecodeBytes(Key),
                    Exponent = Base64UrlEncoder.DecodeBytes(Expo)
                });
 
            return new RsaSecurityKey(rsa);
        }
    }
 
    public TokenValidationParameters TokenValidationParameters
    {
        get
        {
            // Basic settings - signing key to validate with, audience and issuer.
            return new TokenValidationParameters
            {
                // Basic settings - signing key to validate with, IssuerSigningKey and issuer.
                IssuerSigningKey = SigningKey,
                ValidIssuer = Issuer,
                     
                // when receiving a token, check that the signing key
                ValidateIssuerSigningKey = true,
 
                // When receiving a token, check that we've signed it.
                ValidateIssuer = true,
 
                // When receiving a token, check that it is still valid.
                ValidateLifetime = true,
                    
                // Do not validate Audience on the "access" token since Cognito does not supply it but it is      on the "id"
                ValidateAudience = false,
 
                // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
                // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
                // machines which should have synchronised time, this can be set to zero. Where external tokens are
                // used, some leeway here could be useful.
                ClockSkew = TimeSpan.FromMinutes(0)
            };
        }
    }
}
}
