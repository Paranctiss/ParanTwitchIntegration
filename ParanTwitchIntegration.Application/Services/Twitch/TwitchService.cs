using Microsoft.Extensions.Options;
using ParanTwitchIntegration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using TwitchLib.Client;
using ParanTwitchIntegration.Domain.Interfaces.Twitch;
using System.Diagnostics;
using ParanTwitchIntegration.Domain.Events;

namespace ParanTwitchIntegration.Application.Services.Twitch
{
    public class TwitchService : ITwitchService
    {
        private readonly TwitchClient _client;
        private readonly EventBus _eventBus;
        private readonly ITwitchChatService _twitchChatService;

        public TwitchService(
            TwitchClient client,
            EventBus eventBus, 
            ITwitchChatService twitchChatService)
        {
            _client = client;
            _eventBus = eventBus;
            _twitchChatService = twitchChatService;
        }

        public void Start()
        {
            // Abonnement aux événements Twitch
            _client.OnMessageReceived += (sender, e) =>
            {
                var messageEvent = new TwitchMessageReceivedEvent(
                    e.ChatMessage.Message,
                    e.ChatMessage.Username,
                    e.ChatMessage.Channel,
                    _client
                );
                _eventBus.Publish(messageEvent);
            };
            _client.OnNewSubscriber += (sender, e) => _eventBus.Publish(e);
            _client.OnConnected += (sender, e) => Debug.WriteLine($"Connecté à {e.BotUsername}");

            _client.Connect();
        }

        public void Stop()
        {
            _client.Disconnect();
        }
    }
}
