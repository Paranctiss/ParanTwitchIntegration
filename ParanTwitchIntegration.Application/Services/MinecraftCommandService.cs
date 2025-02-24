using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System;
using System.Text;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using ParanTwitchIntegration.Domain.Models;

namespace ParanTwitchIntegration.Application.Services
{
    public class MinecraftCommandService
    {
        /*private readonly string _hostname = "57.129.71.128"; // L'IP ou le nom de domaine de ton VPS
        private readonly string _username = "minecraft"; // Ton utilisateur SSH
        private readonly string _password = "taruminecraft"; // Ton mot de passe SSH */
        private readonly int _port = 22; // Le port SSH par défaut est 22

        private readonly EnvSettings _settings;

        public MinecraftCommandService(IOptions<EnvSettings> options)
        {
            _settings = options.Value;
        }

        public void ExecuteCommand(string command)
        {
            if (_settings.Environment == "local")
            {
                ExecuteLocalCommand(command);
            }
            else if (_settings.Environment == "remote")
            {
                ExecuteRemoteCommand(command);
            }
        }

        private void ExecuteRemoteCommand(string command)
        {
            try
            {
                using (var sshClient = new SshClient(_settings.Remote.Hostname, _settings.Remote.Port, _settings.Remote.Username, _settings.Remote.Password))
                {
                    sshClient.Connect();
                    if (sshClient.IsConnected)
                    {
                        Debug.WriteLine("Connexion SSH réussie !");

                        // Exécuter la commande Minecraft (comme give)
                        var minecraftCommand = $"screen -S minecraft -p 0 -X stuff \"{command}\r\"";
                        var result = sshClient.RunCommand(minecraftCommand);

                        Debug.WriteLine($"Commande exécutée : {result.Result}");
                    }
                    else
                    {
                        Debug.WriteLine("Erreur de connexion SSH.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur d'exécution de la commande : {ex.Message}");
            }
        }

        //A Tester plus tard
        private void ExecuteLocalCommand(string command)
        {
            try
            {
                // Chemin du fichier ou commande à exécuter
                string localCommand = _settings.LocalCommand;

                // Configuration du processus
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash", // Utilisation de bash sous Linux/MacOS
                    Arguments = $"-c \"{localCommand} {command}\"", // Commande complète
                    RedirectStandardOutput = true, // Rediriger la sortie standard
                    RedirectStandardError = true, // Rediriger les erreurs
                    UseShellExecute = false, // Ne pas utiliser de shell externe
                    CreateNoWindow = true // Ne pas créer de fenêtre de terminal
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    // Démarrer le processus
                    process.Start();

                    // Lire les sorties
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    // Log des résultats
                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        Debug.WriteLine($"Résultat de la commande : {output}");
                    }

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Debug.WriteLine($"Erreur de la commande : {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur lors de l'exécution locale : {ex.Message}");
            }
        }
    }
}
