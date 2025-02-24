using ParanTwitchIntegration.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.PubSub;

using Microsoft.Extensions.Options;
using TwitchLib.Client.Events;
using System.Diagnostics;
using ParanTwitchIntegration.Application.Helpers;
using OBSWebsocketDotNet.Types;
using System.Reactive.Joins;
using System.Text.RegularExpressions;
using TwitchLib.Api.Services;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api;
using ParanTwitchIntegration.Domain.Models;
using ParanTwitchIntegration.Application.Services.Twitch;

namespace ParanTwitchIntegration.Application
{
   /* public class AuthTwitch
    {
        private readonly TwitchClient _client;
        private readonly FollowerService _followService;
        private readonly OBSSockets _obsSockets;
        private readonly BanjoTTSService _banjoTTSHelper;

        private readonly MinecraftCommandService _minecraftService;

        public AuthTwitch(
            TwitchTokenService tokenService, 
            IOptions<TwitchSettings> options, 
            MinecraftCommandService minecraftService, 
            OBSSockets obsSockets,
            BanjoTTSService banjoTTSHelper
            )
        {
            var settings = options.Value;

            _minecraftService = minecraftService;

            // Obtient un token valide
            var accessToken = tokenService.GetValidAccessTokenAsync().Result;
            var credentials = new ConnectionCredentials(settings.BotUsername, accessToken);

            _client = new TwitchClient();
            _client.Initialize(credentials, settings.ChannelName);

            // Abonnement aux événements de chat
            _client.OnMessageReceived += OnMessageReceived;
            _client.OnNewSubscriber += OnNewSub;
            _client.OnConnected += (sender, e) =>
            {
                Debug.WriteLine($"Connecté à {e.BotUsername} dans le canal {e.AutoJoinChannel}");
            };
            // Connecte le client Twitch
            try
            {
                _client.Connect();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            var twitchApi = new TwitchAPI();
            twitchApi.Settings.ClientId = "yw59r08rmajxr4z27udkrncgidhjm4";
            twitchApi.Settings.AccessToken = accessToken;
            twitchApi.Settings.Secret = "fswiy4bq8ahgpsn7zuxda3ckatc2za";

            _followService = new FollowerService(twitchApi, 5);
            _followService.OnNewFollowersDetected += OnNewFollowers;

            _obsSockets = obsSockets;
            _banjoTTSHelper = banjoTTSHelper;
        }

        private void OnNewFollowers(object? sender, TwitchLib.Api.Services.Events.FollowerService.OnNewFollowersDetectedArgs e)
        {
            Console.WriteLine(e.NewFollowers[0].FromUserName);
           // _banjoTTSHelper.ThanksFollow(e.NewFollowers[0].FromUserName);
        }

        // Gestionnaire d'événements pour les messages du tchat
        private async void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            // Affiche le message du tchat dans la console
            Debug.WriteLine($"{e.ChatMessage.Username}: {e.ChatMessage.Message}");

            if (e.ChatMessage.Message.StartsWith("!ai"))
            {
                BanjoTTSFromChat(e);
            }

            // Si le message contient la commande !give
            if (e.ChatMessage.Message.StartsWith("!give"))
            {
                    // Construire la commande Minecraft
                    var command = $"/give Paranctiss minecraft:diamond_sword";

                    // Appeler le service pour exécuter la commande sur le serveur Minecraft
                    _minecraftService.ExecuteCommand(command);
            }
        }


        private async void BanjoTTSFromChat(OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message;
            var username = e.ChatMessage.Username;
            var channelName = e.ChatMessage.Channel;
            
            await _banjoTTSHelper.BanjoTTSFromChatAsync(message, username, channelName);
        }

        private async void OnNewSub(object sender, OnNewSubscriberArgs e)
        {
            Console.WriteLine(e.Subscriber.DisplayName);
        }
    } */
}

