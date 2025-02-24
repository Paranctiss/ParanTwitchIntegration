using Microsoft.Extensions.Options;
using ParanTwitchIntegration.Application.Factories;
using ParanTwitchIntegration.Application.Helpers;
using ParanTwitchIntegration.Domain.Events;
using ParanTwitchIntegration.Domain.Interfaces;
using ParanTwitchIntegration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Communication.Interfaces;

namespace ParanTwitchIntegration.Application.Services
{
    public class BanjoTTSService : IBanjoTTSService
    {
        private readonly OBSSockets _obsSockets;
        private readonly IOpenAIService _openAIService;
        private readonly TwitchClientFactory _twitchClientFactory;
        private readonly List<TwitchAccount> _twitchAccounts;
        private readonly string[] characters = ["Banjo", "Gruntilda", "Kazooie"];

        public BanjoTTSService(OBSSockets obsSockets, 
            IOpenAIService openAIService,
            TwitchClientFactory twitchClientFactory,
            IOptions<List<TwitchAccount>> twitchAccounts,
            EventBus eventBus)
        {
            _obsSockets = obsSockets;
            _openAIService = openAIService;
            _twitchClientFactory = twitchClientFactory;
            _twitchAccounts = twitchAccounts.Value;
            eventBus.Subscribe<TwitchMessageReceivedEvent>(OnTwitchMessageReceived);
        }

        private async void OnTwitchMessageReceived(TwitchMessageReceivedEvent e)
        {
            if (e.Message.StartsWith("!ai")) {
                //var account = _twitchAccounts.FirstOrDefault(a => a.ChannelName.ToLower() == e.ChannelName);

                //var client = _twitchClientFactory.CreateClient(account);

                await BanjoTTSFromChatAsync(e.Message, e.Username, e.ChannelName, e.Client);
            }

            if (e.Message.StartsWith("!discord") || e.Message.StartsWith("!Discord"))
            {
                e.Client.SendMessage(e.ChannelName, $"Rejoint le Discord pour rester informé de l'avancé et des futurs tests du projet, ainsi que la programmation de la DozCup ! https://discord.gg/YQtSH5y7QA ");
            }

        } 

        public async Task BanjoTTSFromChatAsync(string message, string username, string channelName, TwitchClient _twitchClient)
        {

            if (message.Length <= 4)
            {
                _twitchClient.SendMessage(channelName, $"@{username}, Discute avec Banjo, Kazooie ou Gruntilda avec la commande '!ai (nomPersonnage) message' :) ");
                return;
            }
            else
            {
                var prompt = message.Substring(4).Trim();
               

                // Génère la réponse avec OpenAI
                try
                {
                    string pattern = @"^\(([^)]+)\)\s*(.*)$";
                    string slicedMessage;
                    string choosedCharacter;
                    Match match = Regex.Match(prompt, pattern);
                    if (match.Success)
                    {
                        choosedCharacter = match.Groups[1].Value; // Ce qui est entre parenthèses
                        slicedMessage = match.Groups[2].Value;      // Le reste du texte
                    }
                    else
                    {
                        slicedMessage = prompt;
                        Random rnd = new Random();
                        int charIndex = rnd.Next(characters.Length);
                        choosedCharacter = characters[charIndex];
                    }

                    FileHelper fileHelper = new FileHelper();
                    var aiResponse = await _openAIService.GenerateResponseAsync(slicedMessage, username, choosedCharacter);

                    fileHelper.WriteToFile(choosedCharacter, aiResponse);

                    _obsSockets.ShowElementBanjo();
                    // Envoie la réponse dans le chat
                    //_client.SendMessage(channelName, $"@{username}, {aiResponse}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erreur lors de l'appel à OpenAI : {ex.Message}");
                    _twitchClient.SendMessage(channelName, $"@{username}, une erreur est survenue lors de la génération de la réponse.");
                }
            }
        }

        public async Task ThanksFollow(string username)
        {
            FileHelper fileHelper = new FileHelper();

            Random rnd = new Random();
            int charIndex = rnd.Next(characters.Length);

            var aiResponse = await _openAIService.GenerateThanksAsync(username, characters[charIndex]);

            fileHelper.WriteToFile(characters[charIndex], aiResponse);

            _obsSockets.ShowElementBanjo();
            return;
        }

        public async Task RandomTTS(string username, string message)
        {
            FileHelper fileHelper = new FileHelper();

            Random rnd = new Random();
            int charIndex = rnd.Next(characters.Length);

            var aiResponse = await _openAIService.GenerateRandomResponseAsync(username, message, characters[charIndex]);

            fileHelper.WriteToFile(characters[charIndex], aiResponse);

            _obsSockets.ShowElementBanjo();
            return;
        }
    }
}
