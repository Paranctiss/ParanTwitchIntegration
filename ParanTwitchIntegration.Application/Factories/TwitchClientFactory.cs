using ParanTwitchIntegration.Domain.Interfaces.Twitch;
using ParanTwitchIntegration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using TwitchLib.Client;

namespace ParanTwitchIntegration.Application.Factories
{
    public class TwitchClientFactory
    {
        private readonly ITwitchTokenService _tokenService;

        public TwitchClientFactory(ITwitchTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public TwitchClient CreateClient(TwitchAccount account)
        {
            var accessToken = _tokenService.GetValidAccessTokenAsync(account).Result;
            var credentials = new ConnectionCredentials(account.BotUsername, accessToken);

            var client = new TwitchClient();
            client.Initialize(credentials, account.ChannelName);
            client.Connect();
            return client;
        }
    }
}
