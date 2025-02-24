using Microsoft.Extensions.Options;
using ParanTwitchIntegration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Client;
using ParanTwitchIntegration.Domain.Interfaces.Twitch;
using ParanTwitchIntegration.Domain.Interfaces;
using System.Diagnostics;
using ParanTwitchIntegration.Application.Factories;
using ParanTwitchIntegration.Domain.Events;
using TwitchLib.Client.Interfaces;

namespace ParanTwitchIntegration.Application.Services.Twitch
{
    public class TwitchChatService : ITwitchChatService
    {
        private readonly IBanjoTTSService _banjoTTSHelper;
        private readonly List<TwitchAccount> _twitchAccounts;
        private readonly TwitchClientFactory _twitchClientFactory;

        public TwitchChatService(
            IBanjoTTSService banjoTTSHelper,
            TwitchClientFactory twitchClientFactory,
            IOptions<List<TwitchAccount>> twitchAccounts,
            EventBus eventBus)
        {
            _banjoTTSHelper = banjoTTSHelper;
            _twitchClientFactory = twitchClientFactory;
            _twitchAccounts = twitchAccounts.Value;

            // Utilise le nouveau nom pour la méthode
            //eventBus.Subscribe<OnMessageReceivedArgs>(HandleMessageReceived);
            eventBus.Subscribe<TwitchMessageReceivedEvent>(HandleMessageReceived);
        }


        // Méthode renommée pour éviter la collision de noms
        private async void HandleMessageReceived(TwitchMessageReceivedEvent e)
        {

            var account = _twitchAccounts.FirstOrDefault(a => a.ChannelName == e.ChannelName);
            if (account == null)
            {
                Debug.WriteLine($"⚠️ Aucun compte trouvé pour {e.ChannelName}");
                return;
            }

            var client = _twitchClientFactory.CreateClient(account);
            if (client == null)
            {
                Debug.WriteLine($"❌ Impossible de récupérer un client pour {e.ChannelName}");
                return;
            }

            if (e.Message.StartsWith("!ai"))
            {
                await _banjoTTSHelper.BanjoTTSFromChatAsync(
                    e.Message,
                    e.Username,
                    e.ChannelName,
                    client
                );
            }

            if (e.Message.StartsWith("!discord") || e.Message.StartsWith("!Discord"))
            {
                client.SendMessage(e.ChannelName, $"Rejoint le Discord pour rester informé de l'avancé et des futurs tests du projet, ainsi que la programmation de la DozCup ! https://discord.gg/YQtSH5y7QA ");
            }

            // Déclencher l'événement de l'interface
            //OnMessageReceived?.Invoke(this, e);
        }
    }
}
