using DemoApplication.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace DemoApplication.Services
{
    public class OktaTokenService : ITokenService
    {
        private readonly IOptions<OktaSetting> _oktaSettings;
        private OktaToken token = new OktaToken();
        public OktaTokenService(IOptions<OktaSetting> oktaSettings)
        {
            _oktaSettings = oktaSettings;
        }

        public async Task<OktaToken> GetToken(string username, string password)
        {
            return await this.GetNewAccessToken(username, password);            
        }

        private async Task<OktaToken> GetNewAccessToken(string username, string password)
        {
            var token = new OktaToken();
            var client = new HttpClient();
            var client_id = _oktaSettings.Value.ClientId;
            var client_secret = _oktaSettings.Value.ClientSecret;
            var clientCreds = System.Text.Encoding.UTF8.GetBytes($"{client_id}:{client_secret}");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(clientCreds));

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var postMessage = new Dictionary<string, string>();
            postMessage.Add("grant_type", "password");
            postMessage.Add("username", username);
            postMessage.Add("password", password);
            postMessage.Add("scope", "openid");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_oktaSettings.Value.Domain}/oauth2/default/v1/token")
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var jsonSerializerSetting = new JsonSerializerSettings();

                var json = await response.Content.ReadAsStringAsync();

                token = JsonConvert.DeserializeObject<OktaToken>(json, jsonSerializerSetting);
                token.ExpiresAt = DateTime.UtcNow.AddSeconds(this.token.ExpiresIn);
            }
            else
            {
                var error = await response.Content?.ReadAsStringAsync();

                throw new ApplicationException(error);
            }

            return token;
        }        
    }
}
