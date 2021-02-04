using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/");
            HttpResponseMessage response = await client.GetAsync(
              "x509/securetoken@system.gserviceaccount.com");
            if (!response.IsSuccessStatusCode) { return; }
            var st = await response.Content.ReadAsStreamAsync();
            var x509Data = await JsonSerializer.DeserializeAsync<Dictionary<string,string>>(st);
            SecurityKey[] keys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray();
        }

        static SecurityKey CreateSecurityKeyFromPublicKey(string data)
        {
            return new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(data)));
        }
    }
}
