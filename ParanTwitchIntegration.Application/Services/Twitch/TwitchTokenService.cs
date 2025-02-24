using Microsoft.Extensions.Options;
using ParanTwitchIntegration.Domain.Interfaces.Twitch;
using ParanTwitchIntegration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Application.Services.Twitch
{
    public class TwitchTokenService : ITwitchTokenService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private TwitchToken _token;

        public TwitchTokenService(IOptions<TwitchSettings> twitchSettings, IOptions<TwitchToken> initialToken)
        {
            _clientId = twitchSettings.Value.ClientId;
            _clientSecret = twitchSettings.Value.ClientSecret;
            _token = initialToken.Value;
        }

        public async Task<string> GetValidAccessTokenAsync(TwitchAccount account)
        {
            return await RefreshAccessTokenAsync(account);
        }

        private async Task<string> RefreshAccessTokenAsync(TwitchAccount account)
        {
            using var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
              {
                  { "client_id", account.ClientId },
                  { "client_secret", account.ClientSecret},
                  { "refresh_token", account.RefreshToken },
                  { "grant_type", "refresh_token" }
              });

            var response = await httpClient.PostAsync("https://id.twitch.tv/oauth2/token", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JsonSerializer.Deserialize<JsonElement>(responseBody);

            return json.GetProperty("access_token").GetString();
        }

        /** OLD METHODS FOR ONLY ONE ACCOUNT **/
        /*  public async Task<string> GetValidAccessTokenAsync(TwitchAccount account)
          {
              await RefreshAccessTokenAsync();
              if (_token.Expiry <= DateTime.UtcNow.AddMinutes(-1))
              {
                  await RefreshAccessTokenAsync();
              }
              return _token.AccessToken;
          }

          private async Task RefreshAccessTokenAsync()
          {
              using var httpClient = new HttpClient();
              var content = new FormUrlEncodedContent(new Dictionary<string, string>
              {
                  { "client_id", _clientId },
                  { "client_secret", _clientSecret },
                  { "refresh_token", _token.RefreshToken },
                  { "grant_type", "refresh_token" }
              });

              var response = await httpClient.PostAsync("https://id.twitch.tv/oauth2/token", content);
              response.EnsureSuccessStatusCode();

              var responseBody = await response.Content.ReadAsStringAsync();
              var json = JsonSerializer.Deserialize<JsonElement>(responseBody);

              _token.AccessToken = json.GetProperty("access_token").GetString();
              _token.Expiry = DateTime.UtcNow.AddSeconds(json.GetProperty("expires_in").GetInt32());
              _token.RefreshToken = json.GetProperty("refresh_token").GetString();
          } */
    }
}
