using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Communication.Interfaces;

namespace ParanTwitchIntegration.Application.Helpers
{
    public class OBSSockets
    {
        private OBSWebsocket client;
        public OBSSockets() {
            connectToOBS();
        }

        private void connectToOBS()
        {
            client = new OBSWebsocket();

            client.CurrentProgramSceneChanged += onSceneChanged;
            client.ConnectAsync("ws://localhost:4455", "j9Wcfbdkh480fXr8");
            Console.WriteLine("Connexion établie avec OBS.");
        }


        private void onSceneChanged(object sender, ProgramSceneChangedEventArgs e)
        {
            Console.WriteLine("Le scène a changé lol");
        }

        public void ShowElementBanjo()
        {
            client.SetSceneItemEnabled("Scène jeu", 10, true);
            client.SetSceneItemEnabled("Waiting", 25, true);
        }

        public void HideElementBanjo()
        {
            client.SetSceneItemEnabled("Scène jeu", 10, false);
            client.SetSceneItemEnabled("Waiting", 25, false);
        }
    }
}
