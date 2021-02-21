using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azwebapp.Services
{
    public class KeyVaultService : IKeyVaultService
    {
        public async Task<string>  GetValue()
        {

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            KeyVaultClient client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            var result = await client.GetSecretAsync("https://azlearnkvnew.vault.azure.net/secrets/ConnectionString/54431c3817114c0c811269cc6bb2638f");
            return result.Value;
        }

        //private async static Task<string> GetAccessToken(string a, string b, string c)
        //{
        //    const string ClientId = "9632bd2e-f950-48e3-9d55-3dd08833a8a8";
        //    const string TennantId = "549a973a-fda2-4190-9ee8-67445857b006";
        //    const string ClientSecret = "uKm.N_sY7rAjRt81L.pS-JBxQplyiz-_D0";

        //    var confidentialApp = ConfidentialClientApplicationBuilder.Create(ClientId).
        //        WithAuthority(AzureCloudInstance.AzurePublic, TennantId).WithClientSecret(ClientSecret).Build();
        //    List<string> scopes = new List<string>() { "https://vault.azure.net/.default" };
        //    var result = await confidentialApp.AcquireTokenForClient(scopes).ExecuteAsync();
        //    Console.WriteLine(result.AccessToken);
        //    return result.AccessToken;
        //}
    }
}
