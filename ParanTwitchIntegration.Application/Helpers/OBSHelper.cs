using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanTwitchIntegration.Application.Helpers
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

  /*  public class OBSHelper
    {
        private ClientWebSocket ws;
        private Uri obsUri;

        public OBSHelper(string obsUrl)
        {
            ws = new ClientWebSocket();
            obsUri = new Uri(obsUrl);
            ConnectAsync().Wait();
        }
        public async Task ConnectAsync()
        {
            try
            {
                await ws.ConnectAsync(obsUri, CancellationToken.None);
                Console.WriteLine("Connexion établie avec OBS.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la connexion : {ex.Message}");
            }
        }

        public async Task ShowSourceInSceneAsync(string sceneName, string sourceName)
        {
            /*var command = new
            {
                op = 6,  // Requête à OBS
                d = new
                {
                    requestType = "SetSceneItemProperties",  // Commande pour manipuler une source dans une scène
                    requestId = Guid.NewGuid().ToString(),  // ID unique pour la requête
                    requestData = new
                    {
                        sceneName = sceneName,  // Nom de la scène
                        item = sourceName,  // Nom de la source
                        visible = false  // Cacher la source
                    }
                }
            }; */
                /*
            var command = new
            {
                requestType = "SetSceneItemEnabled",
                requestData = new
                {
                    sceneName = "Scène jeu",
                    sceneItemId = 10,
                    sceneItemEnabled = false
                }
            };


            var jsonCommand = JsonConvert.SerializeObject(command);
            var commandBytes = Encoding.UTF8.GetBytes(jsonCommand);

            await SendMessageAsync(commandBytes);
            await ListenForMessagesAsync();
        }

        public async Task HideSourceInSceneAsync(string sceneName, string sourceName)
        {
            var command = new
            {
              requestType= "SetSceneItemEnabled",
              requestData= new {
                            sceneName= "Scène jeu",
                sceneItemId= 10,
                sceneItemEnabled= false
              }
            };


            var jsonCommand = JsonConvert.SerializeObject(command);
            var commandBytes = Encoding.UTF8.GetBytes(jsonCommand);

            await SendMessageAsync(commandBytes);
        }

        private async Task SendMessageAsync(byte[] message)
        {
            try
            {
                await ws.SendAsync(new ArraySegment<byte>(message), WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine("Message envoyé à OBS.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi du message : {ex.Message}");
            }
        }

        public async Task DisconnectAsync()
        {
            if (ws.State == WebSocketState.Open)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Fermeture demandée", CancellationToken.None);
                Console.WriteLine("Connexion fermée.");
            }
        }

        // Méthode pour écouter les messages d'OBS
        public async Task ListenForMessagesAsync()
        {
            var buffer = new byte[1024];
            while (ws.State == WebSocketState.Open)
            {
                try
                {
                    var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine($"Message reçu : {message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la réception du message : {ex.Message}");
                }
            }
        }
    } */

}
