using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using ParanTwitchIntegration.Domain.Events;
using ParanTwitchIntegration.Domain.Interfaces;
using ParanTwitchIntegration.Domain.Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Application.Services
{
    public class MinecraftIntegrationService : IMinecraftIntegrationService
    {
        private readonly IOptions<MinecraftSettings> _options;
        private SshClient _sshClient;
        public MinecraftIntegrationService(IOptions<MinecraftSettings> options, EventBus eventBus)
        {
            _options = options;
            eventBus.Subscribe<TwitchRewardRedeemEvent>(OnRedeemRewardReceived);
        }


        private void OnRedeemRewardReceived(TwitchRewardRedeemEvent e)
        {
            if (e.RewardTitle.StartsWith("MC"))
            {
                ExecuteCommandForReward(e);
                //ExecuteCommand(command);
            }
        }


        private async void ExecuteCommandForReward(TwitchRewardRedeemEvent reward)
        {
            string rewardAction = reward.RewardTitle.Substring(5);
            string channelName = FindPlayerByChannelID(reward.ChannelID);

            string command = "";
            switch (rewardAction)
            {
                case "test":
                    command = $"give {channelName} gold_sword 1";
                    break;
                case "give épée en pierre":
                    command = $"give {channelName} stone_sword 1";
                    break;
                case "give pioche en fer":
                    command = $"give {channelName} iron_pickaxe 1";
                    break;
                case "Give arc":
                    command = $"give {channelName} bow 1";
                    ExecuteCommand($"say {reward.UserName} te donne un arc");
                    break;
                case "Give 10 flèches":
                    command = $"give {channelName} arrow 10";
                    ExecuteCommand($"say {reward.UserName} 10 flèches");
                    break;
                case "Donner 5 pains":
                    command = $"give {channelName} bread 5";
                    ExecuteCommand($"say {reward.UserName} te donne 5 pains");
                    break;
                case "Spawn Arraignée":
                    command = $"execute at {channelName} run summon spider ~3 ~ ~3";
                    ExecuteCommand($"say {reward.UserName} a spawn une arraignée");
                    break;
                case "Spawn Creeper":
                    command = $"execute at {channelName} run summon creeper ~3 ~ ~3";
                    ExecuteCommand($"say {reward.UserName} a spawn un creeper j'espère t'es mort");
                    break;
                case "Temps orageux":
                    command = "weather world storm";
                    ExecuteCommand($"say {reward.UserName} fait pleuvoir");
                    break;
                case "Temps clair":
                    command = "weather world sun";
                    ExecuteCommand($"say {reward.UserName} rend le temps clair");
                    break;
                case "Effet nausée 1min":
                    command = $"effect give {channelName} minecraft:nausea 60 1 true";
                    ExecuteCommand($"say {reward.UserName} t'inflige une nausée");
                    break;
                case "Annonce":
                    ExecuteCommand(SerializeJsonAnnonce(reward.UserName, "ANNONCE : ", channelName));
                    command = SerializeJsonAnnonce(reward.UserName, reward.UserInput, channelName, "subtitle", "yellow");
                    break;
                case "Message":
                    command = $"say [{reward.UserName}] : {reward.UserInput}";
                    break;
                case "Vision nocturne 5min":
                    command = $"effect give {channelName} night_vision 300 1";
                    ExecuteCommand($"say {reward.UserName} te donne la vision nocturne");
                    break;
                case "Son de creeper":
                    command = $"playsound minecraft:entity.creeper.primed master @a ~ ~ ~ 1 1 1";
                    break;
                case "Son qui fait peur":
                    command = SelectRandomScarySound();
                    break;
                case "Disque 11":
                    command = "playsound minecraft:music_disc.11 master @a ~ ~ ~ 1 1 1";
                    break;
                case "Coffre de butin":
                    command = "execute at "+channelName+" run setblock ~ ~ ~ chest{LootTable:\"minecraft:chests/simple_dungeon\"}";
                    ExecuteCommand($"say {reward.UserName} a fait spawn un coffre de butin");
                    break;
                case "Envoyer en l'air":
                    ExecuteCommand($"effect give {channelName} minecraft:slow_falling 30 1 true");
                    command = $"effect give {channelName} minecraft:levitation 3 50 true";
                    ExecuteCommand($"say {reward.UserName} te fait pousser des ailes");
                    break;
                case "SURPRISE 2":
                    ExecuteSurprise1Command();
                    break;
                case "SURPRISE 1":
                    ExecuteCommand("effect give @a minecraft:speed 180 50 true");
                    ExecuteCommand("effect give @a minecraft:jump_boost 180 15 true");
                    ExecuteCommand($"effect give @a minecraft:slow_falling 30 1 true");
                    break;
                case "SURPRISE 3":
                    break;
                case "Combat à mort":
                    ExecuteCommand(await FightToDeathCommand());
                    break;
            }

            ExecuteCommand(command);
        }

        private string FindPlayerByChannelID(string channelID)
        {
            switch (channelID) {
                case "243236704"://Kokonath_
                    return "Paranctiss";
                    break;
                case "126793096":
                    return "Luucy_";
                    break;
                case "699038600":
                    return "saauui";
                    break;
                case "240478156":
                    return "Taruuki";
                    break;
                default:
                    return "Paranctiss";
                    break;
            }
        }

        private string SelectRandomScarySound()
        {
            string[] scarySounds = {
                "entity.ender_dragon.growl",
                "entity.warden.roar",
                "entity.wither.spawn",
                "entity.wither.spawn",
                "entity.warden.dig",
                "entity.warden.emerge",
                "entity.warden.sonic_boom",
                "entity.ender_dragon.ambient",
                "entity.ender_dragon.death",
                "entity.enderman.death",
                "entity.generic.explode",
                "block.portal.ambient"
            };

            Random rand = new Random();
            string selectedSound = scarySounds[rand.Next(scarySounds.Length)];
            return $"playsound minecraft:{selectedSound} master @a ~ ~ ~ 1 1 1";
        }

        private string ExecuteSurprise1Command()
        {
            for(int i=0; i<20; i++)
            {
                Random rand = new Random();
                int x = rand.Next(3);
                int z = rand.Next(3);
                int n = rand.Next(0, 2);
                string v = n == 0 ? "" : "-";
                ExecuteCommand("execute at @a run summon creeper ~" + v +x + " ~" + 2 + " ~" + v + z + " {ExplosionRadius:0, Fuse:50}");
            }
            return "execute at @a run summon creeper ~ ~ ~ {ExplosionRadius:0,CustomName:\"\\\"SURPRISE\\\"\"}";
        }

        private async Task<string> FightToDeathCommand()
        {
            ExecuteCommand("title @a times 10 150 10");
            ExecuteCommand(SerializeJsonFight("COMBAT A MORT !", "title"));
            await Task.Delay(1000);
            ExecuteCommand(SerializeJsonFight("Début dans 5", "subtitle", "yellow"));
            await Task.Delay(1000);
            ExecuteCommand(SerializeJsonFight("Début dans 4", "subtitle", "blue"));
            await Task.Delay(1000);
            ExecuteCommand(SerializeJsonFight("Début dans 3", "subtitle", "gold"));
            await Task.Delay(1000);
            ExecuteCommand(SerializeJsonFight("Début dans 2", "subtitle", "light_purple"));
            await Task.Delay(1000);
            ExecuteCommand(SerializeJsonFight("Début dans 1", "subtitle", "aqua"));
            await Task.Delay(1000);
            ExecuteCommand(SerializeJsonFight("Début dans 0", "subtitle", "red"));
            ExecuteCommand("title @a times 10 70 10");
            return SerializeJsonFight("LET'S GOO", "title");
        }


        public void ExecuteCommand(string command)
        {
            try
            {
                InitializeSshClient();

                if (_sshClient.IsConnected)
                {
                    // Vérification de la session screen
                    var checkScreen = _sshClient.RunCommand("screen -ls | grep minecraft");
                    if (string.IsNullOrEmpty(checkScreen.Result))
                    {
                        Debug.WriteLine("Démarrage du serveur Minecraft...");
                        _sshClient.RunCommand("screen -dmS minecraft java -Xmx2G -Xms1G -jar minecraft_server.jar nogui");
                        Task.Delay(5000).Wait(); // Attente non-bloquante
                    }

                    // Exécution de la commande
                    var escapedCommand = command.Replace("\"", "\\\"");
                    var minecraftCommand = $"screen -S minecraft -p 0 -X stuff \"{escapedCommand}\r\"";
                    _sshClient.RunCommand(minecraftCommand);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur lors de l'exécution: {ex.Message}");
                // Réinitialisation en cas d'erreur
                if (_sshClient != null)
                {
                    _sshClient.Disconnect();
                    _sshClient = null;
                }
            }
        }

        public void InitializeSshClient()
        {
            if (_sshClient == null || !_sshClient.IsConnected)
            {
                _sshClient = new SshClient(
                    _options.Value.Hostname,
                    _options.Value.Port,
                    _options.Value.Username,
                    _options.Value.Password
                );

                try
                {
                    _sshClient.Connect();
                    Debug.WriteLine("Connexion SSH établie avec succès");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erreur de connexion SSH: {ex.Message}");
                }
            }
        }

        private string SerializeJsonAnnonce(string username, string input, string channel, string title = "title", string color="red")
        {
            string jsonMessage = JsonConvert.SerializeObject(new
            {
                text = $"{input}",
                bold = true,
                color = color,
            });

            return $"title @a {title} {jsonMessage}";
        }

        private string SerializeJsonFight(string text, string title, string color = "red")
        {
            string jsonMessage = JsonConvert.SerializeObject(new
            {
                text = $"{text}",
                bold = true,
                color = color
            });

            return $"title @a {title} {jsonMessage}";
        }
    }
}
