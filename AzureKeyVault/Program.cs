using Microsoft.Azure.KeyVault;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureKeyVault
{
    class Program
    {
        static async Task Main(string[] args)
        {
            KeyVaultClient client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));
            //var result = await client.GetSecretAsync("https://azlearnkvnew.vault.azure.net/secrets/ConnectionString/54431c3817114c0c811269cc6bb2638f");
            var token = await GetAccessToken("", "", "");
            Console.WriteLine(token);

            GraphServiceClient grpahClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
            {
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            }));
           var result = await grpahClient.Users.Request().GetAsync();
            Console.Read();
        }

        private async static Task<string> GetAccessToken(string a, string b, string c)
        {
            const string ClientId = "9632bd2e-f950-48e3-9d55-3dd08833a8a8";
            const string TennantId = "549a973a-fda2-4190-9ee8-67445857b006";
            const string ClientSecret = "uKm.N_sY7rAjRt81L.pS-JBxQplyiz-_D0";

            var confidentialApp = ConfidentialClientApplicationBuilder.Create(ClientId).
                WithAuthority(AzureCloudInstance.AzurePublic, TennantId).WithClientSecret(ClientSecret).Build();
            List<string> scopes = new List<string>() { "https://graph.microsoft.com/.default" };
            var result =  await confidentialApp.AcquireTokenForClient(scopes).ExecuteAsync();
            Console.WriteLine(result.AccessToken);
            return result.AccessToken;
        }
    }
}
