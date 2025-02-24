using Microsoft.Extensions.Options;
using ParanTwitchIntegration.Domain.Interfaces.Twitch;
using ParanTwitchIntegration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using TwitchLib.Client;
using System.Diagnostics;
using TwitchLib.Communication.Interfaces;
using ParanTwitchIntegration.Domain.Events;
using TwitchLib.Client.Events;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;
using ParanTwitchIntegration.Domain.Interfaces;

namespace ParanTwitchIntegration.Application.Services.Twitch
{
    public class TwitchMultiAccountService : ITwitchMultiAccountService
    {
        private readonly List<TwitchClient> _clients = new();
        private readonly IOptions<List<TwitchAccount>> _accounts;
        private readonly ITwitchTokenService _tokenService;
        private readonly EventBus _eventBus;
        private readonly IBanjoTTSService _banjoTTSService;

        public TwitchMultiAccountService(
            IOptions<List<TwitchAccount>> accounts,
            ITwitchTokenService tokenService,
            IBanjoTTSService banjoTTSService,
            EventBus eventBus)
        {
            _accounts = accounts;
            _tokenService = tokenService;
            _eventBus = eventBus;
            _banjoTTSService = banjoTTSService;


        }

        public void StartAll()
        {
            foreach (var account in _accounts.Value)
            {

                var accessToken = _tokenService.GetValidAccessTokenAsync(account).Result;
                var credentials = new ConnectionCredentials(account.BotUsername, "oauth:" + accessToken);

                var clientPubSub = new TwitchLib.PubSub.TwitchPubSub();
                clientPubSub.OnListenResponse += (sender, e) => {
                    if (e.Successful)
                        Console.WriteLine($"Listening to topic: {e.Topic}");
                    else
                        Console.WriteLine($"Failed to listen! Error: {e.Response.Error}");
                };
                clientPubSub.OnPubSubServiceConnected += (sender, e) =>
                {
                    Console.WriteLine($"PubSub connected");
                    clientPubSub.SendTopics(accessToken);
                };
                clientPubSub.OnChannelPointsRewardRedeemed += (sender, e) =>
                {
                    var rewardEvent = new TwitchRewardRedeemEvent(
                        e.RewardRedeemed.Redemption.Reward.ChannelId,
                        e.RewardRedeemed.Redemption.Reward.Title,
                        e.RewardRedeemed.Redemption.UserInput,
                        e.RewardRedeemed.Redemption.User.DisplayName);
                    ;

                    _eventBus.Publish(rewardEvent);

                    Console.WriteLine($"Récompense réclamée : {e.RewardRedeemed.Redemption.Reward.Title}");
                };

                clientPubSub.ListenToChannelPoints(account.UserId);
                clientPubSub.Connect();

                var client = new TwitchClient();
                  client.Initialize(credentials, account.ChannelName);

                  // Abonnement aux événements Twitch
                  client.OnMessageReceived += (sender, e) =>
                  {
                      var messageEvent = new TwitchMessageReceivedEvent(
                          e.ChatMessage.Message,
                          e.ChatMessage.Username,
                          e.ChatMessage.Channel,
                          client
                          );
                      _eventBus.Publish(messageEvent);
                      Console.WriteLine($"Message reçu dans {e.ChatMessage.Channel} par {e.ChatMessage.Username}: {e.ChatMessage.Message}");
  
                  };

                client.OnConnected += (sender, e) =>
                {
                    Console.WriteLine($"Connecté à {e.BotUsername}");
                };
                  try
                  {
                    Console.WriteLine("On essaie de se connecter à " + account.ChannelName);
                    client.Connect();
                    
                  }
                  catch(Exception e)
                  {
                      Console.WriteLine("Echec de la connexion");
                      Console.WriteLine(e.Message);
                  }
                  Console.WriteLine(client.IsConnected);
                  _clients.Add(client); 
            }
        }

        public void StopAll()
        {
            foreach (var client in _clients)
            {
                client.Disconnect();
            }
        }
    }
}
