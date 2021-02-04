using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace firebase_demo
{
    public class CustomTokenValidator : JwtSecurityTokenHandler, ISecurityTokenValidator
    {
        public CustomTokenValidator()
        {

        }

        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");
            HttpResponseMessage response = client.GetAsync(
              "x509/securetoken@system.gserviceaccount.com").Result;
            //if (!response.IsSuccessStatusCode) { return; }
            var x509Data = JsonSerializer.DeserializeAsync<Dictionary<string, string>>(response.Content.ReadAsStreamAsync().Result).Result;
            SecurityKey[] keys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray();

            validationParameters.IssuerSigningKeys = keys;

            ClaimsPrincipal principal;
            principal = base.ValidateToken(securityToken, validationParameters, out validatedToken);
            ClaimsIdentity identity = new ClaimsIdentity(principal.Identity);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            return claimsPrincipal;
        }

        static SecurityKey CreateSecurityKeyFromPublicKey(string data)
        {
            return new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(data)));
        }
    }
}